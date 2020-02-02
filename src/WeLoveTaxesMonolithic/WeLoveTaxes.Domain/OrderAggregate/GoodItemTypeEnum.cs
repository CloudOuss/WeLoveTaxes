
using System;
using System.Collections.Generic;
using System.Linq;
using WeLoveTaxes.Domain.Exception;
using WeLoveTaxes.Domain.Seed;

namespace WeLoveTaxes.Domain.OrderAggregate
{
    public class GoodItemTypeEnum : Enumeration
    {
        public bool IsTaxExempt { get; protected set; }


        public static GoodItemTypeEnum Book = new GoodItemTypeEnum(1, nameof(Book).ToLowerInvariant(), true);
        public static GoodItemTypeEnum Food = new GoodItemTypeEnum(2, nameof(Food).ToLowerInvariant(), true);
        public static GoodItemTypeEnum MedicalProduct = new GoodItemTypeEnum(3, nameof(MedicalProduct).ToLowerInvariant(), true);
        public static GoodItemTypeEnum Other = new GoodItemTypeEnum(5, nameof(Other).ToLowerInvariant(), false);

        protected GoodItemTypeEnum()
        {
        }

        public GoodItemTypeEnum(int id, string name, bool isExempt)
            : base(id, name)
        {
            IsTaxExempt = isExempt;
        }

        public static IEnumerable<GoodItemTypeEnum> List() =>
            new[] { Book, Food, MedicalProduct, Other };

        public static GoodItemTypeEnum FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new WeLoveTaxesDomainException($"Possible values for GoodItemType: {string.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static GoodItemTypeEnum From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new WeLoveTaxesDomainException($"Possible values for GoodItemType: {string.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}