using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMC.Model
{
    /// <summary>
    /// 程式操作信息
    /// </summary>
    [Serializable]
    public class ProgramActionInfo
    {
        /// <summary>
        /// 程式編號
        /// </summary>
        public string ProgramId { get; set; }
        /// <summary>
        /// 用戶或角色：1、用戶；2、角色；
        /// </summary>
        public string IsUserOrRose { get; set; }
        /// <summary>
        /// 程式名稱
        /// </summary>
        public string ProgramName { get; set; }
        /// <summary>
        /// 操作編號
        /// </summary>
        public string ActionId { get; set; }
        /// <summary>
        /// 操作名稱
        /// </summary>
        public string ActionName { get; set; }
        /// <summary>
        /// 是否授權
        /// </summary>
        public string IsUse { get; set; }
    }
}
