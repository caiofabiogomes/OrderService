using OrderService.Domain.ValueObjects;

namespace OrderService.Tests.Domain.ValueObjects
{
    public class OrderTotalAmountTests
    {
        [Fact]
        public void Order_ShouldNotAcceptedNegativeTotalAmount()
        {
            var orderTotalAmount = new OrderTotalAmount(-100);

            Assert.Throws<ArgumentOutOfRangeException>(() => orderTotalAmount.Amount);
        }

        [Fact]
        public void Order_ShouldAcceptedPostiveTotalAmount()
        {
            var orderTotalAmount = new OrderTotalAmount(100);

            Assert.Equal(100, orderTotalAmount.Amount);
        }


        [Fact]
        public void Order_ShouldNotAcceptedZeroTotalAmount()
        {
            var orderTotalAmount = new OrderTotalAmount(0);

            Assert.Throws<ArgumentOutOfRangeException>(() => orderTotalAmount.Amount);
        }
    }
}
