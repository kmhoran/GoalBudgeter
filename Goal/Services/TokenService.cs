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
    }
}