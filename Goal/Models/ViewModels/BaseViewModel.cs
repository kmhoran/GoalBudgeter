using Goal.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Goal.Models.ViewModels
{
    public class BaseViewModel
    {
        public bool IsLoggedIn { get; set; }
        public LogStatusType LogStatus { get; set; }
    }
}