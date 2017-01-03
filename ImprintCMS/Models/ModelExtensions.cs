using System;
using System.Linq;

namespace ImprintCMS.Models
{
    public static class ModelExtensions
    {

        public static string HandlerNotificationBody(this Order order)
        {
            return String.Format(Phrases.EmailBodyWrapper, String.Format(
                "{0}: {1}<br/><br/>{2}<br/>{3}<br/>{4}<br/>{5}<br/>{6}<br/>{7}<br/><br/>{8}<br/>{9}: {10:f2}<br/>{11}: {12:f2}<br/>{13}: {14:f2}",
                Phrases.LabelOrderId,
                order.Id,
                order.Name,
                order.Address,
                order.Postcode,
                order.City,
                order.Phone,
                order.Email,
                String.Join("<br/>", order.OrderLines.Select(l => String.Format(
                    "{0} : {1} : {2:f2}",
                    l.Edition.Isbn,
                    l.Edition.Name,
                    l.Edition.Price
                    )).ToArray()),
                Phrases.LabelOrderSubtotal,
                order.Subtotal,
                Phrases.LabelOrderDistributionCost,
                order.DistributionCost,
                Phrases.LabelOrderTotal,
                order.Subtotal + order.DistributionCost
                )
            );
        }

    }
}