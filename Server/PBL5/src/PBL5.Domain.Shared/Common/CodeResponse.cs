using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PBL5.Common
{
    public class CodeResponse
	{
        public static readonly int OK = 200;
        public static readonly int SERVER_ERROR = 500;
        public static readonly int NOT_FOUND = 404;
        public static readonly int NOT_LOGIN = 401;
        public static readonly int NOT_ACCESS = 403;
        public static readonly int NOT_VALIDATE = 201;
        public static readonly int HAVE_ERROR = 202;
        public static readonly int INFO = 100;
    }
}