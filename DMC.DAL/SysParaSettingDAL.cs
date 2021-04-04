using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using DMC.DAL.Script;
using DMC.Model;

namespace DMC.DAL
{
    public class SysParaSettingDAL
    {
        private ISysParaSetting script = ScriptFactory.GetScript<ISysParaSetting>();
        /// <summary>
        /// 新增系統參數
        /// </summary>
        /// <param name="model"></param>
        public bool AddSysPara(SysParaSettingModel model)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("@CompanyId", DbType.String, model.CompanyId, 20));
            param.Add(DBFactory.Helper.FormatParameter("@ParaKey", DbType.String, model.ParaKey, 100));
            param.Add(DBFactory.Helper.FormatParameter("@ParaName", DbType.String, model.ParaName, 100));
            param.Add(DBFactory.Helper.FormatParameter("@ParaContent", DbType.String, model.ParaContent, 512));
            param.Add(DBFactory.Helper.FormatParameter("@ParaDesc", DbType.String, model.ParaDesc, 256));
            param.Add(DBFactory.Helper.FormatParameter("@InUser", DbType.String, model.InUser, 20));
            param.Add(DBFactory.Helper.FormatParameter("@Usey", DbType.String, model.Usey, 1));
            return DBFactory.Helper.ExecuteNonQuery(script.Add, param.ToArray()) > 0;
        }

        /// <summary>
        /// 修改系統參數
        /// </summary>
        /// <param name="model"></param>
        public bool EditSysPara(SysParaSettingModel model)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("@CompanyId", DbType.String, model.CompanyId, 20));
            param.Add(DBFactory.Helper.FormatParameter("@ParaKey", DbType.String, model.ParaKey, 100));
            param.Add(DBFactory.Helper.FormatParameter("@ParaName", DbType.String, model.ParaName, 100));
            param.Add(DBFactory.Helper.FormatParameter("@ParaContent", DbType.String, model.ParaContent, 512));
            param.Add(DBFactory.Helper.FormatParameter("@ParaDesc", DbType.String, model.ParaDesc, 256));
            param.Add(DBFactory.Helper.FormatParameter("@InUser", DbType.String, model.InUser, 20));
            param.Add(DBFactory.Helper.FormatParameter("@Usey", DbType.String, model.Usey, 1));
            return DBFactory.Helper.ExecuteNonQuery(script.Edit, param.ToArray()) > 0;
        }

        /// <summary>
        /// 刪除系統參數
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="ParaKey"></param>
        public bool DelSysPara(string CompanyId, string ParaKey)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("@CompanyId", DbType.String, CompanyId, 20));
            param.Add(DBFactory.Helper.FormatParameter("@ParaKey", DbType.String, ParaKey, 100));
            return DBFactory.Helper.ExecuteNonQuery(script.Delete, param.ToArray()) > 0;
        }

        /// <summary>
        /// 獲取參數是否存在
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="ParaKey"></param>
        /// <returns></returns>
        public DataTable ExistsPara(string CompanyId, string ParaKey)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("@CompanyId", DbType.String, CompanyId, 20));
            param.Add(DBFactory.Helper.FormatParameter("@ParaKey", DbType.String, ParaKey, 100));
            return DBFactory.Helper.ExecuteDataSet(script.Exists, param.ToArray()).Tables[0];
        }
    }
}
