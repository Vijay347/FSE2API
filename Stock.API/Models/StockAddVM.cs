using System;

namespace Stock.API.Models
{
    public class StockAddVM
    {
        public string CompanyCode { get; set; }
        public decimal Price { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Time { get; set; }
    }
}
