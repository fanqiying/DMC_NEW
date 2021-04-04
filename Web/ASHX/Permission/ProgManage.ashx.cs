using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utility.HelpClass;
using System.Data;
using System.Text;
using DMC.Model;
using DMC.BLL;

namespace Web.ASHX
{
    /// <summary>
    /// 系統程式處理
    /// code by jeven
    /// 2013-6-18
    /// </summary>
    public class ProgManage : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        //變量定義以及相關類的實例化
        private string UserId = "a10857";
        private string DeptId = "IT600";
        private string LanguageId = "0002";
        private UserInfo currentUser = null;
        public LanguageManageService languageManage = new LanguageManageService();
        private UserManageService uservice = new UserManageService();
        private EmpService eservice = new EmpService();
        private UserManageService us = new UserManageService();
        private DeptService dservice = new DeptService();
        private LogService logs = new LogService();
        private ProgramService ps = new ProgramService();
        public void ProcessRequest(HttpContext context)
        {
            //從session中獲取登錄用戶的相關信息
            currentUser = uservice.GetUserMain();
            UserId = currentUser.userID.ToString();//獲取用戶名
            DeptId = currentUser.userDept.ToString();//獲取用戶部門
            LanguageId = currentUser.LanguageId.ToString();//獲取登錄語言別
            context.Response.ContentType = "text/plain";
            //獲取操作方法名
            string method = GetData.GetRequest("M").ToLower();
            switch (method)
            {
                case "add"://新增程式
                    AddProgram(context);
                    break;
                case "search"://按條件搜索程式數據列表
                    Search(context);
                    break;
                case "delete"://刪除程式
                    DelProgram(context);
                    break;
                case "update"://更新程式資料信息
                    ModProgram(context);
                    break;
            }
        }
        /// <summary>
        /// 更新程式資料信息
        /// </summary>
        /// <param name="context"></param>
        private void ModProgram(HttpContext context)
        {
            try
            {
                string result = string.Empty;
                string programID = GetData.GetRequest("programid").Trim();
                string programName = GetData.GetRequest("programname").Trim();
                string functionID = GetData.GetRequest("ids").Trim();
                string usy = GetData.GetRequest("usy").Trim();
                string functionNames = GetData.GetRequest("names").Trim();
                string menuUrl = GetData.GetRequest("menuurl").Trim();
                string menuid = GetData.GetRequest("menu_id").Trim();
                string orderid = GetData.GetRequest("orderid").Trim();
                string ismobile = GetData.GetRequest("ismobile").Trim();
                string mobileurl = GetData.GetRequest("mobileurl").Trim();
                List<string> arr = functionID.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                //验证是否该程式有基本执行功能
                if (ps.IsExitProFunc(programID) == true)
                {
                    //先删除原来的程式的基本功能
                    ps.DelProgFunc(programID);
                }
                //执行增加操作
                result = ps.SetProgFunc(arr, programID);
                if (result == "SB0013")
                {
                    //设置程式与菜单的关联
                    if (ps.IsExitProgVsMenu(programID) == true)
                    {
                        //存在关联的话就执行更新操作
                        result = ps.ModProgMenu(programID, menuid);
                    }
                    else
                    {
                        t_ProgRefMenu pm = new t_ProgRefMenu();
                        pm.programID = programID;
                        pm.menuID = menuid;
                        pm.usy = usy;
                        result = ps.AddProgMenu(pm);
                    }


                    //执行增加程式操作方法

                    t_Program prg = new t_Program();
                    prg.programID = programID;
                    prg.programName = programName;
                    prg.menuUrl = menuUrl;
                    prg.functionStr = functionNames;
                    prg.updaterID = uservice.GetUserMain().userID.To_String();
                    prg.uDeptID = uservice.GetUserMain().userDept.To_String();
                    prg.menuId = menuid;
                    prg.orderid = string.IsNullOrEmpty(orderid) ? 1 : Convert.ToInt32(orderid);
                    prg.usy = usy;
                    prg.IsMobile = string.IsNullOrEmpty(ismobile) ? "N" : ismobile;
                    prg.MobileUrl = mobileurl;

                    if (prg != null)
                    {
                        result = ps.ModProgram(prg);
                        if (result == "SB0011")
                        {
                            LogService logs = new LogService();
                            //写进系统日志
                            t_SysLog syslog = new t_SysLog();
                            syslog.operatorID = uservice.GetUserMain().userID.To_String();
                            syslog.operatorName = uservice.GetUserMain().userName.To_String();
                            syslog.refProgram = "ProgramSet.aspx";
                            syslog.refClass = "ProgramService";
                            syslog.refMethod = "ModProgram";
                            syslog.refRemark = "update program and programID=" + prg.programID + "";
                            syslog.refTime = System.DateTime.Now;
                            syslog.refIP = GetData.GetUserIP().ToString();
                            syslog.refSql = "update t_Program";
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

                }
            }
            catch (Exception e)
            {

                //写进系统日志
                t_SysLog syslog = new t_SysLog();
                syslog.operatorID = uservice.GetUserMain().userID.To_String();
                syslog.operatorName = uservice.GetUserMain().userName.To_String();
                syslog.refProgram = "ProgramSet.aspx";
                syslog.refClass = "ProgramService";
                syslog.refMethod = "ModProgram";
                syslog.refRemark = e.ToString();
                syslog.refTime = System.DateTime.Now;
                syslog.refIP = GetData.GetUserIP().ToString();
                syslog.refSql = "";
                syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                logs.WriteLog(syslog);


                string result = "newLabel122";
                result = languageManage.GetResourceText(result, LanguageId);
                context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
            }
        }
        /// <summary>
        /// 刪除程式
        /// </summary>
        /// <param name="context"></param>
        private void DelProgram(HttpContext context)
        {
            try
            {
                string programID = GetData.GetRequest("programIDstr").Trim();
                List<string> arr = programID.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                string result = string.Empty;
                result = ps.DelProgram(arr);
                if (result == "SB0012")
                {
                    LogService logs = new LogService();
                    //写进系统日志
                    t_SysLog syslog = new t_SysLog();
                    syslog.operatorID = uservice.GetUserMain().userID.To_String();
                    syslog.operatorName = uservice.GetUserMain().userName.To_String();
                    syslog.refProgram = "ProgramSet.aspx";
                    syslog.refClass = "ProgramService";
                    syslog.refMethod = "DelProgram";
                    syslog.refRemark = "delete program and programID=" + arr + "";
                    syslog.refTime = System.DateTime.Now;
                    syslog.refIP = GetData.GetUserIP().ToString();
                    syslog.refSql = "delete from t_Program where programID in(" + arr + ")";
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

                //写进系统日志
                t_SysLog syslog = new t_SysLog();
                syslog.operatorID = uservice.GetUserMain().userID.To_String();
                syslog.operatorName = uservice.GetUserMain().userName.To_String();
                syslog.refProgram = "ProgramSet.aspx";
                syslog.refClass = "ProgramService";
                syslog.refMethod = "DelProgram";
                syslog.refRemark = e.ToString();
                syslog.refTime = System.DateTime.Now;
                syslog.refIP = GetData.GetUserIP().ToString();
                syslog.refSql = "";
                syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                logs.WriteLog(syslog);


                string result = "newLabel123";
                result = languageManage.GetResourceText(result, LanguageId);
                context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
            }
        }
        /// <summary>
        /// 按條件搜索程式數據列表
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
                case "ByKey"://關鍵字搜索
                    string key = GetData.GetRequest("KeyWord");
                    if (!string.IsNullOrEmpty(key) && key != languageManage.GetResourceText("InputKeyWord", LanguageId))
                        where = string.Format("programID like N'%{0}%' or programName like N'%{0}%'  or usy='{0}'", key);
                    break;
                case "ByAdvanced"://高級搜索
                    if (!string.IsNullOrEmpty(GetData.GetRequest("programid")))
                    {
                        where = string.Format(" programID like N'%{0}%' ", GetData.GetRequest("programid"));
                    }
                    if (!string.IsNullOrEmpty(GetData.GetRequest("programname")))
                    {
                        //程式名稱
                        where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format(" (programName like N'%{0}%')", GetData.GetRequest("programname"));
                    }
                    if (!string.IsNullOrEmpty(GetData.GetRequest("usy")))
                    {
                        where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format("Usy='{0}'", GetData.GetRequest("usy"));
                    }
                    if (!string.IsNullOrEmpty(GetData.GetRequest("menuid")))
                    {
                        where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format("menuid='{0}'", GetData.GetRequest("menuid"));
                    }
                    break;
            }

            DataTable progDT = ps.GetAllProg(row, page, out pageCount, out total, where);
            StringBuilder sb = new StringBuilder();
            sb = JsonHelper.DataTableToJSON(progDT, total, true);
            context.Response.Write(sb);
        }
        /// <summary>
        /// 新增程式
        /// </summary>
        /// <param name="context"></param>
        private void AddProgram(HttpContext context)
        {
            try
            {
                string result = string.Empty;
                string programID = GetData.GetRequest("programid").Trim();
                string programName = GetData.GetRequest("programname").Trim();
                string functionID = GetData.GetRequest("ids").Trim();
                string usy = GetData.GetRequest("usy").Trim();
                string functionNames = GetData.GetRequest("names").Trim();
                string menuid = GetData.GetRequest("menu_id").Trim();
                string menuUrl = GetData.GetRequest("menuurl").Trim();
                string orderid = GetData.GetRequest("orderid").Trim();
                string ismobile = GetData.GetRequest("ismobile").Trim();
                string mobileurl = GetData.GetRequest("mobileurl").Trim();
                List<string> arr = functionID.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

                //验证程式ID是否存在
                if (!string.IsNullOrEmpty(programID))
                {
                    bool orr = ps.IsExitProg(programID);
                    if (orr == true)
                    {
                        result = "EB0007";
                    }
                }

                if (result == "EB0007")
                {
                    result = languageManage.GetResourceText(result, LanguageId);
                    context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
                    context.Response.End();
                }
                //验证是否该程式有基本执行功能
                if (ps.IsExitProFunc(programID) == true)
                {
                    //先删除原来的程式的基本功能
                    ps.DelProgFunc(programID);
                }
                //执行增加操作
                result = ps.SetProgFunc(arr, programID);
                if (result == "SB0013")
                {
                    //设置程式与菜单的关联
                    if (ps.IsExitProgVsMenu(programID) == true)
                    {
                        //存在关联的话就执行更新操作
                        result = ps.ModProgMenu(programID, menuid);
                    }
                    else
                    {
                        t_ProgRefMenu pm = new t_ProgRefMenu();
                        pm.programID = programID;
                        pm.menuID = menuid;
                        pm.usy = usy;
                        result = ps.AddProgMenu(pm);
                    }

                    t_Program prg = new t_Program();
                    prg.programID = programID;
                    prg.programName = programName;
                    prg.menuUrl = menuUrl;
                    prg.functionStr = functionNames;
                    prg.createrID = uservice.GetUserMain().userID.To_String();
                    prg.cDeptID = uservice.GetUserMain().userDept.To_String();
                    prg.menuId = menuid;
                    prg.orderid = string.IsNullOrEmpty(orderid) ? 1 : Convert.ToInt32(orderid);
                    prg.usy = usy;
                    prg.IsMobile = string.IsNullOrEmpty(ismobile) ? "N" : ismobile;
                    prg.MobileUrl = mobileurl;

                    if (prg != null)
                    {
                        result = ps.AddProgram(prg);
                        if (result == "SB0011")
                        {
                            LogService logs = new LogService();
                            //写进系统日志
                            t_SysLog syslog = new t_SysLog();
                            syslog.operatorID = uservice.GetUserMain().userID.To_String();
                            syslog.operatorName = uservice.GetUserMain().userName.To_String();
                            syslog.refProgram = "ProgramSet.aspx";
                            syslog.refClass = "ProgramService";
                            syslog.refMethod = "AddProgram";
                            syslog.refRemark = "Add new program and programID=" + prg.programID + "";
                            syslog.refTime = System.DateTime.Now;
                            syslog.refIP = GetData.GetUserIP().ToString();
                            syslog.refSql = "insert into t_Program";
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
                syslog.operatorID = uservice.GetUserMain().userID.To_String();
                syslog.operatorName = uservice.GetUserMain().userName.To_String();
                syslog.refProgram = "ProgramSet.aspx";
                syslog.refClass = "ProgramService";
                syslog.refMethod = "AddProgram";
                syslog.refRemark = e.ToString();
                syslog.refTime = System.DateTime.Now;
                syslog.refIP = GetData.GetUserIP().ToString();
                syslog.refSql = "";
                syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                logs.WriteLog(syslog);


                string result = "newLabel124";
                result = languageManage.GetResourceText(result, LanguageId);
                context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
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