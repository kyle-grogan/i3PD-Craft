using System.Linq;
using System.Web.Mvc;
using Umbraco.Web.Models;
using USNStarterKit.USNModels;
using Umbraco.Web;
using Umbraco.Core.Models;

namespace USNStarterKit.USNControllers
{
    /// <summary>
    /// Not using strongly typed models here so that PureLive mode can be used
    /// </summary>
    public class USNBaseController : Umbraco.Web.Mvc.RenderMvcController
    {
        public override ActionResult Index(RenderModel model)
        {
            IPublishedContent homeNode = model.Content.Site();

            if (homeNode.IsDocumentType("USNHomepage"))
            {
                IPublishedContent globalSettings = homeNode.GetPropertyValue<IPublishedContent>("websiteConfigurationNode").Children.Where(x => x.IsDocumentType("USNGlobalSettings")).First();
                IPublishedContent navigation = homeNode.GetPropertyValue<IPublishedContent>("websiteConfigurationNode").Children.Where(x => x.IsDocumentType("USNNavigation")).First();

                USNBaseViewModel customModel = new USNBaseViewModel(model.Content);
                customModel.GlobalSettings = globalSettings;
                customModel.Navigation = navigation;

                return base.Index(customModel);
            }
            else
                return base.Index(model);
        }
    }
}