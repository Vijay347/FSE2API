using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockDetails.API.Models
{
    public interface IStockService
    {
        Task<List<Stocks>> Get();
        Task<Stocks> Get(Guid Id);
        Task<List<Stocks>> GetStockByCompanyCode(string code);
        Task<List<Stocks>> SearchStocks(StockGetVM input);
    }
    public class StockService : IStockService
    {
        private readonly IMongoCollection<Stocks> _stocks;
        public StockService(IStockDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _stocks = database.GetCollection<Stocks>(settings.StocksCollectionName);
        }

        public async Task<List<Stocks>> Get()
        {
            List<Stocks> stocks;
            stocks = await _stocks.FindAsync(stk => true).Result.ToListAsync();
            return stocks;
        }

        public async Task<Stocks> Get(Guid id)
        {
            return await _stocks.FindAsync(stk => stk.Id == id).Result.FirstOrDefaultAsync();
        }
        public async Task<List<Stocks>> GetStockByCompanyCode(string code)
        {
            return await _stocks.FindAsync(stk => stk.CompanyCode == code).Result.ToListAsync();
        }

        public async Task<List<Stocks>> SearchStocks(StockGetVM input)
        {
            return await _stocks.FindAsync(stk => stk.CompanyCode == input.CompanyCode && stk.Date >= input.StartDate && stk.Date <= input.EndDate).Result.ToListAsync();
        }
    }

    public class StockDynamoDBService
    {

    }
}
