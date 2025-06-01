
namespace OrderService.Domain.Exceptions
{
    public static class DomainExceptionMessages
    {
        public const string InvalidOrderTotalAmount = "Order total amount cannot be zero or negative.";
        public const string InvalidOrderStatusTransitionToCancel = "Cannot cancel an order that is {0}.";
        public const string InvalidOrderStatusTransitionToProcessing = "Cannot process an order that is {0}.";
        public const string InvalidOrderStatusTransitionToCompleted = "Cannot complete an order that is {0}.";
        public const string InvalidOrderStatusTransitionToRejected = "Cannot reject an order that is {0}.";
        public const string InvalidOrderStatusTransitionToAccepted = "Cannot accept an order that  is {0}.";
        public const string InvalidOrderStatusTransitionToCancelledWithoutJustification = "Cannot cancel an order that has been accepted without providing a justification.";
    }
}
