using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OuterAbstract
{
    public interface ITimeProvider
    {
        DateTime Now();
    }

    public class TimerProvider : ITimeProvider
    {
        public DateTime Now()
        {
            return DateTime.Now;
        }
    }

    public interface IApiClientProvider<T>
    {
        Task<T> Get(string url);
    }

    public class ApiClientProvider : IApiClientProvider<string>
    {
        IHttpClientFactory factory;
        public ApiClientProvider(IHttpClientFactory factory)
        {
            this.factory = factory;
        }

        public async Task<string> Get(string url)
        {
            using(var c = factory.CreateClient())
            {
                return await await c.GetAsync(url).ContinueWith(c => c.Result.Content.ReadAsStringAsync());
            }
        }
    }

    public interface IFileProvider
    {
        string Read(string file);
    }

    public class FileProvider : IFileProvider
    {
        public string Read(string file)
        {
            throw new NotImplementedException();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
