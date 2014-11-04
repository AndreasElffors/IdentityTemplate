using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace IdentityTemplate.Infrastructure
{
    /// <summary>
    /// We use these methods to make the output in the views easier to read.
    /// </summary>
    public static class IdentityHelpers
    {
        public static MvcHtmlString ClaimType(this HtmlHelper html, string claimType)
        {
            FieldInfo[] fields = typeof (ClaimTypes).GetFields();
            foreach (var item in fields)
            {
                if (item.GetValue(null).ToString() == claimType)
                {
                    return new MvcHtmlString(item.Name);
                }
            }
            return new MvcHtmlString(string.Format("{0}",claimType.Split('/','.').Last()));
        }
    }
}