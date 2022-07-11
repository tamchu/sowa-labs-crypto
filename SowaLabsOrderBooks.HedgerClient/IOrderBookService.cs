using SowaLabsOrderBooks.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SowaLabsOrderBooks.HedgerClient
{
    public interface IOrderBookService
    {
        List<OrderBook> ListOrderBooks(string path = null);
    }
}
