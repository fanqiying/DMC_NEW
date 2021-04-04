using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using DMC.BLL;
using DMC.Model;
using Utility.HelpClass;

namespace Web.ASHX
{
    /// <summary>
    /// 權限設置操作
    /// code by klint 
    /// 2013-7-21
    /// </summary>
    public class RoseRightSetting : IHttpHandler, IRequiresSessionState
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
        private SupplyManageService supplyManage = new SupplyManageService();
        private DataManageService dataManage = new DataManageService();
        private ProgramManageService programManage = new ProgramManageService();
        private string RoseId = string.Empty;
        public void ProcessRequest(HttpContext context)
        {
            //從session中獲取登錄用戶的相關信息
            currentUser = userManage.GetUserMain();
            UserId = currentUser.userID.ToString();//獲取用戶名
            DeptId = currentUser.userDept.ToString();//獲取用戶部門
            LanguageId = currentUser.LanguageId.ToString();//獲取登錄語言別
            context.Response.ContentType = "text/plain";
            string Method = context.Request.Params["M"];
            RoseId = context.Request.Params["RoseId"];
            if (string.IsNullOrEmpty(RoseId))
                RoseId = "";
            //獲取操作方法名
            switch (Method.ToLower())
            {
                case "search": //按類別執行搜索
                    string type = context.Request.Params["T"];
                    switch (type.ToLower())
                    {
                        case "supply"://搜索供應商權限
                            SearchRoseSupply(context);
                            break;
                        case "data":
                            //搜索資料權限
                            SearchRoseData(context);
                            break;
                        case "program"://搜索程式權限
                            SearchRoseProgram(context);
                            break;
                        case "programaction"://程式權限搜索
                            SearchProgramAction(context);
                            break;
                    }
                    break;
                case "rightsetting":// 設定供應商、公司和資料權限
                    RightSetting(context);
                    break;
                case "delprogram"://刪除程式權限
                    DelProgram(context);
                    break;
                case "saveprogram"://保存程式、操作和資料權限
                    SaveProgramList(context);
                    break;
                case "gettree"://獲取程式生成樹形結構
                    ReadProgramTree(context);
                    break;
            }
        }

        /// <summary>
        /// 供應商權限
        /// </summary>
        /// <param name="context"></param>
        public void SearchRoseSupply(HttpContext context)
        {
            DataTable dt = new DataTable();
            dt = supplyManage.ReadSupplyByRose(RoseId);
            context.Response.Write(JsonHelper.ConvertDTToJson(dt, 0).ToString());
        }

        /// <summary>
        /// 資料權限搜索
        /// </summary>
        /// <param name="context"></param>
        public void SearchRoseData(HttpContext context)
        {
            DataTable dt = dataManage.ReadDataByRose(RoseId);
            context.Response.Write(JsonHelper.ConvertDTToJson(dt, 0).ToString());
        }

        /// <summary>
        /// 程式權限搜索
        /// </summary>
        /// <param name="context"></param>
        public void SearchRoseProgram(HttpContext context)
        {
            string keyword = context.Request.Params["KeyWord"];
            if (!string.IsNullOrEmpty(keyword))
                keyword = keyword.Replace(languageManage.GetResourceText("roseright001", LanguageId), "");
            DataTable dt = programManage.SearchProgram(false, RoseId, keyword, LanguageId);
            context.Response.Write(JsonHelper.ConvertDTToJson(dt, 0).ToString());
        }

        /// <summary>
        /// 程式權限搜索
        /// </summary>
        /// <param name="context"></param>
        public void SearchProgramAction(HttpContext context)
        {
            string programId = context.Request.Params["ProgramId"];
            DataTable dt = programManage.ReadProgramAction(false, RoseId, programId, LanguageId);
            context.Response.Write(JsonHelper.ConvertDTToJson(dt, 0).ToString());
        }

        /// <summary>
        /// 設定供應商、公司和資料權限
        /// </summary>
        /// <param name="context"></param>
        public void RightSetting(HttpContext context)
        {
            string DeptIdList = context.Request.Params["DataList"];
            string SupplyIdList = context.Request.Params["SupplyIdList"];
            string message = string.Empty;
            message = dataManage.SaveDataByRose(RoseId, DeptIdList);
            if (message == "success")
            {
                message = supplyManage.SaveSupplyByRose(RoseId, SupplyIdList);


                //写进系统日志
                t_SysLog syslog = new t_SysLog();
                syslog.operatorID = currentUser.userID;
                syslog.operatorName = currentUser.userName;
                syslog.refProgram = "ExecPerSetting.aspx";
                syslog.refClass = "RoseRightSetting";
                syslog.refMethod = "RightSetting";
                syslog.refRemark = "" + languageManage.GetResourceText("roseringht002", LanguageId) + "：" + RoseId + "" + languageManage.GetResourceText("roseright003", LanguageId) + ":" + message;
                syslog.refTime = System.DateTime.Now;
                syslog.refIP = GetData.GetUserIP().ToString();
                syslog.refSql = "";
                syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                lg.WriteLog(syslog);
                context.Response.Write("{\"success\":true,\"msg\":\"" + languageManage.GetResourceText(message, LanguageId) + "\"}");
            }
            else
            {
                context.Response.Write("{\"success\":false,\"msg\":\"" + languageManage.GetResourceText(message, LanguageId) + "\"}");
            }
        }

        /// <summary>
        /// 刪除程式權限
        /// </summary>
        /// <param name="context"></param>
        public void DelProgram(HttpContext context)
        {
            try
            {
                string roseId = context.Request.Params["RoseId"];
                string programId = context.Request.Params["ProgramId"];
                string result = string.Empty;
                result = programManage.DelProgram(false, roseId, new List<string>() { programId });
                if (result == "success")
                {
                    context.Response.Write("{\"success\":true}");
                }
                else
                {
                    context.Response.Write("{\"success\":false,\"msg\":\"" + languageManage.GetResourceText(result, LanguageId) + "\"}");
                }
                //写进系统日志
                t_SysLog syslog = new t_SysLog();
                syslog.operatorID = currentUser.userID;
                syslog.operatorName = currentUser.userName;
                syslog.refProgram = "ExecPerSetting.aspx";
                syslog.refClass = "ProgramManageServices";
                syslog.refMethod = "DelProgram";
                syslog.refRemark = "刪除權限類別[" + RoseId + "]的程式權限[" + programId + "]操作" + result;
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
                syslog.refProgram = "ExecPerSetting.aspx";
                syslog.refClass = "ProgramManageServices";
                syslog.refMethod = "DelProgram";
                syslog.refRemark = e.To_String();
                syslog.refTime = System.DateTime.Now;
                syslog.refIP = GetData.GetUserIP().ToString();
                syslog.refSql = "";
                syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                lg.WriteLog(syslog);


                string result = "newLabel127";
                result = languageManage.GetResourceText(result, LanguageId);
                context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
            }
        }

        /// <summary>
        /// 保存程式、操作和資料權限
        /// </summary>
        /// <param name="context"></param>
        public void SaveProgramList(HttpContext context)
        {
            try
            {
                string roseId = context.Request.Params["RoseId"];
                string Programs = context.Request.Params["Programs"];
                string list = context.Request.Params["PAList"];
                string message = string.Empty;
                if (!string.IsNullOrEmpty(roseId))
                {
                    string[] paList = list.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    message = programManage.AddProgram(false, roseId, Programs);
                    foreach (string item in paList)
                    {
                        string[] painfo = item.Split(new string[] { "|" }, StringSplitOptions.None);
                        List<string> ActionIdList = painfo[1].Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        message = programManage.SaveProgramAction(false, roseId, painfo[0], ActionIdList);
                    }
                }
                context.Response.Write("{\"success\":true,\"msg\":\"" + languageManage.GetResourceText(message, LanguageId) + "\"}");

                //写进系统日志
                t_SysLog syslog = new t_SysLog();
                syslog.operatorID = currentUser.userID;
                syslog.operatorName = currentUser.userName;
                syslog.refProgram = "ExecPerSetting.aspx";
                syslog.refClass = "ProgramManageServices";
                syslog.refMethod = "SaveProgramList";
                syslog.refRemark = "保存權限類別[" + RoseId + "]的程式權限[" + Programs + "]操作 success";
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
                syslog.refProgram = "ExecPerSetting.aspx";
                syslog.refClass = "ProgramManageServices";
                syslog.refMethod = "DelProgram";
                syslog.refRemark = e.To_String();
                syslog.refTime = System.DateTime.Now;
                syslog.refIP = GetData.GetUserIP().ToString();
                syslog.refSql = "";
                syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                lg.WriteLog(syslog);


                string result = "newLabel128";
                result = languageManage.GetResourceText(result, LanguageId);
                context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
            }
        }
        /// <summary>
        /// 獲取程式生成樹形結構
        /// </summary>
        /// <param name="context"></param>
        public void ReadProgramTree(HttpContext context)
        {
            string tree = string.Empty;
            if (!string.IsNullOrEmpty(RoseId))
            {
                DataTable dt = new DataTable();
                userManage.GetSettingMenuTree("2", RoseId, LanguageId, ref dt);
                tree = GetTreeNode("", dt, true);
            }
            context.Response.Write(string.IsNullOrEmpty(tree) ? "[]" : tree);
        }
        /// <summary>
        /// 獲取程式的樹節點
        /// </summary>
        /// <param name="parentId">程式編號</param>
        /// <param name="dt">程式集合</param>
        /// <param name="isCheckBox">是否勾選</param>
        /// <returns></returns>
        protected string GetTreeNode(string parentId, DataTable dt, bool isCheckBox = true)
        {
            StringBuilder content = new StringBuilder();
            DataRow[] drList = dt.Select("ISNULL(ParentId,'')='" + parentId + "'");
            foreach (DataRow dr in drList)
            {
                if (content.Length > 0)
                    content.Append(",");
                content.Append("{");
                content.AppendFormat("\"id\":\"{0}\"", dr["ID"].ToString());
                //只有為目錄時，才有展開和關閉操作
                if (dr["IsProgram"].ToString() == "N")
                    content.AppendFormat(",\"state\":\"{0}\"", "close");

                content.AppendFormat(",\"checked\":{0}", dr["IsUse"].ToString() == "Y" ? "true" : "false");
                content.AppendFormat(",\"text\":\"{0}\"", dr["DisplayText"].ToString());
                if (dr["IsProgram"].ToString() == "Y")
                {
                    content.AppendFormat(",\"attributes\":{0}", "{\"url\":\"" + dr["Url"].ToString() + "\"}");
                }
                string childrenStr = GetTreeNode(dr["ID"].ToString(), dt);
                if (!string.IsNullOrEmpty(childrenStr))
                {
                    content.AppendFormat(",\"children\":{0}", childrenStr);
                }
                content.Append("}");
            }
            if (content.Length > 0)
                return "[" + content.ToString() + "]";
            return string.Empty;
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