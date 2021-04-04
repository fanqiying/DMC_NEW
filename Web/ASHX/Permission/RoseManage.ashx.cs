using System;
using System.Web;
using System.Text;
using System.Data;
using System.Web.SessionState;
using Utility.HelpClass;
using DMC.Model;
using DMC.BLL;

namespace Web.ASHX
{
    /// <summary>
    /// q權限管理操作
    /// code by klint
    /// 2013-7-4
    /// </summary>
    public class RoseManage : IHttpHandler, IRequiresSessionState
    {
        //變量定義以及相關類的實例化
        private string UserId = "a10857";
        private string DeptId = "IT600";
        private string LanguageId = "0002";
        private UserInfo currentUser = null;
        private t_SysLog syslog = new t_SysLog();
        private LogService lg = new LogService();
        private UserManageService userManage = new UserManageService();
        private LanguageManageService languageManage = new LanguageManageService();
        private RoseManageService roseManage = new RoseManageService();
        public void ProcessRequest(HttpContext context)
        {
            //從session中獲取登錄用戶的相關信息
            currentUser = userManage.GetUserMain();
            UserId = currentUser.userID.ToString();//獲取用戶名
            DeptId = currentUser.userDept.ToString();//獲取用戶部門
            LanguageId = currentUser.LanguageId.ToString();//獲取登錄語言別
            context.Response.ContentType = "text/plain";
            //獲取操作方法名
            string Method = context.Request.Params["M"];
            switch (Method.ToLower())
            {
                case "search":
                    Search(context);//搜索權限列表
                    break;
                case "add"://新增權限操作
                    CreateRose(context);
                    break;
                case "update"://更新權限操作
                    ModRose(context);
                    break;
                case "detail"://查看權限明細
                    RoseDetail(context);
                    break;
                case "delete"://刪除權限資料
                    DelRose(context);
                    break;
                case "copy"://複製權限資料
                    CopyRight(context);
                    break;
            }
        }
        /// <summary>
        /// 搜索權限列表
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
                    if (!string.IsNullOrEmpty(key))
                        where = string.Format(" RoseId like N'%{0}%' OR RoseName like N'%{0}%' OR CreateUserId like N'%{0}%' OR UpdateDeptId like N'%{0}%'", key);
                    break;
                case "ByAdvanced"://高級搜索
                    if (!string.IsNullOrEmpty(context.Request.Params["RoseId"]))
                    {
                        where = string.Format("RoseId like N'%{0}%'", context.Request.Params["RoseId"]);
                    }
                    //權限名稱
                    if (!string.IsNullOrEmpty(context.Request.Params["RoseName"]))
                    {
                        where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format("RoseName like N'%{0}%'", context.Request.Params["RoseName"]);
                    }
                    //系統類別
                    if (!string.IsNullOrEmpty(context.Request.Params["SystemType"]))
                    {
                        where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format("SystemType=N'{0}'", context.Request.Params["SystemType"]);
                    }
                    if (!string.IsNullOrEmpty(context.Request.Params["Usy"]))
                    {
                        where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format("Usy=N'{0}'", context.Request.Params["Usy"]);
                    }
                    break;
            }

            DataTable dt = roseManage.Search(row, page, out pageCount, out total, where);
            StringBuilder dd = JsonHelper.ConvertDTToJson(dt, total);
            context.Response.Write(dd.ToString());
        }
        /// <summary>
        /// 查看權限明細
        /// </summary>
        /// <param name="context"></param>
        public void RoseDetail(HttpContext context)
        {
            string roseId = context.Request.Params["RoseId"];
            if (!string.IsNullOrEmpty(roseId))
            {
                DataTable dt = roseManage.RoseUserInfo(roseId);
                StringBuilder dd = JsonHelper.ConvertDTToJson(dt, dt.Rows.Count);
                context.Response.Write(dd.ToString());
            }
            else
            {
                StringBuilder JsonString = new StringBuilder();
                JsonString.Append("{ ");
                JsonString.Append("\"rows\":[ ]}");
                context.Response.Write(JsonString);
            }
        }

        public void CreateRose(HttpContext context)
        {
            t_Right_Rose model = new t_Right_Rose();
            model.RoseId = context.Request["RoseId"].ToString().Trim().ToUpper();
            model.RoseName = context.Request["RoseName"].ToString().Trim();
            model.SystemType = context.Request["SystemType"].ToString().Trim();
            model.Usy = context.Request["Usy"].ToString().Trim();
            model.CreateUserId = UserId;
            model.CreateDeptId = DeptId;
            model.CreateTime = System.DateTime.Now;

            string result = roseManage.AddRose(model);

            if (result == "success")
                context.Response.Write("{\"success\":true}");
            else
                context.Response.Write("{\"success\":false,\"msg\":\"" + languageManage.GetResourceText(result, LanguageId) + "\"}");

            //写进系统日志
            t_SysLog syslog = new t_SysLog();
            syslog.operatorID = currentUser.userID;
            syslog.operatorName = currentUser.userName;
            syslog.refProgram = "PerTypeManage.aspx";
            syslog.refClass = "RoseManageService";
            syslog.refMethod = "AddRose";
            syslog.refRemark = "創建權限類別:" + model.RoseId + ";創建結果：" + result;
            syslog.refTime = System.DateTime.Now;
            syslog.refIP = GetData.GetUserIP().ToString();
            syslog.refSql = "";
            syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
            lg.WriteLog(syslog);
        }
        /// <summary>
        /// 更新權限操作
        /// </summary>
        /// <param name="context"></param>
        public void ModRose(HttpContext context)
        {
            try
            {
                t_Right_Rose model = new t_Right_Rose();
                model.RoseId = context.Request["RoseId"].ToString().Trim();
                model.RoseName = context.Request["RoseName"].ToString().Trim();
                model.SystemType = context.Request["SystemType"].ToString().Trim();
                model.Usy = context.Request["Usy"].ToString().Trim();
                model.UpdateUserId = UserId;
                model.UpdateDeptId = DeptId;
                model.UpdateTime = System.DateTime.Now;

                string result = roseManage.ModRose(model);
                if (result == "success")
                    context.Response.Write("{\"success\":true}");
                else
                    context.Response.Write("{\"success\":false,\"msg\":\"" + languageManage.GetResourceText(result, LanguageId) + "\"}");

                //写进系统日志
                t_SysLog syslog = new t_SysLog();
                syslog.operatorID = currentUser.userID;
                syslog.operatorName = currentUser.userName;
                syslog.refProgram = "PerTypeManage.aspx";
                syslog.refClass = "RoseManageService";
                syslog.refMethod = "ModRose";
                syslog.refRemark = "修改權限類別:" + model.RoseId + ";修改結果：" + result;
                syslog.refTime = System.DateTime.Now;
                syslog.refIP = GetData.GetUserIP().ToString();
                syslog.refSql = "";
                syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                lg.WriteLog(syslog);
            }
            catch (Exception e)
            {

                //写进系统日志
                t_SysLog syslog = new t_SysLog();
                syslog.operatorID = currentUser.userID;
                syslog.operatorName = currentUser.userName;
                syslog.refProgram = "PerTypeManage.aspx";
                syslog.refClass = "RoseManageService";
                syslog.refMethod = "ModRose";
                syslog.refRemark = e.To_String();
                syslog.refTime = System.DateTime.Now;
                syslog.refIP = GetData.GetUserIP().ToString();
                syslog.refSql = "";
                syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                lg.WriteLog(syslog);


                string result = "newLabel125";
                result = languageManage.GetResourceText(result, LanguageId);
                context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
            }
        }
        /// <summary>
        /// 刪除權限資料
        /// </summary>
        /// <param name="context"></param>
        public void DelRose(HttpContext context)
        {
            try
            {
                string roseList = context.Request["RoseIdList"].ToString().Trim();
                if (!string.IsNullOrEmpty(roseList))
                {
                    string result = roseManage.DelRose(roseList);
                    if (result == "success")
                    {
                        context.Response.Write("{\"success\":true}");
                    }
                    else
                    {
                        if (result == "ER0008")
                        {
                            string id = roseManage.RightIsUsingId(roseList);
                            context.Response.Write("{\"success\":false,\"msg\":\"" + languageManage.GetResourceText("ER0012", LanguageId).Replace("0", id) + "\"}");
                        }
                        else
                        {
                            context.Response.Write("{\"success\":false,\"msg\":\"" + languageManage.GetResourceText(result, LanguageId) + "\"}");
                        }
                    }
                    //写进系统日志
                    t_SysLog syslog = new t_SysLog();
                    syslog.operatorID = currentUser.userID;
                    syslog.operatorName = currentUser.userName;
                    syslog.refProgram = "PerTypeManage.aspx";
                    syslog.refClass = "RoseManageService";
                    syslog.refMethod = "DelRose";
                    syslog.refRemark = "刪除權限類別:" + roseList + ";刪除結果：" + result;
                    syslog.refTime = System.DateTime.Now;
                    syslog.refIP = GetData.GetUserIP().ToString();
                    syslog.refSql = "";
                    syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                    lg.WriteLog(syslog);
                }
            }
            catch (Exception e)
            {

                //写进系统日志
                t_SysLog syslog = new t_SysLog();
                syslog.operatorID = currentUser.userID;
                syslog.operatorName = currentUser.userName;
                syslog.refProgram = "PerTypeManage.aspx";
                syslog.refClass = "RoseManageService";
                syslog.refMethod = "DelRose";
                syslog.refRemark = e.To_String();
                syslog.refTime = System.DateTime.Now;
                syslog.refIP = GetData.GetUserIP().ToString();
                syslog.refSql = "";
                syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                lg.WriteLog(syslog);


                string result = "newLabel126";
                result = languageManage.GetResourceText(result, LanguageId);
                context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
            }
        }
        /// <summary>
        /// 複製權限資料
        /// </summary>
        /// <param name="context"></param>
        private void CopyRight(HttpContext context)
        {
            string socure = context.Request.Params["SocureRoseId"];
            string Aim = context.Request.Params["RoseId"];
            string result = "";
            if (!string.IsNullOrEmpty(socure) && !string.IsNullOrEmpty(Aim))
            {
                result = roseManage.CopyRose(socure, Aim);
                if (result == "success")
                {
                    result = "{\"success\":true}";
                }
                else
                {
                    result = "{\"success\":false,\"msg\":\"" + languageManage.GetResourceText(result, LanguageId) + "\"}";
                }
            }
            else
            {
                //copynotsocure 請輸入需要複製數據的來源
                result = "{\"success\":false,\"msg\":\"" + languageManage.GetResourceText("copynotsocure", LanguageId) + "\"}";
            }
            context.Response.Write(result);
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