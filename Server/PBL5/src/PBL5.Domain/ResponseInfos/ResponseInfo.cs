using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PBL5.Common;

namespace PBL5.ResponseInfos
{
    public class ResponseInfo
    {
        public int Code { get; set; }
        public string Message { get; set; }

        public ResponseInfo()
        {
            Code = CodeResponse.OK;
            Message = "";
        }
    }
}