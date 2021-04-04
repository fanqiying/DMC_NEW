using System;
using DMC.BLL;
using DMC.Model;
using Utility.HelpClass;
using System.Web;
using System.Data;
using System.Text;
using System.Dynamic;
using System.Web.SessionState;

namespace Web.ASHX
{
    public class SysParaSetting : IHttpHandler, IRequiresSessionState
    {
        //變量定義以及相關類的實例化
        private string UserId = "a10857";
        private string DeptId = "IT600";
        private string CompanyId = "avccn";
        private UserInfo currentUser = null;
        private UserManageService uservice = new UserManageService();
        private SysParaSettingService spss = new SysParaSettingService();
        private LanguageManageService languageManage = new LanguageManageService();

        private UserManageService userive = new UserManageService();
        LogService logs = new LogService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string method = GetData.GetRequest("M").ToLower();
            currentUser = uservice.GetUserMain();
            if (currentUser != null)
            {
                try
                {
                    UserId = currentUser.userID.ToString();//獲取用戶名
                    DeptId = currentUser.userDept.ToString();//獲取用戶部門
                    CompanyId = currentUser.Company.companyID.ToString();//獲取登錄公司別
                }
                catch
                { }
            }
            switch (method)
            {
                case "add":
                    Add(context);
                    break;
                case "mod":
                    Mod(context);
                    break;
                case "del":
                    Del(context);
                    break;
                case "search":
                    Search(context);
                    break;
            }
        }

        public void Add(HttpContext context)
        {
            try
            {
                SysParaSettingModel model = new SysParaSettingModel();
                model.CompanyId = context.Request["CompanyID"];
                model.InUser = UserId;
                model.ParaContent = context.Request["ParaContent"];
                model.ParaDesc = context.Request["ParaDesc"];
                model.ParaKey = context.Request["ParaKey"];
                model.ParaName = context.Request["ParaName"];
                model.Usey = context.Request["Usey"];
                string isResult = spss.AddSysPara(model);
                //switch (model.ParaKey)
                //{
                //    case "SysMcid":
                //        ScheduleMode.DataChange.Mcid = spss.GetParaValue("All", "SysMcid");//系統郵箱賬號
                //        break;
                //    case "SysFrom":
                //        ScheduleMode.DataChange.From = spss.GetParaValue("All", "SysFrom");//系統郵箱地址
                //        break;
                //    case "WeiXinAccount":
                //        ScheduleMode.DataChange.WeiXinAccount = spss.GetParaValue("All", "WeiXinAccount");//微信賬號
                //        break;
                //    case "WeiXinPwd":
                //        ScheduleMode.DataChange.WeiXinPwd = spss.GetParaValue("All", "WeiXinPwd");//微信密碼
                //        break;
                //}
                string msg = languageManage.GetResourceText(isResult, currentUser.LanguageId);
                context.Response.Write("{\"success\":" + (isResult == "SPS006" ? "true" : "false") + ",\"msg\":\"" + msg + "\"}");
            }
            catch (Exception e)
            {
                //写进系统日志
                t_SysLog syslog = new t_SysLog();
                syslog.operatorID = userive.GetUserMain().userID.To_String();
                syslog.operatorName = userive.GetUserMain().userName.To_String();
                syslog.refProgram = "/SystemManage/SysParamSetting.aspx";
                syslog.refClass = "SysParaSetting.ashx";
                syslog.refMethod = "Add";
                syslog.refRemark = e.ToString();
                syslog.refTime = System.DateTime.Now;
                syslog.refIP = GetData.GetUserIP().ToString();
                syslog.refSql = "";
                syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                logs.WriteLog(syslog);

                string result = "newLabel069";
                result = languageManage.GetResourceText(result, currentUser.LanguageId);
                context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
            }
        }

        public void Mod(HttpContext context)
        {
            try
            {
                SysParaSettingModel model = new SysParaSettingModel();
                model.CompanyId = context.Request["CompanyID"];
                model.InUser = UserId;
                model.ParaContent = context.Request["ParaContent"];
                model.ParaDesc = context.Request["ParaDesc"];
                model.ParaKey = context.Request["ParaKey"];
                model.ParaName = context.Request["ParaName"];
                model.Usey = context.Request["Usey"];
                string isResult = spss.ModSysPara(model);
                //switch (model.ParaKey)
                //{
                //    case "SysMcid":
                //        ScheduleMode.DataChange.Mcid = spss.GetParaValue("All", "SysMcid");//系統郵箱賬號
                //        break;
                //    case "SysFrom":
                //        ScheduleMode.DataChange.From = spss.GetParaValue("All", "SysFrom");//系統郵箱地址
                //        break;
                //    case "WeiXinAccount":
                //        ScheduleMode.DataChange.WeiXinAccount = spss.GetParaValue("All", "WeiXinAccount");//微信賬號
                //        break;
                //    case "WeiXinPwd":
                //        ScheduleMode.DataChange.WeiXinPwd = spss.GetParaValue("All", "WeiXinPwd");//微信密碼
                //        break;
                //}
                string msg = languageManage.GetResourceText(isResult, currentUser.LanguageId);
                context.Response.Write("{\"success\":" + (isResult == "SPS006" ? "true" : "false") + ",\"msg\":\"" + msg + "\"}");
            }
            catch (Exception e)
            {
                //写进系统日志
                t_SysLog syslog = new t_SysLog();
                syslog.operatorID = userive.GetUserMain().userID.To_String();
                syslog.operatorName = userive.GetUserMain().userName.To_String();
                syslog.refProgram = "/SystemManage/SysParamSetting.aspx";
                syslog.refClass = "SysParaSetting.ashx";
                syslog.refMethod = "Mod";
                syslog.refRemark = e.ToString();
                syslog.refTime = System.DateTime.Now;
                syslog.refIP = GetData.GetUserIP().ToString();
                syslog.refSql = "";
                syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                logs.WriteLog(syslog);

                string result = "newLabel071";
                result = languageManage.GetResourceText(result, currentUser.LanguageId);
                context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
            }
        }

        public void Del(HttpContext context)
        {
            try
            {
                string CompanyID = context.Request["CompanyID"];
                string ParaKey = context.Request["ParaKey"];
                bool isResult = spss.DelSysPara(CompanyID, ParaKey);
                string id = isResult ? "deletesuccess" : "deleteFail";
                string msg = languageManage.GetResourceText("deletesuccess", currentUser.LanguageId);
                context.Response.Write("{\"success\":" + isResult.ToString().ToLower() + ",\"msg\":\"" + msg + "\"}");
            }
            catch (Exception e)
            {
                //写进系统日志
                t_SysLog syslog = new t_SysLog();
                syslog.operatorID = userive.GetUserMain().userID.To_String();
                syslog.operatorName = userive.GetUserMain().userName.To_String();
                syslog.refProgram = "/SystemManage/SysParamSetting.aspx";
                syslog.refClass = "SysParaSetting.ashx";
                syslog.refMethod = "Del";
                syslog.refRemark = e.ToString();
                syslog.refTime = System.DateTime.Now;
                syslog.refIP = GetData.GetUserIP().ToString();
                syslog.refSql = "";
                syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                logs.WriteLog(syslog);

                string result = "newLabel072";
                result = languageManage.GetResourceText(result, currentUser.LanguageId);
                context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
            }
        }

        /// <summary>
        /// 搜索HR配置信息
        /// </summary>
        /// <param name="context"></param>
        public void Search(HttpContext context)
        {
            //創建匿名對象
            dynamic model = new ExpandoObject();
            //接收相關傳參
            model.KeyWord = context.Request.Params["KeyWord"];
            model.type = context.Request.Params["SearchType"];
            model.rows = int.Parse(context.Request.Params["rows"]);
            model.page = int.Parse(context.Request.Params["page"]);

            int total = 0;
            int pageCount = 0;
            //取得相關查詢條件下的數據列表
            DataTable dt = spss.Search(out total, out pageCount, model);
            StringBuilder sb = new StringBuilder();
            //將數據返回給客戶端
            sb = JsonHelper.DataTableToJSON(dt, total, true);
            context.Response.Write(sb);
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