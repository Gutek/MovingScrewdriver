using System;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace MovingScrewdriver.Web.Infrastructure.ActionResults
{
    public class XResult : ActionResult
    {
        private readonly XDocument _document;
        private readonly string _etag;
        private readonly string _contentType;

        public XResult(XDocument document, string etag, string contentType)
        {
            _document = document;
            _etag = etag;
            _contentType = contentType;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (_etag.IsNotNullOrEmpty())
            {
                context.HttpContext.Response.AddHeader("ETag", _etag);
            }
            
            context.HttpContext.Response.ContentType = _contentType.IsNullOrWhiteSpace() ? "text/xml" : _contentType;

            using (var xmlWriter = XmlWriter.Create(context.HttpContext.Response.OutputStream))
            {
                _document.WriteTo(xmlWriter);
                xmlWriter.Flush();
            }
        }
    }
}