using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;
namespace DMC.DAL.Script.SqlServer
{
    public class EmailTaskSqlScript : IEmailTask
    {
        /// <summary>
        /// 添加
        /// <summary>
        private string _Addt_sEmailTask = " insert into t_sEmailTask(TaskId,JobId,EmailName,Tos,CCs,Emailsubmit,Body,EmailIsHtml,EmailAppendixPath,EmailIsZip,EmailZipFormat,EmailTitle) values (@TaskId,@JobId,@EmailName,@Tos,@CCs,@Emailsubmit,@Body,@EmailIsHtml,@EmailAppendixPath,@EmailIsZip,@EmailZipFormat,@EmailTitle)";
        public string Addt_sEmailTask { get { return _Addt_sEmailTask; } }
        /// <summary>
        /// 修改
        /// <summary>
        private string _Modt_sEmailTask = "update t_sEmailTask set EmailName=@EmailName,Tos=@Tos,CCs=@CCs,Emailsubmit=@Emailsubmit,Body=@Body,EmailIsHtml=@EmailIsHtml,EmailAppendixPath=@EmailAppendixPath,EmailIsZip=@EmailIsZip,EmailZipFormat=@EmailZipFormat,EmailTitle=@EmailTitle  where TaskId=@TaskId and JobId=@JobId";
        public string Modt_sEmailTask { get { return _Modt_sEmailTask; } }
        /// <summary>
        /// ?除
        /// <summary>
        private string _Delt_sEmailTask = "delete from t_sEmailTask where JobId=@JobId";
        public string Delt_sEmailTask { get { return _Delt_sEmailTask; } }
        /// <summary>
        /// ?除
        /// <summary>
        private string _DelTaskIDList = "delete from t_sEmailTask where TaskId=@TaskId ";
        public string DelTaskIDList { get { return _DelTaskIDList; } }
        /// <summary>
        /// ??是否存在
        /// <summary>
        private string _IsExitt_sEmailTask = "select count(0) from t_sEmailTask where TaskId=@TaskId  and JobId=@JobId";
        public string IsExitt_sEmailTask { get { return _IsExitt_sEmailTask; } }
        /// <summary>
        /// 根据查?返回?据集
        /// <summary>
        private string _GetListt_sEmailTask = "select AutoId,TaskId,EmailTitle,JobId,EmailName,Tos,CCs,EmailSubmit,Body,EmailIsHtml,EmailAppendixPath,EmailIsZip,EmailZipFormat FROM t_sEmailTask  where 1=1 ";
        public string GetListt_sEmailTask { get { return _GetListt_sEmailTask; } }
        /// <summary>
        /// 取得所有?料列表
        /// <summary>
        private string _GetAllt_sEmailTask = "p_PageView";
        public string GetAllt_sEmailTask { get { return _GetAllt_sEmailTask; } }

        private string _GetEmailSteps = "select A.ErrorInfo,A.RunName,A.RunOrder,A.JobType,B.*,C.[Address],C.Account,C.[Password],D.Pop3Url from t_sTaskDetails A left join t_sEmailTask B on A.JobId=B.JobId AND A.TaskId=B.TaskId left join t_sEmailAccount C on B.EmailName= C.EmailName left join t_sEmailType D on c.EmailTypeInfo=D.EmailTypeName WHERE A.JobId=@JobId AND JobType='Email' ";
        /// <summary>
        /// 獲取工作對應的郵件步驟
        /// </summary>
        public string GetEmailSteps
        {
            get { return _GetEmailSteps; }
        }
    }
}
