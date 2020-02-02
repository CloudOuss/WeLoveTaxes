using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using WeLoveTaxes.Application.Order;
using WeLoveTaxes.Application.Order.Model;

namespace WeLoveTaxes.ConsoleApp
{
    class Program
    {
        private static IServiceProvider _serviceProvider;
        static void Main(string[] args)
        {
            RegisterServices();

            var input1 = new OrderDto
            {
                OrderItems = new List<OrderItemDto>
                {
                    new OrderItemDto {Quantity = 1, Name = "book", Type = "book", IsImported = false, UnitPrice = 12.49M},
                    new OrderItemDto {Quantity = 1, Name = "music CD", Type = "other", IsImported = false, UnitPrice = 14.99M},
                    new OrderItemDto {Quantity = 1, Name = "chocolate bar", Type = "food", IsImported = false, UnitPrice = 0.85M}
                }
            };
            var input2 = new OrderDto
            {
                OrderItems = new List<OrderItemDto>
                {
                    new OrderItemDto {Quantity = 1, Name = "box of chocolates", Type = "food", IsImported = true, UnitPrice = 10.00M},
                    new OrderItemDto {Quantity = 1, Name = "bottle of perfume", Type = "other", IsImported = true, UnitPrice = 47.50M}
                }
            };
            var input3 = new OrderDto
            {
                OrderItems = new List<OrderItemDto>
                {
                    new OrderItemDto {Quantity = 1, Name = "bottle of perfume", Type = "other", IsImported = true, UnitPrice = 27.99M},
                    new OrderItemDto {Quantity = 1, Name = "bottle of perfume", Type = "other", IsImported = false, UnitPrice = 18.99M},
                    new OrderItemDto {Quantity = 1, Name = "packet of headache pills", Type = "MedicalProduct", IsImported = false, UnitPrice = 9.75M},
                    new OrderItemDto {Quantity = 1, Name = "box of chocolates", Type = "food", IsImported = true, UnitPrice = 11.25M}
                }
            };
            Console.WriteLine("OUTPUT 1");
            DisplaySalesTax(input1);
            Console.WriteLine("");
            Console.WriteLine("OUTPUT 2");
            DisplaySalesTax(input2);
            Console.WriteLine("");
            Console.WriteLine("OUTPUT 3");
            DisplaySalesTax(input3);

            DisposeServices();
        }
        private static void RegisterServices()
        {
            var collection = new ServiceCollection();
            collection.AddSingleton<IOrderService, OrderService>();


            _serviceProvider = collection.BuildServiceProvider();
        }
        private static void DisposeServices()
        {
            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        private static void DisplaySalesTax(OrderDto order)
        {
            var orderService = _serviceProvider.GetService<IOrderService>();

            order = orderService.GetTotals(order);
            foreach (var orderItem in order.OrderItems)
            {
                Console.WriteLine(orderItem.ToString());
            }
            Console.WriteLine(order.ToString());
        }
    }
}