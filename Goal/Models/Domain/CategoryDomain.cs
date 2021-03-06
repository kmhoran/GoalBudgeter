﻿using Goal.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Goal.Models.Domain
{
    public class CategoryDomain
    {
        public int CategoryId {get; set;}
        public string UserId { get; set; }
        public string Name { get; set; }
        public TransactionType TypeId { get; set; }
        public string ForecastType { get; set; }
        public PredictionCollectionDomain Predictions { get; set; }
    }
}