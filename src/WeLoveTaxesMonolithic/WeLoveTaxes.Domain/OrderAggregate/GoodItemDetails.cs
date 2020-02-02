using System.Collections.Generic;
using WeLoveTaxes.Domain.Seed;

namespace WeLoveTaxes.Domain.OrderAggregate
{
    public class GoodItemDetails : ValueObject
    {
        public string Name { get; private set; }

        internal GoodItemDetails() { }

        public GoodItemDetails(string name)
        {
            Name = name;
        }

        public GoodItemDetails(GoodItemDetails details)
        {
            Name = details.Name;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
        }
    }
}
