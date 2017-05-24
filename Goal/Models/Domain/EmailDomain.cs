using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Goal.Models.Domain
{
    public class EmailDomain
    {
        public string RecieverName { get; set; }
        public string RecieverEmail { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string SenderEmail { get; set; }

    }
}