using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using DMC.DAL.Script;

namespace DMC.DAL
{
    /// <summary>
    /// 供應商權限管理
    /// </summary>
    public class SupplyManageDAL
    {
        private ISupplyRight script = ScriptFactory.GetScript<ISupplyRight>();
        /// <summary>
        /// 獲取權限類別對應的供應商
        /// </summary>
        /// <param name="RoseId">權限編號</param>
        /// <returns></returns>
        public DataTable ReadSupplyByRose(string RoseId)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("RoseId", System.Data.DbType.String, RoseId, 20));
            DataTable dt = DBFactory.Helper.ExecuteDataSet(script.ReadSupplyRightByRose, param.ToArray()).Tables[0];
            return dt;
        }
        /// <summary>
        /// 添加权限类别的供应商权限
        /// </summary>
        /// <param name="RoseId"></param>
        /// <param name="SupplyId"></param>
        /// <returns></returns>
        public bool AddSupplyByRose(string RoseId, string SupplyId)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("RoseId", System.Data.DbType.String, RoseId, 20));
            param.Add(DBFactory.Helper.FormatParameter("SupplyId", System.Data.DbType.String, SupplyId, 20));
            int result = DBFactory.Helper.ExecuteNonQuery(script.AddSupplyRightByRose, param.ToArray());
            return (result == 1);
        }
        /// <summary>
        /// 使用事物控制添加程式的供应商权限
        /// </summary>
        /// <param name="RoseId"></param>
        /// <param name="SupplyId"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool AddSupplyByRose(string RoseId, string SupplyId, Trans t)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("RoseId", System.Data.DbType.String, RoseId, 20));
            param.Add(DBFactory.Helper.FormatParameter("SupplyId", System.Data.DbType.String, SupplyId, 20));
            int result = DBFactory.Helper.ExecuteNonQuery(script.AddSupplyRightByRose, param.ToArray(), t);
            return (result == 1);
        }

        /// <summary>
        /// 獲取個人權限對應的供應商
        /// </summary>
        /// <param name="UserId">用戶編號</param>
        /// <param name="CompanyId">公司編號</param>
        /// <returns></returns>
        public DataTable ReadSupplyByUser(string UserId)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("UserId", System.Data.DbType.String, UserId, 20));
            DataTable dt = DBFactory.Helper.ExecuteDataSet(script.ReadSupplyRightByUser, param.ToArray()).Tables[0];
            return dt;
        }

        /// <summary>
        /// 獲取個人權限對應的供應商
        /// </summary>
        /// <param name="UserId">用戶編號</param>
        /// <param name="CompanyId">公司編號</param>
        /// <returns></returns>
        public DataTable ReadSupplyByUser(string UserId, Trans t)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("UserId", System.Data.DbType.String, UserId, 20));
            DataTable dt = DBFactory.Helper.ExecuteDataSet(script.ReadSupplyRightByUser, param.ToArray(), t).Tables[0];
            return dt;
        }

        /// <summary>
        /// 添加使用者的供应商权限
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="SupplyId"></param>
        /// <returns></returns>
        public bool AddSupplyByUser(string UserId, string SupplyId)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("UserId", System.Data.DbType.String, UserId, 20));
            param.Add(DBFactory.Helper.FormatParameter("SupplyId", System.Data.DbType.String, SupplyId, 20));
            int result = DBFactory.Helper.ExecuteNonQuery(script.AddSupplyRightByUser, param.ToArray());
            return (result == 1);
        }
        /// <summary>
        /// 使用事物添加使用者的供应商权限
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="SupplyId"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool AddSupplyByUser(string UserId, string SupplyId, Trans t)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("UserId", System.Data.DbType.String, UserId, 20));
            param.Add(DBFactory.Helper.FormatParameter("SupplyId", System.Data.DbType.String, SupplyId, 20));
            int result = DBFactory.Helper.ExecuteNonQuery(script.AddSupplyRightByUser, param.ToArray(), t);
            return (result == 1);
        }

        /// <summary>
        /// 验证部门是否存在，适用事物
        /// </summary>
        /// <param name="DeptId"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool ExistsSupplyId(string SupplyId, Trans t)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("SupplyId", System.Data.DbType.String, SupplyId, 20));
            int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(script.ExistsSupplyId, param.ToArray(), t));
            return result != 0;
        }

        /// <summary>
        /// 验证部门是否存在，适用事物
        /// </summary>
        /// <param name="DeptId"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool ExistsSupplyId(string SupplyId)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("SupplyId", System.Data.DbType.String, SupplyId, 20));
            int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(script.ExistsSupplyId, param.ToArray()));
            return result != 0;
        }

        public bool DeleteSupplyRightByRose(string RoseId, string SupplyId, Trans t)
        {
            try
            {
                List<DbParameter> param = new List<DbParameter>();
                param.Add(DBFactory.Helper.FormatParameter("RoseId", System.Data.DbType.String, RoseId, 20));
                param.Add(DBFactory.Helper.FormatParameter("SupplyId", System.Data.DbType.String, SupplyId, 20));
                DBFactory.Helper.ExecuteScalar(script.DeleteSupplyRightByRose, param.ToArray(), t);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteSupplyRightByUser(string UserId, Trans t)
        {
            try
            {
                List<DbParameter> param = new List<DbParameter>();
                param.Add(DBFactory.Helper.FormatParameter("UserId", System.Data.DbType.String, UserId, 20));
                DBFactory.Helper.ExecuteScalar(script.DeleteSupplyRightByUser, param.ToArray(), t);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
