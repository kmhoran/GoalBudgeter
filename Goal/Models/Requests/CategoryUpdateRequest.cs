using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Goal.Models.Requests
{
    public class CategoryUpdateRequest:CategoryInsertRequest
    {
        public int CategoryId { get; set; }
    }
}