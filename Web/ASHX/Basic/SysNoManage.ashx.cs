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
    /// 系統單號操作
    /// code by abby
    /// </summary>
    public class SysNoManage : IHttpHandler, IRequiresSessionState
    {
        //變量定義以及相關類的實例化
        private SystemNoService systemNoManage = new SystemNoService();
        private LanguageManageService languageManage = new LanguageManageService();
        private UserManageService userManage = new UserManageService();
        private UserManageService userive = new UserManageService();
        LogService logs = new LogService();
        /// <summary>
        /// 用戶信息
        /// </summary>
        private string UserId = "a10857";
        private string DeptId = "IT600";
        private string LanguageId = "0002";
        private string CompanyId = "avccn";
        private UserInfo currentUser = null;

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                //從session中獲取登錄用戶的相關信息
                currentUser = userManage.GetUserMain();
                if (currentUser != null)
                {
                    UserId = currentUser.userID.ToString();//獲取用戶名
                    DeptId = currentUser.userDept.ToString();//獲取用戶部門
                    LanguageId = currentUser.LanguageId.ToString();//獲取登錄語言別
                    CompanyId = currentUser.Company.companyID;//獲取系統公司別
                }
            }
            catch (Exception ex)
            {
                //写进系统日志
                t_SysLog syslog = new t_SysLog();
                syslog.operatorID = userive.GetUserMain().userID.To_String();
                syslog.operatorName = userive.GetUserMain().userName.To_String();
                syslog.refProgram = "/ASHX/Basic/SysNoManage.ashx";
                syslog.refClass = "SysNoManage.ashx";
                syslog.refMethod = "ProcessRequest";
                syslog.refRemark = ex.Message;
                syslog.refTime = System.DateTime.Now;
                syslog.refIP = GetData.GetUserIP().ToString();
                syslog.refSql = "";
                syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                logs.WriteLog(syslog);
            }

            context.Response.ContentType = "text/plain";
            //獲取操作方法名
            string Method = context.Request.Params["M"];
            switch (Method.ToLower())
            {
                case "search":
                    Search(context);//獲取所有的系統單號列表
                    break;
                case "create"://創建新的系統單號
                    CreateSystemNo(context);
                    break;
                case "delete"://刪除系統單號
                    Delete(context);
                    break;
                case "update"://更新系統單號數據
                    UpdateSystemNo(context);
                    break;
            }
        }
        /// <summary>
        /// 更新系統單號數據
        /// </summary>
        /// <param name="context"></param>
        public void UpdateSystemNo(HttpContext context)
        {
            try
            {
                //實例化對象實體
                t_SystemNo model = new t_SystemNo();
                //組織實體對象數據
                model.AutoID = Convert.ToInt32(context.Request["autoid"].ToString());
                model.Category = context.Request["category"].ToString();
                model.CodeLen = Convert.ToInt32(context.Request["codelen"].ToString());
                model.CompanyId = context.Request["companyid"].ToString();
                model.UpdateDeptId = DeptId;
                model.UpdateTime = DateTime.Now;
                model.UpdateUserId = UserId;
                model.DateType = context.Request["datetype"].ToString();
                model.keyword = context.Request["keyword"].ToString();
                model.Mark = context.Request["mark"].ToString();
                model.ModularType = context.Request["modulartype"].ToString();
                model.ModuleType = context.Request["moduletype"].ToString();
                model.ReceiptType = context.Request["receipttype"].ToString();
                model.Usy = context.Request["usy"].ToString();
                model.GeneratTime = null;
                //調用業務方法返回執行結果
                string result = systemNoManage.ModSystemNo(model);
                if (result == "success")
                {
                    context.Response.Write("{\"success\":true}");
                }
                else
                {
                    context.Response.Write("{\"success\":false,\"msg\":\"" + languageManage.GetResourceText(result, LanguageId) + "\"}");
                }
            }
            catch (Exception ex)
            {
                //写进系统日志
                t_SysLog syslog = new t_SysLog();
                syslog.operatorID = userive.GetUserMain().userID.To_String();
                syslog.operatorName = userive.GetUserMain().userName.To_String();
                syslog.refProgram = "/ASHX/Basic/SysNoManage.ashx";
                syslog.refClass = "SysNoManage.ashx";
                syslog.refMethod = "UpdateSystemNo";
                syslog.refRemark = ex.Message;
                syslog.refTime = System.DateTime.Now;
                syslog.refIP = GetData.GetUserIP().ToString();
                syslog.refSql = "";
                syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                logs.WriteLog(syslog);

                context.Response.Write("{\"success\":false,\"msg\":\"" + ex.Message + "\"}");
            }
        }
        /// <summary>
        /// 創建新的系統單號
        /// </summary>
        /// <param name="context"></param>

        public void CreateSystemNo(HttpContext context)
        {
            try
            {
                // //接收傳參
                //實例化對象實體
                t_SystemNo model = new t_SystemNo();
                //組織實體對象數據
                model.Category = context.Request["category"].ToString();
                model.CodeLen = Convert.ToInt32(context.Request["codelen"].ToString());
                model.CompanyId = context.Request["companyid"].ToString();
                model.CreateDeptId = DeptId;
                model.createTime = DateTime.Now;
                model.CreateUserId = UserId;
                model.DateType = context.Request["datetype"].ToString();
                model.keyword = context.Request["keyword"].ToString();
                model.Mark = context.Request["mark"].ToString();
                model.ModularType = context.Request["modulartype"].ToString();
                model.ModuleType = context.Request["moduletype"].ToString();
                model.ReceiptType = context.Request["receipttype"].ToString();
                model.Usy = context.Request["usy"].ToString();
                //調用業務方法返回執行結果
                string result = systemNoManage.AddSystemNo(model);
                if (result == "success")
                {
                    context.Response.Write("{\"success\":true}");
                }
                else
                {
                    context.Response.Write("{\"success\":false,\"msg\":\"" + languageManage.GetResourceText(result, LanguageId) + "\"}");
                }
            }
            catch (Exception ex)
            {
                //写进系统日志
                t_SysLog syslog = new t_SysLog();
                syslog.operatorID = userive.GetUserMain().userID.To_String();
                syslog.operatorName = userive.GetUserMain().userName.To_String();
                syslog.refProgram = "/ASHX/Basic/SysNoManage.ashx";
                syslog.refClass = "SysNoManage.ashx";
                syslog.refMethod = "CreateSystemNo";
                syslog.refRemark = ex.Message;
                syslog.refTime = System.DateTime.Now;
                syslog.refIP = GetData.GetUserIP().ToString();
                syslog.refSql = "";
                syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                logs.WriteLog(syslog);

                context.Response.Write("{\"success\":false,\"msg\":\"" + ex.Message + "\"}");
            }
        }
        /// <summary>
        /// 刪除系統單號
        /// </summary>
        /// <param name="context"></param>
        public void Delete(HttpContext context)
        {
            string Ids = context.Request.Params["IDs"].Trim();
            if (!string.IsNullOrEmpty(Ids))
            {
                List<string> IDList = Ids.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                string result = systemNoManage.DelSystemNo(IDList);
                if (result == "success")
                {
                    context.Response.Write("{\"success\":true}");
                }
                else
                {
                    context.Response.Write("{\"success\":false,\"msg\":\"" + languageManage.GetResourceText(result, LanguageId) + "\"}");

                }
            }
            else
            {
                //請輸入需要刪除的數據
                string selectDataForDelete = languageManage.GetResourceText("selectDataForDelete", LanguageId);
                context.Response.Write("{\"success\":false,\"msg\":\"" + selectDataForDelete + "\"}");
            }
        }
        /// <summary>
        /// 獲取所有的系統單號列表
        /// </summary>
        /// <param name="context"></param>
        public void Search(HttpContext context)
        {
            //row和page從前臺傳入   
            //每页多少條數據
            int row = int.Parse(context.Request["rows"].ToString());
            //當前第幾页
            int page = int.Parse(context.Request["page"].ToString());
            //總共多少條數據   
            //int i = Count();
            int pageCount = 0;
            int total = 0;
            string where = string.Empty;
            string type = context.Request.Params["SearchType"];
            switch (type)
            {
                case "ByKey"://關鍵字搜索
                    string key = context.Request.Params["KeyWord"];
                    //请输入关键字
                    string resulttemp = languageManage.GetResourceText("InputDefaultKey", LanguageId);
                    if (!string.IsNullOrEmpty(key) && key != resulttemp)
                        where = string.Format(" CompanyId like N'%{0}%' OR ModuleType like N'%{0}%' OR ModularType like N'%{0}%' OR Category like N'%{0}%' or ReceiptType like N'%{0}%' or keyword like N'%{0}%' or Mark like N'%{0}%' ", key);
                    break;
                case "ByAdvanced"://高級搜索
                    if (!string.IsNullOrEmpty(context.Request.Params["CompanyId"].ToString().Trim()))
                    {
                        where = string.Format("CompanyId = N'{0}' ", context.Request.Params["CompanyId"]);
                    }
                    //模塊類別
                    if (!string.IsNullOrEmpty(context.Request.Params["ModuleType"].ToString().Trim()))
                    {
                        where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format("ModuleType = N'{0}'", context.Request.Params["ModuleType"]);
                    }
                    if (!string.IsNullOrEmpty(context.Request.Params["ModularType"].ToString().Trim()))
                    {
                        where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format("ModularType='{0}'", context.Request.Params["ModularType"]);
                    }
                    //生成單號種類
                    if (!string.IsNullOrEmpty(context.Request.Params["Category"].ToString().Trim()))
                    {
                        where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format("Category = N'{0}'", context.Request.Params["Category"]);
                    }
                    if (!string.IsNullOrEmpty(context.Request.Params["ReceiptType"].ToString().Trim()))
                    {
                        where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format("ReceiptType = N'{0}'", context.Request.Params["ReceiptType"]);
                    }
                    if (!string.IsNullOrEmpty(context.Request.Params["Word"].ToString().Trim()))
                    {
                        where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format("keyword = N'{0}'", context.Request.Params["keyword"]);
                    }
                    if (!string.IsNullOrEmpty(context.Request.Params["Usy"].ToString().Trim()))
                    {
                        where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format("Usy='{0}'", context.Request.Params["Usy"]);
                    }
                    break;
            }
            //調用業務方法 執行搜索操作
            DataTable dt = systemNoManage.Search(row, page, out pageCount, out total, where);
            string dd = JsonHelper.DataTableToJSON(dt, total, true).ToString();
            context.Response.Write(dd);
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