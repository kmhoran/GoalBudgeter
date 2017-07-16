using Goal.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Goal.Models.Requests
{
    public class CategoriesUpdateRequest
    {
        public List<CategoryUpdateRequest> CategoryList { get; set; }
    }
}