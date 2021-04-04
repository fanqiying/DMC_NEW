
namespace DMC.DAL.Script.SqlServer
{
    public class SysSwitchSqlSscript:ISwitch
    {
		/// <summary>
		/// 停止系统操作
		/// </summary>
        private string _stopSys = "insert into t_SysSwitch (compName,operateType,starTime,endTime,reasons,operaterID,"+
            "operaterName,operateDeptID,operateTime) values (@compName,@operateType,@starTime,@endTime,@reasons,@operaterID," +
            "@operaterName,@operateDeptID,@operateTime)";

        private string _getStopList = "select * from t_SysSwitch";
		/// <summary>
		/// 獲取當前公司別下的所有停機數據資料
		/// </summary>
        private string _getDataCloseListByID = "select * from t_SysSwitch  where compName=@companyID";
		/// <summary>
		/// 是否存在停機記錄
		/// </summary>
        private string _isExitSysStopInfo = "select COUNT (*) from t_SysSwitch where starTime =@startTime and endTime =@endTime";
		/// <summary>
		/// 取得有效的系统停机记录
		/// </summary>
        private string _getLastStopRecord = "select  top 1 * from t_SysSwitch  order by switchID desc";
  

        /// <summary>
        /// 停止系统操作
        /// </summary>
        string ISwitch.StopSys
        {
            get { return _stopSys; }
        }

        string ISwitch.GetStopList
        {
            get { return _getStopList; }
        }


        string ISwitch.GetDataCloseListByID
        {
            get { return _getDataCloseListByID; }
        }


        string ISwitch.IsExitSysStopInfo
        {
            get { return _isExitSysStopInfo; }
        }
		/// <summary>
		/// 取得有效的系统停机记录
		/// </summary>
        string ISwitch.GetLastStopRecord
        {
            get { return _getLastStopRecord; }
        }

		/// <summary>
		/// 驗證是否有關閉系統的權限
		/// </summary>
        private string _datacloseright = "select count(1) from t_SysSwitch where  operateType=2 and compName=@compName and  starTime<GETDATE() and  endTime>GETDATE()";

        public string DataCloseRight
        {
            get { return _datacloseright; }
        }
    }
}
