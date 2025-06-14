using OrderService.Application.Abstractions;
using OrderService.Application.Mediator;
using OrderService.Domain.Enums;
using OrderService.Domain.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace OrderService.Application.Commands.PlaceOrder
{ 
    public class PlaceOrderCommand : IRequest<Result<Guid>>, IValidatableObject
    {
        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public OrderMode Mode { get; set; }

        [Required]
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        { 
            if (Items.GroupBy(i => i.ProductId).Any(g => g.Count() > 1))
            {
                yield return new ValidationResult(DomainExceptionMessages.InvalidOrderItems, new[] { nameof(Items) });
            }
        }
    }

    public class OrderItemDto
    {
        [Required]
        public Guid ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }

}
