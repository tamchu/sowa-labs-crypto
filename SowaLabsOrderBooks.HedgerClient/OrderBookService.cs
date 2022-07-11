using Newtonsoft.Json;
using SowaLabsOrderBooks.Models;
using System.Collections.Generic;
using System.Linq;

namespace SowaLabsOrderBooks.HedgerClient
{
    public class OrderBookService : IOrderBookService
    {
        private static readonly string OrderBooksFilePath = @"Files/order_books_data1";

        public List<OrderBook> ListOrderBooks(string path = null)
        {
            var orderBooksFilePath = path == null ? OrderBooksFilePath : path;
            var text = System.IO.File.ReadAllText(orderBooksFilePath);
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
