using OrderService.Application.Abstractions;
using OrderService.Application.Mediator;
using System.ComponentModel.DataAnnotations;

namespace OrderService.Application.Commands.AcceptOrRejectOrder
{
    public class AcceptOrRejectOrderCommand : IRequest<Result<Guid>>
    {
        public AcceptOrRejectOrderCommand(Guid orderId, bool isAccepted)
        {
            OrderId = orderId;
            IsAccepted = isAccepted;
        }

        [Required]
        public Guid OrderId { get; set; }
        
        [Required]
        public bool IsAccepted { get; set; }
        
    }
}
