
using Goal.Models.Requests;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Goal.Services
{
    public class LogService: BaseService
    {

        public static int InsertTransaction (TransactionRequestModel model)
        {
            int id = 0;

            try
            {
                DataProvider.ExecuteNonQuery(GetConnection, "dbo.insert_transaction",
                    inputParamMapper: delegate(SqlParameterCollection paramCollection)
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
    }
}