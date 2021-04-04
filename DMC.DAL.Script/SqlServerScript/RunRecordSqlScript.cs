using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;
namespace DMC.DAL.Script.SqlServer
{
    public class RunRecordSqlScript : IRunRecord
    {
        /// <summary>
        /// �K�[
        /// <summary>
        private string _Addt_sRunRecord = " insert into t_sRunRecord(JobId,RunStartTime,RunEndTime,RunResult,Repeateds,LastRunStartTime,LastRunEndTime,ErrCode,ErrMsg) values (@JobId,@RunStartTime,@RunEndTime,@RunResult,@Repeateds,@LastRunStartTime,@LastRunEndTime,@ErrCode,@ErrMsg)";
        public string Addt_sRunRecord { get { return _Addt_sRunRecord; } }
        /// <summary>
        /// �ק�
        /// <summary>
        private string _Modt_sRunRecord = "update t_sRunRecord set JobId=@JobId,RunStartTime=@RunStartTime,RunEndTime=@RunEndTime,RunResult=@RunResult,Repeateds=@Repeateds,LastRunStartTime=@LastRunStartTime,LastRunEndTime=@LastRunEndTime,ErrCode=@ErrCode,ErrMsg=@ErrMsg  where =@ ";
        public string Modt_sRunRecord { get { return _Modt_sRunRecord; } }
        /// <summary>
        /// ?��
        /// <summary>
        private string _Delt_sRunRecord = "delete from t_sRunRecord where =@";
        public string Delt_sRunRecord { get { return _Delt_sRunRecord; } }
        /// <summary>
        /// ??�O�_�s�b
        /// <summary>
        private string _IsExitt_sRunRecord = "select count(0) from t_sRunRecord where =@";
        public string IsExitt_sRunRecord { get { return _IsExitt_sRunRecord; } }
        /// <summary>
        /// ���u�d?��^?�u��
        /// <summary>
        private string _GetListt_sRunRecord = "select JobId,RunStartTime,RunEndTime,RunResult,Repeateds,LastRunStartTime,LastRunEndTime,ErrCode,ErrMsg FROM t_sRunRecord  where 1=1 ";
        public string GetListt_sRunRecord { get { return _GetListt_sRunRecord; } }
        /// <summary>
        /// ���o�Ҧ�?�ƦC��
        /// <summary>
        private string _GetAllt_sRunRecord = "p_PageView";
        public string GetAllt_sRunRecord { get { return _GetAllt_sRunRecord; } }

        private string _AddJobRecord = "insert into t_sRunRecord(JobId,RunStartTime,RunEndTime,RunResult,ErrCode,ErrMsg,Repeateds,BKey)Values(@JobId,@RunStartTime,@RunEndTime,@RunResult,@ErrCode,@ErrMsg,@Repeateds,@BKey)";
        /// <summary>
        /// �K�[�u�@��x
        /// </summary>
        public string AddJobRecord
        {
            get { return _AddJobRecord; }
        }

        private string _UpdateJobRecord = "update t_sRunRecord set RunEndTime=@RunEndTime,RunResult=@RunResult,ErrCode=@ErrCode,ErrMsg=@ErrMsg,Repeateds=@Repeateds where BKey=@BKey ";
        /// <summary>
        /// ��s�u�@��x�����A
        /// </summary>
        public string UpdateJobRecord
        {
            get { return _UpdateJobRecord; }
        }
    }
}
