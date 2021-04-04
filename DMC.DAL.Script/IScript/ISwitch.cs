using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMC.DAL.Script
{
    /// <summary>
    /// 系统运行管理
    /// </summary>
    public interface ISwitch
    {
        /// <summary>
        /// 停止系统
        /// </summary>
        string StopSys { get; }


        string GetStopList { get; }

        /// <summary>
        /// 取某个公司的历史关闭记录
        /// </summary>
        string GetDataCloseListByID { get; }

        /// <summary>
        /// 判断是否存在停机记录
        /// </summary>
        string IsExitSysStopInfo { get; }

        /// <summary>
        /// 取到最后设置的停机记录
        /// </summary>
        string GetLastStopRecord { get; }

        string DataCloseRight { get; }
    }
}
