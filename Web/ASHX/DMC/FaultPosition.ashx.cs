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
    /// FaultPosition 的摘要描述
    /// </summary>
    public class FaultPosition : IHttpHandler, IRequiresSessionState
    {
        //變量定義以及相關類的實例化
        string UserId = "a10857";
        string DeptId = "IT600";
        string CompanyId = "avccn";
        UserInfo currentUser = null;
        LanguageManageService languageManage = new LanguageManageService();
        UserManageService umserive = new UserManageService();
        LogService logs = new LogService();

        FaultPositionService fps = new FaultPositionService();

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
                    case "newfaultposition":
                        NewFaultPosition(context);
                        break;
                    case "updatefaultposition":
                        UpdateFaultPosition(context);
                        break;
                    case "getfaultpositiontree":
                        GetFaultPositionTree(context);
                        break;
                    case "getfaultpositionmain":
                        GetFaultPositionMain(context);
                        break;
                    case "getfaultpositionnode":
                        GetFaultPositionNode(context);
                        break;
                }
            }
            else
            {
                context.Response.Write("{\"success\": false ,\"msg\":\"登录失效\"}");
            }
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
                        strWhere.AppendFormat(" AND (PositionId like N'%{0}%' or PositionName like N'%{0}%' or PositionText like N'%{0}%' or Usey like N'%{0}%' ) ", key);
                    }
                    break;
                case "ByAdvanced":
                    if (!string.IsNullOrEmpty(context.Request.Params["PositionName"]))
                    {
                        strWhere.AppendFormat(" AND PositionName like N'%{0}%' ", context.Request.Params["PositionName"]);
                    }

                    if (!string.IsNullOrEmpty(context.Request.Params["Usey"]))
                    {
                        strWhere.AppendFormat(" AND Usey = N'{0}'", context.Request.Params["Usey"]);
                    }
                    break;
            }
            //取得相關查詢條件下的數據列表
            DataTable dt = fps.Search(pagesize, pageindex, out pageCount, out total, strWhere.ToString());
            StringBuilder sb = new StringBuilder();
            //將數據返回給客戶端
            sb = JsonHelper.DataTableToJSON(dt, total, true);
            StringHelper.JsonGZipResponse(context, sb);
        }

        /// <summary>
        /// 归属信息设置
        /// </summary>
        /// <param name="context"></param>
        public void NewFaultPosition(HttpContext context)
        {
            try
            {
                FaultPositionEntity entity = new FaultPositionEntity();
                entity.PositionId = context.Request["PositionId"];
                entity.PositionName = context.Request["PositionName"];
                entity.PPositionId = context.Request["PPositionId"];
                entity.PPositionText = context.Request["PPositionText"];
                entity.OrderId = Convert.ToInt32(context.Request["OrderId"]);
                entity.Usey = context.Request["Usey"];
                entity.GradeTime = context.Request["GradeTime"];
                entity.Grade = context.Request["Grade"];
                entity.PositionText = string.IsNullOrEmpty(entity.PPositionId) ? entity.PositionName : entity.PPositionText + "->" + entity.PositionName;
                string msg = fps.NewFaultPosition(entity);
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

        /// <summary>
        /// 归属信息设置
        /// </summary>
        /// <param name="context"></param>
        public void UpdateFaultPosition(HttpContext context)
        {
            try
            {
                FaultPositionEntity entity = new FaultPositionEntity();
                entity.PositionId = context.Request["PositionId"];
                entity.PositionName = context.Request["PositionName"];
                entity.PPositionId = context.Request["PPositionId"];
                entity.PPositionText = context.Request["PPositionText"];
                entity.OrderId = Convert.ToInt32(context.Request["OrderId"]);
                entity.Usey = context.Request["Usey"];
                entity.GradeTime = context.Request["GradeTime"];
                entity.Grade = context.Request["Grade"];
                entity.PositionText = string.IsNullOrEmpty(entity.PPositionId) ? entity.PositionName : entity.PPositionText + "->" + entity.PositionName;
                string msg = fps.UpdateFaultPosition(entity);
                if (string.IsNullOrWhiteSpace(msg))
                {
                    context.Response.Write("{\"success\":true,\"msg\":\"更新成功\"}");
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
        /// 获取树目录
        /// </summary>
        /// <param name="context"></param>
        public void GetFaultPositionTree(HttpContext context)
        {
            StringHelper.JsonGZipResponse(context, fps.GetFaultPositionTree());
        }
        /// <summary>
        /// 获取故障位置
        /// </summary>
        /// <param name="?"></param>
        public void GetFaultPositionMain(HttpContext context)
        {
            StringBuilder sb = JsonHelper.DataTableToJSON(fps.GetFaultPositionMain());
            StringHelper.JsonGZipResponse(context, sb);
        }

        /// <summary>
        /// 获取故障分类
        /// </summary>
        /// <param name="?"></param>
        public void GetFaultPositionNode(HttpContext context)
        {
            string PPositionId = context.Request["PPositionId"];
            StringBuilder sb = JsonHelper.DataTableToJSON(fps.GetFaultPositionNode(PPositionId));
            StringHelper.JsonGZipResponse(context, sb);
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