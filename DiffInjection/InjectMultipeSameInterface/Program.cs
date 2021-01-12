using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace InjectMultipeSameInterface
{
    public interface IService
    {

    }
    public class CustomerService : IService
    {

    }

    public class OrderService : IService
    {

    }

    public class TestMultiple
    {
        IService orderService;
        IService customerService;
        public TestMultiple(IService orderService, IService customerService)
        {
            this.orderService = orderService;
            this.customerService = customerService; 
        }
    }
    class Program
    {
        static ServiceProvider provider;
        static void CompositionRoot()
        {
            var sc = new ServiceCollection();
            //you can use Assembly.Load to load external dll.
            sc.AddSingleton<TestMultiple>(c =>
                ActivatorUtilities.CreateInstance<TestMultiple>(c, new object[]
                {
                    ActivatorUtilities.CreateInstance<OrderService>(c),
                    ActivatorUtilities.CreateInstance<CustomerService>(c)
                }));
            provider = sc.BuildServiceProvider();
        }

        static void Main(string[] args)
        {
            CompositionRoot();
            var obj = provider.GetRequiredService<TestMultiple>();
            Console.WriteLine("Hello World!");
        }
    }
}
