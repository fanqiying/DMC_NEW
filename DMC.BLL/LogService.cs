using System.Collections.Generic;
using DMC.Model;
using DMC.DAL;
using System.Data;

namespace DMC.BLL
{
    /// <summary>
    /// 系統日誌操作類
    /// code by jeven_xiao
    /// 2013-6-9
    /// </summary>
    public class LogService
    {
        private LogDAL logdal = new LogDAL();
        private PageManage pageView = new PageManage();
        /// <summary>
        /// 寫操作日誌
        /// </summary>
        /// <param name="log">日誌實體</param>
        /// <returns>'UB0002' --增加资料失败'SB0016' --系統日誌增加成功</returns>
        public string WriteLog(t_SysLog log)
        {
            if (logdal.WriteLog(log))
                return "SB0016";
            else
                return "UB0002";

        }

        /// <summary>
        /// 根據ID刪除日誌資料數據
        /// </summary>
        /// <param name="idList">ID字符串：1,2,3,4</param>
        /// <returns></returns>
        public string DelSysLog(List<string> idList)
        {
            if (idList.Count < 1 || idList == null)
                return "EB0015";
            if (logdal.DelSysLog(idList) == true)
                return "SB0016";
            else
                return "UB0004";
        }
		/// <summary>
		/// 按條件查詢所有的日誌記錄
		/// <param name="pageSize">每页條數</param>
		/// <param name="pageIndex">页索引</param>
		/// <param name="pageCount">總页數</param>
		/// <param name="total">總記錄數</param>
		/// <param name="Where">查詢條件</param>
		/// <returns>DataTable</returns>

        public DataTable Search(int pageSize, int pageIndex, out int pageCount, out int total, string Where = "")
        {
            return pageView.PageView("t_SysLog", "autoID", pageIndex, pageSize, "*", "autoID ASC", Where, out total, out pageCount);
        }
    }
}
