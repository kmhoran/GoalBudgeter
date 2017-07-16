using Sabio.Data;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Goal.Models.Domain.Reports;

namespace Goal.Services
{
    public class ReportService
    {
        //public static List<MonthlySpendingByCategoryDomain> GetMonthlySpendingByCategory()
        //{
        //    var categories = new List<MonthlySpendingByCategoryDomain>();
        //    //var debits = new List<CategoryDomain>();
        //    //var collection = new CategoryCollectionDomain { Credits = credits, Debits = debits };

        //    try
        //    {
        //        DataProvider.ExecuteCmd(GetConnection, "dbo.select_category_byUserId",
        //            inputParamMapper: delegate (SqlParameterCollection paramCollection)
        //            {
        //                paramCollection.AddWithValue("@userId", userId);
        //            },
        //            map: delegate (IDataReader reader, short set)
        //            {
        //                var category = new MonthlySpendingByCategoryDomain();
        //                int startingIndex = 0;

        //                category.CategoryId = reader.GetSafeInt32(startingIndex++);
        //                category.CategoryName = reader.GetSafeString(startingIndex++);
        //                category.Predicted = reader.GetSafeDouble(startingIndex++);
        //                category.Real = reader.GetSafeDouble(startingIndex++);

        //                categories.Add(category);
        //            });
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }

        //    return categories;
        //}
    }
}