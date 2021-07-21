using DMC.BLL;
using DMC.Model;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
                    case "download":
                        DownLoad(context);
                        break;
                    case "downloadhour":
                        DownloadHour(context);
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

                    if (!string.IsNullOrEmpty(context.Request.Params["YearMonth"]))
                    {
                        strWhere.AppendFormat(" AND CONVERT(varchar(7),a.RepairETime,120)   = N'{0}'", context.Request.Params["YearMonth"]);
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
                    default:
                        text = item["repairstatus"].ToString() + "-维修完成";
                        break;
                }
                dr["状态"] = text;
                dr["设备编号"] = item["deviceid"].ToString();
                dr["故障位置"] = item["positiontext"].ToString();
                dr["故障现象"] = item["phenomenatext"].ToString();
                dr["故障时间"] = item["faulttime"].ToString();
                dr["指派时间"] = Convert.ToDateTime(item["repairstime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                dr["完成时间"] = (Convert.IsDBNull(item["confirmtime"]) ? "" : Convert.ToDateTime(item["confirmtime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                dr["IPQC确认时间"] = item["repairetime"].ToString();// item.lenovo_part_no;//联想料号
                dr["生产确认时间"] = item["confirmtime"].ToString();
                
                dr["IPQC确认"] = item["ipqcnumber"].ToString();// item.lenovo_part_no;//联想料号
                dr["生产确认"] = item["confirmuser"].ToString();// item.lenovo_part_no;//联想料号
               
                dr["模具编号1"] = item["mouldid"].ToString();
                dr["模具编号2"] = item["mouldid1"].ToString();
                dr["新模编号1"] = item["newmouldid"].ToString();                
                dr["新模编号2"] = item["newmouldid1"].ToString();
                dr["返修原因"] = item["rebackreason"].ToString();
                
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