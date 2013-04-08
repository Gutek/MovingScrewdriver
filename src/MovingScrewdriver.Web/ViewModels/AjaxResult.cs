using System;
using System.Collections.Generic;

namespace MovingScrewdriver.Web.ViewModels
{
    public class AjaxResult
    {
        public bool Success { get; set; }
        public bool Redirect { get { return RedirectUrl.IsNotNullOrEmpty(); } }
        public string RedirectUrl { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public IEnumerable<object> Errors { get; set; }
    }
}