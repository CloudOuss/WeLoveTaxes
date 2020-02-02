using System;
using System.Collections.Generic;
using System.Linq;
using WeLoveTaxes.Domain.Exception;
using WeLoveTaxes.Domain.Seed;

namespace WeLoveTaxes.Domain.OrderAggregate
{
    public class Order : BaseEntity, IAggregateRoot
    {
        private readonly List<GoodItem> _goodItems;
        public IReadOnlyCollection<GoodItem> GoodItems => _goodItems;

        public Order()
        {
            _goodItems = new List<GoodItem>();
        }

        public Order(List<GoodItem> goodItems)
        {
            _goodItems = goodItems;
        }

        public decimal GetTotalPrice()
        {
            return _goodItems.Sum(x => x.GetTotalPrice());
        }

        public decimal GetTotalSalesTax()
        {
            return _goodItems.Sum(x => x.GetSalesTax());
        }

        public void AddGoodItem(GoodItem goodItem)
        {
            var existingOrderForProduct = _goodItems
                .SingleOrDefault(o => Equals(o, goodItem));

            if (existingOrderForProduct != null)
            {
                existingOrderForProduct.AddUnits(goodItem.GetUnits());
            }
            else
            {
                var orderItem = new GoodItem(goodItem);
                _goodItems.Add(orderItem);
            }
        }

        public void RemoveGoodItem(Guid goodItemId, int units = 1)
        {
            var existingOrderForProduct = _goodItems
                .SingleOrDefault(o => o.Id == goodItemId);

            if (existingOrderForProduct != null)
            {
                existingOrderForProduct.RemoveUnits(units);
                if(existingOrderForProduct.GetUnits() == 0)
                {
                    _goodItems.Remove(existingOrderForProduct);
                }
            }
            else
            {
                throw new WeLoveTaxesDomainException("Invalid goodItem");
            }
        }

    }
}
