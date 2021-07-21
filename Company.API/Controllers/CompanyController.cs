﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Company.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    
    public class CompanyController : ControllerBase
    {
        private static int _count = 0;
        private static readonly string[] Movies = new[]
        {
            "Die Another Day", "Top Gun", "Grease", "Dil Bechara", "Jurassic Park"
        };

        private readonly ILogger<CompanyController> _logger;

        public CompanyController(ILogger<CompanyController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Get()
        {
            _count++;
            Console.WriteLine($"get...{_count}");
            Console.WriteLine(string.Join(":", Request.Headers.Keys));
            if (_count <= 5)
            {
                Thread.Sleep(5000);
            }
            var rng = new Random();

            return Ok(Movies[rng.Next(Movies.Length)]);
        }
    }
}
