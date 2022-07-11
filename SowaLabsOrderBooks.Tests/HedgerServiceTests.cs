using FluentAssertions;
using SowaLabsOrderBooks.HedgerClient;
using SowaLabsOrderBooks.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace SowaLabsOrderBook.Tests
{
    public class HedgerServiceTests
    {
        private static readonly string OrderBooksFilePath = @"Files/order_books_data";
        private readonly OrderBookService _orderBookService = new OrderBookService();
        private readonly HedgerService _hedgerService = new HedgerService();

        [Fact]
        public void GetTheBestPriceForSeller_for_1_btc_should_succeed()
        {
            var orderBooks = _orderBookService.ListOrderBooks(OrderBooksFilePath);
            var btcBalance = 0.5M;
            foreach (var orderBook in orderBooks)
            {
                orderBook.Balance.Btc = btcBalance;
            }

            var result = _hedgerService.GetTheBestPriceForSeller(1, orderBooks);
            result.Orders.Should().HaveCount(2);
            result.Orders.TrueForAll(o => o.Btc == btcBalance);
            result.TotalAmount.Should().Be(1);            
        }

        [Fact]
        public void GetTheBestPriceForSeller_for_2_btc_should_succeed()
        {
            var orderBooks = _orderBookService.ListOrderBooks(OrderBooksFilePath);
            var btcBalance = 0.5M;
            foreach (var orderBook in orderBooks)
            {
                orderBook.Balance.Btc = btcBalance;
            }

            var result = _hedgerService.GetTheBestPriceForSeller(2, orderBooks);
            result.Orders.Should().HaveCount(3);
            result.Orders.TrueForAll(o => o.Btc == btcBalance);
            result.TotalAmount.Should().Be(1.5M);
        }

        [Fact]
        public void GetTheBestPriceForBuyer_for_1_btc_should_succeed()
        {
            var orderBooks = _orderBookService.ListOrderBooks(OrderBooksFilePath);
            var eurBalance = 2500;
            foreach (var orderBook in orderBooks)
            {
                orderBook.Balance.Eur = eurBalance;
            }

            var result = _hedgerService.GetTheBestPriceForBuyer(1, orderBooks);
            result.Orders.Should().HaveCount(3);
            result.Orders[0].Amount.Should().Be(0.405M);
            result.Orders[1].Amount.Should().Be(0.405M);
            result.Orders[2].Amount.Should().Be(0.190M);
            result.TotalAmount.Should().Be(1);           
            result.TotalPrice.Should().Be(2964.29000M);
        }

        [Fact]
        public void GetTheBestPriceForBuyer_for_2_btc_should_succeed()
        {
            var orderBooks = _orderBookService.ListOrderBooks(OrderBooksFilePath);
            var eurBalance = 2500;
            foreach (var orderBook in orderBooks)
            {
                orderBook.Balance.Eur = eurBalance;
            }

            var result = _hedgerService.GetTheBestPriceForBuyer(2, orderBooks);
            result.Orders.Should().HaveCount(5);
            result.Orders[0].Amount.Should().Be(0.405M);
            result.Orders[1].Amount.Should().Be(0.405M);
            result.Orders[2].Amount.Should().Be(0.405M);
            result.Orders[3].Amount.Should().Be(0.405M);
            result.Orders[4].Amount.Should().Be(0.380M);
            result.TotalAmount.Should().Be(2);
            result.TotalPrice.Should().Be(5928.58785M);
        }
    }
}
