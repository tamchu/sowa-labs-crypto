using SowaLabsOrderBooks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SowaLabsOrderBooks.HedgerClient
{
    public class HedgerService : IHedgerService
    {
        public OrdersResult GetTheBestPriceForBuyer(decimal numberOfBtc, List<OrderBook> orderBooks)
        {
            var orders = new List<OrderForCalculate>();
            foreach (var orderBook in orderBooks)
            {
                orders.AddRange(orderBook.Asks.Select(ask => new OrderForCalculate { OrderBookNumber = orderBook.OrderBookNumber, Order = ask.Order, Balance = orderBook.Balance }));
            }

            return CalculateForBuyer(orders.OrderBy(o => o.Order.Price).ToList(), numberOfBtc);
        }

        public OrdersResult GetTheBestPriceForSeller(decimal numberOfBtc, List<OrderBook> orderBooks)
        {
            var orders = new List<OrderForCalculate>();
            foreach (var orderBook in orderBooks)
            {
                orders.AddRange(orderBook.Bids.Select(bid => new OrderForCalculate { OrderBookNumber = orderBook.OrderBookNumber, Order = bid.Order, Balance = orderBook.Balance }));
            }

            return CalculateForSeller(orders.OrderByDescending(o => o.Order.Price).ToList(), numberOfBtc);
        }

        private OrdersResult CalculateForBuyer(List<OrderForCalculate> orders, decimal amountOfBtcToBuy)
        {
            var result = new OrdersResult { Orders = new List<OrderResult>() };

            foreach (var order in orders)
            {
                if (order.Balance.Eur == 0.0M) continue;

                var reminder = amountOfBtcToBuy - result.TotalAmount;

                var currentPrice = order.Order.Amount <= reminder
                    ? order.Order.Amount * order.Order.Price
                    : reminder * order.Order.Price;

                var calculatedAmount = order.Order.Amount <= reminder ? order.Order.Amount : reminder;
                var calculatedPrice = currentPrice <= order.Balance.Eur ? currentPrice : order.Balance.Eur;
                if (currentPrice > order.Balance.Eur)
                {
                    calculatedAmount = CalculateAmount(calculatedAmount, order.Balance.Eur, currentPrice);
                }

                result.TotalPrice += calculatedPrice;
                result.TotalAmount += calculatedAmount;
                result.Orders.Add(new OrderResult { ExcangeId = order.OrderBookNumber, Order = order.Order, Amount = calculatedAmount, Eur = order.Balance.Eur });
                order.Balance.Eur -= calculatedPrice;

                if (result.TotalAmount == amountOfBtcToBuy) break;

                if (result.TotalAmount > amountOfBtcToBuy)
                    throw new Exception("CRITICAL - going over.");
            }

            return result;
        }

        private OrdersResult CalculateForSeller(List<OrderForCalculate> orders, decimal amountOfBtcToSell)
        {
            var result = new OrdersResult { Orders = new List<OrderResult>() };

            foreach (var order in orders)
            {
                if (order.Balance.Btc == 0.0M) continue;

                var reminder = amountOfBtcToSell - result.TotalAmount;

                var currentAmount = order.Order.Amount <= reminder ? order.Order.Amount : reminder;
                var calculatedAmount = currentAmount <= order.Balance.Btc ? currentAmount : order.Balance.Btc;

                result.TotalPrice += order.Order.Price * calculatedAmount;
                result.TotalAmount += calculatedAmount;
                result.Orders.Add(new OrderResult { ExcangeId = order.OrderBookNumber, Order = order.Order, Amount = calculatedAmount, Btc = order.Balance.Btc });
                order.Balance.Btc -= calculatedAmount;

                if (result.TotalAmount == amountOfBtcToSell) break;

                if (result.TotalAmount > amountOfBtcToSell)
                    throw new Exception("CRITICAL - going over.");
            }

            return result;
        }

        private decimal CalculateAmount(decimal amount, decimal balance, decimal currentPrice)
        {
            return (balance * amount) / currentPrice;
        }
    }
}
