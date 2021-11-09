using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Kafka;
using StockDetails.API.Logging;

namespace StockDetails.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Kafka(bootstrapServers: "localhost:9092", topic: "stocks-details", securityProtocol: Confluent.Kafka.SecurityProtocol.Plaintext)
                //.WriteTo.File(@"E:\\Estockmarket_stockdetails_api.txt")
                .Enrich.StockDetailsLogEnricher()
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
