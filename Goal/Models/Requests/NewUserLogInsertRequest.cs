using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Goal.Models.Requests
{
    public class NewUserLogInsertRequest
    {
        public string UserId {get; set;}
        public double StartingAmount { get; set; }
        public int GoalAmount { get; set; }
        public DateTime StartDate { get; set; }
    }
}