using Microsoft.Extensions.DependencyInjection;
using System;

namespace PropertyInjection
{
    public interface IAccessService
    {

    }

    public class AccessService : IAccessService
    {

    }

    public interface IService
    {
        IAccessService AccessServ { get; set; }
    }

    public class Service1 : IService
    {
        public IAccessService AccessServ { get; set; }
    }

    class Program
    {
        IServiceProvider provider;
        void CompositeRoot()
        {
            var sc = new ServiceCollection();
            sc.AddScoped<IAccessService, AccessService>();
            sc.AddScoped<IService>(c =>
            {
                var s = new Service1();
                s.AccessServ = c.GetRequiredService<IAccessService>();
                return s;
            });
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
