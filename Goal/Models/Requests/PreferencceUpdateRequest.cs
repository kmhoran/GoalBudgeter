using Goal.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Goal.Models.Requests
{
    public class PreferencceUpdateRequest
    {
        public int PreferenceId { get; set; }

        public string MonthlyGoalType { get; set; }

        public int MonthlyFixedGoal { get; set; }
    }
}