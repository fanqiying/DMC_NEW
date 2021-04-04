using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;
namespace DMC.DAL.Script.SqlServer
{
    public class JobDetailSqlScript : IJobDetail
    {
        /// <summary>
        /// �K�[
        /// <summary>
        private string _Addt_sJobDetail = " insert into t_sJobDetail(JobId,JobDesc,JobType,IsActivate,MaxActivateNumber,IntervalTime,ErrorToMail,ErrorCcMail,IsError,IsMessgex,MessgexType,MessgexUserName,Usey,RunState,[createrID] ,[cDeptID] ,[createTime]) values (@JobId,@JobDesc,@JobType,@IsActivate,@MaxActivateNumber,@IntervalTime,@ErrorToMail,@ErrorCcMail,@IsError,@IsMessgex,@MessgexType,@MessgexUserName,@Usey,@RunState, @createrID ,@cDeptID ,@createTime)";
        public string Addt_sJobDetail { get { return _Addt_sJobDetail; } }
        /// <summary>
        /// �ק�
        /// <summary>
        private string _Modt_sJobDetail = "update t_sJobDetail set JobDesc=@JobDesc,JobType=@JobType,IsActivate=@IsActivate,MaxActivateNumber=@MaxActivateNumber,IntervalTime=@IntervalTime,ErrorToMail=@ErrorToMail,ErrorCcMail=@ErrorCcMail,IsError=@IsError,IsMessgex=@IsMessgex,MessgexType=@MessgexType,MessgexUserName=@MessgexUserName,Usey=@Usey,updaterID=@updaterID,uDeptID=@uDeptID,lastModTime= @lastModTime where JobId=@JobId ";
        public string Modt_sJobDetail { get { return _Modt_sJobDetail; } }

        private string _RunOrStop = "update t_sJobDetail set RunState=@RunState where JobId=@JobId ";
        /// <summary>
        /// �Ұʩΰ������
        /// </summary>
        public string RunOrStop
        {
            get { return _RunOrStop; }
        }
        /// <summary>
        /// ?��
        /// <summary>
        private string _Delt_sJobDetail = "delete from t_sJobDetail where JobId=@JobId";
        public string Delt_sJobDetail { get { return _Delt_sJobDetail; } }

        /// <summary>
        /// ??�O�_�s�b
        /// <summary>
        private string _IsExitt_sJobDetail = "select count(0) from t_sJobDetail where JobId=@JobId";
        public string IsExitt_sJobDetail { get { return _IsExitt_sJobDetail; } }
        /// <summary>
        /// ���u�d?��^?�u��
        /// <summary>
        private string _GetListt_sJobDetail = "select AutoId,JobId,JobDesc,JobType,IsActivate,MaxActivateNumber,IntervalTime,ErrorToMail,ErrorCcMail,IsError,IsMessgex,MessgexType,MessgexUserName,Usey,RunState,createrID,cDeptID,updaterID,uDeptID,createTime,lastModTime FROM t_sJobDetail  where 1=1 ";
        public string GetListt_sJobDetail { get { return _GetListt_sJobDetail; } }
        /// <summary>
        /// ���o�Ҧ�?�ƦC��
        /// <summary>
        private string _GetAllt_sJobDetail = "p_PageView";
        public string GetAllt_sJobDetail { get { return _GetAllt_sJobDetail; } }
    }
}
