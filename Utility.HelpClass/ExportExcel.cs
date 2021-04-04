using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Data;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.Util;

namespace Utility.HelpClass
{
    /// <summary>
    /// NPOI帮助类
    /// 用NPOI操作EXCEL
    /// </summary>
    public class NPOIHepler
    {
        public static void Export(System.Data.DataTable datasource, string headertext, string filename, Dictionary<string, int> colconfig)
        {
            using (MemoryStream ms = Export(datasource, headertext, colconfig))
            {
                using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
                {
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
            }
        }
        /// <summary>
        /// DataTable 导出到Excel 的MemoryStream
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        public static MemoryStream Export(System.Data.DataTable dtSource, string strHeaderText, Dictionary<string, int> colconfig)
        {
            XSSFWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet(strHeaderText);//创建sheet
            ICellStyle dateStyle = workbook.CreateCellStyle();
            IDataFormat format = workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("@");


            //取得列宽
            int[] arrColWidth = new int[dtSource.Columns.Count];
            foreach (DataColumn item in dtSource.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
            }
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                for (int j = 0; j < dtSource.Columns.Count; j++)
                {
                    int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp;
                    }
                }
            }
            int rowIndex = 0;
            foreach (DataRow row in dtSource.Rows)
            {
                #region 新建表，填充表头，填充列头，样式

                if (rowIndex == 65535 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        sheet = workbook.CreateSheet();
                    }

                    #region 表头及样式
                    {

                        ICellStyle headStyle = workbook.CreateCellStyle();//创建单元格样式
                        headStyle.Alignment = HorizontalAlignment.Center;// CellHorizontalAlignment.CENTER;
                        headStyle.BorderBottom = BorderStyle.Thin;//
                        headStyle.BorderLeft = BorderStyle.Thin;// CellBorderType.THIN;
                        headStyle.BorderRight = BorderStyle.Thin;// CellBorderType.THIN;
                        headStyle.BorderTop = BorderStyle.Thin;//CellBorderType.THIN;
                        headStyle.VerticalAlignment = VerticalAlignment.Center;// CellVerticalAlignment.CENTER;
                        IFont font = workbook.CreateFont();
                        font.FontHeightInPoints = 24;//设置字体大小
                        font.Color = HSSFColor.Red.Index;
                        font.FontName = "黑体";
                        headStyle.SetFont(font);
                        int dtRowcount = dtSource.Columns.Count;//获取表格的列数
                        IRow headerRow = sheet.CreateRow(0);
                        headerRow.HeightInPoints = 45;
                        for (int i = 0; i < dtRowcount; i++)
                        {
                            headerRow.CreateCell(i).SetCellValue(strHeaderText);
                            headerRow.GetCell(i).CellStyle = headStyle;
                            headerRow.GetCell(i).SetCellType(CellType.String);//设置列为字符串型
                            if (i % 10 == 0)
                            {
                                ////if (Math.Abs(i + 10 - dtRowcount) >= 10)
                                ////{
                                //    sheet.AddMergedRegion(new Region(0, i.Equals(0) ? i : i + 1, 0, i + 10));//合并单元格
                                //}
                                //else
                                //{
                                sheet.AddMergedRegion(new CellRangeAddress(0, 0, i.Equals(0) ? i : i + 1, dtRowcount - 1 < i + 1 ? dtRowcount : dtRowcount - 1));
                                //}
                            }
                        }

                    }
                    #endregion


                    #region 列头及样式
                    {
                        IRow headerRow = sheet.CreateRow(1);
                        //headerRow.Height = 1000;
                        ICellStyle headStyle = workbook.CreateCellStyle();
                        headStyle.FillBackgroundColor = HSSFColor.Yellow.Index;
                        headStyle.WrapText = true;
                        headStyle.VerticalAlignment = VerticalAlignment.Center;// CellVerticalAlignment.CENTER;
                        headStyle.FillPattern = FillPattern.SolidForeground;// CellFillPattern.SOLID_FOREGROUND;
                        headStyle.FillForegroundColor = HSSFColor.Yellow.Index;
                        headStyle.Alignment = HorizontalAlignment.Center;// CellHorizontalAlignment.CENTER;
                        headStyle.BorderBottom = BorderStyle.Thin;// CellBorderType.THIN;
                        headStyle.BorderLeft = BorderStyle.Thin;// CellBorderType.THIN;
                        headStyle.BorderRight = BorderStyle.Thin;// CellBorderType.THIN;
                        headStyle.BorderTop = BorderStyle.Thin;//CellBorderType.THIN;
                        IFont font = workbook.CreateFont();
                        font.FontHeightInPoints = 12;
                        font.Boldweight = (short)FontBoldWeight.Bold; //HSSFFont.BOLDWEIGHT_BOLD;
                        font.FontName = "宋体";
                        headStyle.SetFont(font);
                        foreach (DataColumn column in dtSource.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;
                            headerRow.GetCell(column.Ordinal).SetCellType(CellType.String);//设置列为字符串型
                            //设置列宽
                            if (colconfig.ContainsKey(column.ColumnName))
                            {
                                sheet.SetColumnWidth(column.Ordinal, colconfig[column.ColumnName]);
                            }
                            else
                            {
                                sheet.SetColumnWidth(column.Ordinal, 10 * 256);
                            }

                        }

                    }
                    #endregion

                    rowIndex = 2;
                }
                #endregion

                #region 填充内容
                IRow dataRow = sheet.CreateRow(rowIndex);

                ICellStyle dataStyle = workbook.CreateCellStyle();//通用单元格样式
                dataStyle.BorderBottom = BorderStyle.Thin;// CellBorderType.THIN;
                dataStyle.BorderLeft = BorderStyle.Thin;// CellBorderType.THIN;
                dataStyle.VerticalAlignment = VerticalAlignment.Center;// CellVerticalAlignment.CENTER;
                dataStyle.Alignment = HorizontalAlignment.Center;// CellHorizontalAlignment.CENTER;
                dataStyle.BorderRight = BorderStyle.Thin;//CellBorderType.THIN;
                dataStyle.BorderTop = BorderStyle.Thin;//CellBorderType.THIN;
                dataStyle.WrapText = true;
                dataStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");
                IFont datafont = workbook.CreateFont();
                datafont.FontHeightInPoints = 12;
                datafont.FontName = "宋体";
                dataStyle.SetFont(datafont);
                foreach (DataColumn column in dtSource.Columns)
                {
                    ICell newCell = dataRow.CreateCell(column.Ordinal);
                    newCell.CellStyle = dataStyle;//设置单元格格式
                    newCell.SetCellType(CellType.String);
                    string drValue = row[column].ToString().Replace(",", ",\n");//填充的值
                    switch (column.DataType.ToString())
                    {
                        case "System.String"://字符串类型
                        case "System.DateTime"://日期类型                            
                            newCell.SetCellValue(drValue);
                            break;
                        case "System.Boolean"://布尔型
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16"://整型
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal"://浮点型
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull"://空值处理
                            newCell.SetCellValue("");
                            break;
                        default:
                            newCell.SetCellValue("");
                            break;
                    }

                }
                #endregion

                rowIndex++;
            }

            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                return ms;
            }
        }

        /// <summary>
        /// 用于 Web 导出
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        /// <param name="strFileName">文件名</param>
        public static void ExportByWeb(System.Data.DataTable dtSource, string strHeaderText, string strFileName, string ReportCode)
        {
            HttpContext curContext = HttpContext.Current;

            // 设置编码和附件格式
            curContext.Response.ContentType = "application/vnd.ms-excel";
            curContext.Response.ContentEncoding = Encoding.UTF8;
            curContext.Response.Charset = "";
            curContext.Response.AppendHeader("Content-Disposition",
            "attachment;filename=" + HttpUtility.UrlEncode(strFileName, Encoding.UTF8));

            curContext.Response.BinaryWrite(Export(dtSource, strHeaderText, GetColConfig(ReportCode)).GetBuffer());
            curContext.Response.End();
        }
        /// <summary>
        /// 获取报表的列宽配置
        /// </summary>
        /// <param name="ReportCode">报表代码</param>
        /// <returns></returns>
        private static Dictionary<string, int> GetColConfig(string ReportCode)
        {
            DataSet ds = new DataSet();
            ds.ReadXml(System.Web.HttpContext.Current.Server.MapPath("~/NPOI/ExportColConfig.xml"));
            DataTable coldt = ds.Tables[ReportCode];//获取对应报表的列配置
            Dictionary<string, int> colConfig = new Dictionary<string, int>();
            foreach (DataRow row in coldt.Rows)
            {
                try
                {
                    colConfig.Add(row["colname"].ToString(), Convert.ToInt32(row["colwidth"].ToString()));
                }
                catch { continue; }
            }
            ds.Dispose();
            return colConfig;
        }
        /// <summary>
        /// DatatTable根据xml导出Excle
        /// </summary>
        /// <param name="dtSource">数据源</param>
        /// <param name="strHeaderText">表头</param>
        /// <param name="strFileName">文件名</param>
        /// <param name="ReportCode">xml值</param>
        public static void CreatDataTableXML(System.Data.DataTable dtSource, string strHeaderText, string strFileName, string ReportCode)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(System.Web.HttpContext.Current.Server.MapPath("~/NPOI/ExportColConfig.xml"));
                for (int k = 0; k < dtSource.Columns.Count; k++)
                {
                    for (int i = 0; i < ds.Tables[ReportCode].Rows.Count; i++)
                    {
                        if (dtSource.Columns[k].ColumnName == ds.Tables[ReportCode].Rows[i]["colcode"].ToString())
                        {
                            dtSource.Columns[k].ColumnName = ds.Tables[ReportCode].Rows[i]["colname"].ToString();
                        }
                    }
                }
                ds.Dispose();
                ExportByWeb(dtSource, strHeaderText, strFileName, ReportCode);
            }
            catch
            { }
        }

        /// <summary>
        /// DatatTable根据xml导出Excle
        /// </summary>
        /// <param name="dtSource">数据源</param>
        /// <param name="strHeaderText">表头</param>
        /// <param name="strFileName">文件名</param>
        /// <param name="ReportCode">xml值</param>
        public static void CreatDataTableXMLForFirst(System.Data.DataTable dtSource, string strHeaderText, string strFileName, string ReportCode)
        {
            try
            {
                ExportByWebForFirst(dtSource, strHeaderText, strFileName, ReportCode);
            }
            catch
            { }
        }
        /// <summary>
        /// 用于 Web 导出
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        /// <param name="strFileName">文件名</param>
        public static void ExportByWebForFirst(System.Data.DataTable dtSource, string strHeaderText, string strFileName, string ReportCode)
        {
            HttpContext curContext = HttpContext.Current;

            // 设置编码和附件格式
            curContext.Response.ContentType = "application/vnd.ms-excel";
            curContext.Response.ContentEncoding = Encoding.UTF8;
            curContext.Response.Charset = "";
            curContext.Response.AppendHeader("Content-Disposition",
            "attachment;filename=" + HttpUtility.UrlEncode(strFileName, Encoding.UTF8));

            curContext.Response.BinaryWrite(ExportForFirst(dtSource, strHeaderText, GetColConfig(ReportCode), ReportCode).GetBuffer());
            curContext.Response.End();
        }
        /// <summary>
        /// DataTable 导出到Excel 的MemoryStream
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        public static MemoryStream ExportForFirst(System.Data.DataTable dtSource, string strHeaderText, Dictionary<string, int> colconfig, string ReportCode)
        {
            IWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet(strHeaderText);//创建sheet
            ICellStyle dateStyle = workbook.CreateCellStyle();
            IDataFormat format = workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("@");
            //datatable轉成list，方便合併單元格
            List<NoMsExportExcel.custommodel> list = ConvertHelper<NoMsExportExcel.custommodel>.ConvertToList(dtSource);
            //抬頭的位置
            int noStart = 0;
            int noEnd = 0;
            //for (int i = 0; i <list.Count; i++)
            //{
            //    //合併棧板重     
            //    noEnd = i;
            //    //版序一樣，invoice一樣，才可以合併
            //    if (list[noStart].palletNum == list[i].palletNum && list[noStart].invoiceid == list[i].invoiceid)
            //    {
            //        sheet.AddMergedRegion(new Region(noStart + 2, 13, noEnd + 2, 13));
            //    }
            //    else
            //    {
            //        noStart = i;                
            //    }
            //}
            //轉成中文列名
            DataSet ds = new DataSet();
            ds.ReadXml(System.Web.HttpContext.Current.Server.MapPath("~/NPOI/ExportColConfig.xml"));
            for (int k = 0; k < dtSource.Columns.Count; k++)
            {
                for (int k1 = 0; k1 < ds.Tables[ReportCode].Rows.Count; k1++)
                {
                    if (dtSource.Columns[k].ColumnName == ds.Tables[ReportCode].Rows[k1]["colcode"].ToString())
                    {
                        dtSource.Columns[k].ColumnName = ds.Tables[ReportCode].Rows[k1]["colname"].ToString();
                    }
                }
            }
            ds.Dispose();
            //取得列宽
            int[] arrColWidth = new int[dtSource.Columns.Count];
            foreach (DataColumn item in dtSource.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
            }
            for (int i1 = 0; i1 < dtSource.Rows.Count; i1++)
            {
                for (int j = 0; j < dtSource.Columns.Count; j++)
                {
                    int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i1][j].ToString()).Length;
                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp;
                    }
                }
            }
            int rowIndex = 0;
            foreach (DataRow row in dtSource.Rows)
            {
                #region 新建表，填充表头，填充列头，样式

                if (rowIndex == 65535 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        sheet = workbook.CreateSheet();
                    }

                    #region 表头及样式
                    {

                        ICellStyle headStyle = workbook.CreateCellStyle();//创建单元格样式
                        headStyle.Alignment = HorizontalAlignment.Center;// CellHorizontalAlignment.CENTER;
                        headStyle.BorderBottom = BorderStyle.Thin;// CellBorderType.THIN;
                        headStyle.BorderLeft = BorderStyle.Thin;//CellBorderType.THIN;
                        headStyle.BorderRight = BorderStyle.Thin;// CellBorderType.THIN;
                        headStyle.BorderTop = BorderStyle.Thin;// CellBorderType.THIN;
                        headStyle.VerticalAlignment = VerticalAlignment.Center;// CellVerticalAlignment.CENTER;
                        IFont font = workbook.CreateFont();
                        font.FontHeightInPoints = 24;//设置字体大小
                        font.Color = HSSFColor.Red.Index;
                        font.FontName = "黑体";
                        headStyle.SetFont(font);
                        int dtRowcount = dtSource.Columns.Count;//获取表格的列数
                        IRow headerRow = sheet.CreateRow(0);
                        headerRow.HeightInPoints = 45;
                        for (int i = 0; i < dtRowcount; i++)
                        {
                            headerRow.CreateCell(i).SetCellValue(strHeaderText);
                            headerRow.GetCell(i).CellStyle = headStyle;
                            headerRow.GetCell(i).SetCellType(CellType.String);//设置列为字符串型
                            if (i % 10 == 0)
                            {
                                ////if (Math.Abs(i + 10 - dtRowcount) >= 10)
                                ////{
                                //    sheet.AddMergedRegion(new Region(0, i.Equals(0) ? i : i + 1, 0, i + 10));//合并单元格
                                //}
                                //else
                                //{
                                sheet.AddMergedRegion(new CellRangeAddress(0, 0, i.Equals(0) ? i : i + 1, dtRowcount - 1 < i + 1 ? dtRowcount : dtRowcount - 1));
                                //}
                            }
                        }

                    }
                    #endregion

                    #region 列头及样式
                    {
                        IRow headerRow = sheet.CreateRow(1);
                        //headerRow.Height = 1000;
                        ICellStyle headStyle = workbook.CreateCellStyle();
                        headStyle.FillBackgroundColor = HSSFColor.Yellow.Index;
                        headStyle.WrapText = true;
                        headStyle.VerticalAlignment = VerticalAlignment.Center;// CellVerticalAlignment.CENTER;
                        headStyle.FillPattern = FillPattern.SolidForeground;// CellFillPattern.SOLID_FOREGROUND;
                        headStyle.FillForegroundColor = HSSFColor.Yellow.Index;
                        headStyle.Alignment = HorizontalAlignment.Center;// CellHorizontalAlignment.CENTER;
                        headStyle.BorderBottom = BorderStyle.Thin;// CellBorderType.THIN;
                        headStyle.BorderLeft = BorderStyle.Thin;//CellBorderType.THIN;
                        headStyle.BorderRight = BorderStyle.Thin;// CellBorderType.THIN;
                        headStyle.BorderTop = BorderStyle.Thin;//CellBorderType.THIN;
                        IFont font = workbook.CreateFont();
                        font.FontHeightInPoints = 12;
                        font.Boldweight = (short)FontBoldWeight.Bold; //HSSFFont.BOLDWEIGHT_BOLD;
                        font.FontName = "宋体";
                        headStyle.SetFont(font);
                        foreach (DataColumn column in dtSource.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;
                            headerRow.GetCell(column.Ordinal).SetCellType(CellType.String);//设置列为字符串型
                            //设置列宽
                            if (colconfig.ContainsKey(column.ColumnName))
                            {
                                sheet.SetColumnWidth(column.Ordinal, colconfig[column.ColumnName]);
                            }
                            else
                            {
                                sheet.SetColumnWidth(column.Ordinal, 10 * 256);
                            }

                        }

                    }
                    #endregion

                    rowIndex = 2;
                }
                #endregion

                #region 填充内容
                IRow dataRow = sheet.CreateRow(rowIndex);

                ICellStyle dataStyle = workbook.CreateCellStyle();//通用单元格样式
                dataStyle.BorderBottom = BorderStyle.Thin;// CellBorderType.THIN;
                dataStyle.BorderLeft = BorderStyle.Thin;// CellBorderType.THIN;
                dataStyle.VerticalAlignment = VerticalAlignment.Center;// CellVerticalAlignment.CENTER;
                dataStyle.Alignment = HorizontalAlignment.Center;// CellHorizontalAlignment.CENTER;
                dataStyle.BorderRight = BorderStyle.Thin;// CellBorderType.THIN;
                dataStyle.BorderTop = BorderStyle.Thin;// CellBorderType.THIN;
                dataStyle.WrapText = true;
                dataStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");
                IFont datafont = workbook.CreateFont();
                datafont.FontHeightInPoints = 12;
                datafont.FontName = "宋体";
                dataStyle.SetFont(datafont);
                foreach (DataColumn column in dtSource.Columns)
                {
                    ICell newCell = dataRow.CreateCell(column.Ordinal);
                    newCell.CellStyle = dataStyle;//设置单元格格式
                    newCell.SetCellType(CellType.String);
                    string drValue = row[column].ToString().Replace(",", ",\n");//填充的值
                    switch (column.DataType.ToString())
                    {
                        case "System.String"://字符串类型
                        case "System.DateTime"://日期类型                            
                            newCell.SetCellValue(drValue);
                            break;
                        case "System.Boolean"://布尔型
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16"://整型
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal"://浮点型
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull"://空值处理
                            newCell.SetCellValue("");
                            break;
                        default:
                            newCell.SetCellValue("");
                            break;
                    }

                }
                #endregion
                rowIndex++;
            }
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                return ms;
            }
        }
        /// <summary> 
        /// 对单元格赋值 
        /// </summary> 
        /// <param name="p_ColumnIndex">行号，从0开始</param> 
        /// <param name="p_RowIndex">列号，从0开始</param> 
        /// <param name="p_Value">期望值，从0开始</param> 
        public static void SetValue(HSSFSheet sheet, int p_RowIndex, int p_ColumnIndex, string p_Value)
        {
            try
            {
                sheet.GetRow(p_RowIndex).GetCell(p_ColumnIndex).SetCellValue(p_Value);
            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        public static DataTable ExcelToDataTable(string fileName, string sheetName, bool isFirstRowColumn)
        {
            IWorkbook workbook = null;
            ISheet sheet = null;
            DataTable data = new DataTable();
            int st, startRow = 0;
            FileStream fs = null;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                    workbook = new XSSFWorkbook(fs);
                else if (fileName.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook(fs);

                if (!string.IsNullOrEmpty(sheetName))
                {
                    sheet = workbook.GetSheet(sheetName);
                }

                if (sheet == null)
                {
                    sheet = workbook.GetSheetAt(0);
                }

                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = cell.StringCellValue;
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }

                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        data.Rows.Add(dataRow);
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }
    }
}

