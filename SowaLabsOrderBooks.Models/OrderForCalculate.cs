using System;
using System.Collections.Generic;
using System.Text;

namespace SowaLabsOrderBooks.Models
{
    public class OrderForCalculate
    {
        public int OrderBookNumber { get; set; }
        public Order Order { get; set; }
        public Balance Balance { get; set; }
    }
}
