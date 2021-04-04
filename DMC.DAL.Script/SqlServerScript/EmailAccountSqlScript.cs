using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;
namespace DMC.DAL.Script.SqlServer
{
    public class EmailAccountSqlScript:IEmailAccount
    {
        /// <summary>
        /// �K�[
        /// <summary>
        private string _Addt_sEmailAccount = " insert into t_sEmailAccount(EmailName,Address,Account,Password,EmailTypeInfo,Usey, [createrID] ,[cDeptID] ,[createTime]) values (@EmailName,@Address,@Account,@Password,@EmailTypeInfo,@Usey,@createrID , @cDeptID ,@createTime)";
      public   string Addt_sEmailAccount{ get { return _Addt_sEmailAccount; } }
        /// <summary>
        /// �ק�
        /// <summary>
      private string _Modt_sEmailAccount = "update t_sEmailAccount set Address=@Address,Account=@Account,Password=@Password,EmailTypeInfo=@EmailTypeInfo,Usey=@Usey,updaterID=@updaterID,uDeptID=@uDeptID,lastModTime= @lastModTime  where EmailName=@EmailName ";
      public   string Modt_sEmailAccount{ get { return _Modt_sEmailAccount; } }
        /// <summary>
        /// ?��
        /// <summary>
      private string _Delt_sEmailAccount = "delete from t_sEmailAccount where EmailName=@EmailName";
       public  string Delt_sEmailAccount{ get { return _Delt_sEmailAccount; } }
        /// <summary>
        /// ??�O�_�s�b
        /// <summary>
       private string _IsExitt_sEmailAccount = "select count(0) from t_sEmailAccount where EmailName=@EmailName";
       public  string IsExitt_sEmailAccount{ get { return _IsExitt_sEmailAccount; } }
        /// <summary>
        /// ���u�d?��^?�u��
        /// <summary>
      private string _GetListt_sEmailAccount="select AutoID,EmailName,Address,Account,Password,EmailTypeInfo,Usey,createrID,cDeptID,updaterID,uDeptID,createTime,lastModTime FROM t_sEmailAccount  where 1=1 "; 
       public  string GetListt_sEmailAccount{ get { return _GetListt_sEmailAccount; } }
        /// <summary>
        /// ���o�Ҧ�?�ƦC��
        /// <summary>
      private string _GetAllt_sEmailAccount="p_PageView";
       public  string GetAllt_sEmailAccount{ get { return _GetAllt_sEmailAccount; } }
    }
}
