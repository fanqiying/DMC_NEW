using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DMC.DAL.Script;

namespace DMC.DAL.Script.SqlServer
{
    public class LogSqlScript : ILog
    {

        /// <summary>
        /// 增加系统日志
        /// </summary>
        private string _writeLog = " insert into t_SysLog (operatorID,operatorName,refProgram,refClass " +
              ",refMethod,refRemark,refTime,refIP,refSql,refEvent)values(@operatorID," +
              "@operatorName,@refProgram,@refClass,@refMethod ,@refRemark,@createTime," +
             " @refIP,@refSql,@refEvent)";



        /// <summary>
        /// 删除日志资料
        /// </summary>
        private string _delSysLog = "delete from  t_SysLog where  autoID =@idList";



        string ILog.WriteLog
        {
            get { return _writeLog; }
        }

        string ILog.DelSysLog
        {
            get { return _delSysLog; }
        }
    }
}
