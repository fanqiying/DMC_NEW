using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using DMC.DAL.Script;

namespace DMC.DAL
{
    /// <summary>
    /// 個人公司權限管理
    /// </summary>
    public class CompanyManageDAL
    {
        private ICompanyRight script = ScriptFactory.GetScript<ICompanyRight>();
        #region 個人公司權限

        public DataTable ReadUserCompanyRight(string UserId, string LanguageId)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("UserId", System.Data.DbType.String, UserId, 20));
            param.Add(DBFactory.Helper.FormatParameter("LanguageId", System.Data.DbType.String, LanguageId, 20));
            DataTable dt = DBFactory.Helper.ExecuteDataSet(script.ReadUserCompanyRight, param.ToArray()).Tables[0];
            return dt;
        }

        public DataTable ReadUserCompanyRight(string UserId, Trans t)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("UserId", System.Data.DbType.String, UserId, 20));
            DataTable dt = DBFactory.Helper.ExecuteDataSet(script.ReadUserCompany, param.ToArray(), t).Tables[0];
            return dt;
        }

        public bool AddUserCompanyRight(string UserId, string CompanyId)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("UserId", System.Data.DbType.String, UserId, 20));
            param.Add(DBFactory.Helper.FormatParameter("CompanyId", System.Data.DbType.String, CompanyId, 20));
            int result = DBFactory.Helper.ExecuteNonQuery(script.AddUserCompanyRight, param.ToArray());
            return (result == 1);
        }

        public bool AddUserCompanyRight(string UserId, string CompanyId, Trans t)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("UserId", System.Data.DbType.String, UserId, 20));
            param.Add(DBFactory.Helper.FormatParameter("CompanyId", System.Data.DbType.String, CompanyId, 20));
            int result = DBFactory.Helper.ExecuteNonQuery(script.AddUserCompanyRight, param.ToArray(), t);
            return (result == 1);
        }

        public bool DeleteUserCompanyRight(string UserId, string CompanyId)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("UserId", System.Data.DbType.String, UserId, 20));
            param.Add(DBFactory.Helper.FormatParameter("CompanyId", System.Data.DbType.String, CompanyId, 20));
            int result = DBFactory.Helper.ExecuteNonQuery(script.DeleteUserCompanyRight, param.ToArray());
            return (result == 1);
        }

        public bool DeleteUserCompanyRight(string UserId, string CompanyId, Trans t)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("UserId", System.Data.DbType.String, UserId, 20));
            param.Add(DBFactory.Helper.FormatParameter("CompanyId", System.Data.DbType.String, CompanyId, 20));
            int result = DBFactory.Helper.ExecuteNonQuery(script.DeleteUserCompanyRight, param.ToArray(), t);
            return (result == 1);
        }

        #endregion

        #region 程式公司權限
        public DataTable ReadProgramCompanyRight(string UserId, string ProgramId)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("UserId", System.Data.DbType.String, UserId, 20));
            param.Add(DBFactory.Helper.FormatParameter("ProgramId", System.Data.DbType.String, ProgramId, 20));
            DataTable dt = DBFactory.Helper.ExecuteDataSet(script.ReadProgramCompanyRight, param.ToArray()).Tables[0];
            return dt;
        }

        public bool AddUserProgramCompanyRight(string UserId, string ProgramId, string CompanyId)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("UserId", System.Data.DbType.String, UserId, 20));
            param.Add(DBFactory.Helper.FormatParameter("CompanyId", System.Data.DbType.String, CompanyId, 20));
            param.Add(DBFactory.Helper.FormatParameter("ProgramId", System.Data.DbType.String, ProgramId, 20));
            int result = DBFactory.Helper.ExecuteNonQuery(script.AddProgramCompanyRight, param.ToArray());
            return (result == 1);
        }

        public bool AddUserProgramCompanyRight(string UserId, string ProgramId, string CompanyId, Trans t)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("UserId", System.Data.DbType.String, UserId, 20));
            param.Add(DBFactory.Helper.FormatParameter("CompanyId", System.Data.DbType.String, CompanyId, 20));
            param.Add(DBFactory.Helper.FormatParameter("ProgramId", System.Data.DbType.String, ProgramId, 20));
            int result = DBFactory.Helper.ExecuteNonQuery(script.AddProgramCompanyRight, param.ToArray(), t);
            return (result == 1);
        }

        public bool DeleteProgramCompanyRight(string UserId, string ProgramId, string CompanyId)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("UserId", System.Data.DbType.String, UserId, 20));
            param.Add(DBFactory.Helper.FormatParameter("CompanyId", System.Data.DbType.String, CompanyId, 20));
            param.Add(DBFactory.Helper.FormatParameter("ProgramId", System.Data.DbType.String, ProgramId, 20));
            int result = DBFactory.Helper.ExecuteNonQuery(script.DeleteProgramCompanyRight, param.ToArray());
            return (result == 1);
        }

        public bool DeleteProgramCompanyRight(string UserId, string ProgramId, string CompanyId, Trans t)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("UserId", System.Data.DbType.String, UserId, 20));
            param.Add(DBFactory.Helper.FormatParameter("CompanyId", System.Data.DbType.String, CompanyId, 20));
            param.Add(DBFactory.Helper.FormatParameter("ProgramId", System.Data.DbType.String, ProgramId, 20));
            int result = DBFactory.Helper.ExecuteNonQuery(script.DeleteProgramCompanyRight, param.ToArray(), t);
            return (result == 1);
        }
        #endregion

        public bool ExistsCompanyId(string CompanyId)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("CompanyId", DbType.String, CompanyId, 20));
            int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(script.ExistsCompanyId, param.ToArray()));
            return result != 0;
        }

        public bool ExistsCompanyId(string CompanyId, Trans t)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("CompanyId", DbType.String, CompanyId, 20));
            int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(script.ExistsCompanyId, param.ToArray(), t));
            return result != 0;
        }
    }
}
