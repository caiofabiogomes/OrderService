
namespace OrderService.Tests.Domain.ValueObjects
{
    public class OrderStatusTests
    {
        #region PendingStatusTests
        [Fact]
        public void Order_CanTransitionFromPendingToAccepted()
        {
            var orderStatus = new OrderService.Domain.ValueObjects.OrderStatus();

            var newOrderStatus = orderStatus.Accepted();

            Assert.Equal(OrderService.Domain.Enums.OrderStatus.Accepted, newOrderStatus.Status);
        }

        [Fact]
        public void Order_CanTransitionFromPendingToRejected()
        {
            var orderStatus = new OrderService.Domain.ValueObjects.OrderStatus();

            var newOrderStatus = orderStatus.Rejected();

            Assert.Equal(OrderService.Domain.Enums.OrderStatus.Rejected, newOrderStatus.Status);
        }

        [Fact]
        public void Order_CantTransitionFromPendingToCancelled()
        {
            var orderStatus = new OrderService.Domain.ValueObjects.OrderStatus();
            Assert.Throws<ArgumentException>(() => orderStatus.Cancelled("Cancel Order"));
        }

        [Fact]
        public void Order_CantTransitionFromPendingToProcessing()
        {
            var orderStatus = new OrderService.Domain.ValueObjects.OrderStatus();
            Assert.Throws<ArgumentException>(() => orderStatus.Processing());
        }

        [Fact]
        public void Order_CantTransitionFromPendingToCompleted()
        {
            var orderStatus = new OrderService.Domain.ValueObjects.OrderStatus();
            Assert.Throws<ArgumentException>(() => orderStatus.Completed());
        }

        #endregion

        #region RejectedStatusTests
        [Fact]
        public void Order_CantTransitionFromRejectedToAccepted()
        {
            var orderStatus = new OrderService.Domain.ValueObjects.OrderStatus();
            var rejectedStatus = orderStatus.Rejected();

            Assert.Throws<InvalidOperationException>(() => rejectedStatus.Accepted());
        }

        [Fact]
        public void Order_CantTransitionFromRejectedToCancelled()
        {
            var orderStatus = new OrderService.Domain.ValueObjects.OrderStatus();
            var rejectedStatus = orderStatus.Rejected();

            Assert.Throws<InvalidOperationException>(() => rejectedStatus.Cancelled("Cannot cancel a rejected order"));
        }

        [Fact]
        public void Order_CantTransitionFromRejectedToProcessing()
        {
            var orderStatus = new OrderService.Domain.ValueObjects.OrderStatus();
            var rejectedStatus = orderStatus.Rejected();

            Assert.Throws<InvalidOperationException>(() => rejectedStatus.Processing());
        }

        [Fact]
        public void Order_CantTransitionFromRejectedToCompleted()
        {
            var orderStatus = new OrderService.Domain.ValueObjects.OrderStatus();
            var rejectedStatus = orderStatus.Rejected();

            Assert.Throws<InvalidOperationException>(() => rejectedStatus.Completed());
        }

        #endregion

        #region AcceptedStatusTests
        [Fact]
        public void Order_CanTransitionFromAcceptedToProcessing()
        {
            var orderStatus = new OrderService.Domain.ValueObjects.OrderStatus();
            var acceptedStatus = orderStatus.Accepted();

            var processingStatus = acceptedStatus.Processing();

            Assert.Equal(OrderService.Domain.Enums.OrderStatus.Processing, processingStatus.Status);
        }

        [Fact]
        public void Order_CanTransitionFromAcceptedToCancelled()
        {
            var orderStatus = new OrderService.Domain.ValueObjects.OrderStatus();
            var acceptedStatus = orderStatus.Accepted();

            var cancelledStatus = acceptedStatus.Cancelled("Cancelling after acceptance");

            Assert.Equal(OrderService.Domain.Enums.OrderStatus.Cancelled, cancelledStatus.Status);
        }

        [Fact]
        public void Order_CantTransitionFromAcceptedToCompleted()
        {
            var orderStatus = new OrderService.Domain.ValueObjects.OrderStatus();
            var acceptedStatus = orderStatus.Accepted();

            Assert.Throws<InvalidOperationException>(() => acceptedStatus.Completed());
        }

        [Fact]
        public void Order_CantTransitionFromAcceptedToRejected()
        {
            var orderStatus = new OrderService.Domain.ValueObjects.OrderStatus();
            var acceptedStatus = orderStatus.Accepted();

            Assert.Throws<InvalidOperationException>(() => acceptedStatus.Rejected());
        }

        #endregion

        #region CancelledStatusTests
        [Fact]
        public void Order_CantTransitionFromCancelledToAccepted()
        {
            var orderStatus = new OrderService.Domain.ValueObjects.OrderStatus();
            var cancelledStatus = orderStatus.Cancelled("Initial cancellation");

            Assert.Throws<InvalidOperationException>(() => cancelledStatus.Accepted());
        }

        [Fact]
        public void Order_CantTransitionFromCancelledToProcessing()
        {
            var orderStatus = new OrderService.Domain.ValueObjects.OrderStatus();
            var cancelledStatus = orderStatus.Cancelled("Initial cancellation");

            Assert.Throws<InvalidOperationException>(() => cancelledStatus.Processing());
        }

        [Fact]
        public void Order_CantTransitionFromCancelledToCompleted()
        {
            var orderStatus = new OrderService.Domain.ValueObjects.OrderStatus();
            var cancelledStatus = orderStatus.Cancelled("Initial cancellation");

            Assert.Throws<InvalidOperationException>(() => cancelledStatus.Completed());
        }

        [Fact]
        public void Order_CantTransitionFromCancelledToRejected()
        {
            var orderStatus = new OrderService.Domain.ValueObjects.OrderStatus();
            var cancelledStatus = orderStatus.Cancelled("Initial cancellation");

            Assert.Throws<InvalidOperationException>(() => cancelledStatus.Rejected());
        }

        #endregion

        #region ProcessingStatusTests
        [Fact]
        public void Order_CanTransitionFromProcessingToCompleted()
        {
            var orderStatus = new OrderService.Domain.ValueObjects.OrderStatus();
            var processingStatus = orderStatus.Accepted().Processing();

            var completedStatus = processingStatus.Completed();

            Assert.Equal(OrderService.Domain.Enums.OrderStatus.Completed, completedStatus.Status);
        }

        [Fact]
        public void Order_CantTransitionFromProcessingToAccepted()
        {
            var orderStatus = new OrderService.Domain.ValueObjects.OrderStatus();
            var processingStatus = orderStatus.Accepted().Processing();

            Assert.Throws<InvalidOperationException>(() => processingStatus.Accepted());
        }

        [Fact]
        public void Order_CantTransitionFromProcessingToCancelled()
        {
            var orderStatus = new OrderService.Domain.ValueObjects.OrderStatus();
            var processingStatus = orderStatus.Accepted().Processing();

            Assert.Throws<InvalidOperationException>(() => processingStatus.Cancelled("Cannot cancel from processing"));
        }

        [Fact]
        public void Order_CantTransitionFromProcessingToRejected()
        {
            var orderStatus = new OrderService.Domain.ValueObjects.OrderStatus();
            var processingStatus = orderStatus.Accepted().Processing();

            Assert.Throws<InvalidOperationException>(() => processingStatus.Rejected());
        }

        #endregion

        #region CancelledJustificationTests
        [Fact]
        public void Order_CantTransitionToCancelledWithoutJustification()
        {
            var orderStatus = new OrderService.Domain.ValueObjects.OrderStatus();
            var acceptedStatus = orderStatus.Accepted();

            // Tenta cancelar sem justificativa (string vazia)
            Assert.Throws<ArgumentException>(() => acceptedStatus.Cancelled(""));

            // Tenta cancelar sem justificativa (null)
            Assert.Throws<ArgumentException>(() => acceptedStatus.Cancelled(null));
        }

        [Fact]
        public void Order_CanTransitionToCancelledWithJustification()
        {
            var orderStatus = new OrderService.Domain.ValueObjects.OrderStatus();
            var acceptedStatus = orderStatus.Accepted();

            var cancelledStatus = acceptedStatus.Cancelled("Valid justification");

            Assert.Equal(OrderService.Domain.Enums.OrderStatus.Cancelled, cancelledStatus.Status);
        }

        #endregion
    }
}
