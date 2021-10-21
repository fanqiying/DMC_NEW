using DMC.BLL;
using DMC.Model;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.SessionState;
using Utility.HelpClass;
using System.Text.RegularExpressions;

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
                    case "download":
                        DownLoad(context);
                        break;
                    case "downloadhour":
                        DownloadHour(context);
                        break;
                    case "getkanbanqty":
                        GetKanbanQty(context);
                        break;
                    case "searchreport":
                        SearchReport(context);
                        break;
                    case "tingjifenxi":
                        TingjiFenxi(context);
                        break;
                    case "tingjifenxinew":
                        TingjiFenxiNew(context);
                        break;
                    case "tingjifenxinewdownload":
                        TingjiFenxiNewdownload(context);
                        break;

                }
            }
            else
            {
                context.Response.Write("{\"success\": false ,\"msg\":\"登录失效\"}");
            }
        }

        private void DownloadHour(HttpContext context)
        {
            int pagesize = 1000000;
            int pageindex = 1;
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

                    if (!string.IsNullOrEmpty(context.Request.Params["YearMonth"]) && !string.IsNullOrEmpty(context.Request.Params["EYearMonth"]))
                    {
                        strWhere.AppendFormat(" AND RepairSTime  >= '{0}'", context.Request.Params["YearMonth"]);
                        strWhere.AppendFormat(" AND RepairSTime   <= '{0}'", context.Request.Params["EYearMonth"]);
                    }

                    break;
            }

            DataTable dt = rrs.SearchHoure(pagesize, pageindex, out pageCount, out total, strWhere.ToString());
            DataTable dtSns = new DataTable();
            dtSns.Columns.Add("指派时间", typeof(string));
            dtSns.Columns.Add("维修单号", typeof(string));
            dtSns.Columns.Add("完成时间", typeof(string));
            dtSns.Columns.Add("故障位置", typeof(string));
            dtSns.Columns.Add("故障现象", typeof(string));
            dtSns.Columns.Add("维修员", typeof(string));
            dtSns.Columns.Add("维修时间(分钟)", typeof(string));
            dtSns.Columns.Add("标准时间(分钟)", typeof(string));
            dtSns.Columns.Add("标准评分", typeof(string));
            dtSns.Columns.Add("绩效比(%)", typeof(string));
            dtSns.Columns.Add("故障位置1", typeof(string));
            dtSns.Columns.Add("故障现象1", typeof(string));
            foreach (DataRow item in dt.Rows)
            {
                DataRow dr = dtSns.NewRow();
                dr["指派时间"] = Convert.ToDateTime(item["repairstime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                dr["维修单号"] = item["repairformno"].ToString();
                dr["完成时间"] = (Convert.IsDBNull(item["repairetime"]) ? "" : Convert.ToDateTime(item["repairetime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                dr["故障位置"] = item["positiontext"].ToString();
                dr["故障现象"] = item["phenomenatext"].ToString();
                dr["维修员"] = item["repairmanname"].ToString();
                dr["维修时间(分钟)"] = item["manhoure"].ToString();
                dr["标准时间(分钟)"] = item["gradetime"].ToString();
                dr["标准评分"] = item["standgrade"].ToString();
                dr["绩效比(%)"] = ((Convert.ToInt32(item["gradetime"]) - Convert.ToInt32(item["manhoure"])) * 100 / Convert.ToInt32(item["gradetime"])).ToString();
                dr["故障位置1"] = item["positiontext1"].ToString();
                dr["故障现象1"] = item["phenomenatext1"].ToString();
                dtSns.Rows.Add(dr);
            }
            byte[] bytes = dataTableToCsv(dtSns);
            string fileName = GetData.GetRequest("fileName");
            context.Response.ContentType = "application/octet-stream";
            context.Response.AddHeader("Content-Disposition", "attachment;  filename=" + HttpUtility.UrlEncode(fileName));
            context.Response.BinaryWrite(bytes);
            context.Response.End();
        }

        private void DownLoad(HttpContext context)
        {
            int pagesize = 1000000;
            int pageindex = 1;
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
                    if (!string.IsNullOrEmpty(context.Request.Params["YearMonth"]) && !string.IsNullOrEmpty(context.Request.Params["EYearMonth"]))
                    {
                        strWhere.AppendFormat(" AND RepairSTime  >= '{0}'", context.Request.Params["YearMonth"]);
                        strWhere.AppendFormat(" AND RepairSTime   <= '{0}'", context.Request.Params["EYearMonth"]);
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
                        //strWhere.AppendFormat(" AND ApplyUserId = '{0}'", UserId);
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
                        //strWhere.AppendFormat(" AND EXISTS(SELECT 1 FROM t_Employee WHERE empid =t_RepairRecord.ApplyUserId and signerID = '{0}')", UserId);
                        break;
                    default:
                        strWhere.AppendFormat(" AND RepairStatus = '{0}'", context.Request.Params["RepairStatus"]);
                        break;
                }

            }
            //取得相關查詢條件下的數據列表
            DataTable dt = rrs.Search(pagesize, pageindex, out pageCount, out total, strWhere.ToString());

            DataTable dtSns = new DataTable();
            dtSns.Columns.Add("维修单号", typeof(string));
            dtSns.Columns.Add("维修员", typeof(string));
            dtSns.Columns.Add("状态", typeof(string));
            dtSns.Columns.Add("设备编号", typeof(string));
            dtSns.Columns.Add("故障位置", typeof(string));
            dtSns.Columns.Add("故障现象", typeof(string));
            dtSns.Columns.Add("故障分析", typeof(string));

            dtSns.Columns.Add("故障时间", typeof(string));
            dtSns.Columns.Add("指派时间", typeof(string));
            dtSns.Columns.Add("完成时间", typeof(string));
            dtSns.Columns.Add("IPQC确认时间", typeof(string));
            dtSns.Columns.Add("生产确认时间", typeof(string));
            dtSns.Columns.Add("IPQC确认", typeof(string));
            dtSns.Columns.Add("生产确认", typeof(string));
            dtSns.Columns.Add("模具编号1", typeof(string));
            dtSns.Columns.Add("模具编号2", typeof(string));
            dtSns.Columns.Add("新模编号1", typeof(string));
            dtSns.Columns.Add("新模编号2", typeof(string));
            dtSns.Columns.Add("返修原因", typeof(string));
            dtSns.Columns.Add("故障位置1", typeof(string));
            dtSns.Columns.Add("故障现象1", typeof(string));
            dtSns.Columns.Add("申请人", typeof(string));
            foreach (DataRow item in dt.Rows)
            {
                DataRow dr = dtSns.NewRow();
                dr["维修单号"] = item["repairformno"].ToString();
                dr["维修员"] = item["repairmanname"].ToString();
                var text = "N/A";
                switch (item["repairstatus"].ToString())
                {
                    case "10":
                        text = "10-待指派";
                        break;
                    case "12":
                        text = "12-待指派(挂单)";
                        break;
                    case "24":
                        text = "24-待维修(IPQC返修)";
                        break;
                    case "25":
                        text = "25-待维修(组长返修)";
                        break;
                    case "20":
                        text = "20-待维修";
                        break;
                    case "23":
                        text = "23-待维修(返修)";
                        break;
                    case "30":
                        text = "30-待生产员确认";
                        break;
                    case "40":
                        text = "40-待IPQC确认";
                        break;
                    case "50":
                        text = "50-待组长确认";
                        break;
                    case "61":
                        text = "61-生产员返修";
                        break;
                    case "62":
                        text = "62 -挂单完结";
                        break;
                    case "64":
                        text = "64-QC返修";
                        break;
                    case "63":
                        text = "63-生产员返修";
                        break;
                    case "60":
                        text = "维修完成";
                        break;
                    case "65":
                        text = "65-生产组长返修";
                        break;
                    default:
                        text = item["repairstatus"].ToString() + "-维修完成";
                        break;
                }
                dr["状态"] = text;
                dr["设备编号"] = Regex.Replace(item["deviceid"].ToString(), @"[\n\r]", "");
                dr["故障位置"] = item["positiontext"].ToString();
                dr["故障现象"] = item["phenomenatext"].ToString();
                dr["故障分析"] = Regex.Replace(item["faultanalysis"].ToString(), @"[\n\r]", "");
                dr["故障时间"] = item["faulttime"].ToString();
                dr["指派时间"] = (Convert.IsDBNull(item["repairstime"]) ? "" : Convert.ToDateTime(item["repairstime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                dr["完成时间"] = (Convert.IsDBNull(item["repairetime"]) ? "" : Convert.ToDateTime(item["repairetime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                dr["IPQC确认时间"] = (Convert.IsDBNull(item["qcconfirmtime"]) ? "" : Convert.ToDateTime(item["qcconfirmtime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                dr["生产确认时间"] = item["confirmtime"].ToString();

                dr["IPQC确认"] = item["ipqcnumber"].ToString();// item.lenovo_part_no;//联想料号
                dr["生产确认"] = item["confirmuser"].ToString();// item.lenovo_part_no;//联想料号

                dr["模具编号1"] = item["mouldid"].ToString();
                dr["模具编号2"] = item["mouldid1"].ToString();
                dr["新模编号1"] = item["newmouldid"].ToString();
                dr["新模编号2"] = item["newmouldid1"].ToString();
                dr["返修原因"] = Regex.Replace(item["rebackreason"].ToString(), @"[\n\r]", "");

                dr["故障位置1"] = item["positiontext1"].ToString();// item.product_line;//产品类别 
                dr["故障现象1"] = item["phenomenatext1"].ToString(); //item.test_station;//产品类别 
                dr["申请人"] = item["applyuserid"].ToString(); //item.product_type;//产品类别  
                dtSns.Rows.Add(dr);
            }
            Dictionary<String, DataTable> dic = new Dictionary<string, DataTable>();
            dic.Add("维修明细", dtSns);
            byte[] bytes = dataTableToCsv(dtSns);
            string fileName = GetData.GetRequest("fileName");
            context.Response.ContentType = "application/octet-stream";
            context.Response.AddHeader("Content-Disposition", "attachment;  filename=" + HttpUtility.UrlEncode(fileName));
            context.Response.BinaryWrite(bytes);
            context.Response.End();
        }

        private byte[] dataTableToCsv(DataTable table)
        {
            byte[] result = null;
            string title = "";
            //FileStream fs = new FileStream(path + "\\" + name, FileMode.Create);
            using (MemoryStream fs = new MemoryStream())
            {
                StreamWriter sw = new StreamWriter(new BufferedStream(fs), System.Text.Encoding.UTF8);

                for (int i = 0; i < table.Columns.Count; i++)
                {
                    title += table.Columns[i].ColumnName + ",";
                }
                title = title.Substring(0, title.Length - 1) + "\n";
                sw.Write(title);

                foreach (DataRow row in table.Rows)
                {
                    if (row.RowState == DataRowState.Deleted) continue;
                    string line = "";
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        line += row[i].ToString().Replace(",", "") + ",";
                    }
                    line = line.Substring(0, line.Length - 1) + "\n";

                    sw.Write(line);
                }

                sw.Close();
                result = fs.ToArray();
            }
            return result;
        }

        public byte[] TablesToExcelByte(Dictionary<String, DataTable> tables)
        {
            //在内存中生成一个Excel文件：
            XSSFWorkbook book = new XSSFWorkbook();
            if (tables != null && tables.Count > 0)
                foreach (var table in tables)
                {
                    ISheet sheet = null;
                    sheet = book.CreateSheet(table.Key);
                    sheet.DefaultRowHeight = 20 * 10;
                    int rowIndex = 0;
                    int StartColIndex = 0;
                    int colIndex = StartColIndex;
                    //创建表头样式
                    ICellStyle style = book.CreateCellStyle();
                    style.Alignment = HorizontalAlignment.Center;
                    style.WrapText = true;
                    IFont font = book.CreateFont();
                    font.FontHeightInPoints = 16;
                    font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                    font.FontName = "简体中文";
                    style.SetFont(font);//HEAD 样式

                    DataTable dt = table.Value;
                    List<String> columnNames = new List<string>();
                    #region 定义表头
                    int m = 0;
                    if (dt != null)
                    {
                        var rowheader = sheet.CreateRow(0);
                        rowheader.Height = rowheader.Height = 20 * 20;
                        foreach (DataColumn cName in dt.Columns)
                        {
                            if (cName != null)
                            {
                                rowheader.CreateCell(m).SetCellValue(cName.ColumnName);
                                columnNames.Add(cName.ColumnName);
                                m++;
                            }
                        }
                    }
                    #endregion

                    #region 定义表体并赋值
                    //如果没有找到可用的属性则结束
                    if (dt.Rows.Count == 0) { continue; }
                    foreach (DataRow dr in dt.Rows)
                    {
                        int n = 0;
                        if (sheet != null)
                        {
                            rowIndex++;
                            var sheetrow = sheet.CreateRow(rowIndex);
                            sheetrow.Height = sheetrow.Height = 20 * 20;
                            foreach (String ColumnName in columnNames)
                            {
                                DataColumn column = dt.Columns[ColumnName];
                                if (!Convert.IsDBNull(dr[ColumnName]))
                                {
                                    if (column.DataType.ToString().ToLower().IndexOf("date") > -1)
                                    {
                                        sheetrow.CreateCell(n).SetCellValue(Convert.ToDateTime(dr[ColumnName]).ToString("yyyy-MM-dd HH:mm:ss"));
                                    }
                                    else if (column.DataType.ToString().ToLower().IndexOf("int") > -1 ||
                                        column.DataType.ToString().ToLower().IndexOf("numeric") > -1 ||
                                        column.DataType.ToString().ToLower().IndexOf("decimal") > -1 ||
                                        column.DataType.ToString().ToLower().IndexOf("float") > -1)
                                    {
                                        sheetrow.CreateCell(n).SetCellValue(Convert.ToDouble(dr[ColumnName]));
                                    }
                                    else
                                    {
                                        sheetrow.CreateCell(n).SetCellValue(dr[ColumnName].ToString());
                                    }
                                }
                                else
                                {
                                    sheetrow.CreateCell(n).SetCellValue("");
                                }
                                n++;
                            }
                        }
                    }
                    #endregion
                }
            else
            {
                //在工作薄中建立工作表
                XSSFSheet sheet = book.CreateSheet() as XSSFSheet;
                sheet.SetColumnWidth(0, 30 * 256);
                if (sheet != null) sheet.CreateRow(0).CreateCell(0).SetCellValue("暂无数据！");
            }
            byte[] result = null;
            using (MemoryStream ms = new MemoryStream())
            {
                book.Write(ms);
                result = ms.ToArray();
            }
            return result;
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


                    if (!string.IsNullOrEmpty(context.Request.Params["YearMonth"]) && !string.IsNullOrEmpty(context.Request.Params["EYearMonth"]))
                    {
                        strWhere.AppendFormat(" AND RepairSTime  >= '{0}'", context.Request.Params["YearMonth"]);
                        strWhere.AppendFormat(" AND RepairSTime   <= '{0}'", context.Request.Params["EYearMonth"]);
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
                    if (!string.IsNullOrEmpty(context.Request.Params["YearMonth"]) && !string.IsNullOrEmpty(context.Request.Params["EYearMonth"]))
                    {
                        strWhere.AppendFormat(" AND RepairSTime  >= '{0}'", context.Request.Params["YearMonth"]);
                        strWhere.AppendFormat(" AND RepairSTime   <= '{0}'", context.Request.Params["EYearMonth"]);
                    }
                    if (!string.IsNullOrEmpty(context.Request.Params["RepairmanId"]))
                    {
                        strWhere.AppendFormat(" AND RepairmanId = '{0}'", context.Request.Params["RepairmanId"]);
                    }
                    if (!string.IsNullOrEmpty(context.Request.Params["RepairmanName"]))
                    {
                        strWhere.AppendFormat(" and RepairmanId =(select top 1 t_Repairman.RepairmanId from t_Repairman where t_Repairman.RepairmanName= '{0}' )", context.Request.Params["RepairmanName"]);
                    }
                    break;
            }
            //UserId维修员


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
                        //strWhere.AppendFormat(" AND ApplyUserId = '{0}'", UserId);
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
                        //strWhere.AppendFormat(" AND EXISTS(SELECT 1 FROM t_Employee WHERE empid =t_RepairRecord.ApplyUserId and signerID = '{0}')", UserId);
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
                string LeaderID = context.Request["LeaderID"];
                string msg = rrs.LeaderReject(AutoId, RepairFormNO, RebackReason, LeaderID, "50", "65", "25");
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
                string LeaderID = context.Request["LeaderID"];
                string msg = rrs.LeaderReject(AutoId, RepairFormNO, RebackReason, LeaderID, "30", "63", "23");
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
                string LeaderID = context.Request["LeaderID"];
                string msg = rrs.LeaderAppraise(AutoId, RepairFormNO, LeaderID);
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
            strWhere.Append(" ((isnull(FormStatus,0) between 20 and 60 and isnull(repairstatus,10)<60) or (isnull(FormStatus,0)<20)) and a.DeviceId!='模房'");
            //获取待排单的数量
            DataTable dt = rrs.KanBan(pagesize, pageindex, out pageCount, out total, strWhere.ToString());
            StringHelper.JsonGZipResponse(context, JsonHelper.DataTableToJSON(dt, total, true));

        }
        /// <summary>
        /// 获取看板任务数量
        /// </summary>
        /// <param name="context"></param>
        public void GetKanbanQty(HttpContext context)
        {
            //获取待排单的数量
            StringBuilder sbSql = new StringBuilder();
            //           sbSql.Append(@"select SUM(case when ISNULL(RepairStatus,0)<20 THEN 1 ELSE 0 END) WaitQty, 
            //      SUM(case when ISNULL(RepairStatus,0)>=20 AND ISNULL(RepairStatus,0)<30 THEN 1 ELSE 0 END) WorkQty,
            //      SUM(case when (( RepairStatus!=30 and RepairStatus!=50) or ISNULL(RepairStatus,0)<20)and  c.GradeTime<datediff(minute ,a.faulttime, GetDate()) THEN 1 ELSE 0 END)  CHAOSHIQty,
            //   SUM(case when RepairStatus=40  THEN 1 ELSE 0 END) QCQty,
            //   SUM(case when RepairStatus=30 or RepairStatus=50  THEN 1 ELSE 0 END) SCQty
            // from t_RepairForm a with(nolock) left join 
            //      t_RepairRecord b  with(nolock) on a.RepairFormNO=b.RepairFormNO and a.RepairRecordId=b.AutoId left join 
            //      t_FaultPosition c  with(nolock) on  a.PositionId=c.PPositionId and a.PhenomenaId=c.PositionId
            //where (isnull(FormStatus,0) between 20 and 60 and isnull(repairstatus,10)<60) or (isnull(FormStatus,0)<20) ");
            sbSql.Append(@"select SUM(case when FormStatus in(10,12) THEN 1 ELSE 0 END) WaitQty, 
       SUM(case when FormStatus in(20,23,24,25,61,65) THEN 1 ELSE 0 END) WorkQty,
       SUM(case when FormStatus not in(30,40,50) and  c.GradeTime+60<datediff(minute ,a.faulttime, GetDate()) THEN 1 ELSE 0 END)  CHAOSHIQty,
	   SUM(case when FormStatus=40  THEN 1 ELSE 0 END) QCQty,
	   SUM(case when FormStatus=30 or FormStatus=50  THEN 1 ELSE 0 END) SCQty
  from t_RepairForm a with(nolock) left join 
       t_RepairRecord b  with(nolock) on a.RepairFormNO=b.RepairFormNO and a.RepairRecordId=b.AutoId left join 
       t_FaultPosition c  with(nolock) on  a.PositionId=c.PPositionId and a.PhenomenaId=c.PositionId
 where (isnull(FormStatus,0) between 20 and 60 and isnull(repairstatus,10)<60) or (isnull(FormStatus,0)<20) ");
            DataTable dt = rrs.ExecSQLTODT(sbSql);
            StringHelper.JsonGZipResponse(context, JsonHelper.DataTableToJSON(dt));
        }

        public void SearchReport(HttpContext context)
        {

            int total = 0;
            int pageCount = 0;
            StringBuilder strWhere = new StringBuilder();
            strWhere.Append(" 1=1 ");
            string type = context.Request.Params["SearchType"];

            string key = string.Empty;
            key = context.Request.Params["KeyWord"];
            if (!string.IsNullOrEmpty(context.Request.Params["phenomenaid"]))
            {
                strWhere.AppendFormat(" AND a.phenomenaid = N'{0}'", context.Request.Params["phenomenaid"]);
            }


            if (!string.IsNullOrEmpty(context.Request.Params["RepairmanId"]))
            {
                strWhere.AppendFormat(" AND a.RepairmanId = N'{0}'", context.Request.Params["RepairmanId"]);
            }


            if (!string.IsNullOrEmpty(context.Request.Params["YearMonth"]) && !string.IsNullOrEmpty(context.Request.Params["EYearMonth"]))
            {
                strWhere.AppendFormat(" AND a.RepairETime  >= '{0}'", context.Request.Params["YearMonth"]);
                strWhere.AppendFormat(" AND a.RepairETime   <= '{0}'", context.Request.Params["EYearMonth"]);
            }
            StringBuilder sql = new StringBuilder();

            switch (type)
            {
                case "chart1":
                    sql.Append(@"select top 15 repairmanname,count(RepairFormNO) rfnum ,sum(cast(StandGrade as numeric))   standgrade,
                                cast(1.0*sum(1.0*(GradeTime-manhoure)/GradeTime)/count(RepairFormNO) as decimal(18,2))*100   opt 
                                from (
                               select a.RepairmanId,a.repairmanname , 
                                b.GradeTime,RepairSTime,RepairETime,b.Grade StandGrade,a.RepairFormNO,
                                datediff(minute ,RepairSTime,isnull(RepairETime,GetDate()))-(case when RepairSTime<cast(convert(varchar(10),RepairSTime,121)+ ' 12:30:00' as datetime) and isnull(RepairETime,GetDate())>cast(convert(varchar(10),RepairSTime,121)+ ' 13:30:00' as datetime) then 1 else 0 end)*40 manhoure
                                 from 
                                t_RepairRecord a left join t_FaultPosition b on a.PhenomenaId=b.PositionId
                                left join t_User c on a.RepairmanId=c.userID
                                where " + strWhere);
                    sql.Append(@")t group by repairmanname order by 3 desc");
                    break;
                case "chart2":

                    sql.Append(@"select top 20 DeviceId, cast(1.0*sum(cast(manhoure as numeric))/60 as decimal(18,1))   manhoure from (
                                select a.DeviceId,
                                b.GradeTime,RepairSTime,RepairETime,b.Grade StandGrade,
                                datediff(minute ,RepairSTime,isnull(RepairETime,GetDate()))-(case when RepairSTime<cast(convert(varchar(10),RepairSTime,121)+ ' 12:30:00' as datetime) and isnull(RepairETime,GetDate())>cast(convert(varchar(10),RepairSTime,121)+ ' 13:30:00' as datetime) then 1 else 0 end)*40 manhoure
                                 from 
                                t_RepairRecord a left join t_FaultPosition b on a.PhenomenaId=b.PositionId
                                left join t_User c on a.RepairmanId=c.userID where a.DeviceId!='模房' and " + strWhere);
                    sql.Append(@")t group by DeviceId order by 2 desc");
                    break;
                case "chart3":
                    sql.Append(@"select repairmanname ,PositionText, cast(1.0*sum(cast(manhoure as numeric))/60 as decimal(18,1))   manhoure from (
                               select a.RepairmanId,a.repairmanname ,c.userName, 
                                b.GradeTime,RepairSTime,RepairETime,b.Grade StandGrade,b.PositionText,
                                datediff(minute ,RepairSTime,isnull(RepairETime,GetDate()))-(case when RepairSTime<cast(convert(varchar(10),RepairSTime,121)+ ' 12:30:00' as datetime) and isnull(RepairETime,GetDate())>cast(convert(varchar(10),RepairSTime,121)+ ' 13:30:00' as datetime) then 1 else 0 end)*40 manhoure
                                 from 
                                t_RepairRecord a left join t_FaultPosition b on a.PhenomenaId=b.PositionId
                                left join t_User c on a.RepairmanId=c.userID
                                where " + strWhere);
                    sql.Append(" and b.PositionText in (");
                    sql.Append(@"select top 10 PositionText from (
                                select a.DeviceId,b.PositionText,
                                b.GradeTime,RepairSTime,RepairETime,b.Grade StandGrade,
                                datediff(minute ,RepairSTime,isnull(RepairETime,GetDate()))-(case when RepairSTime<cast(convert(varchar(10),RepairSTime,121)+ ' 12:30:00' as datetime) and isnull(RepairETime,GetDate())>cast(convert(varchar(10),RepairSTime,121)+ ' 13:30:00' as datetime) then 1 else 0 end)*40 manhoure
                                 from 
                                t_RepairRecord a left join t_FaultPosition b on a.PhenomenaId=b.PositionId
                                left join t_User c on a.RepairmanId=c.userID
                                where " + strWhere);
                    sql.Append(@") t1 group by PositionText order by sum(manhoure) desc)");
                    sql.Append(" and a.RepairmanId in(");
                    sql.Append(@"select top 6 RepairmanId from (
                                select a.DeviceId,b.PositionText,a.RepairmanId,
                                b.GradeTime,RepairSTime,RepairETime,b.Grade StandGrade,
                                datediff(minute ,RepairSTime,isnull(RepairETime,GetDate()))-(case when RepairSTime<cast(convert(varchar(10),RepairSTime,121)+ ' 12:30:00' as datetime) and isnull(RepairETime,GetDate())>cast(convert(varchar(10),RepairSTime,121)+ ' 13:30:00' as datetime) then 1 else 0 end)*40 manhoure
                                 from 
                                t_RepairRecord a left join t_FaultPosition b on a.PhenomenaId=b.PositionId
                                left join t_User c on a.RepairmanId=c.userID
                                where " + strWhere);
                    sql.Append(@") t1 group by RepairmanId order by sum(manhoure) desc)");
                    sql.Append(@")t group by repairmanname ,PositionText order by 2 desc");
                    break;
                case "chart4":
                    sql.Append(@"select top 10 PositionText,cast(1.0*sum(cast(manhoure as numeric))/60 as decimal(18,1))   manhoure from (
                                select a.DeviceId,b.PositionText,
                                b.GradeTime,RepairSTime,RepairETime,b.Grade StandGrade,
                                datediff(minute ,RepairSTime,isnull(RepairETime,GetDate()))-(case when RepairSTime<cast(convert(varchar(10),RepairSTime,121)+ ' 12:30:00' as datetime) and isnull(RepairETime,GetDate())>cast(convert(varchar(10),RepairSTime,121)+ ' 13:30:00' as datetime) then 1 else 0 end)*40 manhoure
                                 from 
                                t_RepairRecord a left join t_FaultPosition b on a.PhenomenaId=b.PositionId
                                left join t_User c on a.RepairmanId=c.userID
                                where " + strWhere);
                    sql.Append(@")t group by PositionText order by 2 desc");
                    break;

            }
            //strWhere.AppendFormat(" AND a.RepairStatus in(20,23,4,5)");
            //取得相關查詢條件下的數據列表
            DataTable dt = rrs.ExecSQLTODT(sql);
            if (type != "chart3")
            {
                StringHelper.JsonGZipResponse(context, JsonHelper.DataTableToJSON(dt));
            }
            else
            {
                //格式化数据
                DataView dv = dt.DefaultView;
                DataTable dtPositionText = dv.ToTable(true, new string[] { "PositionText" });
                DataTable dtUserName = dv.ToTable(true, new string[] { "repairmanname" });
                //获取所有故障原因
                List<string> listPosition = dtPositionText.Select().Select(row => row["PositionText"].ToString()).ToList();
                List<string> listUserName = dtUserName.Select().Select(row => row["repairmanname"].ToString()).ToList();
                List<Object> listPositionUserName = new List<object>();
                foreach (string Position in listPosition)
                {
                    List<int> houres = new List<int>();
                    foreach (string userName in listUserName)
                    {
                        houres.Add(dt.Select(string.Format("PositionText='{0}' and repairmanname='{1}'", Position, userName)).Sum(row => Convert.IsDBNull(row["manhoure"]) ? 0 : Convert.ToInt32(row["manhoure"])));
                    }
                    listPositionUserName.Add(new { PositionText = Position, Datas = houres });
                }
                //组织JSON
                var obj = new
                {
                    PositionText = listPosition,
                    UserName = listUserName,
                    Series = listPositionUserName
                };
                StringHelper.JsonGZipResponse(context, new StringBuilder(obj.ObjectToJsonstring()));
            }
        }



        public void TingjiFenxiNew(HttpContext context)
        {
            string startDate = context.Request.Params["startDate"];
            string endDate = context.Request.Params["endDate"];

            string strWhere = " ";

            StringBuilder sbSql = new StringBuilder();

            sbSql.AppendFormat(@"with t as (
            select    (case when a.DeviceId='模具' then 'PM' ELSE
	              (case when a.PositionText ='其它' then a.PhenomenaText else a.PositionText end) 
	            END) PositionText,
      ( case when   a.PositionText ='其它' then 0 else   datediff(minute,a.FaultTime, a.RepairSTime) end) ddsj,
            
		 ( case when  a.PositionText ='其它' then 0 else  	(case when a.RepairETime is null then 0 else datediff(minute,a.RepairSTime,RepairETime) end) end)  wxys,
         ( case when  a.PositionText ='其它' then 0 else      (case when a.QCConfirmTime is null then 0 else datediff(minute,RepairETime, a.QCConfirmTime) end) end) qcqr ,
 
          ( case when   a.PositionText !='其它' then 0 else     (case when a.confirmtime is null then 0 else datediff(minute,a.FaultTime, a.confirmtime) end) end)  gzsj,
 
             CONVERT(varchar(10),RepairSTime,120) dayName,
			 left(a.RepairFormNO,13) RepairFormNO ,d.classtype
            from t_RepairRecord a inner join t_RepairForm b on a.RepairFormNO=b.RepairFormNO
             
inner join t_Repairman d on a.RepairmanId=d.RepairmanId and CONVERT(varchar(10),RepairSTime,120) =d.WorkDate
            where  a.RepairSTime between '{0}' and '{1}' and a.DeviceId!='模房' and a.PhenomenaText !='PM' {2}
 
            )
             select   PositionText,dayName,cast(1.0*SUM(DDSJ)/60 as decimal(18,1)) as ddys,cast(1.0*SUM(wxys)/60 as decimal(18,1)) as wxys,cast(1.0*SUM(qcqr)/60 as decimal(18,1)) as qcys,
             cast(1.0*SUM(gzsj)/60 as decimal(18,1)) as gzsj,
			 (select  ClassType+'：'+ convert(varchar(50),count(*))  from(
select  ( case when convert(varchar(10),e.ConfirmTime,108) <'19:30:00' and convert(varchar(10),e.ConfirmTime,108) <'07:30:00'  then '早班' else '晚班' end) ClassType,e.RepairFormNO 
 from t_RepairRecord e 
			  inner join t_RepairForm F on e.RepairFormNO=F.RepairFormNO
			  
			 where     E.PositionText='转模' and e.ConfirmTime > T.dayName+' 07:30:00' and e.ConfirmTime <DATEADD(DAY,1, T.dayName)+' 07:30:00'
			  )t1
			  group by ClassType for xml path('')) rown ,classtype  from t
             GROUP BY  PositionText,dayName,classtype
             ORDER BY  dayName desc", startDate, endDate, strWhere);

            List<object> head = new List<object>();
            head.Add(new { field = "type", title = "故障位置", width = 100, align = "center" });
            head.Add(new { field = "ys", title = "类别", width = 100, align = "center" });
            DateTime dtStart = Convert.ToDateTime(startDate);
            DateTime dtEnd = Convert.ToDateTime(endDate);
            DateTime dtCur;
            List<string> listdayName1 = new List<string>();
            for (int i = 1; i < 1000; i++)
            {
                dtCur = dtEnd.AddDays(1 - i);
                if (dtCur.Date >= dtStart.Date)
                {
                    head.Add(new { field = "d" + i.ToString(), title = dtCur.ToString("MM-dd"), width = 90, align = "center" });
                    listdayName1.Add(dtCur.ToString("yyyy-MM-dd"));
                }
                else
                {
                    break;
                }
            }



            DataTable dt = rrs.ExecSQLTODT(sbSql);
            DataView dv = dt.DefaultView;
            DataTable dtPositionText = dv.ToTable(true, new string[] { "dayName" });
            List<string> listdayName = dtPositionText.Select().Select(row => row["dayName"].ToString()).ToList();
            string[] aPositionText = { "转模", "修模", "MP6", "MP1", "NP1", "NP2", "NP4", "NP5" };
            List<object> data = new List<object>();

            List<Object> aDay = new List<Object>();
            double ddyssum = 0.0;
            double wxyssum = 0.0;
            double gzsjsum = 0.0;
            double qcyssum = 0.0;
            double sum = 0.0;
             
            List<object> aSum = new List<Object>();

            for (int i = 0; i < aPositionText.Length; i++)
            {
                aDay.Clear();
                foreach (string dayName in listdayName1)
                {
                     
                    DataRow[] drs = dt.Select("PositionText='" + aPositionText[i] + "'" + " and dayName ='" + dayName + "'");
                    if (drs.Length > 0)
                    {  
                         
                        
                        aDay.Add(new { ddys = drs.Sum(x => x.Field<decimal>("ddys")), wxys = drs.Sum(x => x.Field<decimal>("wxys")), qcys = drs.Sum(x => x.Field<decimal>("qcys")), gzsj = drs.Sum(x => x.Field<decimal>("gzsj")), rownum = drs[0]["rown"] });
                    }
                    else
                    {
                        aDay.Add(new { ddys = "0", wxys = "0", qcys = "0", gzsj = "0", rownum = "0" });
                    }
                    
                }

                if (i < 4)
                {
                    Dictionary<string, string> dcWait = new Dictionary<string, string>();
                    dcWait.Add("type", aPositionText[i]);
                    dcWait.Add("ys", "等待用时(h)");
                    Dictionary<string, string> dcWRepair = new Dictionary<string, string>();
                    dcWRepair.Add("type", aPositionText[i]);
                    dcWRepair.Add("ys", "维修用时(h)");
                    Dictionary<string, string> dcQC = new Dictionary<string, string>();
                    dcQC.Add("type", aPositionText[i]);
                    dcQC.Add("ys", "QC用时(h)");
                    Dictionary<string, string> dcNumber = new Dictionary<string, string>();
                    dcNumber.Add("type", aPositionText[i]);
                    dcNumber.Add("ys", "次数");
                    for (int di = 1; di <= listdayName1.Count; di++)
                    {
                        dcWait.Add("d" + di.ToString(), (((dynamic)aDay[di - 1]).ddys).ToString());
                        dcWRepair.Add("d" + di.ToString(), ((dynamic)aDay[di - 1]).wxys.ToString());
                        dcQC.Add("d" + di.ToString(), ((dynamic)aDay[di - 1]).qcys.ToString());
                        dcNumber.Add("d" + di.ToString(), ((dynamic)aDay[di - 1]).rownum.ToString());
                    }
                    data.Add(dcWait);
                    data.Add(dcWRepair);
                    data.Add(dcQC);
                    data.Add(dcNumber);
                }
                else
                {
                    Dictionary<string, string> dcOther = new Dictionary<string, string>();
                    dcOther.Add("type", "其他");
                    dcOther.Add("ys", aPositionText[i]);
                    for (int di = 1; di <= listdayName1.Count; di++)
                    {
                        dcOther.Add("d" + di.ToString(), ((dynamic)aDay[di - 1]).gzsj.ToString());
                    }
                    data.Add(dcOther);
                }
            }
            //白板维修时间
            sum = 0.0;
            aSum.Clear();
            foreach (string dayName in listdayName1)
            {
                ddyssum = dt.Select("dayName ='" + dayName + "' and classtype=1 ").Sum(row => Convert.IsDBNull(row["ddys"]) ? 0 : Convert.ToDouble(row["ddys"]));
                wxyssum = dt.Select("dayName ='" + dayName + "' and classtype=1 ").Sum(row => Convert.IsDBNull(row["wxys"]) ? 0 : Convert.ToDouble(row["wxys"]));
                gzsjsum = dt.Select("dayName ='" + dayName + "' and classtype=1 ").Sum(row => Convert.IsDBNull(row["gzsj"]) ? 0 : Convert.ToDouble(row["gzsj"]));
                qcyssum = dt.Select("dayName ='" + dayName + "' and classtype=1 ").Sum(row => Convert.IsDBNull(row["qcys"]) ? 0 : Convert.ToDouble(row["qcys"]));
                sum = ddyssum + wxyssum + qcyssum + gzsjsum;
                aSum.Add(new { sum = sum.ToString("0.0") });
            }
            Dictionary<string, string> dcTotalbb = new Dictionary<string, string>();
            dcTotalbb.Add("type", "早班总维修时间");
            dcTotalbb.Add("ys", "早班总维修时间");
            for (int di = 1; di <= listdayName1.Count; di++)
            {
                dcTotalbb.Add("d" + di.ToString(), ((dynamic)aSum[di - 1]).sum.ToString());
            }
            data.Add(dcTotalbb);

            //夜班维修时间
            sum = 0.0;
            aSum.Clear();
            foreach (string dayName in listdayName1)
            {
                ddyssum = dt.Select("dayName ='" + dayName + "'  and classtype=0 ").Sum(row => Convert.IsDBNull(row["ddys"]) ? 0 : Convert.ToDouble(row["ddys"]));
                wxyssum = dt.Select("dayName ='" + dayName + "'  and classtype=0 ").Sum(row => Convert.IsDBNull(row["wxys"]) ? 0 : Convert.ToDouble(row["wxys"]));
                gzsjsum = dt.Select("dayName ='" + dayName + "'  and classtype=0 ").Sum(row => Convert.IsDBNull(row["gzsj"]) ? 0 : Convert.ToDouble(row["gzsj"]));
                qcyssum = dt.Select("dayName ='" + dayName + "'  and classtype=0 ").Sum(row => Convert.IsDBNull(row["qcys"]) ? 0 : Convert.ToDouble(row["qcys"]));
                sum = ddyssum + wxyssum + qcyssum + gzsjsum;
                aSum.Add(new { sum = sum.ToString("0.0") });
            }
            Dictionary<string, string> dcTotalyb = new Dictionary<string, string>();
            dcTotalyb.Add("type", "夜班总维修时间");
            dcTotalyb.Add("ys", "夜班总维修时间");
            for (int di = 1; di <= listdayName1.Count; di++)
            {
                dcTotalyb.Add("d" + di.ToString(), ((dynamic)aSum[di - 1]).sum.ToString());
            }
            data.Add(dcTotalyb);
            //总维修时间
            sum = 0.0;
            aSum.Clear();
            foreach (string dayName in listdayName1)
            {
                ddyssum = dt.Select("dayName ='" + dayName + "'").Sum(row => Convert.IsDBNull(row["ddys"]) ? 0 : Convert.ToDouble(row["ddys"]));
                wxyssum = dt.Select("dayName ='" + dayName + "'").Sum(row => Convert.IsDBNull(row["wxys"]) ? 0 : Convert.ToDouble(row["wxys"]));
                gzsjsum = dt.Select("dayName ='" + dayName + "'").Sum(row => Convert.IsDBNull(row["gzsj"]) ? 0 : Convert.ToDouble(row["gzsj"]));
                qcyssum = dt.Select("dayName ='" + dayName + "'").Sum(row => Convert.IsDBNull(row["qcys"]) ? 0 : Convert.ToDouble(row["qcys"]));
                sum = ddyssum + wxyssum + qcyssum + gzsjsum;
                aSum.Add(new { sum = sum.ToString("0.0") });
            }
            Dictionary<string, string> dcTotal = new Dictionary<string, string>();
            dcTotal.Add("type", "总维修时间");
            dcTotal.Add("ys", "总维修时间");
            for (int di = 1; di <= listdayName1.Count; di++)
            {
                dcTotal.Add("d" + di.ToString(), ((dynamic)aSum[di - 1]).sum.ToString());
            }
            data.Add(dcTotal);


            Object allObject = new { Columns = head, Rows = data };
            StringHelper.JsonGZipResponse(context, new StringBuilder(allObject.ObjectToJsonstring()));
        }

        /// <summary>
        /// 停机分析
        /// </summary>
        /// <param name="context"></param>
        public void TingjiFenxi(HttpContext context)
        {
            StringBuilder sbSql = new StringBuilder();

            sbSql.Append(@"with t as (
            select    (case when a.DeviceId='模具' then 'PM' ELSE
	              (case when a.PositionText ='其它' then a.PhenomenaText else a.PositionText end) 
	            END) PositionText,
      ( case when a.DeviceId='模具' or   a.PositionText ='其它' then 0 else   datediff(minute,b.Intime, a.RepairSTime) end) ddsj,
            
		 ( case when a.DeviceId='模具' or   a.PositionText ='其它' then 0 else  	(case when a.QCConfirmTime is null then 0 else datediff(minute,a.FaultTime,RepairETime) end) end)  wxys,
         ( case when a.DeviceId='模具' or   a.PositionText ='其它' then 0 else      (case when a.QCConfirmTime is null then 0 else datediff(minute,RepairETime, a.QCConfirmTime) end) end) qcqr ,
 
          ( case when a.DeviceId!='模具' or   a.PositionText !='其它' then 0 else     (case when a.confirmtime is null then 0 else datediff(minute,RepairETime, a.confirmtime) end) end)  gzsj,
 
             CONVERT(varchar(10),RepairSTime,120) dayName,
			 left(a.RepairFormNO,13) RepairFormNO 
            from t_RepairRecord a inner join t_RepairForm b on a.RepairFormNO=b.RepairFormNO
            inner join t_FaultPosition c on  a.PositionId=c.PPositionId and a.PhenomenaId=c.PositionId 
            where  DateDiff(dd,a.RepairSTime,getdate()) <7
            )
             select   PositionText,dayName,cast(1.0*SUM(DDSJ)/60 as decimal(18,1)) as ddys,cast(1.0*SUM(wxys)/60 as decimal(18,1)) as wxys,cast(1.0*SUM(qcqr)/60 as decimal(18,1)) as qcys,
             cast(1.0*SUM(gzsj)/60 as decimal(18,1)) as gzsj,count(distinct RepairFormNO) rownum  from t
             GROUP BY  PositionText,dayName
             ORDER BY  dayName asc
      
      ");
            DataTable dt = rrs.ExecSQLTODT(sbSql);
            DataView dv = dt.DefaultView;
            DataTable dtPositionText = dv.ToTable(true, new string[] { "dayName" });
            List<string> listdayName = dtPositionText.Select().Select(row => row["dayName"].ToString()).ToList();
            string[] aPositionText = { "转模", "修模", "MP6", "MP1", "NP1", "NP2", "NP4", "NP5", "PM" };
            List<object> data = new List<object>();

            List<Object> aDay = new List<Object>();
            double ddyssum = 0.0;
            double wxyssum = 0.0;
            double gzsjsum = 0.0;
            double qcyssum = 0.0;
            double sum = 0.0;
            List<object> aSum = new List<Object>();

            List<string> listdayName1 = new List<string>();
            for (int i = -6; i < 1; i++)
            {
                listdayName1.Add(DateTime.Now.AddDays(i).ToString("yyyy-MM-dd"));
            }
            for (int i = 0; i < aPositionText.Length; i++)
            {
                aDay.Clear();
                foreach (string dayName in listdayName1)
                {
                    DataRow[] drs = dt.Select("PositionText='" + aPositionText[i] + "'" + " and dayName ='" + dayName + "'");
                    if (drs.Length > 0)
                    {
                        aDay.Add(new { ddys = drs[0][2], wxys = drs[0][3], qcys = drs[0][4], gzsj = drs[0][5], rownum = drs[0][6] });
                    }
                    else
                    {
                        aDay.Add(new { ddys = "0", wxys = "0", qcys = "0", gzsj = "0", rownum = "0" });
                    }


                }

                if (i < 4)
                {

                    data.Add(new
                    {
                        type = aPositionText[i],
                        ys = "等待用时(h)",
                        d1 = ((dynamic)aDay[0]).ddys,
                        d2 = ((dynamic)aDay[1]).ddys,
                        d3 = ((dynamic)aDay[2]).ddys,
                        d4 = ((dynamic)aDay[3]).ddys,
                        d5 = ((dynamic)aDay[4]).ddys,
                        d6 = ((dynamic)aDay[5]).ddys,
                        d7 = ((dynamic)aDay[6]).ddys,
                    });
                    data.Add(new
                    {
                        type = aPositionText[i],
                        ys = "维修用时(h)",
                        d1 = ((dynamic)aDay[0]).wxys,
                        d2 = ((dynamic)aDay[1]).wxys,
                        d3 = ((dynamic)aDay[2]).wxys,
                        d4 = ((dynamic)aDay[3]).wxys,
                        d5 = ((dynamic)aDay[4]).wxys,
                        d6 = ((dynamic)aDay[5]).wxys,
                        d7 = ((dynamic)aDay[6]).wxys,
                    });


                    data.Add(new
                    {
                        type = aPositionText[i],
                        ys = "QC用时(h)",
                        d1 = ((dynamic)aDay[0]).qcys,
                        d2 = ((dynamic)aDay[1]).qcys,
                        d3 = ((dynamic)aDay[2]).qcys,
                        d4 = ((dynamic)aDay[3]).qcys,
                        d5 = ((dynamic)aDay[4]).qcys,
                        d6 = ((dynamic)aDay[5]).qcys,
                        d7 = ((dynamic)aDay[6]).qcys,
                    });
                    data.Add(new
                    {
                        type = aPositionText[i],
                        ys = "次数",
                        d1 = ((dynamic)aDay[0]).rownum,
                        d2 = ((dynamic)aDay[1]).rownum,
                        d3 = ((dynamic)aDay[2]).rownum,
                        d4 = ((dynamic)aDay[3]).rownum,
                        d5 = ((dynamic)aDay[4]).rownum,
                        d6 = ((dynamic)aDay[5]).rownum,
                        d7 = ((dynamic)aDay[6]).rownum,
                    });


                }
                else
                {
                    data.Add(new
                    {
                        type = "其他",
                        ys = aPositionText[i],
                        d1 = ((dynamic)aDay[0]).gzsj,
                        d2 = ((dynamic)aDay[1]).gzsj,
                        d3 = ((dynamic)aDay[2]).gzsj,
                        d4 = ((dynamic)aDay[3]).gzsj,
                        d5 = ((dynamic)aDay[4]).gzsj,
                        d6 = ((dynamic)aDay[5]).gzsj,
                        d7 = ((dynamic)aDay[6]).gzsj,
                    });
                }

            }
            sum = 0.0;
            foreach (string dayName in listdayName1)
            {
                ddyssum = dt.Select("dayName ='" + dayName + "'").Sum(row => Convert.IsDBNull(row["ddys"]) ? 0 : Convert.ToDouble(row["ddys"]));
                wxyssum = dt.Select("dayName ='" + dayName + "'").Sum(row => Convert.IsDBNull(row["wxys"]) ? 0 : Convert.ToDouble(row["wxys"]));
                gzsjsum = dt.Select("dayName ='" + dayName + "'").Sum(row => Convert.IsDBNull(row["gzsj"]) ? 0 : Convert.ToDouble(row["gzsj"]));
                qcyssum = dt.Select("dayName ='" + dayName + "'").Sum(row => Convert.IsDBNull(row["qcys"]) ? 0 : Convert.ToDouble(row["qcys"]));
                sum = ddyssum + wxyssum + qcyssum + gzsjsum;
                aSum.Add(new { sum = sum.ToString("0.0") });
            }
            data.Add(new
            {
                type = "总维修时间",
                ys = "总维修时间",
                d1 = ((dynamic)aSum[0]).sum,
                d2 = ((dynamic)aSum[1]).sum,
                d3 = ((dynamic)aSum[2]).sum,
                d4 = ((dynamic)aSum[3]).sum,
                d5 = ((dynamic)aSum[4]).sum,
                d6 = ((dynamic)aSum[5]).sum,
                d7 = ((dynamic)aSum[6]).sum,
            });

            StringHelper.JsonGZipResponse(context, new StringBuilder(data.ObjectToJsonstring()));
        }
        public void TingjiFenxiNewdownload(HttpContext context)
        {
            string startDate = context.Request.Params["startDate"];
            string endDate = context.Request.Params["endDate"];

            string strWhere = " ";

            StringBuilder sbSql = new StringBuilder();

            sbSql.AppendFormat(@"with t as (
            select    (case when a.DeviceId='模具' then 'PM' ELSE
	              (case when a.PositionText ='其它' then a.PhenomenaText else a.PositionText end) 
	            END) PositionText,
      ( case when   a.PositionText ='其它' then 0 else   datediff(minute,a.FaultTime, a.RepairSTime) end) ddsj,
            
		 ( case when  a.PositionText ='其它' then 0 else  	(case when a.RepairETime is null then 0 else datediff(minute,a.RepairSTime,RepairETime) end) end)  wxys,
         ( case when  a.PositionText ='其它' then 0 else      (case when a.QCConfirmTime is null then 0 else datediff(minute,RepairETime, a.QCConfirmTime) end) end) qcqr ,
 
          ( case when   a.PositionText !='其它' then 0 else     (case when a.confirmtime is null then 0 else datediff(minute,a.FaultTime, a.confirmtime) end) end)  gzsj,
 
             CONVERT(varchar(10),RepairSTime,120) dayName,
			 left(a.RepairFormNO,13) RepairFormNO ,d.classtype
            from t_RepairRecord a inner join t_RepairForm b on a.RepairFormNO=b.RepairFormNO
             
inner join t_Repairman d on a.RepairmanId=d.RepairmanId and CONVERT(varchar(10),RepairSTime,120) =d.WorkDate
            where  a.RepairSTime between '{0}' and '{1}' and a.DeviceId!='模房' and a.PhenomenaText !='PM' {2}
 
            )
               select   PositionText,dayName,cast(1.0*SUM(DDSJ)/60 as decimal(18,1)) as ddys,cast(1.0*SUM(wxys)/60 as decimal(18,1)) as wxys,cast(1.0*SUM(qcqr)/60 as decimal(18,1)) as qcys,
             cast(1.0*SUM(gzsj)/60 as decimal(18,1)) as gzsj,
			 (select  ClassType+'：'+ convert(varchar(50),count(*))  from(
select  ( case when convert(varchar(10),e.ConfirmTime,108) <'19:30:00' and convert(varchar(10),e.ConfirmTime,108) <'07:30:00'  then '早班' else '晚班' end) ClassType,e.RepairFormNO 
 from t_RepairRecord e 
			  inner join t_RepairForm F on e.RepairFormNO=F.RepairFormNO
			  
			 where     E.PositionText='转模' and e.ConfirmTime > T.dayName+' 07:30:00' and e.ConfirmTime <DATEADD(DAY,1, T.dayName)+' 07:30:00'
			  )t1
			  group by ClassType for xml path('')) rown ,classtype  from t
             GROUP BY  PositionText,dayName,classtype
             ORDER BY  dayName desc ", startDate, endDate, strWhere);


            DateTime dtStart = Convert.ToDateTime(startDate);
            DateTime dtEnd = Convert.ToDateTime(endDate);
            DateTime dtCur;
            List<string> listdayName1 = new List<string>();
            for (int i = 1; i < 1000; i++)
            {
                dtCur = dtEnd.AddDays(1 - i);
                if (dtCur.Date >= dtStart.Date)
                {

                    listdayName1.Add(dtCur.ToString("yyyy-MM-dd"));
                }
                else
                {
                    break;
                }
            }



            DataTable dt = rrs.ExecSQLTODT(sbSql);
            DataView dv = dt.DefaultView;
            DataTable dtPositionText = dv.ToTable(true, new string[] { "dayName" });
            List<string> listdayName = dtPositionText.Select().Select(row => row["dayName"].ToString()).ToList();
            string[] aPositionText = { "转模", "修模", "MP6", "MP1", "NP1", "NP2", "NP4", "NP5" };
            List<object> data = new List<object>();


            List<Object> aDay = new List<Object>();
            double ddyssum = 0.0;
            double wxyssum = 0.0;
            double gzsjsum = 0.0;
            double qcyssum = 0.0;
            double sum = 0.0;
            List<object> aSum = new List<Object>();
            DataTable dtSns = new DataTable();
            dtSns.Columns.Add("故障位置");
            dtSns.Columns.Add("时间类型");
            for (int di = 0; di < listdayName1.Count; di++)
            {

                dtSns.Columns.Add(listdayName1[di].Substring(5));
            }


            for (int i = 0; i < aPositionText.Length; i++)
            {
                ArrayList dcWait = new ArrayList();
                ArrayList dcWRepair = new ArrayList();
                ArrayList dcQC = new ArrayList();
                ArrayList dcNumber = new ArrayList();
                ArrayList dcOther = new ArrayList();
                aDay.Clear();

                foreach (string dayName in listdayName1)
                {
                    DataRow[] drs = dt.Select("PositionText='" + aPositionText[i] + "'" + " and dayName ='" + dayName + "'");
                    if (drs.Length > 0)
                    {
                        aDay.Add(new { ddys = drs.Sum(x => x.Field<decimal>("ddys")), wxys = drs.Sum(x => x.Field<decimal>("wxys")), qcys = drs.Sum(x => x.Field<decimal>("qcys")), gzsj = drs.Sum(x => x.Field<decimal>("gzsj")), rownum = drs[0]["rown"] });
                    }
                    else
                    {
                        aDay.Add(new { ddys = "0", wxys = "0", qcys = "0", gzsj = "0", rownum = "0" });
                    }
                }

                if (i < 4)
                {


                    dcWait.Add(aPositionText[i]);
                    dcWait.Add("等待用时(h)");
                    dcWRepair.Add(aPositionText[i]);
                    dcWRepair.Add("维修用时(h)");
                    dcQC.Add(aPositionText[i]);
                    dcQC.Add("QC用时(h)");
                    dcNumber.Add(aPositionText[i]);
                    dcNumber.Add("次数");
                    for (int di = 1; di <= listdayName1.Count; di++)
                    {
                        dcWait.Add((((dynamic)aDay[di - 1]).ddys).ToString());
                        dcWRepair.Add(((dynamic)aDay[di - 1]).wxys.ToString());
                        dcQC.Add(((dynamic)aDay[di - 1]).qcys.ToString());
                        dcNumber.Add(((dynamic)aDay[di - 1]).rownum.ToString());
                    }


                    object[] array = dcWait.ToArray();
                    dtSns.LoadDataRow(array, true);
                    object[] arrayWRepair = dcWRepair.ToArray();
                    dtSns.LoadDataRow(arrayWRepair, true);
                    object[] arrayQC = dcQC.ToArray();
                    dtSns.LoadDataRow(arrayQC, true);
                    object[] arrayDC = dcNumber.ToArray();
                    dtSns.LoadDataRow(arrayDC, true);
                }
                else
                {

                    dcOther.Add("其他");
                    dcOther.Add(aPositionText[i]);
                    for (int di = 1; di <= listdayName1.Count; di++)
                    {
                        dcOther.Add(((dynamic)aDay[di - 1]).gzsj.ToString());
                    }
                    object[] arrayOther = dcOther.ToArray();
                    dtSns.LoadDataRow(arrayOther, true);
                }

            }
            //白班
            sum = 0.0;
            aSum.Clear();
            foreach (string dayName in listdayName1)
            {
                ddyssum = dt.Select("dayName ='" + dayName + "'  and classtype=1 ").Sum(row => Convert.IsDBNull(row["ddys"]) ? 0 : Convert.ToDouble(row["ddys"]));
                wxyssum = dt.Select("dayName ='" + dayName + "'  and classtype=1 ").Sum(row => Convert.IsDBNull(row["wxys"]) ? 0 : Convert.ToDouble(row["wxys"]));
                gzsjsum = dt.Select("dayName ='" + dayName + "'  and classtype=1 ").Sum(row => Convert.IsDBNull(row["gzsj"]) ? 0 : Convert.ToDouble(row["gzsj"]));
                qcyssum = dt.Select("dayName ='" + dayName + "'  and classtype=1 ").Sum(row => Convert.IsDBNull(row["qcys"]) ? 0 : Convert.ToDouble(row["qcys"]));
                sum = ddyssum + wxyssum + qcyssum + gzsjsum;
                aSum.Add(new { sum = sum.ToString("0.0") });
            }

            ArrayList dcTotalbb = new ArrayList();
            dcTotalbb.Add("白班总维修时间");
            dcTotalbb.Add("");
            for (int di = 1; di <= listdayName1.Count; di++)
            {
                dcTotalbb.Add(((dynamic)aSum[di - 1]).sum.ToString());
            }
            object[] arrayTotalbb = dcTotalbb.ToArray();

            dtSns.LoadDataRow(arrayTotalbb, true);
            //夜班
            sum = 0.0;
            aSum.Clear();
            foreach (string dayName in listdayName1)
            {
                ddyssum = dt.Select("dayName ='" + dayName + "'  and classtype=0 ").Sum(row => Convert.IsDBNull(row["ddys"]) ? 0 : Convert.ToDouble(row["ddys"]));
                wxyssum = dt.Select("dayName ='" + dayName + "'  and classtype=0 ").Sum(row => Convert.IsDBNull(row["wxys"]) ? 0 : Convert.ToDouble(row["wxys"]));
                gzsjsum = dt.Select("dayName ='" + dayName + "'  and classtype=0 ").Sum(row => Convert.IsDBNull(row["gzsj"]) ? 0 : Convert.ToDouble(row["gzsj"]));
                qcyssum = dt.Select("dayName ='" + dayName + "'  and classtype=0 ").Sum(row => Convert.IsDBNull(row["qcys"]) ? 0 : Convert.ToDouble(row["qcys"]));
                sum = ddyssum + wxyssum + qcyssum + gzsjsum;
                aSum.Add(new { sum = sum.ToString("0.0") });
            }

            ArrayList dcTotalyb = new ArrayList();
            dcTotalyb.Add("总维修时间");
            dcTotalyb.Add("");
            for (int di = 1; di <= listdayName1.Count; di++)
            {
                dcTotalyb.Add(((dynamic)aSum[di - 1]).sum.ToString());
            }
            object[] arrayTotalye = dcTotalyb.ToArray();

            dtSns.LoadDataRow(arrayTotalye, true);
            //ALLTOTAL
            sum = 0.0;
            aSum.Clear();
            foreach (string dayName in listdayName1)
            {
                ddyssum = dt.Select("dayName ='" + dayName + "'").Sum(row => Convert.IsDBNull(row["ddys"]) ? 0 : Convert.ToDouble(row["ddys"]));
                wxyssum = dt.Select("dayName ='" + dayName + "'").Sum(row => Convert.IsDBNull(row["wxys"]) ? 0 : Convert.ToDouble(row["wxys"]));
                gzsjsum = dt.Select("dayName ='" + dayName + "'").Sum(row => Convert.IsDBNull(row["gzsj"]) ? 0 : Convert.ToDouble(row["gzsj"]));
                qcyssum = dt.Select("dayName ='" + dayName + "'").Sum(row => Convert.IsDBNull(row["qcys"]) ? 0 : Convert.ToDouble(row["qcys"]));
                sum = ddyssum + wxyssum + qcyssum + gzsjsum;
                aSum.Add(new { sum = sum.ToString("0.0") });
            }

            ArrayList dcTotal = new ArrayList();
            dcTotal.Add("总维修时间");
            dcTotal.Add("");
            for (int di = 1; di <= listdayName1.Count; di++)
            {
                dcTotal.Add(((dynamic)aSum[di - 1]).sum.ToString());
            }
            object[] arrayTotal = dcTotal.ToArray();

            dtSns.LoadDataRow(arrayTotal, true);







            Dictionary<String, DataTable> dic = new Dictionary<string, DataTable>();
            dic.Add("维修明细", dtSns);
            byte[] bytes = dataTableToCsv(dtSns);
            string fileName = GetData.GetRequest("fileName");
            context.Response.ContentType = "application/octet-stream";
            context.Response.AddHeader("Content-Disposition", "attachment;  filename=" + HttpUtility.UrlEncode(fileName));
            context.Response.BinaryWrite(bytes);
            context.Response.End();
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