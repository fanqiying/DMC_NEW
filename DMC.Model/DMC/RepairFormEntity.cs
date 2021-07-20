using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMC.Model
{
    /// <summary>
    /// 报修信息:报修时填写
    /// </summary>
    [Serializable()]
    public class RepairFormEntity
    {
        /// <summary>
        /// 自增序号
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 报修单
        /// </summary>
        public string RepairFormNO { get; set; }
        /// <summary>
        /// 报修人
        /// </summary>
        public string ApplyUserId { get; set; }
        /// <summary>
        /// 故障时间
        /// </summary>
        public string FaultTime { get; set; }
        /// <summary>
        /// 故障设备编码
        /// </summary>
        public string DeviceId { get; set; }
        /// <summary>
        /// 故障位置
        /// </summary>
        public string PositionId { get; set; }
        /// <summary>
        /// 故障位置名称
        /// </summary>
        public string PositionText { get; set; }
        public string PositionText1 { get; set; }
        /// <summary>
        /// 故障位置1
        /// </summary>
        public string PositionId1 { get; set; }
        /// <summary>
        /// 故障现象
        /// </summary>
        public string PhenomenaId { get; set; }
        /// <summary>
        /// 故障现象名称
        /// </summary>
        public string PhenomenaText { get; set; }
        /// <summary>
        /// 故障现象1
        /// </summary>
        public string PhenomenaText1 { get; set; }
        public string PhenomenaId1 { get; set; }
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
        /// 创建时间
        /// </summary>
        public string Intime { get; set; }
        /// <summary>
        /// 确认时间
        /// </summary>
        public string ConfirmTime { get; set; }
        /// <summary>
        /// 确认人
        /// </summary>
        public string ConfirmUser { get; set; }
        /// <summary>
        /// 单据状态：0-开立(待指派)；1-已指派(待维修)；2-已维修（待确认）；3-已维修（待QC确认）；4-已确认
        /// </summary>
        public string FormStatus { get; set; }
        /// <summary>
        /// 挂单类别:0-拒收,1-做不了,2-交接班
        /// </summary>
        public string RebackType { get; set; }
        /// <summary>
        /// 返修原因
        /// </summary>
        public string RebackReason { get; set; }
        /// <summary>
        /// IPQC号码
        /// </summary>
        public string IPQCNumber { get; set; }
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
