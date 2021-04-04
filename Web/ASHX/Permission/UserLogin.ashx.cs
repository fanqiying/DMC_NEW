using System;
using System.Web;
using System.Collections.Generic;
using System.Web.SessionState;
using Utility.HelpClass;
using System.Text;
using DMC.BLL;
using DMC.Model;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Web.ASHX
{
    /// <summary>
    /// 用戶登錄操作
    /// code by jeven
    /// 2013-6-28
    /// </summary>
    public class UserLogin : IHttpHandler, IRequiresSessionState
    {
        LanguageManageService languageManage = new LanguageManageService();
        //變量定義以及相關類的實例化
        LogService lg = new LogService();
        UserManageService user = new UserManageService();
        string resultStr = string.Empty;
        List<UserInfo> list = new List<UserInfo>();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //接收傳參
            string id = GetData.GetRequest("id").Trim();
            string password = HttpUtility.UrlDecode(GetData.GetRequest("p").Trim());
            //string verificationcode = GetData.GetRequest("vc").Trim();//验证码
            string company = GetData.GetRequest("comp").Trim();//公司別
            string languageID = GetData.GetRequest("lag").Trim();//語言別
            string area = GetData.GetRequest("area").Trim();
            string method = GetData.GetRequest("path").ToLower();//谷歌瀏覽器路徑    
            if (method != "")
            {
                GetGoolePath(context);//返回谷歌瀏覽器路徑           
            }

            //try
            //{
            //    if (string.IsNullOrWhiteSpace(verificationcode) || verificationcode != System.Web.HttpContext.Current.Session["verificationcode"].ToString())
            //    {
            //        context.Response.Write("{\"success\":false,\"Err\":'验证码错误,请点击刷新后重新输入'}");
            //        context.Response.End();
            //        return;
            //    }
            //}
            //catch {
            //    context.Response.Write("{\"success\":false,\"Err\":'验证码错误,请点击刷新后重新输入'}");
            //    context.Response.End();
            //    return;
            //}

            string set = GetData.GetRequest("set").ToLower();
            if (set == "setlanguage")
            {
                SetLanguage(context);//返回谷歌瀏覽器路徑                 
            }
            //用戶登錄操作            
            UserInfo u = user.UserLogon(id, password, company, languageID, area, out resultStr, out list);
            if (resultStr == "SU0001" || resultStr == "SU0002")
            {
                //更新登錄信息
                string lastLoginIP = GetData.GetUserIP().ToString();
                user.ModUserLoginInfo(u.userID, lastLoginIP, System.DateTime.Now);
                //写进系统日志
                t_SysLog syslog = new t_SysLog();
                syslog.operatorID = u.userID;
                syslog.operatorName = u.userName;
                syslog.refProgram = "Login.aspx";
                syslog.refClass = "UserManageDAL";
                syslog.refMethod = "UserLogon";
                syslog.refRemark = "login system";
                syslog.refTime = System.DateTime.Now;
                syslog.refIP = GetData.GetUserIP().ToString();
                syslog.refSql = "p_Common_MyLogin";
                syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                lg.WriteLog(syslog);

                StringBuilder str = new StringBuilder();
                str.Append("{\"success\":true,\"list\":[");
                StringBuilder userInfo = new StringBuilder();
                List<string> areas = new List<string>();
                //登錄成功返回用戶信息JSON字符串
                foreach (UserInfo info in list)
                {
                    userInfo.Append((userInfo.Length > 0 ? "," : "") + "{\"userid\":\"" + info.userID + "\",\"userno\":\"" + info.userNo + "\",\"username\":\"" + info.userName + "\",\"adserver\":\"" + info.domainAddr + "\"}");
                    if (!areas.Contains(info.domainAddr))
                    {
                        areas.Add(info.domainAddr);
                    }
                }
                str.Append(userInfo.ToString());
                str.Append("]");
                if (areas.Count > 1)
                {
                    str.Append(",\"doublearea\":true");
                    str.Append(",\"areas\":[");
                    StringBuilder areaStr = new StringBuilder();
                    foreach (string item in areas)
                    {
                        areaStr.Append((areaStr.Length > 0 ? "," : "") + "{\"areaserver\":\"" + item + "\"}");
                    }
                    str.Append(areaStr.ToString());
                    str.Append("]");
                }
                else
                {
                    str.Append(",\"doublearea\":false");
                }
                str.Append("}");
                context.Response.Write(str.ToString());
            }
            else
            {
                resultStr = languageManage.GetResourceText(resultStr, languageID);
                context.Response.Write("{\"success\":false,\"Err\":'" + resultStr + "用户所属公司错误"+"'}");
            }
            context.Response.End();
        }
        /// <summary>
        /// 返回谷歌瀏覽器路徑
        /// </summary>
        /// <param name="context"></param>
        public void GetGoolePath(HttpContext context)
        {
            //讀取配置節點
            string goolePath = System.Configuration.ConfigurationManager.AppSettings["GoogleDownPath"];
            if (goolePath != "")
            {
                context.Response.Write(goolePath);
                context.Response.End();
            }
        }
        /// <summary>
        /// 設置當前語言別
        /// </summary>
        /// <param name="context"></param>
        public void SetLanguage(HttpContext context)
        {
            string languageId = System.Configuration.ConfigurationManager.AppSettings["languageId"];
            if (languageId != "")
            {
                UserInfo userInfo = new UserInfo();
                userInfo.LanguageId = languageId;
                //存储用户的登录信息
                SetUserCookie(userInfo);
                context.Response.Write(languageId);
                context.Response.End();
            }
        }
        //jeven_xiao add  (写用户cookie)
        private void SetUserCookie(UserInfo u)
        {
            int ct = 24 * 30;
            GetData.setCookie("LanguageID", u.LanguageId, DateTime.Now.AddHours(ct), "");
            GetData.setCookie("DefaultRole", u.defaultRole, DateTime.Now.AddHours(ct), "");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }
}