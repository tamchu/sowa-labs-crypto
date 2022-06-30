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
            var orders = new List<OrderForCalculate>();
            foreach (var orderBook in orderBooks)
            {
                foreach (var ask in orderBook.Asks)
                {
                    orders.Add(new OrderForCalculate { OrderBookNumber = orderBook.OrderBookNumber, Order = ask.Order, Balance = orderBook.Balance });
                }
            }

            return CalculateForBuyer(orders.OrderBy(o => o.Order.Price).ToList(), numberOfBtc);
        }

        public OrdersResponse GetTheBestPriceForSeller(decimal numberOfBtc, List<OrderBook> orderBooks)
        {
            var orders = new List<OrderForCalculate>();
            foreach (var orderBook in orderBooks)
            {
                foreach (var bid in orderBook.Bids)
                {
                    orders.Add(new OrderForCalculate { OrderBookNumber = orderBook.OrderBookNumber, Order = bid.Order, Balance = orderBook.Balance });
                }
            }

            return CalculateForSeller(orders.OrderByDescending(o => o.Order.Price).ToList(), numberOfBtc);
        }        

        private OrdersResponse CalculateForBuyer(List<OrderForCalculate> orders, decimal numberOfBtc)
        {
            var amount = 0.0M;
            var price = 0.0M;
            var result = new OrdersResponse { Orders = new List<OrderResult>() };

            foreach (var order in orders)
            {
                if (amount < numberOfBtc)
                {
                    var reminder = numberOfBtc - amount;
                    if (order.Order.Amount < reminder)
                    {
                        var currentPrice = order.Order.Price * order.Order.Amount;
                        if (currentPrice <= order.Balance.Eur)
                        {
                            price += currentPrice;
                            amount += order.Order.Amount;                                
                            result.Orders.Add(new OrderResult { ExcangeId = order.OrderBookNumber, Order = order.Order, Amount = order.Order.Amount, Eur = order.Balance.Eur});
                            order.Balance.Eur -= currentPrice;
                        }
                        else
                        {
                            var currentAmount = (order.Balance.Eur * order.Order.Amount) / currentPrice;
                            price += order.Balance.Eur;
                            amount += currentAmount;                                
                            result.Orders.Add(new OrderResult { ExcangeId = order.OrderBookNumber, Order = order.Order, Amount = currentAmount, Eur = order.Balance.Eur });
                            order.Balance.Eur = 0.0M;
                        }                        
                    }
                    else
                    {
                        var reminderPrice = reminder * order.Order.Price;
                        if (reminderPrice <= order.Balance.Eur)
                        {
                            price += reminderPrice;
                            result.Orders.Add(new OrderResult { ExcangeId = order.OrderBookNumber, Order = order.Order, Amount = reminder, Eur = order.Balance.Eur });
                            break;
                        }
                        else
                        {
                            var ca = (order.Balance.Eur * order.Order.Amount) / reminderPrice;
                            price += order.Balance.Eur;
                            amount += ca;
                            result.Orders.Add(new OrderResult { ExcangeId = order.OrderBookNumber, Order = order.Order, Amount = ca, Eur = order.Balance.Eur });
                            order.Balance.Eur = 0.0M;
                        }                        
                    }
                }
            }

            result.TotalPrice = price;

            return result;
        }

        private OrdersResponse CalculateForSeller(List<OrderForCalculate> orders, decimal numberOfBtc)
        {
            var amount = 0.0M;
            var price = 0.0M;
            var result = new OrdersResponse { Orders = new List<OrderResult>() };

            foreach (var order in orders)
            {
                if (amount < numberOfBtc)
                {
                    var reminder = numberOfBtc - amount;
                    if (order.Order.Amount < reminder)
                    {
                        if (order.Order.Amount <= order.Balance.Btc)
                        {
                            price += (order.Order.Price * order.Order.Amount);
                            amount += order.Order.Amount;
                            result.Orders.Add(new OrderResult { ExcangeId = order.OrderBookNumber, Order = order.Order, Amount = order.Order.Amount, Btc = order.Balance.Btc });
                            order.Balance.Btc -= order.Order.Amount;
                        }
                        else
                        {
                            price += (order.Order.Price * order.Balance.Btc);
                            amount += order.Balance.Btc;
                            result.Orders.Add(new OrderResult { ExcangeId = order.OrderBookNumber, Order = order.Order, Amount = order.Balance.Btc, Btc = order.Balance.Btc });
                            order.Balance.Btc = 0.0M;
                        }
                    }
                    else
                    {                        
                        if (reminder <= order.Balance.Btc)
                        {
                            price += reminder * order.Order.Price;
                            result.Orders.Add(new OrderResult { ExcangeId = order.OrderBookNumber, Order = order.Order, Amount = reminder, Btc = order.Balance.Btc });
                            break;
                        }
                        else
                        {
                            amount += order.Balance.Btc;
                            price += order.Balance.Btc * order.Order.Price;
                            result.Orders.Add(new OrderResult { ExcangeId = order.OrderBookNumber, Order = order.Order, Amount = reminder, Btc = order.Balance.Btc });
                        }                        
                    }
                }
            }

            result.TotalPrice = price;

            return result;
        }
    }
}
