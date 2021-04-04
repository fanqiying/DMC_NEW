/*******************************************************************
 * 類  名（ClassName）  ：QMS.Model
 * 關鍵字（KeyWord）    ：
 * 描  述（Description）：
 * 版  本（Version）    ：1.0
 * 日  期（Date）       ：2020/11/12 15:09:22
 * 作  者（Author）     ：devin_shu
******************************修改記錄******************************
 * 版本       時間      作者      描述
 * 
********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMC.Model
{
    public class OptResultModel
    {
        /// <summary>
        /// 操作結果
        /// </summary>
        public bool optResult { get; set; }
        /// <summary>
        /// 錯誤描述
        /// </summary>
        public string errMsg { get; set; }
    }
}
