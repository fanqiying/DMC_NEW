using System;
using System.Text;
using MSXML2;
using System.IO;
using System.Data;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.HSSF.Util;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.IO.Compression;

/// <summary>
/// 字符串公共操作类，处理字符串的相关操作
/// </summary>
public static class StringHelper
{
    /// <summary>
    /// JSON压缩传送
    /// </summary>
    /// <param name="context"></param>
    /// <param name="sb"></param>
    public static void JsonGZipResponse(HttpContext context, StringBuilder sb)
    {
        try
        {
            context.Response.Headers.Add("content-encoding", "gzip");
            context.Response.ContentType = "application/json";
            var stream = new GZipStream(context.Response.OutputStream, CompressionMode.Compress);
            byte[] jsonBuffer = UTF8Encoding.UTF8.GetBytes(sb.ToString());
            stream.Write(jsonBuffer, 0, jsonBuffer.Length);
            stream.Close();
        }
        catch {
            context.Response.Write(sb);
        }
    }
    /// <summary>
    /// 解決對象間的複製處理，同時進行關聯拆除，注意:T必須是實現了系列化的，即有[Serializable]
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="socure"></param>
    /// <returns></returns>
    public static List<T> Clone<T>(this List<T> socure)
    {
        MemoryStream ms = new MemoryStream();
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(ms, socure);
        ms.Position = 0;
        List<T> ClonedObject = (List<T>)formatter.Deserialize(ms);
        ms.Close();
        return ClonedObject;
    }

    /// <summary>
    /// 月度對賬報表
    /// </summary>
    /// <param name="dtContent"></param>
    /// <param name="strTemplateFileName"></param>
    /// <returns></returns>
    public static MemoryStream ExprotMonthReport(DataTable dtContent, string strTemplateFileName, Dictionary<int, string> map, int maxlength)
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
            int rowIndex = 2;       // 起始行
            int Len = dtContent.Rows.Count;
            DataColumn column = null;
            for (int i = 0; i < Len; i++)
            {
                DataRow row = dtContent.Rows[i];
                #region 填充内容

                IRow dataRow = sheet.GetRow(rowIndex);
                if (dataRow == null)
                    dataRow = sheet.CreateRow(rowIndex);


                for (int ci = 0; ci < maxlength; ci++)
                {
                    ICell newCell = dataRow.GetCell(ci);
                    if (newCell == null)
                        newCell = dataRow.CreateCell(ci);
                    if (map.ContainsKey(ci))
                    {
                        column = dtContent.Columns[map[ci]];
                        #region 給行賦值處理
                        string drValue = row[column.ColumnName].ToString();
                        switch (column.DataType.ToString())
                        {
                            case "System.String"://字符串类型
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
                        #endregion
                    }
                    newCell.CellStyle = headCellStyle;
                }
                #endregion
                rowIndex++;
            }

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
    /// 字符串转byte數組
    /// </summary>
    /// <param name="hex">字符串</param>
    /// <returns>byte</returns>
    public static byte[] HexStringToBytes(string hex)
    {
        if (hex.Length == 0)
        {
            return new byte[] { 0 };
        }

        if (hex.Length % 2 == 1)
        {
            hex = "0" + hex;
        }

        byte[] result = new byte[hex.Length / 2];

        for (int i = 0; i < hex.Length / 2; i++)
        {
            result[i] = byte.Parse(hex.Substring(2 * i, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
        }

        return result;
    }
    /// <summary>
    /// byte數組轉字符串
    /// </summary>
    /// <param name="input">byte數組對象</param>
    /// <returns>string</returns>
    public static string BytesToHexString(byte[] input)
    {
        StringBuilder hexString = new StringBuilder(64);

        for (int i = 0; i < input.Length; i++)
        {
            hexString.Append(String.Format("{0:X2}", input[i]));
        }
        return hexString.ToString();
    }
    /// <summary>
    /// 將byte數組解析成字符串返回
    /// </summary>
    /// <param name="input">byte數組</param>
    /// <returns>string</returns>
    public static string BytesToDecString(byte[] input)
    {
        StringBuilder decString = new StringBuilder(64);

        for (int i = 0; i < input.Length; i++)
        {
            decString.Append(String.Format(i == 0 ? "{0:D3}" : "-{0:D3}", input[i]));
        }
        return decString.ToString();
    }

    /// <summary>
    /// 將byte轉換成ASCII字符串
    /// </summary>
    /// <param name="input">byte數組</param>
    /// <returns>string</returns>
    public static string ASCIIBytesToString(byte[] input)
    {
        System.Text.ASCIIEncoding enc = new ASCIIEncoding();
        return enc.GetString(input);
    }
    /// <summary>
    /// 將byte轉換成UTF16字符串
    /// </summary>
    /// <param name="input">byte數組</param>
    /// <returns>string</returns>
    public static string UTF16BytesToString(byte[] input)
    {
        System.Text.UnicodeEncoding enc = new UnicodeEncoding();
        return enc.GetString(input);
    }
    /// <summary>
    /// 將byte轉換成UTF8字符串
    /// </summary>
    /// <param name="input">byte數組</param>
    /// <returns>string</returns>
    public static string UTF8BytesToString(byte[] input)
    {
        System.Text.UTF8Encoding enc = new UTF8Encoding();
        return enc.GetString(input);
    }
    /// <summary>
    /// 將byte轉換成GB2312字符串
    /// </summary>
    /// <param name="input">byte數組</param>
    /// <returns>string</returns>
    public static string GB2312BytesToString(byte[] input)
    {
        Encoding GB2312 = Encoding.GetEncoding("GB2312");
        return GB2312.GetString(input);
    }
    /// <summary>
    /// 將byte轉換成Base64位字符串
    /// </summary>
    /// <param name="input">byte數組</param>
    /// <returns>string</returns>
    public static string ToBase64(byte[] input)
    {
        return Convert.ToBase64String(input);
    }
    /// <summary>
    /// base64轉換成byte數組
    /// </summary>
    /// <param name="base64">base64字符串</param>
    /// <returns>byte[]</returns>
    public static byte[] FromBase64(string base64)
    {
        return Convert.FromBase64String(base64);
    }

    /// <summary>
    /// 檢查中間件服務器網絡是否正常
    /// </summary>
    /// <returns>false:連接不上；true：連接OK</returns>
    public static bool GetNetWorkStatus()
    {
        string url = System.Configuration.ConfigurationManager.AppSettings["UniAccessServiceUrl"].ToString();
        string urlSecond = System.Configuration.ConfigurationManager.AppSettings["zjtimeout"].ToString();
        ServerXMLHTTP https = new ServerXMLHTTP();
        try
        {
            int timeout = Convert.ToInt32(urlSecond) * 1000;//設置請求超時時間
            https.setTimeouts(timeout, timeout, timeout, timeout);
            https.open("GET", url, false, null, null);
            https.send(null);
            int iStatus = https.status;
            //如果取得的网页状态不正确,   就是不存在或没权访问  
            if (iStatus == 200)
                return true;
            else
                return false;
        }
        catch
        {
            return false;
        }
    }
}