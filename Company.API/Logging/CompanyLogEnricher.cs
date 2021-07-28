using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace Company.API.Logging
{
    public interface ICompanyLogEnricher
    {
        string CorrelationId { get; set; }
    }

    public static class LoggingExtensions
    {
        public static LoggerConfiguration CompanyLogEnricher(
            this LoggerEnrichmentConfiguration enrich)
        {
            if (enrich == null)
                throw new ArgumentNullException(nameof(enrich));

            return enrich.With<CompanyLogEnricher>();
        }
    }

    public class CompanyLogEnricher : ILogEventEnricher, ICompanyLogEnricher
    {
        public static IServiceProvider ServiceProvider { get; set; }
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if ((!(ServiceProvider?.GetService<IHttpContextAccessor>()?.HttpContext is HttpContext httpContext)))
                return;
            var headers = httpContext.Request.Headers;
            CorrelationId = headers["CorrelationId"];

            logEvent.AddOrUpdateProperty(new LogEventProperty("CorrelationId", new ScalarValue(CorrelationId)));
        }

        public string CorrelationId { get; set; }
    }
}
