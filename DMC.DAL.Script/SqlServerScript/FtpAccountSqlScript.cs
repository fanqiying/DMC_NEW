namespace DMC.DAL.Script.SqlServer
{
	public class t_sFtpAccountSqlScript : IFtpAccount {
		/// <summary>
		///  �s�WFTP�b����
		/// <summary>
        private string _Addt_sFtpAccount = " insert into t_sFtpAccount(FtpName,FtpType,FtpAddress,Port,Path,Account,Password , [createrID] ,[cDeptID] ,[createTime],Usey) values (@FtpName,@FtpType,@FtpAddress,@Port,@Path,@Account,@Password,@createrID , @cDeptID ,@createTime,@Usey)";
		public string Addt_sFtpAccount { get { return _Addt_sFtpAccount; } }
		/// <summary>
		/// �ק�FTP�b����
		/// <summary>
        private string _Modt_sFtpAccount = "update t_sFtpAccount set FtpType=@FtpType,FtpAddress=@FtpAddress,Port=@Port,Path=@Path,Account=@Account,Password=@Password, updaterID=@updaterID,uDeptID=@uDeptID,lastModTime= @lastModTime,Usey=@Usey  where FtpName=@FtpName ";
		public string Modt_sFtpAccount { get { return _Modt_sFtpAccount; } }
		/// <summary>
		/// �R��FTP�b����
		/// <summary>
		private string _Delt_sFtpAccount = "delete from t_sFtpAccount where FtpName=@FtpName";
		public string Delt_sFtpAccount { get { return _Delt_sFtpAccount; } }
		/// <summary>
		/// ����FTP�b���ƬO�_�s�b
		/// <summary>
		private string _IsExitt_sFtpAccount = "select count(0) from t_sFtpAccount where FtpName=@FtpName";
		public string IsExitt_sFtpAccount { get { return _IsExitt_sFtpAccount; } }
		/// <summary>
		///  �ھڬd�߱����^FTP�b���Ƶ��G��
		/// <summary>
		private string _GetListt_sFtpAccount = "select AutoId,FtpName,FtpAddress,Port,Path,Account,Password,Usey FROM t_sFtpAccount  where 1=1 ";
		public string GetListt_sFtpAccount { get { return _GetListt_sFtpAccount; } }
		/// <summary>
		/// ����Ҧ�FTP�b���Ƽƾڶ�
		/// <summary>
		private string _GetAllt_sFtpAccount = "p_PageView";
		public string GetAllt_sFtpAccount { get { return _GetAllt_sFtpAccount; } }
	}
}
