using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Commands.CancelOrder;
using OrderService.Application.Commands.PlaceOrder;
using OrderService.Application.Mediator;
using OrderService.Application.Queries.GetOrdersQuery;
using System.Security.Claims;

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
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> PlaceOrder(PlaceOrderCommand command)
        {
            var customerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            command.CustomerId = customerId;

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("CancelOrder")]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> CancelOrder(CancelOrderCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> GetOrders([FromQuery] int page)
        {
            var customerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var query = new GetOrdersQuery() 
            {
                Page = page,
                CustomerId = customerId,
            };

            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
