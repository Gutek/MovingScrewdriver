using System;
using System.Web.Mvc;
using System.Web.Security;
using MovingScrewdriver.Web.Areas.admin.Models;
using MovingScrewdriver.Web.Controllers;
using MovingScrewdriver.Web.Extensions;

namespace MovingScrewdriver.Web.Areas.admin.Controllers
{
    public class LoginController : AbstractController 
    {
        [HttpGet]
        public ActionResult Index(string returnUrl)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectFromLoginPage();
            }

            return View(new SignInInput { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public ActionResult Index(SignInInput input)
        {
            var user = CurrentSession.GetUserByEmail(input.Login);

            if (user == null || user.ValidatePassword(input.Password) == false)
            {
                ModelState.AddModelError("UserNotExistOrPasswordNotMatch",
                                         "Email and password do not match to any known user.");
            }

            if (ModelState.IsValid)
            {
                FormsAuthentication.SetAuthCookie(input.Login, true);
                return RedirectFromLoginPage(input.ReturnUrl);
            }

            return View(new SignInInput { Login = input.Login, ReturnUrl = input.ReturnUrl });
        }

        private ActionResult RedirectFromLoginPage(string retrunUrl = null)
        {
            if (retrunUrl.IsNullOrEmpty())
            {
                return RedirectToAction("AllPosts", "Posts");
            }
                
            return Redirect(retrunUrl);
        }

        [HttpGet]
        public ActionResult LogOut(string returnurl)
        {
            FormsAuthentication.SignOut();
            return RedirectFromLoginPage(returnurl);
        }
    }
}