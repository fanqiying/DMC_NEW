using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util; 
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace DMC.BLL.common
{
    public class ExcelHelper
    {
        public static DataTable ReadExcelToTable(Stream p_Stream)
        {
            try
            {
                DataTable dt = new DataTable();
                IWorkbook wk = WorkbookFactory.Create(p_Stream);
                //获取第一个sheet
                ISheet sheet = wk.GetSheetAt(0);
                //获取第一行
                IRow headrow = sheet.GetRow(0);
                //创建列
                for (int i = headrow.FirstCellNum; i < headrow.Cells.Count; i++)
                {
                    DataColumn datacolum = new DataColumn(headrow.GetCell(i).StringCellValue);
                    dt.Columns.Add(datacolum);
                }
                //读取每行,从第二行起
                for (int r = 1; r <= sheet.LastRowNum; r++)
                {
                    bool result = false;
                    DataRow dr = dt.NewRow();
                    //获取当前行
                    IRow row = sheet.GetRow(r);
                    //读取每列
                    for (int j = 0; j < row.Cells.Count; j++)
                    {
                        ICell cell = row.GetCell(j); //一个单元格
                        dr[j] = GetCellValue(cell); //获取单元格的值
                        //全为空则不取
                        if (dr[j].ToString() != "")
                        {
                            result = true;
                        }
                    }
                    if (result == true)
                    {
                        dt.Rows.Add(dr); //把每行追加到DataTable
                    }
                }
                return dt;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// DataTable 导出到Excel 的MemoryStream
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        public static MemoryStream Export(System.Data.DataTable dtSource, string strHeaderText, Dictionary<string, int> colconfig)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
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
                        headStyle.BorderBottom = BorderStyle.Thin;// CellBorderType.THIN;
                        headStyle.BorderLeft = BorderStyle.Thin;//CellBorderType.THIN;
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
        public static MemoryStream Export(System.Data.DataTable dtSource, string strHeaderText, bool needHeader=false)
        {
            IWorkbook workbook = new HSSFWorkbook();
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
                        if (needHeader)
                        {
                            ICellStyle headStyle = workbook.CreateCellStyle();//创建单元格样式
                            headStyle.Alignment = HorizontalAlignment.Center;
                            headStyle.BorderBottom = BorderStyle.Thin;
                            headStyle.BorderLeft = BorderStyle.Thin;
                            headStyle.BorderRight = BorderStyle.Thin;
                            headStyle.BorderTop = BorderStyle.Thin;
                            headStyle.VerticalAlignment = VerticalAlignment.Center;
                            IFont font = workbook.CreateFont();
                            font.FontHeightInPoints = 24;//设置字体大小
                            font.Color = (short)FontColor.Red;
                            //font.Color = (short)Convert.ToInt32(Color.Red);
                            font.FontName = "黑体";
                            headStyle.SetFont(font);
                            int dtRowcount = dtSource.Columns.Count;//获取表格的列数
                            IRow headerRow = sheet.CreateRow(rowIndex);
                            headerRow.HeightInPoints = 45;
                            for (int i = 0; i < dtRowcount; i++)
                            {
                                headerRow.CreateCell(i).SetCellValue(strHeaderText);
                                headerRow.GetCell(i).CellStyle = headStyle;
                                headerRow.GetCell(i).SetCellType(CellType.String);//设置列为字符串型
                                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, dtRowcount - 1));
                            }
                            rowIndex++;
                        }
                    }
                    #endregion

                    #region 列头及样式
                    {
                        IRow headerRow = sheet.CreateRow(rowIndex);
                        //headerRow.Height = 1000;
                        ICellStyle headStyle = workbook.CreateCellStyle();
                        headStyle.FillBackgroundColor = HSSFColor.Yellow.Index;
                        headStyle.WrapText = true;
                        headStyle.VerticalAlignment = VerticalAlignment.Center;
                        headStyle.FillPattern = FillPattern.SolidForeground;
                        headStyle.FillForegroundColor = HSSFColor.Yellow.Index;
                        headStyle.Alignment = HorizontalAlignment.Center;
                        headStyle.BorderBottom = BorderStyle.Thin;
                        headStyle.BorderLeft = BorderStyle.Thin;
                        headStyle.BorderRight = BorderStyle.Thin;
                        headStyle.BorderTop = BorderStyle.Thin;
                        IFont font = workbook.CreateFont();
                        font.FontHeightInPoints = 12;
                        font.Boldweight = (short)FontBoldWeight.Bold;
                        font.FontName = "宋体";
                        headStyle.SetFont(font);
                        foreach (DataColumn column in dtSource.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;
                            headerRow.GetCell(column.Ordinal).SetCellType(CellType.String);//设置列为字符串型
                            //设置列宽
                            sheet.SetColumnWidth(column.Ordinal, 10000);

                        }

                    }
                    #endregion

                    rowIndex++;
                }
                #endregion


                #region 填充内容
                IRow dataRow = sheet.CreateRow(rowIndex);

                ICellStyle dataStyle = workbook.CreateCellStyle();//通用单元格样式
                dataStyle.BorderBottom = BorderStyle.Thin;
                dataStyle.BorderLeft = BorderStyle.Thin;
                dataStyle.VerticalAlignment = VerticalAlignment.Center;
                dataStyle.Alignment = HorizontalAlignment.Center;
                dataStyle.BorderRight = BorderStyle.Thin;
                dataStyle.BorderTop = BorderStyle.Thin;
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

        //对单元格进行判断取值
        private static string GetCellValue(ICell cell)
        {
            if (cell == null)
                return string.Empty;
            switch (cell.CellType)
            {
                case CellType.Blank: //空数据类型 这里类型注意一下，不同版本NPOI大小写可能不一样,有的版本是Blank（首字母大写)
                    return string.Empty;
                case CellType.Boolean: //bool类型
                    return cell.BooleanCellValue.ToString();
                case CellType.Error:
                    return cell.ErrorCellValue.ToString();
                case CellType.Numeric: //数字类型
                    if (HSSFDateUtil.IsCellDateFormatted(cell))//日期类型
                    {
                        return cell.DateCellValue.ToString();
                    }
                    else //其它数字
                    {
                        return cell.NumericCellValue.ToString();
                    }
                case CellType.Unknown: //无法识别类型
                default: //默认类型
                    return cell.ToString();//
                case CellType.String: //string 类型
                    return cell.StringCellValue;
                case CellType.Formula: //带公式类型
                    try
                    {
                        HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(cell.Sheet.Workbook);
                        e.EvaluateInCell(cell);
                        return cell.NumericCellValue.ToString();
                    }
                    catch
                    {
                        return cell.NumericCellValue.ToString();
                    }
            }
        }
    }
}
