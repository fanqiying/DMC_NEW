namespace DMC.DAL.Script
{
    public interface IFtpAccount
    {
        /// <summary>
		/// �s�WFTP�b����
        /// <summary>
       string Addt_sFtpAccount { get; }
        /// <summary>
	   /// �ק�FTP�b����
        /// <summary>
       string Modt_sFtpAccount { get; }
        /// <summary>
	   /// �R��FTP�b����
        /// <summary>
       string Delt_sFtpAccount { get; }
        /// <summary>
	   /// ����FTP�b���ƬO�_�s�b
        /// <summary>
       string IsExitt_sFtpAccount { get; }
        /// <summary>
	   /// �ھڬd�߱����^FTP�b���Ƶ��G��
        /// <summary>
       string GetListt_sFtpAccount { get; }
        /// <summary>
	   /// ����Ҧ�FTP�b���Ƽƾڶ�
        /// <summary>
       string GetAllt_sFtpAccount { get; }
    }
}
