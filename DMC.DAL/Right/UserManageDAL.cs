using System;
using System.Data;
using Utility.HelpClass;
using System.Collections.Generic;
using System.Data.Common;
using DMC.DAL.Script;
using DMC.Model;
using System.Linq;

namespace DMC.DAL
{
    /// <summary>
    /// 用戶管理
    /// </summary>
    public class UserManageDAL
    {
        private IUser script = ScriptFactory.GetScript<IUser>();
        private IPermission ps = ScriptFactory.GetScript<IPermission>();
        /// <summary>
        /// 獲取用戶或權限類別的程式權限設置樹
        /// </summary>
        /// <param name="type">類別：1、用戶；2、權限類別</param>
        /// <param name="typeId">用戶編號或權限編號</param>
        /// <param name="langId">語言編號</param>
        /// <param name="MenuTree">返回設置的樹結構</param>
        /// <returns>
        /// EU0005 用戶不存在
        /// EU0006 語言類別不存在
        /// EU0007 類別設置錯誤
        /// ER0009 權限類別編號不存在
        /// 返回空表示成功
        /// </returns>
        public string GetSettingMenuTree(string isUserOrRose, string typeId, string langId, ref DataTable MenuTree)
        {
            string result = string.Empty;
            try
            {
                List<DbParameter> param = new List<DbParameter>();
                param.Add(DBFactory.Helper.FormatParameter("TypeId", DbType.String, typeId, 20));
                param.Add(DBFactory.Helper.FormatParameter("LangId", DbType.String, langId, 20));
                string sql = (isUserOrRose == "1" ? ps.GetMenuStatusByUser : ps.GetMenuStatusByRose);
                MenuTree = DBFactory.Helper.ExecuteDataSet(sql, param.ToArray()).Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return result;
        }

        /// <summary>
        /// 獲取菜單的主目錄
        /// </summary>
        /// <param name="langId">語言編號</param>
        /// <param name="MenuTree">返回菜單樹</param>
        /// <returns>
        /// EU0006 語言類別不存在
        /// 返回空表示成功
        /// </returns>
        public string GetMainMenu(string langId, ref DataTable MenuTree)
        {
            string result = string.Empty;
            try
            {
                List<DbParameter> param = new List<DbParameter>();
                param.Add(DBFactory.Helper.FormatParameter("LangId", DbType.String, langId, 20));
                MenuTree = DBFactory.Helper.ExecuteDataSet(ps.GetMainMenu, param.ToArray()).Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return result;
        }



        /// <summary>
        /// 根据用户ID取得用户详细信息的实体
        /// add by jeven_xiao
        /// </summary>
        /// <param name="userID">用户的账号</param>
        /// <returns>userinfo  实体</returns>
        public UserInfo GetUserInfoByID(string UserID)
        {
            UserInfo userInfo = new UserInfo();
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("userID", DbType.String, UserID, 20));
            return DBFactory.ConvertTo<UserInfo>.FillModel(userInfo, DBFactory.Helper.ExecuteDataSet(script.GetUserInfoByID, param.ToArray()).Tables[0]);
        }

        public List<UserInfo> GetUserInfoByDomain(string account, string compID)
        {
            List<UserInfo> userInfo = new List<UserInfo>();
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("userID", DbType.String, account, 50));
            param.Add(DBFactory.Helper.FormatParameter("compID", DbType.String, compID, 20));
            var result = DBFactory.ConvertTo<UserInfo>.FillModel(DBFactory.Helper.ExecuteDataSet(script.GetUserInfoByDomain, param.ToArray()).Tables[0]);
            userInfo = result.ToList();
            return userInfo;
        }


        public DataTable GetUserInfo()
        {
            return DBFactory.Helper.ExecuteDataSet(" select empId as userID,empName as userName,empMail as userMail from t_Employee where usy='Y' ", null).Tables[0];
        }



        /// <summary>
        /// 验证系统账号是否存在
        /// add by jeven_xiao
        /// 20313-6-25
        /// </summary>
        /// <param name="userID">系统账号</param>
        /// <returns>ture or false</returns>
        public bool IsExitUserID(string userID)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("userID", DbType.String, userID, 20));
            int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(script.IsExitUserID, param.ToArray()));
            return result != 0;
        }



        /// <summary>
        /// 验证系统账号是否是有效的帳號
        /// add by jeven_xiao
        /// 20313-6-25
        /// </summary>
        /// <param name="userID">系统账号</param>
        /// <returns>ture or false</returns>
        public bool IsUsedUserID(string userID)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("userID", DbType.String, userID, 20));
            int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(script.IsUsedUserId, param.ToArray()));
            return result != 0;
        }



        /// <summary>
        /// 验证系统密码是否正确
        /// add by jeven_xiao
        /// 2013-6-25
        /// </summary>
        /// <param name="userID">账号</param>
        /// <param name="userPwd">账号密码</param>
        /// <returns>true or false</returns>
        public bool IsOKPwd(string userID, string userPwd)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("userID", DbType.String, userID, 20));
            param.Add(DBFactory.Helper.FormatParameter("userPwd", DbType.String, userPwd, 128));
            object obj = DBFactory.Helper.ExecuteScalar(script.IsOKPwd, param.ToArray());
            return obj == null || string.IsNullOrEmpty(obj.ToString()) ? false : true;
        }

        /// <summary>
        /// 验证用户的公司别是否正确
        /// add by jeven_xiao 
        /// </summary>
        /// <param name="userID">系统账号</param>
        /// <param name="userPwd">账号密码</param>
        /// <param name="compID">账号所属公司别</param>
        /// <returns>true or bool</returns>
        public bool IsOKcomp(string userID, string userPwd, string compID)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("userID", DbType.String, userID, 20));
            param.Add(DBFactory.Helper.FormatParameter("userPwd", DbType.String, userPwd, 128));
            param.Add(DBFactory.Helper.FormatParameter("compID", DbType.String, compID, 20));
            return DBFactory.Helper.ExecuteScalar(script.IsOKcomp, param.ToArray()).To_String() != "" ? true : false;

        }


        /// <summary>
        /// 验证是否是域账号
        /// add by jeven_xiao 
        /// </summary>
        /// <param name="userID">用户的账号</param>
        /// <returns>true or false</returns>
        public bool IsDomainID(string userID)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("userID", DbType.String, userID, 20));
            int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(script.IsDomainID, param.ToArray()));
            return result != 0;
        }

        /// <summary>
        /// 验证域账号的公司别是否正确
        /// </summary>
        /// <param name="compID">公司别ID</param>
        /// <returns>true or false</returns>
        public bool IsDomainComp(string userID, string compID)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("userID", DbType.String, userID, 20));
            param.Add(DBFactory.Helper.FormatParameter("compID", DbType.String, compID, 20));
            return DBFactory.Helper.ExecuteScalar(script.IsDomainComp, param.ToArray()).To_String() != "" ? true : false;
        }

        /// <summary>
        /// 修改用户的登录信息
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="ip">登录IP</param>
        /// <param name="time">登录时间</param>
        /// <returns></returns>
        public bool ModUserLoginInfo(string userID, string ip, DateTime time)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("userID", DbType.String, userID, 20));
            param.Add(DBFactory.Helper.FormatParameter("lastLoginIP", DbType.String, ip, 20));
            param.Add(DBFactory.Helper.FormatParameter("lastLoginTime", DbType.DateTime, time));
            using (Trans tr = new Trans())
            {
                try
                {
                    if (DBFactory.Helper.ExecuteNonQuery(script.ModUserLoginInfo, param.ToArray()) > 0)
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
        /// 修改用户密码
        /// </summary>
        /// <param name="userID">用户的账号</param>
        /// <param name="pwd">用户的新密码</param>
        /// <returns>bool</returns>
        public bool ModUserPwd(string userID, string pwd)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("userID", DbType.String, userID, 20));
            param.Add(DBFactory.Helper.FormatParameter("userPwd", DbType.String, pwd, 128));
            using (Trans tr = new Trans())
            {
                try
                {
                    if (DBFactory.Helper.ExecuteNonQuery(script.ModUserPwd, param.ToArray()) > 0)
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
        /// 验证新密码是否正确
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="oldPwd"></param>
        /// <returns></returns>
        public bool IsOkOldPwd(string userID, string oldPwd)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("userID", DbType.String, userID, 20));
            param.Add(DBFactory.Helper.FormatParameter("userPwd", DbType.String, oldPwd, 128));

            int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(script.IsOkOldPwd, param.ToArray()));
            return result != 0;
        }



        /// <summary>
        /// 验证删除员工时是否删除账号
        /// </summary>
        /// <param name="empID">員工編號</param>
        /// <returns>bool</returns>
        public bool IsExitUserIDByEmpID(string empID)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("empID", DbType.String, empID, 20));
            int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(script.IsExitUserIDByEmpID, param.ToArray()));
            return result != 0;
        }
    }
}
