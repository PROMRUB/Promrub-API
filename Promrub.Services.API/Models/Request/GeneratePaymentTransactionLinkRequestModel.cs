﻿using System.ComponentModel.DataAnnotations;

namespace Promrub.Services.API.Models.Request
{
    public class GeneratePaymentTransactionLinkRequestModel
    {
        public string? PosId { get; set; }
        public string? TransactionId { get; set; }
        public List<RequestItemList> RequestItemList { get; set; } = new List<RequestItemList>();
        public int ItemTotal { get; set; }
        public int QuantityTotal { get; set; }
        public decimal TotalItemsPrices { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalTransactionPrices { get; set; }
    }

    public class RequestItemList
    {
        public int Quantity { get; set; }
        public string? ItemName { get; set; }
        public decimal? Price { get; set; }
        public decimal? TotalPrices { get; set; }
    }
}
