
using Goal.Enums;
using Goal.Models;
using Goal.Models.Domain;
using Goal.Models.Requests;
using MoreLinq;
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
    public class LogService
    {
        public static LogStatusType GetStatusForCurrentMonth()
        {
            string userId = UserService.GetCurrentUserId();

            return Data.GetLogStatus(userId);
        }



        public static Month GetCurrentMonth()
        {
            try
            {
                string userId = UserService.GetCurrentUserId();

                return Log.GetCurrentMonth(userId);
            }
            catch(Exception e)
            {
                throw e;
            }
        }


        public static bool CreateNewLog(NewUserLogInsertRequest newLogRequest)
        {
            bool isSuccess = false;

            try
            {
                string userId = UserService.GetCurrentUserId();

                Log.CreateNewLog(userId, newLogRequest);

                isSuccess = true;
            }
            catch (Exception e)
            {
                throw e;
            }

            return isSuccess;

        }




        public static bool SetNewYearsGoal(NewUserLogInsertRequest newLogRequest)
        {
            bool isSuccess = false;
            try
            {
                string userId = UserService.GetCurrentUserId();

                Data.UpdateYearGoalPreferences(newLogRequest);
                DateTime decemberFirst = GetYearStartOfDate(DateTime.Today).AddMonths(-1);

                TopOffLogToDate(userId, decemberFirst);

                isSuccess = true;
            }
            catch (Exception e)
            {
                throw e;
            }

            return isSuccess;
        }



        public static bool TopOffLogToDate(string userId, DateTime date)
        {
            bool isSuccess = false;

            try
            {
                Month firstMonth = GetMonthZeroForUser(userId);
                FillGapBetweenMonthZeroAndDate(userId, firstMonth, date);
                isSuccess = true;
            }
            catch (Exception e)
            {
                throw e;
            }

            return isSuccess;
        }



        private static void FillGapBetweenMonthZeroAndDate(string userId, Month monthZero, DateTime date)
        {
            try
            {
                if (!UserOwnsThisMonth(userId, monthZero.MonthId))
                    throw new ApplicationException("User is not authorized to edit this log");


                DateTime targetDate = GetMonthStartOfDate(date);

                int indexIncrement;
                int dateDiff = DateDiff.Days(monthZero.StartDate, targetDate);

                if (dateDiff == 0)
                    return;
                else if (dateDiff > 0)
                    indexIncrement = 1;
                else
                    indexIncrement = -1;

                var newDbMonthList = new List<DbMonth>();

                if (indexIncrement > 0)
                {
                    DbMonth newestDbMonthToDate = Data.GetNewestDbMonthOnFile(userId);

                    // Verify newestMonthToDate is not the current Month
                    if (DateFallsInCurrentMonth(newestDbMonthToDate.StartDate))
                        return;

                    if (DateDiff.DoDatesSpanFromLastYearToThisYear(newestDbMonthToDate.StartDate, targetDate))
                        throw new ApplicationException("The bridge from last year to this year should not be auto-populated.");

                    DbMonth newDbMonthToAdd = Log.CreateSuccessorDbMonth(userId, newestDbMonthToDate);

                    while (DateDiff.Days(newDbMonthToAdd.StartDate, targetDate) <= 0)
                    {
                        newDbMonthList.Add(newDbMonthToAdd);
                        newDbMonthToAdd = Log.CreateSuccessorDbMonth(userId, newDbMonthToAdd);
                    }

                    Data.InsertListOfDbMonths(newDbMonthList);
                }
                else
                {
                    DbMonth earliestDbMonthToDate = Data.GetEarliestMonthToDate(userId);

                    DbMonth newDbMonthToAdd = Log.CreatePredecessorDbMonth(userId, earliestDbMonthToDate);

                    while (DateDiff.Days(targetDate, newDbMonthToAdd.StartDate) <= 0)
                    {
                        newDbMonthList.Add(newDbMonthToAdd);
                        newDbMonthToAdd = Log.CreatePredecessorDbMonth(userId, newDbMonthToAdd);
                    }

                    Data.InsertListOfDbMonths(newDbMonthList);
                }


            }
            catch (Exception e)
            {
                throw e;
            }
        }



        public static void RefreshCurrentMonthBalance(string userId)
        {
            try
            {
                Data.RefreshCurrentMonthBalance(userId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        public static void BalanceLogForPastTransaction(
            string userId,
            DateTime transactionDate, 
            double netImpactOnBalance)
        {
            try
            {
                UpdateLogBeforeAndUpToTransactionDate(userId, transactionDate, netImpactOnBalance);

                UpdateLogAfterTransactionDate(userId, transactionDate, netImpactOnBalance);
            }
            catch(Exception e)
            {
                throw e;
            }
        }


        private static void UpdateLogBeforeAndUpToTransactionDate(
            string userId, 
            DateTime date, 
            double netImpactOnBalance)
        {
            try
            {
                if (Data.DoesConfirmedMonthStartAmountExistOnOrBeforeDate(userId, date))
                    return;

                List<DbMonth> unconfirmedMonths = Data.GetUnconfirmedMonthsOnOrBeforeDate(userId, date);

                var updateList = new List<DbMonth>();

                double updatedNewYearStartAmount = 0;


                foreach (var dbMonth in unconfirmedMonths)
                {
                    var monthToUpdate = new DbMonth(
                        monthId: dbMonth.MonthId,
                        userId: dbMonth.UserId,
                        startDate: dbMonth.StartDate,
                        grossCredits: dbMonth.GrossCredits,
                        grossDebits: dbMonth.GrossDebits,
                        startAmount: dbMonth.StartAmount + netImpactOnBalance,
                        previousMonthId: dbMonth.PreviousMonthId,
                        isStartAmountConfirmed: dbMonth.IsStartAmountConfirmed);

                    updateList.Add(monthToUpdate);

                    if (DateFallsInJanuaryOfThisYear(monthToUpdate.StartDate))
                        updatedNewYearStartAmount = monthToUpdate.StartAmount;
                }

                Data.UpdateDbMonthCollection(updateList);

                if (updatedNewYearStartAmount != 0)
                    Data.UpdateYearStartAmount(userId, updatedNewYearStartAmount);

            }
            catch(Exception e)
            {
                throw e;
            }

        }



        private static void UpdateLogAfterTransactionDate(
            string userId,
            DateTime date,
            double netImpactOnBalance)
        {
            try
            {
                List<DbMonth> unconfirmedMonths = Data.GetUnconfirmedMonthsAfterDate(userId, date);

                var updateList = new List<DbMonth>();

                double updatedNewYearStartAmount = 0;

                foreach (var dbMonth in unconfirmedMonths)
                {
                    if (dbMonth.IsStartAmountConfirmed)
                        throw new ApplicationException("Unexpected value: DbMonth.StartAmount is confirmed!");

                    var monthToUpdate = new DbMonth(
                        monthId: dbMonth.MonthId,
                        userId: dbMonth.UserId,
                        startDate: dbMonth.StartDate,
                        grossCredits: dbMonth.GrossCredits,
                        grossDebits: dbMonth.GrossDebits,
                        startAmount: dbMonth.StartAmount + netImpactOnBalance,
                        previousMonthId: dbMonth.PreviousMonthId,
                        isStartAmountConfirmed: dbMonth.IsStartAmountConfirmed);

                    updateList.Add(monthToUpdate);

                    if (DateFallsInJanuaryOfThisYear(monthToUpdate.StartDate))
                        updatedNewYearStartAmount = monthToUpdate.StartAmount;
                }

                Data.UpdateDbMonthCollection(updateList);


                if (updatedNewYearStartAmount != 0)
                    Data.UpdateYearStartAmount(userId, updatedNewYearStartAmount);


                if (updateList.Count > 0)
                {
                    DbMonth lastMonth = updateList.MaxBy(m => m.StartDate);

                    string AdjustmentNote = String.Format("Transaction on {0} for $({1})", date, netImpactOnBalance);

                    Data.AddAdjustment(userId, lastMonth.MonthId, netImpactOnBalance, AdjustmentNote);
                }
                else
                {
                    DbMonth transactionMonth = Data.GetDbMonthByDate(userId, date);

                    string AdjustmentNote = String.Format("Transaction on {0} for $({1})", date, netImpactOnBalance);

                    Data.AddAdjustment(userId, transactionMonth.MonthId, netImpactOnBalance, AdjustmentNote);
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }


        public static class DateDiff
        {
            public static int Days(DateTime dateOne, DateTime dateTwo)
            {
                return (dateOne - dateTwo).Days;
            }

            public static int Years(DateTime dateOne, DateTime dateTwo)
            {
                double Years = Math.Round(((dateOne - dateTwo).Days) / 365.25);
                return (int)Years;
            }
            public static bool DoDatesSpanFromLastYearToThisYear(DateTime dateOne, DateTime dateTwo)
            {
                DateTime januaryFirstOfThisYear = GetYearStartOfDate(DateTime.Today);
                bool areWeTalkingAboutThisYear = (DateDiff.Years(dateOne, januaryFirstOfThisYear) == 0)
                    || (DateDiff.Years(dateTwo, januaryFirstOfThisYear) == 0);

                return (DateDiff.Years(dateOne, dateTwo) != 0 && areWeTalkingAboutThisYear);
            }
        }



        public static bool DateFallsInCurrentMonth(DateTime date)
        {
            return DateDiff.Days(GetMonthStartOfDate(DateTime.Today), GetMonthStartOfDate(date)) == 0;
        }



        public static DateTime GetMonthStartOfDate(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }



        public static DateTime GetYearStartOfDate(DateTime date)
        {
            return new DateTime(date.Year, 1, 1);
        }


        public static bool DateFallsInJanuaryOfThisYear(DateTime date)
        {
            return DateDiff.Days(GetYearStartOfDate(DateTime.Today), GetMonthStartOfDate(date)) == 0;
        }


        public static Month GetMonthZeroForUser(string userId)
        {
            DbMonth dbMonth = Data.GetDbMonthZeroForUser(userId);
            return Log.RenderMonth(dbMonth);
        }



        public static Month GetNewestMonthOnFile(string userId)
        {
            DbMonth dbMonth = Data.GetNewestDbMonthOnFile(userId);
            return Log.RenderMonth(dbMonth);
        }




        public static Month GetMonthByUserAndMonthId(string userId, int monthId)
        {
            DbMonth dbMonth = Data.GetDbMonthById(userId, monthId);
            return Log.RenderMonth(dbMonth);
        }

        public static Month GetMonthByUserIdAndDate(string userId, DateTime date)
        {
            try
            {
                DateTime monthStart = GetMonthStartOfDate(date);
                DbMonth dbMonth = Data.GetDbMonthByDate(userId, monthStart);
                return Log.RenderMonth(dbMonth);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public static bool UserOwnsThisMonth(string userId, int monthId)
        {
            List<DbMonth> dbMonthList = Data.GetDbMonthsByUserId(userId);

            foreach (var dbMonth in dbMonthList)
            {
                if (dbMonth.MonthId == monthId)
                    return true;
            }

            return false;
        }

        // --| Database Access |------------------------------------------------------------------->
        public class Data : BaseService
        {

            // --| Categories |-------------------------------------------------------------------->

            public static bool SetDefaultCategories(string userId)
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






            public static CategoryCollectionDomain GetUserCategories(string userId)
            {
                var credits = new List<CategoryDomain>();
                var debits = new List<CategoryDomain>();
                var collection = new CategoryCollectionDomain { Credits = credits, Debits = debits };

                try
                {
                    DataProvider.ExecuteCmd(GetConnection, "dbo.select_category_byUserId",
                        inputParamMapper: delegate (SqlParameterCollection paramCollection)
                        {
                            paramCollection.AddWithValue("@userId", userId);
                        },
                        map: delegate (IDataReader reader, short set)
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

                            if (category.TypeId == TransactionType.Credit)
                            {
                                collection.Credits.Add(category);
                            }
                            else
                            {
                                collection.Debits.Add(category);
                            }
                        });
                }
                catch (Exception e)
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
                        inputParamMapper: delegate (SqlParameterCollection paramCollection)
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
                catch (Exception e)
                {
                    throw e;
                }

                return categoryId;
            }


            // .........................................................................................

            public static bool InsertCategoriesByTable(DataTable categoryTable)
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

            public static bool UpdateCategory(CategoryUpdateRequest model)
            {
                bool isSuccess = false;

                try
                {
                    DataProvider.ExecuteNonQuery(GetConnection, "dbo.Update_Category",
                        inputParamMapper: delegate (SqlParameterCollection paramCollection)
                        {
                            paramCollection.AddWithValue("@CategoryId", model.CategoryId);
                            paramCollection.AddWithValue("@UserId", model.UserId);
                            paramCollection.AddWithValue("@ForcastType", model.ForecastType);
                            paramCollection.AddWithValue("@FixedPrediction", model.FixedPrediction);

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

            public static bool UpdateCategoryCollection(CategoriesUpdateRequest model, string userId)
            {
                bool isSuccess = false;
                if (model.CategoryList != null && model.CategoryList.Count != 0)
                {
                    foreach (CategoryUpdateRequest category in model.CategoryList)
                    {
                        try
                        {
                            category.UserId = userId;
                            UpdateCategory(category);
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                    }

                    isSuccess = true;
                }

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


                // Updated sept-04-17
            public static int InsertDbMonth(DbMonth model)
            {
                int monthId = 0;
                try
                {
                    DataProvider.ExecuteNonQuery(GetConnection, "dbo.sp_Month_Insert",
                        inputParamMapper: delegate (SqlParameterCollection paramCollection)
                        {
                            paramCollection.AddWithValue("@userId", model.UserId);
                            paramCollection.AddWithValue("@startDate", model.StartDate);
                            paramCollection.AddWithValue("@grossCredits", model.GrossCredits);
                            paramCollection.AddWithValue("@grossDebits", model.GrossDebits);
                            paramCollection.AddWithValue("@startAmount", model.StartAmount);
                            paramCollection.AddWithValue("@previousMonthId", model.PreviousMonthId);
                            paramCollection.AddWithValue("@isStartAmountConfirmed", model.IsStartAmountConfirmed);

                            var p = new SqlParameter("@monthId", System.Data.SqlDbType.Int);
                            p.Direction = System.Data.ParameterDirection.Output;

                            paramCollection.Add(p);
                        },
                        returnParameters: delegate (SqlParameterCollection param)
                        {
                            int.TryParse(param["@monthId"].Value.ToString(), out monthId);
                        });

                }
                catch (Exception e)
                {
                    throw e;
                }

                return monthId;
            }

            // updated sept-04-17
            public static DbMonth GetDbMonthByDate(string userId, DateTime date)
            {
                var dbMonth = new DbMonth();
                try
                {
                    DataProvider.ExecuteCmd(GetConnection, "dbo.sp_Month_Select_ByDate",
                        inputParamMapper: delegate (SqlParameterCollection paramCollection)
                        {
                            paramCollection.AddWithValue("@userId", userId);
                            paramCollection.AddWithValue("@date", date);
                        },
                        map: delegate (IDataReader reader, short set)
                        {
                            int startingIndex = 0;

                            dbMonth = new DbMonth(
                                monthId: reader.GetSafeInt32(startingIndex++),
                                userId: reader.GetSafeString(startingIndex++),
                                startDate: reader.GetSafeUtcDateTime(startingIndex++),
                                grossCredits: reader.GetSafeDouble(startingIndex++),
                                grossDebits: reader.GetSafeDouble(startingIndex++),
                                startAmount: reader.GetSafeDouble(startingIndex++),
                                previousMonthId: reader.GetSafeInt32(startingIndex++),
                                isStartAmountConfirmed: reader.GetSafeBool(startingIndex++));
                        });
                }
                catch (Exception e)
                {
                    throw e;
                }

                return dbMonth;
            }




            public static DbMonth GetDbMonthById(string userId, int monthId)
            {
                var dbMonth = new DbMonth();
                try
                {
                    DataProvider.ExecuteCmd(GetConnection, "dbo.sp_Month_Select_ByMonthId",
                        inputParamMapper: delegate (SqlParameterCollection paramCollection)
                        {
                            paramCollection.AddWithValue("@userId", userId);
                            paramCollection.AddWithValue("@monthId", monthId);
                        },
                        map: delegate (IDataReader reader, short set)
                        {
                            int startingIndex = 0;

                            dbMonth = new DbMonth(
                                monthId: reader.GetSafeInt32(startingIndex++),
                                userId: reader.GetSafeString(startingIndex++),
                                startDate: reader.GetSafeUtcDateTime(startingIndex++),
                                grossCredits: reader.GetSafeDouble(startingIndex++),
                                grossDebits: reader.GetSafeDouble(startingIndex++),
                                startAmount: reader.GetSafeDouble(startingIndex++),
                                previousMonthId: reader.GetSafeInt32(startingIndex++),
                                isStartAmountConfirmed: reader.GetSafeBool(startingIndex++));
                        });
                }
                catch (Exception e)
                {
                    throw e;
                }

                return dbMonth;
            }



            // updated sept-04-17
            public static List<DbMonth> GetDbMonthsByUserId(string userId)
            {
                var dbMonths = new List<DbMonth>();
                try
                {
                    try
                    {
                        DataProvider.ExecuteCmd(GetConnection, "dbo.sp_Month_Select_ByMonthId",
                            inputParamMapper: delegate (SqlParameterCollection paramCollection)
                            {
                                paramCollection.AddWithValue("@userId", userId);
                            },
                            map: delegate (IDataReader reader, short set)
                            {
                                int startingIndex = 0;

                                var newDbMonth = new DbMonth(
                                    monthId: reader.GetSafeInt32(startingIndex++),
                                    userId: reader.GetSafeString(startingIndex++),
                                    startDate: reader.GetSafeUtcDateTime(startingIndex++),
                                    grossCredits: reader.GetSafeDouble(startingIndex++),
                                    grossDebits: reader.GetSafeDouble(startingIndex++),
                                    startAmount: reader.GetSafeDouble(startingIndex++),
                                    previousMonthId: reader.GetSafeInt32(startingIndex++),
                                    isStartAmountConfirmed: reader.GetSafeBool(startingIndex++));

                                dbMonths.Add(newDbMonth);

                            });
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }

                return dbMonths;
            }



            // updated sept-04-17
            public static DbMonth GetNewestDbMonthOnFile(string userId)
            {
                var dbMonth = new DbMonth();
                try
                {
                    DataProvider.ExecuteCmd(GetConnection, "dbo.sp_Month_Select_LatestOnRecordForUser",
                        inputParamMapper: delegate (SqlParameterCollection paramCollection)
                        {
                            paramCollection.AddWithValue("@userId", userId);
                        },
                        map: delegate (IDataReader reader, short set)
                        {
                            int startingIndex = 0;

                            dbMonth = new DbMonth(
                                monthId: reader.GetSafeInt32(startingIndex++),
                                userId: reader.GetSafeString(startingIndex++),
                                startDate: reader.GetSafeUtcDateTime(startingIndex++),
                                grossCredits: reader.GetSafeDouble(startingIndex++),
                                grossDebits: reader.GetSafeDouble(startingIndex++),
                                startAmount: reader.GetSafeDouble(startingIndex++),
                                previousMonthId: reader.GetSafeInt32(startingIndex++),
                                isStartAmountConfirmed: reader.GetSafeBool(startingIndex++));
                        });
                }
                catch (Exception e)
                {
                    throw e;
                }

                return dbMonth;
            }



            public static bool InsertListOfDbMonths(List<DbMonth> dbMonthList)
            {
                bool isSuccess = false;
                try
                {
                    throw new NotImplementedException();
                }
                catch (Exception e)
                {
                    throw e;
                }

                return isSuccess;
            }



            public static DbMonth GetEarliestMonthToDate(string userId)
            {
                var dbMonth = new DbMonth();
                try
                {
                    DataProvider.ExecuteCmd(GetConnection, "dbo.sp_Month_Select_EarliestOnRecordForUser",
                        inputParamMapper: delegate (SqlParameterCollection paramCollection)
                        {
                            paramCollection.AddWithValue("@userId", userId);
                        },
                        map: delegate (IDataReader reader, short set)
                        {
                            int startingIndex = 0;

                            dbMonth = new DbMonth(
                                monthId: reader.GetSafeInt32(startingIndex++),
                                userId: reader.GetSafeString(startingIndex++),
                                startDate: reader.GetSafeUtcDateTime(startingIndex++),
                                grossCredits: reader.GetSafeDouble(startingIndex++),
                                grossDebits: reader.GetSafeDouble(startingIndex++),
                                startAmount: reader.GetSafeDouble(startingIndex++),
                                previousMonthId: reader.GetSafeInt32(startingIndex++),
                                isStartAmountConfirmed: reader.GetSafeBool(startingIndex++));
                        });
                }
                catch (Exception e)
                {
                    throw e;
                }

                return dbMonth;
            }



            // updated sept-04-17
            public static DbMonth GetDbMonthZeroForUser(string userId)
            {
                var dbMonth = new DbMonth();
                try
                {
                    DataProvider.ExecuteCmd(GetConnection, "dbo.sp_Months_Select_FirstDateCreatedForUser",
                        inputParamMapper: delegate (SqlParameterCollection paramCollection)
                        {
                            paramCollection.AddWithValue("@userId", userId);
                        },
                        map: delegate (IDataReader reader, short set)
                        {
                            int startingIndex = 0;

                            dbMonth = new DbMonth(
                                monthId: reader.GetSafeInt32(startingIndex++),
                                userId: reader.GetSafeString(startingIndex++),
                                startDate: reader.GetSafeUtcDateTime(startingIndex++),
                                grossCredits: reader.GetSafeDouble(startingIndex++),
                                grossDebits: reader.GetSafeDouble(startingIndex++),
                                startAmount: reader.GetSafeDouble(startingIndex++),
                                previousMonthId: reader.GetSafeInt32(startingIndex++),
                                isStartAmountConfirmed: reader.GetSafeBool(startingIndex++));
                        });
                }
                catch (Exception e)
                {
                    throw e;
                }

                return dbMonth;
            }



            public static void RefreshCurrentMonthBalance(string userId)
            {
                try
                {
                    throw new NotImplementedException();
                }
                catch(Exception e)
                {
                    throw e;
                }
            }



            public static bool DoesConfirmedMonthStartAmountExistOnOrBeforeDate(string userId, DateTime date)
            {
                bool doesConfirmedAmountExist = false;

                try
                {
                    throw new NotImplementedException();
                }
                catch (Exception e)
                {
                    throw e;
                }

                return doesConfirmedAmountExist;
            }




            public static List<DbMonth> GetUnconfirmedMonthsOnOrBeforeDate(string userId, DateTime date)
            {
                var unconfirmedList = new List<DbMonth>();

                try
                {
                    throw new NotImplementedException();
                }
                catch (Exception e)
                {
                    throw e;
                }

                return unconfirmedList;
            }



            public static List<DbMonth> GetUnconfirmedMonthsAfterDate(string userId, DateTime date)
            {
                var unconfirmedList = new List<DbMonth>();

                try
                {
                    throw new NotImplementedException();
                }
                catch (Exception e)
                {
                    throw e;
                }

                return unconfirmedList;
            }




            public static void UpdateDbMonthCollection(List<DbMonth> updateList)
            {
                try
                {
                    throw new NotImplementedException();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }


            // --| Year |------------------------------------------------------------------------------>

            public static bool InsertYear(string userId, DateTime startDate, double startAmount, int goalAmount)
            {
                bool isSuccess = false;
                try
                {
                    DataProvider.ExecuteNonQuery(GetConnection, "dbo.sp_Year_Insert",
                        inputParamMapper: delegate (SqlParameterCollection paramCollection)
                        {
                            paramCollection.AddWithValue("@userId", userId);
                            paramCollection.AddWithValue("@startingDate", startDate);
                            paramCollection.AddWithValue("@startingAmount", startAmount);
                            paramCollection.AddWithValue("@goal", goalAmount);

                            isSuccess = true;
                        });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }

                return isSuccess;
            }

            // --| Adjustments |------------------------------------------------------------------->


            public static void AddAdjustment(string userId, int monthId, double amount, string note)
            {
                try
                {
                    throw new NotImplementedException();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }


            public static List<double> GetMonthAdjustments(int monthId)
            {
                List<double> adjustments = new List<double>();
                try
                {
                    throw new NotImplementedException();
                }
                catch (Exception e)
                {
                    throw e;
                }

                return adjustments;
            }




            // --| User Log |-------------------------------------------------------------------------->
            public static bool InitializeUserLog(NewUserLogInsertRequest model)
            {
                bool isSuccess = false;
                //try
                //{
                //    DataProvider.ExecuteNonQuery(GetConnection, "dbo.Insert_FreshYearAndMonth",
                //        inputParamMapper: delegate (SqlParameterCollection paramCollection)
                //        {
                //            paramCollection.AddWithValue("@UserId", model.UserId);
                //            paramCollection.AddWithValue("@StartingDate", model.StartDate);
                //            paramCollection.AddWithValue("@StartingAmount", model.StartingAmount);
                //            paramCollection.AddWithValue("@YearGoal", model.GoalAmount);

                //            isSuccess = true;
                //        });
                //}
                //catch (Exception e)
                //{
                //    throw e;
                //}

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

            public static bool InsertUserPreferences(string userId, string monthGoalType, int monthFixedGoal)
            {
                bool isSuccess = false;
                try
                {
                    DataProvider.ExecuteNonQuery(GetConnection, "dbo.sp_Preferences_Insert",
                        inputParamMapper: delegate (SqlParameterCollection paramCollection)
                        {
                            paramCollection.AddWithValue("@userId", userId);
                            paramCollection.AddWithValue("@monthlyGoalType", monthGoalType);
                            paramCollection.AddWithValue("@monthlyFixedGoal", monthFixedGoal);

                            isSuccess = true;
                        });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }

                return isSuccess;
            }




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


            public static void UpdateYearGoalPreferences(NewUserLogInsertRequest newLogRequest)
            {
                throw new NotImplementedException();

            }



            public static DbPreferences GetPreferencesByUserId(string userId)
            {
                var dbPreferences = new DbPreferences();
                try
                {
                    DataProvider.ExecuteCmd(GetConnection, "dbo.sp_Month_Select_ByMonthId",
                        inputParamMapper: delegate (SqlParameterCollection paramCollection)
                        {
                            paramCollection.AddWithValue("@userId", userId);
                        },
                        map: delegate (IDataReader reader, short set)
                        {
                            int startingIndex = 0;

                            dbPreferences = new DbPreferences(
                                preferenceId: reader.GetSafeInt32(startingIndex++),
                                userId: reader.GetSafeString(startingIndex++),
                                monthGoalType: (MonthGoalType)reader.GetSafeInt32(startingIndex++),
                                monthFixedGoal: reader.GetSafeInt32(startingIndex++),
                                yearGoal: reader.GetSafeInt32(startingIndex++),
                                yearStartAmount: reader.GetSafeDouble(startingIndex++));
                        });
                }
                catch (Exception e)
                {
                    throw e;
                }

                return dbPreferences;
            }



            public static void UpdateYearStartAmount(string userId, double newStartAmount)
            {
                try
                {
                    throw new NotImplementedException();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }


        }

    }
}