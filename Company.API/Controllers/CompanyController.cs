using Company.API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Company.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class CompanyController : ControllerBase
    {
        private readonly EstockmarketContext _context;
        private readonly ILogger<CompanyController> _logger;

        public CompanyController(ILogger<CompanyController> logger, EstockmarketContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("GetAllCompanies")]
        [ProducesResponseType(typeof(IEnumerable<CompanyDetails>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CompanyDetails>>> GetCompanies()
        {
           return await _context.CompanyDetails.ToListAsync();
        }

        [HttpGet("GetCompany/{id}")]
        [ProducesResponseType(typeof(IEnumerable<CompanyDetails>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CompanyDetails>> GetCompany([FromRoute] Guid id)
        {
            var company = await _context.CompanyDetails.FindAsync(id);

            if (company == null)
                return Ok(null);

            return company;
        }

        [HttpPut("UpdateCompany/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutCompany([FromRoute] Guid id, [FromBody] CompanyDetails companyDetails)
        {
            if (id != companyDetails.Id)
            {
                return BadRequest();
            }

            _context.Entry(companyDetails).State = EntityState.Modified;

            using (IDbContextTransaction _transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync();
                    _transaction.Commit();
                }
                catch (DbUpdateConcurrencyException)
                {
                    _transaction.Rollback();
                    if (!CompanyExists(id))
                    {
                        return Ok("Company not found");
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return Ok("Company updated successfully");
        }

        [HttpPost("AddCompany")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> PostCompany([FromBody] CompanyDetails companyDetails)
        {
            using (IDbContextTransaction _transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    _context.CompanyDetails.Add(companyDetails);
                    await _context.SaveChangesAsync();
                    _transaction.Commit();

                }
                catch (DbUpdateException)
                {
                    _transaction.Rollback();
                    throw;
                }
            }

            return Ok(companyDetails);
        }

        [HttpDelete("DeleteCompany/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CompanyDetails>> DeleteCompany([FromRoute] Guid id)
        {
            var company = await _context.CompanyDetails.FindAsync(id);
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
                catch (DbUpdateException)
                {
                    _transaction.Rollback();
                    throw;
                }
            }

            return company;
        }

        private bool CompanyExists(Guid id)
        {
            return _context.CompanyDetails.Any(e => e.Id == id);
        }
    }
}
