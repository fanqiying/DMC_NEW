using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMC.Model
{
    /// <summary>
    /// 指派记录：指派时产生一笔记录
    /// </summary>
    [Serializable()]
    public class RepairAssignLogEntity
    {
        /// <summary>
        /// 自增序号
        /// </summary>
        public string AutoId { get; set; }
        /// <summary>
        /// 报修单
        /// </summary>
        public string RepairFormNO { get; set; }
        /// <summary>
        /// 指派人
        /// </summary>
        public string LeaderUserId { get; set; }
        /// <summary>
        /// 指派时间
        /// </summary>
        public string AssignTime { get; set; }
        /// <summary>
        /// 维修员
        /// </summary>
        public string AssignUser { get; set; }
    }
}
