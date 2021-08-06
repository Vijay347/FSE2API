using Amazon.DynamoDBv2.DataModel;
using System;

namespace StockDetails.API.Models
{
    [DynamoDBTable("Stocks")]
    public class DynamoDBStocks
    {
        [DynamoDBProperty("Id")]
        [DynamoDBHashKey]
        public Guid Id { get; set; }
        [DynamoDBProperty("CompanyCode")]
        public string CompanyCode { get; set; }
        [DynamoDBProperty("Price")]
        public decimal Price { get; set; }
        [DynamoDBProperty("Date")]
        public DateTime? Date { get; set; }
        [DynamoDBProperty("Time")]
        public DateTime? Time { get; set; }
    }
}
