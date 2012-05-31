using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImprintCMS.Models;

namespace ImprintCMS.Controllers
{
    public class ShopController : SiteControllerBase
    {

        const string _cookieName = "ImprintCMSShopId";
        Guid _shopId;

        public ActionResult Index()
        {
            var vm = Repository.GetOrder(_shopId);
            if (vm != null)
                vm.DistributionCost = vm.Subtotal >= Config.DistributionCostLimit ? 0 : Config.DistributionCostAmount;
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
