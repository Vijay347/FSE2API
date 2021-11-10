using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;
using Stock.API.Models;
using System;
using System.Threading.Tasks;

namespace Stock.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/market/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService; // Mongo DB service local
                                                      //private readonly IDynamoDBService _dynamoDbstockService; // AWS Dynamo DB service local

        //private readonly ILogger<StockController> _logger;

        // Mongo DB service local
        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }
        //public StockController(ILogger<StockController> logger, IStockService stockService)
        //{
        //    _stockService = stockService;
        //    _logger = logger;
        //}

        // AWS Dynamo DB service local
        //public StockController(ILogger<StockController> logger, IDynamoDBService dynamoDbstockService)
        //{
        //    _dynamoDbstockService = dynamoDbstockService;
        //    _logger = logger;
        //}

        [HttpPost("AddCompanyStock")]
        [ProducesResponseType(typeof(Stocks), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Stocks>> PostCompanyStock([FromBody] StockAddVM input)
        {
            Log.Information("Start calling PostCompanyStock function");
            Stocks stocks = null;

            try
            {
                stocks = await _stockService.Add(input);
            }
            catch (Exception ex)
            {
                Log.Error("There is an exception", ex);
                throw;
            }
            Log.Information("End calling PostCompanyStock function");
            return stocks;
        }

        [HttpDelete("DeleteCompanyStocks/{code}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult<string>> DeleteCompanyStocks([FromRoute] string code)
        {
            Log.Information("Start calling DeleteCompanyStocks function");
            string result = null;
            try
            {
                result = await _stockService.DeleteStocks(code);
            }
            catch (Exception ex)
            {
                Log.Error("There is an exception", ex);
                throw;
            }
            Log.Information("End calling DeleteCompanyStocks function");
            return result;
        }

        //[HttpPost("AddCompanyStock")]
        //[ProducesResponseType(typeof(DynamoDBStocks), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<ActionResult<DynamoDBStocks>> PostCompanyStock([FromBody] StockAddVM input)
        //{
        //    _logger.LogInformation("Start calling PostCompanyStock function");
        //    DynamoDBStocks stocks = null;

        //    try
        //    {
        //        stocks = await _dynamoDbstockService.Add(input);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("There is an exception", ex);
        //        throw;
        //    }
        //    _logger.LogInformation("End calling PostCompanyStock function");
        //    return stocks;
        //}

        //[HttpDelete("DeleteCompanyStocks/{code}")]
        //[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        //public async Task<ActionResult<string>> DeleteCompanyStocks([FromRoute] string code)
        //{
        //    _logger.LogInformation("Start calling DeleteCompanyStocks function");
        //    string result = null;
        //    try
        //    {
        //        result = await _dynamoDbstockService.DeleteStocks(code);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("There is an exception", ex);
        //        throw;
        //    }
        //    _logger.LogInformation("End calling DeleteCompanyStocks function");
        //    return result;
        //}
    }
}
