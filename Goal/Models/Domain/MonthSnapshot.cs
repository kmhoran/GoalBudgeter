using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Goal.Models.Domain
{
    public class MonthSnapshot : Month
    {
        public MonthSnapshot(
            int monthId,
            int? previousMonthId,
            DateTime startDate,
            string monthName,
            int goal,
            double grossCredits,
            double grossDebits,
            double startAmount,
            List<double> adustments,
            int year,
            DateTime yearStartDate,
            bool isCurrentMonth)
        {
            MonthId = monthId;
            PreviousMonthId = previousMonthId;
            StartDate = startDate;
            MonthName = monthName;
            Goal = goal;
            GrossCredits = grossCredits;
            GrossDebits = grossDebits;
            StartAmount = StartAmount;
            Adustments = adustments;
            Year = year;
            YearStartDate = yearStartDate;
            IsCurrentMonth = isCurrentMonth;
        }
    }
}