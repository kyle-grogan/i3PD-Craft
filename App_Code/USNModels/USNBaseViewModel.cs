using Umbraco.Core.Models;
using Umbraco.Web.Models;

namespace USNStarterKit.USNModels
{
    /// <summary>
    /// Not using strongly typed models here so that PureLive mode can be used
    /// </summary>
    public class USNBaseViewModel : RenderModel
    {
        //Standard Model Pass Through
        public USNBaseViewModel(IPublishedContent content) : base(content) { }

        public IPublishedContent GlobalSettings { get; set; }

        public IPublishedContent Navigation { get; set; }
    }
}