using System;
using System.Web;
using System.Data;
using System.Text;
using System.Linq;
using Utility.HelpClass;
using System.Collections.Generic;
using System.Web.SessionState;
using System.Net.Mail;
using DMC.Model;
using DMC.BLL;
using System.Configuration;

namespace Web.ASHX
{
    /// <summary>
    /// 用戶權限管理
    /// code by klint
    /// </summary>
    public class UserRightManage : IHttpHandler, IRequiresSessionState
    {
        //變量定義以及相關類的實例化
        private string UserId = "a10857";
        private string DeptId = "IT600";
        private string LanguageId = "0002";
        private string CompanyId = "avccn";
        private UserInfo currentUser = null;
        private t_SysLog syslog = new t_SysLog();
        private LogService lg = new LogService();

        //相關業務類實例化
        private UserRightManageService userManage = new UserRightManageService();
        private UserManageService umanage = new UserManageService();
        private ProgramManageService programManage = new ProgramManageService();
        private CompanyManageService companyManage = new CompanyManageService();
        private SupplyManageService supplyManage = new SupplyManageService();
        private DataManageService dataManage = new DataManageService();
        private LanguageManageService languageManage = new LanguageManageService();
        private UserManageService userive = new UserManageService();
        LogService logs = new LogService();

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                //從session中獲取登錄用戶的相關信息
                currentUser = umanage.GetUserMain();
                if (currentUser != null)
                {
                    UserId = currentUser.userID.ToString();//獲取用戶名
                    DeptId = currentUser.userDept.ToString();//獲取用戶部門
                    LanguageId = currentUser.LanguageId.ToString();//獲取登錄語言別
                    CompanyId = currentUser.Company.ToString();
                }
            }
            catch (Exception ex)
            {
                //写进系统日志
                t_SysLog syslog = new t_SysLog();
                syslog.operatorID = userive.GetUserMain().userID.To_String();
                syslog.operatorName = userive.GetUserMain().userName.To_String();
                syslog.refProgram = "/ASHX/Permission/UserRightManage.ashx";
                syslog.refClass = "UserRightManage.ashx";
                syslog.refMethod = "ProcessRequest";
                syslog.refRemark = ex.Message;
                syslog.refTime = System.DateTime.Now;
                syslog.refIP = GetData.GetUserIP().ToString();
                syslog.refSql = "";
                syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                logs.WriteLog(syslog);
            }

            context.Response.ContentType = "text/plain";
            string Method = context.Request.Params["M"];
            switch (Method.ToLower())
            {
                case "search"://用戶搜索
                    Search(context);
                    break;
                case "add"://新增用戶
                    AddUser(context);
                    break;
                case "mod"://修改用戶
                    ModUser(context);
                    break;
                case "delete"://刪除用戶
                    DelUser(context);
                    break;
                case "reset"://重置用戶密碼
                    RestUserPwd(context);
                    break;
                case "getright":
                    GetRight(context);//權限獲取
                    break;
                case "delprogram"://刪除程式
                    DelProgram(context);
                    break;
                case "saveprogram"://保存程式列表
                    SaveProgramList(context);
                    break;
                case "jointesc"://自動獲取對應員工、供應商和客戶的信息
                    JointESC(context);
                    break;
                case "prightsetting"://保存個人程式的公司和資料權限
                    PRightSetting(context);
                    break;
            }
        }

        /// <summary>
        /// 保存個人程式的公司和資料權限
        /// </summary>
        /// <param name="context"></param>
        public void PRightSetting(HttpContext context)
        {
            string UserId = context.Request.Params["UB"];
            string ProgramId = context.Request.Params["ProgramId"];
            string Companys = context.Request.Params["CompanyList"];
            string Datas = context.Request.Params["DataList"];
            string message = dataManage.SaveDataByUserAndProgram(UserId, ProgramId, Datas);
            if (message == "success")
            {
                List<string> CompanyList = Companys.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                userManage.SaveUserProgramCompanyRight(UserId, ProgramId, CompanyList);
            }
            context.Response.Write("{\"success\":true,\"msg\":\"" + languageManage.GetResourceText(message, LanguageId) + "\"}");
        }
        /// <summary>
        /// 自動獲取對應員工、供應商和客戶的信息
        /// </summary>
        /// <param name="context"></param>
        public void JointESC(HttpContext context)
        {
            string Keyword = context.Request.Params["KeyID"];
            string UserType = context.Request.Params["UserType"];
            DataTable dt = userManage.JointESC(Keyword, UserType);
            context.Response.Write(JsonHelper.DataTableToJSON(dt));
        }
        /// <summary>
        /// 保存程式列表
        /// </summary>
        /// <param name="context"></param>
        public void SaveProgramList(HttpContext context)
        {
            string userId = context.Request.Params["UB"];
            string list = context.Request.Params["PAList"];
            string Programs = context.Request.Params["Programs"];
            string pdata = context.Request.Params["PData"];
            string message = string.Empty;
            string[] paList = list.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            if (!string.IsNullOrEmpty(userId))
            {
                message = programManage.AddProgram(true, userId, Programs);
                foreach (string item in paList)
                {
                    string[] painfo = item.Split(new string[] { "|" }, StringSplitOptions.None);
                    List<string> ActionIdList = painfo[1].Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    message = programManage.SaveProgramAction(true, userId, painfo[0], ActionIdList);
                }

                string[] pdList = pdata.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                // programId:deptId|isall
                foreach (string pdInfo in pdList)
                {
                    string[] PD = pdInfo.Split(new string[] { ":" }, StringSplitOptions.None);
                    dataManage.SaveDataByUserAndProgram(userId, PD[0], PD[1].Replace(";", ","));
                }
            }
            context.Response.Write("{\"success\":true,\"msg\":\"" + languageManage.GetResourceText(message, LanguageId) + "\"}");
            //写进系统日志
            t_SysLog syslog = new t_SysLog();
            syslog.operatorID = currentUser.userID;
            syslog.operatorName = currentUser.userName;
            syslog.refProgram = "UserBaseInfo.aspx";
            syslog.refClass = "ProgramManageService";
            syslog.refMethod = "SaveProgramAction";
            syslog.refRemark = "使用者：" + userId + " 保存程式权限:" + message;
            syslog.refTime = System.DateTime.Now;
            syslog.refIP = GetData.GetUserIP().ToString();
            syslog.refSql = "";
            syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
            lg.WriteLog(syslog);
        }

        private class datatemp
        {
            public string ProgramId { get; set; }
            public string ActionIds { get; set; }
        }

        /// <summary>
        /// 刪除程式
        /// </summary>
        /// <param name="context"></param>
        private void DelProgram(HttpContext context)
        {
            string userId = context.Request.Params["UB"];
            string programId = context.Request.Params["ProgramId"];
            string result = string.Empty;
            result = programManage.DelProgram(true, userId, new List<string>() { programId });
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
            syslog.refProgram = "UserBaseInfo.aspx";
            syslog.refClass = "ProgramManageService";
            syslog.refMethod = "DelProgram";
            syslog.refRemark = "使用者：" + userId + " 删除程式权限:" + result;
            syslog.refTime = System.DateTime.Now;
            syslog.refIP = GetData.GetUserIP().ToString();
            syslog.refSql = "";
            syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
            lg.WriteLog(syslog);
        }
        /// <summary>
        /// 權限獲取
        /// </summary>
        /// <param name="context"></param>
        private void GetRight(HttpContext context)
        {
            string RightType = context.Request.Params["RightType"];
            switch (RightType.ToLower())
            {
                case "rosetype"://獲取用戶權限類別
                    GetUserRose(context);
                    break;
                case "program"://程式權限
                    SearchProgram(context);
                    break;
                case "company"://公司權限
                    SearchCompany(context);
                    break;
                case "supply"://供應商權限
                    SearchSupply(context);
                    break;
                case "data":
                    SearchData(context);//資料權限搜索
                    break;
                case "programcompany":
                    SearchProgramCompany(context);//程式公司權限
                    break;
                case "programaction"://獲取用戶程式權限搜索
                    SearchProgramAction(context);
                    break;
            }
        }

        /// <summary>
        /// 程式權限搜索
        /// </summary>
        /// <param name="context"></param>
        public void SearchProgramAction(HttpContext context)
        {
            string userId = context.Request.Params["UB"];
            string programId = context.Request.Params["ProgramId"];
            System.Data.DataTable dt = new System.Data.DataTable();
            dt = programManage.ReadProgramAction(true, string.IsNullOrEmpty(userId) ? "000" : userId, programId, LanguageId);

            context.Response.Write(JsonHelper.ConvertDTToJson(dt, 0).ToString());
        }
        /// <summary>
        /// 程式公司權限
        /// </summary>
        /// <param name="context"></param>
        public void SearchProgramCompany(HttpContext context)
        {
            string userId = context.Request.Params["UB"];
            string ProgramId = context.Request.Params["ProgramId"];
            System.Data.DataTable dt = new System.Data.DataTable();
            dt = companyManage.ReadProgramCompanyRight(string.IsNullOrEmpty(userId) ? "000" : userId, ProgramId);
            context.Response.Write(JsonHelper.ConvertDTToJson(dt, 0).ToString());
        }

        /// <summary>
        /// 資料權限搜索
        /// </summary>
        /// <param name="context"></param>
        public void SearchData(HttpContext context)
        {
            string userId = context.Request.Params["UB"];
            string programId = context.Request.Params["ProgramId"];
            DataTable dt = new DataTable();
            dt = dataManage.ReadDataByUserAndProgram(string.IsNullOrEmpty(userId) ? "000" : userId, programId);
            context.Response.Write(JsonHelper.ConvertDTToJson(dt, 0).ToString());
        }

        /// <summary>
        /// 供應商權限
        /// </summary>
        /// <param name="context"></param>
        public void SearchSupply(HttpContext context)
        {
            string userId = context.Request.Params["UB"];
            System.Data.DataTable dt = new System.Data.DataTable();
            dt = supplyManage.ReadSupplyByUser(string.IsNullOrEmpty(userId) ? "000" : userId);
            context.Response.Write(JsonHelper.ConvertDTToJson(dt, 0).ToString());
        }

        /// <summary>
        /// 公司權限搜索
        /// </summary>
        /// <param name="context"></param>
        public void SearchCompany(HttpContext context)
        {
            string userId = context.Request.Params["UB"];
            System.Data.DataTable dt = new System.Data.DataTable();
            dt = companyManage.ReadCompanyRight(string.IsNullOrEmpty(userId) ? "000" : userId, LanguageId);
            context.Response.Write(JsonHelper.ConvertDTToJson(dt, 0).ToString());
        }

        /// <summary>
        /// 程式權限搜索
        /// </summary>
        /// <param name="context"></param>
        public void SearchProgram(HttpContext context)
        {
            string userId = context.Request.Params["UB"];
            string keyword = context.Request.Params["KeyWord"];
            DataTable dt = new DataTable();
            dt = programManage.SearchProgram(true, string.IsNullOrEmpty(userId) ? "000" : userId, keyword, LanguageId);
            context.Response.Write(JsonHelper.ConvertDTToJson(dt, 0).ToString());
        }
        /// <summary>
        /// 獲取用戶權限類別
        /// </summary>
        /// <param name="context"></param>
        private void GetUserRose(HttpContext context)
        {
            string UserId = "000";
            if (context.Request["UB"] != null)
                UserId = context.Request["UB"].ToString();
            string UserType = "01";
            if (context.Request["UserType"] != null)
                UserType = context.Request["UserType"].ToString();
            string Keyword = "";
            if (context.Request["Keyword"] != null)
            {
                Keyword = context.Request["Keyword"].ToString();
            }
            DataTable dt = new DataTable();
            dt = userManage.GetUserRose(UserId, UserType, Keyword);
            context.Response.Write(JsonHelper.ConvertDTToJson(dt, 0).ToString());

        }

        /// <summary>
        /// 重置用戶密碼
        /// </summary>
        /// <param name="context"></param>
        private void RestUserPwd(HttpContext context)
        {
            string UserId = context.Request.Params["UB"];
            string pwd = string.Empty;
            string result = userManage.ResetPwd(UserId, out pwd);

            if (result == "success")
            {
                //獲取用戶郵箱，重置密碼時發送郵件
                UserInfo userInfo = umanage.GetUserInfoByID(UserId);
                if (!string.IsNullOrEmpty(userInfo.userMail) && userInfo.userMail.IndexOf("@") > -1)
                {
                    try
                    {
                        SysParaSettingService spss = new SysParaSettingService();
                        var SysId = ConfigurationManager.AppSettings["SysId"].ToString();
                        var AuthKey = ConfigurationManager.AppSettings["AuthKey"].ToString();
                        var Mcid = ConfigurationManager.AppSettings["Mcid"].ToString();
                        var From = ConfigurationManager.AppSettings["MailAddress"].ToString();
                        //var subject = "QMS系統帳號密碼重置";
                        //string title = "";
                        //if (ConfigurationManager.AppSettings["IsTestSystem"].Trim().ToLower() == "true")
                        //{
                        //    title = "[測試系統]";
                        //}
                        //bool blnRT = javaEmailApi.EmailApi.Send(new javaEmailApi.SendMailModel()
                        //{
                        //    mcid = Mcid,
                        //    authStr = AuthKey,
                        //    sysid = SysId,
                        //    from = From,
                        //    sendto = userInfo.userMail,//,"lei_zeng@avc.co"
                        //    content = "帳號：" + UserId + " 的新密碼為：" + pwd,
                        //    subject = title + subject,
                        //    IsHtml = true,
                        //    Priority = System.Net.Mail.MailPriority.High
                        //});
                        //SendEmail(userInfo.userMail, "WPMS系統帳號密碼重置", "帳號：" + UserId + " 的新密碼為：" + pwd);
                    }
                    catch
                    { }
                    context.Response.Write("{\"success\":true,\"pwd\":\"" + pwd + "\",\"msg\":\"" + languageManage.GetResourceText("Passwordmailbox", LanguageId) + "" + userInfo.userMail + "中\"}");
                }
                else
                {
                    context.Response.Write("{\"success\":true,\"pwd\":\"" + pwd + "\",\"msg\":\"" + languageManage.GetResourceText("Passwordcontactuser", LanguageId) + "！\"}");
                }
            }
            else
            {
                context.Response.Write("{\"success\":false,\"msg\":\"" + languageManage.GetResourceText(result, LanguageId) + "\"}");
            }
            //写进系统日志
            t_SysLog syslog = new t_SysLog();
            syslog.operatorID = currentUser.userID;
            syslog.operatorName = currentUser.userName;
            syslog.refProgram = "UserBaseInfo.aspx";
            syslog.refClass = "UserRightManageService";
            syslog.refMethod = "ResetPwd";
            syslog.refRemark = "使用者：" + UserId + " 密码重置:" + result;
            syslog.refTime = System.DateTime.Now;
            syslog.refIP = GetData.GetUserIP().ToString();
            syslog.refSql = "";
            syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
            lg.WriteLog(syslog);
        }

        /// <summary>
        /// 刪除用戶
        /// </summary>
        /// <param name="context"></param>
        private void DelUser(HttpContext context)
        {
            string userList = context.Request["UserIdList"].ToString().Trim();
            if (!string.IsNullOrEmpty(userList))
            {
                List<string> UserIdList = userList.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                string result = userManage.DeleteUserInfo(UserIdList);
                if (result == "success")
                {
                    context.Response.Write("{\"success\":true}");
                    //写进系统日志
                    t_SysLog syslog = new t_SysLog();
                    syslog.operatorID = currentUser.userID;
                    syslog.operatorName = currentUser.userName;
                    syslog.refProgram = "UserBaseInfo.aspx";
                    syslog.refClass = "UserRightManageService";
                    syslog.refMethod = "DeleteUserInfo";
                    syslog.refRemark = "删除使用者：" + userList + " 状态:" + result;
                    syslog.refTime = System.DateTime.Now;
                    syslog.refIP = GetData.GetUserIP().ToString();
                    syslog.refSql = "";
                    syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                    lg.WriteLog(syslog);
                }
                else
                {
                    context.Response.Write("{\"success\":false,\"msg\":\"" + languageManage.GetResourceText(result, LanguageId) + "\"}");
                }

            }
            else
            {
                context.Response.Write("{\"success\":false,\"msg\":\"" + languageManage.GetResourceText("copynotsocure", LanguageId) + "\"}");
            }
        }
        /// <summary>
        /// 修改用戶
        /// </summary>
        /// <param name="context"></param>
        private void ModUser(HttpContext context)
        {
            try
            {
                t_User model = new t_User();
                model.userType = context.Request["userType"].ToString();
                model.userID = context.Request["userID"].ToString();
                model.userNo = context.Request["userNo"].ToString();
                model.userMail = context.Request["userMail"].ToString();
                model.userName = context.Request["userName"].ToString();
                model.userDept = context.Request["userDept"].ToString();
                model.domainID = context.Request["domainID"].ToString();
                model.domainAddr = context.Request["domainAddr"].ToString();
                
                model.usy = context.Request["usy"].ToString();
                model.defLanguage = LanguageId;
                model.updaterID = UserId;// userManage.GetUserMain().userID;
                model.uDeptID = DeptId;// userManage.GetUserMain().uDeptID;
                model.lastModTime = System.DateTime.Now;
                model.defaultRole = context.Request["defaultRole"].ToString();
                string RoseList = context.Request["RoseList"].ToString();
                string CompanyList = context.Request["CompanyList"].ToString();
                string SupplyList = context.Request["SupplyList"].ToString();
                //拆分字符串，組織成新的數組
                List<string> RoseIdList = RoseList.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                List<string> CompanyIdList = CompanyList.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                List<string> SupplyIdList = SupplyList.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

                string result = userManage.ModUserInfo(model, RoseIdList, CompanyIdList, SupplyIdList);
                if (result == "success")
                {
                    context.Response.Write("{\"success\":true}");
                    //写进系统日志
                    t_SysLog syslog = new t_SysLog();
                    syslog.operatorID = currentUser.userID;
                    syslog.operatorName = currentUser.userName;
                    syslog.refProgram = "UserBaseInfo.aspx";
                    syslog.refClass = "UserRightManageService";
                    syslog.refMethod = "ModUserInfo";
                    syslog.refRemark = "修改使用者：" + model.userID + " 状态:" + result;
                    syslog.refTime = System.DateTime.Now;
                    syslog.refIP = GetData.GetUserIP().ToString();
                    syslog.refSql = "";
                    syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                    lg.WriteLog(syslog);
                }
                else
                    context.Response.Write("{\"success\":false,\"msg\":\"" + languageManage.GetResourceText(result, LanguageId) + "\"}");


            }
            catch (Exception ex)
            {   //写进系统日志
                t_SysLog syslog = new t_SysLog();
                syslog.operatorID = userive.GetUserMain().userID.To_String();
                syslog.operatorName = userive.GetUserMain().userName.To_String();
                syslog.refProgram = "/ASHX/Permission/UserRightManage.ashx";
                syslog.refClass = "UserRightManage.ashx";
                syslog.refMethod = "ModUser";
                syslog.refRemark = ex.Message;
                syslog.refTime = System.DateTime.Now;
                syslog.refIP = GetData.GetUserIP().ToString();
                syslog.refSql = "";
                syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                logs.WriteLog(syslog);

                context.Response.Write("{\"success\":false,\"msg\":\"" + ex.Message + "\"}");
            }
        }
        /// <summary>
        /// 新增用戶
        /// </summary>
        /// <param name="context"></param>
        private void AddUser(HttpContext context)
        {
            try
            {
                //實例化用戶對象
                t_User model = new t_User();
                //組織對象實體
                model.userType = context.Request["userType"].ToString();
                model.userID = context.Request["userID"].ToString();
                model.userNo = context.Request["userNo"].ToString();
                model.userMail = context.Request["userMail"].ToString();
                model.userName = context.Request["userName"].ToString();
                model.userDept = context.Request["userDept"].ToString();
                model.domainID = context.Request["domainID"].ToString();
                model.domainAddr = context.Request["domainAddr"].ToString();
                model.usy = context.Request["usy"].ToString();
                model.defLanguage = LanguageId;
                model.createrID = UserId;
                model.cDeptID = DeptId;
                model.createTime = System.DateTime.Now;
                string RoseList = context.Request["RoseList"].ToString();
                string CompanyList = context.Request["CompanyList"].ToString();
                string SupplyList = context.Request["SupplyList"].ToString();

                List<string> RoseIdList = RoseList.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                List<string> CompanyIdList = CompanyList.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                List<string> SupplyIdList = SupplyList.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

                string result = userManage.AddUserInfo(model, RoseIdList, CompanyIdList, SupplyIdList);
                if (result == "success")
                {
                    context.Response.Write("{\"success\":true}");
                    //写进系统日志
                    t_SysLog syslog = new t_SysLog();
                    syslog.operatorID = currentUser.userID;
                    syslog.operatorName = currentUser.userName;
                    syslog.refProgram = "UserBaseInfo.aspx";
                    syslog.refClass = "UserRightManageService";
                    syslog.refMethod = "AddUserInfo";
                    syslog.refRemark = "添加使用者：" + model.userID + " 状态:" + result;
                    syslog.refTime = System.DateTime.Now;
                    syslog.refIP = GetData.GetUserIP().ToString();
                    syslog.refSql = "";
                    syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                    lg.WriteLog(syslog);
                }
                else
                    context.Response.Write("{\"success\":false,\"msg\":\"" + languageManage.GetResourceText(result, LanguageId) + "\"}");


            }
            catch (Exception ex)
            {
                //写进系统日志
                t_SysLog syslog = new t_SysLog();
                syslog.operatorID = userive.GetUserMain().userID.To_String();
                syslog.operatorName = userive.GetUserMain().userName.To_String();
                syslog.refProgram = "/ASHX/Permission/UserRightManage.ashx";
                syslog.refClass = "UserRightManage.ashx";
                syslog.refMethod = "AddUser";
                syslog.refRemark = ex.Message;
                syslog.refTime = System.DateTime.Now;
                syslog.refIP = GetData.GetUserIP().ToString();
                syslog.refSql = "";
                syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                logs.WriteLog(syslog);

                context.Response.Write("{\"success\":false,\"msg\":\"" + ex.Message + "\"}");
            }
        }

        /// <summary>
        /// 用戶搜索
        /// </summary>
        /// <param name="context"></param>
        private void Search(HttpContext context)
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
                    if (!string.IsNullOrEmpty(key) && key != languageManage.GetResourceText("InputDefaultKey", LanguageId))
                        where = string.Format(" UserId like N'%{0}%' OR UserName like N'%{0}%' OR userMail like N'%{0}%' OR userNo like N'%{0}%' or domainID like N'%{0}%' ", key);
                    break;
                case "ByAdvanced":
                    if (!string.IsNullOrEmpty(context.Request.Params["UD"].ToString().Trim()))
                    {
                        where = string.Format("UserId like N'%{0}%' ", context.Request.Params["UD"]);
                    }
                    if (!string.IsNullOrEmpty(context.Request.Params["UserNo"].ToString().Trim()))
                    {
                        where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format("UserNo like N'%{0}%'", context.Request.Params["UserNo"]);
                    }
                    if (!string.IsNullOrEmpty(context.Request.Params["UserType"].ToString().Trim()))
                    {
                        where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format("UserType='{0}'", context.Request.Params["UserType"]);
                    }
                    if (!string.IsNullOrEmpty(context.Request.Params["UserMail"].ToString().Trim()))
                    {
                        where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format("UserMail like N'%{0}%'", context.Request.Params["UserMail"]);
                    }
                    if (!string.IsNullOrEmpty(context.Request.Params["UserName"].ToString().Trim()))
                    {
                        where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format("UserName like N'%{0}%'", context.Request.Params["UserName"]);
                    }
                    if (!string.IsNullOrEmpty(context.Request.Params["Usy"].ToString().Trim()))
                    {
                        where += (string.IsNullOrEmpty(where) ? "" : " AND ") + string.Format("Usy='{0}'", context.Request.Params["Usy"]);
                    }
                    break;
            }

            DataTable dt = userManage.Search(row, page, out pageCount, out total, where);
            StringBuilder dd = JsonHelper.ConvertDTToJson(dt, total);
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