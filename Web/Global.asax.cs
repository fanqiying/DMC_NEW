
using DMC.BLL;
using DMC.DAL;
using DMC.Model;
using System;
using System.Configuration;
using System.Data;
using System.Text;

namespace Web
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            try
            {
            }
            catch { }
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            try
            {
                System.Threading.Thread.Sleep(5000);
                string strUrl = ConfigurationManager.AppSettings["HostUrl"].ToString();
                System.Net.HttpWebRequest _HttpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(strUrl);
                System.Net.HttpWebResponse _HttpWebResponse = (System.Net.HttpWebResponse)_HttpWebRequest.GetResponse();
                System.IO.Stream _Stream = _HttpWebResponse.GetResponseStream();//得到回写的字节流
            }
            catch
            {
            }
        }
    }
}