using System.Collections.Generic;
using Utility.HelpClass;
using System.Data;
using System.Data.Common;
using System;
using DMC.DAL.Script;
using DMC.Model;

namespace DMC.DAL
{
    /// <summary>
    /// 部门资料维护作业数据操作类
    /// code by jeven_xiao
    /// 2013-6-8
    /// </summary>
    public class DeptDAL
    {
        private IDept script = ScriptFactory.GetScript<IDept>();
        /// <summary>
        /// 增加新部门资料
        /// </summary>
        /// <param name="dept">部门实体</param>
        /// <returns>bool</returns>
        public bool AddDept(t_Dept dept)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("deptID", DbType.String, dept.deptID, 20));
            param.Add(DBFactory.Helper.FormatParameter("companyID", DbType.String, dept.companyID, 20));
            param.Add(DBFactory.Helper.FormatParameter("falseDeptID", DbType.String, dept.falseDeptID, 20));
            param.Add(DBFactory.Helper.FormatParameter("simpleName", DbType.String, dept.simpleName, 20));
            param.Add(DBFactory.Helper.FormatParameter("fullName", DbType.String, dept.fullName, 100));
            param.Add(DBFactory.Helper.FormatParameter("deptNature", DbType.String, dept.deptNature, 1));
            param.Add(DBFactory.Helper.FormatParameter("deptGroup", DbType.String, dept.deptGroup, 50));
            param.Add(DBFactory.Helper.FormatParameter("deptHeader", DbType.String, dept.deptHeader, 20));
            param.Add(DBFactory.Helper.FormatParameter("createTime", DbType.DateTime, dept.createTime));
            param.Add(DBFactory.Helper.FormatParameter("usy", DbType.String, dept.usy, 1));
            param.Add(DBFactory.Helper.FormatParameter("createrID", DbType.String, dept.createrID, 20));
            param.Add(DBFactory.Helper.FormatParameter("cDeptID", DbType.String, dept.cDeptID, 20));

            using (Trans tr = new Trans())
            {
                try
                {
                    if (DBFactory.Helper.ExecuteNonQuery(script.AddDept, param.ToArray()) > 0)
                    {
                        tr.Commit();
                        return true;
                    }
                    else
                        return false;
                }
                catch(Exception ex)
                {
                    tr.RollBack();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 修改部门资料信息
        /// </summary>
        /// <param name="dept">部门资料实体</param>
        /// <returns>string</returns>
        public bool UpdateDept(t_Dept dept)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("deptID", DbType.String, dept.deptID, 20));
            param.Add(DBFactory.Helper.FormatParameter("companyID", DbType.String, dept.companyID, 20));
            param.Add(DBFactory.Helper.FormatParameter("falseDeptID", DbType.String, dept.falseDeptID, 20));
            param.Add(DBFactory.Helper.FormatParameter("simpleName", DbType.String, dept.simpleName, 20));
            param.Add(DBFactory.Helper.FormatParameter("fullName", DbType.String, dept.fullName, 100));
            param.Add(DBFactory.Helper.FormatParameter("deptNature", DbType.String, dept.deptNature, 1));
            param.Add(DBFactory.Helper.FormatParameter("deptGroup", DbType.String, dept.deptGroup, 50));
            param.Add(DBFactory.Helper.FormatParameter("deptHeader", DbType.String, dept.deptHeader, 20));
            param.Add(DBFactory.Helper.FormatParameter("modTime", DbType.DateTime, System.DateTime.Now));
            param.Add(DBFactory.Helper.FormatParameter("usy", DbType.String, dept.usy, 1));
            param.Add(DBFactory.Helper.FormatParameter("updaterID", DbType.String, dept.updaterID, 20));
            param.Add(DBFactory.Helper.FormatParameter("uDeptID", DbType.String, dept.uDeptID, 20));

            using (Trans tr = new Trans())
            {
                try
                {
                    if (DBFactory.Helper.ExecuteNonQuery(script.ModDept, param.ToArray()) > 0)
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
        /// 删除部门资料数据
        /// </summary>
        /// <param name="idList">部门编号集合</param>
        /// <returns>bool</returns>
        public bool DelDept(List<string> idList)
        {
            using (Trans tr = new Trans())
            {
                try
                {
                    foreach (string s in idList)
                    {
                        List<DbParameter> param = new List<DbParameter>();
                        param.Add(DBFactory.Helper.FormatParameter("idList", DbType.String, s, 20));
                        DBFactory.Helper.ExecuteNonQuery(script.DelDept, param.ToArray());

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
        /// 检查部门资料信息是否存在
        /// </summary>
        /// <param name="deptID">部门编号</param>
        /// <param name="compID">公司编号</param>
        /// <returns></returns>
        public bool IsExitDept(string deptID)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("deptID", System.Data.DbType.String, deptID, 20));
            return DBFactory.Helper.ExecuteScalar(script.IsExitDept, param.ToArray()) != null ? true : false;

        }

        /// <summary>
        /// 输入关键字带出部门资料
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetAllDeptToDT()
        {
            return DBFactory.Helper.ExecuteDataSet(script.GetAllDeptToDT, null).Tables[0];
        }

        /// <summary>
        /// 根据部门ID取主管
        /// </summary>
        /// <param name="deptID">部门编号</param>
        /// <returns>bool</returns>
        public string GetDeptHeaderByID(string deptID)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("deptID", System.Data.DbType.String, deptID, 20));
            return DBFactory.Helper.ExecuteScalar(script.GetDeptHeaderByID, param.ToArray()).To_String();
        }

        /// <summary>
        /// 取得部门性质
        /// </summary>
        /// <param name="deptID">部门编号</param>
        /// <returns></returns>
        public string GetDeptNature(string deptID)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("deptID", System.Data.DbType.String, deptID, 20));
            return DBFactory.Helper.ExecuteScalar(script.GetDeptNature, param.ToArray()).To_String();
        }
    }


}
