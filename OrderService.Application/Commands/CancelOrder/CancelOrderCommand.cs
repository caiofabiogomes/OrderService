using OrderService.Application.Abstractions;
using OrderService.Application.Mediator;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OrderService.Application.Commands.CancelOrder
{
    public class CancelOrderCommand : IRequest<Result<Guid>>
    {
        [Required]
        public Guid OrderId { get; set; }

        [Required]
        public string Justification { get; set; }

        [Required]
        [JsonIgnore]
        public Guid CustomerId { get; set; } = Guid.Empty;
    }
}
