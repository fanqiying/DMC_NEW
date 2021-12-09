using DMC.BLL;
using DMC.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using Utility.HelpClass;
using DMC.DAL;
using System.Globalization;

namespace Web.ASHX.DMC
{
    /// <summary>
    /// Repairman 的摘要描述
    /// </summary>
    public class Repairman : IHttpHandler, IRequiresSessionState
    {
        //變量定義以及相關類的實例化
        string UserId = "a10857";
        string DeptId = "IT600";
        string CompanyId = "avccn";
        UserInfo currentUser = null;
        LanguageManageService languageManage = new LanguageManageService();
        UserManageService umserive = new UserManageService();
        LogService logs = new LogService();

        RepairmanService ds = new RepairmanService();

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
                    case "newrepairman":
                        NewRepairman(context);
                        break;
                    case "updaterepairman":
                        UpdateRepairman(context);
                        break;
                    case "deleterepairman":
                        DeleteRepairman(context);
                        break;
                    case "setworking":
                        SetWorking(context);
                        break;
                    case "getonduty":
                        //获取指定班别的人员
                        GetOnDuty(context);
                        break;
                    case "saveupload":
                        SaveUpload(context);//模板导入
                        break;
                    case "getrepairmworking":
                        GetRepairmWorking(context);
                        break;
                }
            }
            else
            {
                context.Response.Write("{\"success\": false ,\"msg\":\"登录失效\"}");
            }
        }
        RepairRecordService rrs = new RepairRecordService();
        /// <summary>
        /// 获取工作状况
        /// </summary>
        /// <param name="context"></param>
        public void GetRepairmWorking(HttpContext context)
        {
            DataTable dt = ds.GetRepairmWorking();
            List<object> mans = new List<object>();
            foreach (DataRow dr in dt.Rows)
            {
                mans.Add(new
                {
                    repairmanid = dr["RepairmanId"].ToString(),
                    repairmanname = dr["RepairmanName"].ToString(),
                    workedtime = Convert.ToInt32(dr["workedtime"]),
                    totaltime = Convert.ToInt32(dr["totaltime"]),
                    resttime = Convert.ToInt32(dr["resttime"]),
                    workingtime = Convert.ToInt32(dr["workingtime"]),
                    surplustime = Convert.ToInt32(dr["surplustime"])
                });
            }
            //获取待排单的数量
            DataTable dtQty = rrs.GetKanbanQty();
            var obj = new { waitqty = 0, workqty = 0, chaoshiqty = 0, qcqty = 0, scqty = 0, wscqty = 0, scsyqty = 0, pmqty = 0 };
            if (dtQty != null && dtQty.Rows.Count >= 0)
            {
                DataRow dr = dtQty.Rows[0];
                obj = new
                {
                    waitqty = Convert.ToInt32(dr["waitqty"]),
                    workqty = Convert.ToInt32(dr["workqty"]),
                    chaoshiqty = Convert.ToInt32(dr["chaoshiqty"]),
                    qcqty = Convert.ToInt32(dr["qcqty"]),
                    scqty = Convert.ToInt32(dr["scqty"]),
                    wscqty = Convert.ToInt32(dr["wscqty"]),
                    scsyqty = Convert.ToInt32(dr["scsyqty"]),
                    pmqty = Convert.ToInt32(dr["pmqty"])
                };
            }
            //StringBuilder sb = JsonHelper.DataTableToJSON(dt);
            var objData = new
            {
                ReportData = mans,
                RefershQTY = obj
            };
            StringHelper.JsonGZipResponse(context, new StringBuilder(objData.ObjectToJsonstring()));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void GetOnDuty(HttpContext context)
        {
            string ClassType = "0";
            RepairmanEntity entity = new RepairmanEntity();

            UserManageService umserive = new UserManageService();
            var currentUser = umserive.GetUserMain();
            entity.RepairmanId = currentUser.userID;

            entity.YearMonth = DateTime.Now.ToString("yyyy-MM");
            entity.WorkDate = DateTime.Now.ToString("yyyy-MM-dd");
            ClassType = ds.GetClassType(entity);
            //if ((DateTime.Now.Hour > 8 && DateTime.Now.Hour < 19) ||
            //    (DateTime.Now.Hour == 7 && DateTime.Now.Minute >= 30) ||
            //    (DateTime.Now.Hour == 19 && DateTime.Now.Minute <= 30))
            //{
            //    ClassType = "1";
            //}
            DataTable dt = ds.GetOnDutyUser(ClassType);
            StringBuilder sb = JsonHelper.DataTableToJSON(dt);
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
            switch (type)
            {
                case "ByKey":
                    string key = context.Request.Params["KeyWord"];
                    if (!string.IsNullOrEmpty(key))
                    {
                        strWhere.AppendFormat(" AND (RepairmanId like N'%{0}%' or RepairmanName like N'%{0}%' or ClassType like N'%{0}%' or IsLeader like N'%{0}%' ) ", key);
                    }
                    break;
                case "ByAdvanced":
                    if (!string.IsNullOrEmpty(context.Request.Params["Repairman"]))
                    {
                        strWhere.AppendFormat(" AND (RepairmanId like N'%{0}%' or RepairmanName like N'%{0}%')", context.Request.Params["Repairman"]);
                    }

                    if (!string.IsNullOrEmpty(context.Request.Params["IsLeader"]))
                    {
                        strWhere.AppendFormat(" AND IsLeader = N'{0}'", context.Request.Params["IsLeader"]);
                    }

                    if (!string.IsNullOrEmpty(context.Request.Params["YearMonth"]))
                    {
                        strWhere.AppendFormat(" AND YearMonth = N'{0}'", context.Request.Params["YearMonth"]);
                    }
                    break;
            }
            //取得相關查詢條件下的數據列表
            DataTable dt = ds.Search(pagesize, pageindex, out pageCount, out total, strWhere.ToString());
            StringBuilder sb = new StringBuilder();
            //將數據返回給客戶端
            sb = JsonHelper.DataTableToJSON(dt, total, true);
            StringHelper.JsonGZipResponse(context, sb);
        }

        /// <summary>
        /// 归属信息设置
        /// </summary>
        /// <param name="context"></param>
        public void NewRepairman(HttpContext context)
        {
            try
            {
                RepairmanEntity entity = new RepairmanEntity();
                entity.RepairmanId = context.Request["RepairmanId"];
                entity.RepairmanName = context.Request["RepairmanName"];
                entity.ClassType = context.Request["ClassType"];
                entity.IsLeader = context.Request["IsLeader"];
                entity.IsWorking = context.Request["IsWorking"];
                entity.PhotoUrl = context.Request["PhotoUrl"];
                entity.WorkRangeTime = context.Request["WorkRangeTime"];
                entity.YearMonth = context.Request["YearMonth"];
                entity.WorkRangeTimeBegin = context.Request["WorkRangeTimeBegin"];
                entity.WorkRangeTimeEnd = context.Request["WorkRangeTimeEnd"];
                entity.WorkDate = context.Request["WorkDate"];
                entity.WorkNum = Convert.ToSingle(context.Request["WorkNum"]);
                string msg = ds.NewRepairman(entity);
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
        public void UpdateRepairman(HttpContext context)
        {
            try
            {
                RepairmanEntity entity = new RepairmanEntity();
                entity.RepairmanId = context.Request["RepairmanId"];
                entity.RepairmanName = context.Request["RepairmanName"];
                entity.ClassType = context.Request["ClassType"];
                entity.IsLeader = context.Request["IsLeader"];
                entity.IsWorking = context.Request["IsWorking"];
                entity.PhotoUrl = context.Request["PhotoUrl"];
                entity.WorkRangeTime = context.Request["WorkRangeTime"];
                entity.YearMonth = context.Request["YearMonth"];
                entity.WorkRangeTimeBegin = context.Request["WorkRangeTimeBegin"];
                entity.WorkRangeTimeEnd = context.Request["WorkRangeTimeEnd"];
                entity.WorkDate = context.Request["WorkDate"];
                entity.WorkNum = Convert.ToSingle(context.Request["WorkNum"]);
                string msg = ds.UpdateRepairman(entity);
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
        public void DeleteRepairman(HttpContext context)
        {
            try
            {
                RepairmanEntity entity = new RepairmanEntity();
                entity.RepairmanId = context.Request["RepairmanId"];
                entity.WorkDate = context.Request["WorkDate"];
                string msg = ds.DeleteRepairman(entity);
                if (string.IsNullOrWhiteSpace(msg))
                {
                    context.Response.Write("{\"success\":true,\"msg\":\"删除成功\"}");
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
        public void SetWorking(HttpContext context)
        {
            try
            {
                RepairmanEntity entity = new RepairmanEntity();
                entity.RepairmanId = context.Request["RepairmanId"];
                entity.WorkDate = context.Request["WorkDate"];
                entity.IsWorking = context.Request["IsWorking"];
                string msg = ds.SetWorking(entity);
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
        /// 添加上傳記錄
        /// </summary>
        /// <param name="context"></param>
        private void SaveUpload(HttpContext context)
        {
            string msg = string.Empty;
            string date = "";
            DateTime dtDay;
            var files = context.Request.Files;
            string FullFilePath = string.Empty;

            try
            {
                HttpPostedFile fileData = files.Get("Filedata");
                if (fileData != null && fileData.ContentLength > 0)
                {
                    List<int> fails = new List<int>();
                    context.Request.ContentEncoding = Encoding.GetEncoding("UTF-8");
                    string fileName = Path.GetFileName(fileData.FileName);      //原始文件名称
                    string fileExtension = Path.GetExtension(fileName);         //文件扩展名
                    string Uuid = context.Request["guid"].ToString();
                    FullFilePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("..\\..\\TempFile"), Uuid + fileExtension);
                    fileData.SaveAs(FullFilePath);
                    DataTable tbExcel = NPOIExcel.ExcelToDataTable(FullFilePath, true);
                    double datenum;
                    date = tbExcel.Rows[0][0].ToString();
                    if (date == null)
                    {
                        date = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString().PadLeft(2, '0');
                    }
                    ds.DeleteRepairman_WordDate(date);

                    for (int i = 2; i < tbExcel.Rows.Count; i++)
                    {
                        try
                        {
                            DataRow dr = tbExcel.Rows[i];
                            if (Convert.IsDBNull(dr[0]) || string.IsNullOrWhiteSpace(dr[0].ToString()))
                                break;
                            RepairmanEntity item = new RepairmanEntity();
                            item.RepairmanId = dr[0].ToString().Trim();
                            item.RepairmanName = dr[1].ToString().Trim();
                            item.IsLeader = dr[2].ToString().Trim() == "Y" ? "1" : "0";
                            item.ClassType = dr[3].ToString().Trim() == "M" ? "1" : "0";
                            item.GroupName = dr[4].ToString().Trim();

                            item.YearMonth = "";
                            item.PhotoUrl = "";

                            for (int j = 6; j < tbExcel.Columns.Count; j++)
                            {
                                if (tbExcel.Rows[0][j].ToString() == "")
                                {
                                    break;
                                }

                                date = tbExcel.Rows[0][0].ToString();
                                item.YearMonth = date;
                                date = date + "-" + tbExcel.Rows[0][j].ToString().PadLeft(2, '0');
                                item.WorkDate = date;

                                if (item.ClassType == "1")
                                {

                                    if (Convert.ToDouble(dr[j]) == 0)
                                    {
                                        item.IsWorking = "0";
                                        item.WorkRangeTimeBegin = "";
                                        item.WorkRangeTimeEnd = "";
                                        item.WorkNum = 0;
                                    }
                                    else
                                    {
                                        item.IsWorking = "1";
                                        item.WorkRangeTimeBegin = date + " 07:15:00";
                                        if (Convert.ToDouble(dr[j]) > 8)
                                        {
                                            datenum = Convert.ToDouble(dr[j]);
                                        }
                                        else
                                        {
                                            datenum = Convert.ToDouble(dr[j]) + 8;
                                        }
                                        dtDay = DateTime.ParseExact(date + " 07:15:00", "yyyy-MM-dd HH:mm:ss", null).AddHours(datenum).AddMinutes(90);
                                        item.WorkNum = datenum;
                                        item.WorkRangeTimeEnd = dtDay.ToString();
                                    }
                                }
                                else
                                {
                                    if (Convert.ToDouble(dr[j]) == 0)
                                    {
                                        item.IsWorking = "0";
                                        item.WorkRangeTimeBegin = "";
                                        item.WorkRangeTimeEnd = "";
                                    }
                                    else
                                    {
                                        item.IsWorking = "1";
                                        item.WorkRangeTimeBegin = date + " 19:15:00";

                                        if (Convert.ToDouble(dr[j]) > 8)
                                        {
                                            datenum = Convert.ToDouble(dr[j]);

                                        }
                                        else
                                        {

                                            datenum = Convert.ToDouble(dr[j]) + 8;

                                        }

                                        dtDay = DateTime.ParseExact(date + " 19:15:00", "yyyy-MM-dd HH:mm:ss", null).AddHours(datenum).AddMinutes(90);

                                        item.WorkNum = datenum;
                                        item.WorkRangeTimeEnd = dtDay.ToString();


                                    }
                                }
                                if (Convert.ToDouble(dr[j]) == 0)
                                {
                                    item.WorkRangeTime = "";
                                }
                                else
                                {
                                    item.WorkRangeTime = item.WorkRangeTimeBegin + "-*" + item.WorkRangeTimeEnd;
                                }
                                if (!string.IsNullOrWhiteSpace(ds.NewRepairman(item)))
                                {
                                    fails.Add(i);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            msg = ex.ToString();
                            fails.Add(i);

                        }
                    }
                    if (fails.Count == tbExcel.Rows.Count)
                    {
                        msg = "数据导入失败";
                    }
                    else if (fails.Count > 0)
                    {
                        msg = string.Format("第【{0}】行数据导入失败", string.Join(",", fails));
                    }
                }
                else
                {
                    msg = "未获取到文件内容";
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
                //删除文件
                if (!string.IsNullOrWhiteSpace(FullFilePath))
                {
                    File.Delete(FullFilePath);
                }
            }
            ////需要返回单号或错误信息
            StringBuilder res = new StringBuilder();
            res.Append("{");
            res.Append("\"success\":" + (string.IsNullOrEmpty(msg) ? true : false).ToString().ToLower());
            res.Append(",\"msg\":\"" + msg + "\"");
            res.Append("}");
            context.Response.Write(res.ToString());
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