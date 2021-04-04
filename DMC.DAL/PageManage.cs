using System;
using System.Data;
using DMC.DAL.Script;
using DMC.Model;

namespace DMC.DAL {
	public class PageManage {
		private IPageView script = ScriptFactory.GetScript<IPageView>();
        /// <summary>
        /// 不分页處理，add by shenglin_yu 2016/02/24
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="KeyField"></param>
        /// <param name="PageCurrent"></param>
        /// <param name="PageSize"></param>
        /// <param name="ShowField"></param>
        /// <param name="OrderField"></param>
        /// <param name="Condition"></param>
        /// <param name="Total"></param>
        /// <param name="PageCount"></param>
        /// <param name="GroupField"></param>
        /// <returns></returns>
        public DataTable PageViewTotal(string TableName, string KeyField, int PageCurrent, int PageSize, string ShowField, string OrderField, string Condition, out int Total, out int PageCount, string GroupField = "")
        {
            Total = 0;
            PageCount = 0;
            string msg = string.Empty;

            if (string.IsNullOrEmpty(TableName))
            {
                throw new Exception("nosettable");
            }
            if (string.IsNullOrEmpty(KeyField))
            {
                throw new Exception("fieldisemplty");
            }
            if (PageCurrent <= 0)
                PageCurrent = 1;
            if (PageSize <= 0)
                PageSize = 10;
            if (string.IsNullOrEmpty(ShowField))
                ShowField = "*";

            if (string.IsNullOrEmpty(OrderField))
            {
                OrderField = " ";
            }
            else
            {
                OrderField = " ORDER BY " + OrderField.Trim();
            }

            if (string.IsNullOrEmpty(GroupField))
            {
                GroupField = " ";
            }
            else
            {
                GroupField = " Group BY " + GroupField.Trim();
            }


            if (string.IsNullOrEmpty(Condition))
            {
                Condition = " WHERE 1=1 ";
            }
            else
            {
                Condition = " WHERE (" + Condition + " ) ";
            }

            //計算總條數及页數
            string strTotal = script.GetTotalData.Replace("{TableName}", TableName).Replace("{Condition}", Condition);

            Total = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(strTotal, null));
            if (Total % PageSize > 0)
            {
                PageCount = (Total / PageSize) + 1;
            }
            else
            {
                PageCount = (Total / PageSize);
            }

            if (PageCurrent > PageCount)
                PageCurrent = 1;

            //設置查詢條件
            string strData = script.GetData.Replace("{TableName}", TableName)
                             .Replace("{PageSize}", PageSize.ToString())
                             .Replace("{ShowField}", ShowField)
                             .Replace("{Condition}", Condition)
                             .Replace("{KeyField}", KeyField)
                             .Replace("{HistoryTotal}", "0")
                             .Replace("{OrderField}", OrderField)
                             .Replace("{GroupField}", GroupField);

            DataTable dt = DBFactory.Helper.ExecuteDataSet(strData, null).Tables[0];
            return dt;
        }

		/// <summary>
		/// 分页存儲過程
		/// </summary>
		/// <param name="TableName">表明</param>
		/// <param name="KeyField">唯一識別字段</param>
		/// <param name="PageCurrent">當前页</param>
		/// <param name="PageSize">每页顯示數</param>
		/// <param name="ShowField">顯示字段</param>
		/// <param name="OrderField">排序字段</param>
		/// <param name="Condition">查詢條件，不需要帶where</param>
		/// <param name="Total">總記錄數</param>
		/// <param name="PageCount">總页數</param>
		/// <returns></returns>
		public DataTable PageView(string TableName, string KeyField, int PageCurrent, int PageSize, string ShowField, string OrderField, string Condition, out int Total, out int PageCount, string GroupField = "") {
			Total = 0;
			PageCount = 0;
			string msg = string.Empty;

			if (string.IsNullOrEmpty(TableName)) {
				throw new Exception("nosettable");
			}
			if (string.IsNullOrEmpty(KeyField)) {
				throw new Exception("fieldisemplty");
			}
			if (PageCurrent <= 0)
				PageCurrent = 1;
			if (PageSize <= 0)
				PageSize = 10;
			if (string.IsNullOrEmpty(ShowField))
				ShowField = "*";

			if (string.IsNullOrEmpty(OrderField)) {
				OrderField = " ";
			}
			else {
				OrderField = " ORDER BY " + OrderField.Trim();
			}

			if (string.IsNullOrEmpty(GroupField)) {
				GroupField = " ";
			}
			else {
				GroupField = " Group BY " + GroupField.Trim();
			}


			if (string.IsNullOrEmpty(Condition)) {
				Condition = " WHERE 1=1 ";
			}
			else {
				Condition = " WHERE (" + Condition + " ) ";
			}

			//計算總條數及页數
			string strTotal = script.GetTotalData.Replace("{TableName}", TableName).Replace("{Condition}", Condition);

			Total = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(strTotal, null));
			if (Total % PageSize > 0) {
				PageCount = (Total / PageSize) + 1;
			}
			else {
				PageCount = (Total / PageSize);
			}

			if (PageCurrent > PageCount)
				PageCurrent = 1;

			//設置查詢條件
			string strData = script.GetData.Replace("{TableName}", TableName)
							 .Replace("{PageSize}", PageSize.ToString())
							 .Replace("{ShowField}", ShowField)
							 .Replace("{Condition}", Condition)
							 .Replace("{KeyField}", KeyField)
							 .Replace("{HistoryTotal}", ((PageCurrent - 1) * PageSize).ToString())
							 .Replace("{OrderField}", OrderField)
							 .Replace("{GroupField}", GroupField);

			DataTable dt = DBFactory.Helper.ExecuteDataSet(strData, null).Tables[0];
			return dt;
		}

        /// <summary>
        /// 重寫分页處理，使用id的大小進行分页判斷處理
        /// </summary>
        /// <returns></returns>
        public DataTable PageViewIsIntID(string TableName, string KeyField, int PageCurrent, int PageSize, string ShowField, string OrderField, string Condition, out int Total, out int PageCount, string GroupField = "")
        {
            Total = 0;
            PageCount = 0;
            string msg = string.Empty;

            if (string.IsNullOrEmpty(TableName))
            {
                throw new Exception("nosettable");
            }
            if (string.IsNullOrEmpty(KeyField))
            {
                throw new Exception("fieldisemplty");
            }
            if (PageCurrent <= 0)
                PageCurrent = 1;
            if (PageSize <= 0)
                PageSize = 10;
            if (string.IsNullOrEmpty(ShowField))
                ShowField = "*";

            if (string.IsNullOrEmpty(OrderField))
            {
                OrderField = " ";
            }
            else
            {
                OrderField = " ORDER BY " + OrderField.Trim();
            }

            if (string.IsNullOrEmpty(GroupField))
            {
                GroupField = " ";
            }
            else
            {
                GroupField = " Group BY " + GroupField.Trim();
            }


            if (string.IsNullOrEmpty(Condition))
            {
                Condition = " WHERE 1=1 ";
            }
            else
            {
                Condition = " WHERE (" + Condition + " ) ";
            }

            //計算總條數及页數
            string strTotal = script.GetTotalData.Replace("{TableName}", TableName).Replace("{Condition}", Condition);
            Total = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(strTotal, null));
            if (Total % PageSize > 0)
            {
                PageCount = (Total / PageSize) + 1;
            }
            else
            {
                PageCount = (Total / PageSize);
            }

            if (PageCurrent > PageCount)
                PageCurrent = 1;

            //設置查詢條件
            string strData = script.GetDataByIntId.Replace("{TableName}", TableName)
                             .Replace("{PageSize}", PageSize.ToString())
                             .Replace("{ShowField}", ShowField)
                             .Replace("{Condition}", Condition)
                             .Replace("{KeyField}", KeyField)
                             .Replace("{HistoryTotal}", PageCurrent.ToString())
                             .Replace("{OrderField}", OrderField)
                             .Replace("{GroupField}", GroupField);
            DataTable dt = DBFactory.Helper.ExecuteDataSet(strData, null).Tables[0];
            return dt;
        }
	}
}
