using System.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using DMC.DAL.Script;

namespace DMC.DAL
{
    /// <summary>
    /// 權限類別資料權限管理
    /// </summary>
    public class DataManageDAL
    {
        private IDataRight script = ScriptFactory.GetScript<IDataRight>();
        /// <summary>
        /// 獲取權限類別的資料權限
        /// </summary>
        /// <param name="RoseId"></param>
        /// <returns></returns>
        public DataTable ReadDataByRose(string RoseId)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("RoseId", System.Data.DbType.String, RoseId, 20));
            DataTable dt = DBFactory.Helper.ExecuteDataSet(script.ReadRoseDataRight, param.ToArray()).Tables[0];
            return dt;
        }

        public DataTable ReadDataRightByRose(string RoseId)
        { 
             List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("RoseId", System.Data.DbType.String, RoseId, 20));
            DataTable dt = DBFactory.Helper.ExecuteDataSet(script.ReadRightDataList, param.ToArray()).Tables[0];
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RoseId"></param>
        /// <param name="DeptId"></param>
        /// <param name="IsAll"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool SaveDataByRose(string RoseId, string DeptId, string IsAll)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("RoseId", System.Data.DbType.String, RoseId, 20));
            param.Add(DBFactory.Helper.FormatParameter("DeptId", System.Data.DbType.String, DeptId, 20));
            param.Add(DBFactory.Helper.FormatParameter("IsAll", System.Data.DbType.String, IsAll, 1));
            int result = DBFactory.Helper.ExecuteNonQuery(script.AddRoseDataRight, param.ToArray());
            return (result == 1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="RoseId"></param>
        /// <param name="DeptId"></param>
        /// <param name="IsAll"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool SaveDataByRose(string RoseId, string DeptId, string IsAll, Trans t)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("RoseId", System.Data.DbType.String, RoseId, 20));
            param.Add(DBFactory.Helper.FormatParameter("DeptId", System.Data.DbType.String, DeptId, 20));
            param.Add(DBFactory.Helper.FormatParameter("IsAll", System.Data.DbType.String, IsAll, 1));
            int result = DBFactory.Helper.ExecuteNonQuery(script.AddRoseDataRight, param.ToArray(), t);
            return (result == 1);
        }

        public bool UpdateDataByRose(string RoseId, string DeptId, string IsAll, Trans t)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("RoseId", System.Data.DbType.String, RoseId, 20));
            param.Add(DBFactory.Helper.FormatParameter("DeptId", System.Data.DbType.String, DeptId, 20));
            param.Add(DBFactory.Helper.FormatParameter("IsAll", System.Data.DbType.String, IsAll, 1));
            int result = DBFactory.Helper.ExecuteNonQuery(script.UpdateRoseDataRight, param.ToArray(), t);
            return (result == 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="ProgramId"></param>
        /// <returns></returns>
        public DataTable ReadDataByUserAndProgram(string UserId, string ProgramId)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("UserId", System.Data.DbType.String, UserId, 20));
            param.Add(DBFactory.Helper.FormatParameter("ProgramId", System.Data.DbType.String, ProgramId, 20));
            DataTable dt = DBFactory.Helper.ExecuteDataSet(script.ReadProgramDataRight, param.ToArray()).Tables[0];
            return dt;
        }

        public DataTable ReadProgramDataList(string UserId, string ProgramId)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("UserId", System.Data.DbType.String, UserId, 20));
            param.Add(DBFactory.Helper.FormatParameter("ProgramId", System.Data.DbType.String, ProgramId, 20));
            DataTable dt = DBFactory.Helper.ExecuteDataSet(script.ReadProgramDataList, param.ToArray()).Tables[0];
            return dt;
        }

        /// <summary>
        /// 保存使用者的程式资料权限
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="ProgramId"></param>
        /// <param name="DeptId"></param>
        /// <param name="IsAll"></param>
        /// <returns></returns>
        public bool SaveDataByUserAndProgram(string UserId, string ProgramId, string DeptId, string IsAll)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("UserId", System.Data.DbType.String, UserId, 20));
            param.Add(DBFactory.Helper.FormatParameter("ProgramId", System.Data.DbType.String, ProgramId, 20));
            param.Add(DBFactory.Helper.FormatParameter("DeptId", System.Data.DbType.String, DeptId, 20));
            param.Add(DBFactory.Helper.FormatParameter("IsAll", System.Data.DbType.String, IsAll, 1));
            int result = DBFactory.Helper.ExecuteNonQuery(script.AddProgramDataRight, param.ToArray());
            return (result == 1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="ProgramId"></param>
        /// <param name="DeptId"></param>
        /// <param name="IsAll"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool SaveDataByUserAndProgram(string UserId, string ProgramId, string DeptId, string IsAll, Trans t)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("UserId", System.Data.DbType.String, UserId, 20));
            param.Add(DBFactory.Helper.FormatParameter("ProgramId", System.Data.DbType.String, ProgramId, 20));
            param.Add(DBFactory.Helper.FormatParameter("DeptId", System.Data.DbType.String, DeptId, 20));
            param.Add(DBFactory.Helper.FormatParameter("IsAll", System.Data.DbType.String, IsAll, 1));
            int result = DBFactory.Helper.ExecuteNonQuery(script.AddProgramDataRight, param.ToArray(), t);
            return (result == 1);
        }
        /// <summary>
        /// 更新資料權限
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="ProgramId"></param>
        /// <param name="DeptId"></param>
        /// <param name="IsAll"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool UpdateDataByUserAndProgram(string UserId, string ProgramId, string DeptId, string IsAll, Trans t)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("UserId", System.Data.DbType.String, UserId, 20));
            param.Add(DBFactory.Helper.FormatParameter("ProgramId", System.Data.DbType.String, ProgramId, 20));
            param.Add(DBFactory.Helper.FormatParameter("DeptId", System.Data.DbType.String, DeptId, 20));
            param.Add(DBFactory.Helper.FormatParameter("IsAll", System.Data.DbType.String, IsAll, 1));
            int result = DBFactory.Helper.ExecuteNonQuery(script.UpdateProgramDataRight, param.ToArray(), t);
            return (result == 1);
        }
        /// <summary>
        /// 验证部门是否存在，适用事物
        /// </summary>
        /// <param name="DeptId"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool ExistsDeptId(string DeptId, Trans t)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("DeptId", System.Data.DbType.String, DeptId, 20));
            int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(script.ExistsDeptId, param.ToArray(), t));
            return result != 0;
        }
        /// <summary>
        /// 验证部门是否存在,不使用事物
        /// </summary>
        /// <param name="DeptId"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool ExistsDeptId(string DeptId)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("DeptId", System.Data.DbType.String, DeptId, 20));
            int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(script.ExistsDeptId, param.ToArray()));
            return result != 0;
        }

        public bool DeleteRoseDataRight(string RoseId, string DeptId, Trans t)
        {
            try
            {
                List<DbParameter> param = new List<DbParameter>();
                param.Add(DBFactory.Helper.FormatParameter("RoseId", System.Data.DbType.String, RoseId, 20));
                param.Add(DBFactory.Helper.FormatParameter("DeptId", System.Data.DbType.String, DeptId, 20));
                DBFactory.Helper.ExecuteNonQuery(script.DeleteRoseDataRight, param.ToArray(), t);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteProgramDataRight(string UserId, string ProgramId, string DeptId, Trans t)
        {
            try
            {
                List<DbParameter> param = new List<DbParameter>();
                param.Add(DBFactory.Helper.FormatParameter("UserId", System.Data.DbType.String, UserId, 20));
                param.Add(DBFactory.Helper.FormatParameter("ProgramId", System.Data.DbType.String, ProgramId, 20));
                param.Add(DBFactory.Helper.FormatParameter("DeptId", System.Data.DbType.String, DeptId, 20));
                DBFactory.Helper.ExecuteNonQuery(script.DeleteProgramDataRight, param.ToArray(), t);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
