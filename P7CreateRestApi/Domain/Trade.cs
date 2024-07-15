using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Domain
{
    public class Trade : IValidatable
    {
        public int TradeId { get; set; }

        [Required(ErrorMessage = "Account is mandatory")]
        [DataType(DataType.Text, ErrorMessage = "Account must be a string")]
        [MaxLength(50, ErrorMessage = "Account can't be longer than 50 characters")]
        public required string Account { get; set; }

        [Required(ErrorMessage = "AccountType is mandatory")]
        [DataType(DataType.Text, ErrorMessage = "AccountType must be a string")]
        [MaxLength(50, ErrorMessage = "AccountType can't be longer than 50 characters")]
        public required string AccountType { get; set; }

        [DoubleValidation(ErrorMessage = "BuyQuantity must be a valid double")]
        public double? BuyQuantity { get; set; }

        [DoubleValidation(ErrorMessage = "SellQuantity must be a valid double")]
        public double? SellQuantity { get; set; }

        [DoubleValidation(ErrorMessage = "BuyPrice must be a valid double")]
        public double? BuyPrice { get; set; }

        [DoubleValidation(ErrorMessage = "SellPrice must be a valid double")]
        public double? SellPrice { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "TradeDate must be a date and a time of day")]
        public DateTime? TradeDate { get; set; }

        [Required(ErrorMessage = "TradeSecurity is mandatory")]
        [DataType(DataType.Text, ErrorMessage = "TradeSecurity must be a string")]
        [MaxLength(50, ErrorMessage = "TradeSecurity can't be longer than 50 characters")]
        public required string TradeSecurity { get; set; }

        [Required(ErrorMessage = "TradeStatus is mandatory")]
        [DataType(DataType.Text, ErrorMessage = "TradeStatus must be a string")]
        [MaxLength(50, ErrorMessage = "TradeStatus can't be longer than 50 characters")]
        public required string TradeStatus { get; set; }

        [Required(ErrorMessage = "Trader is mandatory")]
        [DataType(DataType.Text, ErrorMessage = "Trader must be a string")]
        [MaxLength(50, ErrorMessage = "Trader can't be longer than 50 characters")]
        public required string Trader { get; set; }

        [Required(ErrorMessage = "Benchmark is mandatory")]
        [DataType(DataType.Text, ErrorMessage = "Benchmark must be a string")]
        [MaxLength(100, ErrorMessage = "Benchmark can't be longer than 100 characters")]
        public required string Benchmark { get; set; }

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

        public void Validate()
        {
            ValidationExtensions.Validate(this);
        }
    }
}