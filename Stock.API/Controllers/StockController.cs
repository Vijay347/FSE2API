using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly IStockService _stockService;

        private readonly ILogger<StockController> _logger;

        public StockController(ILogger<StockController> logger, IStockService stockService)
        {
            _stockService = stockService;
            _logger = logger;
        }

        [HttpPost("AddCompanyStock")]
        [ProducesResponseType(typeof(Stocks), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Stocks>> PostCompanyStock([FromBody] StockAddVM input)
        {
            _logger.LogInformation("Start calling PostCompanyStock function");
            Stocks stocks = null;

            try
            {
                stocks = await _stockService.Add(input);
            }
            catch (Exception ex)
            {
                _logger.LogError("There is an exception", ex);
                throw;
            }
            _logger.LogInformation("End calling PostCompanyStock function");
            return stocks;
        }

        [HttpDelete("DeleteCompanyStocks/{code}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult<string>> DeleteCompanyStocks([FromRoute] string code)
        {
            _logger.LogInformation("Start calling DeleteCompanyStocks function");
            string result = null;
            try
            {
                result = await _stockService.DeleteStocks(code);
            }
            catch(Exception ex)
            {
                _logger.LogError("There is an exception", ex);
                throw;
            }
            _logger.LogInformation("End calling DeleteCompanyStocks function");
            return result;
        }
    }
}
