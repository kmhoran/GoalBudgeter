using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Goal.Models.Requests
{
    public class NewUserLogInsertRequest
    {
        //public string UserId {get; set;}
        public double StartAmount { get; set; }
        public int YearGoal { get; set; }
        //public DateTime StartDate { get; set; }
    }
}