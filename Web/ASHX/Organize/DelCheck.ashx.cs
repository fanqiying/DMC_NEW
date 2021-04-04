using System.Web;
using Utility.HelpClass;
using DMC.BLL;
using System.Web.SessionState;

namespace Web.ASHX
{
    /// <summary>
    /// 刪除資料時驗證存在性 
    /// </summary>
    public class DelCheck : IHttpHandler, IRequiresSessionState
    {
        //實例化用戶業務操作類
        private UserManageService userManage = new UserManageService();
        private string companyID = string.Empty;

        public void ProcessRequest(HttpContext context)
        {
            //接參
            context.Response.ContentType = "text/plain";
            string deptID = context.Request.Params["deptID"];
            string empID = context.Request.Params["empID"];
            string fileID = context.Request.Params["fileID"];
            deptID = context.Server.UrlDecode(deptID);
            empID = context.Server.UrlDecode(empID);

            fileID = context.Server.UrlDecode(fileID);
            companyID = userManage.GetUserMain().Company.companyID.To_String();
            if (!string.IsNullOrEmpty(deptID))
            {
                //驗證該部門下是否存在某員工編號
                IsExitEmpByDeptID(deptID, context);
            }

            if (!string.IsNullOrEmpty(empID))
            {
                //驗證員工下是否含有帳號
                IsExitUserIDByEmpID(empID, context);
            }
        }

        /// <summary>
        /// 驗證部門下是否含有員工數據
        /// </summary>
        /// <param name="deptID"></param>
        public void IsExitEmpByDeptID(string deptID, HttpContext context)
        {
            EmpService emps = new EmpService();
            context.Response.Write(emps.IsExitEmpByDeptID(deptID));

        }

        /// <summary>
        /// 驗證員工下是否含有帳號
        /// </summary>
        /// <param name="empID">員工編號</param>
        /// <param name="context"></param>
        public void IsExitUserIDByEmpID(string empID, HttpContext context)
        {
            UserManageService us = new UserManageService();
            context.Response.Write(us.IsExitUserIDByEmpID(empID));

        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}