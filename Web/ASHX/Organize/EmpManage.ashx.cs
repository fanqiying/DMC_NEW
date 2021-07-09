using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utility.HelpClass;
using System.Text;
using System.Data;
using DMC.Model;
using DMC.BLL;

namespace Web.ASHX
{
    /// <summary>
    /// 员工操作管理
    /// code by jeven
    /// 2013-6-05
    /// </summary>
    public class EmpManage : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        //設置多語言程式編號
        private string _programId = string.Empty;
        public string ProgramId
        {
            get { return "sbmi003"; }
        }
        //變量初始化
        private string UserId = "a10857";
        private string DeptId = "IT600";
        private string LanguageId = "0002";
        private string companyID = "avccn";
        private UserInfo currentUser = null;
        //變量定義以及相關類的實例化
        public LanguageManageService languageManage = new LanguageManageService();
        private UserManageService uservice = new UserManageService();
        private EmpService eservice = new EmpService();
        private UserManageService us = new UserManageService();
        private DeptService dservice = new DeptService();
        private LogService logs = new LogService();
        private UserManageService userive = new UserManageService();


        private PermissionServices ps = new PermissionServices();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //獲取操作方法名
            string method = GetData.GetRequest("M").ToLower();
            currentUser = uservice.GetUserMain();
            UserId = currentUser.userID.To_String();//獲取用戶名
            DeptId = currentUser.userDept.To_String();//獲取用戶部門
            LanguageId = currentUser.LanguageId.ToString();//獲取登錄語言別        
            companyID = currentUser.Company.companyID.ToString();//獲取登錄公司別
            switch (method)
            {
                case "search"://按條件搜索員工信息
                    Search(context);
                    break;
                case "delete"://刪除員工信息
                    DelEmp(context);
                    break;
                case "add":
                    AddEmp(context);//新增員工信息
                    break;
                case "update"://更新員工信息
                    ModEmp(context);
                    break;
                case "loademptel"://獲取員工分機號碼
                    LoadEmpTel(context);
                    break;
                case "getmanageid"://獲取員工上級主管編號
                    GetManageID(context); break;
                case "getmanagelikeid"://獲取員工主管帶關鍵字搜索
                    GetManageLikeID(context); break;
                case "autotips"://根據用戶關鍵字獲取員工詳細信息
                    GetEmpInfoByID(context);
                    break;
                case "searchgroup"://按條件搜索員工信息
                    SearchGroup(context);
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
        /// 根據用戶關鍵字獲取員工詳細信息
        /// </summary>
        /// <param name="context"></param>
        private void GetEmpInfoByID(HttpContext context)
        {
            var key = GetData.GetRequest("empid").Trim();
            if (!string.IsNullOrEmpty(key))
            {
                string strWhere = " and empid like '%" + key + "%'";
                DataTable dt = eservice.GetEmpInfoByID(strWhere);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string dd = JsonHelper.DataTableToJSON(dt).ToString();
                    context.Response.Write(dd);
                }
            }
        }
        /// <summary>
        /// 獲取員工主管帶關鍵字搜索
        /// </summary>
        /// <param name="context"></param>
        private void GetManageLikeID(HttpContext context)
        {
            if (context.Request["id"] != null)
            {
                string strWhere = " and empid like '%" + context.Request["id"] + "%'";
                DataTable dt = eservice.GetEmpList(strWhere);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string dd = JsonHelper.DataTableToJSON(dt).ToString();
                    context.Response.Write(dd);
                }
                else
                {
                    context.Response.Write("");
                }
            }
        }
        /// <summary>
        /// 獲取員工上級主管編號
        /// </summary>
        /// <param name="context"></param>
        private void GetManageID(HttpContext context)
        {
            if (context.Request["id"] != null)
            {
                string strWhere = " and empid='" + context.Request["id"] + "'";
                DataTable dt = eservice.GetEmpList(strWhere);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string dd = JsonHelper.DataTableToJSON(dt).ToString();
                    context.Response.Write(dd);
                }
                else
                {
                    context.Response.Write("");
                }
            }
        }
        /// <summary>
        /// 獲取員工分機號碼
        /// </summary>
        /// <param name="context"></param>
        public void LoadEmpTel(HttpContext context)
        {
            string Create = context.Request.Params["Create"];
            if (Create.Split('-')[0] != "")
                Create = Create.Split('-')[0];
            else
                Create = context.Request.Params["Create"];
            string Update = context.Request.Params["Update"];
            DataTable dt = eservice.GetEmpTel(Create, Update);
            string dd = JsonHelper.DataTableToJSON(dt).ToString();
            context.Response.Write(dd);
        }
        /// <summary>
        /// 按條件搜索員工信息
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
                    string keyStr = languageManage.GetResourceText("InputDefaultKey", LanguageId);
                    if (!string.IsNullOrEmpty(key) && key != keyStr)
                    {
                        where += string.Format("empID like N'%{0}%' or empName like N'%{0}%' or empDept like N'%{0}%' or empTitle like N'%{0}%' or extTelNo like N'%{0}%' or empMail like N'%{0}%' or usy=N'{0}'", key);
                    }
                    break;
                //高級搜索
                case "ByAdvanced":
                    if (!string.IsNullOrEmpty(GetData.GetRequest("empid")))
                    {
                        //員工編號
                        where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format(" empID like N'%{0}%' ", GetData.GetRequest("empid"));
                    }
                    if (!string.IsNullOrEmpty(GetData.GetRequest("empname")))
                    {
                        //員工姓名
                        where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format(" (empName like N'%{0}%') ", GetData.GetRequest("empname"));
                    }
                    if (!string.IsNullOrEmpty(GetData.GetRequest("empdept")))
                    {
                        //員工部門
                        where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format(" (empDept =N'{0}') ", GetData.GetRequest("empdept"));
                    }
                    if (!string.IsNullOrEmpty(GetData.GetRequest("emptitle")) && GetData.GetRequest("emptitle") != "")
                    {
                        //員工職稱
                        where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format(" (empTitle like N'%{0}%') ", GetData.GetRequest("emptitle"));
                    }
                    if (!string.IsNullOrEmpty(GetData.GetRequest("exttelno")))
                    {
                        //員工分機號碼
                        where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format(" (extTelNo like N'%{0}%') ", GetData.GetRequest("exttelno"));
                    }
                    if (!string.IsNullOrEmpty(GetData.GetRequest("empmail")))
                    {
                        //員工郵箱
                        where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format(" (empMail like N'%{0}%') ", GetData.GetRequest("empmail"));
                    }

                    if (!string.IsNullOrEmpty(GetData.GetRequest("signerid")))
                    {
                        //簽核人
                        where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format(" (signerID like N'%{0}%') ", GetData.GetRequest("signerid"));
                    }
                    if (!string.IsNullOrEmpty(GetData.GetRequest("usy")))
                    {
                        //有效否
                        where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format("  (usy=N'{0}') ", GetData.GetRequest("usy"));
                    }
                    break;
            }
            //獲取用戶操作的部門權限
            List<DeptDataRight> deptList = ps.GetUserProgramData(UserId, ProgramId);
            StringBuilder sb = new StringBuilder();
            DataTable dt = new DataTable();
            //檢查資料權限
            if (deptList.Count > 0)
            {
                where += (string.IsNullOrEmpty(where) ? "" : " AND ") + " ( ";
                bool isFirst = true;
                foreach (DeptDataRight ddr in deptList)
                {
                    if (ddr.DeptId != "0")
                    {   //有權限
                        if (ddr.IsAll == "Y")
                        {
                            where += string.Format((isFirst ? "" : "OR") + " (empDept=N'{0}')", ddr.DeptId);
                        }
                        else
                        {
                            //沒有權限
                            where += string.Format((isFirst ? "" : "OR") + " (empDept=N'{0}' and createrID=N'{1}')", ddr.DeptId, UserId);
                        }
                        isFirst = false;
                    }
                    else
                    {
                        if (ddr.IsAll != "Y")
                        {
                            where += string.Format(" createrID=N'{0}' ", UserId);
                        }
                        else
                        {
                            where += " 1=1 ";
                        }
                        break;
                    }
                    isFirst = false;
                }
                where += " )";
            }
            //根據條件取得員工資料集合
            dt = eservice.GetAllEmp(row, page, out pageCount, out total, where);
            sb = JsonHelper.DataTableToJSON(dt, total, true);
            context.Response.Write(sb);
        }
        /// <summary>
        /// 刪除員工信息
        /// </summary>
        /// <param name="context"></param>
        protected void DelEmp(HttpContext context)
        {
            try
            {
                string idstr = GetData.GetRequest("empIDstr").To_String();

                List<string> arr = idstr.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                string result = string.Empty;
                result = eservice.DelEmp(arr);
                if (result == "SB0009")
                {
                    context.Response.Write("{\"success\":true}");
                }
                else
                {
                    context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
                }
            }
            catch
            {
                string result = "newLabel102";
                result = languageManage.GetResourceText(result, LanguageId);
                context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
            }
        }
        /// <summary>
        /// 新增員工信息
        /// </summary>
        /// <param name="context"></param>
        protected void AddEmp(HttpContext context)
        {
            string result = string.Empty;
            try
            {
                //實例化對象實體
                t_Employee emp = new t_Employee();
                emp.empID = GetData.GetRequest("empid").Trim();
                emp.companyID = GetData.GetRequest("companyid").To_String();
                emp.empName = GetData.GetRequest("empname").Trim();
                emp.empDept = GetData.GetRequest("empdept").Trim();
                emp.empTitle = GetData.GetRequest("emptitle").Trim();
                emp.extTelNo = GetData.GetRequest("exttelno").Trim();
                emp.empMail = GetData.GetRequest("empmail").Trim();
                emp.signerID = GetData.GetRequest("signerid").Trim();
                emp.usy = GetData.GetRequest("usy").Trim();
                //從存儲信息中取得登錄用戶的某些信息
                emp.createrID = us.GetUserMain().userID.To_String();
                emp.cDeptID = us.GetUserMain().userDept.To_String();

                if (emp.empID != "" && emp.empName != "")
                {
                    if (!string.IsNullOrEmpty(emp.empDept) && !dservice.IsExitDept(emp.empDept))
                    {
                        result = "EB0023";
                    }

                    if (string.IsNullOrWhiteSpace(result) && !string.IsNullOrEmpty(emp.signerID) && !us.IsExitUserID(emp.signerID))
                    {
                        result = "EB0022";
                    }

                    if (string.IsNullOrWhiteSpace(result))
                    {
                        //調用系統業務方法，返回執行結果
                        result = eservice.AddEmp(emp);
                    }

                    if (result == "SB0002")
                    {
                        context.Response.Write("{\"success\":true}");
                    }
                    else
                    {
                        result = languageManage.GetResourceText(result, LanguageId);
                        context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
                    }
                }
            }
            catch
            {
                if (result != "EB0023" && result != "EB0023")
                {
                    result = "newLabel103";
                }
                result = languageManage.GetResourceText(result, LanguageId);
                context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
            }
        }
        /// <summary>
        /// 更新員工信息
        /// </summary>
        /// <param name="context"></param>
        protected void ModEmp(HttpContext context)
        {
            string result = string.Empty;
            try
            {
                //實例化對象實體
                t_Employee emp = new t_Employee();
                emp.empID = GetData.GetRequest("empid").Trim();
                emp.companyID = GetData.GetRequest("companyid").To_String();
                emp.empName = GetData.GetRequest("empname").Trim();
                emp.empDept = GetData.GetRequest("empdept").Trim();
                emp.empTitle = GetData.GetRequest("emptitle").Trim();
                emp.extTelNo = GetData.GetRequest("exttelno").Trim();
                emp.empMail = GetData.GetRequest("empmail").Trim();
                emp.signerID = GetData.GetRequest("signerid").Trim();
                emp.usy = GetData.GetRequest("usy").Trim();
                //從存儲信息中取得登錄用戶的某些信息
                emp.updaterID = us.GetUserMain().userID.To_String();
                emp.uDeptID = us.GetUserMain().userDept.To_String();
                if (!string.IsNullOrEmpty(emp.empDept) && !dservice.IsExitDept(emp.empDept))
                {
                    result = "EB0023";
                }
                if (string.IsNullOrWhiteSpace(result) && !string.IsNullOrEmpty(emp.signerID) && !us.IsExitUserID(emp.signerID))
                {
                    result = "EB0022";
                }
                if (string.IsNullOrWhiteSpace(result))
                {
                    result = eservice.ModEmp(emp);
                }

                if (result == "SB0006")
                {
                    context.Response.Write("{\"success\":true}");
                }
                else
                {
                    context.Response.Write("{\"success\":false,\"msg\":\"" + languageManage.GetResourceText(result, LanguageId) + "\"}");
                }
            }
            catch
            {
                result = "newLabel104";
                context.Response.Write("{\"success\":false,\"msg\":\"" + languageManage.GetResourceText(result, LanguageId) + "\"}");
            }
        }

        /// <summary>
        /// 按條件搜索員工信息
        /// </summary>
        /// <param name="context"></param>
        protected void SearchGroup(HttpContext context)
        {
            //每页多少條數據
            int row = GetData.GetRequest("rows").To_Int();
            //當前第幾页
            int page = GetData.GetRequest("page").To_Int();
            //總共多少條數據   
            //int i = Count();
            int pageCount = 0;
            int total = 0;
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.Append(" Usy='Y' ");
            string type = GetData.GetRequest("SearchType").To_String();
            switch (type)
            {
                //關鍵字搜索
                case "ByKey":
                    string key = GetData.GetRequest("KeyWord");
                    string keyStr = languageManage.GetResourceText("InputDefaultKey", LanguageId);
                    if (!string.IsNullOrEmpty(key) && key != keyStr)
                    {
                        sbWhere.AppendFormat(" AND (empID like N'%{0}%' or empName like N'%{0}%' or empMail like N'%{0}%' or empDept like N'%{0}%') ", key);
                    }
                    //if (!string.IsNullOrEmpty(GetData.GetRequest("rose")))
                    //{
 
                    //}
                    break;
            }
            //过滤掉已存在的用户
            string empids = GetData.GetRequest("empids");
            if (!string.IsNullOrWhiteSpace(empids))
            {
                List<String> ids = empids.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                sbWhere.AppendFormat(" AND NOT empID IN('{0}')", string.Join("','", ids));
            }

            string program = GetData.GetRequest("program");
            if (!string.IsNullOrWhiteSpace(program))
            {
                sbWhere.AppendFormat(" AND empID IN(select distinct UserID from dbo.t_Right_Urole where roseid in(SELECT distinct RoseId FROM dbo.t_Right_Rprogram WHERE ProgramId='{0}'))", program);
            }
            //根據條件取得員工資料集合
            DataTable dt = eservice.GetAllEmp(row, page, out pageCount, out total, sbWhere.ToString(), "AutoId,EmpId,EmpName,EmpMail, EmpId + '/' + EmpName as DisaplyText");
            StringBuilder sb = JsonHelper.DataTableToJSON(dt, total, true);
            context.Response.Write(sb);
        }
    }
}