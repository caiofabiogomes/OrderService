using OrderService.Domain.Exceptions;

namespace OrderService.Domain.ValueObjects
{
    public sealed class OrderItemQuantity : IEquatable<OrderItemQuantity>
    {
        public int Value { get; private set; }

        public OrderItemQuantity(int value)
        {
            Validate(value);
            Value = value;
        }

        public override bool Equals(object? obj) => Equals(obj as OrderItemQuantity);

        public bool Equals(OrderItemQuantity? other)
        {
            if (other is null)
                return false;

            return Value == other.Value;
        }

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value.ToString();

        private static void Validate(int value)
        {
            if (value <= 0)
                throw new DomainException(DomainExceptionMessages.InvalidOrderItemQuantity);
        }

    }
}
