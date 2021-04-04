using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;
namespace DMC.DAL.Script.SqlServer
{
    public class t_sTaskFrequencySqlScript:ITaskFrequency
    {
        /// <summary>
        /// 添加
        /// <summary>
      private  string _Addt_sTaskFrequency=" insert into t_sTaskFrequency(JobId,StartDate,EndDate,RepeatedType,repeatedStartTime,sIntervalTime,sIntervalDateTime,DateYear,DateMonth,DateDay,Week,CExpression) values (@JobId,@StartDate,@EndDate,@RepeatedType,@repeatedStartTime,@sIntervalTime,@sIntervalDateTime,@DateYear,@DateMonth,@DateDay,@Week,@CExpression)";
      public   string Addt_sTaskFrequency{ get { return _Addt_sTaskFrequency; } }
        /// <summary>
        /// 修改
        /// <summary>
      private string _Modt_sTaskFrequency = "update t_sTaskFrequency set StartDate=@StartDate,EndDate=@EndDate,RepeatedType=@RepeatedType,repeatedStartTime=@repeatedStartTime,sIntervalTime=@sIntervalTime,sIntervalDateTime=@sIntervalDateTime,DateYear=@DateYear,DateMonth=@DateMonth,DateDay=@DateDay,Week=@Week,CExpression=@CExpression  where JobId=@JobId ";
      public   string Modt_sTaskFrequency{ get { return _Modt_sTaskFrequency; } }
        /// <summary>
        /// ?除
        /// <summary>
      private string _Delt_sTaskFrequency = "delete from t_sTaskFrequency where JobId=@JobId";
       public  string Delt_sTaskFrequency{ get { return _Delt_sTaskFrequency; } }
        /// <summary>
        /// ??是否存在
        /// <summary>
       private string _IsExitt_sTaskFrequency = "select count(0) from t_sTaskFrequency where JobId=@JobId";
       public  string IsExitt_sTaskFrequency{ get { return _IsExitt_sTaskFrequency; } }
        /// <summary>
        /// 根据查?返回?据集
        /// <summary>
      private string _GetListt_sTaskFrequency="select AutoID,JobId,StartDate,EndDate,RepeatedType,repeatedStartTime,sIntervalTime,sIntervalDateTime,DateYear,DateMonth,DateDay,Week,CExpression FROM t_sTaskFrequency  where 1=1 "; 
       public  string GetListt_sTaskFrequency{ get { return _GetListt_sTaskFrequency; } }
        /// <summary>
        /// 取得所有?料列表
        /// <summary>
      private string _GetAllt_sTaskFrequency="p_PageView";
       public  string GetAllt_sTaskFrequency{ get { return _GetAllt_sTaskFrequency; } }
    }
}
