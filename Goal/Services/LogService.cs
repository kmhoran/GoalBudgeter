
using Goal.Enums;
using Goal.Models.Domain;
using Goal.Models.Requests;
using Sabio.Data;
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
        // --| Transactions |---------------------------------------------------------------------->

        public static int InsertTransaction(TransactionInsertRequest model)
        {
            int transactionId = 0;

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
                        int.TryParse(param["@transactionId"].Value.ToString(), out transactionId);
                    });
            }
            catch (Exception e)
            {
                throw e;
            }

            return transactionId;

        }

        // --| Categories |------------------------------------------------------------------------>

        public static CategoryCollectionDomain GetUserCategories(string userId)
        {
            var credits = new List<CategoryDomain>();
            var debits = new List<CategoryDomain>();
            var collection = new CategoryCollectionDomain { Credits=credits, Debits=debits};

            try
            {
                DataProvider.ExecuteCmd(GetConnection, "dbo.select_category_byUserId",
                    inputParamMapper: delegate (SqlParameterCollection paramCollection)
                    {
                        paramCollection.AddWithValue("@userId", userId);
                    }, 
                    map: delegate(IDataReader reader, short set)
                    {
                        var category = new CategoryDomain();
                        var predictions = new PredictionCollectionDomain();
                        int startingIndex = 0;

                        category.CategoryId = reader.GetSafeInt32(startingIndex++);
                        category.Name = reader.GetSafeString(startingIndex++);
                        category.TypeId = (TransactionType)reader.GetSafeInt32(startingIndex++);
                        category.ForecastType = reader.GetSafeString(startingIndex++);
                        predictions.Fixed = reader.GetSafeInt32(startingIndex++);
                        predictions.Average = reader.GetSafeInt32(startingIndex++);

                        category.Predictions = predictions;

                        if(category.TypeId == TransactionType.Credit)
                        {
                            collection.Credits.Add(category);
                        }
                        else
                        {
                            collection.Debits.Add(category);
                        }
                    });   
            }
            catch(Exception e )
            {
                throw e;
            }

            return collection;
        }



        // .........................................................................................

        public static int InsertCategory(CategoryInsertRequest model)
        {
            int categoryId = 0;

            try
            {
                DataProvider.ExecuteNonQuery(GetConnection, "dbo.insert_category",
                    inputParamMapper: delegate(SqlParameterCollection paramCollection)
                    {
                        paramCollection.AddWithValue("@userId", model.UserId);
                        paramCollection.AddWithValue("@name", model.Name);
                        paramCollection.AddWithValue("@transactionTypeId", (int)model.TypeId);
                        paramCollection.AddWithValue("@forecastType", model.ForecastType);
                        paramCollection.AddWithValue("@fixedPrediction", model.FixedPrediction);
                        var p = new SqlParameter("@categoryId", System.Data.SqlDbType.Int);
                        p.Direction = System.Data.ParameterDirection.Output;

                        paramCollection.Add(p);
                    },
                    returnParameters: delegate (SqlParameterCollection param)
                    {
                        int.TryParse(param["@categoryId"].Value.ToString(), out categoryId);
                    });
            }
            catch(Exception e)
            {
                throw e;
            }

            return categoryId;
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
            table.Columns.Add("ForecastType", typeof(string));
            table.Columns.Add("FixedPrediction", typeof(int));


            // Add rows to Table
            int count = 0;
            foreach (var kvp in defaultCategories)
            {
                table.Rows.Add(count, userId, kvp.Key, (int)kvp.Value, "Fixed", 20);
                count += 1;
            }

            // Send User's default categories to the DB
            bool isSuccess = InsertCategoriesByTable(table);

            return isSuccess;
        }


        // .........................................................................................

        public static bool UpdateCategory(CategoryDomain model)
        {
            bool isSuccess = false;

            try
            {
                DataProvider.ExecuteNonQuery(GetConnection, "dbo.Update_Category",
                    inputParamMapper:delegate( SqlParameterCollection paramCollection)
                    {
                        paramCollection.AddWithValue("@CategoryId", model.CategoryId);
                        paramCollection.AddWithValue("@ForcastType", model.ForecastType);
                        paramCollection.AddWithValue("@FixedPrediction", model.Predictions.Fixed);

                        isSuccess = true;
                    }
                    );
            }
            catch (Exception e)
            {
                throw e;
            }

            return isSuccess;   
        }

        

        // .........................................................................................

        public static bool UpdateCategoryCollection(CategoriesUpdateRequest model)
        {
            bool isSuccess = false;

            foreach(CategoryDomain category in model.CategoryList)
            {
                try
                {
                    UpdateCategory(category);
                }
                catch(Exception e)
                {
                    throw e;
                }
            }

            isSuccess = true;

            return isSuccess;
        }


        // .........................................................................................

        public static bool DeleteCategory(int categoryId)
        {
            bool isSuccess = false;

            try
            {
                DataProvider.ExecuteNonQuery(GetConnection, "dbo.Delete_Category",
                    inputParamMapper: delegate (SqlParameterCollection paramCollection)
                    {
                        paramCollection.AddWithValue("@CategoryId", categoryId);

                        isSuccess = true;
                    }
                    );
            }
            catch (Exception e)
            {
                throw e;
            }

            return isSuccess;
        }


        // --| Months |--------------------------------------------------------------------------->

        public static CurrentMonthDomain GetCurrentMonth(string userId)
        {
            var currentMonth = new CurrentMonthDomain();
            try
            {
                DataProvider.ExecuteCmd(GetConnection, "dbo.CurrentMonth_Calculate",
                    inputParamMapper: delegate (SqlParameterCollection paramCollection)
                    {
                        paramCollection.AddWithValue("@userId", userId);
                    },
                    map: delegate (IDataReader reader, short set)
                    {
                        int staringIndex = 0;
                        currentMonth.YearId = reader.GetSafeInt32(staringIndex++);
                        currentMonth.YearStart = reader.GetSafeDateTime(staringIndex++);
                        currentMonth.YearStartingAmount = reader.GetSafeDouble(staringIndex++);
                        currentMonth.YearGoal = reader.GetSafeInt32(staringIndex++);
                        currentMonth.Balance = reader.GetSafeDouble(staringIndex++);
                        currentMonth.YearTotalSaved = reader.GetSafeDouble(staringIndex++);
                        currentMonth.YearAmountToReachGoal = reader.GetSafeDouble(staringIndex++);
                        currentMonth.MonthId = reader.GetSafeInt32(staringIndex++);
                        currentMonth.MonthStart = reader.GetSafeDateTime(staringIndex++);
                        currentMonth.TotalSaved = reader.GetSafeDouble(staringIndex++);
                        currentMonth.MonthCredits = reader.GetSafeDouble(staringIndex++);
                        currentMonth.MonthDebits = reader.GetSafeDouble(staringIndex++);
                        currentMonth.MonthStartingAmount = reader.GetSafeDouble(staringIndex++);
                        currentMonth.ExpenseBudget = reader.GetSafeInt32(staringIndex++);
                        currentMonth.IncomeBudget = reader.GetSafeInt32(staringIndex++);
                        currentMonth.PreferenceId = reader.GetSafeInt32(staringIndex++);
                        currentMonth.GoalType = reader.GetSafeString(staringIndex++);
                        currentMonth.FixedMonthGoal = reader.GetSafeInt32(staringIndex++);
                        currentMonth.ToReachFixedGoal = reader.GetSafeDouble(staringIndex++);
                        currentMonth.CalculatedMonthGoal = reader.GetSafeInt32(staringIndex++);
                        currentMonth.ToReachCalculatedGoal = reader.GetSafeDouble(staringIndex++);
                       
                    });
            }
            catch (Exception e)
            {
                throw e;
            }

            return currentMonth;
        }


        // --| User Log |-------------------------------------------------------------------------->
        public static bool InitializeUserLog(NewUserLogInsertRequest model)
        {
            bool isSuccess = false;
            try
            {
                DataProvider.ExecuteNonQuery(GetConnection, "dbo.Insert_FreshYearAndMonth",
                    inputParamMapper: delegate (SqlParameterCollection paramCollection)
                    {
                        paramCollection.AddWithValue("@UserId", model.UserId);
                        paramCollection.AddWithValue("@StartingDate", model.StartDate);
                        paramCollection.AddWithValue("@StartingAmount", model.StartingAmount);
                        paramCollection.AddWithValue("@YearGoal", model.GoalAmount);

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

        public static LogStatusType GetLogStatus(string userId)
        {
            LogStatusType logStatus = new LogStatusType();

            try
            {
                DataProvider.ExecuteNonQuery(GetConnection, "dbo.LogStatus_GetStatus",
                    inputParamMapper: delegate (SqlParameterCollection paramCollection)
                    {
                        paramCollection.AddWithValue("@UserId", userId);

                        var p = new SqlParameter("@LogStatus", System.Data.SqlDbType.Int);
                        p.Direction = System.Data.ParameterDirection.Output;

                        paramCollection.Add(p);
                    },
                    returnParameters: delegate (SqlParameterCollection param)
                    {
                        LogStatusType.TryParse(param["@LogStatus"].Value.ToString(), out logStatus);
                    });

            
            }
            catch
            {
                logStatus = LogStatusType.Error;
            }

            return logStatus;
        }


        // --| Preferences |----------------------------------------------------------------------->

        public static bool UpdateUserPreferences(PreferencceUpdateRequest model)
        {
            bool isSuccess = false;

            try
            {
                DataProvider.ExecuteNonQuery(GetConnection, "dbo.Preferences_Update",
                    inputParamMapper: delegate (SqlParameterCollection paramCollection)
                    {
                        paramCollection.AddWithValue("@PreferenceId", model.PreferenceId);
                        paramCollection.AddWithValue("@MonthlyGoalType", model.MonthlyGoalType);
                        paramCollection.AddWithValue("@MonthlyFixedGoal", model.MonthlyFixedGoal);

                        isSuccess = true;
                    }
                    );
            }
            catch (Exception e)
            {
                isSuccess = false;
            }

            return isSuccess;
        }
    }
}