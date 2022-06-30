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
        public Balance Balance { get; set; } = new Balance { Eur = new Random().Next(1500, 3000), Btc = (decimal)new Random().NextDouble() };
    }
}
