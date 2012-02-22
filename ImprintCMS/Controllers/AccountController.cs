using System;
using System.Web.Mvc;
using System.Web.Security;
using ImprintCMS.Models;
using ImprintCMS.Models.ViewModels;

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
				ModelState.AddModelError("", Phrases.ValidationInvalidLogin);
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
