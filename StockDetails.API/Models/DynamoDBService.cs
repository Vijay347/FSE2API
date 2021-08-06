using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockDetails.API.Models
{
    public interface IDynamoDBService
    {
        Task<List<DynamoDBStocks>> Get();
        Task<DynamoDBStocks> Get(Guid Id);
        Task<List<DynamoDBStocks>> GetStockByCompanyCode(string code);
        Task<List<DynamoDBStocks>> SearchStocks(StockGetVM input);
    }
    public class DynamoDBService : IDynamoDBService
    {
        private readonly IDynamoDBContext _dynamoDbContext;
        public DynamoDBService(IDynamoDBContext dynamoDbContext)
        {
            _dynamoDbContext = dynamoDbContext;
        }

        public async Task<List<DynamoDBStocks>> Get()
        {
            try
            {
                var scanConditions = new List<ScanCondition>() {
                new ScanCondition("Id", ScanOperator.IsNotNull) };
                var searchResults = _dynamoDbContext.ScanAsync<DynamoDBStocks>(scanConditions, null);
                return await searchResults.GetNextSetAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DynamoDBStocks> Get(Guid Id)
        {
            try
            {
                return await _dynamoDbContext.LoadAsync<DynamoDBStocks>(Id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DynamoDBStocks>> GetStockByCompanyCode(string code)
        {
            try
            {
                var scanConditions = new List<ScanCondition>() {
                new ScanCondition("CompanyCode", ScanOperator.Equal, new object[] { code })};
                var searchResults = _dynamoDbContext.ScanAsync<DynamoDBStocks>(scanConditions, null);
                return await searchResults.GetNextSetAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DynamoDBStocks>> SearchStocks(StockGetVM input)
        {
            try
            {
                var scanConditions = new List<ScanCondition>() {
                new ScanCondition("CompanyCode", ScanOperator.Equal, new object[] { input.CompanyCode }),
                new ScanCondition("Date", ScanOperator.Between, new object[] { input.StartDate, input.EndDate})};
                var searchResults = _dynamoDbContext.ScanAsync<DynamoDBStocks>(scanConditions, null);
                return await searchResults.GetNextSetAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
