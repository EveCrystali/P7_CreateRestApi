using System;

namespace Dot.Net.WebApi.Domain
{
    public class Trade
    {
        public int TradeId { get; set; }
        public string Account {get; set;}
        public string AccountType {get; set;}
        public double? BuyQuantity {get; set;}
        public double? SellQuantity {get; set;}
        public double? BuyPrice {get; set;}
        public double? SellPrice {get; set;}
        public DateTime? TradeDate {get; set;}
        public string TradeSecurity {get; set;}
        public string TradeStatus {get; set;}
        public string Trader {get; set;}
        public string Benchmark {get; set;}
        public string Book {get; set;}
        public string CreationName {get; set;}
        public DateTime? CreationDate {get; set;}
        public string RevisionName {get; set;}
        public DateTime? RevisionDate {get; set;}
        public string DealName {get; set;}
    }
}