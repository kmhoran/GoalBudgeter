using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Goal.Models.Domain.Reports
{
    public class MonthlySpendingByCategoryDomain
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public double Predicted { get; set; }
        public double Real { get; set; }
    }
}