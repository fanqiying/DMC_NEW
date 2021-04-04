using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using DMC.DAL.Script;
using DMC.Model;

namespace DMC.DAL
{
    /// <summary>
    /// 系統日誌數據訪問操作類
    /// code by jeven_xiao
    /// 2013-6-9
    /// </summary>
    public class LogDAL
    {
        private ILanguage script = ScriptFactory.GetScript<ILanguage>();
        private ILog logScript = ScriptFactory.GetScript<ILog>();
        /// <summary>
        /// 寫操作日誌
        /// </summary>
        /// <param name="log">日誌實體</param>
        /// <returns>'EB0015' --該條系統日誌已經存在'SB0016' --系統日誌增加成功</returns>
        public bool WriteLog(t_SysLog log)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("operatorID", DbType.String, log.operatorID, 20));
            param.Add(DBFactory.Helper.FormatParameter("operatorName", DbType.String, log.operatorName, 50));
            param.Add(DBFactory.Helper.FormatParameter("refProgram", DbType.String, log.refProgram, 100));
            param.Add(DBFactory.Helper.FormatParameter("refClass", DbType.String, log.refClass, 100));
            param.Add(DBFactory.Helper.FormatParameter("refMethod", DbType.String, log.refMethod, 100));
            param.Add(DBFactory.Helper.FormatParameter("refRemark", DbType.String, log.refRemark,1000));
            param.Add(DBFactory.Helper.FormatParameter("refIP", DbType.String, log.refIP, 20));
            param.Add(DBFactory.Helper.FormatParameter("refSql", DbType.String, log.refSql, 800));
            param.Add(DBFactory.Helper.FormatParameter("refEvent", DbType.String, log.refEvent, 2000));
            param.Add(DBFactory.Helper.FormatParameter("createTime", DbType.DateTime, System.DateTime .Now));

            using (Trans tr = new Trans())
            {
                try
                {
                    if (DBFactory.Helper.ExecuteNonQuery(logScript.WriteLog, param.ToArray()) > 0)
                    {
                        tr.Commit();
                        return true;
                    }
                    else
                        return false;
                }
                catch
                {
                    tr.RollBack();
                    throw;
                }
            }
        }

        /// <summary>
        /// 根據ID刪除日誌資料數據
        /// </summary>
        /// <param name="idList">list  string</param>
        /// <returns></returns>
        public bool DelSysLog(List <string> idList)
        {
            using (Trans tr = new Trans())
            {
                try
                {
                    foreach (string s in idList)
                    {
                        List<DbParameter> param = new List<DbParameter>();
                        param.Add(DBFactory.Helper.FormatParameter("idList", DbType.String, s, 20));
                        DBFactory.Helper.ExecuteNonQuery(logScript.DelSysLog, param.ToArray());

                    }
                    tr.Commit();
                    return true;
                }
                catch
                {
                    tr.RollBack();
                    return false;

                }
            }
        }
    }
}
