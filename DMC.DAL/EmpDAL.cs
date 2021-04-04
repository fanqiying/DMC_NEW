using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using DMC.DAL.Script;
using DMC.Model;

namespace DMC.DAL
{
    /// <summary>
    /// 員工數據訪問操作類
    /// code by jeven_xiao
    /// 2013-6-9
    /// </summary>
    public class EmpDAL
    {
        private IEmplyee script = ScriptFactory.GetScript<IEmplyee>();
        /// <summary>
        /// 增加新員工資料信息
        /// code by jeven_xiao
        /// 2013-6-9
        /// </summary>
        /// <param name="emp">員工實體</param>
        /// <returns>bool
        public bool AddEmp(t_Employee emp)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("empID", DbType.String, emp.empID, 20));
            param.Add(DBFactory.Helper.FormatParameter("companyID", DbType.String, emp.companyID, 20));
            param.Add(DBFactory.Helper.FormatParameter("empName", DbType.String, emp.empName, 20));
            param.Add(DBFactory.Helper.FormatParameter("empDept", DbType.String, emp.empDept, 20));
            param.Add(DBFactory.Helper.FormatParameter("empTitle", DbType.String, emp.empTitle, 50));
            param.Add(DBFactory.Helper.FormatParameter("extTelNo", DbType.String, emp.extTelNo, 50));
            param.Add(DBFactory.Helper.FormatParameter("empMail", DbType.String, emp.empMail, 100));
            param.Add(DBFactory.Helper.FormatParameter("signerID", DbType.String, emp.signerID, 20));
            param.Add(DBFactory.Helper.FormatParameter("createTime", DbType.DateTime, System.DateTime.Now));
            param.Add(DBFactory.Helper.FormatParameter("usy", DbType.String, emp.usy, 1));
            param.Add(DBFactory.Helper.FormatParameter("createrID", DbType.String, emp.createrID, 20));
            param.Add(DBFactory.Helper.FormatParameter("cDeptID", DbType.String, emp.cDeptID, 20));
            using (Trans t = new Trans())
            {
                try
                {
                    if (DBFactory.Helper.ExecuteNonQuery(script.AddEmp, param.ToArray()) > 0)
                    {
                        t.Commit();
                        return true;
                    }
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    t.RollBack();
                    throw ex;
                }
            }

        }


        /// <summary>
        /// 更新員工資料信息
        /// code by jeven_xiao
        /// 2013-6-9
        /// </summary>
        /// <param name="emp">員工實體</param>
        /// <returns>bool </returns>
        public bool ModEmp(t_Employee emp)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("empID", DbType.String, emp.empID, 20));
            param.Add(DBFactory.Helper.FormatParameter("companyID", DbType.String, emp.companyID, 20));
            param.Add(DBFactory.Helper.FormatParameter("empName", DbType.String, emp.empName, 20));
            param.Add(DBFactory.Helper.FormatParameter("empDept", DbType.String, emp.empDept, 20));
            param.Add(DBFactory.Helper.FormatParameter("empTitle", DbType.String, emp.empTitle, 50));
            param.Add(DBFactory.Helper.FormatParameter("extTelNo", DbType.String, emp.extTelNo, 50));
            param.Add(DBFactory.Helper.FormatParameter("empMail", DbType.String, emp.empMail, 100));
            param.Add(DBFactory.Helper.FormatParameter("signerID", DbType.String, emp.signerID, 20));
            param.Add(DBFactory.Helper.FormatParameter("modTime", DbType.DateTime, System.DateTime.Now));
            param.Add(DBFactory.Helper.FormatParameter("usy", DbType.String, emp.usy, 1));
            param.Add(DBFactory.Helper.FormatParameter("updaterID", DbType.String, emp.updaterID, 20));
            param.Add(DBFactory.Helper.FormatParameter("uDeptID", DbType.String, emp.uDeptID, 20));
            using (Trans t = new Trans())
            {
                try
                {
                    if (DBFactory.Helper.ExecuteNonQuery(script.ModEmp, param.ToArray()) > 0)
                    {
                        t.Commit();
                        return true;
                    }
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    t.RollBack();
                    throw ex;
                }
            }

        }

        /// <summary>
        /// 刪除員工資料信息
        /// </summary>
        /// <param name="idList">ID字符串：1,2,3</param>
        /// <param name="compID">員工的公司ID</param>
        /// <returns>'SB0009' --成功刪除員工資料'EB0006'  --未設置需要刪除的員工編號</returns>
        public bool DelEmp(List<string> empList)
        {
            using (Trans tr = new Trans())
            {
                try
                {
                    foreach (string s in empList)
                    {
                        List<DbParameter> param = new List<DbParameter>();
                        param.Add(DBFactory.Helper.FormatParameter("idList", DbType.String, s, 20));
                        DBFactory.Helper.ExecuteNonQuery(script.DelEmp, param.ToArray());

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
        /// 驗證員工是否存在
        /// </summary>
        /// <param name="empID">員工的編號</param>
        /// <param name="compID">員工公司ID</param>
        /// <returns>string</returns>
        public bool IsExistEmp(string compID)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("empID", System.Data.DbType.String, compID, 20));
            return DBFactory.Helper.ExecuteScalar(script.IsExitEmp, param.ToArray()) != null ? true : false;
        }

        /// <summary>
        /// 獲取所有的存在合法郵箱的員工帳號
        /// </summary>
        /// <returns></returns>
        public DataTable GetExistsEmailEmp()
        {
            return DBFactory.Helper.ExecuteDataSet(script.GetExistsEmailEmp, null).Tables[0];
        }
        /// <summary>
        /// 獲取員工的電話號碼
        /// </summary>
        /// <param name="CreateUserId">創建人</param>
        /// <param name="UpdateUserId">更新人</param>
        /// <returns></returns>
        public DataTable GetEmpTel(string CreateUserId, string UpdateUserId)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("Create", System.Data.DbType.String, CreateUserId, 20));
            param.Add(DBFactory.Helper.FormatParameter("Update", System.Data.DbType.String, UpdateUserId, 20));
            return DBFactory.Helper.ExecuteDataSet(script.ReadEmpTel, param.ToArray()).Tables[0];
        }
		/// <summary>
		/// 修改員工郵件信息
		/// </summary>
		/// <param name="empID">員工編號</param>
		/// <param name="empName">員工姓名</param>
		/// <param name="empMail">員工郵箱</param>
		/// <param name="t">事物對象</param>
        public void UpdateEmpEmailInfo(string empID, string empName, string empMail, Trans t)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("empID", System.Data.DbType.String, empID, 20));
            param.Add(DBFactory.Helper.FormatParameter("empName", System.Data.DbType.String, empName, 50));
            param.Add(DBFactory.Helper.FormatParameter("empMail", System.Data.DbType.String, empMail, 50));
            DBFactory.Helper.ExecuteNonQuery(script.UpdateEmpEmailInfo, param.ToArray(), t);
        }

		/// <summary>
		/// 獲取所有的員工數據集
		/// </summary>
		/// <returns>DataTable</returns>
        public DataTable GetAllEmpToDT()
        {
            return DBFactory.Helper.ExecuteDataSet(script.GetAllEmpToDT, null).Tables[0];
        }

        /// <summary>
        /// 验证某个部门下是否存在员工
        /// </summary>
        /// <param name="deptID">部门ID</param>
        /// <returns></returns>
        public bool IsExitEmpByDeptID(string deptID)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("deptID", DbType.String, deptID, 20));
            int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(script.IsExitEmpByDeptID, param.ToArray()));
            return result != 0;
        }
        public DataTable GetEmpList(string strWhere)
        {
            return DBFactory.Helper.ExecuteDataSet(script.GetEmpList+strWhere,null).Tables[0];
        }
		/// <summary>
		/// 根據員工編號，獲取員工詳細信息
		/// </summary>
		/// <param name="strWhere">查詢條件</param>
		/// <returns>DataTable</returns>
		public DataTable GetEmpInfoByID(string strWhere) {
			return DBFactory.Helper.ExecuteDataSet(script.GetEmpInfoByID + strWhere, null).Tables[0];
		}
    }
}
