using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;
namespace DMC.DAL.Script.SqlServer
{
    public class FtpTaskSqlScript : IFtpTask
    {
        /// <summary>
        /// 添加
        /// <summary>
        private string _Addt_sFtpTask = " insert into t_sFtpTask(TaskId,JobId,FtpName,FtpType,FtpFilePath,FtpAppendixPath,FtpIsZip,FtpZipFormat,CreateName,FtpIsDel) values (@TaskId,@JobId,@FtpName,@FtpType,@FtpFilePath,@FtpAppendixPath,@FtpIsZip,@FtpZipFormat,@CreateName,@FtpIsDel)";
        public string Addt_sFtpTask { get { return _Addt_sFtpTask; } }
        /// <summary>
        /// 修改
        /// <summary>
        private string _Modt_sFtpTask = "update t_sFtpTask set FtpName=@FtpName,FtpType=@FtpType,FtpFilePath=@FtpFilePath,FtpAppendixPath=@FtpAppendixPath,FtpIsZip=@FtpIsZip,FtpZipFormat=@FtpZipFormat,CreateName=@CreateName,FtpIsDel=@FtpIsDel  where TaskId=@TaskId  and JobId=@JobId ";
        public string Modt_sFtpTask { get { return _Modt_sFtpTask; } }
        /// <summary>
        /// ?除
        /// <summary>
        private string _Delt_sFtpTask = "delete from t_sFtpTask where JobId=@JobId";
        public string Delt_sFtpTask { get { return _Delt_sFtpTask; } }
        /// <summary>
        /// ?除
        /// <summary>
        private string _DelTaskIDList = "delete from t_sFtpTask where TaskId=@TaskId";
        public string DelTaskIDList { get { return _DelTaskIDList; } }
        /// <summary>
        /// ??是否存在
        /// <summary>
        private string _IsExitt_sFtpTask = "select count(0) from t_sFtpTask where TaskId=@TaskId  and JobId=@JobId ";
        public string IsExitt_sFtpTask { get { return _IsExitt_sFtpTask; } }
        /// <summary>
        /// 根据查?返回?据集
        /// <summary>
        private string _GetListt_sFtpTask = "select AutoId,TaskId,JobId,FtpName,FtpType,FtpFilePath,FtpAppendixPath,FtpIsZip,FtpIsDel,FtpZipFormat,CreateName FROM t_sFtpTask  where 1=1 ";
        public string GetListt_sFtpTask { get { return _GetListt_sFtpTask; } }
        /// <summary>
        /// 取得所有?料列表
        /// <summary>
        private string _GetAllt_sFtpTask = "p_PageView";
        public string GetAllt_sFtpTask { get { return _GetAllt_sFtpTask; } }

        private string _GetFtpSteps = "select A.*,B.FtpAppendixPath,B.FtpFilePath,B.FtpIsZip,B.FtpIsDel,B.FtpType,B.FtpZipFormat,B.CreateName,C.Account,C.FtpAddress,C.[Password],C.[Path],C.Port from t_sTaskDetails A left join t_sFtpTask B on A.JobId=B.JobId AND A.TaskId=B.TaskId left join t_sFtpAccount C on B.FtpName=C.FtpName WHERE  A.JobId=@JobId AND JobType='FTP' ";
        /// <summary>
        /// 獲取工作對應的FTP步驟
        /// </summary>
        public string GetFtpSteps
        {
            get { return _GetFtpSteps; }
        }
    }
}
