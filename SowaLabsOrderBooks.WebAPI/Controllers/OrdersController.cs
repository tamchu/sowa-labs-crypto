using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SowaLabsOrderBooks.HedgerClient;
using SowaLabsOrderBooks.Models;

namespace SowaLabsOrderBooks.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IOrderBookService _orderBookService;
        private readonly IHedgerService _hedgerService;


        public OrdersController(ILogger<OrdersController> logger, IOrderBookService orderBookService, IHedgerService hedgerService)
        {
            _logger = logger;
            _orderBookService = orderBookService;
            _hedgerService = hedgerService;
        }

        [HttpGet]
        [Route("get-best-price")]
        public OrdersResponse Get([FromQuery]decimal amount, [FromQuery]OrderType type)
        {
            var orderBooks = _orderBookService.ListOrderBooks();

            if (type == OrderType.Buy)
                return _hedgerService.GetTheBestPriceForBuyer(amount, orderBooks);
            else
                return _hedgerService.GetTheBestPriceForSeller(amount, orderBooks);
        }
    }
}
