using System.Web;
using Utility.HelpClass;
using DMC.BLL;

namespace Web.ASHX
{
    /// <summary>
    /// 用戶資料操作管理
    /// </summary>
    public class GetUserInfo : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            //接收參數
            context.Response.ContentType = "text/plain";
            string userName = context.Request.Params["userName"];
            string deptID = context.Request.Params["deptID"];
            string menuID = context.Request.Params["menuID"];
            string empID = context.Request.Params["empID"];

            userName = context.Server.UrlDecode(userName);
            deptID = context.Server.UrlDecode(deptID);
            menuID = context.Server.UrlDecode(menuID);
            empID = context.Server.UrlEncode(empID);
            //根據用戶名稱的關鍵字帶出包含該關鍵字的所有用戶列表
            if (!string.IsNullOrEmpty(userName))
            {
                GetUserNameInfo(userName, context);
            }
            //根据部门ID的关键字取得部门的列表
            if (!string.IsNullOrEmpty(deptID))
            {
                GetDeptListByKey(deptID, context);
            }
            //根据菜單ID的关键字取得菜單的列表
            if (!string.IsNullOrEmpty(menuID))
            {
                GetMenuListByKey(menuID, context);
            }
            //根據員工編號的關鍵字獲取包含該關鍵字的員工列表
            if (!string.IsNullOrEmpty(empID))
            {
                GetEmpListByKey(empID, context);
            }

        }
        /// <summary>
        /// 根據用戶名稱的關鍵字帶出包含該關鍵字的所有用戶列表
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="context"></param>
        public void GetUserNameInfo(string userName, HttpContext context)
        {
            context.Response.Write(JsonHelper.ObjectToJson(UserManageService.GetUserInfo(userName)));
        }
        /// <summary>
        /// 根据部门ID的关键字取得部门的列表
        /// </summary>
        /// <param name="deptID"></param>
        public void GetDeptListByKey(string deptID, HttpContext context)
        {
            context.Response.Write(JsonHelper.ObjectToJson(DeptService.GetDeptListByKey(deptID)));
        }
        /// <summary>
        /// 根据菜單ID的关键字取得菜單的列表
        /// </summary>
        /// <param name="menuID"></param>
        /// <param name="context"></param>
        public void GetMenuListByKey(string menuID, HttpContext context)
        {
            context.Response.Write(JsonHelper.ObjectToJson(MenusService.GetMenuListByKey(menuID)));
        }
        /// <summary>
        /// 根據員工編號的關鍵字獲取包含該關鍵字的員工列表
        /// </summary>
        /// <param name="empID">員工編號</param>
        /// <param name="context"></param>
        public void GetEmpListByKey(string empID, HttpContext context)
        {
            context.Response.Write(JsonHelper.ObjectToJson(EmpService.GetEmpListByKey(empID)));
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}