using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MovingScrewdriver.Web.Models
{
    public class ScrewdriverConfig
    {
        public string Id { get; set; }

        // blog header
        // H1
        public string Title { get; set; }
        // small in H1
        public string SubTitle { get; set; }
        // H2
        public string Tagline { get; set; }
        public string BlogFullTitle { get { return "{0} {1}".FormatWith(Title, SubTitle); } }

        // author/owner info
        public string OwnerEmail { get; set; }
        public string OwnerNick { get; set; }
        public string OwnerFirstName { get; set; }
        public string OwnerLastName { get; set; }
        public string OwnerFullName { get { return "{0} {1}".FormatWith(OwnerFirstName, OwnerLastName); } }

        // key settings
        public string GoogleAnalyticsKey { get; set; }
        public bool GoogleAnalyticsShouldRender { get { return GoogleAnalyticsKey.IsNotNullOrEmpty(); } }
        public string AkismetKey { get; set; }
        public string GoogleSiteVerificationKey { get; set; }

        // blog metadata
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaCopyright { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaApplicationName { get; set; }
        public string MetaLang { get; set; }
        
        // blog meta social
        public bool ShowSocialMeta { get; set; }
        public bool MetaTwitterShouldRender { get { return MetaTwitterName.IsNotNullOrEmpty(); } }
        public string MetaTwitterName { get; set; }
        public bool MetaFbAppShouldRender { get { return MetaFbAppId.IsNotNullOrEmpty(); } }
        public string MetaFbAppId { get; set; }
        
        // blog meta og
        public bool ShowOgMeta { get; set; }
        public string MetaOgLocale { get; set; }
        public string MetaOgImage { get; set; }

        // blog meta Dublin Core
        public bool ShowDcMeta { get; set; }

        // blog meta apple
        public bool ShowAppleMeta { get; set; }
        public string MetaAppleMobileWebAppTitle { get; set; }
        public string MetaAppleMobileWebAppCapable { get; set; }
        public string MetaAppleITunesApp { get; set; }
        public bool MetaAppleITunesAppShouldRender { get { return MetaAppleITunesApp.IsNotNullOrEmpty(); } }
        public string MetaAppleMobileWebAppStatusBarStyle { get; set; }

        // blog meta ms ie9
        public bool ShowWinIe9Meta { get; set; }
        public string MetaIe9Tooltip { get; set; }
        public string MetaIe9StartUrl { get; set; }
        public ICollection<string> MetaIe9Tasks { get; set; }

        // blog meta ms win8
        public bool ShowWin8Meta { get; set; }
        public string MetaWin8TileColor { get; set; }
        public string MetaWin8TileImage { get; set; }
        public bool MetaWin8BadgeShouldShow { get; set; }

        // blog meta me
        public bool ShowMeMeta { get; set; }
        public ICollection<string> MetaMeLinks { get; set; }

        // blog settings
        public int NumberOfDayToCloseComments { get; set; }
        public int MinNumberOfPostForSignificantTag { get; set; }

        public string FeedUrl { get; set; }
        public string FeedCommentsUrl { get; set; }

        public string FooterHtml { get; set; }

        public string MetaFbAppLikeBoxUrl { get; set; }
        public bool FbLikeBoxShouldRender { get { return MetaFbAppLikeBoxUrl.IsNotNullOrWhiteSpace(); } }

        public string MetaIe9NavBarColor { get; set; }

        public static ScrewdriverConfig Create()
        {
            return new ScrewdriverConfig
            {
                Id = "Blog/Config"
            };
        }

        public ScrewdriverConfig()
        {
            MetaMeLinks = new Collection<string>();
            MetaIe9Tasks = new Collection<string>();
        }
    }
}