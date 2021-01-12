using Microsoft.Extensions.DependencyInjection;
using System;

namespace InjectInheritance
{
    public abstract class BaseClass
    {
        public abstract void SayHello();
    }

    public class DerivedClass : BaseClass
    {
        public override void SayHello()
        {
            Console.WriteLine("i am Derived");
        }
    }

    public class ConsumerBaseClass
    {
        BaseClass inner;
        public ConsumerBaseClass(BaseClass inner)
        {
            this.inner = inner;
        }
    }

    public class ConsumerDerivedClass
    {
        BaseClass inner;
        public ConsumerDerivedClass(DerivedClass inner)
        {
            this.inner = inner;
        }        
    }


    class Program
    {
        static ServiceProvider provider;

        static void CompositionRoot()
        {
            var sc = new ServiceCollection();
            //because we are register base class, not derivedclass, so if object require derivedclass, will faill.
            //sc.AddScoped<BaseClass, DerivedClass>();

            sc.AddScoped<DerivedClass>();
            sc.AddScoped<BaseClass>(s => s.GetRequiredService<DerivedClass>());

            sc.AddTransient<ConsumerBaseClass>();
            sc.AddTransient<ConsumerDerivedClass>();
            provider = sc.BuildServiceProvider();
        }

        static void Main(string[] args)
        {
            CompositionRoot();            
            var obj1 = provider.GetRequiredService<ConsumerBaseClass>();
            var obj2 = provider.GetRequiredService<ConsumerDerivedClass>();
            Console.ReadLine();
        }
    }
}
