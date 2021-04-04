using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMC.DAL.Script
{
    public interface IB2BParaSetting
    {
        /// <summary>
        /// 新增系統參數
        /// </summary>
        string Add { get; }

        /// <summary>
        /// 修改系統參數
        /// </summary>
        string Edit { get; }

        /// <summary>
        /// 刪除系統參數
        /// </summary>
        string Delete { get; }

        /// <summary>
        /// 驗證系統參數是否存在(如果公司別為ALL，則只判斷關鍵字是否存在，如果公司別不為ALL，則驗證公司+關鍵字是否重複)
        /// </summary>
        string Exists { get; }

        /// <summary>
        ///  驗證出貨invoice參數設置參數是否為Y 2016/09/01)
        /// </summary>
        string VolidPara { get; }
    }
}
