using DMC.BLL;
using DMC.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using Utility.HelpClass;

namespace Web.ASHX.DMC
{
    /// <summary>
    /// RepairForm 的摘要描述
    /// </summary>
    public class RepairForm : IHttpHandler, IRequiresSessionState
    {
        //變量定義以及相關類的實例化
        string UserId = "a10857";
        string DeptId = "IT600";
        string CompanyId = "avccn";
        UserInfo currentUser = null;
        LanguageManageService languageManage = new LanguageManageService();
        UserManageService umserive = new UserManageService();
        LogService logs = new LogService();

        RepairFormService rfs = new RepairFormService();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string method = GetData.GetRequest("M").ToLower();
            currentUser = umserive.GetUserMain();
            if (currentUser != null)
            {
                try
                {
                    UserId = currentUser.userID.ToString();//獲取用戶名
                    DeptId = currentUser.userDept.ToString();//獲取用戶部門
                    CompanyId = currentUser.Company.companyID.ToString();//獲取登錄公司別 
                }
                catch
                { }
                switch (method)
                {
                    case "search":
                        Search(context);
                        break;
                    case "newrepairform":
                        NewRepairForm(context);
                        break;
                    //case "updaterepairman":
                    //    UpdateRepairman(context);
                    //    break;
                    //case "deleterepairman":
                    //    DeleteRepairman(context);
                    //    break;
                    case "confirm":
                        Confirm(context);
                        break;
                    case "repairassign":
                        RepairAssign(context);
                        break;
                    case "hisrecord":
                        HisRecord(context);
                        break;
                    case "getassignqty":
                        GetAssignQty(context);
                        break;

                    case "deleterepair":
                        deleteRepair(context);
                        break;
                        
                }
            }
            else
            {
                context.Response.Write("{\"success\": false ,\"msg\":\"登录失效\"}");
            }
        }

        public void HisRecord(HttpContext context)
        {
            int pagesize = 10;
            int pageindex = 1;
            int total = 0;
            int pageCount = 0;
            StringBuilder strWhere = new StringBuilder();
            if (!string.IsNullOrEmpty(context.Request.Params["DeviceId"]))
            {
                strWhere.AppendFormat(" DeviceId = N'{0}'", context.Request.Params["DeviceId"]);
            }

            if (!string.IsNullOrEmpty(context.Request.Params["PositionId"]))
            {
                strWhere.AppendFormat(" AND PositionId = N'{0}'", context.Request.Params["PositionId"]);
            }

            if (!string.IsNullOrEmpty(context.Request.Params["PhenomenaId"]))
            {
                strWhere.AppendFormat(" AND PhenomenaId = N'{0}'", context.Request.Params["PhenomenaId"]);
            }
            RepairRecordService rrs = new RepairRecordService();
            //取得相關查詢條件下的數據列表
            DataTable dt = rrs.Search(pagesize, pageindex, out pageCount, out total, strWhere.ToString());
            StringHelper.JsonGZipResponse(context, JsonHelper.DataTableToJSON(dt));
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="context"></param>
        public void Search(HttpContext context)
        {
            int pagesize = int.Parse(context.Request.Params["rows"]);
            int pageindex = int.Parse(context.Request.Params["page"]);
            int total = 0;
            int pageCount = 0;
            StringBuilder strWhere = new StringBuilder();
            strWhere.Append(" 1=1 ");
            string type = context.Request.Params["SearchType"];
            switch (type)
            {
                case "ByKey":
                    string key = context.Request.Params["KeyWord"];
                    if (!string.IsNullOrEmpty(key))
                    {
                        strWhere.AppendFormat(" AND (RepairFormNO like N'%{0}%' or ApplyUserId like N'%{0}%' or DeviceId like N'%{0}%' or PositionId like N'%{0}%' or PhenomenaId like N'%{0}%' or FaultCode like N'%{0}%' or FaultReason like N'%{0}%' ) ", key);
                    }

                    if (!string.IsNullOrEmpty(context.Request.Params["FormStatus"]))
                    {
                        strWhere.AppendFormat(" AND FormStatus = N'{0}'", context.Request.Params["FormStatus"]);
                    }
                    if (!string.IsNullOrEmpty(context.Request.Params["NoFormStatus"]))
                    {
                        strWhere.AppendFormat(" AND NOT FormStatus like N'{0}%'", context.Request.Params["NoFormStatus"]);
                        //if (context.Request.Params["NoFormStatus"] == "6")
                        //{
                        //    strWhere.AppendFormat(" AND ApplyUserId = N'{0}'", UserId);
                        //}
                    }
                    //判断类别：返修 || 挂单
                    if (!string.IsNullOrEmpty(context.Request.Params["DType"]))
                    {
                        switch (context.Request.Params["DType"])
                        {
                            case "0":
                                //正常申请
                                strWhere.Append(" AND FormStatus = '10'");
                                break;
                            case "1":
                                //返修
                                strWhere.Append(" AND FormStatus IN('23','24','25') ");
                                break;
                            case "2":
                                //挂单
                                //挂单AND charindex('_R',RepairFormNO) = 0  
                                strWhere.Append(" AND FormStatus = '12'");
                                break;
                        }
                    }
                    break;
                case "ByAdvanced":
                    if (!string.IsNullOrEmpty(context.Request.Params["RepairFormNO"]))
                    {
                        strWhere.AppendFormat(" AND (RepairFormNO like N'%{0}%')", context.Request.Params["RepairFormNO"]);
                    }

                    if (!string.IsNullOrEmpty(context.Request.Params["ApplyUserId"]))
                    {
                        strWhere.AppendFormat(" AND (ApplyUserId like N'%{0}%')", context.Request.Params["ApplyUserId"]);
                    }

                    if (!string.IsNullOrEmpty(context.Request.Params["DeviceId"]))
                    {
                        strWhere.AppendFormat(" AND DeviceId = N'{0}'", context.Request.Params["DeviceId"]);
                    }

                    if (!string.IsNullOrEmpty(context.Request.Params["PositionId"]))
                    {
                        strWhere.AppendFormat(" AND PositionId = N'{0}'", context.Request.Params["PositionId"]);
                    }

                    if (!string.IsNullOrEmpty(context.Request.Params["PhenomenaId"]))
                    {
                        strWhere.AppendFormat(" AND PhenomenaId = N'{0}'", context.Request.Params["PhenomenaId"]);
                    }

                    if (!string.IsNullOrEmpty(context.Request.Params["FaultCode"]))
                    {
                        strWhere.AppendFormat(" AND FaultCode = N'{0}'", context.Request.Params["FaultCode"]);
                    }

                    if (!string.IsNullOrEmpty(context.Request.Params["FaultReason"]))
                    {
                        strWhere.AppendFormat(" AND FaultReason = like N'%{0}%'", context.Request.Params["FaultReason"]);
                    }

                    if (!string.IsNullOrEmpty(context.Request.Params["FormStatus"]))
                    {
                        strWhere.AppendFormat(" AND FormStatus like N'{0}%'", context.Request.Params["FormStatus"]);
                    }
                    break;
            }
            //取得相關查詢條件下的數據列表
            DataTable dt = rfs.Search(pagesize, pageindex, out pageCount, out total, strWhere.ToString());
            StringBuilder sb = new StringBuilder();
            //將數據返回給客戶端
            sb = JsonHelper.DataTableToJSON(dt, total, true);
            StringHelper.JsonGZipResponse(context, sb);
        }

        /// <summary>
        /// 归属信息设置
        /// </summary>
        /// <param name="context"></param>
        public void NewRepairForm(HttpContext context)
        {
            try
            {
                RepairFormEntity entity = new RepairFormEntity();
                entity.ApplyUserId = context.Request["ApplyUserId"];
                entity.DeviceId = context.Request["DeviceId"];
                entity.FaultTime = context.Request["FaultTime"];
                entity.PositionId = context.Request["PositionId"];
                entity.PositionText = context.Request["PositionText"];
                entity.PositionId1 = context.Request["PositionId1"];
                entity.PhenomenaId = context.Request["PhenomenaId"];
                entity.PhenomenaText = context.Request["PhenomenaText"];
                entity.PhenomenaId1 = context.Request["PhenomenaId1"];
                entity.PhenomenaText1 = context.Request["PhenomenaText1"];
                entity.PositionText1 = context.Request["PositionText1"];
                entity.FaultCode = context.Request["FaultCode"];
                entity.FaultReason = context.Request["FaultReason"];
                entity.FaultStatus = context.Request["FaultStatus"];
                entity.MouldId = context.Request["MouldId"];
                entity.NewMouldId = context.Request["NewMouldId"];
                entity.MouldId1 = context.Request["MouldId1"];
                entity.NewMouldId1 = context.Request["NewMouldId1"];
                entity.FormStatus = "10";
                //entity.RepairFormNO = "0";//报修单号，自动生成
                string msg = rfs.NewRepairForm(entity);
                if (string.IsNullOrWhiteSpace(msg))
                {
                    context.Response.Write("{\"success\":true,\"msg\":\"新增成功\"}");
                }
                else
                {
                    context.Response.Write("{\"success\":false,\"msg\":\"" + msg + "\"}");
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"success\":false,\"msg\":\"" + ex.Message + "\"}");
            }
        }

        public void deleteRepair(HttpContext context)
        {
            try
            {
                RepairFormEntity entity = new RepairFormEntity();
                var repairformno = context.Request["repairformno"];
                string msg = rfs.deleteRepair(repairformno);
                if (string.IsNullOrWhiteSpace(msg))
                {
                    context.Response.Write("{\"success\":true,\"msg\":\"撤销成功\"}");
                }
                else
                {
                    context.Response.Write("{\"success\":false,\"msg\":\"" + msg + "\"}");
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"success\":false,\"msg\":\"" + ex.Message + "\"}");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Confirm(HttpContext context)
        {
            try
            {
                RepairFormEntity entity = new RepairFormEntity();
                entity.RepairFormNO = context.Request["RepairFormNO"];
                entity.ConfirmUser = UserId;
                string msg = rfs.Confirm(entity);
                if (string.IsNullOrWhiteSpace(msg))
                {
                    context.Response.Write("{\"success\":true,\"msg\":\"确认成功\"}");
                }
                else
                {
                    context.Response.Write("{\"success\":false,\"msg\":\"" + msg + "\"}");
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"success\":false,\"msg\":\"" + ex.Message + "\"}");
            }
        }
        /// <summary>
        /// 指派
        /// </summary>
        /// <param name="context"></param>
        public void RepairAssign(HttpContext context)
        {
            try
            {
                string RepairFormNO = context.Request["RepairFormNO"];
                string AssignUser = context.Request["AssignUser"];
                string oldFormStatus = context.Request["FormStatus"];
                //操作员
                string opuser = context.Request["opuser"];
                string msg = rfs.RepairAssign(RepairFormNO, AssignUser, opuser, oldFormStatus);
                if (string.IsNullOrWhiteSpace(msg))
                {
                    context.Response.Write("{\"success\":true,\"msg\":\"指派成功\"}");
                }
                else
                {
                    context.Response.Write("{\"success\":false,\"msg\":\"" + msg + "\"}");
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"success\":false,\"msg\":\"" + ex.Message + "\"}");
            }
        }
        /// <summary>
        /// 获取指派的数量
        /// </summary>
        /// <param name="context"></param>
        public void GetAssignQty(HttpContext context)
        {
            //获取待排单的数量
            DataTable dt = rfs.GetAssignQty();
            StringHelper.JsonGZipResponse(context, JsonHelper.DataTableToJSON(dt));
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