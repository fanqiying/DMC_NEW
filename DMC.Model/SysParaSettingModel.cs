using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMC.Model
{
    public class SysParaSettingModel
    {
        /// <summary>
        /// 自增編號
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 公司編號
        /// </summary>
        public string CompanyId { get; set; }
        /// <summary>
        /// 參數編號
        /// </summary>
        public string ParaKey { get; set; }
        /// <summary>
        /// 參數名稱
        /// </summary>
        public string ParaName { get; set; }
        /// <summary>
        /// 參數內容
        /// </summary>
        public string ParaContent { get; set; }
        /// <summary>
        /// 參數描述
        /// </summary>
        public string ParaDesc { get; set; }
        /// <summary>
        /// 有效否
        /// </summary>
        public string Usey { get; set; }
        /// <summary>
        /// 寫入人員，最後維護的人員
        /// </summary>
        public string InUser { get; set; }
    }
}
