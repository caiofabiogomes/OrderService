using OrderService.Domain.Exceptions;
using OrderService.Domain.ValueObjects;

namespace OrderService.Tests.Domain.ValueObjects
{
    public class PriceTests
    {
        [Fact]
        public void Order_ShouldNotAcceptedNegativeTotalAmount()
        {
            decimal invalidAmount = -100;

            var exception = Assert.Throws<DomainException>(() => new Price(invalidAmount));

            Assert.Equal(DomainExceptionMessages.InvalidOrderTotalAmount, exception.Message);
        }

        [Fact]
        public void Order_ShouldAcceptedPostiveTotalAmount()
        {
            var orderTotalAmount = new Price(100);

            Assert.Equal(100, orderTotalAmount.Amount);
        }


        [Fact]
        public void Order_ShouldNotAcceptedZeroTotalAmount()
        {
            decimal invalidAmount = 0;

            var exception = Assert.Throws<DomainException>(() => new Price(invalidAmount));

            Assert.Equal(DomainExceptionMessages.InvalidOrderTotalAmount, exception.Message);
        }
    }
}
