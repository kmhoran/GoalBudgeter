using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Goal.Models.Domain
{
    public class CategoryCollectionDomain
    {
        public List<CategoryDomain> Credits { get; set; }
        public List<CategoryDomain> Debits { get; set; }
    }
}