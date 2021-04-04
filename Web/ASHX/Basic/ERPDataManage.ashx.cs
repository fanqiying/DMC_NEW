using System;
using System.Web;
using System.Web.SessionState;
using System.Collections.Generic;
using Utility.HelpClass;
using System.Data;
using System.Linq; 
using DMC.BLL;
using DMC.Model;

namespace Web.ASHX
{
    /// <summary>
    /// ERP數據管理操作
    /// 處理與ERP數據交互的相關操作
    /// </summary>
    public class ERPDataManage : IHttpHandler, IRequiresSessionState
    {
        //業務類初始化
        private UserManageService userManage = new UserManageService();
        private UserManageService userive = new UserManageService();
        LogService logs = new LogService();
          

        private UserInfo currentUser = null;
        private string CompanyId = "avccn";
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                //獲取登錄用戶資料信息
                currentUser = userManage.GetUserMain();
                if (currentUser != null && currentUser.Company != null)
                {
                    CompanyId = currentUser.Company.companyID;//獲取公司別
                }
            }
            catch (Exception ex)
            {

                //写进系统日志
                t_SysLog syslog = new t_SysLog();
                syslog.operatorID = userive.GetUserMain().userID.To_String();
                syslog.operatorName = userive.GetUserMain().userName.To_String();
                syslog.refProgram = "/ASHX/Basic/ERPDataManage.ashx";
                syslog.refClass = "ERPDataManage.ashx";
                syslog.refMethod = "ProcessRequest";
                syslog.refRemark = ex.Message;
                syslog.refTime = System.DateTime.Now;
                syslog.refIP = GetData.GetUserIP().ToString();
                syslog.refSql = "";
                syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                logs.WriteLog(syslog);
            }

            context.Response.ContentType = "text/plain";
            //獲取前臺傳過來的操作方法
            string Method = context.Request.Params["M"];
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