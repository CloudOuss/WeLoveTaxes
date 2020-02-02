using WeLoveTaxes.Application.Order.Model;
using WeLoveTaxes.Domain.OrderAggregate;

namespace WeLoveTaxes.Application.Order
{
    public class OrderService : IOrderService
    {
        public OrderDto GetTotals(OrderDto orderDto)
        {
            var order = new Domain.OrderAggregate.Order();
            foreach (var orderItemDto in orderDto.OrderItems)
            {
                var goodItem = new GoodItem(orderItemDto.UnitPrice, orderItemDto.Quantity, orderItemDto.IsImported, GoodItemTypeEnum.FromName(orderItemDto.Type), null, new GoodItemDetails(orderItemDto.Name));
                order.AddGoodItem(goodItem);
            }

            //todo: use object-object mapper instead
            return new OrderDto(order);
        }
    }
}
