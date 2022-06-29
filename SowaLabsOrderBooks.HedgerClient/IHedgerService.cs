using SowaLabsOrderBooks.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SowaLabsOrderBooks.HedgerClient
{
    public interface IHedgerService
    {
        OrdersResponse GetTheBestPriceForSeller(decimal numberOfBtc, List<OrderBook> orderBooks);
        OrdersResponse GetTheBestPriceForBuyer(decimal numberOfBtc, List<OrderBook> orderBooks);
    }
}
