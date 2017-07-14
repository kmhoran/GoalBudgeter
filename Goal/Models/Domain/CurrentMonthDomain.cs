using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Goal.Models.Domain
{
    public class CurrentMonthDomain
    {
        public int YearId { get; set; }
        public DateTime YearStart { get; set; }
        public double YearStartingAmount { get; set; }
        public int YearGoal { get; set; }
        public double Balance { get; set; }
        public double YearTotalSaved { get; set; }
        public double YearAmountToReachGoal { get; set; }
        public int MonthId { get; set; }
        public DateTime MonthStart { get; set; }
        public double TotalSaved { get; set; }
        public double MonthCredits { get; set; }
        public double MonthDebits { get; set; }
        public double MonthStartingAmount { get; set; }
        public int ExpenseBudget { get; set; }
        public int IncomeBudget { get; set; }
        public int PreferenceId { get; set; }
        public string GoalType { get; set; }
        public int FixedMonthGoal { get; set; }
        public double ToReachFixedGoal { get; set; }
        public int CalculatedMonthGoal { get; set; }
        public double ToReachCalculatedGoal { get; set; }
    }
}