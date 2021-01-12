using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnumerableInject
{
    public interface IInjection
    { 
    }

    public class Injection : IInjection
    {
    }

    public class Injection1 : IInjection
    {
    }

    public class CollectionInjection
    {

        IEnumerable<IInjection> objs;

        public CollectionInjection(IEnumerable<IInjection> objs)
        {
            this.objs = objs;
        }
    }

    class Program
    {
        static ServiceProvider provider;
        static void CompositionRoot()
        {
            var sc = new ServiceCollection();
            //collection injection.
            sc.AddSingleton<IInjection, Injection>();
            sc.AddSingleton<IInjection, Injection1>();
            sc.AddTransient<CollectionInjection>();
            sc.AddTransient<CollectionInjection>( c =>  
            new CollectionInjection(new List<IInjection>() { 
                new Injection(),
                new Injection()
            }));

            provider = sc.BuildServiceProvider();
        }

        static void Main(string[] args)
        {
            CompositionRoot();
            var cols = provider.GetRequiredService<CollectionInjection>();
            Console.ReadLine();
        }
    }
}
