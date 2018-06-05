using System.Web.Mvc;
using Umbraco.Core.Models;
using System.Web.Security;
using USNStarterKit.USNModels;
using Umbraco.Web;

namespace USNStarterKit.USNControllers
{
    /// <summary>
    /// Not using strongly typed models here so that PureLive mode can be used
    /// </summary>
    public class USNLoginFormSurfaceController : Umbraco.Web.Mvc.SurfaceController
    {

        public ActionResult Index()
        {
            var model = new USNLoginFormViewModel();

            if (CurrentPage.Url != Request.Url.PathAndQuery)
                model.ReturnUrl = Request.Url.PathAndQuery;
            else
                model.ReturnUrl = CurrentPage.GetPropertyValue<IPublishedContent>("loginSuccessPage").Url;

            model.LoginFormButtonText = CurrentPage.GetPropertyValue<string>("loginFormButtonText");

            return PartialView("USNForms/USN_LoginForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HandleLoginSubmit(USNLoginFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            // Login
            if (Membership.ValidateUser(model.Username, model.Password))
            {
                FormsAuthentication.SetAuthCookie(model.Username, false);

                return Redirect(model.ReturnUrl);
            }
            else
            {
                TempData.Add("LoginFailure", umbraco.library.GetDictionaryItem("USN Login Form Login Error"));
                return RedirectToCurrentUmbracoPage();
            }
        }
    }

}