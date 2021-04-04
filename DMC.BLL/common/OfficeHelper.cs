using Microsoft.Office.Interop.Excel;
/*******************************************************************
 * 類  名（ClassName）  ：QMS.BLL.common
 * 關鍵字（KeyWord）    ：
 * 描  述（Description）：
 * 版  本（Version）    ：1.0
 * 日  期（Date）       ：2020/12/2 16:00:40
 * 作  者（Author）     ：devin_shu
******************************修改記錄******************************
 * 版本       時間      作者      描述
 * 
********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMC.BLL.common
{
    public class OfficeHelper
    {
        /// <summary>
        /// 将excel文档转换成PDF格式
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="targetPath">目标文件路径</param> 
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static bool ExcelConvertPDF(string sourcePath, string targetPath, XlFixedFormatType targetType)
        {
            bool result;
            object missing = Type.Missing;
            ApplicationClass application = null;
            Workbook workBook = null;
            try
            {
                application = new ApplicationClass();
                object target = targetPath;
                object type = targetType;
                workBook = application.Workbooks.Open(sourcePath, missing, missing, missing, missing, missing,
                        missing, missing, missing, missing, missing, missing, missing, missing, missing);

                workBook.ExportAsFixedFormat(targetType, target, XlFixedFormatQuality.xlQualityStandard, true, false, missing, missing, missing, missing);
                result = true;
            }
            catch
            {
                result = false;
            }
            finally
            {
                if (workBook != null)
                {
                    workBook.Close(true, missing, missing);
                    workBook = null;
                }
                if (application != null)
                {
                    application.Quit();
                    application = null;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return result;
        }
    }
}
