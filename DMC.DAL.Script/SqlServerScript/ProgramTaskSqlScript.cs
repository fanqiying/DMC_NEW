using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;
namespace DMC.DAL.Script.SqlServer
{
    public class ProgramTaskSqlScript : IProgramTask
    {
        /// <summary>
        /// �K�[
        /// <summary>
        private string _Addt_sProgramTask = " insert into t_sProgramTask(TaskId,JobId,FilePath,Arguments,WorkingDirectory,OpenWindow,IsWaitResult) values (@TaskId,@JobId,@FilePath,@Arguments,@WorkingDirectory,@OpenWindow,@IsWaitResult)";
        public string Addt_sProgramTask { get { return _Addt_sProgramTask; } }
        /// <summary>
        /// �ק�
        /// <summary>
        private string _Modt_sProgramTask = "update t_sProgramTask set FilePath=@FilePath,Arguments=@Arguments,WorkingDirectory=@WorkingDirectory,OpenWindow=@OpenWindow,IsWaitResult=@IsWaitResult  where TaskId=@TaskId  and JobId=@JobId";
        public string Modt_sProgramTask { get { return _Modt_sProgramTask; } }
        /// <summary>
        /// ?��
        /// <summary>
        private string _Delt_sProgramTask = "delete from t_sProgramTask where JobId=@JobId";
        public string Delt_sProgramTask { get { return _Delt_sProgramTask; } }
        /// <summary>
        /// ?��
        /// <summary>
        private string _DelTaskIDList = "delete from t_sProgramTask where TaskId=@TaskId";
        public string DelTaskIDList { get { return _DelTaskIDList; } }
        /// <summary>
        /// ??�O�_�s�b
        /// <summary>
        private string _IsExitt_sProgramTask = "select count(0) from t_sProgramTask where TaskId=@TaskId  and JobId=@JobId";
        public string IsExitt_sProgramTask { get { return _IsExitt_sProgramTask; } }
        /// <summary>
        /// ���u�d?��^?�u��
        /// <summary>
        private string _GetListt_sProgramTask = "select AutoId,TaskId,JobId,FilePath,Arguments,WorkingDirectory,OpenWindow,IsWaitResult FROM t_sProgramTask  where 1=1 ";
        public string GetListt_sProgramTask { get { return _GetListt_sProgramTask; } }
        /// <summary>
        /// ���o�Ҧ�?�ƦC��
        /// <summary>
        private string _GetAllt_sProgramTask = "p_PageView";
        public string GetAllt_sProgramTask { get { return _GetAllt_sProgramTask; } }

        private string _GetProgramSteps = "select A.*,B.Arguments,B.FilePath,B.OpenWindow,B.WorkingDirectory from t_sTaskDetails A left join t_sProgramTask B on A.JobId=B.JobId AND A.TaskId=B.TaskId WHERE A.JobId=@JobId AND JobType='Program' ";
        /// <summary>
        /// ����u�@����������{�ǨB�J
        /// </summary>
        public string GetProgramSteps
        {
            get { return _GetProgramSteps; }
        }
    }
}
