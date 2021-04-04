using System;
using System.Data;
using DMC.Model;
using DMC.DAL;
namespace DMC.BLL
{
    /// <summary>
    /// 系統開關業務邏輯操作類
    /// code by jeven_xiao
    /// 2013-6-9
    /// </summary>
    public class SwitchService
    {
        SwitchDAL sdal = new SwitchDAL();
        /// <summary>
        /// 停止系统
        /// </summary>
        /// <param name="tch">开关实体</param>
        /// <returns>bool</returns>
        public bool StopSys(t_SysSwitch tch)
        {
            return sdal.StopSys(tch);

        }

        /// <summary>
        /// 取停机列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllStopList()
        {
            return sdal.GetAllStopList();
        }

        /// <summary>
        /// 取某家公司数据维护历史记录
        /// </summary>
        /// <param name="companyID">公司別</param>
		/// <returns>DataTable</returns>
        public DataTable GetDataCloseListByID(string companyID)
        {
            return sdal.GetDataCloseListByID(companyID);
        }

        /// <summary>
        /// 是否存在相同停机记录
        /// </summary>
        /// <param name="startTime">開始時間</param>
        /// <param name="endTime">結束時間</param>
        /// <returns></returns>
        public bool IsExitSysStopInfo(DateTime startTime, DateTime endTime)
        {
            if (sdal.IsExitSysStopInfo(startTime, endTime) == true)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 取得有效的系统停机记录
        /// </summary>
        /// <returns></returns>
        public DataTable GetLastStopRecord()
        {

            return sdal.GetLastStopRecord();

        }
		/// <summary>
		/// 驗證是否有關閉系統的權限
		/// </summary>
		/// <param name="compName">公司別</param>
		/// <returns>bool</returns>
        public bool IsDataClose(string compName)
        {
            return sdal.DataCloseRight(compName);
        }
    }
}
