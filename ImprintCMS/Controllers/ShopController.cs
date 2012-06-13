using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImprintCMS.Models;
using ImprintCMS.Models.ViewModels;

namespace ImprintCMS.Controllers
{
    public class ShopController : SiteControllerBase
    {

        const string _cookieName = "ImprintCMSShopId";
        Guid _shopId;

        public ActionResult Index()
        {
            var vm = Repository.GetOrder(_shopId);
            CalculateDistributionCost(vm);
            return View(vm);
        }

        public ActionResult Details()
        {
            var order = Repository.GetOrder(_shopId);
            if (order == null)
                return RedirectToAction("index");
            var vm = new OrderDetails
            {
                Name = order.Name,
                Address = order.Address,
                Postcode = order.Postcode,
                City = order.City,
                Phone = order.Phone,
                Email = order.Email
            };
            return View(vm);
        }

        [HttpPost]
        public ActionResult Details(OrderDetails vm)
        {
            var order = Repository.GetOrder(_shopId);
            if (order == null)
                return RedirectToAction("index");
            if (!ModelState.IsValid)
                return View(vm);
            order.Name = vm.Name;
            order.Address = vm.Address;
            order.Postcode = vm.Postcode;
            order.City = vm.City;
            order.Email = vm.Email;
            order.Phone = vm.Phone;
            Repository.Save();
            return RedirectToAction("confirm");
        }

        public ActionResult Confirm()
        {
            var vm = Repository.GetOrder(_shopId);
            if (vm == null)
                return RedirectToAction("index");
            CalculateDistributionCost(vm);
            return View(vm);
        }

        [HttpPost]
        public ActionResult Confirm(Order vm)
        {
            var order = Repository.GetOrder(_shopId);
            if (order == null)
                return RedirectToAction("index");
            CloseOrder(order);
            return RedirectToAction("receipt", new { id = order.ExternalId });
        }

        public ActionResult Receipt(Guid id)
        {
            var vm = Repository.GetOrder(id);
            if (vm == null)
                return HttpNotFound();
            return View(vm);
        }

        public ActionResult Count()
        {
            var count = 0;
            var order = Repository.GetOrder(_shopId);
            if (order == null) return ShopCount(count);
            return ShopCount(order.OrderLines.Count());
        }

        public ActionResult Add(int id)
        {
            var edition = Repository.GetEdition(id);
            if (edition == null) return HttpNotFound();
            var order = GetOrSetOrder(_shopId);
            Repository.Add(new OrderLine
            {
                OrderId = order.Id,
                EditionId = id
            });
            Repository.Save();
            return ShopReturn();
        }

        public ActionResult Empty()
        {
            ResetShop();
            return ShopReturn();
        }

        public ActionResult Remove(int id)
        {
            var line = Repository.GetOrderLine(id);
            if (line == null) return HttpNotFound();
            if (line.Order.OrderLines.Count() < 2)
            {
                ResetShop();
            }
            else
            {
                Repository.Delete(line);
                Repository.Save();
            }
            return ShopReturn();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            _shopId = Guid.Parse(GetOrSetCookie().Value);
        }

        private void ResetShop()
        {
            var order = GetOrSetOrder(_shopId);
            if (order == null) return;
            Repository.Delete(order);
            Repository.Save();
            Response.Cookies[_cookieName].Value = Guid.NewGuid().ToString();
        }

        private ActionResult ShopReturn()
        {
            return Request.UrlReferrer != null ? (ActionResult)Redirect(Request.UrlReferrer.PathAndQuery) : RedirectToAction("index");
        }

        private JsonResult ShopCount(int count)
        {
            return Json(new { shopCount = count }, JsonRequestBehavior.AllowGet);
        }

        private Order GetOrSetOrder(Guid guid)
        {
            var order = Repository.GetOrder(guid);
            if (order == null)
            {
                Repository.Add(new Order
                {
                    ExternalId = guid,
                    CreatedAt = DateTime.Now
                });
                Repository.Save();
            }
            return Repository.GetOrder(guid);
        }

        private void CalculateDistributionCost(Order order)
        {
            if (order == null) return;
            order.DistributionCost = order.Subtotal >= Config.DistributionCostLimit ? 0 : Config.DistributionCostAmount;
        }

        private void CloseOrder(Order order)
        {
            CalculateDistributionCost(order);
            order.ClosedAt = DateTime.Now;
            Repository.Save();
            Response.Cookies[_cookieName].Value = Guid.NewGuid().ToString();
        }

        private HttpCookie GetOrSetCookie()
        {
            var cookie = Request.Cookies[_cookieName];
            if (cookie == null)
            {
                cookie = new HttpCookie(_cookieName)
                {
                    Expires = DateTime.Today.AddYears(99),
                    Value = Guid.NewGuid().ToString()
                };
                Response.SetCookie(cookie);
            }
            return cookie;
        }

    }
}
