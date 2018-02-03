using Goal.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Goal.Models
{
    public class DbPreferences
    {
        public DbPreferences() { }

        public DbPreferences(
            int preferenceId,
            string userId,
            MonthGoalType monthGoalType,
            int monthFixedGoal,
            int yearGoal,
            double yearStartAmount)
        {
            PreferenceId = preferenceId;
            UserId = userId;
            MonthGoalType = monthGoalType;
            MonthFixedGoal = monthFixedGoal;
            YearGoal = yearGoal;
            YearStartAmount = yearStartAmount;
            
        }

        public int PreferenceId { get; private set; }
        public string UserId { get; private set; }
        public MonthGoalType MonthGoalType { get; private set; }
        public int MonthFixedGoal { get; private set; }
        public int YearGoal { get; private set; }
        public double YearStartAmount { get; private set; }
    }
}