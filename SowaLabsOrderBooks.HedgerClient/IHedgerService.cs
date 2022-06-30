using SowaLabsOrderBooks.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SowaLabsOrderBooks.HedgerClient
{
    public interface IHedgerService
    {
        OrdersResult GetTheBestPriceForSeller(decimal numberOfBtc, List<OrderBook> orderBooks);
        OrdersResult GetTheBestPriceForBuyer(decimal numberOfBtc, List<OrderBook> orderBooks);
    }
}
