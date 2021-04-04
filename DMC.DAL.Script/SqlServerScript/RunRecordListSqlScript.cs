using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;
namespace DMC.DAL.Script.SqlServer
{
    public class RunRecordListSqlScript:IRunRecordList
    {
        /// <summary>
        /// 添加
        /// <summary>
        private string _Addt_sRunRecordList = " insert into t_sRunRecordList(JobId,TaskID,RunStartTime,RunEndTime,RUNNum,RunResult,Repeateds,ErrCode,ErrMsg,BKey) values (@JobId,@TaskID,@RunStartTime,@RunEndTime,@RUNNum,@RunResult,@Repeateds,@ErrCode,@ErrMsg,@BKey)";
      public   string Addt_sRunRecordList{ get { return _Addt_sRunRecordList; } }
        /// <summary>
        /// 修改
        /// <summary>
      private string _Modt_sRunRecordList="update t_sRunRecordList set JobId=@JobId,TaskID=@TaskID,RunStartTime=@RunStartTime,RunEndTime=@RunEndTime,RUNNum=@RUNNum,RunResult=@RunResult,Repeateds=@Repeateds,ErrCode=@ErrCode,ErrMsg=@ErrMsg  where =@ ";
      public   string Modt_sRunRecordList{ get { return _Modt_sRunRecordList; } }
        /// <summary>
        /// ?除
        /// <summary>
       private string _Delt_sRunRecordList="delete from t_sRunRecordList where =@";
       public  string Delt_sRunRecordList{ get { return _Delt_sRunRecordList; } }
        /// <summary>
        /// ??是否存在
        /// <summary>
       private string _IsExitt_sRunRecordList="select count(0) from t_sRunRecordList where =@";
       public  string IsExitt_sRunRecordList{ get { return _IsExitt_sRunRecordList; } }
        /// <summary>
        /// 根据查?返回?据集
        /// <summary>
      private string _GetListt_sRunRecordList="select JobId,TaskID,RunStartTime,RunEndTime,RUNNum,RunResult,Repeateds,ErrCode,ErrMsg FROM t_sRunRecordList  where 1=1 "; 
       public  string GetListt_sRunRecordList{ get { return _GetListt_sRunRecordList; } }
        /// <summary>
        /// 取得所有?料列表
        /// <summary>
      private string _GetAllt_sRunRecordList="p_PageView";
       public  string GetAllt_sRunRecordList{ get { return _GetAllt_sRunRecordList; } }
    }
}
