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
    /// RepairRecord 的摘要描述
    /// </summary>
    public class RepairRecord : IHttpHandler, IRequiresSessionState
    {
        //變量定義以及相關類的實例化
        string UserId = "a10857";
        string DeptId = "IT600";
        string CompanyId = "avccn";
        UserInfo currentUser = null;
        LanguageManageService languageManage = new LanguageManageService();
        UserManageService umserive = new UserManageService();
        LogService logs = new LogService();

        RepairRecordService rrs = new RepairRecordService();

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
                    case "searchhoure":
                        SearchHoure(context);
                        break;
                    case "kanban":
                        KanBan(context);
                        break;
                    //case "newrepairform":
                    //    NewRepairForm(context);
                    //    break;
                    case "leaderappraise":
                        LeaderAppraise(context);
                        break;
                    case "qcconfirm":
                        QCConfirm(context);
                        break;
                    case "qcrconfirm":
                        QCRConfirm(context);
                        break;
                    case "confirm":
                        Confirm(context);
                        break;
                    case "reject":
                        Reject(context);
                        break;
                    case "leaderreject":
                        LeaderReject(context);
                        break;
                    case "repairmanreject":
                        RepairManReject(context);
                        break;
                    case "productionconfirm":
                        ProductionConfirm(context);
                        break;
                }
            }
            else
            {
                context.Response.Write("{\"success\": false ,\"msg\":\"登录失效\"}");
            }
        }

        /// <summary>
        /// 工时报表
        /// </summary>
        /// <param name="context"></param>
        public void SearchHoure(HttpContext context)
        {
            int pagesize = int.Parse(context.Request.Params["rows"]);
            int pageindex = int.Parse(context.Request.Params["page"]);
            int total = 0;
            int pageCount = 0;
            StringBuilder strWhere = new StringBuilder();
            strWhere.Append(" 1=1 ");
            string type = context.Request.Params["SearchType"];
            string key = string.Empty;
            switch (type)
            {
                case "ByKey":
                    key = context.Request.Params["KeyWord"];
                    if (!string.IsNullOrEmpty(key))
                    {
                        strWhere.AppendFormat(" AND (a.RepairFormNO like N'%{0}%' or a.DeviceId like N'%{0}%' or a.PositionId like N'%{0}%' or a.PhenomenaId like N'%{0}%' or a.FaultCode like N'%{0}%' or a.FaultReason like N'%{0}%' ) ", key);
                    }
                    break;
                case "ByAdvanced":
                    key = context.Request.Params["KeyWord"];
                    if (!string.IsNullOrEmpty(key))
                    {
                        strWhere.AppendFormat(" AND (a.RepairFormNO like N'%{0}%' or a.DeviceId like N'%{0}%' or a.PositionId like N'%{0}%' or a.PhenomenaId like N'%{0}%' or a.FaultCode like N'%{0}%' or a.FaultReason like N'%{0}%' ) ", key);
                    }
                    if (!string.IsNullOrEmpty(context.Request.Params["RepairFormNO"]))
                    {
                        strWhere.AppendFormat(" AND (a.RepairFormNO like N'%{0}%')", context.Request.Params["RepairFormNO"]);
                    }

                    if (!string.IsNullOrEmpty(context.Request.Params["ApplyUserId"]))
                    {
                        strWhere.AppendFormat(" AND (a.ApplyUserId like N'%{0}%')", context.Request.Params["ApplyUserId"]);
                    }

                    if (!string.IsNullOrEmpty(context.Request.Params["DeviceId"]))
                    {
                        strWhere.AppendFormat(" AND a.DeviceId = N'{0}'", context.Request.Params["DeviceId"]);
                    }

                    if (!string.IsNullOrEmpty(context.Request.Params["PositionId"]))
                    {
                        strWhere.AppendFormat(" AND a.PositionId = N'{0}'", context.Request.Params["PositionId"]);
                    }

                    if (!string.IsNullOrEmpty(context.Request.Params["PhenomenaId"]))
                    {
                        strWhere.AppendFormat(" AND a.PhenomenaId = N'{0}'", context.Request.Params["PhenomenaId"]);
                    }

                    if (!string.IsNullOrEmpty(context.Request.Params["FaultCode"]))
                    {
                        strWhere.AppendFormat(" AND a.FaultCode = N'{0}'", context.Request.Params["FaultCode"]);
                    }

                    if (!string.IsNullOrEmpty(context.Request.Params["FaultReason"]))
                    {
                        strWhere.AppendFormat(" AND a.FaultReason = like N'%{0}%'", context.Request.Params["FaultReason"]);
                    }

                    if (!string.IsNullOrEmpty(context.Request.Params["RepairmanId"]))
                    {
                        strWhere.AppendFormat(" AND a.RepairmanId = N'{0}'", context.Request.Params["RepairmanId"]);
                    }

                    if (!string.IsNullOrEmpty(context.Request.Params["RepairmanName"]))
                    {
                        strWhere.AppendFormat(" AND a.RepairmanName = N'{0}'", context.Request.Params["RepairmanName"]);
                    }

                    if (!string.IsNullOrEmpty(context.Request.Params["YearMonth"]))
                    {
                        strWhere.AppendFormat(" AND CONVERT(varchar(7),a.RepairETime,120)   = N'{0}'", context.Request.Params["YearMonth"]);
                    }

                    break;
            }
            //strWhere.AppendFormat(" AND a.RepairStatus in(20,23,4,5)");
            //取得相關查詢條件下的數據列表
            DataTable dt = rrs.SearchHoure(pagesize, pageindex, out pageCount, out total, strWhere.ToString());
            StringBuilder sb = new StringBuilder();
            //將數據返回給客戶端
            sb = JsonHelper.DataTableToJSON(dt, total, true);
            StringHelper.JsonGZipResponse(context, sb);
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
            string key = string.Empty;
            switch (type)
            {
                case "ByKey":
                    key = context.Request.Params["KeyWord"];
                    if (!string.IsNullOrEmpty(key))
                    {
                        strWhere.AppendFormat(" AND (RepairFormNO like N'%{0}%' or DeviceId like N'%{0}%' or PositionId like N'%{0}%' or PhenomenaId like N'%{0}%' or FaultCode like N'%{0}%' or FaultReason like N'%{0}%' ) ", key);
                    }
                    break;
                case "ByAdvanced":
                    key = context.Request.Params["KeyWord"];
                    if (!string.IsNullOrEmpty(key))
                    {
                        strWhere.AppendFormat(" AND (RepairFormNO like N'%{0}%' or DeviceId like N'%{0}%' or PositionId like N'%{0}%' or PhenomenaId like N'%{0}%' or FaultCode like N'%{0}%' or FaultReason like N'%{0}%' ) ", key);
                    }
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
                    if (!string.IsNullOrEmpty(context.Request.Params["YearMonth"]))
                    {
                        strWhere.AppendFormat(" AND CONVERT(varchar(7),RepairETime,120)   = N'{0}'", context.Request.Params["YearMonth"]);
                    }
                    break;
            }
            //UserId维修员
            if (!string.IsNullOrEmpty(context.Request.Params["RepairmanId"]))
            {
                //strWhere.AppendFormat(" AND RepairmanId = '{0}'", context.Request.Params["RepairmanId"]);
            }


            if (!string.IsNullOrEmpty(context.Request.Params["RepairStatus"]))
            {
                switch (context.Request.Params["RepairStatus"])
                {
                    case "100":
                        break;
                    case "1":
                        strWhere.Append(" AND RepairStatus in(20,23,24,25) ");
                        break; 
                    case "3":
                        //生产员确认
                        //UserManageService umserive = new UserManageService();
                        //currentUser = umserive.GetUserMain();
                        strWhere.AppendFormat(" AND RepairStatus = '{0}'", "30");
                        strWhere.AppendFormat(" AND ApplyUserId = '{0}'", UserId);
                        break;
                    case "4":
                        //QC确认
                        //UserManageService umserive = new UserManageService();
                        //currentUser = umserive.GetUserMain();
                        strWhere.AppendFormat(" AND RepairStatus = '{0}'", "40");
                        break;
                    case "5":
                        //组长确认and exists(select 1 from t_Repairman a,t_Repairman b
             //           where a.WorkDate = b.WorkDate

             //and a.groupname = b.groupname

             // and a.RepairmanId = t_RepairRecord.RepairmanId

             //AND a.ClassType = b.ClassType

             //and b.IsLeader = 'Y'

             //and b.RepairmanId = '50012369')
                        strWhere.AppendFormat(" AND RepairStatus = '{0}'", "50");
                        strWhere.AppendFormat(" AND EXISTS(SELECT 1 FROM t_Employee WHERE empid =t_RepairRecord.ApplyUserId and signerID = '{0}')", UserId);
                        break;
                    default:
                        strWhere.AppendFormat(" AND RepairStatus = '{0}'", context.Request.Params["RepairStatus"]);
                        break;
                }

            }
            //取得相關查詢條件下的數據列表
            DataTable dt = rrs.Search(pagesize, pageindex, out pageCount, out total, strWhere.ToString());
            StringBuilder sb = new StringBuilder();
            //將數據返回給客戶端
            sb = JsonHelper.DataTableToJSON(dt, total, true);
            StringHelper.JsonGZipResponse(context, sb);
        }

        ///// <summary>
        ///// 归属信息设置
        ///// </summary>
        ///// <param name="context"></param>
        //public void NewRepairForm(HttpContext context)
        //{
        //    try
        //    {
        //        RepairFormEntity entity = new RepairFormEntity();
        //        entity.ApplyUserId = context.Request["ApplyUserId"];
        //        entity.DeviceId = context.Request["DeviceId"];
        //        entity.FaultTime = context.Request["FaultTime"];
        //        entity.PositionId = context.Request["PositionId"];
        //        entity.PositionText = context.Request["PositionText"];
        //        entity.PhenomenaId = context.Request["PhenomenaId"];
        //        entity.PhenomenaText = context.Request["PhenomenaText"];

        //        entity.FaultCode = context.Request["FaultCode"];
        //        entity.FaultReason = context.Request["FaultReason"];
        //        entity.FaultStatus = context.Request["FaultStatus"];
        //        entity.FormStatus = "0";
        //        //entity.RepairFormNO = "0";//报修单号，自动生成
        //        string msg = rfs.NewRepairForm(entity);
        //        if (string.IsNullOrWhiteSpace(msg))
        //        {
        //            context.Response.Write("{\"success\":true,\"msg\":\"新增成功\"}");
        //        }
        //        else
        //        {
        //            context.Response.Write("{\"success\":false,\"msg\":\"" + msg + "\"}");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        context.Response.Write("{\"success\":false,\"msg\":\"" + ex.Message + "\"}");
        //    }
        //}
        /// <summary>
        /// 组长返修
        /// </summary>
        /// <param name="context"></param>
        public void LeaderReject(HttpContext context)
        {
            try
            {
                string RepairFormNO = context.Request["RepairFormNO"];
                string RebackReason = context.Request["RebackReason"];
                int AutoId = Convert.ToInt32(context.Request["AutoId"]);
                string msg = rrs.LeaderReject(AutoId, RepairFormNO, RebackReason, "50", "65", "25");
                if (string.IsNullOrWhiteSpace(msg))
                {
                    context.Response.Write("{\"success\":true,\"msg\":\"QC返修成功\"}");
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
        /// 生产员返修
        /// </summary>
        /// <param name="context"></param>
        public void RepairManReject(HttpContext context)
        {
            try
            {
                string RepairFormNO = context.Request["RepairFormNO"];
                string RebackReason = context.Request["RebackReason"];
                int AutoId = Convert.ToInt32(context.Request["AutoId"]);
                string msg = rrs.LeaderReject(AutoId, RepairFormNO, RebackReason, "30", "63", "23");
                if (string.IsNullOrWhiteSpace(msg))
                {
                    context.Response.Write("{\"success\":true,\"msg\":\"QC返修成功\"}");
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
        /// 组长确认
        /// </summary>
        /// <param name="context"></param>
        public void LeaderAppraise(HttpContext context)
        {
            try
            {
                string RepairFormNO = context.Request["RepairFormNO"];
                //string Appraise = context.Request["Appraise"];
                int AutoId = Convert.ToInt32(context.Request["AutoId"]);
                string msg = rrs.LeaderAppraise(AutoId, RepairFormNO, UserId);
                if (string.IsNullOrWhiteSpace(msg))
                {
                    context.Response.Write("{\"success\":true,\"msg\":\"QC确认成功\"}");
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
        /// 生产员确认
        /// </summary>
        /// <param name="context"></param>
        public void Confirm(HttpContext context)
        {
            try
            {
                RepairRecordEntity entity = new RepairRecordEntity();
                entity.AutoId = Convert.ToInt32(context.Request["AutoId"]);
                entity.RepairFormNO = context.Request["RepairFormNO"];
                entity.FaultReason = context.Request["FaultReason"];
                entity.FaultStatus = context.Request["FaultStatus"];
                entity.FaultCode = context.Request["FaultCode"];
                entity.PositionText = context.Request["PositionText"];
                entity.PositionId = context.Request["PositionId"];
                entity.PhenomenaId = context.Request["PhenomenaId"];
                entity.PhenomenaText = context.Request["PhenomenaText"];
                entity.FaultAnalysis = context.Request["FaultAnalysis"];
                entity.RepairStatus = context.Request["RepairStatus"];

                string OldRepairStatus = context.Request["OldRepairStatus"];

                string msg = rrs.Confirm(entity, OldRepairStatus);
                if (string.IsNullOrWhiteSpace(msg))
                {
                    context.Response.Write("{\"success\":true,\"msg\":\"值班状态修改成功\"}");
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
        /// 维修员挂单
        /// </summary>
        /// <param name="context"></param>
        public void Reject(HttpContext context)
        {
            try
            {
                string RepairFormNO = context.Request["RepairFormNO"];
                int AutoId = Convert.ToInt32(context.Request["AutoId"]);
                string RebackType = context.Request["RebackType"];
                string RebackReason = context.Request["RebackReason"];
                string OldRepairStatus = context.Request["OldRepairStatus"];
                string msg = rrs.Reject(RepairFormNO, AutoId, RebackType, RebackReason, OldRepairStatus);
                if (string.IsNullOrWhiteSpace(msg))
                {
                    context.Response.Write("{\"success\":true,\"msg\":\"挂单成功\"}");
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
        /// QC确认
        /// </summary>
        /// <param name="context"></param>
        public void QCConfirm(HttpContext context)
        {
            try
            {
                string RepairFormNO = context.Request["RepairFormNO"];
                int AutoId = Convert.ToInt32(context.Request["AutoId"]);
                string IPQCNumber = context.Request["IPQCNumber"];
                string msg = rrs.QCConfirm(AutoId, RepairFormNO, IPQCNumber);
                if (string.IsNullOrWhiteSpace(msg))
                {
                    context.Response.Write("{\"success\":true,\"msg\":\"QC确认成功\"}");
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
        /// 生产组长确认
        /// </summary>
        /// <param name="context"></param>
        public void ProductionConfirm(HttpContext context)
        {
            try
            {
                string RepairFormNO = context.Request["RepairFormNO"];
                int AutoId = Convert.ToInt32(context.Request["AutoId"]);

                string msg = rrs.ProductionConfirm(AutoId, RepairFormNO);
                if (string.IsNullOrWhiteSpace(msg))
                {
                    context.Response.Write("{\"success\":true,\"msg\":\"QC确认成功\"}");
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
        /// QC返修
        /// </summary>
        /// <param name="context"></param>
        public void QCRConfirm(HttpContext context)
        {
            try
            {
                string RepairFormNO = context.Request["RepairFormNO"];
                int AutoId = Convert.ToInt32(context.Request["AutoId"]);
                string RebackReason = context.Request["RebackReason"];
                string IPQCNumber = context.Request["IPQCNumber"];
                string msg = rrs.QCRConfirm(AutoId, RepairFormNO, RebackReason, IPQCNumber);
                if (string.IsNullOrWhiteSpace(msg))
                {
                    context.Response.Write("{\"success\":true,\"msg\":\"QC返修成功\"}");
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

        public void KanBan(HttpContext context)
        {
            // string strRows = context.Request.Params["rows"];
            //  string[] rows = strRows.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            //  int pagesize = 20;
            // foreach (string row in rows)
            // {
            //     if (pagesize < int.Parse(row)) {
            //         pagesize = int.Parse(row);
            //     }
            //   }

            int pagesize = int.Parse(context.Request.Params["rows"]);
            int pageindex = int.Parse(context.Request.Params["page"]);
            int total = 0;
            int pageCount = 0;
            StringBuilder strWhere = new StringBuilder();
            strWhere.Append(" isnull(repairstatus,0)<60 ");
            //获取待排单的数量
            DataTable dt = rrs.KanBan(pagesize, pageindex, out pageCount, out total, strWhere.ToString());
            StringHelper.JsonGZipResponse(context, JsonHelper.DataTableToJSON(dt, total, true));

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