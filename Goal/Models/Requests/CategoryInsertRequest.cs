using Goal.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Goal.Models.Requests
{
    public class CategoryInsertRequest
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public TransactionType TypeId { get; set; }
        public ForecastType ForecastType { get; set; }
        public decimal Amount { get; set; }
    }
}