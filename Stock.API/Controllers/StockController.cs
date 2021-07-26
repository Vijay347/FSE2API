using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stock.API.Models;
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
            var stocks = await _stockService.Add(input);

            if (stocks == null)
                return Ok(null);

            return stocks;
        }

        [HttpDelete("DeleteCompanyStocks/{code}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult<string>> DeleteCompanyStocks([FromRoute] string code)
        {
            return await _stockService.DeleteStocks(code);
        }
    }
}
