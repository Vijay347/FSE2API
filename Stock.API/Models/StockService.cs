using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Stock.API.Models
{
    public interface IStockService
    {
        Task<Stocks> Add(StockAddVM input);
        Task<string> DeleteStocks(string code);
    }
    public class StockService : IStockService
    {
        private readonly IMongoCollection<Stocks> _stocks;
        private readonly IMongoClient _client;
        public StockService(IStockDatabaseSettings settings)
        {
            _client = new MongoClient(settings.ConnectionString);
            var database = _client.GetDatabase(settings.DatabaseName);

            _stocks = database.GetCollection<Stocks>(settings.StocksCollectionName);
        }

        public async Task<Stocks> Add(StockAddVM input)
        {
            Stocks st = new Stocks();
            Stocks stk = new Stocks { Id = Guid.NewGuid(), CompanyCode = input.CompanyCode, Price = input.Price, Date = input.Date, Time = input.Time };
            try
            {
                await _stocks.InsertOneAsync(stk);
                st = await _stocks.FindAsync(x => x.Id == stk.Id).Result.FirstOrDefaultAsync();
            }
            catch(MongoException)
            {
                throw new Exception("Adding stock failed");
            }
            return st;
        }

        public async Task<string> DeleteStocks(string code)
        {
            try
            {
                await _stocks.DeleteManyAsync(stk => stk.CompanyCode == code);
            }
            catch (MongoException)
            {
                throw new Exception("No document found");
            }
            return "Stocks deleted successfully";
        }
    }
}
