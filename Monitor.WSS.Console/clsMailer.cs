using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Monitor.WSS.Console
{
    static class clsMailer
    {
        public static bool SendMail()
        {
            SmtpClient s = new SmtpClient();
            s.Host = "1";
            
            return true;
        }
    }
}
