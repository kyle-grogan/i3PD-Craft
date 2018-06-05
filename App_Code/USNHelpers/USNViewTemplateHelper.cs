using System;
using Umbraco.Core.Models;
using USNStarterKit.USNEntities;
using USNStarterKit.USNBackgroundOptions.Models;

namespace USNStarterKit.USNHelpers
{
    public static class USNViewTemplateHelper
    {
        public static USNTemplateStyleSettings GetTemplateStyleSettings(string backgroundColor = null, string buttonColour = null)
        {
            USNTemplateStyleSettings settings = new USNTemplateStyleSettings();

            if (!String.IsNullOrEmpty(backgroundColor))
            {
                switch (backgroundColor)
                {
                    case "bc9366":
                        settings.BackGroundStyle = "c1-bg";
                        settings.HeadingStyle = "c5-text";
                        settings.SecondaryHeadingStyle = "c5-text";
                        settings.TextStyle = "c5-text";
                        break;
                    case "6f0108":
                        settings.BackGroundStyle = "c2-bg";
                        settings.HeadingStyle = "c5-text";
                        settings.SecondaryHeadingStyle = "c1-text";
                        settings.TextStyle = "c5-text";
                        break;
                    case "000000":
                        settings.BackGroundStyle = "c3-bg";
                        settings.HeadingStyle = "c5-text";
                        settings.SecondaryHeadingStyle = "c1-text";
                        settings.TextStyle = "c4-text";
                        break;
                    case "f6f2ed":
                        settings.BackGroundStyle = "c4-bg";
                        settings.HeadingStyle = "c3-text";
                        settings.SecondaryHeadingStyle = "c1-text";
                        settings.TextStyle = "c6-text text-perc_80";
                        break;
                    case "ffffff":
                        settings.BackGroundStyle = "c5-bg";
                        settings.HeadingStyle = "c3-text";
                        settings.SecondaryHeadingStyle = "c1-text";
                        settings.TextStyle = "c6-text text-perc_80";
                        break;
                    case "1e1819":
                        settings.BackGroundStyle = "c6-bg";
                        settings.HeadingStyle = "c5-text";
                        settings.SecondaryHeadingStyle = "c1-text";
                        settings.TextStyle = "c4-text";
                        break;
                    default:
                        settings.BackGroundStyle = "c6-bg";
                        settings.HeadingStyle = "c5-text";
                        settings.SecondaryHeadingStyle = "c1-text";
                        settings.TextStyle = "c4-text";
                        break;
                }
            }
            else
            {
                settings.BackGroundStyle = "c6-bg";
                settings.HeadingStyle = "c5-text";
                settings.SecondaryHeadingStyle = "c1-text";
                settings.TextStyle = "c4-text";
            }

            if (!String.IsNullOrEmpty(buttonColour))
            {
                switch (buttonColour)
                {
                    case "bc9366":
                        settings.ButtonStyle = "c1-bg c5-text";
                        break;
                    case "6f0108":
                        settings.ButtonStyle = "c2-bg c5-text";
                        break;
                    case "000000":
                        settings.ButtonStyle = "c3-bg c5-text";
                        break;
                    case "f6f2ed":
                        settings.ButtonStyle = "c4-bg c2-text";
                        break;
                    case "ffffff":
                        settings.ButtonStyle = "c5-bg c2-text";
                        break;
                    case "1e1819":
                        settings.ButtonStyle = "c6-bg c5-text";
                        break;
                    default:
                        settings.ButtonStyle = "c1-bg c5-text";
                        break;
                }
            }
            else
            {
                settings.ButtonStyle = "c1-bg c5-text";
            }

            return settings;
        }

        public static string GetBackgroundImageStyle(IPublishedContent image = null, USNBackgroundOption backgroundImageOptions = null)
        {
            string output = String.Empty;
            string backgroundImage = String.Empty;
            string backgroundStyle = String.Empty;
            string backgroundPosition = String.Empty;

            if (image != null)
            {
                backgroundImage = String.Format("background-image:url('{0}');", image.Url);

                if (backgroundImageOptions != null)
                {
                    switch (backgroundImageOptions.Style)
                    {
                        case "Cover":
                            backgroundStyle = "background-repeat:no-repeat;background-size:cover;";
                            break;
                        case "Full width":
                            backgroundStyle = "background-repeat:no-repeat;background-size:100% auto;";
                            break;
                        case "Original":
                            backgroundStyle = "background-repeat:no-repeat;background-size:auto;";
                            break;
                        case "Repeat":
                            backgroundStyle = "background-repeat:repeat;background-size:auto;";
                            break;
                        case "Repeat X":
                            backgroundStyle = "background-repeat:repeat-x;background-size:auto;";
                            break;
                        case "Repeat Y":
                            backgroundStyle = "background-repeat:repeat-y;background-size:auto;";
                            break;
                        default:
                            backgroundStyle = "background-repeat:no-repeat;background-size:auto;";
                            break;
                    }

                    switch (backgroundImageOptions.Position)
                    {
                        case "Center / Top":
                            backgroundPosition = "background-position:center top;";
                            break;
                        case "Center / Center":
                            backgroundPosition = "background-position:center center;";
                            break;
                        case "Center / Bottom":
                            backgroundPosition = "background-position:center bottom;";
                            break;
                        case "Right / Top":
                            backgroundPosition = "background-position:right top;";
                            break;
                        case "Right / Center":
                            backgroundPosition = "background-position:right center;";
                            break;
                        case "Right / Bottom":
                            backgroundPosition = "background-position:right bottom;";
                            break;
                        case "Left / Top":
                            backgroundPosition = "background-position:left top;";
                            break;
                        case "Left / Center":
                            backgroundPosition = "background-position:left center;";
                            break;
                        case "Left / Bottom":
                            backgroundPosition = "background-position:left bottom;";
                            break;
                        default:
                            backgroundPosition = "background-position:center center;";
                            break;

                    }

                }

                output = backgroundImage + backgroundStyle + backgroundPosition;

                if (output != String.Empty)
                {
                    output = String.Format(" style=\"{0}\"", output);
                }
            }

            return output;
        }

    }
}