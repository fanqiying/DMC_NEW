/*******************************************************************
 * 類  名（ClassName）  ：QMS.BLL.common
 * 關鍵字（KeyWord）    ：
 * 描  述（Description）：
 * 版  本（Version）    ：1.0
 * 日  期（Date）       ：2020/12/1 18:07:22
 * 作  者（Author）     ：devin_shu
******************************修改記錄******************************
 * 版本       時間      作者      描述
 * 
********************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DMC.BLL.common
{
    public class HttpHelper
    {
        public static HttpWebResponse CreateHttpResponse(string url, string method, string body, string contentType, string uid = "", string pwd = "")
        {
            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                SetCertificatePolicy();
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = method;
            request.ContentType = contentType;            
            request.Headers.Add("Charset", "UTF-8");
            if (string.IsNullOrWhiteSpace(uid) == false && string.IsNullOrWhiteSpace(pwd) == false)
            {
                request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(uid + ":" + pwd)).Trim());
            }        
            if (!string.IsNullOrEmpty(body))
            {
                request.Connection = "KeepAlive";
                byte[] data = Encoding.UTF8.GetBytes(body);
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            return request.GetResponse() as HttpWebResponse;
        }

        public static string Post(string url,string body, string contentType,string uid="", string pwd="")
        {
            string result = string.Empty;
            HttpWebRequest request = null;
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                SetCertificatePolicy();
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";
            request.ContentType = contentType;
            request.Headers.Add("Charset", "UTF-8");
            if (string.IsNullOrWhiteSpace(uid)==false&&string.IsNullOrWhiteSpace(pwd)==false)
            {
                request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(uid + ":" + pwd)).Trim());   
            }            
            if (!string.IsNullOrWhiteSpace(body))
            {
                request.Connection = "KeepAlive";
                byte[] data = Encoding.UTF8.GetBytes(body);
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            using (HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse())
            using (StreamReader stream = new StreamReader(webResponse.GetResponseStream(),Encoding.UTF8))
            {
                result = stream.ReadToEnd();
            }
            return result;
        }

        /// <summary>
        /// 注册证书验证回调事件，在请求之前注册
        /// </summary>
        private static void SetCertificatePolicy()
        {
            ServicePointManager.ServerCertificateValidationCallback
                       += RemoteCertificateValidate;
        }
        /// <summary>  
        /// 远程证书验证，固定返回true 
        /// </summary>  
        private static bool RemoteCertificateValidate(object sender, X509Certificate cert,
            X509Chain chain, SslPolicyErrors error)
        {
            return true;
        }
    }
}
