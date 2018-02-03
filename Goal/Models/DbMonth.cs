using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Goal.Models
{
    public class DbMonth
    {
        public DbMonth() { }

        public DbMonth(
            int monthId,
            string userId,
            DateTime startDate,
            double grossCredits,
            double grossDebits,
            double startAmount,
            int? previousMonthId,
            bool isStartAmountConfirmed)
        {
            MonthId = monthId;
            UserId = userId;
            StartDate = startDate;
            GrossCredits = grossCredits;
            GrossDebits = grossDebits;
            StartAmount = startAmount;
            PreviousMonthId = previousMonthId;
            IsStartAmountConfirmed = isStartAmountConfirmed;
        }

        public int MonthId { get; private set; }
        public string UserId { get; private set; }
        public DateTime StartDate { get; private set; }
        public double GrossCredits { get; private set; }
        public double GrossDebits { get; private set; }
        public double StartAmount { get; private set; }
        public int? PreviousMonthId { get; private set; }
        public bool IsStartAmountConfirmed { get; private set; }
    }
}