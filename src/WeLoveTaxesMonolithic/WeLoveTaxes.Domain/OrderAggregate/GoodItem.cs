using System;
using WeLoveTaxes.Domain.Exception;
using WeLoveTaxes.Domain.Seed;

namespace WeLoveTaxes.Domain.OrderAggregate
{
    public sealed class GoodItem : BaseEntity
    {
        private const decimal SalesTax = 0.1M;
        private const decimal ImportTax = 0.05M;

        private decimal _discountRate;
        private int _units;
        private decimal _unitPrice;
        private readonly bool _isImported;
        private readonly bool _isTaxExempt;
        private readonly int _goodItemType;
        public  GoodItemDetails Details { get; private set; }


        internal GoodItem() { }

        public GoodItem( decimal unitPrice, int units, bool isImported, GoodItemTypeEnum goodItemType, decimal? discountRate, GoodItemDetails details, Guid? id = null)
        {
            //set units
            _units = 0;
            AddUnits(units);
            //set unit price
            SetUnitPrice(unitPrice);

            Id = id ?? new Guid();
            _isImported = isImported;
            _isTaxExempt = goodItemType.IsTaxExempt;
            _discountRate = discountRate ?? 0;
            _goodItemType = goodItemType.Id;

            Details = details;
        }

        public GoodItem(GoodItem item)
        {
            //set units
            _units = 0;
            AddUnits(item.GetUnits());
            //set unit price
            SetUnitPrice(item.GetUnitPrice());

            Id = item.Id;
            _isImported = item.GetImportStatus();
            _isTaxExempt = item.GetExemptStatus();
            _discountRate = item.GetDiscountRate();
            _goodItemType = item.GetItemType();

            Details = item.Details;
        }

        public int GetItemType()
        {
            return _goodItemType;
        }

        public decimal GetPrice()
        {
            return GetUnitPrice() * GetUnits();
        }

        public int GetUnits()
        {
            return _units;
        }

        public decimal GetUnitPrice()
        {
            return _unitPrice;
        }

        public decimal GetDiscountRate()
        {
            return _discountRate;
        }

        public bool GetImportStatus()
        {
            return _isImported;
        }

        public bool GetExemptStatus()
        {
            return _isTaxExempt;
        }

        //
        public void SetUnitPrice(decimal newPrice)
        {
            if (newPrice <= 0)
            {
                throw new WeLoveTaxesDomainException("Invalid unit price");
            }

            _unitPrice = newPrice;
        }

        public decimal GetTotalPrice()
        {
            //todo: apply discount
            return GetPrice() + GetSalesTax();
        }

        public decimal GetSalesTax()
        {
            decimal taxMultiplier = 0;
            if (!GetExemptStatus())
            {
                taxMultiplier += SalesTax;
            }
            if (GetImportStatus())
            {
                taxMultiplier += ImportTax;
            }
            return RoundUpPrice(GetPrice() * taxMultiplier);
        }

        public void AddUnits(int units)
        {
            if (units <= 0)
            {
                throw new WeLoveTaxesDomainException("Invalid units");
            }

            _units += units;
        }

        public void RemoveUnits(int units)
        {
            if (units <= 0)
            {
                throw new WeLoveTaxesDomainException("Invalid units");
            }

            if (_units - units < 0)
            {
                throw new WeLoveTaxesDomainException("Cannot remove additional units");
            }

            _units -= units;
        }

        internal static decimal RoundUpPrice(decimal amount)
        {
            var ceiling = Math.Ceiling(amount * 20);
            if (ceiling == 0)
            {
                return 0;
            }
            return ceiling / 20;
        }
    }
}
