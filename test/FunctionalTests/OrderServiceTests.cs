using System;
using System.Collections.Generic;
using System.Linq;
using WeLoveTaxes.Application.Order;
using WeLoveTaxes.Application.Order.Model;
using WeLoveTaxes.Domain.Exception;
using Xunit;

namespace FunctionalTests
{
    public class OrderServiceTests
    {
        private readonly OrderService _orderService;
        public OrderServiceTests()
        {
            _orderService = new OrderService();
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void GetTotalsReturnsCorrectOutput1()
        {
            //arrange
            var input1 = new OrderDto
            {
                OrderItems = new List<OrderItemDto>
                {
                    new OrderItemDto {Quantity = 1, Name = "book", Type = "book", IsImported = false, UnitPrice = 12.49M},
                    new OrderItemDto {Quantity = 1, Name = "music CD", Type = "other", IsImported = false, UnitPrice = 14.99M},
                    new OrderItemDto {Quantity = 1, Name = "chocolate bar", Type = "food", IsImported = false, UnitPrice = 0.85M}
                }
            };
            //act
            var output = _orderService.GetTotals(input1);
            //assert
            Assert.Equal(12.49M, output.OrderItems.First().UnitPrice);
            Assert.Equal(16.49M, output.OrderItems.ElementAt(1).UnitPrice);
            Assert.Equal(0.85M, output.OrderItems.Last().UnitPrice);

            Assert.Equal("$1.50", output.TotalSalesTax);
            Assert.Equal("$29.83", output.TotalPrice);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void GetTotalsReturnsCorrectOutput2()
        {
            //arrange
            var input2 = new OrderDto
            {
                OrderItems = new List<OrderItemDto>
                {
                    new OrderItemDto {Quantity = 1, Name = "box of chocolates", Type = "food", IsImported = true, UnitPrice = 10.00M},
                    new OrderItemDto {Quantity = 1, Name = "bottle of perfume", Type = "other", IsImported = true, UnitPrice = 47.50M}
                }
            };
            //act
            var output = _orderService.GetTotals(input2);
            //assert
            Assert.Equal(10.50M, output.OrderItems.First().UnitPrice);
            Assert.Equal(54.65M, output.OrderItems.Last().UnitPrice);

            Assert.Equal("$7.65", output.TotalSalesTax);
            Assert.Equal("$65.15", output.TotalPrice);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void GetTotalsReturnsCorrectOutput3()
        {
            //arrange
            var input3 = new OrderDto
            {
                OrderItems = new List<OrderItemDto>
                {
                    new OrderItemDto {Quantity = 1, Name = "bottle of perfume", Type = "other", IsImported = true, UnitPrice = 27.99M},
                    new OrderItemDto {Quantity = 1, Name = "bottle of perfume", Type = "other", IsImported = false, UnitPrice = 18.99M},
                    new OrderItemDto {Quantity = 1, Name = "packet of headache pills", Type = "MedicalProduct", IsImported = false, UnitPrice = 9.75M},
                    new OrderItemDto {Quantity = 1, Name = "box of chocolates", Type = "food", IsImported = true, UnitPrice = 11.25M}
                }
            };
            //act
            var output = _orderService.GetTotals(input3);
            //assert
            Assert.Equal(32.19M, output.OrderItems.First().UnitPrice);
            Assert.Equal(20.89M, output.OrderItems.ElementAt(1).UnitPrice);
            Assert.Equal(9.75M, output.OrderItems.ElementAt(2).UnitPrice);
            Assert.Equal(11.85M, output.OrderItems.Last().UnitPrice);

            Assert.Equal("$6.70", output.TotalSalesTax);
            Assert.Equal("$74.68", output.TotalPrice);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void GetTotalsThrowsExceptionOnBadInput()
        {
            //arrange
            var input3 = new OrderDto
            {
                OrderItems = new List<OrderItemDto>
                {
                    new OrderItemDto {Quantity = 1, Name = "bottle of perfume", Type = "badInput", IsImported = true, UnitPrice = 27.99M}
                }
            };
            //act
            //assert
            Exception ex = Assert.Throws<WeLoveTaxesDomainException>(() => _orderService.GetTotals(input3));
            Assert.Contains("Possible values for GoodItemType", ex.Message);
        }
    }
}
