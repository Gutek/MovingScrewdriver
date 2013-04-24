using System.Diagnostics;
using Raven.Imports.Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;

namespace System
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object @this, bool forceNoFormatting = false)
        {
            if (@this == null)
            {
                return string.Empty;
            }

            var formatting = Debugger.IsAttached && !forceNoFormatting ? Formatting.Indented : Formatting.None;

            return JsonConvert.SerializeObject(@this, formatting);
        }

        public static Dictionary<string, object> ToDict(this object @this)
        {
            var dict = new Dictionary<string, object>();

            if (@this != null)
            {
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(@this);
                foreach (PropertyDescriptor prop in props)
                {
                    object val = prop.GetValue(@this);
                    dict.Add(prop.Name, val);
                }
            }

            return dict;
        }
    }
}