using OrderService.Domain.Entities;
using OrderService.Domain.Enums;
using OrderService.Domain.Exceptions;
using OrderService.Domain.ValueObjects;

namespace OrderService.Tests.Domain.Entites
{
    public class OrderTests
    {
        [Fact]
        public void Order_ShouldHaveDefaultStatusPending_WhenCreated()
        {
            var orderTotalAmount = new OrderTotalAmount(100);
            var order = new Order(Guid.NewGuid(), OrderMode.Counter, orderTotalAmount);
            
            var status = order.OrderStatus.Status;
            Assert.Equal(OrderService.Domain.Enums.OrderStatus.Pending, status);
        }

        [Fact]
        public void Order_ShouldHaveCorrectProperties_WhenCreated()
        {
            var customerId = Guid.NewGuid();
            var mode = OrderMode.DriveThru;
            var totalAmount = new OrderTotalAmount(100);
            var order = new Order(customerId, mode, totalAmount);

            Assert.Equal(customerId, order.CustomerId);
            Assert.Equal(OrderService.Domain.Enums.OrderStatus.Pending, order.OrderStatus.Status);
            Assert.Equal(mode, order.Mode);
            Assert.Equal(totalAmount, order.TotalAmount);
        }

        [Fact]
        public void Orders_WithSameCustomerId_ShouldHaveDifferentIds()
        {
            var customerId = Guid.NewGuid();
            var orderTotalAmount1 = new OrderTotalAmount(100);
            var orderTotalAmount2 = new OrderTotalAmount(200);

            var order1 = new Order(customerId, OrderMode.Counter, orderTotalAmount1);
            var order2 = new Order(customerId, OrderMode.DriveThru, orderTotalAmount2);
            Assert.NotEqual(order1.Id, order2.Id);
        }

        [Fact]
        public void Order_ShouldHaveUniqueId_WhenCreated()
        {
            var orderTotalAmount = new OrderTotalAmount(100);

            var order1 = new Order(Guid.NewGuid(), OrderMode.Counter, orderTotalAmount);
            var order2 = new Order(Guid.NewGuid(), OrderMode.DriveThru, orderTotalAmount);

            var id1 = order1.Id;
            var id2 = order2.Id;

            Assert.NotEqual(id1, id2);
        }

        [Fact]
        public void Order_CanMarkAsAcceptedFromPending()
        {
            var orderTotalAmount = new OrderTotalAmount(100);

            var order = new Order(Guid.NewGuid(), OrderMode.Counter, orderTotalAmount);

            order.MarkAsAccepted();

            Assert.Equal(OrderService.Domain.Enums.OrderStatus.Accepted, order.OrderStatus.Status);
        }

        [Fact]
        public void Order_CanMarkAsRejectedFromPending()
        {
            var orderTotalAmount = new OrderTotalAmount(100);

            var order = new Order(Guid.NewGuid(), OrderMode.Counter, orderTotalAmount);

            order.MarkAsRejected();

            Assert.Equal(OrderService.Domain.Enums.OrderStatus.Rejected, order.OrderStatus.Status);
        }

        [Fact]
        public void Order_CantMarkAsCompletedFromPending()
        {
            var orderTotalAmount = new OrderTotalAmount(100);

            var order = new Order(Guid.NewGuid(), OrderMode.Counter, orderTotalAmount);

            Assert.Throws<DomainException>(() => order.MarkAsCompleted());
        }

        [Fact]
        public void Order_CantMarkAsCancelledWithoutJustification()
        {
            var orderTotalAmount = new OrderTotalAmount(100);

            var order = new Order(Guid.NewGuid(), OrderMode.Counter, orderTotalAmount);
            order.MarkAsAccepted();

            Assert.Throws<DomainException>(() => order.MarkAsCancelled(null));
            Assert.Throws<DomainException>(() => order.MarkAsCancelled(""));
        }

        [Fact]
        public void Order_CanMarkAsCancelledWithJustificationFromPending()
        {
            var orderTotalAmount = new OrderTotalAmount(100);

            var order = new Order(Guid.NewGuid(), OrderMode.Counter, orderTotalAmount);
            order.MarkAsAccepted();

            order.MarkAsCancelled("Valid reason");

            Assert.Equal(OrderService.Domain.Enums.OrderStatus.Cancelled, order.OrderStatus.Status);
        }

        [Fact]
        public void Order_CantMarkAsProcessingFromPending()
        {
            var orderTotalAmount = new OrderTotalAmount(100);

            var order = new Order(Guid.NewGuid(), OrderMode.Counter, orderTotalAmount);

            Assert.Throws<DomainException>(() => order.MarkAsProcessing());
        }

        [Fact]
        public void Order_CanMarkAsProcessingFromAccepted()
        {
            var orderTotalAmount = new OrderTotalAmount(100);

            var order = new Order(Guid.NewGuid(), OrderMode.Counter, orderTotalAmount);
            order.MarkAsAccepted();

            order.MarkAsProcessing();

            Assert.Equal(OrderService.Domain.Enums.OrderStatus.Processing, order.OrderStatus.Status);
        }

        [Fact]
        public void Order_CantMarkAsCompletedFromAccepted()
        {
            var orderTotalAmount = new OrderTotalAmount(100);

            var order = new Order(Guid.NewGuid(), OrderMode.Counter, orderTotalAmount);
            order.MarkAsAccepted();

            Assert.Throws<DomainException>(() => order.MarkAsCompleted());
        }

        [Fact]
        public void Order_CanMarkAsCompletedFromProcessing()
        {
            var orderTotalAmount = new OrderTotalAmount(100);

            var order = new Order(Guid.NewGuid(), OrderMode.Counter, orderTotalAmount);
            order.MarkAsAccepted();
            order.MarkAsProcessing();

            order.MarkAsCompleted();

            Assert.Equal(OrderService.Domain.Enums.OrderStatus.Completed, order.OrderStatus.Status);
        }

        [Fact]
        public void Order_CantChangeStatusAfterRejected()
        {
            var orderTotalAmount = new OrderTotalAmount(100);

            var order = new Order(Guid.NewGuid(), OrderMode.Counter, orderTotalAmount);
            order.MarkAsRejected();

            Assert.Throws<DomainException>(() => order.MarkAsAccepted());
            Assert.Throws<DomainException>(() => order.MarkAsProcessing());
            Assert.Throws<DomainException>(() => order.MarkAsCompleted());
            Assert.Throws<DomainException>(() => order.MarkAsCancelled("Trying to cancel rejected"));
        }

        [Fact]
        public void Order_CantChangeStatusAfterCancelled()
        {
            var orderTotalAmount = new OrderTotalAmount(100);

            var order = new Order(Guid.NewGuid(), OrderMode.Counter, orderTotalAmount);
            order.MarkAsAccepted();

            order.MarkAsCancelled("Cancelling order");

            Assert.Throws<DomainException>(() => order.MarkAsAccepted());
            Assert.Throws<DomainException>(() => order.MarkAsProcessing());
            Assert.Throws<DomainException>(() => order.MarkAsCompleted());
            Assert.Throws<DomainException>(() => order.MarkAsRejected());
        }

    }
}
