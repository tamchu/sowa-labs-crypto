using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SowaLabsOrderBooks.Models
{
    public class OrdersResponse
    {
        public decimal TotalPrice { get; set; }
        public List<OrderResult> Orders { get; set; }        
    }

    public class OrderResult
    {
        public int ExcangeId { get; set; }
        public Order Order { get; set; }
        public decimal Amount { get; set; }
        public decimal? Eur { get; set; }
        public decimal? Btc { get; set; }
    }
}
