using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMC.Model
{
    /// <summary>
    /// 维修员
    /// </summary>
    [Serializable()]
    public class RepairmanEntity
    {
        /// <summary>
        /// 自增序号
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 年月
        /// </summary>
        public string YearMonth { get; set; }
        /// <summary>
        /// 维修员编号
        /// </summary>
        public string RepairmanId { get; set; }
        /// <summary>
        /// 维修员姓名
        /// </summary>
        public string RepairmanName { get; set; }
        /// <summary>
        /// 班别
        /// </summary>
        public string ClassType { get; set; }
        /// <summary>
        /// 是否为组长
        /// </summary>
        public string IsLeader { get; set; }
        /// <summary>
        /// 是否正常上班
        /// </summary>
        public string IsWorking { get; set; }
        /// <summary>
        /// 上班时间
        /// </summary>
        public string WorkRangeTime { get; set; }
        /// <summary>
        /// 头像路径
        /// </summary>
        public string PhotoUrl { get; set; }
        /// <summary>
        /// 当天上班时间
        /// </summary>
        public string WorkRangeTimeBegin { get; set; }
        /// <summary>
        /// 当天下班时间
        /// </summary>
        public string WorkRangeTimeEnd { get; set; }
        /// <summary>
        /// 工作日2020-02-08
        /// </summary>
        public string WorkDate { get; set; }
        /// <summary>
        /// 当前日工作时长
        /// </summary>
        public double WorkNum { get; set; }
        public String GroupName { get; set; }
    }
}
