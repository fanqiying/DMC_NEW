using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Utility.HelpClass;
using DMC.DAL.Script;
using DMC.Model;

namespace DMC.DAL
{
    /// <summary>
    /// 菜單目錄數據訪問操作類
    /// code by jeven_xiao
    /// 2013-6-9
    /// </summary>
    public class MenusDAL
    {
        private IMenus script = ScriptFactory.GetScript<IMenus>();
        /// <summary>
        /// 新建菜单目录
        /// </summary>
        /// <param name="menu">菜单实体</param>
        /// <returns>bool</returns>
        public bool AddMenu( t_Menu  menu)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("menuID", DbType.String, menu.menuID, 20));
            param.Add(DBFactory.Helper.FormatParameter("fatherID", DbType.String, menu.fatherID, 20));
            param.Add(DBFactory.Helper.FormatParameter("menuName", DbType.String, menu.menuName, 20));
            param.Add(DBFactory.Helper.FormatParameter("usy", DbType.String, menu.usy, 1));
            param.Add(DBFactory.Helper.FormatParameter("createrID", DbType.String, menu.createrID, 20));
            param.Add(DBFactory.Helper.FormatParameter("cDeptID", DbType.String, menu.cDeptID, 20));
            param.Add(DBFactory.Helper.FormatParameter("orderid", DbType.Int32, menu.orderid));
            param.Add(DBFactory.Helper.FormatParameter("createTime", DbType.DateTime, System.DateTime.Now));
            using (Trans tr = new Trans())
            {
                try
                {
                    if (DBFactory.Helper.ExecuteNonQuery(script.AddMenu, param.ToArray()) > 0)
                    {
                        tr.Commit();
                        return true;
                    }
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    tr.RollBack();
                    throw ex;
                }
            }
        
        }
        /// <summary>
        /// 修改菜单目录
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>

        public bool ModMenu(t_Menu menu)
        {

            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("menuID", DbType.String, menu.menuID, 20));
            param.Add(DBFactory.Helper.FormatParameter("fatherID", DbType.String, menu.fatherID, 20));
            param.Add(DBFactory.Helper.FormatParameter("menuName", DbType.String, menu.menuName, 20));
            param.Add(DBFactory.Helper.FormatParameter("usy", DbType.String, menu.usy, 1));
            param.Add(DBFactory.Helper.FormatParameter("updaterID", DbType.String, menu.updaterID, 20));
            param.Add(DBFactory.Helper.FormatParameter("uDept", DbType.String, menu.uDeptID, 20));
            param.Add(DBFactory.Helper.FormatParameter("orderid", DbType.Int32, menu.orderid));
            param.Add(DBFactory.Helper.FormatParameter("modTime", DbType.DateTime, System.DateTime.Now));

            using (Trans tr = new Trans())
            {
                try
                {
                    if (DBFactory.Helper.ExecuteNonQuery(script.ModMenu, param.ToArray()) > 0)
                    {
                        tr.Commit();
                        return true;
                    }
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    tr.RollBack();
                    throw ex;
                }
            }
 
        }
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="idList">菜单目录集合</param>
        /// <returns></returns>

        public bool DelMenu(List<string> idList)
        {
            using (Trans tr = new Trans())
            {
                try
                {
                    foreach (string s in idList)
                    {
                        List<DbParameter> param = new List<DbParameter>();
                        param.Add(DBFactory.Helper.FormatParameter("menuID", DbType.String, s, 20));
                        DBFactory.Helper.ExecuteNonQuery(script.DelMenu, param.ToArray());

                    }
                    tr.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    tr.RollBack();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 验证菜单目录是否存在
        /// </summary>
        /// <param name="menuID"></param>
        /// <returns></returns>
        public bool IsExitMenu(string menuID)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("menuID", System.Data.DbType.String, menuID, 20));  
            int result =Convert.ToInt32(DBFactory.Helper.ExecuteScalar(script.IsExitMenu, param.ToArray()));
            return result != 0;
        }

		/// <summary>
		/// 獲取搜有的菜單數據列表
		/// </summary>
		/// <returns>DataTable</returns>
        public DataTable GetAllMenuToDT()
        {
            return DBFactory.Helper.ExecuteDataSet(script.GetAllMenuToDT, null).Tables[0];
        }

		/// <summary>
		/// 取最大菜单编号
		/// </summary>
		/// <returns>最大的菜单编号</returns>
		public string GetMaxMenuID() {
			string str= DBFactory.Helper.ExecuteScalar(script.GetMaxMenuID, null).To_String();
            return str;
		}


		/// <summary>
		/// 取得某一菜单下的所有子菜单的最大值
		/// </summary>
		/// <param name="fatherID">该菜单的父级菜单编号</param>
		/// <returns>string
		/// </returns>
		public string GetNodeMaxMenuID(string fatherID) {
			List<DbParameter> param = new List<DbParameter>();
			param.Add(DBFactory.Helper.FormatParameter("fatherID", System.Data.DbType.String, fatherID, 20));
			return DBFactory.Helper.ExecuteScalar(script.GetNodeMaxMenuID, param.ToArray ()).To_String();
			
		
		}

    }
}
