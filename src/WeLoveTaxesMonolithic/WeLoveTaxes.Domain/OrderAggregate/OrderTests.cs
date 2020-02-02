using System;
using System.Collections.Generic;
using System.Linq;
using WeLoveTaxes.Domain.Exception;
using Xunit;

namespace WeLoveTaxes.Domain.OrderAggregate
{
    public class OrderTests
    {
        private readonly GoodItem _good1;
        private readonly GoodItem _good2;
        private readonly GoodItem _good3;
        private readonly GoodItem _good4;
        private readonly List<GoodItem> _genericGoodItems;
        public OrderTests()
        {
            _good1 = new GoodItem(15.8M, 1, true, GoodItemTypeEnum.Book, null, new GoodItemDetails(), Guid.NewGuid());
            _good2 = new GoodItem(1.783M, 2, false, GoodItemTypeEnum.MedicalProduct, null, new GoodItemDetails(), Guid.NewGuid());
            _good3 = new GoodItem(33.75M, 3, true, GoodItemTypeEnum.Other, null, new GoodItemDetails(), Guid.NewGuid());
            _good4 = new GoodItem(4.18M, 4, false, GoodItemTypeEnum.Other, null, new GoodItemDetails(), Guid.NewGuid());
            _genericGoodItems = new List<GoodItem> { _good1, _good2, _good3, _good4 };
        }

        [Fact]
        public void GetTotalPriceSumsAllGoodPrices()
        {
            //arrange
            var expectedTotalPrice = _good1.GetTotalPrice() + _good2.GetTotalPrice() + _good3.GetTotalPrice() + _good4.GetTotalPrice();
            //act
            var sut = new Order(_genericGoodItems);
            //assert
            Assert.Equal(expectedTotalPrice, sut.GetTotalPrice());
        }

        [Fact]
        public void GetTotalSalesTaxSumsAllGoodSalesTax()
        {
            //arrange
            var expectedTotalSalesTax = _good1.GetSalesTax() + _good2.GetSalesTax() + _good3.GetSalesTax() + _good4.GetSalesTax();
            //act
            var sut = new Order(_genericGoodItems);
            //assert
            Assert.Equal(expectedTotalSalesTax, sut.GetTotalSalesTax());
        }

        [Fact]
        public void AddGoodItemAddsNewItem()
        {
            //arrange
            var newGoodItem = new GoodItem(9.99M, 5, false, GoodItemTypeEnum.Other, null, new GoodItemDetails("new item"), Guid.NewGuid());
            var sut = new Order(_genericGoodItems);
            //act
            sut.AddGoodItem(newGoodItem);
            //assert
            Assert.Contains(newGoodItem, sut.GoodItems);
        }

        [Fact]
        public void AddGoodItemAddsUnitsExistingItem()
        {
            //arrange
            var good2Duplicate = new GoodItem(_good2);
            var sut = new Order(_genericGoodItems);
            var initialGoodItemCount = sut.GoodItems.Count;
            //act
            sut.AddGoodItem(good2Duplicate);
            var units = sut.GoodItems.FirstOrDefault(x => x.Equals(good2Duplicate))?.GetUnits();
            //assert
            Assert.Equal(4, units);
            Assert.Equal(initialGoodItemCount, sut.GoodItems.Count);
        }

        [Fact]
        public void RemoveGoodItemRemovesExistingItem()
        {
            //arrange
            var sut = new Order(_genericGoodItems);
            //act
            sut.RemoveGoodItem(_good2.Id, 1);
            var goodAfterUnitRemoval = sut.GoodItems.FirstOrDefault(x => x.Id == _good2.Id)?.GetUnits();
            //assert
            Assert.Equal(1, goodAfterUnitRemoval);
        }

        [Fact]
        public void RemoveGoodItemThrowsExceptionForNonExistentItem()
        {
            //arrange
            var sut = new Order(_genericGoodItems);
            //act
            //assert
            System.Exception ex = Assert.Throws<WeLoveTaxesDomainException>(() => sut.RemoveGoodItem(Guid.Empty));
        }

        [Fact]
        public void RemoveGoodItemRemovesItemIfUnitsReachZero()
        {
            //arrange
            var sut = new Order(_genericGoodItems);
            var initialGoodItemCount = sut.GoodItems.Count;
            //act
            sut.RemoveGoodItem(_good1.Id, 1);
            var goodAfterUnitRemoval = sut.GoodItems.FirstOrDefault(x => x.Id == _good1.Id);
            //assert
            Assert.Null(goodAfterUnitRemoval);
            Assert.Equal(initialGoodItemCount-1, sut.GoodItems.Count);
        }
    }
}
