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
            var orderItems = new List<OrderItem>();
            orderItems.Add(new OrderItem(
                                new Guid(),
                                new OrderItemQuantity(1),
                                new ItemDescription("test", "test"),
                                new Price(1)
            ));

            var order = new Order(Guid.NewGuid(), OrderMode.Counter, orderItems);

            var status = order.OrderStatus.Status;
            Assert.Equal(OrderService.Domain.Enums.OrderStatus.Pending, status);
        }

        [Fact]
        public void Order_ShouldHaveCorrectProperties_WhenCreated()
        {
            var customerId = Guid.NewGuid();
            var mode = OrderMode.DriveThru;

            var orderItems = new List<OrderItem>();
            orderItems.Add(new OrderItem(
                                new Guid(),
                                new OrderItemQuantity(1),
                                new ItemDescription("test", "test"),
                                new Price(1)
            ));

            var order = new Order(customerId, mode, orderItems);

            Assert.Equal(customerId, order.CustomerId);
            Assert.Equal(OrderService.Domain.Enums.OrderStatus.Pending, order.OrderStatus.Status);
            Assert.Equal(mode, order.Mode);
        }

        [Fact]
        public void Orders_WithSameCustomerId_ShouldHaveDifferentIds()
        {
            var customerId = Guid.NewGuid();

            var orderItems = new List<OrderItem>();
            orderItems.Add(new OrderItem(
                                new Guid(),
                                new OrderItemQuantity(1),
                                new ItemDescription("test", "test"),
                                new Price(1)
            ));

            var order1 = new Order(customerId, OrderMode.Counter, orderItems);
            var order2 = new Order(customerId, OrderMode.DriveThru, orderItems);
            Assert.NotEqual(order1.Id, order2.Id);
        }

        [Fact]
        public void Order_ShouldHaveUniqueId_WhenCreated()
        {
            var orderItems = new List<OrderItem>();
            orderItems.Add(new OrderItem(
                                new Guid(),
                                new OrderItemQuantity(1),
                                new ItemDescription("test", "test"),
                                new Price(1)
            ));

            var order1 = new Order(Guid.NewGuid(), OrderMode.Counter, orderItems);
            var order2 = new Order(Guid.NewGuid(), OrderMode.DriveThru, orderItems);

            var id1 = order1.Id;
            var id2 = order2.Id;

            Assert.NotEqual(id1, id2);
        }

        [Fact]
        public void Order_CanMarkAsAcceptedFromPending()
        {
            var orderItems = new List<OrderItem>();
            orderItems.Add(new OrderItem(
                                new Guid(),
                                new OrderItemQuantity(1),
                                new ItemDescription("test", "test"),
                                new Price(1)
            ));

            var order = new Order(Guid.NewGuid(), OrderMode.Counter, orderItems);

            order.MarkAsAccepted();

            Assert.Equal(OrderService.Domain.Enums.OrderStatus.Accepted, order.OrderStatus.Status);
        }

        [Fact]
        public void Order_CanMarkAsRejectedFromPending()
        {
            var orderItems = new List<OrderItem>();
            orderItems.Add(new OrderItem(
                                new Guid(),
                                new OrderItemQuantity(1),
                                new ItemDescription("test", "test"),
                                new Price(1)
            ));

            var order = new Order(Guid.NewGuid(), OrderMode.Counter, orderItems);

            order.MarkAsRejected();

            Assert.Equal(OrderService.Domain.Enums.OrderStatus.Rejected, order.OrderStatus.Status);
        }

        [Fact]
        public void Order_CantMarkAsCompletedFromPending()
        {
            var orderItems = new List<OrderItem>();
            orderItems.Add(new OrderItem(
                                new Guid(),
                                new OrderItemQuantity(1),
                                new ItemDescription("test", "test"),
                                new Price(1)
            ));

            var order = new Order(Guid.NewGuid(), OrderMode.Counter, orderItems);

            Assert.Throws<DomainException>(() => order.MarkAsCompleted());
        }

        [Fact]
        public void Order_CantMarkAsCancelledWithoutJustification()
        {
            var orderItems = new List<OrderItem>();
            orderItems.Add(new OrderItem(
                                new Guid(),
                                new OrderItemQuantity(1),
                                new ItemDescription("test", "test"),
                                new Price(1)
            ));

            var order = new Order(Guid.NewGuid(), OrderMode.Counter, orderItems);
            order.MarkAsAccepted();

            Assert.Throws<DomainException>(() => order.MarkAsCancelled(null));
            Assert.Throws<DomainException>(() => order.MarkAsCancelled(""));
        }

        [Fact]
        public void Order_CanMarkAsCancelledWithJustificationFromPending()
        {
            var orderItems = new List<OrderItem>();
            orderItems.Add(new OrderItem(
                                new Guid(),
                                new OrderItemQuantity(1),
                                new ItemDescription("test", "test"),
                                new Price(1)
            ));

            var order = new Order(Guid.NewGuid(), OrderMode.Counter, orderItems);
            order.MarkAsAccepted();

            order.MarkAsCancelled("Valid reason");

            Assert.Equal(OrderService.Domain.Enums.OrderStatus.Cancelled, order.OrderStatus.Status);
        }

        [Fact]
        public void Order_CantMarkAsProcessingFromPending()
        {
            var orderItems = new List<OrderItem>();
            orderItems.Add(new OrderItem(
                                new Guid(),
                                new OrderItemQuantity(1),
                                new ItemDescription("test", "test"),
                                new Price(1)
            ));

            var order = new Order(Guid.NewGuid(), OrderMode.Counter, orderItems);

            Assert.Throws<DomainException>(() => order.MarkAsProcessing());
        }

        [Fact]
        public void Order_CanMarkAsProcessingFromAccepted()
        {
            var orderItems = new List<OrderItem>();
            orderItems.Add(new OrderItem(
                                new Guid(),
                                new OrderItemQuantity(1),
                                new ItemDescription("test", "test"),
                                new Price(1)
            ));

            var order = new Order(Guid.NewGuid(), OrderMode.Counter, orderItems);
            order.MarkAsAccepted();

            order.MarkAsProcessing();

            Assert.Equal(OrderService.Domain.Enums.OrderStatus.Processing, order.OrderStatus.Status);
        }

        [Fact]
        public void Order_CantMarkAsCompletedFromAccepted()
        {
            var orderItems = new List<OrderItem>();
            orderItems.Add(new OrderItem(
                                new Guid(),
                                new OrderItemQuantity(1),
                                new ItemDescription("test", "test"),
                                new Price(1)
            ));

            var order = new Order(Guid.NewGuid(), OrderMode.Counter, orderItems);
            order.MarkAsAccepted();

            Assert.Throws<DomainException>(() => order.MarkAsCompleted());
        }

        [Fact]
        public void Order_CanMarkAsCompletedFromProcessing()
        {
            var orderItems = new List<OrderItem>();
            orderItems.Add(new OrderItem(
                                new Guid(),
                                new OrderItemQuantity(1),
                                new ItemDescription("test", "test"),
                                new Price(1)
            ));

            var order = new Order(Guid.NewGuid(), OrderMode.Counter, orderItems);
            order.MarkAsAccepted();
            order.MarkAsProcessing();

            order.MarkAsCompleted();

            Assert.Equal(OrderService.Domain.Enums.OrderStatus.Completed, order.OrderStatus.Status);
        }

        [Fact]
        public void Order_CantChangeStatusAfterRejected()
        {
            var orderItems = new List<OrderItem>();
            orderItems.Add(new OrderItem(
                                new Guid(),
                                new OrderItemQuantity(1),
                                new ItemDescription("test", "test"),
                                new Price(1)
            ));

            var order = new Order(Guid.NewGuid(), OrderMode.Counter, orderItems);
            order.MarkAsRejected();

            Assert.Throws<DomainException>(() => order.MarkAsAccepted());
            Assert.Throws<DomainException>(() => order.MarkAsProcessing());
            Assert.Throws<DomainException>(() => order.MarkAsCompleted());
            Assert.Throws<DomainException>(() => order.MarkAsCancelled("Trying to cancel rejected"));
        }

        [Fact]
        public void Order_CantChangeStatusAfterCancelled()
        {
            var orderItems = new List<OrderItem>();
            orderItems.Add(new OrderItem(
                                new Guid(),
                                new OrderItemQuantity(1),
                                new ItemDescription("test", "test"),
                                new Price(1)
            ));

            var order = new Order(Guid.NewGuid(), OrderMode.Counter, orderItems);
            order.MarkAsAccepted();

            order.MarkAsCancelled("Cancelling order");

            Assert.Throws<DomainException>(() => order.MarkAsAccepted());
            Assert.Throws<DomainException>(() => order.MarkAsProcessing());
            Assert.Throws<DomainException>(() => order.MarkAsCompleted());
            Assert.Throws<DomainException>(() => order.MarkAsRejected());
        }

    }
}
