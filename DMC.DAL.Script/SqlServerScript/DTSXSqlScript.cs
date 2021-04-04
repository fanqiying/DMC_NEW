namespace DMC.DAL.Script.SqlServer
{
    public class t_sDTSXSqlScript : IDTSX
    {
        /// <summary>
        /// �K�[
        /// <summary>
        private string _Addt_sDTSX = " insert into t_sDTSX(TaskId,JobId,DTSXPath) values (@TaskId,@JobId,@DTSXPath)";
        public string Addt_sDTSX { get { return _Addt_sDTSX; } }
        /// <summary>
        /// �ק�
        /// <summary>
        private string _Modt_sDTSX = "update t_sDTSX set DTSXPath=@DTSXPath  where TaskId=@TaskId  and JobId=@JobId";
        public string Modt_sDTSX { get { return _Modt_sDTSX; } }
        /// <summary>
        /// ?��
        /// <summary>
        private string _Delt_sDTSX = "delete from t_sDTSX where JobId=@JobId";
        public string Delt_sDTSX { get { return _Delt_sDTSX; } }
        /// <summary>
        /// ?��
        /// <summary>
        private string _DelTaskIDList = "delete from t_sDTSX where TaskId=@TaskId";
        public string DelTaskIDList { get { return _DelTaskIDList; } }
        /// <summary>
        /// ??�O�_�s�b
        /// <summary>
        private string _IsExitt_sDTSX = "select count(0) from t_sDTSX where TaskId=@TaskId  and JobId=@JobId";
        public string IsExitt_sDTSX { get { return _IsExitt_sDTSX; } }
        /// <summary>
        /// ���u�d?��^?�u��
        /// <summary>
        private string _GetListt_sDTSX = "select AutoId,TaskId,JobId,DTSXPath FROM t_sDTSX  where 1=1 ";
        public string GetListt_sDTSX { get { return _GetListt_sDTSX; } }
        /// <summary>
        /// ���o�Ҧ�?�ƦC��
        /// <summary>
        private string _GetAllt_sDTSX = "p_PageView";
        public string GetAllt_sDTSX { get { return _GetAllt_sDTSX; } }

        private string _GetDtsxSteps = "select A.*,B.DTSXPath from t_sTaskDetails A left join t_sDTSX B on A.JobId=B.JobId AND A.TaskId=B.TaskId WHERE A.JobId=@JobId AND JobType='DTSX' ";
        /// <summary>
        /// ����u�@������DTSX�B�J
        /// </summary>
        public string GetDtsxSteps
        {
            get { return _GetDtsxSteps; }
        }
    }
}
