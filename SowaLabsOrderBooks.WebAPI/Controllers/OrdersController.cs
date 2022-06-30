using Microsoft.AspNetCore.Mvc;
using SowaLabsOrderBooks.HedgerClient;
using SowaLabsOrderBooks.Models;

namespace SowaLabsOrderBooks.WebAPI.Controllers
{
    [ApiController]
    [Route("v1/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderBookService _orderBookService;
        private readonly IHedgerService _hedgerService;

        public OrdersController(IOrderBookService orderBookService, IHedgerService hedgerService)
        {
            _orderBookService = orderBookService;
            _hedgerService = hedgerService;
        }

        [HttpGet]
        [Route("get-best-price")]
        public OrdersResult Get([FromQuery]decimal amount, [FromQuery]OrderType type)
        {
            var orderBooks = _orderBookService.ListOrderBooks();

            if (type == OrderType.Buy)
                return _hedgerService.GetTheBestPriceForBuyer(amount, orderBooks);
            else
                return _hedgerService.GetTheBestPriceForSeller(amount, orderBooks);
        }
    }
}
