using Goal.Enums;
using Goal.Models.Requests;
using Goal.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using static Goal.Services.LogService;

namespace Goal.Models.Domain
{
    public static class Log
    {

        public static LogStatusType GetLogStatus(string userId)
        {
            return LogService.Data.GetLogStatus(userId);
        }


        public static bool CreateNewLog(string userId, NewUserLogInsertRequest model)
        {
            bool isSuccess = true;
            // verify log does not already exist
            LogStatusType logStatus = LogService.Data.GetLogStatus(userId);

            if (logStatus != LogStatusType.NewUser)
            {
                throw new ApplicationException("A log already exists for the current user.");
            }

            DateTime today = DateTime.Today;

            DateTime startOfThisYear = LogService.GetYearStartOfDate(today);
            DateTime startOfThisMonth = LogService.GetMonthStartOfDate(today);
            string defaultMonthGoalType = "Fixed";
            int defaultMonthFixedGoal = 100;
            var firstMonth = new DbMonth(
                monthId: 0,
                userId: userId,
                startDate: startOfThisMonth,
                grossCredits: 0,
                grossDebits: 0,
                startAmount: model.StartAmount,
                previousMonthId: null,
                isStartAmountConfirmed: true);

            try
            {
                // Create Preferences
                LogService.Data.InsertUserPreferences(userId, defaultMonthGoalType, defaultMonthFixedGoal);

                // Create Year
                LogService.Data.InsertYear(userId, startOfThisYear, model.StartAmount, model.YearGoal);


                // Create Month
                LogService.Data.InsertDbMonth(firstMonth);

                // Create Categories
                LogService.Data.SetDefaultCategories(userId);

            }
            catch (Exception e)
            {
                isSuccess = false;
                throw e;
            }

            return isSuccess;
        }




        public static DbMonth CreateSuccessorDbMonth(string userId, DbMonth sourceMonth)
        {
            // build a check in the insert Month/ Year Proc to make sure no duplicate Log Entites are being made
            DateTime successorStartDate = LogService.GetMonthStartOfDate(sourceMonth.StartDate.AddMonths(1));

            double sourceBalance = CalculateBalance(sourceMonth.StartAmount, sourceMonth.GrossCredits, sourceMonth.GrossDebits);

            var successorMonth = new DbMonth(
                monthId: 0,
                userId: userId,
                startDate: successorStartDate,
                grossCredits: 0,
                grossDebits: 0,
                startAmount: sourceBalance,
                previousMonthId: sourceMonth.MonthId,
                isStartAmountConfirmed: false);

            return successorMonth;
        }



        public static DbMonth CreatePredecessorDbMonth(string userId, DbMonth sourceMonth)
        {
            // build a check in the insert Month/ Year Proc to make sure no duplicate Log Entites are being made
            DateTime predecessorStartDate = LogService.GetMonthStartOfDate(sourceMonth.StartDate.AddMonths(-1));

            double sourceBalance = CalculateBalance(sourceMonth.StartAmount, sourceMonth.GrossCredits, sourceMonth.GrossDebits);

            var predecessorMonth = new DbMonth(
                monthId: 0,
                userId: userId,
                startDate: predecessorStartDate,
                grossCredits: 0,
                grossDebits: 0,
                startAmount: sourceMonth.StartAmount,
                previousMonthId: 0,
                isStartAmountConfirmed: false);

            return predecessorMonth;
        }



        public static Month RenderMonth(DbMonth dbMonth)
        {
            DbPreferences dbPrederences = LogService.Data.GetPreferencesByUserId(dbMonth.UserId);
            List<double> adjustments = LogService.Data.GetMonthAdjustments(dbMonth.MonthId);


            bool isCurrentMonth = (GetMonthStartOfDate(DateTime.Today) == GetMonthStartOfDate(dbMonth.StartDate));

            return new MonthSnapshot(
                monthId: dbMonth.MonthId,
                previousMonthId: dbMonth.PreviousMonthId,
                startDate: dbMonth.StartDate,
                monthName: MonthOrdinalToName(dbMonth.StartDate.Month),
                goal: 0,
                grossCredits: dbMonth.GrossCredits,
                grossDebits: dbMonth.GrossDebits,
                startAmount: dbMonth.StartAmount,
                adustments: adjustments,
                year: dbMonth.StartDate.Year,
                yearStartDate: LogService.GetYearStartOfDate(dbMonth.StartDate),
                isCurrentMonth: isCurrentMonth);
        }




        private static string MonthOrdinalToName(int monthOrdinal)
        {
            if (monthOrdinal > 12 || monthOrdinal < 0)
            {
                throw new ArgumentException("Invalid Month Ordinal");
            }

            switch (monthOrdinal)
            {
                case 1:
                    return "January";
                case 2:
                    return "February";
                case 3:
                    return "March";
                case 4:
                    return "April";
                case 5:
                    return "May";
                case 6:
                    return "June";
                case 7:
                    return "July";
                case 8:
                    return "August";
                case 9:
                    return "September";
                case 10:
                    return "October";
                case 11:
                    return "November";
                case 12:
                    return "December";
                default:
                    return "NONE";
            }
        }



        private static int CalculateMonthGoal(
            double currentBalance,
            double yearGoal,
            DateTime startDate,
            int monthFixedGoal)
        {
            int monthsLeftThisYear = 12 - (startDate.Month) + 1;

            double simpleGoal = (yearGoal - currentBalance) / monthsLeftThisYear;

            return (simpleGoal > 0 ? (int)Math.Ceiling(simpleGoal) : monthFixedGoal);
        }


        private static double CalculateBalance(double startingAmount, double credits, double debits)
        {
            return startingAmount + (credits - debits);

        }


        public static CurrentMonth GetCurrentMonth(string userId)
        {
            try
            {
                DbMonth dbThisMonth = LogService.Data.GetDbMonthByDate(userId, DateTime.Now);
                DbPreferences dbPreferences = LogService.Data.GetPreferencesByUserId(userId);

                string _monthName = MonthOrdinalToName(dbThisMonth.StartDate.Month);
                


                List<double> _ajustments = new List<double>();
                //int _yearId = 0;
                DateTime _yearStartDate = new DateTime();
                int _year = _yearStartDate.Year;
                double _yearStartAmount = 0;
                int _yearGoal = 0;
                bool _isCurrentYear = true;
                double _monthSaved = dbThisMonth.GrossCredits - dbThisMonth.GrossDebits;
                double _balance = CalculateBalance(dbThisMonth.StartAmount, dbThisMonth.GrossCredits, dbThisMonth.GrossDebits);
                int _monthCalculatedGoal = CalculateMonthGoal(
                    currentBalance: _balance,
                    yearGoal: dbPreferences.YearGoal,
                    startDate: dbThisMonth.StartDate,
                    monthFixedGoal: dbPreferences.MonthFixedGoal);
                int _goal = dbPreferences.MonthGoalType == MonthGoalType.Fixed
                    ? dbPreferences.MonthFixedGoal : _monthCalculatedGoal;
                double _yearSaved = _balance - _yearStartAmount;
                double _amountLeftToSave = _goal - _balance;
                List<string> _categories = new List<string>();

                return new CurrentMonth(
                    monthId: dbThisMonth.MonthId,
                    previousMonthId: dbThisMonth.PreviousMonthId,
                    startDate: dbThisMonth.StartDate,
                    monthName: _monthName,
                    goal: _goal,
                    monthFixedGoal: dbPreferences.MonthFixedGoal,
                    monthGoalType: dbPreferences.MonthGoalType,
                    monthCalculatedGoal: _monthCalculatedGoal,
                    grossCredits: dbThisMonth.GrossCredits,
                    grossDebits: dbThisMonth.GrossDebits,
                    startAmount: dbThisMonth.StartAmount,
                    ajustments: _ajustments,
                    year: _year,
                    yearStartDate: _yearStartDate,
                    yearStartAmount: _yearStartAmount,
                    yearGoal: _yearGoal,
                    isCurrentYear: _isCurrentYear,
                    yearSaved: _yearSaved,
                    monthSaved: _monthSaved,
                    balance: _balance,
                    amountLeftToSave: _amountLeftToSave,
                    categories: _categories);
            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}