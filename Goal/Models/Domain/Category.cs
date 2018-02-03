using Goal.Enums;
using Goal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Goal.Models.Domain
{
    public class Category
    {

        public Category(string userId)
        {
            Categories = LogService.Data.GetUserCategories(userId);
        }


        public CategoryCollectionDomain ToDTO()
        {
            return Categories;
        }

        public CategoryCollectionDomain Categories { get; private set; }
    }
}