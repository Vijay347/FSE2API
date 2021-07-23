using System;
using System.ComponentModel.DataAnnotations;

namespace Company.API.Models
{
    public partial class CompanyDetails
    {
        [Required]
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(20, ErrorMessage = "Allowed length should be maximum 20")]
        public string Code { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Allowed length should be maximum 50")]
        public string Name { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Allowed length should be maximum 100")]
        public string Ceo { get; set; }
        [Required]
        [Range(1,Int64.MaxValue)]
        public long Turnover { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Allowed length should be maximum 100")]
        public string Website { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Allowed length should be maximum 50")]
        public string StockExchange { get; set; }
        [MaxLength(50, ErrorMessage = "Allowed length should be maximum 50")]
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        [MaxLength(50, ErrorMessage = "Allowed length should be maximum 50")]
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
