//using Sabio.Data;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Goal.Models.Domain.Reports;
using Sabio.Data;

namespace Goal.Services
{
    public class ReportService: BaseService
    {
        public static List<MonthlySpendingByCategoryDomain> GetMonthlySpendingByCategory(string userId)
        {
            var categories = new List<MonthlySpendingByCategoryDomain>();

            try
            {
                DataProvider.ExecuteCmd(GetConnection, "dbo.Report_GetMonthlySpendingByCategory",
                    inputParamMapper: delegate (SqlParameterCollection paramCollection)
                    {
                        paramCollection.AddWithValue("@userId", userId);
                    },
                    map: delegate (IDataReader reader, short set)
                    {
                        var category = new MonthlySpendingByCategoryDomain();
                        string forcastType;
                        double averageValue = 0;
                        double fixedValue = 0;

                        int startingIndex = 0;

                        category.CategoryId = reader.GetSafeInt32(startingIndex++);
                        category.CategoryName = reader.GetSafeString(startingIndex++);
                        category.Real = reader.GetSafeDouble(startingIndex++);
                        // Determine which category forcast to use
                        forcastType = reader.GetSafeString(startingIndex++);
                        averageValue = reader.GetSafeInt32(startingIndex++);
                        fixedValue = reader.GetSafeInt32(startingIndex++);

                        category.Predicted = forcastType == "Average" ? averageValue : fixedValue;
                        

                        categories.Add(category);
                    });
            }
            catch (Exception e)
            {
                throw e;
            }

            return categories;
        }
    }
}