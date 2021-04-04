using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;
namespace DMC.DAL.Script.SqlServer
{
    public class EmailTypeSqlScript:IEmailType
    {
        /// <summary>
        /// �K�[
        /// <summary>
        private string _Addt_sEmailType = " insert into t_sEmailType(EmailTypeName,Pop3Url,Pop3Port,Pop3Ssl,SmtpUrl,SmtpPort,SmtpSsl,SmtpTls,IsAuthentication, [createrID] ,[cDeptID] ,[createTime],Usey) values (@EmailTypeName,@Pop3Url,@Pop3Port,@Pop3Ssl,@SmtpUrl,@SmtpPort,@SmtpSsl,@SmtpTls,@IsAuthentication,@createrID , @cDeptID ,@createTime,@Usey)";
      public   string Addt_sEmailType{ get { return _Addt_sEmailType; } }
        /// <summary>
        /// �ק�
        /// <summary>
      private string _Modt_sEmailType = "update t_sEmailType set Pop3Url=@Pop3Url,Pop3Port=@Pop3Port,Pop3Ssl=@Pop3Ssl,SmtpUrl=@SmtpUrl,SmtpPort=@SmtpPort,SmtpSsl=@SmtpSsl,SmtpTls=@SmtpTls,IsAuthentication=@IsAuthentication,updaterID=@updaterID,uDeptID=@uDeptID,lastModTime= @lastModTime,Usey=@Usey where EmailTypeName=@EmailTypeName ";
      public   string Modt_sEmailType{ get { return _Modt_sEmailType; } }
        /// <summary>
        /// ?��
        /// <summary>
       private string _Delt_sEmailType="delete from t_sEmailType where EmailTypeName=@EmailTypeName";
       public  string Delt_sEmailType{ get { return _Delt_sEmailType; } }
        /// <summary>
        /// ??�O�_�s�b
        /// <summary>
       private string _IsExitt_sEmailType="select count(0) from t_sEmailType where EmailTypeName=@EmailTypeName";
       public  string IsExitt_sEmailType{ get { return _IsExitt_sEmailType; } }
        /// <summary>
        /// ���u�d?��^?�u��
        /// <summary>
       private string _GetListt_sEmailType = "select EmailTypeName,Pop3Url,Pop3Port,Pop3Ssl,SmtpUrl,SmtpPort,SmtpSsl,SmtpTls,IsAuthentication,createrID,cDeptID,updaterID,uDeptID,createTime,lastModTime,Usey FROM t_sEmailType  where 1=1 "; 
       public  string GetListt_sEmailType{ get { return _GetListt_sEmailType; } }
        /// <summary>
        /// ���o�Ҧ�?�ƦC��
        /// <summary>
      private string _GetAllt_sEmailType="p_PageView";
       public  string GetAllt_sEmailType{ get { return _GetAllt_sEmailType; } }
    }
}
