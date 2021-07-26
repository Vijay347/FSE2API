using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockDetails.API.Models;
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
        private readonly IStockService _stockService;

        private readonly ILogger<StockDetailsController> _logger;

        public StockDetailsController(ILogger<StockDetailsController> logger, IStockService stockService)
        {
            _stockService = stockService;
            _logger = logger;
        }

        [HttpGet("GetAllCompanyStocks")]
        [ProducesResponseType(typeof(Stocks), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Stocks>>> GetAllCompanyStocks()
        {
           return await _stockService.Get();
        }

        [HttpGet("GetCompanyStocksByCode/{code}")]
        [ProducesResponseType(typeof(Stocks), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Stocks>>> GetCompanyStocksCode([FromRoute] string code)
        {
            return await _stockService.GetStockByCompanyCode(code);
        }

        [HttpPost("GetCompanyStocks")]
        [ProducesResponseType(typeof(Stocks), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Stocks>>> GetCompanyStocks([FromBody] StockGetVM input)
        {
           return await _stockService.SearchStocks(input);
        }
    }
}
