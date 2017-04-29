
using Goal.Enums;
using Goal.Models.Requests;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Goal.Services
{
    public class LogService : BaseService
    {

        public static int InsertTransaction(TransactionRequestModel model)
        {
            int id = 0;

            try
            {
                DataProvider.ExecuteNonQuery(GetConnection, "dbo.insert_transaction",
                    inputParamMapper: delegate (SqlParameterCollection paramCollection)
                    {
                        paramCollection.AddWithValue("@amount", model.Amount);
                        paramCollection.AddWithValue("@category", model.Category);
                        paramCollection.AddWithValue("@date", model.Date);
                        paramCollection.AddWithValue("@description", model.Description);
                        paramCollection.AddWithValue("@typeId", model.TypeId);
                        paramCollection.AddWithValue("@userId", model.UserId);

                        var p = new SqlParameter("@transactionId", System.Data.SqlDbType.Int);
                        p.Direction = System.Data.ParameterDirection.Output;

                        paramCollection.Add(p);

                    }, returnParameters: delegate (SqlParameterCollection param)
                    {
                        int.TryParse(param["@transactionId"].Value.ToString(), out id);
                    });
            }
            catch (Exception e)
            {
                throw e;
            }

            return id;

        }


        // .........................................................................................

        static bool InsertCategoriesByTable(DataTable categoryTable)
        {
            bool isSuccess = false;

            try
            {
                DataProvider.ExecuteNonQuery(GetConnection, "dbo.insert_category_byTable",
                    inputParamMapper: delegate (SqlParameterCollection paramCollection)
                    {
                        paramCollection.AddWithValue("@categoryTable", categoryTable);

                        isSuccess = true;
                    });
            }
            catch (Exception e)
            {
                throw e;
            }

            return isSuccess;
        }


        // .........................................................................................

        public static bool InsertDefaultCategories(string userId)
        {
            var defaultCategories = new Dictionary<string, TransactionType>();

            // Set Default Categories
            defaultCategories.Add("Food", TransactionType.Debit);
            defaultCategories.Add("Medical", TransactionType.Debit);
            defaultCategories.Add("Necessities", TransactionType.Debit);
            defaultCategories.Add("Transportation", TransactionType.Debit);
            defaultCategories.Add("Personal", TransactionType.Debit);
            defaultCategories.Add("Fun", TransactionType.Debit);
            defaultCategories.Add("Utilities", TransactionType.Debit);
            defaultCategories.Add("Vacation", TransactionType.Debit);
            defaultCategories.Add("Rent", TransactionType.Debit);
            defaultCategories.Add("Other Expenses", TransactionType.Debit);
            defaultCategories.Add("Paycheck", TransactionType.Credit);
            defaultCategories.Add("Gifts", TransactionType.Credit);
            defaultCategories.Add("Other Income", TransactionType.Credit);

            // Here we create a CategoryTable with four columns.
            DataTable table = new DataTable();
            table.Columns.Add("CategoryId", typeof(int));
            table.Columns.Add("UserId", typeof(string));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("TransactionTypeId", typeof(int));


            // Add rows to Table
            int count = 0;
            foreach (var kvp in defaultCategories)
            {
                table.Rows.Add(count, userId, kvp.Key, (int)kvp.Value);
                count += 1;
            }

            // Send User's default categories to the DB
            bool isSuccess = InsertCategoriesByTable(table);

            return isSuccess;
        }
    }

}