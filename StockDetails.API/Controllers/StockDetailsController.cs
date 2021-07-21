using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StockDetails.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class StockDetailsController : ControllerBase
    {
        private static int _count = 0;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<StockDetailsController> _logger;

        public StockDetailsController(ILogger<StockDetailsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Get()
        {
            _count++;
            Console.WriteLine($"get...{_count}");
            if (_count <= 5)
            {
                Thread.Sleep(5000);
            }
            var rng = new Random();

            return Ok(Summaries[rng.Next(Summaries.Length)]);
        }
    }
}
