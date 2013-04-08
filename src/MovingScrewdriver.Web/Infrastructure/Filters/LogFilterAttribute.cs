using System;
using System.Web.Mvc;
using NLog;

namespace MovingScrewdriver.Web.Infrastructure.Filters
{
    public class LogFilterAttribute : ActionFilterAttribute
    {
        private Logger get_loger(ControllerContext controller)
        {
            return LogManager.GetLogger(controller.Controller.GetType().FullName);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var log = get_loger(filterContext);
            log.Trace(() => LogInActionExecutingContext(filterContext));
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var log = get_loger(filterContext);
            log.Trace(() => LogInActionExecutedContext(filterContext));
            base.OnActionExecuted(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var log = get_loger(filterContext);
            log.Trace(() => LogInActionExecutingContext(filterContext));
            base.OnResultExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var log = get_loger(filterContext);
            log.Trace(() => LogInResultExecutedContext(filterContext));
            base.OnResultExecuted(filterContext);
        }


        private static string LogInActionExecutingContext(ActionExecutingContext filterContext)
        {
            return string.Format("{0}|{1}|{2}|Ajax: {3}|Action Executing|{4}",
                      filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                      filterContext.ActionDescriptor.ActionName,
                      filterContext.HttpContext.Request.RequestType,
                      filterContext.HttpContext.Request.IsAjaxRequest(),
                      filterContext.ActionParameters.ToJson());
        }

        private static string LogInActionExecutedContext(ActionExecutedContext filterContext)
        {
            return string.Format("{0}|{1}|{2}|Ajax: {3}|Action Executed|Result: {4}",
                      filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                      filterContext.ActionDescriptor.ActionName,
                      filterContext.HttpContext.Request.RequestType,
                      filterContext.HttpContext.Request.IsAjaxRequest(),
                      filterContext.Result is FileResult ? "image" : filterContext.Result.ToJson());
        }

        private static string LogInActionExecutingContext(ResultExecutingContext filterContext)
        {
            string controllerName = (string)filterContext.RouteData.Values["controller"];
            string actionName = (string)filterContext.RouteData.Values["action"];

            return string.Format("{0}|{1}|{2}|Ajax: {3}|View rendering|",
                      controllerName,
                      actionName,
                      filterContext.HttpContext.Request.RequestType,
                      filterContext.HttpContext.Request.IsAjaxRequest());
        }

        private static string LogInResultExecutedContext(ResultExecutedContext filterContext)
        {
            string controllerName = (string)filterContext.RouteData.Values["controller"];
            string actionName = (string)filterContext.RouteData.Values["action"];

            return string.Format("{0}|{1}|{2}|Ajax: {3}|View rendered|",
                      controllerName,
                      actionName,
                      filterContext.HttpContext.Request.RequestType,
                      filterContext.HttpContext.Request.IsAjaxRequest());
        }
    }
}