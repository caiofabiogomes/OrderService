
namespace OrderService.Domain.ValueObjects
{
    public sealed class OrderStatus : IEquatable<OrderStatus>
    {
        public OrderStatus()
        {
            Status = Enums.OrderStatus.Pending;
        }

        private OrderStatus(Enums.OrderStatus status, string justification)
        {
            Status = status;
            Justification = justification;
        }


        public Enums.OrderStatus Status { get; private set; }

        public string Justification { get; private set; }

        public OrderStatus Accepted() => new OrderStatus(Enums.OrderStatus.Accepted, "");
        public OrderStatus Rejected() => new OrderStatus(Enums.OrderStatus.Rejected, "");
        public OrderStatus Processing() => new OrderStatus(Enums.OrderStatus.Processing, "");
        public OrderStatus Completed() => new OrderStatus(Enums.OrderStatus.Completed, "");
        public OrderStatus Cancelled(string justification) => new OrderStatus(Enums.OrderStatus.Cancelled, justification);

        public override bool Equals(object? obj) => Equals(obj as OrderStatus);

        public bool Equals(OrderStatus? other)
        {
            if (other is null)
                return false;

            return Status == other.Status;
        }

        public override int GetHashCode() => Status.GetHashCode();

        public override string ToString() => Status.ToString();
    }
}
