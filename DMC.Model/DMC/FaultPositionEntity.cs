using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMC.Model
{
    [Serializable()]
    public class FaultPositionEntity
    {
        /// <summary>
        /// 自增序号
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 故障位置编号
        /// </summary>
        public string PositionId { get; set; }
        /// <summary>
        /// 故障位置名称
        /// </summary>
        public string PositionName { get; set; }
        /// <summary>
        /// 故障位置全路径(每级以箭头[->]分开)
        /// </summary>
        public string PositionText { get; set; }
        /// <summary>
        /// 排序序号
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 父级故障位置
        /// </summary>
        public string PPositionId { get; set; }
        /// <summary>
        /// 故障位置全路径(每级以箭头[->]分开)
        /// </summary>
        public string PPositionText { get; set; }
        /// <summary>
        /// 有效否
        /// </summary>
        public string Usey { get; set; }
        /// <summary>
        /// 评分间隔时间(分钟)
        /// </summary>
        public string GradeTime { get; set; }
        /// <summary>
        /// 评分分数
        /// </summary>
        public string Grade { get; set; }
    }
}
