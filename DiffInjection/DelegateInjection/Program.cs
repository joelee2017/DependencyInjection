using Microsoft.Extensions.DependencyInjection;
using System;

namespace DelegateInjection
{
    public interface IService
    {
        Action<string> CallEvent { get; }
    }

    public class Service : IService
    {
        public Action<string> CallEvent { get; private set; }

        public Service(Action<string> callEvent)
        {
            CallEvent = callEvent;
        }
    }


    class Program
    {
        static ServiceProvider provider;
        static void CompositeRoot()
        {
            var sc = new ServiceCollection();
            sc.AddScoped<Action<string>>(c => s => Console.WriteLine(s));
            sc.AddScoped<IService, Service>();
            provider = sc.BuildServiceProvider();
        }
        static void Main(string[] args)
        {
            CompositeRoot();
            var s = provider.GetRequiredService<IService>();
        }
    }
}
