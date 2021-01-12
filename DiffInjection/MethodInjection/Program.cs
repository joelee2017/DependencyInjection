using Microsoft.Extensions.DependencyInjection;
using System;

namespace MethodInjection
{
    public interface IHello
    {
        void SayHello();
    }

    public interface IService
    {
        void CallDependency();
    }

    public class Service : IService
    {
        IHello helloservice;
        public void SetCallDependency(IHello helloService)
        {
            this.helloservice = helloService;
        }

        public void CallDependency()
        {
            if (helloservice == null)
                throw new ArgumentException("hello service not available.");
            helloservice.SayHello();
        }
    }

    public class Hello : IHello
    {
        public void SayHello()
        {
            Console.WriteLine("hello");
        }
    }

    class Program
    {
        ServiceProvider provider;
        void CompositeRoot()
        {
            var sc = new ServiceCollection();
            sc.AddScoped<IHello, Hello>();
            sc.AddScoped<IService>(c =>
            {
                var s = new Service();
                s.SetCallDependency(c.GetRequiredService<IHello>());
                return s;
            });
            this.provider = sc.BuildServiceProvider();

        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
