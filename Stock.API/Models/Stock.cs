using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Stock.API.Models
{
    public class StockDatabaseSettings : IStockDatabaseSettings
    {
        public string StocksCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IStockDatabaseSettings
    {
        public string StocksCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
    
    public class Stocks
    {
        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        public Guid Id { get; set; }
        [BsonElement]
        public string CompanyCode { get; set; }
        [BsonElement]
        public decimal Price { get; set; }
        [BsonElement]
        [BsonDateTimeOptions(DateOnly = true, Kind = DateTimeKind.Local)]
        public DateTime? Date { get; set; }
        [BsonElement]
        [BsonDateTimeOptions(DateOnly = false, Kind = DateTimeKind.Local)]
        public DateTime? Time { get; set; }
    }
}
