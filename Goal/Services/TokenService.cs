using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Goal.Services
{
    public class TokenService : BaseService
    {
        public static Guid CreateToken(String UserId)
        {
            Guid token = Guid.Empty;

            try
            {
                DataProvider.ExecuteNonQuery(GetConnection, "dbo.Token_insert",
                    inputParamMapper: delegate (SqlParameterCollection paramCollection)
                    {
                        paramCollection.AddWithValue("@UserId", UserId);

                        SqlParameter p = new SqlParameter("@Token", System.Data.SqlDbType.UniqueIdentifier);
                        p.Direction = System.Data.ParameterDirection.Output;

                        paramCollection.Add(p);

                    }, returnParameters: delegate (SqlParameterCollection param)
                    {
                        Guid.TryParse(param["@Token"].Value.ToString(), out token);
                    });

            }
            catch (Exception e)
            {
                throw e;
            }

            return token;
        }


        public static bool IsValidToken(string userId, Guid token)
        {
            bool isValid = false;

            try
            {
                DataProvider.ExecuteNonQuery(GetConnection, "dbo.Token_Validate_ByUserId",
                    inputParamMapper: delegate (SqlParameterCollection paramCollection)
                    {
                        paramCollection.AddWithValue("@UserId", userId);
                        paramCollection.AddWithValue("@Token", token);

                        SqlParameter p = new SqlParameter("@isValid", System.Data.SqlDbType.Bit);
                        p.Direction = System.Data.ParameterDirection.Output;

                        paramCollection.Add(p);

                    }, returnParameters: delegate (SqlParameterCollection param)
                    {
                        bool.TryParse(param["@isValid"].Value.ToString(), out isValid);
                    });
            }
            catch(Exception e)
            {
                throw e;
            }
            return isValid;
        }
    }
}