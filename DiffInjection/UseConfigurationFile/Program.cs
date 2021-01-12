using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ninject.Activation;
using System;
using System.IO;

namespace UseConfigurationFile
{
    public interface IService
    {

    }

    public class Service:IService
    {

    }

    class Program
    {
        static ServiceProvider provider;
        static void CompositeRoot()
        {
            var configBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            IConfiguration config = configBuilder.Build();
            var sc = new ServiceCollection();
            var servicesConfig = config.GetSection("Services");
            foreach(var service in servicesConfig.GetChildren())
            {
                sc.AddSingleton(Type.GetType(service.Key), Type.GetType(service.Value));
            }
            provider = sc.BuildServiceProvider();
        }

        static void Main(string[] args)
        {
            CompositeRoot();
            var s = provider.GetRequiredService<IService>();
            Console.WriteLine("Hello World!");
        }
    }
}
