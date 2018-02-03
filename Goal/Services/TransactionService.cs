using Goal.Models.Domain;
using Goal.Models.Requests;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Goal.Services
{
    public class TransactionService: BaseService
    {
        public static bool InsertNewTransaction(TransactionInsertRequest transaction)
        {
            bool isSuccess = false;
            try
            {
                string userId = UserService.GetCurrentUserId();
                LogService.TopOffLogToDate(userId, transaction.Date);

                Data.InsertTransaction(transaction);

                // Past-Month Transaction
                if (!DateFallsInCurrentMonth(transaction.Date))
                {
                    double netImpactOnBalance = transaction.TypeId == Enums.TransactionType.Credit ?
                        transaction.Amount : (transaction.Amount * -1);

                    LogService.BalanceLogForPastTransaction(userId, transaction.Date, netImpactOnBalance);
                }

                LogService.RefreshCurrentMonthBalance(userId);

                isSuccess = true;
            }
            catch(Exception e)
            {
                throw e;
            }

            return isSuccess;

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



       



        public static class Data
        {
            // --| Transactions |------------------------------------------------------------------>

            public static int InsertTransaction(TransactionInsertRequest model)
            {
                int transactionId = 0;

                try
                {
                    DataProvider.ExecuteNonQuery(GetConnection, "dbo.insert_transaction",
                        inputParamMapper: delegate (SqlParameterCollection paramCollection)
                        {
                            paramCollection.AddWithValue("@amount", model.Amount);
                            paramCollection.AddWithValue("@categoryId", model.CategoryId);
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
        }
    }
}