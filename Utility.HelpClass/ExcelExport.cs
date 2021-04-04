using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace Utility.HelpClass
{
    public static class NoMsExportExcel
    {
        /// <summary>
        /// 利用模板，DataTable导出到Excel（单个类别）
        /// </summary>
        /// <param name="dtSource">DataTable</param>
        /// <param name="strFileName">生成的文件路径、名称</param>
        /// <param name="strTemplateFileName">模板的文件路径、名称</param>
        /// <param name="flg">文件标识（1：经营贸易情况/2：生产经营情况/3：项目投资情况/4：房产销售情况/其他：总表）</param>
        /// <param name="titleName">表头名称</param>
        public static void ExportExcelForDtByNPOI(DataTable dtSource, string strFileName, string strTemplateFileName)
        {
            // 利用模板，DataTable导出到Excel（单个类别）
            using (MemoryStream ms = ExprotExcelForDtByNPOI(dtSource, strTemplateFileName))
            {
                byte[] data = ms.ToArray();
                #region 客户端保存
                HttpResponse response = System.Web.HttpContext.Current.Response;
                response.Clear();
                response.Charset = "UTF-8";
                response.ContentType = "application/vnd-excel";//"application/vnd.ms-excel";
                System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename=" + HttpUtility.UrlPathEncode(strFileName)));
                System.Web.HttpContext.Current.Response.BinaryWrite(data);
                System.Web.HttpContext.Current.Response.End();
                #endregion
            }
        }
        /// <summary>
        /// 倉庫報表
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="hisSource"></param>
        /// <param name="strFileName"></param>
        /// <param name="strTemplateFileName"></param>
        public static void ExportExcelForDtByNPOI(DataTable dtSource, DataTable hisSource, string strFileName, string strTemplateFileName)
        {
            List<CompareRows> result = CompareRow(dtSource, hisSource);
            if (result != null && result.Count > 0)
            {
                // 利用模板，DataTable导出到Excel（单个类别）
                using (MemoryStream ms = ExprotExcelForDtByNPOI(result, dtSource, strTemplateFileName))
                {
                    byte[] data = ms.ToArray();
                    #region 客户端保存
                    HttpResponse response = System.Web.HttpContext.Current.Response;
                    response.Clear();
                    response.Charset = "UTF-8";
                    response.ContentType = "application/vnd-excel";//"application/vnd.ms-excel";
                    System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename=" + HttpUtility.UrlPathEncode(strFileName)));
                    System.Web.HttpContext.Current.Response.BinaryWrite(data);
                    System.Web.HttpContext.Current.Response.End();
                    #endregion
                }
            }
        }

        /// <summary>
        /// #006400
        /// </summary>
        /// <param name="mm"></param>
        /// <returns></returns>
        private static IFont SetColor(HSSFWorkbook workbook, string str)
        {
            int[] color = new int[3];
            color[0] = Convert.ToInt16(str.Substring(1, 2), 16);
            color[1] = Convert.ToInt16(str.Substring(3, 2), 16);
            color[2] = Convert.ToInt16(str.Substring(5, 2), 16);
            HSSFPalette palette = workbook.GetCustomPalette();
            palette.SetColorAtIndex(HSSFColor.Black.Index, (byte)color[0], (byte)color[1], (byte)color[2]);
            //将自定义的颜色引入进来 
            IFont font = workbook.CreateFont();
            font.Color = HSSFColor.Black.Index;
            return font;
        }

        /// <summary> 
        /// 对单元格赋值 
        /// </summary> 
        /// <param name="p_ColumnIndex">行号，从0开始</param> 
        /// <param name="p_RowIndex">列号，从0开始</param> 
        /// <param name="p_Value">期望值，从0开始</param> 
        public static void SetValue(ISheet sheet, int p_RowIndex, int p_ColumnIndex, string p_Value)
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
        /// 倉庫報表
        /// </summary>
        /// <param name="result"></param>
        /// <param name="dtContent"></param>
        /// <param name="strTemplateFileName"></param>
        /// <returns></returns>
        private static MemoryStream ExprotExcelForDtByNPOI(List<CompareRows> result, DataTable dtContent, string strTemplateFileName)
        {
            HSSFWorkbook hssfworkbook = null;
            //加載模板
            if (System.IO.File.Exists(strTemplateFileName))
            {
                using (FileStream file = new FileStream(strTemplateFileName, FileMode.Open, FileAccess.Read))
                {
                    hssfworkbook = new HSSFWorkbook(file);
                }
                //紅色 刪除
                IFont red = hssfworkbook.CreateFont();
                red.Color = HSSFColor.Red.Index;
                red.IsStrikeout = true;
                red.FontName = "Calibri";
                red.FontHeightInPoints = 12;
                //綠色 修改
                IFont green = SetColor(hssfworkbook, "#006400");
                green.FontName = "Calibri";
                green.FontHeightInPoints = 12;
                //藍色 新增
                IFont blue = hssfworkbook.CreateFont();
                blue.Color = (short)4;
                blue.FontName = "Calibri";
                blue.FontHeightInPoints = 12;
                //黑色
                IFont black = hssfworkbook.CreateFont();
                black.FontName = "Calibri";
                black.FontHeightInPoints = 12;

                ISheet sheet = hssfworkbook.GetSheetAt(0);
                #region 樣式設定
                //設置顏色
                ICellStyle rowCellStyle = hssfworkbook.CreateCellStyle();
                rowCellStyle.Alignment = HorizontalAlignment.Left;// CellHorizontalAlignment.LEFT;
                rowCellStyle.VerticalAlignment = VerticalAlignment.Center;// CellVerticalAlignment.CENTER;
                //邊框
                rowCellStyle.BorderBottom = BorderStyle.Thin;// CellBorderType.THIN; //下边框
                rowCellStyle.BorderLeft = BorderStyle.Thin;// CellBorderType.THIN;//左边框
                rowCellStyle.BorderTop = BorderStyle.Thin;// CellBorderType.THIN;//上边框
                rowCellStyle.BorderRight = BorderStyle.Thin;// CellBorderType.THIN;//右边框
                rowCellStyle.BottomBorderColor = black.Color;
                rowCellStyle.LeftBorderColor = black.Color;
                rowCellStyle.TopBorderColor = black.Color;
                rowCellStyle.RightBorderColor = black.Color;

                //設置紅顏色
                ICellStyle rowCellStyleRed = hssfworkbook.CreateCellStyle();
                rowCellStyleRed.Alignment = HorizontalAlignment.Left;// CellHorizontalAlignment.LEFT;
                rowCellStyleRed.VerticalAlignment = VerticalAlignment.Center;// CellVerticalAlignment.CENTER;
                //邊框
                rowCellStyleRed.BorderBottom = BorderStyle.Thin;// CellBorderType.THIN; //下边框
                rowCellStyleRed.BorderLeft = BorderStyle.Thin;// CellBorderType.THIN;//左边框
                rowCellStyleRed.BorderTop = BorderStyle.Thin;// CellBorderType.THIN;//上边框
                rowCellStyleRed.BorderRight = BorderStyle.Thin;// CellBorderType.THIN;//右边框
                rowCellStyleRed.BottomBorderColor = black.Color;
                rowCellStyleRed.LeftBorderColor = black.Color;
                rowCellStyleRed.TopBorderColor = black.Color;
                rowCellStyleRed.RightBorderColor = black.Color;
                rowCellStyleRed.SetFont(red);

                //設置紅綠色
                ICellStyle rowCellStyleGreen = hssfworkbook.CreateCellStyle();
                rowCellStyleGreen.Alignment = HorizontalAlignment.Left;// CellHorizontalAlignment.LEFT;
                rowCellStyleGreen.VerticalAlignment = VerticalAlignment.Center;// CellVerticalAlignment.CENTER;
                //邊框
                rowCellStyleGreen.BorderBottom = BorderStyle.Thin;// CellBorderType.THIN; //下边框
                rowCellStyleGreen.BorderLeft = BorderStyle.Thin;// CellBorderType.THIN;//左边框
                rowCellStyleGreen.BorderTop = BorderStyle.Thin;// CellBorderType.THIN;//上边框
                rowCellStyleGreen.BorderRight = BorderStyle.Thin;// CellBorderType.THIN;//右边框
                rowCellStyleGreen.BottomBorderColor = black.Color;
                rowCellStyleGreen.LeftBorderColor = black.Color;
                rowCellStyleGreen.TopBorderColor = black.Color;
                rowCellStyleGreen.RightBorderColor = black.Color;
                rowCellStyleGreen.SetFont(green);

                //設置紅綠色
                ICellStyle rowCellStyleBlue = hssfworkbook.CreateCellStyle();
                rowCellStyleBlue.Alignment = HorizontalAlignment.Left;// CellHorizontalAlignment.LEFT;
                rowCellStyleBlue.VerticalAlignment = VerticalAlignment.Center;// CellVerticalAlignment.CENTER;
                //邊框
                rowCellStyleBlue.BorderBottom = BorderStyle.Thin;// CellBorderType.THIN; //下边框
                rowCellStyleBlue.BorderLeft = BorderStyle.Thin;// CellBorderType.THIN;//左边框
                rowCellStyleBlue.BorderTop = BorderStyle.Thin;// CellBorderType.THIN;//上边框
                rowCellStyleBlue.BorderRight = BorderStyle.Thin;// CellBorderType.THIN;//右边框
                rowCellStyleBlue.BottomBorderColor = black.Color;
                rowCellStyleBlue.LeftBorderColor = black.Color;
                rowCellStyleBlue.TopBorderColor = black.Color;
                rowCellStyleBlue.RightBorderColor = black.Color;
                rowCellStyleBlue.SetFont(blue);
                #endregion

                int rowIndex = 1;       // 起始行
                int Len = result.Count;
                //抬頭的位置
                int noStart = 0;
                int noEnd = 0;
                //發票的合併處理
                int inStart = 0;
                int inEnd = 0;
                //包裝方式的合併
                int pStart = 0;
                int pEnd = 0;
                for (int i = 0; i < Len; i++)
                {
                    DataRow row = result[i].Row;
                    #region 填充内容
                    IRow dataRow = sheet.GetRow(rowIndex);
                    if (dataRow == null)
                        dataRow = sheet.CreateRow(rowIndex);

                    int columnIndex = 0;        // 开始列（0为标题列，从1开始）
                    foreach (DataColumn column in dtContent.Columns)
                    {
                        string status = "";
                        if (columnIndex > 8 || columnIndex == 6)
                        {
                            status = result[i].RowCUD;
                        }
                        else
                        {
                            status = result[i].CUD;
                        }

                        // 列序号赋值
                        if (columnIndex >= dtContent.Columns.Count)
                            break;

                        ICell newCell = dataRow.GetCell(columnIndex);
                        if (newCell == null)
                            newCell = dataRow.CreateCell(columnIndex);

                        #region 給行賦值處理
                        string drValue = row[column.ColumnName].ToString();
                        switch (row[column.ColumnName].GetType().ToString())
                        {
                            case "System.String"://字符串类型
                                newCell.SetCellValue(drValue);
                                break;
                            case "System.DateTime"://日期类型
                                if (column.ColumnName == "SendData")
                                    newCell.SetCellValue(Convert.ToDateTime(drValue).ToString("yyyy-MM-dd"));
                                else
                                    newCell.SetCellValue(Convert.ToDateTime(drValue).ToString("yyyy-MM-dd hh:mm:ss"));
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
                        #endregion

                        switch (status)
                        {
                            case "1":
                                newCell.CellStyle = rowCellStyleBlue;
                                break;
                            case "2":
                                newCell.CellStyle = rowCellStyleGreen;
                                break;
                            case "3":
                                newCell.CellStyle = rowCellStyleRed;
                                break;
                            default:
                                newCell.CellStyle = rowCellStyle;
                                break;
                        }
                        columnIndex++;
                    }
                    #endregion

                    #region 表單合併
                    //合併主表單     
                    if (result[noStart].Row["SingleNumber"].ToString() != result[i].Row["SingleNumber"].ToString())
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            if (j != 6)
                            {
                                //刪除歷史格子
                                for (int deRowStart = noStart + 2; deRowStart <= noEnd + 1; deRowStart++)
                                {
                                    SetValue(sheet, deRowStart, j, "");
                                }
                                sheet.AddMergedRegion(new CellRangeAddress(noStart + 1, noEnd + 1, j, j));
                            }
                        }
                        //合併發票信息
                        noStart = i;
                    }
                    noEnd = i;
                    //合併發票信息                    
                    if ((result[inStart].Row["InvoiceNo"].ToString() != result[i].Row["InvoiceNo"].ToString() ||
                        result[inStart].Row["InvoiceNumber"].ToString() != result[i].Row["InvoiceNumber"].ToString()) ||
                        result[inStart].Row["SingleNumber"].ToString() != result[i].Row["SingleNumber"].ToString())
                    {
                        //刪除歷史格子
                        for (int deRowStart = inStart + 2; deRowStart <= inEnd + 1; deRowStart++)
                        {
                            SetValue(sheet, deRowStart, 6, "");
                        }

                        sheet.AddMergedRegion(new CellRangeAddress(inStart + 1, inEnd + 1, 6, 6));
                        for (int j = 14; j < 28; j++)
                        {
                            //刪除歷史格子
                            for (int deRowStart = inStart + 2; deRowStart <= inEnd + 1; deRowStart++)
                            {
                                SetValue(sheet, deRowStart, j, "");
                            }
                            sheet.AddMergedRegion(new CellRangeAddress(inStart + 1, inEnd + 1, j, j));
                        }
                        inStart = i;
                    }
                    inEnd = i;
                    //合併包裝方式                    
                    //如果發票號碼相同，包裝方式不同，或發票號碼不同，都表示包裝方式切換
                    if (((result[pStart].Row["InvoiceNo"].ToString() == result[i].Row["InvoiceNo"].ToString() ||
                        result[pStart].Row["InvoiceNumber"].ToString() == result[i].Row["InvoiceNumber"].ToString()) &&
                        result[pStart].Row["Packing"].ToString() != result[i].Row["Packing"].ToString()) ||
                        (result[pStart].Row["InvoiceNo"].ToString() != result[i].Row["InvoiceNo"].ToString()
                        || result[pStart].Row["InvoiceNumber"].ToString() != result[i].Row["InvoiceNumber"].ToString()) ||
                        (result[pStart].Row["SingleNumber"].ToString() != result[i].Row["SingleNumber"].ToString()))
                    {
                        for (int j = 9; j < 11; j++)
                        {
                            //刪除歷史格子
                            for (int deRowStart = pStart + 2; deRowStart <= pEnd + 1; deRowStart++)
                            {
                                SetValue(sheet, deRowStart, j, "");
                            }
                            sheet.AddMergedRegion(new CellRangeAddress(pStart + 1, pEnd + 1, j, j));
                        }
                        pStart = i;
                    }
                    pEnd = i;
                    #endregion

                    rowIndex++;
                }

                #region 尾數合併
                if (noStart != noEnd)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (j != 6)
                        {
                            //刪除歷史格子
                            for (int deRowStart = noStart + 2; deRowStart <= noEnd + 1; deRowStart++)
                            {
                                SetValue(sheet, deRowStart, j, "");
                            }
                            sheet.AddMergedRegion(new CellRangeAddress(noStart + 1, noEnd + 1, j, j));
                        }
                    }
                }
                if (inStart != inEnd)
                {
                    //刪除歷史格子
                    for (int deRowStart = inStart + 2; deRowStart <= inEnd + 1; deRowStart++)
                    {
                        SetValue(sheet, deRowStart, 6, "");
                    }
                    sheet.AddMergedRegion(new CellRangeAddress(inStart + 1, inEnd + 1, 6, 6));
                    for (int j = 14; j < 28; j++)
                    {
                        //刪除歷史格子
                        for (int deRowStart = inStart + 2; deRowStart <= inEnd + 1; deRowStart++)
                        {
                            SetValue(sheet, deRowStart, j, "");
                        }
                        sheet.AddMergedRegion(new CellRangeAddress(inStart + 1, inEnd + 1, j, j));
                    }
                }
                if (pStart != pEnd)
                {
                    for (int j = 9; j < 11; j++)
                    {
                        //刪除歷史格子
                        for (int deRowStart = pStart + 2; deRowStart <= pEnd + 1; deRowStart++)
                        {
                            SetValue(sheet, deRowStart, j, "");
                        }
                        sheet.AddMergedRegion(new CellRangeAddress(pStart + 1, pEnd + 1, j, j));
                    }
                }
                #endregion

                using (MemoryStream ms = new MemoryStream())
                {
                    hssfworkbook.Write(ms);
                    ms.Flush();
                    ms.Position = 0;
                    sheet = null;
                    hssfworkbook = null;
                    return ms;
                }
            }

            return null;
        }

        /// <summary>
        /// 行比較結果
        /// </summary>
        public class CompareRows
        {
            public string SingleNumber { get; set; }
            public DataRow Row { get; set; }
            /// <summary>
            /// 1：新增
            /// 2：修改
            /// 3：刪除
            /// </summary>
            public string RowCUD { get; set; }
            public string CUD { get; set; }
        }

        public class custommodel
        {
            public DataRow Row { get; set; }
            //船務單號
            public string singlenumber { get; set; }
            //Invoice編號
            public string invoiceid { get; set; }
            //出通單
            public string outnumber { get; set; }
            //出貨日期
            public string shippingdate { get; set; }
            //出貨類型
            public string shiptype { get; set; }
            //料號
            public string part { get; set; }
            //英文名稱
            public string englishname { get; set; }
            //單位
            public string unit { get; set; }
            //單價
            public decimal radomprice { get; set; }
            //出貨數量     
            public int shipqty { get; set; }
            //國家代碼
            public string statecode { get; set; }
            //箱數
            public int boxnum { get; set; }
            //總毛總
            public decimal totalgross { get; set; }
            //棧板重
            public decimal palletweight { get; set; }
            //總凈重
            public decimal totalweight { get; set; }
            //箱號
            public string boxno { get; set; }
            //總價
            public decimal totalprice { get; set; }
            //庫存
            public string instock { get; set; }
            //欠貨
            public string lesscargo { get; set; }
            //車間別
            public string workshop { get; set; }
            //客戶
            public string customername { get; set; }
            //單位凈重
            public decimal grosspcs { get; set; }
            //單位毛重
            public decimal weightbox { get; set; }
            //整箱數量
            public int qntybox { get; set; }
            //棧板數
            public int stacknum { get; set; }
            //後端業務
            public string businesspeople { get; set; }
            //產銷
            public string username { get; set; }
            //備註
            public string remark { get; set; }
            //公司ID
            public string companyid { get; set; }
            //自動編號
            public int autoid { get; set; }
            //狀態
            public string invoicestatus { get; set; }
            //未用到的
            public int partdateqnty { get; set; }
            //版序
            public string palletNum { get; set; }
        }



        /// <summary>
        /// 比較行信息
        /// </summary>
        /// <param name="dtNew"></param>
        /// <param name="dtOld"></param>
        /// <returns></returns>
        private static List<CompareRows> CompareRow(DataTable dtNew, DataTable dtOld)
        {
            List<CompareRows> result = new List<CompareRows>();
            List<string> pp = new List<string>();
            List<string> add = new List<string>();
            List<string> upd = new List<string>();
            List<string> del = new List<string>();
            #region 結果合併處理
            foreach (DataRow newRow in dtNew.Rows)
            {
                CompareRows c = new CompareRows();
                c.Row = newRow;
                c.SingleNumber = newRow["SingleNumber"].ToString();
                //比較SystemNumber、InvoiceNo、Packing、PartId 判斷數值是否相等
                DataRow[] list = dtOld.Select("SingleNumber='" + newRow["SingleNumber"].ToString() +
                                              "' and InvoiceNo='" + newRow["InvoiceNo"].ToString() +
                                              "' and InvoiceNumber='" + newRow["InvoiceNumber"].ToString() +
                                              "' and Packing='" + newRow["Packing"].ToString() +
                                              "' and PartId='" + newRow["PartId"].ToString() +
                                              "' and PlayBoardType='" + newRow["PlayBoardType"].ToString() + "'");
                if (list != null && list.Length > 0)
                {
                    DataRow oldRow = list[0];
                    c.RowCUD = "0";
                    bool isUpdate = false;
                    for (int i = 0; i < newRow.ItemArray.Length; i++)
                    {
                        if (newRow[i].ToString().Trim() != oldRow[i].ToString().Trim())
                        {
                            if (!upd.Contains(c.SingleNumber))
                            {
                                upd.Add(c.SingleNumber);
                            }
                            isUpdate = true;
                            break;
                        }
                    }
                    if (isUpdate)
                    {
                        c.RowCUD = "2";
                    }
                    else
                    {
                        if (!pp.Contains(c.SingleNumber))
                        {
                            pp.Add(c.SingleNumber);
                        }
                        c.RowCUD = "0";
                    }
                }
                else
                {
                    if (!add.Contains(c.SingleNumber))
                    {
                        add.Add(c.SingleNumber);
                    }
                    c.RowCUD = "1";
                }
                result.Add(c);
            }
            foreach (DataRow oldRow in dtOld.Rows)
            {

                //比較SystemNumber、InvoiceNo、Packing、PartId 判斷數值是否相等
                DataRow[] list = dtNew.Select("SingleNumber='" + oldRow["SingleNumber"].ToString() +
                                              "' and InvoiceNo='" + oldRow["InvoiceNo"].ToString() +
                                              "' and InvoiceNumber='" + oldRow["InvoiceNumber"].ToString() +
                                              "' and Packing='" + oldRow["Packing"].ToString() +
                                              "' and PartId='" + oldRow["PartId"].ToString() +
                                              "' and PlayBoardType='" + oldRow["PlayBoardType"].ToString() + "'");
                if (list == null || list.Length == 0)
                {
                    CompareRows c = new CompareRows();
                    c.Row = oldRow;
                    c.SingleNumber = oldRow["SingleNumber"].ToString();
                    c.RowCUD = "3";
                    if (!del.Contains(c.SingleNumber))
                    {
                        del.Add(c.SingleNumber);
                    }
                    result.Add(c);
                }
            }
            #endregion

            foreach (CompareRows item in result)
            {
                string SingleNumber = item.Row["SingleNumber"].ToString();
                if (add.Contains(SingleNumber) && !upd.Contains(SingleNumber) && !del.Contains(SingleNumber) && !pp.Contains(SingleNumber))
                    item.CUD = "1";
                else if (upd.Contains(SingleNumber) && !add.Contains(SingleNumber) && !del.Contains(SingleNumber) && !pp.Contains(SingleNumber))
                    item.CUD = "2";
                else if (del.Contains(SingleNumber) && !add.Contains(SingleNumber) && !upd.Contains(SingleNumber) && !pp.Contains(SingleNumber))
                    item.CUD = "3";
                else
                    item.CUD = "4";
            }
            result = result.OrderBy(o => o.SingleNumber).ToList();
            return result;
        }
        /// <summary>
        /// 船務報表
        /// </summary>
        /// <param name="dtContent"></param>
        /// <param name="strTemplateFileName"></param>
        /// <returns></returns>
        private static MemoryStream ExprotExcelForDtByNPOI(DataTable dtContent, string strTemplateFileName)
        {
            HSSFWorkbook hssfworkbook = null;
            //加載模板
            if (System.IO.File.Exists(strTemplateFileName))
            {
                using (FileStream file = new FileStream(strTemplateFileName, FileMode.Open, FileAccess.Read))
                {
                    hssfworkbook = new HSSFWorkbook(file);
                }

                IFont font = hssfworkbook.CreateFont();
                font.Color = HSSFColor.Red.Index;


                ISheet sheet = hssfworkbook.GetSheetAt(0);
                #region 樣式定義
                //設置格式
                ICellStyle headCellStyle = hssfworkbook.CreateCellStyle();
                headCellStyle.Alignment = HorizontalAlignment.Left;// CellHorizontalAlignment.LEFT;
                headCellStyle.VerticalAlignment = VerticalAlignment.Center;// CellVerticalAlignment.CENTER;
                //邊框
                headCellStyle.BorderBottom = BorderStyle.Thin;// CellBorderType.THIN; //下边框
                headCellStyle.BorderLeft = BorderStyle.Thin;// CellBorderType.THIN;//左边框
                headCellStyle.BorderTop = BorderStyle.Thin;// CellBorderType.THIN;//上边框
                headCellStyle.BorderRight = BorderStyle.Thin;// CellBorderType.THIN;//右边框
                headCellStyle.BottomBorderColor = HSSFColor.Black.Index;
                headCellStyle.LeftBorderColor = HSSFColor.Black.Index;
                headCellStyle.TopBorderColor = HSSFColor.Black.Index;
                headCellStyle.RightBorderColor = HSSFColor.Black.Index;

                #endregion
                int rowIndex = 1;       // 起始行
                int Len = dtContent.Rows.Count;
                //抬頭的位置
                int noStart = 0;
                int noEnd = 0;
                //發票的合併處理
                int inStart = 0;
                int inEnd = 0;
                //包裝方式的合併
                int pStart = 0;
                int pEnd = 0;
                for (int i = 0; i < Len; i++)
                {
                    DataRow row = dtContent.Rows[i];
                    #region 填充内容

                    IRow dataRow = sheet.GetRow(rowIndex);
                    if (dataRow == null)
                        dataRow = sheet.CreateRow(rowIndex);

                    int columnIndex = 0;        // 开始列（0为标题列，从1开始）
                    foreach (DataColumn column in dtContent.Columns)
                    {
                        // 列序号赋值
                        if (columnIndex >= dtContent.Columns.Count)
                            break;

                        ICell newCell = dataRow.GetCell(columnIndex);
                        if (newCell == null)
                            newCell = dataRow.CreateCell(columnIndex);

                        #region 給行賦值處理
                        string drValue = row[column].ToString();
                        switch (column.DataType.ToString())
                        {
                            case "System.String"://字符串类型
                                newCell.SetCellValue(drValue);
                                break;
                            case "System.DateTime"://日期类型
                                //DateTime dateV;
                                //DateTime.TryParse(drValue, out dateV);
                                if (!Convert.IsDBNull(drValue) && !string.IsNullOrEmpty(drValue.ToString()))
                                {
                                    if (column.ColumnName == "SendData")
                                        newCell.SetCellValue(Convert.ToDateTime(drValue).ToString("yyyy-MM-dd"));
                                    else
                                        newCell.SetCellValue(Convert.ToDateTime(drValue).ToString("yyyy-MM-dd hh:mm:ss"));
                                }
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
                        #endregion

                        newCell.CellStyle = headCellStyle;
                        columnIndex++;
                    }
                    #endregion

                    #region 表單合併
                    //合併主表單
                    if (dtContent.Rows[noStart]["SingleNumber"].ToString() != dtContent.Rows[i]["SingleNumber"].ToString())
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            //刪除歷史格子
                            for (int deRowStart = noStart + 2; deRowStart <= noEnd + 1; deRowStart++)
                            {
                                SetValue(sheet, deRowStart, j, "");
                            }
                            sheet.AddMergedRegion(new CellRangeAddress(noStart + 1, noEnd + 1, j, j));
                        }
                        //合併發票信息
                        noStart = i;
                    }
                    noEnd = i;
                    //合併發票信息                    
                    if ((dtContent.Rows[inStart]["SingleNumber"].ToString() == dtContent.Rows[i]["SingleNumber"].ToString() &&
                        (dtContent.Rows[inStart]["InvoiceNo"].ToString() != dtContent.Rows[i]["InvoiceNo"].ToString() ||
                        dtContent.Rows[inStart]["InvoiceNumber"].ToString() != dtContent.Rows[i]["InvoiceNumber"].ToString()
                        )) ||
                        (dtContent.Rows[inStart]["InvoiceNo"].ToString() != dtContent.Rows[i]["InvoiceNo"].ToString()) ||
                        (dtContent.Rows[inStart]["SingleNumber"].ToString() != dtContent.Rows[i]["SingleNumber"].ToString()))
                    {
                        for (int j = 10; j < 31; j++)
                        {
                            //刪除歷史格子
                            for (int deRowStart = inStart + 2; deRowStart <= inEnd + 1; deRowStart++)
                            {
                                SetValue(sheet, deRowStart, j, "");
                            }
                            sheet.AddMergedRegion(new CellRangeAddress(inStart + 1, inEnd + 1, j, j));
                        }
                        for (int j = 43; j < 59; j++)
                        {
                            //刪除歷史格子
                            for (int deRowStart = inStart + 2; deRowStart <= inEnd + 1; deRowStart++)
                            {
                                SetValue(sheet, deRowStart, j, "");
                            }
                            sheet.AddMergedRegion(new CellRangeAddress(inStart + 1, inEnd + 1, j, j));
                        }
                        inStart = i;
                    }
                    inEnd = i;
                    //合併包裝方式                    
                    //如果發票號碼相同，包裝方式不同，或發票號碼不同，都表示包裝方式切換
                    if (((dtContent.Rows[pStart]["InvoiceNo"].ToString() == dtContent.Rows[i]["InvoiceNo"].ToString() ||
                        dtContent.Rows[pStart]["InvoiceNumber"].ToString() == dtContent.Rows[i]["InvoiceNumber"].ToString()) &&
                        dtContent.Rows[pStart]["Packing"].ToString() != dtContent.Rows[i]["Packing"].ToString()) ||
                        (dtContent.Rows[pStart]["InvoiceNo"].ToString() != dtContent.Rows[i]["InvoiceNo"].ToString() ||
                        dtContent.Rows[pStart]["InvoiceNumber"].ToString() != dtContent.Rows[i]["InvoiceNumber"].ToString()
                        ) ||
                        (dtContent.Rows[pStart]["SingleNumber"].ToString() != dtContent.Rows[i]["SingleNumber"].ToString()))
                    {
                        for (int j = 31; j < 34; j++)
                        {
                            //刪除歷史格子
                            for (int deRowStart = pStart + 2; deRowStart <= pEnd + 1; deRowStart++)
                            {
                                SetValue(sheet, deRowStart, j, "");
                            }
                            sheet.AddMergedRegion(new CellRangeAddress(pStart + 1, pEnd + 1, j, j));
                        }
                        pStart = i;
                    }
                    pEnd = i;
                    #endregion

                    rowIndex++;
                }

                #region 尾數合併
                if (noStart != noEnd)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        //刪除歷史格子
                        for (int deRowStart = noStart + 2; deRowStart <= noEnd + 1; deRowStart++)
                        {
                            SetValue(sheet, deRowStart, j, "");
                        }
                        sheet.AddMergedRegion(new CellRangeAddress(noStart + 1, noEnd + 1, j, j));
                    }
                }
                if (inStart != inEnd)
                {
                    for (int j = 10; j < 31; j++)
                    {
                        //刪除歷史格子
                        for (int deRowStart = inStart + 2; deRowStart <= inEnd + 1; deRowStart++)
                        {
                            SetValue(sheet, deRowStart, j, "");
                        }
                        sheet.AddMergedRegion(new CellRangeAddress(inStart + 1, inEnd + 1, j, j));
                    }
                    for (int j = 43; j < 59; j++)
                    {
                        //刪除歷史格子
                        for (int deRowStart = inStart + 2; deRowStart <= inEnd + 1; deRowStart++)
                        {
                            SetValue(sheet, deRowStart, j, "");
                        }
                        sheet.AddMergedRegion(new CellRangeAddress(inStart + 1, inEnd + 1, j, j));
                    }
                }
                if (pStart != pEnd)
                {
                    for (int j = 31; j < 34; j++)
                    {
                        //刪除歷史格子
                        for (int deRowStart = pStart + 2; deRowStart <= pEnd + 1; deRowStart++)
                        {
                            SetValue(sheet, deRowStart, j, "");
                        }
                        sheet.AddMergedRegion(new CellRangeAddress(pStart + 1, pEnd + 1, j, j));
                    }
                }
                #endregion

                using (MemoryStream ms = new MemoryStream())
                {
                    hssfworkbook.Write(ms);
                    ms.Flush();
                    ms.Position = 0;
                    sheet = null;
                    hssfworkbook = null;
                    return ms;
                }
            }
            return null;
        }
    }
}
