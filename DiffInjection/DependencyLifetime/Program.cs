using Microsoft.Extensions.DependencyInjection;
using System;
using System.Runtime.CompilerServices;
using System.Security.Authentication.ExtendedProtection;

namespace DependencyLifetime
{
    public class MyService : IDisposable
    {
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~MyService()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            Console.WriteLine("object dispose.");
            GC.SuppressFinalize(this);
        }
    }

    public class MyServiceHold
    {
        MyService service;
        public MyServiceHold(MyService service)
        {
            this.service = service;
        }
    }

    class Program
    {
        static ServiceProvider provider;
        static void CompositeRoot()
        {
            var sc = new ServiceCollection();
            sc.AddTransient<MyService>();
            //sc.AddScoped<MyService>();
            //sc.AddSingleton<MyService>();
            sc.AddScoped<MyServiceHold>();
            provider = sc.BuildServiceProvider();
        }

        static void CompositeRootWithLifetime()
        {
            var sc = new ServiceCollection();
            sc.AddTransient<MyService>();
            //sc.AddScoped<MyService>();
            //sc.AddSingleton<MyService>();

            // CreateInstance will not put the instance into life tracking.
            sc.AddScoped<MyServiceHold>(c => ActivatorUtilities.CreateInstance<MyServiceHold>(c, ActivatorUtilities.CreateInstance<MyService>(c)));
            // use GetServiceOrCreateInstance if you want the instance include of life tracking when get from container.
            //sc.AddScoped<MyServiceHold>(c => ActivatorUtilities.CreateInstance<MyServiceHold>(c, ActivatorUtilities.GetServiceOrCreateInstance<MyService>(c)));
            provider = sc.BuildServiceProvider();
        }

        static void TestScopeWithTransientWrong()
        {
            using (var scope = provider.CreateScope())
            {
                //it's use outside provider, not scope provider.
                var obj = provider.GetRequiredService<MyServiceHold>();
            }
        }

        static void TestScopeWithTransient()
        {
            using(var scope = provider.CreateScope())
            {
                //the MyService will dispose when register with Transient or Scope but not Singleton.
                var obj = scope.ServiceProvider.GetRequiredService<MyServiceHold>();
            }
        }

        static void Main(string[] args)
        {
            CompositeRootWithLifetime();
            TestScopeWithTransient();
            Console.ReadLine();
        }
    }
}
