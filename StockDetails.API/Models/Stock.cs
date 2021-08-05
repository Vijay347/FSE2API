using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace StockDetails.API.Models
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
        public virtual Guid Id { get; set; }
        [BsonElement]
        public virtual string CompanyCode { get; set; }
        [BsonElement]
        public virtual decimal Price { get; set; }
        [BsonElement]
        [BsonDateTimeOptions(DateOnly = true, Kind = DateTimeKind.Local)]
        public virtual DateTime? Date { get; set; }
        [BsonElement]
        public virtual string Time { get; set; }
    }

}
