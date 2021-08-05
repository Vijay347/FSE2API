using Amazon.DynamoDBv2.DataModel;
using System;

namespace Stock.API.Models
{
    [DynamoDBTable("stocks")]
    public class DynamoDBStocks
    {
        [DynamoDBProperty("id")]
        [DynamoDBHashKey]
        public Guid Id { get; set; }
        [DynamoDBProperty("companyCode")]
        public string CompanyCode { get; set; }
        [DynamoDBProperty("price")]
        public decimal Price { get; set; }
        [DynamoDBProperty("date")]
        public DateTime? Date { get; set; }
        [DynamoDBProperty("time")]
        public string Time { get; set; }
    }
}
