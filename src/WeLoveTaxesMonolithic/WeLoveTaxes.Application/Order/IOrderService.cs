using System;
using System.Collections.Generic;
using System.Text;
using WeLoveTaxes.Application.Order.Model;

namespace WeLoveTaxes.Application.Order
{
    public interface IOrderService
    {
        OrderDto GetTotals(OrderDto order);
    }
}
