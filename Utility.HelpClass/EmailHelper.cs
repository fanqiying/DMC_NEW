using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Configuration;

namespace Utility.HelpClass
{
	/// <summary>
	/// 邮件公共操作类
	/// 主要用于邮件发送
	/// </summary>
    public static class EmailHelper
    {
        private static string m_EmailServer = ConfigurationManager.AppSettings[""].ToString();
        public static void SendEmail(string toMail, string subject, string body)
        {
            try
            {
                MailMessage mailobj = new MailMessage();
                string strFrom = "apadmin@avc.com.cn";
                mailobj.From = new MailAddress(strFrom);
                string[] mails = toMail.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string mail in mails)
                    mailobj.To.Add(new MailAddress(mail));
                mailobj.Subject = subject;
                mailobj.Body = body;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = string.IsNullOrEmpty(m_EmailServer) ? "srv013sz.sz.avc.local" : m_EmailServer;
                smtp.Send(mailobj);
            }
            catch
            { 
            }
        }
    }
}
