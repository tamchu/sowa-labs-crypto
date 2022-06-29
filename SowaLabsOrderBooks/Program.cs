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
            Console.WriteLine($"SELLING BTCS - PRESS 1!");
            Console.WriteLine($"BUYING BTCS - PRESS 2!");
            var choice = Console.ReadLine();

            if (int.TryParse(choice, out int result))
            {
                if (result != 1 && result != 2)
                {
                    Console.WriteLine("WRONG CHOICE! If you want to try again, press 1 for selling, 2 for buying or 3 for exit!");
                    choice = Console.ReadLine();
                    if (int.Parse(choice) == 1 || int.Parse(choice) == 2)
                    {
                        SetChoice();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    var type = result == 1 ? OrderType.Sell : OrderType.Buy;
                    Console.WriteLine($"How many BTC do you want to {type}?");
                    var numberOfBtc = Console.ReadLine();
                    if (!decimal.TryParse(numberOfBtc, out decimal numOfBtc))
                    {
                        Console.WriteLine($"Invalid input - {numberOfBtc}");
                        return;
                    }                        

                    Console.WriteLine("Calculating. Please wait.");
                    var orderBooks = _orderBookService.ListOrderBooks();
                    var stringResult = new StringBuilder();
                    if (result == 1)
                    {
                        var resultForSeller = _hedgerService.GetTheBestPriceForSeller(numOfBtc, orderBooks);
                        foreach (var order in resultForSeller.Orders)
                        {
                            stringResult.AppendLine($"{type} {order.Amount} where 1 BTC costs {order.Order.Price} in Excange - {order.ExcangeId}");
                        }
                        stringResult.AppendLine($"Price is {resultForSeller.TotalPrice}");
                        Console.WriteLine(stringResult.ToString());
                    }
                    else
                    {
                        var resultForBuyer = _hedgerService.GetTheBestPriceForBuyer(numOfBtc, orderBooks);
                        foreach (var order in resultForBuyer.Orders)
                        {
                            stringResult.AppendLine($"{type} {order.Amount} where 1 BTC costs {order.Order.Price} in Excange - {order.ExcangeId}");
                        }
                        stringResult.AppendLine($"Price is {resultForBuyer.TotalPrice}");
                        Console.WriteLine(stringResult.ToString());
                    }
                }

                Console.ReadLine();
            };
        }
    } 
}
