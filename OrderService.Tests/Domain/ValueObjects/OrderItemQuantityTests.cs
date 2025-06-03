using OrderService.Domain.Exceptions;
using OrderService.Domain.ValueObjects;

namespace OrderService.Tests.Domain.ValueObjects
{
    public class OrderItemQuantityTests
    {
        [Fact]
        public void Constructor_ValidValue_SetsValue()
        {
            var expected = 5;

            var quantity = new OrderItemQuantity(expected);

            Assert.Equal(expected, quantity.Value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        public void Constructor_InvalidValue_ThrowsDomainException(int invalidValue)
        {
            var ex = Assert.Throws<DomainException>(() => new OrderItemQuantity(invalidValue));
            Assert.Equal(DomainExceptionMessages.InvalidOrderItemQuantity, ex.Message);
        }

        [Fact]
        public void Equals_SameValue_ReturnsTrue()
        {
            var q1 = new OrderItemQuantity(10);
            var q2 = new OrderItemQuantity(10);

            Assert.True(q1.Equals(q2));
            Assert.True(q1.Equals((object)q2));
        }

        [Fact]
        public void Equals_DifferentValue_ReturnsFalse()
        {
            var q1 = new OrderItemQuantity(10);
            var q2 = new OrderItemQuantity(20);

            Assert.False(q1.Equals(q2));
        }

        [Fact]
        public void Equals_Null_ReturnsFalse()
        {
            var q1 = new OrderItemQuantity(10);

            Assert.False(q1.Equals(null));
            Assert.False(q1.Equals((object?)null));
        }

        [Fact]
        public void GetHashCode_SameValue_ReturnsSameHashCode()
        {
            var q1 = new OrderItemQuantity(15);
            var q2 = new OrderItemQuantity(15);

            Assert.Equal(q1.GetHashCode(), q2.GetHashCode());
        }

        [Fact]
        public void ToString_ReturnsValueAsString()
        {
            var q = new OrderItemQuantity(7);

            var str = q.ToString();

            Assert.Equal("7", str);
        }
    }

}
