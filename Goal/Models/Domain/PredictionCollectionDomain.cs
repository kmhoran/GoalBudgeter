using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Goal.Models.Domain
{
    public class PredictionCollectionDomain
    {
        public int Fixed { get; set; }
        public int Average { get; set; }
        public int Relative { get; set; }
    }
}