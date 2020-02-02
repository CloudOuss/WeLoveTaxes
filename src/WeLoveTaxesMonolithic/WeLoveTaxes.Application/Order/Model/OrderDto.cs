using System.Collections.Generic;
using WeLoveTaxes.Domain.OrderAggregate;

namespace WeLoveTaxes.Application.Order.Model
{
    public class OrderDto
    {
        public List<OrderItemDto> OrderItems { get; set; }
        public string TotalPrice { get; set; }
        public string TotalSalesTax { get; set; }

        public OrderDto()
        {
            OrderItems = new List<OrderItemDto>();
        }

        public OrderDto(Domain.OrderAggregate.Order order)
        {
            OrderItems = new List<OrderItemDto>();

            foreach (var orderGoodItem in order.GoodItems)
            {
                var orderItemDto = new OrderItemDto
                    {Quantity = orderGoodItem.GetUnits(), Name = orderGoodItem.Details.Name, Type = GoodItemTypeEnum.From(orderGoodItem.GetItemType()).Name, IsImported = orderGoodItem.GetImportStatus(), UnitPrice = orderGoodItem.GetTotalPrice() };
                OrderItems.Add(orderItemDto);
            }

            TotalPrice = order.GetTotalPrice().ToString("C");
            TotalSalesTax = order.GetTotalSalesTax().ToString("C");
        }

        public override string ToString()
        {
            return $"Sales Taxes: {TotalSalesTax} Total: {TotalPrice}";
        }
    }
}
