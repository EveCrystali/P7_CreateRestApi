using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Domain
{
    public class BidList
    {
        [Required(ErrorMessage = "BidListId is mandatory")]
        [Range(int.MinValue, int.MaxValue, ErrorMessage = "BidListId must be an integer")]
        public required int BidListId { get; set; }

        [Required(ErrorMessage = "Account is mandatory")]
        [DataType(DataType.Text, ErrorMessage = "Account must be a string")]
        [MaxLength(50, ErrorMessage = "Account can't be longer than 50 characters")]
        public required string Account { get; set; }

        [Required(ErrorMessage = "BidType is mandatory")]
        [DataType(DataType.Text, ErrorMessage = "BidType must be a string")]
        [MaxLength(50, ErrorMessage = "BidType can't be longer than 50 characters")]
        public required string BidType { get; set; }

        [DoubleValidation(ErrorMessage = "BidQuantity must be a valid double")]
        public double? BidQuantity { get; set; }

        [DoubleValidation(ErrorMessage = "AskQuantity must be a valid double")]
        public double? AskQuantity { get; set; }

        [DoubleValidation(ErrorMessage = "Bid must be a valid double")]
        public double? Bid { get; set; }

        [DoubleValidation(ErrorMessage = "Ask must be a valid double")]
        public double? Ask { get; set; }

        [Required(ErrorMessage = "Benchmark is mandatory")]
        [DataType(DataType.Text, ErrorMessage = "Benchmark must be a string")]
        [MaxLength(100, ErrorMessage = "Benchmark can't be longer than 100 characters")]
        public required string Benchmark { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "BidListDate must be a date and a time of day")]
        public DateTime? BidListDate { get; set; }

        [Required(ErrorMessage = "Commentary is mandatory")]

        [DataType(DataType.MultilineText, ErrorMessage = "Commentary must be a string")]
        [MaxLength(500, ErrorMessage = "Commentary can't be longer than 500 characters")]
        public required string Commentary { get; set; }


        [Required(ErrorMessage = "BidSecurity is mandatory")]
        [DataType(DataType.Text, ErrorMessage = "BidSecurity must be a string")]
        [MaxLength(50, ErrorMessage = "BidSecurity can't be longer than 50 characters")]
        public required string BidSecurity { get; set; }
        
        [Required(ErrorMessage = "BidStatus is mandatory")]
        [DataType(DataType.Text, ErrorMessage = "BidStatus must be a string")]
        [MaxLength(50, ErrorMessage = "BidStatus can't be longer than 50 characters")]
        public required string BidStatus { get; set; }
        
        [Required(ErrorMessage = "Trader is mandatory")]
        [DataType(DataType.Text, ErrorMessage = "Trader must be a string")]
        [MaxLength(50, ErrorMessage = "Trader can't be longer than 50 characters")]
        public required string Trader { get; set; }
        
        [Required(ErrorMessage = "Book is mandatory")]
        [DataType(DataType.Text, ErrorMessage = "Book must be a string")]
        [MaxLength(50, ErrorMessage = "Book can't be longer than 50 characters")]
        public required string Book { get; set; }
        
        [Required(ErrorMessage = "CreationName is mandatory")]
        [DataType(DataType.Text, ErrorMessage = "CreationName must be a string")]
        [MaxLength(50, ErrorMessage = "CreationName can't be longer than 50 characters")]
        public required string CreationName { get; set; }
        
        [DataType(DataType.DateTime, ErrorMessage = "CreationDate must be a date and a time of day")]
        public DateTime? CreationDate { get; set; }


        [Required(ErrorMessage = "RevisionName is mandatory")]
        [DataType(DataType.Text, ErrorMessage = "RevisionName must be a string")]
        [MaxLength(50, ErrorMessage = "RevisionName can't be longer than 50 characters")]
        public required string RevisionName { get; set; }
        
        [DataType(DataType.DateTime, ErrorMessage = "RevisionDate must be a date and a time of day")]
        public DateTime? RevisionDate { get; set; }
        
        [Required(ErrorMessage = "DealName is mandatory")]
        [DataType(DataType.Text, ErrorMessage = "DealName must be a string")]
        [MaxLength(50, ErrorMessage = "DealName can't be longer than 50 characters")]
        public required string DealName { get; set; }
        

        [Required(ErrorMessage = "DealType is mandatory")]
        [DataType(DataType.Text, ErrorMessage = "DealType must be a string")]
        [MaxLength(50, ErrorMessage = "DealType can't be longer than 50 characters")]
        public required string DealType { get; set; }
        
        [Required(ErrorMessage = "SourceListId is mandatory")]
        [DataType(DataType.Text, ErrorMessage = "SourceListId must be a string")]
        [MaxLength(25, ErrorMessage = "SourceListId can't be longer than 25 characters")]
        public required string SourceListId { get; set; }
        
        [Required(ErrorMessage = "Side is mandatory")]
        [DataType(DataType.Text, ErrorMessage = "Side must be a string")]
        [MaxLength(50, ErrorMessage = "Side can't be longer than 50 characters")]
        public required string Side { get; set; }
    }
}