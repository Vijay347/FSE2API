using Company.API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Company.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/market/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class CompanyController : ControllerBase
    {
        private readonly EstockmarketContext _context;
        //private readonly ILogger<CompanyController> _logger;

        //public CompanyController(ILogger<CompanyController> logger, EstockmarketContext context)
        //{
        //    _logger = logger;
        //    _context = context;
        //}

        public CompanyController(EstockmarketContext context)
        {
            _context = context;
        }

        [HttpPost("Register")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> PostCompany([FromBody] CompanyDetails companyDetails)
        {
            Log.Information("Start calling PostCompany function");
            //_logger.LogInformation("Start calling PostCompany function");
            using (IDbContextTransaction _transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.CompanyDetails.Add(companyDetails);
                    await _context.SaveChangesAsync();
                    _transaction.Commit();
                }
                catch (DbUpdateException ex)
                {
                    Log.Error("There is an exception", ex);
                    _transaction.Rollback();
                    throw;
                }
            }
            Log.Information("End calling PostCompany function");
            return Ok(companyDetails);
        }

        [HttpGet("Info/{companycode}")]
        [ProducesResponseType(typeof(CompanyDetails), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CompanyDetails>> GetCompanyByCode([FromRoute] string companycode)
        {
            CompanyDetails det = null;
            Log.Information("Start calling GetCompanyByCode function");
            try
            {
                det = await _context.CompanyDetails
                      .SingleOrDefaultAsync(x => x.Code.ToLower() == companycode.ToLower());
            }
            catch (Exception ex)
            {
                Log.Error("There is an exception", ex);
                throw;
            }
            Log.Information("End calling GetCompanyByCode function");
            return det;
        }

        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(IEnumerable<CompanyDetails>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CompanyDetails>>> GetCompanies()
        {
            Log.Information("End calling GetCompanies function");
            List<CompanyDetails> dets = null;
            try
            {
                dets = await _context.CompanyDetails.ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error("There is an exception", ex);
                throw;
            }
            Log.Information("End calling GetCompanies function");
            return dets;
        }

        [HttpDelete("Delete/{companycode}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CompanyDetails>> DeleteCompany([FromRoute] string companycode)
        {
            Log.Information("End calling DeleteCompany function");
            var company = await _context.CompanyDetails.SingleOrDefaultAsync(x => x.Code.ToLower() == companycode.ToLower());
            if (company == null)
                return Ok("Company details not found");

            using (IDbContextTransaction _transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.CompanyDetails.Remove(company);
                    await _context.SaveChangesAsync();
                    _transaction.Commit();
                }
                catch (DbUpdateException ex)
                {
                    Log.Error("There is an exception", ex);
                    _transaction.Rollback();
                    throw;
                }
            }
            Log.Information("End calling DeleteCompany function");
            return company;
        }
    }
}
