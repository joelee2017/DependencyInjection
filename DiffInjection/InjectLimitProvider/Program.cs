using Microsoft.Extensions.DependencyInjection;
using System;

namespace InjectLimitProvider
{
    public interface IService1
    {
    }

    public interface IService2
    {
    }

    public class Service1:IService1
    {

    }

    public class Service2:IService2
    {

    }

    public class LimitServiceProvider<T1, T2>
    {
        IServiceProvider provider;
        public T1 GetService1()
        {
            return provider.GetRequiredService<T1>();
        }

        public T2 GetService2()
        {
            return provider.GetRequiredService<T2>();
        }

        public LimitServiceProvider(IServiceProvider provider)
        {
            this.provider = provider;
        }
    }

    public class TestLimit
    {
        LimitServiceProvider<IService1, IService2> provider;
        Lazy<IService1> service1;
        Lazy<IService2> service2;
        public void CallLimit(int type)
        {
            if (type == 1)
            {
                Console.WriteLine(service1.GetType());
            }
            else
            {
                Console.WriteLine(service2.GetType());
            }
        }

        public TestLimit(LimitServiceProvider<IService1, IService2> provider)
        {
            this.provider = provider;
            service1 = new Lazy<IService1>(() => this.provider.GetService1());
            service2 = new Lazy<IService2>(() => this.provider.GetService2());
        }
    }
    class Program
    {
        static IServiceProvider provider;

        static void CompositeRoot()
        {
            var sc = new ServiceCollection();
            sc.AddSingleton<IService1, Service1>();
            sc.AddSingleton<IService2, Service2>();
            sc.AddSingleton(c => new LimitServiceProvider<IService1, IService2>(c));
            sc.AddSingleton<TestLimit>();
            provider = sc.BuildServiceProvider();
        }

        static void Main(string[] args)
        {
            CompositeRoot();
            var obj = provider.GetService<TestLimit>();
            obj.CallLimit(2);
            Console.ReadLine();
        }
    }
}
