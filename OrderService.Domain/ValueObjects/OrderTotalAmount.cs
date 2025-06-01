namespace OrderService.Domain.ValueObjects
{
    public sealed class OrderTotalAmount : IEquatable<OrderTotalAmount>
    {
        public OrderTotalAmount(decimal amount)
        {
            Amount = amount;
        }

        public decimal Amount { get; private set; }

        public override bool Equals(object? obj) => Equals(obj as OrderTotalAmount);

        public bool Equals(OrderTotalAmount? other)
        {
            if (other is null) 
                return false;
            
            return Amount == other.Amount;
        }

        public override int GetHashCode() => Amount.GetHashCode();

        public override string ToString() => Amount.ToString();
    }
}
