
using OrderService.Domain.Enums;
using OrderService.Domain.Exceptions;

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
            
            var exception = Assert.Throws<DomainException>(() => orderStatus.Cancelled("Cancel Order"));

            var expectedMessage = GetExpectedMessage(DomainExceptionMessages.InvalidOrderStatusTransitionToCancel, OrderService.Domain.Enums.OrderStatus.Pending);

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void Order_CantTransitionFromPendingToProcessing()
        {
            var orderStatus = new OrderService.Domain.ValueObjects.OrderStatus();
            
            var exception =  Assert.Throws<DomainException>(() => orderStatus.Processing());

            var expectedMessage = GetExpectedMessage(DomainExceptionMessages.InvalidOrderStatusTransitionToProcessing,
                OrderService.Domain.Enums.OrderStatus.Pending);

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void Order_CantTransitionFromPendingToCompleted()
        {
            var orderStatus = new OrderService.Domain.ValueObjects.OrderStatus();
            
            var exception = Assert.Throws<DomainException>(() => orderStatus.Completed());

            var expectedMessage = GetExpectedMessage(DomainExceptionMessages.InvalidOrderStatusTransitionToCompleted, 
                OrderStatus.Pending);

            Assert.Equal(expectedMessage, exception.Message);
        }

        #endregion

        #region RejectedStatusTests
        [Fact]
        public void Order_CantTransitionFromRejectedToAccepted()
        {
            var orderStatus = new OrderService.Domain.ValueObjects.OrderStatus();
            var rejectedStatus = orderStatus.Rejected();

            var exception = Assert.Throws<DomainException>(() => rejectedStatus.Accepted());

            var expectedMessage = GetExpectedMessage(DomainExceptionMessages.InvalidOrderStatusTransitionToAccepted,
                OrderStatus.Rejected);

            Assert.Equal(expectedMessage, exception.Message);

        }

        [Fact]
        public void Order_CantTransitionFromRejectedToCancelled()
        {
            var orderStatus = new OrderService.Domain.ValueObjects.OrderStatus();
            var rejectedStatus = orderStatus.Rejected();

            var exception = Assert.Throws<DomainException>(() => rejectedStatus.Cancelled("Cannot cancel a rejected order"));

            var expectedMessage = GetExpectedMessage(DomainExceptionMessages.InvalidOrderStatusTransitionToCancel, OrderStatus.Rejected);

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void Order_CantTransitionFromRejectedToProcessing()
        {
            var orderStatus = new OrderService.Domain.ValueObjects.OrderStatus();
            var rejectedStatus = orderStatus.Rejected();

            var exception = Assert.Throws<DomainException>(() => rejectedStatus.Processing());

            var expectedMessage = GetExpectedMessage(DomainExceptionMessages.InvalidOrderStatusTransitionToProcessing, 
                OrderStatus.Rejected);

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void Order_CantTransitionFromRejectedToCompleted()
        {
            var orderStatus = new OrderService.Domain.ValueObjects.OrderStatus();
            var rejectedStatus = orderStatus.Rejected();

            var exception = Assert.Throws<DomainException>(() => rejectedStatus.Completed());

            var expectedMessage = GetExpectedMessage(DomainExceptionMessages.InvalidOrderStatusTransitionToCompleted,
                OrderService.Domain.Enums.OrderStatus.Rejected);

            Assert.Equal(expectedMessage, exception.Message);
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

            var exception = Assert.Throws<DomainException>(() => acceptedStatus.Completed());

            var expectedMessage = string.Format(DomainExceptionMessages.InvalidOrderStatusTransitionToCompleted, 
                OrderService.Domain.Enums.OrderStatus.Accepted.ToString());

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void Order_CantTransitionFromAcceptedToRejected()
        {
            var orderStatus = new OrderService.Domain.ValueObjects.OrderStatus();
            var acceptedStatus = orderStatus.Accepted();

            var exception = Assert.Throws<DomainException>(() => acceptedStatus.Rejected());

            var expectedMessage = GetExpectedMessage(DomainExceptionMessages.InvalidOrderStatusTransitionToRejected, 
                OrderService.Domain.Enums.OrderStatus.Accepted);

            Assert.Equal(expectedMessage, exception.Message);
        }

        #endregion

        #region CancelledStatusTests
        [Fact]
        public void Order_CantTransitionFromCancelledToAccepted()
        {
            var orderStatus = new OrderService.Domain.ValueObjects.OrderStatus();
            var acceptedStatus = orderStatus.Accepted();
            var cancelledStatus = acceptedStatus.Cancelled("Initial cancellation");

            var exception = Assert.Throws<DomainException>(() => cancelledStatus.Accepted());

            var expectedMessage = GetExpectedMessage(DomainExceptionMessages.InvalidOrderStatusTransitionToAccepted,
                OrderService.Domain.Enums.OrderStatus.Cancelled);

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void Order_CantTransitionFromCancelledToProcessing()
        {
            var orderStatus = new OrderService.Domain.ValueObjects.OrderStatus();
            var acceptedStatus = orderStatus.Accepted();
            var cancelledStatus = acceptedStatus.Cancelled("Initial cancellation");

            var exception = Assert.Throws<DomainException>(() => cancelledStatus.Processing());

            var expectedMessage = GetExpectedMessage(DomainExceptionMessages.InvalidOrderStatusTransitionToProcessing,
                OrderService.Domain.Enums.OrderStatus.Cancelled);

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void Order_CantTransitionFromCancelledToCompleted()
        {
            var orderStatus = new OrderService.Domain.ValueObjects.OrderStatus();
            var acceptedStatus = orderStatus.Accepted();
            var cancelledStatus = acceptedStatus.Cancelled("Initial cancellation");

            var exception  = Assert.Throws<DomainException>(() => cancelledStatus.Completed());

            var expectedMessage = GetExpectedMessage(DomainExceptionMessages.InvalidOrderStatusTransitionToCompleted,
                OrderService.Domain.Enums.OrderStatus.Cancelled);

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void Order_CantTransitionFromCancelledToRejected()
        {
            var orderStatus = new OrderService.Domain.ValueObjects.OrderStatus();
            var acceptedStatus = orderStatus.Accepted();
            var cancelledStatus = acceptedStatus.Cancelled("Initial cancellation");

            var exception = Assert.Throws<DomainException>(() => cancelledStatus.Rejected());

            var expectedMessage = GetExpectedMessage(DomainExceptionMessages.InvalidOrderStatusTransitionToRejected,
                OrderService.Domain.Enums.OrderStatus.Cancelled);

            Assert.Equal(expectedMessage, exception.Message);
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

            var exception = Assert.Throws<DomainException>(() => processingStatus.Accepted());

            var expectedMessage = GetExpectedMessage(DomainExceptionMessages.InvalidOrderStatusTransitionToAccepted,
                OrderService.Domain.Enums.OrderStatus.Processing);

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void Order_CantTransitionFromProcessingToCancelled()
        {
            var orderStatus = new OrderService.Domain.ValueObjects.OrderStatus();
            var processingStatus = orderStatus.Accepted().Processing();

            var exception = Assert.Throws<DomainException>(() => processingStatus.Cancelled("Cannot cancel from processing"));

            var expectedMessage = GetExpectedMessage(DomainExceptionMessages.InvalidOrderStatusTransitionToCancel,
                OrderService.Domain.Enums.OrderStatus.Processing);

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void Order_CantTransitionFromProcessingToRejected()
        {
            var orderStatus = new OrderService.Domain.ValueObjects.OrderStatus();
            var processingStatus = orderStatus.Accepted().Processing();

            var exception = Assert.Throws<DomainException>(() => processingStatus.Rejected());

            var expectedMessage = GetExpectedMessage(DomainExceptionMessages.InvalidOrderStatusTransitionToRejected,
                OrderService.Domain.Enums.OrderStatus.Processing);

            Assert.Equal(expectedMessage, exception.Message);
        }

        #endregion

        #region CancelledJustificationTests
        [Fact]
        public void Order_CantTransitionToCancelledWithoutJustification()
        {
            var orderStatus = new OrderService.Domain.ValueObjects.OrderStatus();
            var acceptedStatus = orderStatus.Accepted();

            var exception1 = Assert.Throws<DomainException>(() => acceptedStatus.Cancelled(""));
            var exception2 = Assert.Throws<DomainException>(() => acceptedStatus.Cancelled(null));


            Assert.Equal(DomainExceptionMessages.InvalidOrderStatusTransitionToCancelledWithoutJustification, exception1.Message);
            Assert.Equal(DomainExceptionMessages.InvalidOrderStatusTransitionToCancelledWithoutJustification, exception2.Message);
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

        private static string GetExpectedMessage(string message, OrderStatus status)
        {
            return string.Format(message, status);
        }
    }
}
