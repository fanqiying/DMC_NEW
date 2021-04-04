using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using DMC.DAL.Script;
using DMC.Model;
namespace DMC.DAL
{
    /// <summary>
    /// 系統開關數據訪問操作類
    /// code by jeven_xiao
    /// 2013-6-9
    /// </summary>
    public class SwitchDAL
    {
        private ISwitch script = ScriptFactory.GetScript<ISwitch>();

        /// <summary>
        /// 停止系统
        /// </summary>
        /// <param name="tch">开关实体</param>
        /// <returns>bool</returns>
        public bool StopSys(t_SysSwitch tch)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("compName", DbType.String, tch.compName, 20));
            param.Add(DBFactory.Helper.FormatParameter("operateType", DbType.String, tch.operateType, 1));
            param.Add(DBFactory.Helper.FormatParameter("starTime", DbType.DateTime, tch.starTime));
            param.Add(DBFactory.Helper.FormatParameter("endTime", DbType.DateTime, tch.endTime));
            param.Add(DBFactory.Helper.FormatParameter("reasons", DbType.String, tch.reasons, 1000));
            param.Add(DBFactory.Helper.FormatParameter("operaterID", DbType.String, tch.operaterID));
            param.Add(DBFactory.Helper.FormatParameter("operaterName", DbType.String, tch.operaterName));
            param.Add(DBFactory.Helper.FormatParameter("operateDeptID", DbType.String, tch.operateDeptID));
            param.Add(DBFactory.Helper.FormatParameter("operateTime", DbType.DateTime, System.DateTime.Now));

            using (Trans tr = new Trans())
            {
                try
                {
                    if (DBFactory.Helper.ExecuteNonQuery(script.StopSys, param.ToArray()) > 0)
                    {
                        tr.Commit();
                        return true;
                    }
                    else
                        return false;
                }
                catch
                {
                    tr.RollBack();
                    throw;
                }
            }
        }

        /// <summary>
        /// 取停机列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllStopList()
        {
            return DBFactory.Helper.ExecuteDataSet(script.GetStopList, null).Tables[0];
        }


        /// <summary>
        /// 取某家公司数据维护历史记录
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public DataTable GetDataCloseListByID(string companyID)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("companyID", DbType.String, companyID, 20));
            return DBFactory.Helper.ExecuteDataSet(script.GetDataCloseListByID, param.ToArray()).Tables[0];
        }

        /// <summary>
        /// 是否存在相同停机记录
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public bool IsExitSysStopInfo(DateTime startTime, DateTime endTime)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("startTime", DbType.DateTime, startTime));
            param.Add(DBFactory.Helper.FormatParameter("endTime", DbType.DateTime, endTime));
            return DBFactory.Helper.ExecuteScalar(script.IsExitSysStopInfo, param.ToArray()) != null ? true : false;
        }

        /// <summary>
        /// 取得有效的系统停机记录
        /// </summary>
        /// <returns></returns>
        public DataTable GetLastStopRecord()
        {

            return DBFactory.Helper.ExecuteDataSet(script.GetLastStopRecord, null).Tables[0];

        }
		/// <summary>
		/// 驗證是否有關閉系統的權限
		/// </summary>
		/// <param name="compName"></param>
		/// <returns></returns>
        public bool DataCloseRight(string compName)
        {
            try
            {
                List<DbParameter> param = new List<DbParameter>();
                param.Add(DBFactory.Helper.FormatParameter("compName", DbType.String, compName, 50));
                int count = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(script.DataCloseRight, param.ToArray()));
                return count > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
