using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMC.DAL.Script.SqlServer {
	/// <summary>
	/// {TableName} 表明
	/// {Condition} 查詢條件 包含WHERE
	/// {KeyField}  關鍵字段，唯一字段
	/// {ShowField} 顯示字段
	/// {OrderField} 排序字段
	/// {PageSize} 分页大小
	/// {HistoryTotal} 歷史最大页碼
	/// </summary>
	public class PageViewSqlScript : IPageView {
		//1、查詢主語句
		// select Count(1) from {Table} {Condition}
		private string _TotalData = "SELECT Count(1) FROM {TableName} {Condition} ";
		public string GetTotalData {
			get {
				return _TotalData;
			}
		}

		//1、查詢主語句
		// SELECT TOP {PageSize} {ShowField} FROM {Table} {Condition} {OrderField}
		//2、分页處理語句
		private string _getData = "SELECT TOP {PageSize} {ShowField} FROM {TableName} {Condition} AND {KeyField} NOT IN (SELECT TOP {HistoryTotal} {KeyField} FROM {TableName} {Condition} {OrderField}) {GroupField} {OrderField}";
		public string GetData {
			get {
				return _getData;
			}
		}

        private string _getDataByIntId = "SELECT {ShowField} FROM {TableName} {Condition} AND {KeyField} between ({HistoryTotal}-1)*{PageSize}+1 and {HistoryTotal}*{PageSize} {GroupField} {OrderField}";
        public string GetDataByIntId {
            get {
                return _getDataByIntId;
            }
        }
	}
}
