using System.Linq;

namespace ImprintCMS.Models
{
    public partial class Order
    {
        public decimal Subtotal
        {
            get
            {
                return OrderLines.Sum(l => (int)l.Edition.Price);
            }
        }
    }
}