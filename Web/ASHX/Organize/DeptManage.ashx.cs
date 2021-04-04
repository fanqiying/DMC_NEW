using System;
using System.Web;
using Utility.HelpClass;
using System.Text;
using System.Data;
using System.Collections.Generic;
using DMC.Model;
using DMC.BLL;
using System.Linq;
using System.Web.SessionState;

namespace Web.ASHX
{
    /// <summary>
    /// 基本資料--部門信息管理
    /// code by jeven
    /// 2013-6-8
    /// </summary>
    public class DeptManage : IHttpHandler, IRequiresSessionState
    {
        //變量定義以及相關類的實例化
        private string LanguageId = "0002";
        private string UserId = "a10857";
        private string DeptId = "IT600";
        private UserInfo currentUser = null;
        private UserManageService userManage = new UserManageService();
        private DeptService dservice = new DeptService();
        private UserManageService uservice = new UserManageService();
        private LanguageManageService languageManage = new LanguageManageService();

        private UserManageService userive = new UserManageService();
        LogService logs = new LogService();
        private EmpService empservice = new EmpService();
        public void ProcessRequest(HttpContext context)
        {
            //從session中獲取登錄用戶的相關信息
            currentUser = userManage.GetUserMain();
            UserId = currentUser.userID.ToString();//獲取用戶名
            DeptId = currentUser.userDept.ToString();//獲取用戶部門
            LanguageId = currentUser.LanguageId.ToString();//獲取登錄語言別
            context.Response.ContentType = "text/plain";
            //獲取操作方法名
            string method = GetData.GetRequest("M").ToLower();
            switch (method)
            {
                case "add"://新增部門信息
                    AddDept(context);
                    break;
                case "search"://按條件搜索部門資料
                    Search(context);
                    break;
                case "delete"://刪除部門資料
                    DelDept(context);
                    break;
                case "update"://更新部門資料
                    ModDept(context);
                    break;
            }
        }

        /// <summary>
        /// 取得所有部门资料信息的集合
        /// </summary>
        /// <param name="context"></param>
        protected void Search(HttpContext context)
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
                    if (!string.IsNullOrEmpty(key) && key != languageManage.GetResourceText("InputKeyWord", LanguageId))

                        where = string.Format("deptID like '%{0}%' or simpleName like N'%{0}%' or fullName like N'%{0}%' or deptHeader like N'%{0}%' or deptNature like N'%{0}%' or usy=N'{0}'", key);
                    break;
                //高級搜索
                case "ByAdvanced":
                    if (!string.IsNullOrEmpty(GetData.GetRequest("deptid")))
                    {

                        where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format("deptID like N'%{0}%' ", GetData.GetRequest("deptid"));
                    }
                    if (!string.IsNullOrEmpty(GetData.GetRequest("deptname")))
                    {
                        where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format(" (simpleName like N'%{0}%' or fullName like N'{0}%')", GetData.GetRequest("deptname"));
                    }//部門領導
                    if (!string.IsNullOrEmpty(GetData.GetRequest("header")))
                    {
                        where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format(" (deptHeader =N'{0}')", GetData.GetRequest("header"));
                    }
                    //部門性質
                    if (!string.IsNullOrEmpty(GetData.GetRequest("natrue")) && GetData.GetRequest("natrue").ToString() != "0")
                    {

                        where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format(" deptNature=N'{0}' ", GetData.GetRequest("natrue"));
                    }//部門組別
                    if (!string.IsNullOrEmpty(GetData.GetRequest("group")))
                    {

                        where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format(" (deptGroup like N'%{0}%')", GetData.GetRequest("group"));
                    }

                    //有效否
                    if (!string.IsNullOrEmpty(GetData.GetRequest("usy")))
                    {
                        where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format("Usy=N'{0}'", GetData.GetRequest("usy"));
                    }
                    break;
            }
            //取得所有部门资料信息的集合
            DataTable dt = dservice.GetAllDept(row, page, out pageCount, out total, where);
            StringBuilder sb = new StringBuilder();
            sb = JsonHelper.DataTableToJSON(dt, total, true);
            context.Response.Write(sb);
        }
        /// <summary>
        /// 删除部门资料数据
        /// </summary>
        /// <param name="context"></param>
        protected void DelDept(HttpContext context)
        {
            try
            {
                string idstr = GetData.GetRequest("deptIDstr").To_String();

                List<string> arr = idstr.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                string result = string.Empty;
                result = dservice.DelDept(arr);
                if (result == "SB0007")
                {

                    LogService logs = new LogService();

                    //写进系统日志
                    t_SysLog syslog = new t_SysLog();
                    syslog.operatorID = uservice.GetUserMain().userID.To_String();
                    syslog.operatorName = uservice.GetUserMain().userName.To_String();
                    syslog.refProgram = "Dept.aspx";
                    syslog.refClass = "DeptService";
                    syslog.refMethod = "DelDept";
                    syslog.refRemark = "delete deptment and deptID=" + idstr + "";
                    syslog.refTime = System.DateTime.Now;
                    syslog.refIP = GetData.GetUserIP().ToString();
                    syslog.refSql = "p_DelDept";
                    syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                    logs.WriteLog(syslog);
                    context.Response.Write("{\"success\":true}");
                }
                else
                {
                    result = languageManage.GetResourceText(result, LanguageId);
                    context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
                }
            }
            catch (Exception e)
            {
                //写进系统日志
                t_SysLog syslog = new t_SysLog();
                syslog.operatorID = userive.GetUserMain().userID.To_String();
                syslog.operatorName = userive.GetUserMain().userName.To_String();
                syslog.refProgram = "/Organize/Dept.aspx";
                syslog.refClass = "DeptManage.ashx";
                syslog.refMethod = "DelDept";
                syslog.refRemark = e.ToString();
                syslog.refTime = System.DateTime.Now;
                syslog.refIP = GetData.GetUserIP().ToString();
                syslog.refSql = "";
                syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                logs.WriteLog(syslog);

                string result = "newLabel099";
                result = languageManage.GetResourceText(result, LanguageId);
                context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
            }
        }
        /// <summary>
        /// 更新部门资料信息
        /// </summary>
        /// <param name="context"></param>
        protected void ModDept(HttpContext context)
        {
            string result = string.Empty;
            try
            {
                //獲取更新后的數據

                string deptID = GetData.GetRequest("deptid").Trim();
                string simpleName = GetData.GetRequest("simplename").To_String();
                string header = GetData.GetRequest("deptheader").Trim();
                string nature = GetData.GetRequest("deptnatureid").Trim();
                string fullname = GetData.GetRequest("fullname").Trim();
                string group = GetData.GetRequest("deptgroup").Trim();
                string usy = GetData.GetRequest("usy").Trim();
                string dd = GetData.GetRequest("dd").Trim();
                string falsedeptID = GetData.GetRequest("falsedeptid").Trim();

                //組織新的數據實體
                t_Dept deptment = new t_Dept();
                deptment.deptID = deptID;
                deptment.falseDeptID = falsedeptID;
                deptment.simpleName = simpleName;
                deptment.deptHeader = header;
                deptment.deptNature = nature;
                deptment.fullName = fullname;
                deptment.deptGroup = group;
                deptment.usy = usy;
                deptment.companyID = uservice.GetUserMain().Company.companyID.To_String();
                deptment.updaterID = uservice.GetUserMain().userID.To_String(); ;
                deptment.uDeptID = uservice.GetUserMain().userDept.To_String();

                if (!string.IsNullOrEmpty(deptment.deptHeader))
                {
                    //驗證員工是否存在
                    bool orr = empservice.IsExistEmp(deptment.deptHeader);
                    if (orr != true)
                    {
                        result = "EB0022";
                    }

                }
                if (result == "EB0022")
                {
                    //返回多語言的值
                    //result = languageManage.GetResourceText(result, LanguageId);
                    //context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
                    context.Response.End();
                }

                if (!string.IsNullOrEmpty(deptment.deptGroup))
                {
                    //检查部门资料信息是否存在
                    bool rer = dservice.IsExitDept(deptment.deptGroup);
                    if (rer != true)
                    {
                        result = "EB0023";
                    }
                    if (result == "EB0023")
                    {
                        //返回多語言的值
                        //result = languageManage.GetResourceText(result, LanguageId);
                        //context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
                        context.Response.End();
                    }
                }
                result = dservice.UpdateDept(deptment);
                if (result == "SB0004")
                {
                    LogService logs = new LogService();

                    //写进系统日志
                    t_SysLog syslog = new t_SysLog();
                    syslog.operatorID = deptment.updaterID;
                    syslog.operatorName = uservice.GetUserMain().userName.To_String();
                    syslog.refProgram = "Dept.aspx";
                    syslog.refClass = "DeptService";
                    syslog.refMethod = "UpdateDept";
                    syslog.refRemark = "update deptment and deptID=" + deptment.deptID + "";
                    syslog.refTime = System.DateTime.Now;
                    syslog.refIP = GetData.GetUserIP().ToString();
                    syslog.refSql = "p_ModDept";
                    syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                    logs.WriteLog(syslog);
                    context.Response.Write("{\"success\":true}");
                }
                else
                    context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
            }
            catch (Exception e)
            {
                //写进系统日志
                t_SysLog syslog = new t_SysLog();
                syslog.operatorID = userive.GetUserMain().userID.To_String();
                syslog.operatorName = userive.GetUserMain().userName.To_String();
                syslog.refProgram = "/Organize/Dept.aspx";
                syslog.refClass = "DeptManage.ashx";
                syslog.refMethod = "ModDept";
                syslog.refRemark = e.ToString();
                syslog.refTime = System.DateTime.Now;
                syslog.refIP = GetData.GetUserIP().ToString();
                syslog.refSql = "";
                syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                logs.WriteLog(syslog);

                if (result == "EB0022")
                {
                    result = languageManage.GetResourceText(result, LanguageId);
                    context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
                }
                else if (result == "EB0023")
                {
                    result = languageManage.GetResourceText(result, LanguageId);
                    context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
                }
                else
                {
                    result = "newLabel100";
                    result = languageManage.GetResourceText(result, LanguageId);
                    context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
                }
            }

        }
        /// <summary>
        /// 新增部門信息
        /// </summary>
        /// <param name="context"></param>
        protected void AddDept(HttpContext context)
        {
            string result = string.Empty;
            try
            {
                string deptID = GetData.GetRequest("deptid").Trim();
                string simpleName = GetData.GetRequest("simplename").To_String();
                string header = GetData.GetRequest("deptheader").Trim();
                string nature = GetData.GetRequest("deptnatureid").Trim();
                string fullname = GetData.GetRequest("fullname").Trim();
                string group = GetData.GetRequest("deptgroup").Trim();
                string usy = GetData.GetRequest("usy").Trim();
                string dd = GetData.GetRequest("dd").Trim();
                if (deptID != "" && simpleName != "")
                {

                    t_Dept dept = new t_Dept();
                    dept.deptID = deptID;
                    dept.falseDeptID = deptID;
                    dept.simpleName = simpleName;
                    dept.deptHeader = header;
                    if (!string.IsNullOrEmpty(dept.deptHeader))
                    {
                        //驗證員工是否存在
                        bool orr = empservice.IsExistEmp(dept.deptHeader);
                        if (orr != true)
                        {
                            result = "EB0022";
                        }

                    }
                    if (result == "EB0022")
                    {
                        //轉換成多語言
                        //result = languageManage.GetResourceText(result, LanguageId);
                        //context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
                        context.Response.End();

                    }
                    dept.deptNature = nature;
                    dept.fullName = fullname;
                    dept.deptGroup = group;
                    if (!string.IsNullOrEmpty(dept.deptGroup))
                    {
                        //检查部门资料信息是否存在
                        bool rer = dservice.IsExitDept(dept.deptGroup);
                        if (rer != true)
                        {
                            result = "EB0023";
                        }
                        if (result == "EB0023")
                        {
                            //result = languageManage.GetResourceText(result, LanguageId);
                            //context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
                            context.Response.End();
                        }
                        //取得部门性质
                        string natre = dservice.GetDeptNature(dept.deptGroup);

                    }
                    dept.usy = usy;
                    dept.createTime = (DateTime)System.DateTime.Now;
                    //從存儲信息中取得登錄用戶的某些信息

                    dept.createrID = uservice.GetUserMain().userID.To_String();
                    dept.cDeptID = uservice.GetUserMain().userDept.To_String();
                    dept.companyID = uservice.GetUserMain().Company.companyID.To_String();
                    result = dservice.AddDept(dept);

                    if (result == "SB0001")
                    {

                        LogService logs = new LogService();

                        //写进系统日志
                        t_SysLog syslog = new t_SysLog();
                        syslog.operatorID = dept.createrID;
                        syslog.operatorName = uservice.GetUserMain().userName.To_String();
                        syslog.refProgram = "Dept.aspx";
                        syslog.refClass = "DeptService";
                        syslog.refMethod = "AddDept";
                        syslog.refRemark = "create new deptment and deptID=" + dept.deptID + "";
                        syslog.refTime = System.DateTime.Now;
                        syslog.refIP = GetData.GetUserIP().ToString();
                        syslog.refSql = "p_AddDept";
                        syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                        logs.WriteLog(syslog);
                        context.Response.Write("{\"success\":true}");
                    }
                    else
                    {
                        result = languageManage.GetResourceText(result, LanguageId);
                        context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");

                    }
                }
                else
                {
                    result = "deptnoempty";
                    result = languageManage.GetResourceText(result, LanguageId);
                    context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
                }
            }
            catch (Exception e)
            {
                //写进系统日志
                t_SysLog syslog = new t_SysLog();
                syslog.operatorID = userive.GetUserMain().userID.To_String();
                syslog.operatorName = userive.GetUserMain().userName.To_String();
                syslog.refProgram = "/Organize/Dept.aspx";
                syslog.refClass = "DeptManage.ashx";
                syslog.refMethod = "AddDept";
                syslog.refRemark = e.ToString();
                syslog.refTime = System.DateTime.Now;
                syslog.refIP = GetData.GetUserIP().ToString();
                syslog.refSql = "";
                syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                logs.WriteLog(syslog);


                if (result == "EB0022")
                {
                    result = languageManage.GetResourceText(result, LanguageId);
                    context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
                }
                else if (result == "EB0023")
                {
                    result = languageManage.GetResourceText(result, LanguageId);
                    context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
                }
                else
                {
                    result = "newLabel101";
                    result = languageManage.GetResourceText(result, LanguageId);
                    context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
                }

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