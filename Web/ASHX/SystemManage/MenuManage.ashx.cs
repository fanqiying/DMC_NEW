using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using Utility.HelpClass;
using System.Web.SessionState;
using DMC.BLL;
using DMC.Model;

namespace Web.ASHX
{
    /// <summary>
    /// 系統菜單操作
    /// code by jeven
    /// 2013-6-23
    /// </summary>
    public class MenuManage : IHttpHandler, IRequiresSessionState
    {
        //變量定義以及相關類的實例化
        private UserManageService us = new UserManageService();
        private MenusService ms = new MenusService();
        private LanguageManageService languageManage = new LanguageManageService();
        private UserManageService uservice = new UserManageService();
        private UserManageService umanage = new UserManageService();
        private UserInfo currentUser = null;
        private string UserId = "a10857";
        private string DeptId = "IT600";
        private string LanguageId = "0002";
        private string CompanyId = "avccn";
        private string newNodeValue = string.Empty;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string method = GetData.GetRequest("M").ToLower();
            //從session中獲取登錄用戶的相關信息
            currentUser = umanage.GetUserMain();
            if (currentUser != null)
            {
                UserId = currentUser.userID.ToString();//獲取用戶名
                DeptId = currentUser.userDept.ToString();//獲取用戶部門
                LanguageId = currentUser.LanguageId.ToString();//獲取登錄語言別
                CompanyId = currentUser.Company.ToString();
            }
            //獲取操作方法名
            switch (method)
            {
                case "add":
                    AddMenu(context);//新增菜單
                    break;
                case "load"://加載菜單數據
                    LoadMenu(context);
                    break;
                case "delete"://刪除菜單
                    DelMenu(context);
                    break;
                case "update"://更新菜單資料
                    ModMenu(context);
                    break;
                case "getnode"://獲取最大的菜單節點
                    GetNodeMaxMenuID(context);
                    break;
            }
        }

        /// <summary>
        /// 獲取最大的菜單節點
        /// </summary>
        /// <param name="context"></param>
        private void GetNodeMaxMenuID(HttpContext context)
        {
            string selMenuID = GetData.GetRequest("selectedNode").To_String();
            if (selMenuID != "")
            {
                newNodeValue = ms.GetNodeMaxMenuID(selMenuID);
                if (newNodeValue != "")
                {
                    newNodeValue = ms.NextNumber(newNodeValue);
                    context.Response.Write("OK|" + newNodeValue);
                }
                else
                {
                    newNodeValue = selMenuID + "01";
                    context.Response.Write("OK|" + newNodeValue);
                }

            }
        }
        /// <summary>
        /// 加載菜單數據
        /// </summary>
        /// <param name="context"></param>
        private void LoadMenu(HttpContext context)
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

            DataTable progDT = ms.GetAllMenu(row, page, out pageCount, out total, where);
            StringBuilder sb = new StringBuilder();
            sb = JsonHelper.DataTableToJSON(progDT, total, true);
            context.Response.Write(sb);
        }
        /// <summary>
        /// 更新菜單資料
        /// </summary>
        /// <param name="context"></param>
        private void ModMenu(HttpContext context)
        {
            string result = string.Empty;
            try
            {

                //接收傳參
                string menuID = GetData.GetRequest("menuid").Trim();
                string menuName = GetData.GetRequest("menuname").Trim();
                string fatherID = GetData.GetRequest("fatherID").Trim();
                string orderid = GetData.GetRequest("orderid").Trim();
                string usy = GetData.GetRequest("usy").Trim();
                //實例化對象實體
                t_Menu menu = new t_Menu();
                //組織實體對象數據
                menu.menuID = menuID;
                menu.menuName = menuName;
                menu.fatherID = fatherID;
                menu.usy = usy;
                //從存儲信息中取得登錄用戶的某些信息
                menu.updaterID = us.GetUserMain().userID.To_String();
                menu.orderid = string.IsNullOrEmpty(orderid) ? 0 : Convert.ToInt32(orderid);
                menu.uDeptID = us.GetUserMain().userDept.To_String();
                //驗證是否存在需要更改的菜單編號
                if (ms.IsExitMenu(menu.menuID) != true)
                {
                    result = "EB0036";
                }
                if (result == "EB0036")
                {
                    //result = languageManage.GetResourceText(result, LanguageId);
                    //context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
                    context.Response.End();
                }
                //調用系統的業務方法執行更新操作并返回執行的結果
                result = ms.ModMenu(menu);
                if (result == "SB0019")
                {
                    LogService logs = new LogService();

                    //写进系统日志
                    t_SysLog syslog = new t_SysLog();
                    syslog.operatorID = us.GetUserMain().userID.To_String();
                    syslog.operatorName = us.GetUserMain().userName.To_String();
                    syslog.refProgram = "MenuSetting.aspx";
                    syslog.refClass = "MenusService";
                    syslog.refMethod = "ModMenu";
                    syslog.refRemark = "update Menu and menuID=" + menu.menuID + "";
                    syslog.refTime = System.DateTime.Now;
                    syslog.refIP = GetData.GetUserIP().ToString();
                    syslog.refSql = "update";
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
                LogService logs = new LogService();
                //写进系统日志
                t_SysLog syslog = new t_SysLog();
                syslog.operatorID = us.GetUserMain().userID.To_String();
                syslog.operatorName = us.GetUserMain().userName.To_String();
                syslog.refProgram = "MenuSetting.aspx";
                syslog.refClass = "MenusService";
                syslog.refMethod = "ModMenu";
                syslog.refRemark = e.To_String();
                syslog.refTime = System.DateTime.Now;
                syslog.refIP = GetData.GetUserIP().ToString();
                syslog.refSql = "update";
                syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                logs.WriteLog(syslog);

                if (result == "EB0036")
                {
                    result = languageManage.GetResourceText(result, LanguageId);
                    context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
                }
                else
                {
                    result = "newLabel119";
                    result = languageManage.GetResourceText(result, LanguageId);
                    context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
                }

            }
        }
        /// <summary>
        /// 刪除菜單
        /// </summary>
        /// <param name="context"></param>
        private void DelMenu(HttpContext context)
        {
            try
            {
                //接收傳參
                string idstr = GetData.GetRequest("menuIDstr").To_String();
                //拆分字符串，組織成數組對象
                List<string> arr = idstr.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                string result = string.Empty;
                result = ms.DelMenu(arr);
                if (result == "SB0020")
                {

                    LogService logs = new LogService();

                    //写进系统日志
                    t_SysLog syslog = new t_SysLog();
                    syslog.operatorID = uservice.GetUserMain().userID.To_String();
                    syslog.operatorName = uservice.GetUserMain().userName.To_String();
                    syslog.refProgram = "MenuSetting.aspx";
                    syslog.refClass = "MenusService";
                    syslog.refMethod = "DelMenu";
                    syslog.refRemark = "delete Menu and menuID=" + idstr + "";
                    syslog.refTime = System.DateTime.Now;
                    syslog.refIP = GetData.GetUserIP().ToString();
                    syslog.refSql = "";
                    syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                    logs.WriteLog(syslog);
                    context.Response.Write("{\"success\":true}");
                }
                else
                {
                    context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
                }
            }
            catch (Exception e)
            {
                LogService logs = new LogService();
                //写进系统日志
                t_SysLog syslog = new t_SysLog();
                syslog.operatorID = us.GetUserMain().userID.To_String();
                syslog.operatorName = us.GetUserMain().userName.To_String();
                syslog.refProgram = "MenuSetting.aspx";
                syslog.refClass = "MenusService";
                syslog.refMethod = "DelMenu";
                syslog.refRemark = e.To_String();
                syslog.refTime = System.DateTime.Now;
                syslog.refIP = GetData.GetUserIP().ToString();
                syslog.refSql = "";
                syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                logs.WriteLog(syslog);


                string result = "newLabel120";
                result = languageManage.GetResourceText(result, LanguageId);
                context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
            }
        }


        /// <summary>
        /// 新增菜單
        /// </summary>
        /// <param name="context"></param>
        private void AddMenu(HttpContext context)
        {
            string result = string.Empty;
            try
            {
                //接收傳參

                string menuID = GetData.GetRequest("menuid").Trim();
                string menuName = GetData.GetRequest("menuname").Trim();
                string fatherID = GetData.GetRequest("fatherID").Trim();
                string orderid = GetData.GetRequest("orderid").Trim();
                string usy = GetData.GetRequest("usy").Trim();
                //實例化對象實體
                t_Menu menu = new t_Menu();
                //組織實體對象數據
                menu.menuID = menuID;
                menu.menuName = menuName;
                menu.fatherID = fatherID;
                menu.usy = usy;
                menu.createrID = us.GetUserMain().userID.To_String();
                menu.cDeptID = us.GetUserMain().userDept.To_String();
                menu.orderid = string.IsNullOrEmpty(orderid) ? 0 : Convert.ToInt32(orderid);
                if (ms.IsExitMenu(menu.menuID) == true)
                {
                    result = "EB0035";
                }
                if (result == "EB0035")
                {
                    //result = languageManage.GetResourceText(result, LanguageId);
                    //context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
                    context.Response.End();
                }

                result = ms.AddMenu(menu);
                if (result == "SB0018")
                {
                    LogService logs = new LogService();

                    //写进系统日志
                    t_SysLog syslog = new t_SysLog();
                    syslog.operatorID = us.GetUserMain().userID.To_String();
                    syslog.operatorName = us.GetUserMain().userName.To_String();
                    syslog.refProgram = "MenuSetting.aspx";
                    syslog.refClass = "MenusService";
                    syslog.refMethod = "AddMenu";
                    syslog.refRemark = "create new Menu and menuID=" + menu.menuID + "";
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
            catch (Exception e)
            {
                LogService logs = new LogService();
                //写进系统日志
                t_SysLog syslog = new t_SysLog();
                syslog.operatorID = us.GetUserMain().userID.To_String();
                syslog.operatorName = us.GetUserMain().userName.To_String();
                syslog.refProgram = "MenuSetting.aspx";
                syslog.refClass = "MenusService";
                syslog.refMethod = "AddMenu";
                syslog.refRemark = e.To_String();
                syslog.refTime = System.DateTime.Now;
                syslog.refIP = GetData.GetUserIP().ToString();
                syslog.refSql = "";
                syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                logs.WriteLog(syslog);

                if (result == "EB0035")
                {
                    result = languageManage.GetResourceText(result, LanguageId);
                    context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
                }
                else
                {
                    result = "newLabel121";
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