using System;
using System.Collections.Generic;
using System.Web.Mvc;
using createsend_dotnet;
using USNStarterKit.USNModels;
using Umbraco.Web;
using System.Web.UI.WebControls;
using MailChimp;
using MailChimp.Helper;
using MailChimp.Lists;
using USNStarterKit.USNHelpers;
using System.Web;

namespace USNStarterKit.USNControllers
{
    /// <summary>
    /// Not using strongly typed models here so that PureLive mode can be used
    /// </summary>
    public class USNContactFormSurfaceController : Umbraco.Web.Mvc.SurfaceController
    {

        public ActionResult Index(int NodeID, int GlobalSettingsID)
        {
            var model = new USNContactFormViewModel();
            model.CurrentNodeID = NodeID;
            model.GlobalSettingsID = GlobalSettingsID;

            return PartialView("USNForms/USN_ContactForm", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HandleContactSubmit(USNContactFormViewModel model)
        {
            System.Threading.Thread.Sleep(1000);

            string returnValue = String.Empty;

            if (!ModelState.IsValid)
            {
                return JavaScript(String.Format("$(ContactError{0}).show();$(ContactError{0}).html('{1}');", model.CurrentNodeID, HttpUtility.JavaScriptStringEncode(umbraco.library.GetDictionaryItem("USN Contact Form General Error"))));
            }

            //Need to get NodeID from hidden field. CurrentPage does not work with Ajax.BeginForm
            var contactFormNode = Umbraco.TypedContent(model.CurrentNodeID);
            var globalSettings = Umbraco.TypedContent(model.GlobalSettingsID);


            string mailTo = contactFormNode.GetPropertyValue<string>("recipientEmailAddress");
            string websiteName = globalSettings.GetPropertyValue<string>("websiteName");
            string pageName = contactFormNode.Parent.Parent.Name;

            string errorMessage = String.Empty;

            if (!SendContactFormMail(model, mailTo, websiteName, pageName, out errorMessage))
            {
                return JavaScript(String.Format("$(ContactError{0}).show();$(ContactError{0}).html('<div class=\"info\"><p>{1}</p><p>{2}</p></div>');", model.CurrentNodeID, HttpUtility.JavaScriptStringEncode(umbraco.library.GetDictionaryItem("USN Contact Form Mail Send Error")), HttpUtility.JavaScriptStringEncode(errorMessage)));
            }

            try
            {
                if (model.NewsletterSignup && globalSettings.HasValue("newsletterAPIKey") &&
                    (globalSettings.HasValue("defaultNewsletterSubscriberListID") || contactFormNode.HasValue("newsletterSubscriberListID")))
                {
                    if (globalSettings.GetPropertyValue<string>("emailMarketingPlatform") == "Campaign Monitor")
                    {
                        AuthenticationDetails auth = new ApiKeyAuthenticationDetails(globalSettings.GetPropertyValue<string>("newsletterAPIKey"));

                        string subsciberListID = String.Empty;

                        if (contactFormNode.HasValue("newsletterSubscriberListID"))
                            subsciberListID = contactFormNode.GetPropertyValue<string>("newsletterSubscriberListID");
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

                        if (contactFormNode.HasValue("newsletterSubscriberListID"))
                            subsciberListID = contactFormNode.GetPropertyValue<string>("newsletterSubscriberListID");
                        else
                            subsciberListID = globalSettings.GetPropertyValue<string>("defaultNewsletterSubscriberListID");

                        var email = new EmailParameter()
                        {
                            Email = model.Email
                        };

                        var myMergeVars = new MergeVar();
                        myMergeVars.Add("FNAME", model.FirstName);
                        myMergeVars.Add("LNAME", model.LastName);

                        EmailParameter results = mc.Subscribe(subsciberListID, email, myMergeVars, "html", false, true, false, false);
                    }
                }
            }
            catch (Exception ex)
            {
                return JavaScript(String.Format("$(ContactError{0}).show();$(ContactError{0}).html('<div class=\"info\"><p>{1}</p><p>{2}</p></div>');", model.CurrentNodeID, HttpUtility.JavaScriptStringEncode(umbraco.library.GetDictionaryItem("USN Contact Form Signup Error")), HttpUtility.JavaScriptStringEncode(ex.Message)));
            }

            returnValue = String.Format("<div class=\"spc alert alert-success alert-dismissible fade in\" role=\"alert\"><div class=\"info\">{0}</div></div>", contactFormNode.GetPropertyValue<string>("submissionMessage"));

            return Content(returnValue);
        }

        public static bool SendContactFormMail(USNContactFormViewModel model, string mailTo, string websiteName, string pageName, out string lsErrorMessage)
        {
            lsErrorMessage = String.Empty;

            try
            {
                //Create MailDefinition 
                MailDefinition md = new MailDefinition();
                string lsSendTo = String.Empty;

                //specify the location of template 
                md.BodyFileName = "/usn/emailtemplates/contactform.htm";
                md.IsBodyHtml = true;

                //Build replacement collection to replace fields in template 
                System.Collections.Specialized.ListDictionary replacements = new System.Collections.Specialized.ListDictionary();
                replacements.Add("<% formFirstName %>", model.FirstName == null ? "" : model.FirstName);
                replacements.Add("<% formLastName %>", model.LastName == null ? "" : model.LastName);
                replacements.Add("<% formEmail %>", model.Email == null ? "" : model.Email);
                replacements.Add("<% formPhone %>", model.Telephone == null ? "" : model.Telephone);
                replacements.Add("<% formMessage %>", model.Message == null ? "" : umbraco.library.ReplaceLineBreaks(model.Message));
                replacements.Add("<% WebsitePage %>", pageName);
                replacements.Add("<% WebsiteName %>", websiteName);

                lsSendTo = mailTo;

                //now create mail message using the mail definition object 
                System.Net.Mail.MailMessage msg = md.CreateMailMessage(lsSendTo, replacements, new System.Web.UI.Control());
                msg.ReplyToList.Add(model.Email);
                msg.Subject = websiteName + " Website: " + pageName + " Page Enquiry";

                //this uses SmtpClient in 2.0 to send email, this can be configured in web.config file.
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                smtp.Send(msg);

                return true;
            }
            catch (Exception ex)
            {
                lsErrorMessage = ex.Message;
            }

            return false;
        }
    }
}