
using OrderService.Domain.Exceptions;

namespace OrderService.Domain.ValueObjects
{
    public sealed class OrderStatus : IEquatable<OrderStatus>
    {
        public OrderStatus()
        {
            Status = Enums.OrderStatus.Pending;
            Justification = string.Empty;
        }

        private OrderStatus(Enums.OrderStatus status, string justification)
        {
            Status = status;
            Justification = justification;
        }


        public Enums.OrderStatus Status { get; private set; }

        public string Justification { get; private set; }

        public OrderStatus Accepted()
        {
            var invalidsStatus = new List<Enums.OrderStatus>
            {
                Enums.OrderStatus.Rejected,
                Enums.OrderStatus.Cancelled,
                Enums.OrderStatus.Processing
            };

            if (invalidsStatus.Contains(Status))
                throw new DomainException(string.Format(DomainExceptionMessages.InvalidOrderStatusTransitionToAccepted, Status.ToString()));

            return new OrderStatus(Enums.OrderStatus.Accepted, "");
        }

        public OrderStatus Rejected()
        {
            var invalidsStatus = new List<Enums.OrderStatus>
            {
                Enums.OrderStatus.Accepted,
                Enums.OrderStatus.Cancelled,
                Enums.OrderStatus.Processing
            };

            if (invalidsStatus.Contains(Status))
                throw new DomainException(string.Format(DomainExceptionMessages.InvalidOrderStatusTransitionToRejected, Status.ToString()));

            return new OrderStatus(Enums.OrderStatus.Rejected, "");
        }

        public OrderStatus Processing() 
        {
            var invalidsStatus = new List<Enums.OrderStatus>
            {
                Enums.OrderStatus.Pending,
                Enums.OrderStatus.Rejected,
                Enums.OrderStatus.Cancelled,
                Enums.OrderStatus.Completed
            };

            if (invalidsStatus.Contains(Status))
                throw new DomainException(string.Format(DomainExceptionMessages.InvalidOrderStatusTransitionToProcessing, Status.ToString()));

            return new OrderStatus(Enums.OrderStatus.Processing, ""); 
        }

        public OrderStatus Completed() 
        {
            var invalidsStatus = new List<Enums.OrderStatus>
            {
                Enums.OrderStatus.Pending,
                Enums.OrderStatus.Rejected,
                Enums.OrderStatus.Cancelled,
                Enums.OrderStatus.Accepted
            };
            if (invalidsStatus.Contains(Status))
                throw new DomainException(string.Format(DomainExceptionMessages.InvalidOrderStatusTransitionToCompleted, Status.ToString()));

            return new OrderStatus(Enums.OrderStatus.Completed, "");
        }

        public OrderStatus Cancelled(string justification) 
        {
            var invalidsStatus = new List<Enums.OrderStatus>
            {
                Enums.OrderStatus.Pending,
                Enums.OrderStatus.Rejected,
                Enums.OrderStatus.Processing
            };
            if (invalidsStatus.Contains(Status))
                throw new DomainException(string.Format(DomainExceptionMessages.InvalidOrderStatusTransitionToCancel, Status.ToString()));

            if (string.IsNullOrWhiteSpace(justification))
                throw new DomainException(DomainExceptionMessages.InvalidOrderStatusTransitionToCancelledWithoutJustification);

            return new OrderStatus(Enums.OrderStatus.Cancelled, justification); 
        }

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
