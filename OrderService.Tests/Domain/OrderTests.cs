using OrderService.Domain.Entities;
using OrderService.Domain.Enums;

namespace OrderService.Tests.Domain
{
    public class OrderTests
    {
        [Fact]
        public void Order_ShouldNotAcceptedNegativeTotalAmount() 
        {
            // Arrange
            var order = new Order(Guid.NewGuid(), OrderMode.Counter, -100);
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => order.TotalAmount);
        }

        [Fact]
        public void Order_ShouldAcceptedPostiveTotalAmount()
        {
            // Arrange
            var order = new Order(Guid.NewGuid(), OrderMode.Counter, 100);
            // Act & Assert
            Assert.Equal(100, order.TotalAmount);
        }


        [Fact]
        public void Order_ShouldNotAcceptedZeroTotalAmount()
        {
            // Arrange
            var order = new Order(Guid.NewGuid(), OrderMode.Counter, 0);
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => order.TotalAmount);
        }

        [Fact]
        public void Order_ShouldHaveDefaultStatusPending_WhenCreated()
        {
            // Arrange
            var order = new Order(Guid.NewGuid(), OrderMode.Counter, 100);
            // Act
            var status = order.Status;
            // Assert
            Assert.Equal(OrderStatus.Pending, status);
        }

        [Fact]
        public void Order_ShouldHaveCorrectProperties_WhenCreated()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var mode = OrderMode.DriveThru;
            var totalAmount = 150.00m;
            // Act
            var order = new Order(customerId, mode, totalAmount);
            // Assert
            Assert.Equal(customerId, order.CustomerId);
            Assert.Equal(OrderStatus.Pending, order.Status);
            Assert.Equal(mode, order.Mode);
            Assert.Equal(totalAmount, order.TotalAmount);
        }

        [Fact]
        public void Order_CanTransitionFromPendingToAccepted()
        {
            var order = new Order(Guid.NewGuid(), OrderMode.Delivery, 100);
            order.Status = OrderStatus.Accepted;
            Assert.Equal(OrderStatus.Accepted, order.Status);
        }

        [Fact]
        public void Order_CanTransitionFromAcceptedToProcessing()
        {
            var order = new Order(Guid.NewGuid(), OrderMode.Delivery, DateTime.UtcNow, 100);
            order.SetStatus(OrderStatus.Accepted);
            order.SetStatus(OrderStatus.Processing);
            Assert.Equal(OrderStatus.Processing, order.Status);
        }

        [Fact]
        public void Order_CanTransitionFromProcessingToCompleted()
        {
            var order = new Order(Guid.NewGuid(), OrderMode.Delivery, DateTime.UtcNow, 100);
            order.SetStatus(OrderStatus.Accepted);
            order.SetStatus(OrderStatus.Processing);
            order.SetStatus(OrderStatus.Completed);
            Assert.Equal(OrderStatus.Completed, order.Status);
        }


        [Fact]
        public void Order_CantTransitionFromCompletedToCancelled()
        {
            var order = new Order(Guid.NewGuid(), OrderMode.Delivery, 100);
            order.Status = OrderStatus.Completed;
            Assert.Throws<InvalidOperationException>(() => order.SetStatus(OrderStatus.Cancelled));
        }

        [Fact]
        public void Order_CanBeCancelledWithJustification()
        {
            var order = new Order(Guid.NewGuid(), OrderMode.Counter, DateTime.UtcNow, 100);
            order.Cancel("Changed my mind");
            Assert.Equal(OrderStatus.Cancelled, order.Status);
            Assert.Equal("Changed my mind", order.CancellationReason);
        }

        [Fact]
        public void Order_CantBeCancelledWithoutJustification()
        {
            var order = new Order(Guid.NewGuid(), OrderMode.Counter, 100);
            Assert.Throws<ArgumentException>(() => order.Cancel(""));
        }

        [Fact]
        public void Order_CantBeCancelledWhenProcessingOrCompleted()
        {
            var order = new Order(Guid.NewGuid(), OrderMode.DriveThru, 100);
            order.SetStatus(OrderStatus.Processing);
            Assert.Throws<InvalidOperationException>(() => order.Cancel("Too late"));
        }

        [Fact]
        public void Orders_WithSameCustomerId_ShouldHaveDifferentIds()
        {
            var customerId = Guid.NewGuid();
            var order1 = new Order(customerId, OrderMode.Counter, 100);
            var order2 = new Order(customerId, OrderMode.DriveThru, 200);
            Assert.NotEqual(order1.Id, order2.Id);
        }

        [Fact]
        public void Order_ShouldHaveUniqueId_WhenCreated()
        {
            // Arrange
            var order1 = new Order(Guid.NewGuid(), OrderMode.Counter, 100);
            var order2 = new Order(Guid.NewGuid(), OrderMode.DriveThru, 200);
            // Act
            var id1 = order1.Id;
            var id2 = order2.Id;
            // Assert
            Assert.NotEqual(id1, id2);
        }

        [Fact]
        public void Order_CanBeCancelledByCustomerOnlyBeforeStatusProcessing() 
        {
            var order = new Order(Guid.NewGuid(), OrderMode.DriveThru, 100);

            var order.Status = OrderStatus.Accepted;
            var order.Status = OrderStatus.Cancelled;

            Assert.Throws<InvalidOperationException>(() => order.Status);
        }

        [Fact]
        public void Order_CanBeCancelledByCustomerWhenStatusProcessing()
        {
            var order = new Order(Guid.NewGuid(), OrderMode.DriveThru, 100);

            var order.Status = OrderStatus.Accepted;
            var order.Status = OrderStatus.Cancelled;

            Assert.Equal(OrderStatus.Cancelled, order.Status);
        }

        [Fact]
        public void Order_CantBeCancelledByCustomerWhenStatusProcessing()
        {
            var order = new Order(Guid.NewGuid(), OrderMode.DriveThru, 100);

            var order.Status = OrderStatus.Processing;
            var order.Status = OrderStatus.Cancelled;

            Assert.Throws<InvalidOperationException>(() => order.Status);
        }
    }
}
