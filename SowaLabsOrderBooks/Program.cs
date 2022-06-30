using SowaLabsOrderBooks.HedgerClient;
using SowaLabsOrderBooks.Models;
using System;
using System.Text;

namespace SowaLabsOrderBooks
{
    class Program
    {
        private static readonly HedgerService _hedgerService = new HedgerService();
        private static readonly OrderBookService _orderBookService = new OrderBookService();

        static void Main(string[] args)
        {   
            SetChoice();           
        }

        private static void SetChoice()
        {
            string choice;
            do
            {
                Console.WriteLine($"SELLING BTCS - PRESS 1!");
                Console.WriteLine($"BUYING BTCS - PRESS 2!");
                choice = Console.ReadLine();

                if (int.TryParse(choice, out int result) && result == 1 || result == 2)
                {
                    string numberOfBtc;
                    do
                    {
                        var type = result == 1 ? OrderType.Sell : OrderType.Buy;
                        Console.WriteLine($"How many BTC do you want to {type}?");
                        numberOfBtc = Console.ReadLine();

                        if (decimal.TryParse(numberOfBtc, out decimal numOfBtc))
                        {
                            Console.WriteLine("Calculating. Please wait.");
                            var orderBooks = _orderBookService.ListOrderBooks();
                            var resultWithOrders = result == 1 ? _hedgerService.GetTheBestPriceForSeller(numOfBtc, orderBooks) : _hedgerService.GetTheBestPriceForBuyer(numOfBtc, orderBooks);
                            var balance = result == 1 ? "BTC" : "EUR";

                            var stringResult = new StringBuilder();
                            foreach (var order in resultWithOrders.Orders)
                            {
                                var balanceResult = result == 1 ? order.Btc : order.Eur;
                                stringResult.AppendLine($"{type} {order.Amount} where 1 BTC costs {order.Order.Price} in Excange - {order.ExcangeId}. Balance {balance} {balanceResult}");
                            }
                            stringResult.AppendLine($"Price is {resultWithOrders.TotalPrice}");
                            Console.WriteLine(stringResult.ToString());
                        }

                    } while (!decimal.TryParse(numberOfBtc, out decimal num));

                    Console.ReadLine();
                };
            } while (choice != "1" && choice != "2");
        }
    } 
}
