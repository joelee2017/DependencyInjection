using Microsoft.Extensions.DependencyInjection;
using System;

namespace DecoratorInject
{   

    public interface IValueObject
    {

    }

    public class ValueObject : IValueObject
    {

    }

    public class ValueObjectDecorator:IValueObject
    {
        IValueObject obj;
        public ValueObjectDecorator(IValueObject obj)
        {
            this.obj = obj;
        }
    }

    public class TestDecorator
    {
        IValueObject obj;
        public TestDecorator(IValueObject obj)
        {
            this.obj = obj;
        }
    }

    class Program
    {
        static ServiceProvider provider;
        static void CompositionRoot()
        {
            var sc = new ServiceCollection();
            sc.AddTransient<IValueObject, ValueObject>();
            sc.AddTransient<ValueObjectDecorator>();
            //createinstance will create the object direct but fill all paramter from container.
            //GetServiceOrCreateInstance will try to get container service then create instance direct if not found.
            sc.AddSingleton(c =>
              ActivatorUtilities.CreateInstance<TestDecorator>(c, ActivatorUtilities.GetServiceOrCreateInstance<ValueObjectDecorator>(c)));

            provider = sc.BuildServiceProvider();
        }

        static void Main(string[] args)
        {
            CompositionRoot();
            var obj = provider.GetRequiredService<TestDecorator>();
            Console.ReadLine();
        }
    }
}
