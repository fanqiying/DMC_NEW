using System;
using System.Web;
using Utility.HelpClass;
using System.Web.SessionState;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using DMC.BLL;
using DMC.Model;

namespace Web.ASHX
{
    /// <summary>
    /// 系統日誌操作
    /// </summary>
    public class LogManage : IHttpHandler, IRequiresSessionState
    {
        //實例化方法類
        UserManageService uservice = new UserManageService();
        LogService ls = new LogService();
        private LanguageManageService languageManage = new LanguageManageService();
        private string LanguageId = "0002";
        private UserInfo currentUser = null;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string method = GetData.GetRequest("M").ToLower();
            currentUser = uservice.GetUserMain();
            LanguageId = currentUser.LanguageId.ToString();//獲取登錄語言別
            switch (method)
            {
                case "search"://按條件搜索所有日誌記錄
                    Search(context);
                    break;
                case "delete"://刪除日誌
                    DelLog(context);
                    break;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 刪除日誌
        /// </summary>
        /// <param name="context"></param>
        private void DelLog(HttpContext context)
        {
            string idstr = GetData.GetRequest("IDstr").To_String();
            List<string> arr = idstr.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            string result = string.Empty;
            result = ls.DelSysLog(arr);
            if (result == "SB0016")
            {
                context.Response.Write("{\"success\":true}");
            }
            else
            {
                context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
            }
        }
        /// <summary>
        /// 按條件搜索所有日誌記錄
        /// </summary>
        /// <param name="context"></param>
        private void Search(HttpContext context)
        {
            //每页多少條數據
            int row = GetData.GetRequest("rows").To_Int();
            //當前第幾页
            int page = GetData.GetRequest("page").To_Int();
            //總共多少條數據   
            //int i = Count();
            int pageCount = 0;
            int total = 0;
            string where = string.Empty;
            string type = GetData.GetRequest("SearchType").To_String();
            switch (type)
            {
                //關鍵字搜索
                case "ByKey":
                    string key = GetData.GetRequest("KeyWord");
                    if (!string.IsNullOrEmpty(key) && key != languageManage.GetResourceText("InputDefaultKey", LanguageId))

                        where = string.Format(" operatorID =N'{0}' or refProgram like N'{0}%' or refClass like N'{0}%' or refMethod like N'{0}%' or refIP =N'{0}' ", key);
                    break;
                //高級搜索
                case "ByAdvanced":
                    //執行者
                    if (!string.IsNullOrEmpty(GetData.GetRequest("operatorid")))
                    {

                        where = string.Format(" operatorID=N'{0}' ", GetData.GetRequest("operatorid"));
                    }
                    if (!string.IsNullOrEmpty(GetData.GetRequest("refprogram")))
                    {
                        where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format(" (refProgram like N'{0}%')", GetData.GetRequest("refprogram"));
                    }
                    if (!string.IsNullOrEmpty(GetData.GetRequest("refclass")))
                    {
                        //程式類別
                        where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format("  (refClass like N'{0}%')", GetData.GetRequest("refclass"));
                    }
                    if (!string.IsNullOrEmpty(GetData.GetRequest("refip")))
                    {
                        //IP地址
                        where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format("  (refIP=N'{0}')", GetData.GetRequest("refip"));
                    }
                    //結束時間
                    if (!string.IsNullOrEmpty(GetData.GetRequest("startTime")) && !string.IsNullOrEmpty(GetData.GetRequest("endTime")))
                    {
                        //開始時間
                        if (GetData.GetRequest("startTime") != GetData.GetRequest("endTime"))
                            where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format(" (refTime between N'{0}' and N'{1}')", GetData.GetRequest("startTime"), GetData.GetRequest("endTime"));
                        else
                            where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format(" (convert (date ,refTime) between N'{0}' and N'{1}')", GetData.GetRequest("startTime"), GetData.GetRequest("endTime"));
                    }


                    break;
            }

            DataTable dt = ls.Search(row, page, out pageCount, out total, where);
            StringBuilder sb = new StringBuilder();
            sb = JsonHelper.DataTableToJSON(dt, total, true);
            context.Response.Write(sb);
        }
    }
}