using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using AutoMapper;
using MovingScrewdriver.Web.Infrastructure.ActionResults;
using MovingScrewdriver.Web.Models;
using Raven.Client;

namespace MovingScrewdriver.Web.Controllers
{
    public abstract class AbstractController : Controller
    {
        public IDocumentSession CurrentSession { get; set; }
        public IMappingEngine Mapper { get; set; }

        private ScrewdriverConfig _blogConfig;
        private BlogOwner _blogOwner;
        public ScrewdriverConfig BlogConfig
        {
            get
            {
                if (_blogConfig == null)
                {
                    using (CurrentSession.Advanced.DocumentStore.AggressivelyCacheFor(TimeSpan.FromMinutes(5)))
                    {
                        _blogConfig = CurrentSession.Load<ScrewdriverConfig>("Blog/Config");
                    }
                }

                return _blogConfig;
            }
        }
        public BlogOwner BlogOwner
        {
            get
            {
                if (_blogOwner == null)
                {
                    using (CurrentSession.Advanced.DocumentStore.AggressivelyCacheFor(TimeSpan.FromMinutes(5)))
                    {
                        _blogOwner = CurrentSession.Load<BlogOwner>("Blog/Owner");
                    }
                }

                return _blogOwner;
            }
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            ViewBag.BlogConfig = BlogConfig;
            ViewBag.BlogOwner = BlogOwner;
        }

        protected IEnumerable<object> ValidationErrors
        {
            get
            {
                var errors = from m in ModelState
                             where m.Value.Errors.Count > 0
                             select new
                             {
                                 key = "{0}{1}".FormatWith(char.ToLower(m.Key[0]), m.Key.Substring(1)),
                                 errors = m.Value.Errors.Select(x => x.ErrorMessage).ToList()
                             };


                return errors.ToList();
            }
        }

        protected ActionResult XDoc(XDocument document
            , string etag = ""
            , string contentType = "")
        {
            return new XResult(document, etag, contentType);
        }

        protected HttpStatusCodeResult HttpNotModified()
        {
            return new HttpStatusCodeResult(304);
        }
    }
}