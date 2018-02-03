using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Goal.Models.Domain
{
    public abstract class Month
    {
        public int MonthId { get; protected set; }
        public int? PreviousMonthId { get; protected set; }
        public DateTime StartDate { get; protected set; }
        public string MonthName { get; protected set; }
        public int Goal { get; protected set; }
        public double GrossCredits { get; protected set; }
        public double GrossDebits { get; protected set; }
        public double StartAmount { get; protected set; }
        public List<double> Adustments { get; protected set; }

        public int Year { get; protected set; }
        public DateTime YearStartDate { get; protected set; }
        public bool IsCurrentMonth { get; protected set; }


        /*
         *public int YearId { get; private set; }
          **  public double YearStartAmount { get; private set; }
            * public DateTime YearStartDate { get; private set; }
           * public int Year { get; private set; }
           * public bool IsCurrentYear { get; private set; }
            
        
        // Months
            * public int MonthId { get; private set; }
           * public DateTime MonthStartDate { get; private set; }
           * public double MonthStartAmount { get; private set; }
           * public int MonthGoal { get; private set; }

            // User
            ** public UserType UserType { get; private set; }
            xx public string UserId { get; private set; }
            xx public string UserEmail { get; private set; }

            //Preferences
            xx public int PreferenceId { get; private set; }
            ** public int MonthFixedGoal { get; private set; }
            ** public int MonthCalculatedGoal { get; private set; }
            ** public MonthGoalType MonthGoalType { get; set; }
            ** public int YearGoal { get; private set; }

            // Transactions
            ** public double Balance { get; private set; }
            ** public double YearSaved { get; private set; }
            ** public double MonthSaved { get; private set; }
            xx public int MonthTransactionCount { get; private set; }
            * public double GrossCredits { get; private set; }
            * public double GrossDebits { get; private set; }

            // Categories
            public CategoryCollectionDomain Categories { get; private set; }

            private LogDbReaderDTO _logTemplate { get; set; } 
         */

    }
}