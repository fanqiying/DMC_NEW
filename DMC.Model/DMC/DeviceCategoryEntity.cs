using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMC.Model
{
    [Serializable()]
    public class DeviceCategoryEntity
    {
        /// <summary>
        /// 自增序号
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 设备类别编号
        /// </summary>
        public string CategoryId { get; set; }
        /// <summary>
        /// 设备类别名称
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 设备类别全路径(每级以箭头[->]分开)
        /// </summary>
        public string CategoryText { get; set; }
        /// <summary>
        /// 分类排序序号
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 父级设备类别
        /// </summary>
        public string PCategoryId { get; set; }
        /// <summary>
        /// 设备类别全路径(每级以箭头[->]分开)
        /// </summary>
        public string PCategoryText { get; set; }
        /// <summary>
        /// 有效否
        /// </summary>
        public string Usey { get; set; }
    }
}
