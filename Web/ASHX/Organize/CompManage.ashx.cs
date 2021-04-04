using System;
using System.Web;
using Utility.HelpClass;
using System.Text;
using System.Data;
using System.Collections.Generic;
using DMC.Model;
using DMC.BLL;
using System.Linq;

namespace Web.ASHX
{
    /// <summary>
    /// 系統公司別處理相關操作
    /// code by jeven
    /// 2013-6
    /// </summary>
    public class CompManage : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        //變量定義以及相關類的實例化
        private string UserId = "a10857";
        private string DeptId = "IT600";
        private string LanguageId = "0002";
        private UserInfo currentUser = null;
        private UserManageService uservice = new UserManageService();
        public LanguageManageService languageManage = new LanguageManageService();
        LogService logs = new LogService();
        private CompService cserive = new CompService();
        private UserManageService userive = new UserManageService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //從session中獲取登錄用戶的相關信息
            string method = GetData.GetRequest("M").ToLower();
            currentUser = uservice.GetUserMain();
            UserId = currentUser.userID.To_String();//獲取用戶名
            DeptId = currentUser.userDept.To_String();//獲取用戶部門
            LanguageId = currentUser.LanguageId.ToString();//獲取登錄語言別
            switch (method)
            {
                case "search"://按條件搜索系統公司別列表
                    Search(context);
                    break;
                case "delete"://刪除公司別
                    DelComp(context);
                    break;//新增公司別資料
                case "add":
                    AddComp(context);
                    break;
                case "update"://更新公司別資料信息
                    ModComp(context);
                    break;
            }
        }
        /// <summary>
        /// 按條件搜索系統公司別列表
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
            //1.组织搜索条件
            #region  组织搜索条件
            switch (type)
            {
                case "ByKey":
                    string key = GetData.GetRequest("KeyWord");
                    string keyStr = languageManage.GetResourceText("InputDefaultKey", LanguageId);
                    if (!string.IsNullOrEmpty(key) && key != keyStr)
                        where = string.Format("and (companyID like N'%{0}%' or interName like N'%{0}%' or outerName like N'%{0}%' or simpleName like N'%{0}%' or addrOne like N'%{0}%' or addrTwo like N'%{0}%' or compTel=N'{0}') ", key);
                    break;
                case "ByAdvanced":
                    if (!string.IsNullOrEmpty(GetData.GetRequest("companyid")))
                    {

                        where = string.Format("and  companyID like N'%{0}%' ", GetData.GetRequest("companyid"));
                    }
                    if (!string.IsNullOrEmpty(GetData.GetRequest("compcategory")))
                    {
                        where += string.Format("and compCategory =N'{0}' ", GetData.GetRequest("compcategory"));
                    }
                    if (!string.IsNullOrEmpty(GetData.GetRequest("simplename")))
                    {
                        where += string.Format("and (simpleName like N'%{0}%') ", GetData.GetRequest("simplename"));
                    }
                    if (!string.IsNullOrEmpty(GetData.GetRequest("addrone")))
                    {

                        where += string.Format("and (addrOne like N'%{0}%' or addrTwo like N'%{0}%') ", GetData.GetRequest("addrone"));
                    }
                    if (!string.IsNullOrEmpty(GetData.GetRequest("comptel")))
                    {

                        where += string.Format("and (compTel like N'%{0}%') ", GetData.GetRequest("comptel"));
                    }

                    if (!string.IsNullOrEmpty(GetData.GetRequest("usy")))
                    {

                        where += string.Format("and (usy=N'{0}') ", GetData.GetRequest("usy"));
                    }
                    break;
            }
            if (!string.IsNullOrEmpty(where))
            {
                where = where.Substring(3, where.Length - 3);
            }
            #endregion
            //2.執行搜索
            DataTable dt = cserive.GetAllComp(row, page, out pageCount, out total, where);
            StringBuilder sb = new StringBuilder();
            sb = JsonHelper.DataTableToJSON(dt, total, true);
            context.Response.Write(sb);

        }
        /// <summary>
        /// 刪除公司別
        /// </summary>
        /// <param name="context"></param>
        protected void DelComp(HttpContext context)
        {
            try
            {
                //接收傳參
                string idstr = GetData.GetRequest("empIDstr").To_String();
                //拆分字符串，組織成數組對象
                List<string> arr = idstr.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (idstr != "")
                {
                    string result = string.Empty;
                    //執行刪除操作，并返回刪除結果
                    result = cserive.DelComp(arr);
                    if (result == "SB0008")
                    {
                        //写进系统日志
                        t_SysLog syslog = new t_SysLog();
                        syslog.operatorID = userive.GetUserMain().userID.To_String();
                        syslog.operatorName = userive.GetUserMain().userName.To_String();
                        syslog.refProgram = "Company.aspx";
                        syslog.refClass = "CompService";
                        syslog.refMethod = "DelComp";
                        syslog.refRemark = "delete Company and companyID=" + arr + "";
                        syslog.refTime = System.DateTime.Now;
                        syslog.refIP = GetData.GetUserIP().ToString();
                        syslog.refSql = "delete from t_Company  where companyID=" + arr + "";
                        syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                        logs.WriteLog(syslog);
                        context.Response.Write("{\"success\":true}");
                    }
                    else
                    {
                        context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
                    }
                }
            }
            catch (Exception e)
            {
                //写进系统日志
                t_SysLog syslog = new t_SysLog();
                syslog.operatorID = userive.GetUserMain().userID.To_String();
                syslog.operatorName = userive.GetUserMain().userName.To_String();
                syslog.refProgram = "/Organize/Company.aspx";
                syslog.refClass = "CompManage.ashx";
                syslog.refMethod = "DelComp";
                syslog.refRemark = e.ToString();
                syslog.refTime = System.DateTime.Now;
                syslog.refIP = GetData.GetUserIP().ToString();
                syslog.refSql = "";
                syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                logs.WriteLog(syslog);

                string result = "newLabel096";
                result = languageManage.GetResourceText(result, LanguageId);
                context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
            }
        }
        /// <summary>
        /// 新增公司別資料
        /// </summary>
        /// <param name="context"></param>
        protected void AddComp(HttpContext context)
        {
            try
            {
                string result = string.Empty;
                //接收傳參
                string companyid = GetData.GetRequest("companyid").Trim();
                string complanguage = GetData.GetRequest("complanguage").To_String();
                string compcategory = GetData.GetRequest("compcategory").Trim();
                string intername = GetData.GetRequest("intername").Trim();
                string outername = GetData.GetRequest("outername").Trim();
                string simplename = GetData.GetRequest("simplename").Trim();
                string companyno = GetData.GetRequest("companyno").Trim();
                string addrone = GetData.GetRequest("addrone").Trim();
                string addrtwo = GetData.GetRequest("addrtwo").Trim();
                string comptel = GetData.GetRequest("comptel").Trim();
                string compfax = GetData.GetRequest("compfax").Trim();
                string compregno = GetData.GetRequest("compregno").Trim();
                string remark = GetData.GetRequest("remark").Trim();
                string usy = GetData.GetRequest("usy").Trim();
                if (companyid != "" && simplename != "")
                {
                    //接收傳參, 組織實體對象數據
                    t_Company comp = new t_Company();
                    CompService cs = new CompService();
                    comp.companyID = companyid;
                    comp.compLanguage = complanguage;
                    comp.compCategory = compcategory;
                    comp.interName = intername;
                    comp.outerName = outername;
                    comp.simpleName = simplename;
                    comp.companyNo = companyno;
                    comp.addrOne = addrone;
                    comp.addrTwo = addrtwo;
                    comp.compTel = comptel;
                    comp.compFax = compfax;
                    comp.compRegNo = compregno;
                    comp.remark = remark;
                    comp.createTime = System.DateTime.Now;
                    comp.usy = usy;
                    //從存儲信息中取得登錄用戶的某些信息
                    comp.createrID = userive.GetUserMain().userID.To_String();
                    comp.cDeptID = userive.GetUserMain().userDept.To_String();
                    //調用系統業務方法，返回執行結果
                    result = cs.AddComp(comp);

                    if (result == "SB0003")
                    {
                        //写进系统日志
                        t_SysLog syslog = new t_SysLog();
                        syslog.operatorID = comp.createrID;
                        syslog.operatorName = userive.GetUserMain().userName.To_String();
                        syslog.refProgram = "Company.aspx";
                        syslog.refClass = "CompService";
                        syslog.refMethod = "AddComp";
                        syslog.refRemark = "add new Company and companyID=" + comp.companyID + "";
                        syslog.refTime = System.DateTime.Now;
                        syslog.refIP = GetData.GetUserIP().ToString();
                        syslog.refSql = "insert into t_Company(companyID,compLanguage,compCategory,interName,outerName," +
                         "simpleName,companyNo,addrOne,addrTwo,compTel,compFax,compRegNo," +
                         "remark,usy,createrID,cDeptID,createTime)" +
                         "values(@companyID,@compLanguage ,@compCategory ,@interName ,@outerName ," +
                         "@simpleName ,@companyNo ,@addrOne ,@addrTwo ,@compTel ,@compFax ," +
                         "@compRegNo ,@remark ,@usy ,@createrID ,@cDeptID,@createTime)";
                        syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                        logs.WriteLog(syslog);
                        ////返回正確的執行結果
                        context.Response.Write("{\"success\":true}");
                    }
                    else
                    {
                        //返回錯誤的執行結果
                        result = languageManage.GetResourceText(result, LanguageId);
                        context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
                    }
                }
                else
                {
                    //返回錯誤的執行結果
                    context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
                }
            }
            catch (Exception e)
            {
                //写进系统日志
                t_SysLog syslog = new t_SysLog();
                syslog.operatorID = userive.GetUserMain().userID.To_String();
                syslog.operatorName = userive.GetUserMain().userName.To_String();
                syslog.refProgram = "/Organize/Company.aspx";
                syslog.refClass = "CompManage.ashx";
                syslog.refMethod = "AddComp";
                syslog.refRemark = e.ToString();
                syslog.refTime = System.DateTime.Now;
                syslog.refIP = GetData.GetUserIP().ToString();
                syslog.refSql = "";
                syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                logs.WriteLog(syslog);

                string result = "newLabel097";
                result = languageManage.GetResourceText(result, LanguageId);
                context.Response.Write("{\"success\":false,\"msg\":\"" + result + "\"}");
            }


        }
        /// <summary>
        /// 更新公司別資料信息
        /// </summary>
        /// <param name="context"></param>
        protected void ModComp(HttpContext context)
        {
            try
            {
                //1.接收傳參, 組織實體對象數據
                string companyid = GetData.GetRequest("companyid").Trim();
                string complanguage = GetData.GetRequest("complanguage").To_String();
                string compcategory = GetData.GetRequest("compcategory").Trim();
                string intername = GetData.GetRequest("intername").Trim();
                string outername = GetData.GetRequest("outername").Trim();
                string simplename = GetData.GetRequest("simplename").Trim();
                string companyno = GetData.GetRequest("companyno").Trim();
                string addrone = GetData.GetRequest("addrone").Trim();
                string addrtwo = GetData.GetRequest("addrtwo").Trim();
                string comptel = GetData.GetRequest("comptel").Trim();
                string compfax = GetData.GetRequest("compfax").Trim();
                string compregno = GetData.GetRequest("compregno").Trim();
                string remark = GetData.GetRequest("remark").Trim();
                string usy = GetData.GetRequest("usy").Trim();

                //2.實例化對象實體
                t_Company comp = new t_Company();
                comp.companyID = companyid;
                comp.compLanguage = complanguage;
                comp.compCategory = compcategory;
                comp.interName = intername;
                comp.outerName = outername;
                comp.simpleName = simplename;
                comp.companyNo = companyno;
                comp.addrOne = addrone;
                comp.addrTwo = addrtwo;
                comp.compTel = comptel;
                comp.compFax = compfax;
                comp.compRegNo = compregno;
                comp.remark = remark;
                comp.usy = usy;
                //從存儲信息中取得登錄用戶的某些信息
                comp.updaterID = userive.GetUserMain().userID.To_String();
                comp.uDeptID = userive.GetUserMain().userDept.To_String();
                comp.lastModTime = System.DateTime.Now;
                // //調用系統業務方法，返回執行結果
                string result = cserive.ModComp(comp);
                if (result == "SB0005")
                {
                    //3.写进系统日志
                    t_SysLog syslog = new t_SysLog();
                    syslog.operatorID = comp.updaterID;
                    syslog.operatorName = userive.GetUserMain().userName.To_String();
                    syslog.refProgram = "Company.aspx";
                    syslog.refClass = "CompService";
                    syslog.refMethod = "ModComp";
                    syslog.refRemark = "update Company and companyID=" + comp.companyID + "";
                    syslog.refTime = System.DateTime.Now;
                    syslog.refIP = GetData.GetUserIP().ToString();
                    syslog.refSql = "update t_Company set compLanguage=@compLanguage,compCategory=@compCategory ," +
                        "interName=@interName,outerName=@outerName ,simpleName=@simpleName,companyNo=@companyNo,addrOne=@addrOne," +
                        "addrTwo=@addrTwo,compTel=@compTel ,compFax=@compFax,compRegNo=@compRegNo,remark=@remark," +
                        "usy=@usy,updaterID=@updaterID ,uDeptID=@uDeptID ,lastModTime=@modTime where companyID=@companyID";
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
                syslog.refProgram = "/Organize/Company.aspx";
                syslog.refClass = "CompManage.ashx";
                syslog.refMethod = "ModComp";
                syslog.refRemark = e.ToString();
                syslog.refTime = System.DateTime.Now;
                syslog.refIP = GetData.GetUserIP().ToString();
                syslog.refSql = "";
                syslog.refEvent = "用戶" + syslog.operatorName + " 在" + syslog.refTime + "&nbsp" + syslog.refProgram + "中" + syslog.refRemark + "";
                logs.WriteLog(syslog);

                string result = "newLabel098";
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