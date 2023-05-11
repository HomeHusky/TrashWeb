using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trashwebWinForm.Common.Const
{
    public class CONSTANTS
    {
        public static string CONNECTIONSTRING = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
    }
}
