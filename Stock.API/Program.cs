using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Kafka;
using Stock.API.Logging;

namespace Stock.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Kafka(bootstrapServers: "localhost:9092", topic: "stocks", securityProtocol: Confluent.Kafka.SecurityProtocol.Plaintext)
                .Enrich.StockLogEnricher()
                //.WriteTo.File("E:\\Estockmarket_stock_api.txt")
                .CreateLogger();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
