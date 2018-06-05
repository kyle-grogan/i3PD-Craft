using System;
using System.Globalization;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Publishing;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using USNStarterKit.USNControllers;

namespace USNStarterKit.USNBusinessLogic
{
    public class USNRegisterEvents : ApplicationEventHandler
    {
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            DefaultRenderMvcControllerResolver.Current.SetDefaultControllerType(typeof(USNBaseController));
        }

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ContentService.Published += ContentService_Published;
            ContentService.Trashing += ContentService_Trashing;
            ContentService.Saving += ContentService_Saving;
            //ContentService.UnPublishing += Unpublishing;
        }

        //TODO: Pre 7.6 used Saved Event to run this code. ContentService.SaveAndPublishWithStatus not working in Saved Event for Umbraco 7.6
        //Results in Link to document 'This document is published but is not in the cache'.
        //Change back to Saved Event when bug fixed.
        private void ContentService_Published(IPublishingStrategy sender, PublishEventArgs<IContent> e)
        {
            foreach (var node in e.PublishedEntities)
            {
                if ((node.ContentType.Alias == "USNStandardPage" || node.ContentType.Alias == "USNStandardPageBlogPost") &&
                    node.Children().Where(x => x.ContentType.Alias == "USNStandardPageComponents").FirstOrDefault() == null)
                {
                    var contentService = ApplicationContext.Current.Services.ContentService;
                    var pageComponentsFolder = contentService.CreateContent("Page Components", node.Id, "USNStandardPageComponents");
                    pageComponentsFolder.SetValue("disableDelete", true);
                    contentService.SaveAndPublishWithStatus(pageComponentsFolder);
                }
                else if ((node.ContentType.Alias == "USNHomepage" || node.ContentType.Alias == "USNAdvancedPage" || node.ContentType.Alias == "USNAdvancedPageBlogPost") &&
                         node.Children().Where(x => x.ContentType.Alias == "USNAdvancedPageComponents").FirstOrDefault() == null)
                {
                    var contentService = ApplicationContext.Current.Services.ContentService;
                    var pageComponentsFolder = contentService.CreateContent("Page Components", node.Id, "USNAdvancedPageComponents");
                    pageComponentsFolder.SetValue("disableDelete", true);
                    contentService.SaveAndPublishWithStatus(pageComponentsFolder);
                }
            }
        }

        private void ContentService_Trashing(IContentService sender, MoveEventArgs<IContent> e)
        {
            foreach (var node in e.MoveInfoCollection)
            {
                if (node.Entity.HasProperty("disableDelete"))
                {
                    var contentService = ApplicationContext.Current.Services.ContentService;
                    IContent content = contentService.GetById(node.Entity.Id);
                    if (content.HasProperty("disableDelete") && content.GetValue<bool>("disableDelete"))
                    {
                        e.Cancel = true;
                        e.Messages.Add(new EventMessage("Deletion of this item has been blocked", "This item is important to the successfull operation of this website.<br>If you would still like to delete this item, please uncheck the 'Disable delete' field on the 'Properties' tab.", EventMessageType.Warning));
                    }
                }
            }
        }

        private void ContentService_Saving(IContentService sender, SaveEventArgs<IContent> e)
        {
            foreach (var node in e.SavedEntities)
            {
                //we are only interested in news items
                if (node.ContentType.Alias == "USNStandardPageBlogPost" || node.ContentType.Alias == "USNAdvancedPageBlogPost")
                {
                    var contentService = ApplicationContext.Current.Services.ContentService;
                    int parentID;
                    DateTime newsItemDate = DateTime.Parse(node.GetValue("postDate").ToString());

                    IContent parentNode = node.Parent();
                    IContent yearNode = null;
                    IContent monthNode = null;

                    //is it a new item or an existing item?
                    //do this with the node id
                    if (node.Id == 0)
                    {
                        //get the correct parent for the blog post
                        parentID = GetParentNodeForNewsItem(contentService, parentNode, newsItemDate);
                        node.ParentId = parentID;
                    }
                    else
                    {
                        monthNode = parentNode;
                        yearNode = parentNode.Parent();

                        if (monthNode.GetValue<int>("month") != newsItemDate.Month || yearNode.GetValue<int>("year") != newsItemDate.Year)
                        {
                            IContent postNode = yearNode.Parent();
                            parentID = GetParentNodeForNewsItem(contentService, postNode, newsItemDate);

                            contentService.Move(node, parentID);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get the correct parent node for a news item
        /// It will create if it needs to
        /// </summary>
        private int GetParentNodeForNewsItem(IContentService contentService, IContent blogPostsFolder, DateTime date)
        {
            int parentID;

            //Create year folder if not present
            if (blogPostsFolder.Children().Where(x => x.GetValue<int>("year") == date.Year).Count() != 1)
            {
                var yearFolder = contentService.CreateContent(date.Year.ToString(), blogPostsFolder.Id, "USNBlogYearFolder");
                yearFolder.SetValue("year", date.Year.ToString());
                contentService.SaveAndPublishWithStatus(yearFolder);

                //Create month folder if not present
                if (yearFolder.Children().Where(x => x.GetValue<int>("month") == date.Month).Count() != 1)
                {
                    var monthFolder = contentService.CreateContent(date.ToString("MMMM", CultureInfo.CreateSpecificCulture("en")), yearFolder.Id, "USNBlogMonthFolder");
                    monthFolder.SetValue("month", date.Month.ToString());
                    contentService.SaveAndPublishWithStatus(monthFolder);
                    parentID = monthFolder.Id;
                }
                else
                {
                    var monthFolder = yearFolder.Children().Where(x => x.GetValue<int>("month") == date.Month).First();
                    parentID = monthFolder.Id;
                }
            }
            else
            {
                var yearFolder = blogPostsFolder.Children().Where(x => x.GetValue<int>("year") == date.Year).First();

                if (yearFolder.Children().Where(x => x.GetValue<int>("month") == date.Month).Count() != 1)
                {
                    var monthFolder = contentService.CreateContent(date.ToString("MMMM", CultureInfo.CreateSpecificCulture("en")), yearFolder.Id, "USNBlogMonthFolder");
                    monthFolder.SetValue("disableDelete", 1);
                    monthFolder.SetValue("month", date.Month.ToString());
                    contentService.SaveAndPublishWithStatus(monthFolder);
                    parentID = monthFolder.Id;
                }
                else
                {
                    var monthFolder = yearFolder.Children().Where(x => x.GetValue<int>("month") == date.Month).First();
                    parentID = monthFolder.Id;
                }
            }

            return parentID;
        }

        //Notification messages not displaying for unpublished pages. Add back in when Umbraco bug fixed
        //http://issues.umbraco.org/issue/U4-7757
        //private void Unpublishing(IPublishingStrategy sender, PublishEventArgs<IContent> e)
        //{
        //    foreach (var node in e.PublishedEntities)
        //    {
        //        if (node.HasProperty("disableDelete") && node.GetValue<bool>("disableDelete"))
        //        {
        //            e.Cancel = true;
        //            e.Messages.Add(new EventMessage("Unpublishing of this item has been blocked", "This item is important to the successfull operation of this website.<br>If you would still like to unpublish this item, please uncheck the 'Disable delete' field on the 'Properties' tab.", EventMessageType.Warning));
        //        }
        //    }
        //}
    }

}