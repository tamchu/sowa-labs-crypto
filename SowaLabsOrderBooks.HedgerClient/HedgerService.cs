using SowaLabsOrderBooks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SowaLabsOrderBooks.HedgerClient
{
    public class HedgerService : IHedgerService
    {
        public OrdersResponse GetTheBestPriceForBuyer(decimal numberOfBtc, List<OrderBook> orderBooks)
        {
            var b = new List<KeyValuePair<int, Order>>();
            foreach (var orderBook in orderBooks)
            {
                foreach (var bid in orderBook.Asks)
                {
                    b.Add(new KeyValuePair<int, Order>(orderBook.OrderBookNumber, bid.Order));
                }
            }

            var orders = b.OrderBy(o => o.Value.Price).ToList();

            return Calculate(orders, numberOfBtc);
        }

        public OrdersResponse GetTheBestPriceForSeller(decimal numberOfBtc, List<OrderBook> orderBooks)
        {
            var b = new List<KeyValuePair<int, Order>>();
            foreach (var orderBook in orderBooks)
            {
                foreach (var bid in orderBook.Bids)
                {
                    b.Add(new KeyValuePair<int, Order>(orderBook.OrderBookNumber, bid.Order));
                }
            }

            var orders = b.OrderByDescending(o => o.Value.Price).ToList();

            return Calculate(orders, numberOfBtc);
        }

        private OrdersResponse Calculate(List<KeyValuePair<int, Order>> orders, decimal numberOfBtc)
        {
            var amount = 0.0M;
            var price = 0.0M;
            var result = new OrdersResponse { Orders = new List<OrderResult>() };

            foreach (var bid in orders)
            {
                if (amount < numberOfBtc)
                {
                    var reminder = numberOfBtc - amount;
                    if (bid.Value.Amount < reminder)
                    {
                        price += (bid.Value.Price * bid.Value.Amount);
                        amount += bid.Value.Amount;
                        result.Orders.Add(new OrderResult { ExcangeId = bid.Key, Order = bid.Value, Amount = bid.Value.Amount });
                    }
                    else
                    {
                        price += (reminder * bid.Value.Price);
                        result.Orders.Add(new OrderResult { ExcangeId = bid.Key, Order = bid.Value, Amount = reminder});
                        break;
                    }
                }
            }

            result.TotalPrice = price;

            return result;
        }
    }
}
