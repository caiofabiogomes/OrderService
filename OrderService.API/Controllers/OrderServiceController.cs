using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Commands.PlaceOrder;
using OrderService.Application.Mediator;

namespace OrderService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderServiceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderServiceController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("PlaceOrder")]
        public async Task<IActionResult> PlaceOrder(PlaceOrderCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
