namespace DMC.DAL.Script
{
    public interface IFtpAccount
    {
        /// <summary>
		/// 新增FTP帳戶資料
        /// <summary>
       string Addt_sFtpAccount { get; }
        /// <summary>
	   /// 修改FTP帳戶資料
        /// <summary>
       string Modt_sFtpAccount { get; }
        /// <summary>
	   /// 刪除FTP帳戶資料
        /// <summary>
       string Delt_sFtpAccount { get; }
        /// <summary>
	   /// 驗證FTP帳戶資料是否存在
        /// <summary>
       string IsExitt_sFtpAccount { get; }
        /// <summary>
	   /// 根據查詢條件返回FTP帳戶資料結果集
        /// <summary>
       string GetListt_sFtpAccount { get; }
        /// <summary>
	   /// 獲取所有FTP帳戶資料數據集
        /// <summary>
       string GetAllt_sFtpAccount { get; }
    }
}
