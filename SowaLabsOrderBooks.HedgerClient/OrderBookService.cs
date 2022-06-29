using Newtonsoft.Json;
using SowaLabsOrderBooks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SowaLabsOrderBooks.HedgerClient
{
    public class OrderBookService : IOrderBookService
    {
        private static readonly string OrderBooksFilePath = @".\Files\order_books_data";

        public List<OrderBook> ListOrderBooks()
        {
            var text = System.IO.File.ReadAllText(OrderBooksFilePath);
            var lines = text.Split('\n').Where(l => !string.IsNullOrWhiteSpace(l)).ToList();

            var orderBooks = new List<OrderBook>();
            for (var i = 1; i <= lines.Count; i++)
            {
                var splitLine = lines[i - 1].Split('\t');
                var orderBookInString = splitLine.Last();

                var orderBook = JsonConvert.DeserializeObject<OrderBook>(orderBookInString);
                orderBook.OrderBookNumber = i;
                orderBooks.Add(orderBook);
            }

            return orderBooks;
        }
    }
}
