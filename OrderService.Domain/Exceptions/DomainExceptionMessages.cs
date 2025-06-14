
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
        public const string InvalidOrderItemQuantity = "Order item quantity must be greater than zero.";
        public const string InvalidOrderItem = "Order item cannot be null.";
        public const string InvalidItemTitle = "Item title cannot be null or empty.";
        public const string InvalidItemDescription = "Item title cannot be null or empty.";
        public const string InvalidOrderItems = "Order must have at least one item";
        public const string DuplicatedOrderItem = "Order cannot have duplicated items.";
    }
}
