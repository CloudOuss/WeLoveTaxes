namespace WeLoveTaxes.Application.Order.Model
{
    public class OrderItemDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public bool IsImported { get; set; }

        public override string ToString()
        {
            var name = IsImported ? $"imported {Name}" : Name;
            return $"{Quantity} {name} at {UnitPrice} the unit";
        }
    }
}
