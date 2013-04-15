using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Routing;

namespace MovingScrewdriver.Web
{
    public class RouteConfig
    {
        private const string MatchPositiveInteger = @"\d{1,10}";

        public static void RegisterRoutes(RouteCollection routes)
        {
            new RouteConfig(routes).Configure();
        }

        private readonly RouteCollection _routes;
        public RouteConfig(RouteCollection routes)
        {
            _routes = routes;
        }

        public void Configure()
        {
            _routes.LowercaseUrls = true;

            ignore();
            
            errors();
            legacy();
            screw();
            post();
            archive();
            posts();
            syndication();
            services();
            about();
            aside();
            //default
            
            _routes.MapRoute(
                name: "default",
                url: "",
                defaults: new { controller = "Posts", action = "AllPosts" }
            );

            _routes.MapRoute(
                name: "404",
                url: "{*url}",
                defaults: new { controller = "Error", action = "Error404" }
            );

            testing_fallback();
        }

        private void ignore()
        {
            _routes.IgnoreRoute("{*allaxd}", new { allaxd = @".*\.axd(/.*)?" });
            _routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
        }

        [Conditional("DEBUG")]
        private void testing_fallback()
        {
            _routes.MapRoute(
                name: "testing-default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Ui", action = "Index", id = UrlParameter.Optional }
            );
        }

        private void services()
        {
            _routes.MapRoute(
                name: "rsd",
                url: "services/rsd",
                defaults: new { controller = "Services", action = "Rsd" }
            );

            _routes.MapRoute(
                name: "trackback",
                url: "trackback/{id}",
                defaults: new { controller = "Services", action = "Trackback" },
                constraints: new
                {
                    id = MatchPositiveInteger,
                    //httpMethod = new HttpMethodConstraint("POST", "PUT")
                }
            );

            _routes.MapRoute(
                name: "google-site-map",
                url: "sitemap.xml",
                defaults: new { controller = "Services", action = "GoogleSiteMap" }
            );
        }

        private void syndication()
        {
            _routes.MapRoute(
                name: "rss",
                url: "rss/posty",
                defaults: new { controller = "Syndication", action = "Rss" }
            );

            _routes.MapRoute(
                name: "rss-comments",
                url: "rss/komentarze",
                defaults: new { controller = "Syndication", action = "RssComments" }
            );

            _routes.MapRoute(
                name: "rss-tag",
                url: "rss/tag/{slug}",
                defaults: new { controller = "Syndication", action = "RssByTag" }
            );

            _routes.MapRoute(
                name: "rss-category",
                url: "rss/kategoria/{slug}",
                defaults: new { controller = "Syndication", action = "RssByCategory" }
            );
        }

        private void legacy()
        {
            _routes.MapRoute(
                name: "legacy-by-year-month-day",
                url: "{year}/{month}/{day}/default.aspx",
                defaults: new { controller = "Legacy", action = "PostsByDate" },
                constraints: new
                {
                    year = MatchPositiveInteger,
                    month = MatchPositiveInteger,
                    day = MatchPositiveInteger
                }
            );

            _routes.MapRoute(
                name: "legacy-by-year-month",
                url: "{year}/{month}/default.aspx",
                defaults: new { controller = "Legacy", action = "PostsByDate" },
                constraints: new
                {
                    year = MatchPositiveInteger,
                    month = MatchPositiveInteger
                }
            );

            _routes.MapRoute(
                name: "legacy-by-year",
                url: "{year}/default.aspx",
                defaults: new { controller = "Legacy", action = "PostsByDate" },
                constraints: new
                {
                    year = MatchPositiveInteger
                }
            );

            _routes.MapRoute(
                name: "legacy-post-details-special-case-from-webmasters-tool",
                url: "post/{year}/{month}/{day}/{slug}.aspxSee",
                defaults: new { controller = "Legacy", action = "PostBySlug" },
                constraints: new
                {
                    year = MatchPositiveInteger,
                    month = MatchPositiveInteger,
                    day = MatchPositiveInteger
                }
            );

            _routes.MapRoute(
                name: "legacy-post-details",
                url: "post/{year}/{month}/{day}/{slug}.aspx",
                defaults: new { controller = "Legacy", action = "PostBySlug" },
                constraints: new
                {
                    year = MatchPositiveInteger,
                    month = MatchPositiveInteger,
                    day = MatchPositiveInteger
                }
            );

            _routes.MapRoute(
                name: "legacy-posts-by-category",
                url: "category/{slug}.aspx",
                defaults: new { controller = "Legacy", action = "PostsByCategory" }
            );

            _routes.MapRoute(
                name: "legacy-about",
                url: "page/About.aspx",
                defaults: new { controller = "Legacy", action = "About" }
            );

            _routes.MapRoute(
                name: "legacy-author",
                url: "author/gutek.aspx",
                defaults: new { controller = "Legacy", action = "About" }
            );

            _routes.MapRoute(
                name: "legacy-contact",
                url: "contact.aspx",
                defaults: new { controller = "Legacy", action = "Contact" }
            );

            _routes.MapRoute(
                name: "legacy-post-details-by-id",
                url: "post.aspx",
                defaults: new { controller = "Legacy", action = "PostByUid" }
            );
            _routes.MapRoute(
                name: "legacy-default-page",
                url: "default.aspx",
                defaults: new { controller = "Legacy", action = "AllPosts" }
            );
        }

        private void archive()
        {
            _routes.MapRoute(
                name: "by-year-month-day-paging",
                url: "{year}/{month}/{day}/strona/{page}",
                defaults: new { controller = "PostsByDate", action = "ByDate" },
                constraints: new
                {
                    page = MatchPositiveInteger,
                    year = MatchPositiveInteger,
                    month = MatchPositiveInteger,
                    day = MatchPositiveInteger
                }
            );

            _routes.MapRoute(
                name: "by-year-month-day",
                url: "{year}/{month}/{day}/",
                defaults: new { controller = "PostsByDate", action = "ByDate" },
                constraints: new
                {
                    year = MatchPositiveInteger,
                    month = MatchPositiveInteger,
                    day = MatchPositiveInteger
                }
            );
            _routes.MapRoute(
                name: "by-year-month-paging",
                url: "{year}/{month}/strona/{page}",
                defaults: new { controller = "PostsByDate", action = "ByDate" },
                constraints: new
                {
                    page = MatchPositiveInteger,
                    year = MatchPositiveInteger,
                    month = MatchPositiveInteger
                }
            );

            _routes.MapRoute(
                name: "by-year-month",
                url: "{year}/{month}/",
                defaults: new { controller = "PostsByDate", action = "ByDate" },
                constraints: new
                {
                    year = MatchPositiveInteger,
                    month = MatchPositiveInteger
                }
            );
            _routes.MapRoute(
                name: "by-year-paging",
                url: "{year}/strona/{page}",
                defaults: new { controller = "PostsByDate", action = "ByDate" },
                constraints: new
                {
                    page = MatchPositiveInteger,
                    year = MatchPositiveInteger
                }
            );

            _routes.MapRoute(
                name: "by-year",
                url: "{year}/",
                defaults: new { controller = "PostsByDate", action = "ByDate" },
                constraints: new
                {
                    year = MatchPositiveInteger
                }
            );

            _routes.MapRoute(
                name: "archive-paging",
                url: "archiwum/strona/{page}",
                defaults: new { controller = "PostsByDate", action = "Archive" },
                constraints: new
                {
                    page = MatchPositiveInteger
                }
            );

            _routes.MapRoute(
                name: "archive",
                url: "archiwum",
                defaults: new { controller = "PostsByDate", action = "Archive" }
            );
        }

        private void post()
        {
            _routes.MapRoute(
                name: "post-details",
                url: "{year}/{month}/{day}/{slug}",
                defaults: new { controller = "PostDetails", action = "Details" },
                constraints: new
                {
                    year = MatchPositiveInteger,
                    month = MatchPositiveInteger,
                    day = MatchPositiveInteger
                }
            );

            _routes.MapRoute(
                name: "post-comment-validate",
                url: "{id}/komentarz/sprawdz-email",
                defaults: new { controller = "PostComment", action = "ValidateEmail" },
                constraints: new
                {
                    id = MatchPositiveInteger
                }
            );

            _routes.MapRoute(
                name: "post-comment",
                url: "{id}/komentarz",
                defaults: new { controller = "PostComment", action = "Add" },
                constraints: new
                {
                    id = MatchPositiveInteger
                }
            );
        }

        private void posts()
        {
            _routes.MapRoute(
                name: "posts-paging",
                url: "strona/{page}",
                defaults: new { controller = "Posts", action = "AllPosts" },
                constraints: new { page = MatchPositiveInteger }
            );

            _routes.MapRoute(
                name: "tags",
                url: "tag/{slug}",
                defaults: new { controller = "PostsByTags", action = "ByTags" }
            );


            _routes.MapRoute(
                name: "categories-paging",
                url: "kategoria/{slug}/strona/{page}",
                defaults: new { controller = "PostsByCategory", action = "ByCategory" },
                constraints: new { page = MatchPositiveInteger }
            );

            _routes.MapRoute(
                name: "tags-paging",
                url: "tag/{slug}/strona/{page}",
                defaults: new { controller = "PostsByTags", action = "ByTags" },
                constraints: new { page = MatchPositiveInteger }
            );

            _routes.MapRoute(
                name: "categories",
                url: "kategoria/{slug}",
                defaults: new { controller = "PostsByCategory", action = "ByCategory" }
            );
        }

        private void errors()
        {
            _routes.MapRoute(
                name: "error-ravendb",
                url: "blad/ravendb",
                defaults: new { controller = "ErrorRaven", action = "RavenDb" }
            );
            _routes.MapRoute(
                name: "error-404",
                url: "blad/404",
                defaults: new { controller = "Error", action = "Error404" }
            );
            _routes.MapRoute(
                name: "error-ups",
                url: "blad/ups",
                defaults: new { controller = "Error", action = "Ups" }
            );
        }

        private void screw()
        {
            _routes.MapRoute(
                name: "screw-on",
                url: "przykrec-srubke",
                defaults: new { controller = "Screw", action = "On" }
            );

            _routes.MapRoute(
                name: "screw-done",
                url: "srubka-przykrecona",
                defaults: new { controller = "Screw", action = "Done" }
            );
        }
        private void about()
        {
            _routes.MapRoute(
                name: "about-me",
                url: "o-mnie",
                defaults: new { controller = "About", action = "Me" }
            );
        }

        private void aside()
        {
            _routes.MapRoute(
                name: "aside-me",
                url: "aside/o-mnie",
                defaults: new { controller = "Aside", action = "Me" }
            );
            _routes.MapRoute(
                name: "aside-twitter",
                url: "aside/twitter",
                defaults: new { controller = "Aside", action = "Twitter" }
            );

            _routes.MapRoute(
                name: "aside-recent-comments",
                url: "aside/ostatnie-komentarze",
                defaults: new { controller = "Aside", action = "RecentComments" }
            );
            _routes.MapRoute(
                name: "aside-recommended",
                url: "aside/polecam",
                defaults: new { controller = "Aside", action = "Recommend" }
            );
        }
    }
}