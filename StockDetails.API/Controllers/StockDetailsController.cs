using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockDetails.API.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockDetails.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/market/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class StockDetailsController : ControllerBase
    {
        //private readonly IStockService _stockService; // Mongo DB service local
        private readonly IDynamoDBService _dynamoDbstockService; // AWS Dynamo DB service local

        private readonly ILogger<StockDetailsController> _logger;

        // Mongo DB service local
        //public StockDetailsController(ILogger<StockDetailsController> logger, IStockService stockService)
        //{
        //    _stockService = stockService;
        //    _logger = logger;
        //}

        // AWS Dynamo DB service local
        public StockDetailsController(ILogger<StockDetailsController> logger, IDynamoDBService dynamoDbstockService)
        {
            _dynamoDbstockService = dynamoDbstockService;
            _logger = logger;
        }

        //[HttpGet("GetAllCompanyStocks")]
        //[ProducesResponseType(typeof(Stocks), StatusCodes.Status200OK)]
        //public async Task<ActionResult<List<Stocks>>> GetAllCompanyStocks()
        //{
        //    _logger.LogInformation("Start calling GetAllCompanyStocks function");
        //    List<Stocks> stocks;
        //    try
        //    {
        //        stocks = await _stockService.Get();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("There is an exception", ex);
        //        throw;
        //    }
        //    _logger.LogInformation("End calling GetAllCompanyStocks function");
        //    return stocks;
        //}

        //[HttpGet("GetCompanyStocksByCode/{code}")]
        //[ProducesResponseType(typeof(Stocks), StatusCodes.Status200OK)]
        //public async Task<ActionResult<List<Stocks>>> GetCompanyStocksByCode([FromRoute] string code)
        //{
        //    _logger.LogInformation("Start calling GetCompanyStocksByCode function");
        //    List<Stocks> stocks;
        //    try
        //    {
        //        stocks = await _stockService.GetStockByCompanyCode(code);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("There is an exception", ex);
        //        throw;
        //    }
        //    _logger.LogInformation("End calling GetCompanyStocksByCode function");
        //    return stocks;
        //}

        //[HttpPost("GetCompanyStocks")]
        //[ProducesResponseType(typeof(Stocks), StatusCodes.Status200OK)]
        //public async Task<ActionResult<List<Stocks>>> GetCompanyStocks([FromBody] StockGetVM input)
        //{
        //    _logger.LogInformation("Start calling GetCompanyStocks function");
        //    List<Stocks> stocks;
        //    try
        //    {
        //        stocks = await _stockService.SearchStocks(input);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("There is an exception", ex);
        //        throw;
        //    }
        //    _logger.LogInformation("End calling GetCompanyStocks function");
        //    return stocks;
        //}

        [HttpGet("GetAllCompanyStocks")]
        [ProducesResponseType(typeof(DynamoDBStocks), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<DynamoDBStocks>>> GetAllCompanyStocks()
        {
            _logger.LogInformation("Start calling GetAllCompanyStocks function");
            List<DynamoDBStocks> stocks;
            try
            {
                stocks = await _dynamoDbstockService.Get();
            }
            catch (Exception ex)
            {
                _logger.LogError("There is an exception", ex);
                throw;
            }
            _logger.LogInformation("End calling GetAllCompanyStocks function");
            return stocks;
        }

        [HttpGet("GetCompanyStocksByCode/{code}")]
        [ProducesResponseType(typeof(DynamoDBStocks), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<DynamoDBStocks>>> GetCompanyStocksByCode([FromRoute] string code)
        {
            _logger.LogInformation("Start calling GetCompanyStocksByCode function");
            List<DynamoDBStocks> stocks;
            try
            {
                stocks = await _dynamoDbstockService.GetStockByCompanyCode(code);
            }
            catch (Exception ex)
            {
                _logger.LogError("There is an exception", ex);
                throw;
            }
            _logger.LogInformation("End calling GetCompanyStocksByCode function");
            return stocks;
        }

        [HttpPost("GetCompanyStocks")]
        [ProducesResponseType(typeof(DynamoDBStocks), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<DynamoDBStocks>>> GetCompanyStocks([FromBody] StockGetVM input)
        {
            _logger.LogInformation("Start calling GetCompanyStocks function");
            List<DynamoDBStocks> stocks;
            try
            {
                stocks = await _dynamoDbstockService.SearchStocks(input);
            }
            catch (Exception ex)
            {
                _logger.LogError("There is an exception", ex);
                throw;
            }
            _logger.LogInformation("End calling GetCompanyStocks function");
            return stocks;
        }
    }
}
