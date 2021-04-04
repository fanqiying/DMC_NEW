using System;
using System.Web;
using System.Text;
using System.Data;
using System.Web.SessionState;
using Utility.HelpClass;
using DMC.BLL;
using DMC.Model;

namespace Web.ASHX
{
    /// <summary>
    /// 系統停機操作
    /// </summary>
    public class StopSysManege : IHttpHandler, IRequiresSessionState
    {
        //實例化相關業務類
        private SwitchService ss = new SwitchService();
        private UserManageService uc = new UserManageService();
        private LogService log = new LogService();
        private LanguageManageService languageManage = new LanguageManageService();
        private string LanguageId = "0002";
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string method = GetData.GetRequest("M").ToLower();
            LanguageId = uc.GetUserMain().LanguageId.ToString();
            switch (method)
            {
                case "syscolse":
                    StopSys(context);//做停机处理
                    break;
                case "load":
                    GetStopList(context);//取系统停机记录
                    break;
                case "datacolse"://做数据维护
                    DataColse(context);
                    break;
                case "loaddata":
                    GetdataCloseList(context);//取得某个公司下的数据维护列表
                    break;

            }
        }
        /// <summary>
        /// 取系统停机记录
        /// </summary>
        protected void GetStopList(HttpContext context)
        {
            DataTable dt = ss.GetAllStopList();
            StringBuilder sb = new StringBuilder();
            sb = JsonHelper.DataTableToJSON((DataTable)dt, true);
            context.Response.Write(sb);
        }


        /// <summary>
        /// 做停机处理
        /// </summary>
        protected void StopSys(HttpContext context)
        {
            string startTime = GetData.GetRequest("st").Trim();
            string endTime = GetData.GetRequest("et").Trim();
            string resons = GetData.GetRequest("reson").Trim();
            string notice = string.Empty;

            t_SysSwitch stich = new t_SysSwitch();
            stich.compName = "";
            stich.operateType = "1";
            stich.starTime = startTime.To_DateTime();
            stich.endTime = endTime.To_DateTime();
            stich.reasons = resons;
            stich.operaterID = uc.GetUserMain().userID.To_String();
            stich.operaterName = uc.GetUserMain().userName.To_String();
            stich.operateDeptID = uc.GetUserMain().userDept.To_String();

            //判断当前设置的停机时间是否有效
            bool okStatus = ss.IsExitSysStopInfo(Convert.ToDateTime(startTime), Convert.ToDateTime(endTime));
            if (okStatus == true)
            {
                DataTable stopDt = ss.GetLastStopRecord();
                if (stopDt != null && stopDt.Rows.Count > 0)
                {
                    if (stich.starTime > ((DateTime)stopDt.Rows[0]["starTime"]) && stich.endTime > ((DateTime)stopDt.Rows[0]["endTime"]))
                    {
                        bool result = ss.StopSys(stich);
                        if (result == true)
                        {
                            //写进系统日志
                            t_SysLog syslog = new t_SysLog();
                            syslog.operatorID = uc.GetUserMain().userID.To_String();
                            syslog.operatorName = uc.GetUserMain().userName.To_String();
                            syslog.refProgram = "SystemSetting.aspx";
                            syslog.refClass = "SwitchService";
                            syslog.refMethod = "StopSys";
                            syslog.refRemark = "stop system and startTime=" + stich.starTime + "";
                            syslog.refTime = System.DateTime.Now;
                            syslog.refIP = GetData.GetUserIP().ToString();
                            syslog.refSql = "nsert into t_SysSwitch";
                            syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                            log.WriteLog(syslog);
                            context.Response.Write("{\"success\":true}");
                        }
                    }
                    else
                    {
                        notice = "EB0039";
                        notice = languageManage.GetResourceText(notice, LanguageId);
                        context.Response.Write("{\"success\":false,\"msg\":\"" + notice + "\"}");
                    }
                }
            }
            else
            {
                notice = "EB0038"; //记录存在
                notice = languageManage.GetResourceText(notice, LanguageId);
                context.Response.Write("{\"success\":false,\"msg\":\"" + notice + "\"}");

            }
        }

        /// <summary>
        /// 做数据维护
        /// </summary>
        protected void DataColse(HttpContext context)
        {
            string startTime = GetData.GetRequest("st").Trim();
            string endTime = GetData.GetRequest("et").Trim();
            string resons = GetData.GetRequest("reson").Trim();
            string conpanyID = GetData.GetRequest("cid").Trim();

            t_SysSwitch stich = new t_SysSwitch();
            stich.compName = conpanyID;
            stich.operateType = "2";
            stich.starTime = startTime.To_DateTime();
            stich.endTime = endTime.To_DateTime();
            stich.reasons = resons;
            stich.operaterID = uc.GetUserMain().userID.To_String();
            stich.operaterName = uc.GetUserMain().userName.To_String();
            stich.operateDeptID = uc.GetUserMain().userDept.To_String();

            bool result = ss.StopSys(stich);
            if (result == true)
            {
                //写进系统日志
                t_SysLog syslog = new t_SysLog();
                syslog.operatorID = uc.GetUserMain().userID.To_String();
                syslog.operatorName = uc.GetUserMain().userName.To_String();
                syslog.refProgram = "SystemSetting.aspx";
                syslog.refClass = "SwitchService";
                syslog.refMethod = "StopSys";
                syslog.refRemark = "Data closed and startTime=" + stich.starTime + "";
                syslog.refTime = System.DateTime.Now;
                syslog.refIP = GetData.GetUserIP().ToString();
                syslog.refSql = "nsert into t_SysSwitch";
                syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                log.WriteLog(syslog);
                context.Response.Write("{\"success\":true, \"msg\":\"" + languageManage.GetResourceText("DataSaveSuccess", LanguageId) + "\"}");
            }
            else
            {
                context.Response.Write("{\"success\":false, \"msg\":\"" + languageManage.GetResourceText("sysfailture", LanguageId) + "\"}");
            }

        }

        /// <summary>
        /// 取得某个公司下的数据维护列表
        /// </summary>
        protected void GetdataCloseList(HttpContext context)
        {
            string compid = GetData.GetRequest("cpid").Trim();
            if (compid != "")
            {
                DataTable dt = ss.GetDataCloseListByID(compid);
                StringBuilder sb = new StringBuilder();
                sb = JsonHelper.DataTableToJSON((DataTable)dt, true);
                context.Response.Write(sb);
            }
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