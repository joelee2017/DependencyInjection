using Microsoft.Extensions.DependencyInjection;
using System;

namespace GenericInject
{
    public interface IRepository<T>
    {

    }

    public class DataObject
    {

    }

    public class BaseRepository<T> : IRepository<T>
    {
    }

    public class TestGeneric
    {
        IRepository<DataObject> obj;
        public TestGeneric(IRepository<DataObject> obj)
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
            sc.AddSingleton<TestGeneric>();
            sc.AddTransient(typeof(IRepository<>), typeof(BaseRepository<>));
            provider = sc.BuildServiceProvider();
        }

        static void Main(string[] args)
        {
            CompositionRoot();
            var obj = provider.GetRequiredService<TestGeneric>();
        }
    }
}
