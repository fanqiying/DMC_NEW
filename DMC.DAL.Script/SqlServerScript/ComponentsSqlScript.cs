namespace DMC.DAL.Script.SqlServer
{
    public class ComponentsSqlScript : IComponents
    {
        /// <summary>
        /// �K�[
        /// <summary>
        private string _Addt_sComponents = " insert into t_sComponents(TaskId,JobId,ComponentsFuntion,ComponentsName,ComponentsParameters) values (@TaskId,@JobId,@ComponentsFuntion,@ComponentsName,@ComponentsParameters)";
        public string Addt_sComponents { get { return _Addt_sComponents; } }
        /// <summary>
        /// �ק�
        /// <summary>
        private string _Modt_sComponents = "update t_sComponents set ComponentsFuntion=@ComponentsFuntion,ComponentsName=@ComponentsName,ComponentsParameters=@ComponentsParameters  where TaskId=@TaskId  and JobId=@JobId ";
        public string Modt_sComponents { get { return _Modt_sComponents; } }
        /// <summary>
        /// ?��
        /// <summary>
        private string _Delt_sComponents = "delete from t_sComponents where JobId=@JobId";
        public string Delt_sComponents { get { return _Delt_sComponents; } }
        /// <summary>
        /// ?��
        /// <summary>
        private string _DelTaskIDList = "delete from t_sComponents where TaskId=@TaskId";
        public string DelTaskIDList { get { return _DelTaskIDList; } }
        /// <summary>
        /// ??�O�_�s�b
        /// <summary>
        private string _IsExitt_sComponents = "select count(0) from t_sComponents where TaskId=@TaskId  and JobId=@JobId";
        public string IsExitt_sComponents { get { return _IsExitt_sComponents; } }
        /// <summary>
        /// ���u�d?��^?�u��
        /// <summary>
        private string _GetListt_sComponents = "select AutoId,TaskId,JobId,ComponentsFuntion,ComponentsName,ComponentsParameters FROM t_sComponents  where 1=1 ";
        public string GetListt_sComponents { get { return _GetListt_sComponents; } }
        /// <summary>
        /// ���o�Ҧ�?�ƦC��
        /// <summary>
        private string _GetAllt_sComponents = "p_PageView";
        public string GetAllt_sComponents { get { return _GetAllt_sComponents; } }

        private string _GetJobComponentStep = "select A.*,B.ComponentsName,B.ComponentsFuntion,B.ComponentsParameters from t_sTaskDetails A left join t_sComponents B on A.JobId=B.JobId AND A.TaskId=B.TaskId WHERE A.JobId=@JobId AND JobType='Components' ";
        /// <summary>
        /// ����u�@�������B�J
        /// </summary>
        public string GetJobComponentStep
        {
            get { return _GetJobComponentStep; }
        }
    }
}
