using System;
using System.Collections.Generic;
using System.Web.Mvc;
using createsend_dotnet;
using USNStarterKit.USNModels;
using Umbraco.Web;
using MailChimp;
using MailChimp.Helper;
using MailChimp.Lists;
using System.Web;

namespace USNStarterKit.USNControllers
{
    /// <summary>
    /// Not using strongly typed models here so that PureLive mode can be used
    /// </summary>
    public class USNNewsletterSignupSurfaceController : Umbraco.Web.Mvc.SurfaceController
    {
        public ActionResult Index(int NodeID, int GlobalSettingsID)
        {
            var model = new USNNewsletterFormViewModel();
            model.CurrentNodeID = NodeID;
            model.GlobalSettingsID = GlobalSettingsID;

            return PartialView("USNForms/USN_NewsletterSignup", model);
        }

        public ActionResult TextSignup(int NodeID, int GlobalSettingsID)
        {
            var model = new USNNewsletterFormViewModel();
            model.CurrentNodeID = NodeID;
            model.GlobalSettingsID = GlobalSettingsID;

            return PartialView("USNAdvancedPageComponents/USN_AC_TextSignupSection", model);
        }

        public ActionResult SubpageSignup(int NodeID, int GlobalSettingsID)
        {
            var model = new USNNewsletterFormViewModel();
            model.CurrentNodeID = NodeID;
            model.GlobalSettingsID = GlobalSettingsID;

            return PartialView("USNAdvancedPageComponents/USN_AC_SignupSubpageListingSection", model);
        }

        [HttpPost]
        public ActionResult HandleNewsletterSubmit(USNNewsletterFormViewModel model)
        {
            System.Threading.Thread.Sleep(1000);

            string lsReturnValue = String.Empty;

            var currentNode = Umbraco.TypedContent(model.CurrentNodeID);
            var globalSettings = Umbraco.TypedContent(model.GlobalSettingsID);

            if (!ModelState.IsValid)
            {
                return JavaScript(String.Format("$(NewsletterError{0}).show();$(NewsletterError{0}).html('<div class=\"info\"><p>{1}</p></div>');", model.CurrentNodeID, HttpUtility.JavaScriptStringEncode(umbraco.library.GetDictionaryItem("USN Newsletter Form General Error"))));
            }
            try
            {
                if (globalSettings.GetPropertyValue<string>("emailMarketingPlatform") == "Campaign Monitor")
                {
                    AuthenticationDetails auth = new ApiKeyAuthenticationDetails(globalSettings.GetPropertyValue<string>("newsletterAPIKey"));

                    string subsciberListID = String.Empty;

                    if (currentNode.GetPropertyValue<string>("newsletterSubscriberListID") != String.Empty)
                        subsciberListID = currentNode.GetPropertyValue<string>("newsletterSubscriberListID");
                    else
                        subsciberListID = globalSettings.GetPropertyValue<string>("defaultNewsletterSubscriberListID");

                    Subscriber loSubscriber = new Subscriber(auth, subsciberListID);

                    List<SubscriberCustomField> customFields = new List<SubscriberCustomField>();

                    string subscriberID = loSubscriber.Add(model.Email, model.FirstName + " " + model.LastName, customFields, false);
                }
                else if (globalSettings.GetPropertyValue<string>("emailMarketingPlatform") == "MailChimp")
                {

                    var mc = new MailChimpManager(globalSettings.GetPropertyValue<string>("newsletterAPIKey"));

                    string subsciberListID = String.Empty;

                    if (currentNode.HasValue("newsletterSubscriberListID"))
                        subsciberListID = currentNode.GetPropertyValue<string>("newsletterSubscriberListID");
                    else
                        subsciberListID = globalSettings.GetPropertyValue<string>("defaultNewsletterSubscriberListID");


                    var email = new EmailParameter()
                    {
                        Email = model.Email
                    };

                    var myMergeVars = new MergeVar();
                    myMergeVars.Add("FNAME", model.FirstName);
                    myMergeVars.Add("LNAME", model.LastName);

                    EmailParameter results = mc.Subscribe(subsciberListID,email, myMergeVars, "html", false, true, false, false);
                }

                lsReturnValue = String.Format("<div class=\"spc alert alert-success alert-dismissible fade in\" role=\"alert\"><div class=\"info\">{0}</div></div>", currentNode.GetPropertyValue<string>("submissionMessage"));
                return Content(lsReturnValue);
            }
            catch (Exception ex)
            {
                return JavaScript(String.Format("$(NewsletterError{0}).show();$(NewsletterError{0}).html('<div class=\"info\"><p>{1}</p><p>{2}</p></div>');", model.CurrentNodeID, HttpUtility.JavaScriptStringEncode(umbraco.library.GetDictionaryItem("USN Newsletter Form Signup Error")), HttpUtility.JavaScriptStringEncode(ex.Message)));
            }
        }
    }
}