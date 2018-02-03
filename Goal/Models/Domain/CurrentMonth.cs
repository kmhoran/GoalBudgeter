using Goal.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Goal.Models.Domain
{
    public class CurrentMonth: Month
    {
        public CurrentMonth(
            int monthId,
            int? previousMonthId,
            DateTime startDate,
            string monthName,
            int goal,
            int monthFixedGoal,
            int monthCalculatedGoal,
            MonthGoalType monthGoalType,
            double grossCredits,
            double grossDebits,
            double startAmount,
            List<double> ajustments,
            int year,
            DateTime yearStartDate,
            double yearStartAmount,
            int yearGoal,
            bool isCurrentYear,
            double yearSaved,
            double monthSaved,
            double balance,
            double amountLeftToSave,
            List<string> categories)
        {
            MonthId = monthId;
            PreviousMonthId = previousMonthId;
            StartDate = startDate;
            MonthName = monthName;
            Goal = goal;
            MonthFixedGoal = monthFixedGoal;
            MonthCalculatedGoal = monthCalculatedGoal;
            MonthGoalType = monthGoalType;
            GrossCredits = grossCredits;
            GrossDebits = grossDebits;
            StartAmount = startAmount;
            Adustments = ajustments;
            Year = year;
            YearStartDate = yearStartDate;
            YearStartAmount = yearStartAmount;
            YearGoal = yearGoal;
            IsCurrentMonth = isCurrentYear;
            YearSaved = yearSaved;
            MonthSaved = monthSaved;
            Balance = balance;
            AmountLeftToSave = amountLeftToSave;
            Categories = categories;
        }

        public List<string> Categories { get; private set; }

        public double YearStartAmount { get; private set; }
        public int YearGoal { get; private set; }

        public UserType UserType { get; private set; }

        public int MonthFixedGoal { get; private set; }
        public int MonthCalculatedGoal { get; private set; }
        public MonthGoalType MonthGoalType { get; private set; }

        public double YearSaved { get; private set; }
        public double MonthSaved { get; private set; }
        public double Balance { get; private set; }
        public double AmountLeftToSave { get; private set; }


        /*
       @ public int MonthId { get; protected set; }
       @ public int PreviousMonthId { get; protected set; }
        @public DateTime StartDate { get; protected set; }
        @ public int Goal { get; protected set; }
        @public int MonthFixedGoal { get; private set; }
        @ public int MonthCalculatedGoal { get; private set; }
        @ public MonthGoalType MonthGoalType { get; private set; }
        @public double GrossCredits { get; protected set; }
        @public double GrossDebits { get; protected set; }
        @public double StartAmount { get; protected set; }


        @public List<double> Adustments { get; protected set; }

        @ public int YearId { get; protected set; }
        @ public int Year { get; protected set; }
        @ public DateTime YearStartDate { get; protected set; }
        @public double YearStartAmount { get; private set; }
        @public int YearGoal { get; private set; }
        @ public bool IsCurrentYear { get; protected set; }
        @ public double YearSaved { get; private set; }
        @ public double MonthSaved { get; private set; }
        @ public double Balance { get; private set; }
        @public double AmountLeftToSave { get; private set; }
         */

    }
}
 