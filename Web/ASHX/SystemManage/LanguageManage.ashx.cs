using System;
using System.Web;
using System.Data;
using Utility.HelpClass;
using System.Collections.Generic;
using System.Web.SessionState;
using DMC.BLL;
using DMC.Model;

namespace Web.ASHX
{
    /// <summary>
    /// 多語言操作
    /// </summary>
    public class LanguageManage : IHttpHandler, IRequiresSessionState
    {
        //變量定義以及相關類的實例化
        private UserManageService uservice = new UserManageService();
        private string UserId = "a10857";
        private string DeptId = "IT600";
        private string LanguageId = "0002";
        private UserInfo currentUser = null;
        private t_SysLog syslog = new t_SysLog();
        private LogService lg = new LogService();
        private LanguageManageService languageManage = new LanguageManageService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //從session中獲取登錄用戶的相關信息
            currentUser = uservice.GetUserMain();
            UserId = currentUser.userID.To_String();//獲取用戶名
            DeptId = currentUser.userDept.To_String();//獲取用戶部門
            LanguageId = currentUser.LanguageId.To_String();//獲取登錄語言別
            //獲取操作方法名
            string Method = context.Request.Params["M"];
            //獲取請求方法關鍵字
            switch (Method.ToLower())
            {
                case "search"://搜索多語言列表
                    Search(context);
                    break;
                case "add"://新增多語言
                    AddInfo(context);
                    break;
                case "delete"://刪除多語言資料
                    DeleteInfo(context);
                    break;
                case "update"://更新多語言資料
                    Update(context);
                    break;
                case "savetype"://保存語言類別
                    SaveType(context);
                    break;
                case "loadresvalue"://加載多語言顯示的值
                    LoadResValue(context);
                    break;
                case "generatebit"://生成語言包
                    GenerateBit(context);
                    break;
            }
        }
        /// <summary>
        /// 生成語言包
        /// </summary>
        /// <param name="context"></param>
        public void GenerateBit(HttpContext context)
        {
            try
            {
                languageManage.GenerateLanguageBit();
                //写进系统日志
                t_SysLog syslog = new t_SysLog();
                syslog.operatorID = currentUser.userID;
                syslog.operatorName = currentUser.userName;
                syslog.refProgram = "LanguageSetting.aspx";
                syslog.refClass = "LanguageManageService";
                syslog.refMethod = "GenerateLanguageBit";
                syslog.refRemark = "用戶重新加載語言包";
                syslog.refTime = System.DateTime.Now;
                syslog.refIP = GetData.GetUserIP().ToString();
                syslog.refSql = "";
                syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                lg.WriteLog(syslog);
                context.Response.Write("{\"success\":true}");
            }
            catch
            {
                //genfailed 生成失敗
                context.Response.Write("{\"success\":false,\"msg\":\"" + languageManage.GetResourceText("genfailed", LanguageId) + "\"}");
            }
        }
        /// <summary>
        /// 加載多語言顯示的值
        /// </summary>
        /// <param name="context"></param>
        public void LoadResValue(HttpContext context)
        {
            string ResourceId = context.Request.Params["ResourceId"];
            DataTable dt = languageManage.GetResourceValues(ResourceId);
            context.Response.Write(JsonHelper.DataTableToJSON(dt));
        }
        /// <summary>
        /// 保存語言類別
        /// </summary>
        /// <param name="context"></param>
        public void SaveType(HttpContext context)
        {
            try
            {
                string Languages = context.Request.Params["Languages"];
                string resultmsg = string.Empty;
                //刪除所有語言類別
                if (string.IsNullOrEmpty(Languages))
                {
                    languageManage.DeleteAllLanguageType();
                }
                else
                {
                    //獲取所有語言類別
                    DataTable dt = languageManage.GetAllLanguageType();
                    List<String> exists = new List<string>();
                    foreach (DataRow dr in dt.Rows)
                        exists.Add(dr["LanguageId"].ToString());

                    string[] list = Languages.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in list)
                    {
                        string[] perptoyList = item.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        t_Language_Type model = new t_Language_Type();
                        foreach (string per in perptoyList)
                        {
                            string[] detail = per.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                            if (detail[0] == "LanguageId")
                            {
                                model.LanguageId = detail.Length <= 1 ? "" : detail[1];
                            }
                            else if (detail[0] == "LanguageName")
                            {
                                model.LanguageName = detail.Length <= 1 ? "" : detail[1];
                            }
                            else if (detail[0] == "Usy")
                            {
                                model.Usy = detail.Length <= 1 ? "" : (detail[1].ToLower() == "true" ? "Y" : "N");
                            }
                        }

                        if (exists.Contains(model.LanguageId))
                        {
                            //如果包含，則修改
                            model.UpdateUser = UserId;
                            model.UpdateDeptId = DeptId;
                            model.UpdateTime = System.DateTime.Now;
                            model.Description = "";
                            resultmsg = languageManage.UpdateLanguageType(model);
                            if (resultmsg != "success")
                            {
                                break;
                            }
                            exists.RemoveAll(o => o == model.LanguageId);
                        }
                        else
                        {
                            //否則新增
                            model.CreateUser = UserId;
                            model.CreateDeptId = DeptId;
                            model.CreateTime = System.DateTime.Now;
                            model.Description = "";
                            resultmsg = languageManage.AddLanguageType(model);
                            if (resultmsg != "success")
                            {
                                break;
                            }
                        }
                    }
                    languageManage.DeleteLanguageTypeByKey(exists);
                }
                Cache.Remove("LanguageType");
                if (resultmsg == "success" || string.IsNullOrEmpty(resultmsg))
                {
                    context.Response.Write("{\"success\":true}");
                }
                else
                {
                    context.Response.Write("{\"success\":false,\"msg\":\"" + languageManage.GetResourceText(resultmsg, LanguageId) + "\"}");
                }
            }
            catch
            {
                //PM006 保存失敗
                context.Response.Write("{\"success\":false,\"msg\":\"" + languageManage.GetResourceText("PM006", LanguageId) + "\"}");
            }
        }
        /// <summary>
        /// 新增多語言
        /// </summary>
        /// <param name="context"></param>
        public void AddInfo(HttpContext context)
        {
            t_Language_Resources model = new t_Language_Resources();
            model.ResourceId = context.Request["ResourceId"].ToString().Trim();
            model.CreateDeptId = UserId;
            model.CreateUser = DeptId;
            model.DefaultValue = context.Request["DefaultValue"].ToString().Trim();
            model.GroupKey = context.Request["GroupKey"].ToString().Trim();
            model.GroupValue = context.Request["GroupValue"].ToString().Trim();
            model.ResourceType = context.Request["ResourceType"].ToString().Trim();
            model.Usy = context.Request["Usy"].ToString().Trim();

            string LanguageValues = context.Request.Params["LanguageValues"];
            List<LangValue> list = new List<LangValue>();
            string[] values = LanguageValues.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in values)
            {
                string[] perptoyList = item.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                LangValue langValue = new LangValue();
                langValue.ResourceId = model.ResourceId;
                foreach (string per in perptoyList)
                {
                    string[] detail = per.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                    if (detail[0] == "LanguageId")
                    {
                        langValue.LanguageId = detail.Length <= 1 ? "" : detail[1];
                    }
                    else if (detail[0] == "LanguageValue")
                    {
                        langValue.DisplayValue = detail.Length <= 1 ? "" : detail[1];
                    }
                }
                list.Add(langValue);
            }

            string result = languageManage.CreateLanguage(model, list);
            if (result == "success")
                context.Response.Write("{\"success\":true}");
            else
                context.Response.Write("{\"success\":false,\"msg\":\"" + languageManage.GetResourceText(result, LanguageId) + "\"}");

            //写进系统日志
            t_SysLog syslog = new t_SysLog();
            syslog.operatorID = currentUser.userID;
            syslog.operatorName = currentUser.userName;
            syslog.refProgram = "LanguageSetting.aspx";
            syslog.refClass = "LanguageManageService";
            syslog.refMethod = "CreateLanguage";
            syslog.refRemark = "創建資源類型:" + model.ResourceId + ";創建結果：" + result;
            syslog.refTime = System.DateTime.Now;
            syslog.refIP = GetData.GetUserIP().ToString();
            syslog.refSql = "";
            syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
            lg.WriteLog(syslog);
        }
        /// <summary>
        /// 更新多語言資料
        /// </summary>
        /// <param name="context"></param>
        public void Update(HttpContext context)
        {
            t_Language_Resources model = new t_Language_Resources();
            model.ResourceId = context.Request["ResourceId"].ToString().Trim();
            model.UpdateDeptId = UserId;
            model.UpdateUser = DeptId;
            model.DefaultValue = context.Request["DefaultValue"].ToString().Trim();
            model.GroupKey = context.Request["GroupKey"].ToString().Trim();
            model.GroupValue = context.Request["GroupValue"].ToString().Trim();
            model.ResourceType = context.Request["ResourceType"].ToString().Trim();
            model.Usy = context.Request["Usy"].ToString().Trim();

            string LanguageValues = context.Request.Params["LanguageValues"];
            List<LangValue> list = new List<LangValue>();
            string[] values = LanguageValues.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in values)
            {
                string[] perptoyList = item.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                LangValue langValue = new LangValue();
                langValue.ResourceId = model.ResourceId;
                foreach (string per in perptoyList)
                {
                    string[] detail = per.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                    if (detail[0] == "LanguageId")
                    {
                        langValue.LanguageId = detail.Length <= 1 ? "" : detail[1];
                    }
                    else if (detail[0] == "LanguageValue")
                    {
                        langValue.DisplayValue = detail.Length <= 1 ? "" : detail[1];
                    }
                }
                list.Add(langValue);
            }

            string result = languageManage.ModifyLanguage(model, list);
            if (result == "success")
                context.Response.Write("{\"success\":true}");
            else
                context.Response.Write("{\"success\":false,\"msg\":\"" + languageManage.GetResourceText(result, LanguageId) + "\"}");

            //写进系统日志
            t_SysLog syslog = new t_SysLog();
            syslog.operatorID = currentUser.userID;
            syslog.operatorName = currentUser.userName;
            syslog.refProgram = "LanguageSetting.aspx";
            syslog.refClass = "LanguageManageService";
            syslog.refMethod = "ModifyLanguage";
            syslog.refRemark = "修改資源類型:" + model.ResourceId + ";修改結果：" + result;
            syslog.refTime = System.DateTime.Now;
            syslog.refIP = GetData.GetUserIP().ToString();
            syslog.refSql = "";
            syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
            lg.WriteLog(syslog);
        }
        /// <summary>
        /// 刪除多語言資料
        /// </summary>
        /// <param name="context"></param>
        public void DeleteInfo(HttpContext context)
        {
            string listStr = context.Request.Params["LanguageList"];

            if (!string.IsNullOrEmpty(listStr))
            {
                string[] temp = listStr.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
                list.AddRange(temp);
                string result = languageManage.DeleteLanguage(list);
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
                syslog.refProgram = "LanguageSetting.aspx";
                syslog.refClass = "LanguageManageService";
                syslog.refMethod = "DeleteLanguage";
                syslog.refRemark = "刪除資源類型:" + listStr + ";刪除結果：" + result;
                syslog.refTime = System.DateTime.Now;
                syslog.refIP = GetData.GetUserIP().ToString();
                syslog.refSql = "";
                syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                lg.WriteLog(syslog);
            }
            else
            {
                //DelTips 請選擇需要刪除的數據
                context.Response.Write("{\"success\":false,\"msg\":\"" + languageManage.GetResourceText("DelTips", LanguageId) + "\"}");
            }
        }
        /// <summary>
        /// 搜索多語言列表
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
                case "ByKey":
                    string key = context.Request.Params["KeyWord"];
                    if (!string.IsNullOrEmpty(key))
                        where = string.Format(" ResourceId like N'%{0}%' OR DefaultValue like N'%{0}%' OR GroupValue like N'%{0}%' OR GroupKey like N'%{0}%'", key);
                    break;
                case "ByAdvanced":
                    if (!string.IsNullOrEmpty(context.Request.Params["ResourceId"]))
                    {
                        where = string.Format("ResourceId like N'%{0}%'", context.Request.Params["ResourceId"]);
                    }
                    if (!string.IsNullOrEmpty(context.Request.Params["ResourceType"]))
                    {
                        where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format("ResourceType ='{0}'", context.Request.Params["ResourceType"]);
                    }
                    if (!string.IsNullOrEmpty(context.Request.Params["DefaultValue"]))
                    {
                        where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format("DefaultValue like N'%{0}%'", context.Request.Params["DefaultValue"]);
                    }
                    if (!string.IsNullOrEmpty(context.Request.Params["Usy"]))
                    {
                        where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format("Usy='{0}'", context.Request.Params["Usy"]);
                    }
                    break;
            }

            DataTable dt = languageManage.Search(row, page, out pageCount, out total, where);
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