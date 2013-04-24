using System;
using System.Linq;
using System.Web.Mvc;
using NLog;

namespace MovingScrewdriver.Web.Infrastructure.ActionAttributes
{
    public class AjaxOnlyAttribute : ActionMethodSelectorAttribute
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        public override bool IsValidForRequest(ControllerContext controllerContext, System.Reflection.MethodInfo methodInfo)
        {
            var isAjax = controllerContext.RequestContext.HttpContext.Request.IsAjaxRequest();

            if (isAjax == false)
            {
                object obj = null;
                object form = null; 
                try
                {
                    obj = controllerContext.Controller.ValueProvider.GetValue("input");

                    var local = controllerContext.HttpContext.Request.Form;
                    form = local.AllKeys.SelectMany(local.GetValues, (k, v) => new { key = k, value = v });
                }
                catch {}
                _log.Warn("--START--");
                _log.Warn("Someone is trying to execute ajax only action with normal POST");
                _log.Warn("OBJ Data posted instead of ajax: {0}", obj.ToJson());
                _log.Warn("FORM Data posted instead of ajax: {0}", form.ToJson());
                _log.Warn("--END--");
            }

            return isAjax;
        }
    }
}