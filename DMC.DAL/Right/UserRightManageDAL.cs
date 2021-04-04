using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using DMC.DAL.Script;
using DMC.Model;

namespace DMC.DAL
{
    public class UserRightManageDAL
    {
        private ILanguage script = ScriptFactory.GetScript<ILanguage>();
        private IUserRight userRight = ScriptFactory.GetScript<IUserRight>(); 
        /// <summary>
        /// 添加執行者基本信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddUserInfo(t_User model, Trans t)
        {
            try
            {
                //參數設置
                List<DbParameter> param = new List<DbParameter>();
                param.Add(DBFactory.Helper.FormatParameter("userID", System.Data.DbType.String, model.userID, 20));
                param.Add(DBFactory.Helper.FormatParameter("userNo", System.Data.DbType.String, model.userNo, 20));
                param.Add(DBFactory.Helper.FormatParameter("userName", System.Data.DbType.String, model.userName, 50));
                param.Add(DBFactory.Helper.FormatParameter("userMail", System.Data.DbType.String, model.userMail, 50));
                param.Add(DBFactory.Helper.FormatParameter("userDept", System.Data.DbType.String, model.userDept, 20));
                param.Add(DBFactory.Helper.FormatParameter("defLanguage", System.Data.DbType.String, model.defLanguage, 20));
                param.Add(DBFactory.Helper.FormatParameter("userType", System.Data.DbType.String, model.userType, 2));
                param.Add(DBFactory.Helper.FormatParameter("domainID", System.Data.DbType.String, model.domainID, 80));
                param.Add(DBFactory.Helper.FormatParameter("domainAddr", System.Data.DbType.String, model.domainAddr, 50));
                param.Add(DBFactory.Helper.FormatParameter("usy", System.Data.DbType.String, model.usy, 1));
                param.Add(DBFactory.Helper.FormatParameter("createrID", System.Data.DbType.String, model.createrID, 20));
                param.Add(DBFactory.Helper.FormatParameter("cDeptID", System.Data.DbType.String, model.cDeptID, 20));
                param.Add(DBFactory.Helper.FormatParameter("createTime", System.Data.DbType.DateTime, System.DateTime.Now));

                //添加資源基本屬性
                int result = DBFactory.Helper.ExecuteNonQuery(userRight.AddUserInfo, param.ToArray(), t);
                return result == 1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool UpdateUserInfo(t_User model, Trans t)
        {
            try
            {
                //參數設置
                List<DbParameter> param = new List<DbParameter>();
                param.Add(DBFactory.Helper.FormatParameter("userID", System.Data.DbType.String, model.userID, 20));
                param.Add(DBFactory.Helper.FormatParameter("userNo", System.Data.DbType.String, model.userNo, 20));
                param.Add(DBFactory.Helper.FormatParameter("userName", System.Data.DbType.String, model.userName, 50));
                param.Add(DBFactory.Helper.FormatParameter("userMail", System.Data.DbType.String, model.userMail, 50));
                param.Add(DBFactory.Helper.FormatParameter("userDept", System.Data.DbType.String, model.userDept, 20));
                //param.Add(DBFactory.Helper.FormatParameter("defLanguage", System.Data.DbType.String, model.defLanguage, 20));
                param.Add(DBFactory.Helper.FormatParameter("userType", System.Data.DbType.String, model.userType, 2));
                param.Add(DBFactory.Helper.FormatParameter("domainID", System.Data.DbType.String, model.domainID, 80));
                param.Add(DBFactory.Helper.FormatParameter("domainAddr", System.Data.DbType.String, model.domainAddr, 50));
                param.Add(DBFactory.Helper.FormatParameter("usy", System.Data.DbType.String, model.usy, 1));
                param.Add(DBFactory.Helper.FormatParameter("updaterID", System.Data.DbType.String, model.updaterID, 20));
                param.Add(DBFactory.Helper.FormatParameter("uDeptID", System.Data.DbType.String, model.uDeptID, 20));
                param.Add(DBFactory.Helper.FormatParameter("lastModTime", System.Data.DbType.DateTime, System.DateTime.Now));
                param.Add(DBFactory.Helper.FormatParameter("defaultRole", System.Data.DbType.String, model.defaultRole));

                //添加資源基本屬性
                int result = DBFactory.Helper.ExecuteNonQuery(userRight.ModUserInfo, param.ToArray(), t);
                return result == 1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        /// <summary>
        /// 驗證用戶編號是否存在
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool ExistsUserId(string UserId)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("userID", DbType.String, UserId, 20));
            int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(userRight.ExistsUserId, param.ToArray()));
            return result != 0;
        }
        /// <summary>
        /// 驗證用戶編號是否存在
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool ExistsUserId(string UserId, Trans t)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("userID", DbType.String, UserId, 20));
            int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(userRight.ExistsUserId, param.ToArray(), t));
            return result != 0;
        }
        /// <summary>
        /// 刪除使用者
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool DeleteUserInfo(string UserId, Trans t)
        {
            try
            {
                //參數設置
                List<DbParameter> param = new List<DbParameter>();
                param.Add(DBFactory.Helper.FormatParameter("UserId", System.Data.DbType.String, UserId, 20));
                DBFactory.Helper.ExecuteScalar(userRight.DeleteUserInfo, param.ToArray(), t);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsEmployee(string EmployeeID, Trans t)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("EmployeeID", DbType.String, EmployeeID, 20));
            int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(userRight.IsEmployee, param.ToArray(), t));
            return result != 0;
        }

        public bool IsEmployee(string EmployeeID)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("EmployeeID", DbType.String, EmployeeID, 20));
            int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(userRight.IsEmployee, param.ToArray()));
            return result != 0;
        }

        public bool IsSupply(string SupplyId, Trans t)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("SupplyId", DbType.String, SupplyId, 20));
            int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(userRight.IsSupply, param.ToArray(), t));
            return result != 0;
        }

        public bool IsSupply(string SupplyId)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("SupplyId", DbType.String, SupplyId, 20));
            int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(userRight.IsSupply, param.ToArray()));
            return result != 0;
        }

        //public bool IsCustomer(string CustomerId, Trans t)
        //{
        //    List<DbParameter> param = new List<DbParameter>();
        //    param.Add(DBFactory.Helper.FormatParameter("CustomerId", DbType.String, CustomerId, 20));
        //    int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(userRight.IsCustomer, param.ToArray(), t));
        //    return result != 0;
        //}
        //public bool IsCustomer(string CustomerId)
        //{
        //    List<DbParameter> param = new List<DbParameter>();
        //    param.Add(DBFactory.Helper.FormatParameter("CustomerId", DbType.String, CustomerId, 20));
        //    int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(userRight.IsCustomer, param.ToArray()));
        //    return result != 0;
        //}
		/// <summary>
		/// 重置密碼
		/// </summary>
		/// <param name="UserId">用戶ID</param>
		/// <returns>結果</returns>
        public bool ResetPwd(string UserId, string userPwd)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("UserId", DbType.String, UserId, 20));
            param.Add(DBFactory.Helper.FormatParameter("userPwd", DbType.String, userPwd, 20));
            int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(userRight.ResetPwd, param.ToArray()));
            return result != 1;
        }
		/// <summary>
		/// 獲取用戶權限類別
		/// </summary>
		/// <param name="UserId">用戶編號</param>
		/// <param name="UserType">用戶類型</param>
		/// <param name="KeyWord">關鍵字</param>
		/// <returns>DataTable</returns>
        public DataTable GetUserRose(string UserId, string UserType, string KeyWord)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("UserId", DbType.String, UserId, 20));
            param.Add(DBFactory.Helper.FormatParameter("UserType", DbType.String, UserType, 2));
            param.Add(DBFactory.Helper.FormatParameter("KeyWord", DbType.String, KeyWord, 20));
            DataTable dt = DBFactory.Helper.ExecuteDataSet(userRight.ReadUserRose, param.ToArray()).Tables[0];
            return dt;
        }
		/// <summary>
		/// 自動獲取對應員工、供應商和客戶的信息
		/// </summary>
		/// <param name="Keyword">關鍵字</param>
		/// <param name="UserType">用戶類別</param>
		/// <returns>DataTable</returns>
        public DataTable JointESC(string Keyword, string UserType)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("Keyword", DbType.String, Keyword, 20));
            param.Add(DBFactory.Helper.FormatParameter("UserType", DbType.String, UserType, 2));
            DataTable dt = DBFactory.Helper.ExecuteDataSet(userRight.JointESC, param.ToArray()).Tables[0];
            return dt;
        }
        /// <summary>
        /// 獲取用戶的權限類別
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public DataTable GetUserRose(string UserId, string UserType, Trans t, string KeyWord = "")
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("UserId", DbType.String, UserId, 20));
            param.Add(DBFactory.Helper.FormatParameter("UserType", DbType.String, UserType, 2));
            param.Add(DBFactory.Helper.FormatParameter("KeyWord", DbType.String, KeyWord, 20));
            DataTable dt = DBFactory.Helper.ExecuteDataSet(userRight.ReadUserRose, param.ToArray(), t).Tables[0];
            return dt;
        }
        /// <summary>
        /// 獲取域服務器地址
        /// </summary>
        /// <returns></returns>
        public DataTable GetDomainServer()
        {
            return DBFactory.Helper.ExecuteDataSet(userRight.DomainServer, null).Tables[0];
        }

        /// <summary>
        /// 刪除用戶的權限類別
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="RoseId"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool DeleteUserRose(string UserId, string RoseId, Trans t)
        {
            try
            {
                List<DbParameter> param = new List<DbParameter>();
                param.Add(DBFactory.Helper.FormatParameter("UserId", DbType.String, UserId, 20));
                param.Add(DBFactory.Helper.FormatParameter("RoseId", DbType.String, RoseId, 20));
                DBFactory.Helper.ExecuteNonQuery(userRight.DeleteUserRose, param.ToArray(), t);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AddUserRose(string UserId, string RoseId, Trans t)
        {
            try
            {
                List<DbParameter> param = new List<DbParameter>();
                param.Add(DBFactory.Helper.FormatParameter("UserId", DbType.String, UserId, 20));
                param.Add(DBFactory.Helper.FormatParameter("RoseId", DbType.String, RoseId, 20));
                DBFactory.Helper.ExecuteNonQuery(userRight.AddUserRose, param.ToArray(), t);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
