using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sabio.Web.Models.Responses
{
    public abstract class BaseResponseModel
    {
        public bool IsSuccessful { get; set; }

        public string TransactionId { get; set; }

        public BaseResponseModel()
        {
            //CrudApp: This TxId we are just faking to demo the purpose
            this.TransactionId = Guid.NewGuid().ToString();
        }
    }
}