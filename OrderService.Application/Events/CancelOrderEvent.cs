using MassTransit;
using System.ComponentModel.DataAnnotations;

namespace OrderService.Application.Events
{
    [EntityName("order-cancel-event")]
    public class CancelOrderEvent
    {
        [Required]
        public Guid OrderId { get; set; }

        [Required]
        public string Justification { get; set; }
    }
}
