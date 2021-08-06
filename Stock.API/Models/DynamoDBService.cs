using Amazon.DynamoDBv2.DataModel;
using System;
using System.Threading.Tasks;

namespace Stock.API.Models
{
    public interface IDynamoDBService
    {
        Task<DynamoDBStocks> Add(StockAddVM input);
        Task<string> DeleteStocks(string code);
    }
    public class DynamoDBService : IDynamoDBService
    {
        private readonly IDynamoDBContext _dynamoDbContext;
        public DynamoDBService(IDynamoDBContext dynamoDbContext)
        {
            _dynamoDbContext = dynamoDbContext;
        }
        public async Task<DynamoDBStocks> Add(StockAddVM input)
        {
            DynamoDBStocks st = new DynamoDBStocks();
            DynamoDBStocks stk = new DynamoDBStocks { Id = Guid.NewGuid(), CompanyCode = input.CompanyCode, Price = input.Price, Date = input.Date, Time = input.Time };
            try
            {
                await _dynamoDbContext.SaveAsync(stk);
                st = await _dynamoDbContext.LoadAsync<DynamoDBStocks>(stk.Id);
            }
            catch (Exception ex)
            {
                throw new Exception("Adding stock failed");
            }
            return st;
        }

        public async Task<string> DeleteStocks(string code)
        {
            try
            {
                await _dynamoDbContext.DeleteAsync(code);
            }
            catch (Exception)
            {
                throw new Exception("No document found");
            }
            return "Stocks deleted successfully";
        }
    }
}
