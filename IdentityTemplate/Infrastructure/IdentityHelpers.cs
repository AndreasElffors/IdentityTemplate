using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using IdentityTemplate.Models;
using Microsoft.AspNet.Identity.Owin;

namespace IdentityTemplate.Infrastructure
{
    /// <summary>
    /// We use these methods to make the output in the views easier to read.
    /// </summary>
    public static class IdentityHelpers
    {
        public static MvcHtmlString GetUserName(this HtmlHelper html, string id)
        {
            ApplicationUserManager manager =
                HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            return new MvcHtmlString(manager.FindByIdAsync(id).Result.UserName);
        }

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