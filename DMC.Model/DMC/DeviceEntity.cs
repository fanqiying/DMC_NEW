using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMC.Model
{
    /// <summary>
    /// 设备信息
    /// </summary>
    [Serializable()]
    public class DeviceEntity
    {
        /// <summary>
        /// 自增序号
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 设备编号
        /// </summary>
        public string DeviceId { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }
        /// <summary>
        /// 设备类别编号
        /// </summary>
        public string CategoryId { get; set; }
        /// <summary>
        /// 设备类别全路径(每级以箭头[->]分开)
        /// </summary>
        public string CategoryText { get; set; }
        /// <summary>
        /// 摆放位置
        /// </summary>
        public string Placement { get; set; }
        /// <summary>
        /// 设备备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 保养人
        /// </summary>
        public string KeepUserId { get; set; }
        /// <summary>
        /// 有效否
        /// </summary>
        public string Usey { get; set; }
        /// <summary>
        /// 设备状态：0-停机；1-运行
        /// </summary>
        public string Status { get; set; }
    }
}
