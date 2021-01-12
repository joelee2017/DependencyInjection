using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Linq;

namespace ScanRegisterByInterface
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

    class Program
    {
        static ServiceProvider provider;
        static void CompositionRoot()
        {
            var sc = new ServiceCollection();
            //you can use Assembly.Load to load external dll.
            Assembly.GetExecutingAssembly().GetTypes().Where(a =>
                 !a.IsGenericType &&
                 !a.IsInterface &&
                 !a.IsAbstract &&
                 a.GetInterfaces().Any(c => c ==  typeof(IService))).ToList().ForEach(t =>
                 sc.AddSingleton(t));
            provider = sc.BuildServiceProvider();
        }

        static void Main(string[] args)
        {
            CompositionRoot();
            var obj = provider.GetRequiredService<OrderService>();
            Console.WriteLine("Hello World!");
        }
    }
}
