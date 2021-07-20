using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMC.Model
{
    /// <summary>
    /// 维修记录：一单多次维修
    /// </summary>
    [Serializable()]
    public class RepairRecordEntity
    {
        /// <summary>
        /// 自增序号
        /// </summary>
        public Int32 AutoId { get; set; }
        /// <summary>
        /// 报修单
        /// </summary>
        public string RepairFormNO { get; set; }
        /// <summary>
        /// 维修员
        /// </summary>
        public string RepairmanId { get; set; }
        /// <summary>
        /// 设备编号
        /// </summary>
        public string DeviceId { get; set; }
        /// <summary>
        /// 故障时间
        /// </summary>
        public string FaultTime { get; set; }
        /// <summary>
        /// 维修故障位置
        /// </summary>
        public string PositionId { get; set; }
        /// <summary>
        /// 维修故障位置名称
        /// </summary>
        public string PositionText { get; set; }
        /// <summary>
        /// 维修故障现象
        /// </summary>
        public string PhenomenaId { get; set; }
        /// <summary>
        /// 维修故障现象名称
        /// </summary>
        public string PhenomenaText { get; set; }
        /// <summary>
        /// 故障状态
        /// </summary>
        public string FaultStatus { get; set; }
        /// <summary>
        /// 故障编码
        /// </summary>
        public string FaultCode { get; set; }
        /// <summary>
        /// 故障原因
        /// </summary>
        public string FaultReason { get; set; }
        /// <summary>
        /// 故障分析
        /// </summary>
        public string FaultAnalysis { get; set; }
        /// <summary>
        /// 返修原因
        /// </summary>
        public string RebackReason { get; set; }
        /// <summary>
        /// 维修开始时间
        /// </summary>
        public string RepairSTime { get; set; }
        /// <summary>
        /// 维修完成时间
        /// </summary>
        public string RepairETime { get; set; }
        /// <summary>
        /// 维修状态：10 待指派
        /// 12 待指派(挂单)
        /// 14 待指派(IPQC返修)
        /// 15 待指派(组长返修)
        /// 20 待维修
        /// 23 待维修(返修)
        /// 30 待生产员确认
        /// 40 待IPQC确认
        /// 50 待组长确认
        /// 60 已完成
        /// 62 挂单完结
        /// </summary>
        public string RepairStatus { get; set; }
        /// <summary>
        /// 评分人
        /// </summary>
        public string LeaderUserId { get; set; }
        /// <summary>
        /// 评分
        /// </summary>
        public string Grade { get; set; }
        /// <summary>
        /// 评价
        /// </summary>
        public string Appraise { get; set; }
        /// <summary>
        /// 模具编号
        /// </summary>
        public string MouldId { get; set; }
        /// <summary>
        /// 新模编号
        /// </summary>
        public string NewMouldId { get; set; }
        /// <summary>
        /// 模具编号
        /// </summary>
        public string MouldId1 { get; set; }
        /// <summary>
        /// 新模编号
        /// </summary>
        public string NewMouldId1 { get; set; }
    }
}
