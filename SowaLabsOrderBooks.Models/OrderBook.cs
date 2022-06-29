using System;
using System.Collections.Generic;
using System.Text;

namespace SowaLabsOrderBooks.Models
{
    public class OrderBook
    {
        public int OrderBookNumber { get; set; }
        public DateTimeOffset AcqTime { get; set; }
        public List<Bids> Bids { get; set; }
        public List<Asks> Asks { get; set; }
    }
}
