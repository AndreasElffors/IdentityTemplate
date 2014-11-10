using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Facebook;
using IdentityTemplate.Infrastructure;
using IdentityTemplate.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace IdentityTemplate.Controllers
{
    public class ClaimsController : Controller
    {
        // GET: Claims
        public async Task<ActionResult> Index()
        {
            var externalIdentity = await HttpContext.GetOwinContext().Authentication.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);
            if (externalIdentity != null)
            {
                var facebookAccessToken = externalIdentity.FindAll("FacebookAccessToken").First();
                var facebook = new FacebookClient(facebookAccessToken.Value);
                dynamic myInfo = facebook.Get("/me/friends");
                var friendsList = new List<FacebookViewModel>();
                foreach (dynamic friend in myInfo.data)
                {
                    friendsList.Add(new FacebookViewModel()
                    {
                        Name=friend.name,
                        ImgUrl = @"https://graph.facebook.com/" + friend.id+ "/picture?type=large"
                    });
                }

                ViewBag.FacebookFriends = friendsList;
                if (externalIdentity.Claims.FirstOrDefault().Issuer=="Google")
                {
                    var picture = externalIdentity.Claims.FirstOrDefault(c => c.Type.Equals("picture")).Value;
                    var profile = externalIdentity.Claims.FirstOrDefault(c => c.Type.Equals("profile")).Value;
                    if (picture != null)
                    {
                        ViewBag.Picture = picture;
                        ViewBag.Profile = profile;
                    }
                }

                return View(externalIdentity.Claims); 
            }
            ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return View("About", "Home");
            }
            else
            {
                return View(identity.Claims); 
            }
        }
    }
}