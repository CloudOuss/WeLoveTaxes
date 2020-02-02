using System;
using WeLoveTaxes.Domain.Exception;
using WeLoveTaxes.Domain.Seed;
using Xunit;

namespace WeLoveTaxes.Domain.OrderAggregate
{
    public class GoodItemTests
    {

        [Fact]
        public void CtorAreSynced()
        {
            //arrange
            //act
            var sut1 = new GoodItem(15m, 2, true, GoodItemTypeEnum.Book, null, new GoodItemDetails(), Guid.NewGuid());
            var sut2 = new GoodItem(sut1);
            //assert
            Assert.True(sut1.Equals(sut2));
            Assert.True(sut2.Equals(sut1));
        }

        [Fact]
        public void CtorSetsNullDiscountRateToNull()
        {
            //arrange
            //act
            var sut = new GoodItem(15M, 2, true, GoodItemTypeEnum.Book, null, new GoodItemDetails());
            //assert
            Assert.Equal(0, sut.GetDiscountRate());
        }

        [Theory]
        [InlineData(13.78, 1)]
        [InlineData(2.78, 3)]
        [InlineData(13, 1)]
        [InlineData(13.7804, 1)]
        [InlineData(28, 7)]
        public void GetPriceReturnsUnitMultipliedByUnitPrice(decimal unitPrice, int units)
        {
            //arrange
            //act
            var sut = new GoodItem(unitPrice, units, true, GoodItemTypeEnum.Book, null, new GoodItemDetails());
            //assert
            Assert.Equal(sut.GetUnitPrice() * sut.GetUnits(), sut.GetPrice());
        }

        [Fact]
        public void GetSalesTaxProcessRegularTax()
        {
            //arrange
            var sut = new GoodItem(24, 1, false, GoodItemTypeEnum.Other, null, new GoodItemDetails());

            //act
            var salesTax = sut.GetSalesTax();
            //assert
            Assert.True(2.4M == salesTax); //10%
        }

        [Fact]
        public void GetTotalPriceReturnsSumPriceSalesTax()
        {
            //arrange
            var sut = new GoodItem(24, 1, false, GoodItemTypeEnum.Other, null, new GoodItemDetails());
            var expectedTotalPrice = sut.GetSalesTax() + sut.GetPrice();

            //act
            var actualTotal = sut.GetTotalPrice();
            //assert
            Assert.True(expectedTotalPrice == actualTotal);
        }

        [Fact]
        public void GetSalesTaxProcessImportTax()
        {
            //arrange
            var sut = new GoodItem(24, 1, true, GoodItemTypeEnum.MedicalProduct, null, new GoodItemDetails());

            //act
            var salesTax = sut.GetSalesTax();
            //assert
            Assert.True(1.2M == salesTax); //5%
        }

        [Fact]
        public void GetSalesTaxProcessBothTaxes()
        {
            //arrange
            var sut = new GoodItem(24, 1, true, GoodItemTypeEnum.Other, null, new GoodItemDetails());

            //act
            var salesTax = sut.GetSalesTax();
            //assert
            Assert.True(3.6M == salesTax); //15%
        }

        [Fact]
        public void AddUnitsThrowsExceptionNegativeInput()
        {
            //arrange
            var sut = new GoodItem();
            //act
            //assert
            System.Exception ex = Assert.Throws<WeLoveTaxesDomainException>(() => sut.AddUnits(-5));
        }

        [Fact]
        public void AddUnitsWorksFine()
        {
            //arrange
            var sut = new GoodItem(24, 1, true, GoodItemTypeEnum.Other, null, new GoodItemDetails());
            //act
            sut.AddUnits(7);
            //assert
            Assert.Equal(8, sut.GetUnits());
        }

        [Fact]
        public void RemoveUnitsThrowsExceptionNegativeInput()
        {
            //arrange
            var sut = new GoodItem();
            //act
            //assert
            System.Exception ex = Assert.Throws<WeLoveTaxesDomainException>(() => sut.RemoveUnits(-5));
        }

        [Fact]
        public void RemoveUnitsThrowsExceptionNegativeResult()
        {
            //arrange
            var sut = new GoodItem(24, 1, true, GoodItemTypeEnum.Other, null, new GoodItemDetails());
            //act
            //assert
            System.Exception ex = Assert.Throws<WeLoveTaxesDomainException>(() => sut.RemoveUnits(-2));
        }

        [Fact]
        public void RemoveUnitsWorksFine()
        {
            //arrange
            var sut = new GoodItem(24, 2, true, GoodItemTypeEnum.Other, null, new GoodItemDetails());
            //act
            sut.RemoveUnits(1);
            //assert
            Assert.Equal(1, sut.GetUnits());
        }

        [Theory]

        [InlineData(13.70, 13.70)]
        [InlineData(13.71, 13.75)]
        [InlineData(13.72222, 13.75)]
        [InlineData(13.730, 13.75)]
        [InlineData(13.748952613594, 13.75)]
        [InlineData(13.750000, 13.75)]
        [InlineData(13.751, 13.80)]
        public void RoundUpPriceToUpperFiveCents(decimal amount, decimal expectedAmount)
        {
            //arrange
            //act
            var actual = GoodItem.RoundUpPrice(amount);
            //assert
            Assert.Equal(expectedAmount, actual);
        }

        [Theory]
        [InlineData(-13.70)]
        [InlineData(0)]
        public void SetUnitPriceThrowsExceptionNegativeInput(decimal newPrice)
        {
            //arrange
            var sut = new GoodItem(24, 2, true, GoodItemTypeEnum.Other, null, new GoodItemDetails());
            //act
            //assert
            System.Exception ex = Assert.Throws<WeLoveTaxesDomainException>(() => sut.SetUnitPrice(newPrice));
        }

        [Fact]
        public void SetUnitPriceWorksFine()
        {
            //arrange
            var newPrice = 43.489M;
            var sut = new GoodItem(24, 2, true, GoodItemTypeEnum.Other, null, new GoodItemDetails());
            //act
            sut.SetUnitPrice(newPrice);
            //assert
            Assert.Equal(newPrice, sut.GetUnitPrice());
        }
    }
}
