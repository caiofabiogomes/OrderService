using OrderService.Domain.Exceptions;

namespace OrderService.Domain.ValueObjects
{
    public sealed class Price : IEquatable<Price>
    {
        public Price(decimal amount)
        {
            Validate(amount);
            Amount = amount;
        }

        public decimal Amount { get; private set; }

        public override bool Equals(object? obj) => Equals(obj as Price);

        public bool Equals(Price? other)
        {
            if (other is null) 
                return false;
            
            return Amount == other.Amount;
        }

        public override int GetHashCode() => Amount.GetHashCode();

        public override string ToString() => Amount.ToString();

        private static void Validate(decimal amount)
        {
            if (amount <= 0)
                throw new DomainException(DomainExceptionMessages.InvalidOrderTotalAmount);
        }
    }
}
