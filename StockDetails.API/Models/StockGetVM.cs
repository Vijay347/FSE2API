using System;

namespace StockDetails.API.Models
{
    public class StockGetVM
    {
        public string CompanyCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
