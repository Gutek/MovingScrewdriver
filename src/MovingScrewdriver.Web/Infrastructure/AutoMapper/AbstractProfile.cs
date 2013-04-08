using System.Web;
using System.Web.Mvc;
using AutoMapper;

namespace MovingScrewdriver.Web.Infrastructure.AutoMapper
{
    public abstract class AbstractProfile : Profile
    {
        protected UrlHelper UrlHelper
        {
            get { return new UrlHelper(HttpContext.Current.Request.RequestContext); }
        }
    }
}