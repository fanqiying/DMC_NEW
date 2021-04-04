using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;
namespace DMC.DAL.Script.SqlServer
{
    public class t_sTaskDetailsSqlScript : ITaskDetails
    {
        /// <summary>
        /// �K�[
        /// <summary>
        private string _Addt_sTaskDetails = " insert into t_sTaskDetails(TaskId,JobId,RunOrder,JobType,RunName,ErrorInfo) values (@TaskId,@JobId,@RunOrder,@JobType,@RunName,@ErrorInfo)";
        public string Addt_sTaskDetails { get { return _Addt_sTaskDetails; } }
        /// <summary>
        /// �ק�
        /// <summary>
        private string _Modt_sTaskDetails = "update t_sTaskDetails set RunOrder=@RunOrder,JobType=@JobType,RunName=@RunName,ErrorInfo=@ErrorInfo  where TaskId=@TaskId and JobId=@JobId ";
        public string Modt_sTaskDetails { get { return _Modt_sTaskDetails; } }
        /// <summary>
        /// ?��
        /// <summary>
        private string _Delt_sTaskDetails = "delete from t_sTaskDetails where AutoId=@AutoId";
        public string Delt_sTaskDetails { get { return _Delt_sTaskDetails; } }
        /// <summary>
        /// ?��
        /// <summary>
        private string _DelList = "delete from t_sTaskDetails where JobId=@JobId";
        public string DelList { get { return _DelList; } }
        /// <summary>
        /// ?��
        /// <summary>
        private string _DelTaskIDList = "delete from t_sTaskDetails where TaskId=@TaskId";
        public string DelTaskIDList { get { return _DelTaskIDList; } }

        /// <summary>
        /// ??�O�_�s�b
        /// <summary>
        private string _IsExitt_sTaskDetails = "select count(0) from t_sTaskDetails where TaskId=@TaskId and JobId=@JobId ";
        public string IsExitt_sTaskDetails { get { return _IsExitt_sTaskDetails; } }
        /// <summary>
        /// ���u�d?��^?�u��
        /// <summary>
        private string _GetListt_sTaskDetails = "select AutoId,TaskId,JobId,RunOrder,JobType,RunName,ErrorInfo FROM t_sTaskDetails  where 1=1 ";
        public string GetListt_sTaskDetails { get { return _GetListt_sTaskDetails; } }
        /// <summary>
        /// ���o�Ҧ�?�ƦC��
        /// <summary>
        private string _GetAllt_sTaskDetails = "p_PageView";
        public string GetAllt_sTaskDetails { get { return _GetAllt_sTaskDetails; } }

        private string _GetJobStepTypes = "select distinct JobType from dbo.t_sTaskDetails where  JobId=@JobId ";
        /// <summary>
        /// ����u�@�ҥ]�t���B�J����
        /// </summary>
        public string GetJobStepTypes
        {
            get { return _GetJobStepTypes; }
        }
    }
}
