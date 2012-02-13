using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImprintCMS.Models.ViewModels;
using System.Web.Security;

namespace ImprintCMS.Controllers
{
	public class AccountController : Controller
	{
		public ActionResult LogIn(string ReturnUrl)
		{
			var vm = new Login
			{
				ReturnUrl = ReturnUrl
			};
			return View(vm);
		}

		[HttpPost]
		public ActionResult LogIn(Login vm)
		{
			if (!ModelState.IsValid)
			{
				return View(vm);
			}
			if (!FormsAuthentication.Authenticate(vm.Username, vm.Password))
			{
				ModelState.AddModelError("", "Invalid username/password");
				return View(vm);
			}
			FormsAuthentication.SetAuthCookie(vm.Username, true);
			return !String.IsNullOrWhiteSpace(vm.ReturnUrl) ? (ActionResult)Redirect(vm.ReturnUrl) : (ActionResult)RedirectToAction("index", "home");
		}

		public ActionResult LogOut(string ReturnUrl)
		{
			FormsAuthentication.SignOut();
			return !String.IsNullOrWhiteSpace(ReturnUrl) ? (ActionResult)Redirect(ReturnUrl) : (ActionResult)RedirectToAction("index", "home");
		}

	}
}
