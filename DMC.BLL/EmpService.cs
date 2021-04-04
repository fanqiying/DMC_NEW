using System.Collections.Generic;
using DMC.Model;
using DMC.DAL;
using System.Data;
using Utility.HelpClass;

namespace DMC.BLL
{
    /// <summary>
    /// 員工業務操作類
    /// code by jeven_xiao
    /// 2013-6-9
    /// </summary>
    public class EmpService
    {
        private PageManage pageView = new PageManage();
        private EmpDAL edal = new EmpDAL();
        /// <summary>
        /// 增加新員工資料信息
        /// code by jeven_xiao
        /// 2013-6-9
        /// </summary>
        /// <param name="emp">員工實體</param>
        /// <returns>EB0002 員工資料已經存在；-SB0002員工資料增加成功</returns>
        public string AddEmp(t_Employee emp)
        {
            if (string.IsNullOrEmpty(emp.empID))
                return "EB0018";
            if (string.IsNullOrEmpty(emp.empName))
                return "EB0019";
            if (IsExistEmp(emp.empID))
                return "EB0002";
            if (edal.AddEmp(emp) == true)
            {
                return "SB0002";
            }
            else
            {
                return "UB0002";
            }


        }


        /// <summary>
        /// 更新員工資料信息
        /// code by jeven_xiao
        /// 2013-6-9
        /// </summary>
        /// <param name="emp">員工實體</param>
        /// <returns>'SB0006' --員工資料更新成功'EB0012' --員工資料不存在</returns>
        public string ModEmp(t_Employee emp)
        {
            if (string.IsNullOrEmpty(emp.empID))
                return "EB0018";
            if (string.IsNullOrEmpty(emp.empName))
                return "EB0019";
            if (IsExistEmp(emp.empID))
            {

                if (edal.ModEmp(emp) == true)
                {
                    return "SB0006";
                }

                else
                {
                    return "UB0003";
                }
            }
            else
            {
                return "EB0012";
            }

        }

        /// <summary>
        /// 刪除員工資料信息
        /// </summary>
        /// <param name="idList">ID字符串：1,2,3</param>
        /// <param name="compID">員工的公司ID</param>
        /// <returns>'SB0009' --成功刪除員工資料'EB0006'  --未設置需要刪除的員工編號</returns>
        public string DelEmp(List<string> idList)
        {
            if (idList.Count < 1 || idList == null)
                return "EB0006";
            if (edal.DelEmp(idList) == true)
                return "SB0009";
            else
                return "UB0004";
        }

        /// <summary>
        /// 驗證員工是否存在
        /// </summary>
        /// <param name="empID">員工的編號</param>
        /// <param name="compID">員工公司ID</param>
        /// <returns>string</returns>
        public bool IsExistEmp(string empID)
        {
            return edal.IsExistEmp(empID);
        }

        /// <summary>
        /// 根據條件取得員工資料集合
        /// </summary>
        /// <param name="pageSize">每页個數</param>
        /// <param name="pageIndex">页碼 </param>
        /// <param name="searchWhere">查詢條件</param>
        /// <returns> list</returns>
        public DataTable GetAllEmp(int pageSize, int pageIndex, out int pageCount, out int total, string Where = "", string ShowField = "*")
        {
            return pageView.PageView("t_Employee", "autoid", pageIndex, pageSize, ShowField, "AutoId ASC", Where, out total, out pageCount);
        }

        /// <summary>
        /// 獲取所有存在
        /// </summary>
        /// <returns></returns>
        public DataTable GetExistsEmailEmp()
        {
            return edal.GetExistsEmailEmp();
        }
        /// <summary>
        /// 獲取修訂信息中的電話號碼
        /// </summary>
        /// <param name="CreateUserId"></param>
        /// <param name="UpdateUserId"></param>
        /// <returns></returns>
        public DataTable GetEmpTel(string CreateUserId, string UpdateUserId)
        {
            return edal.GetEmpTel(CreateUserId, UpdateUserId);
        }
        /// <summary>
        /// 根據員工編號的關鍵字獲取包含該關鍵字的員工列表
        /// </summary>
        /// <param name="key">員工編號關鍵字</param>

        public static List<t_Employee> GetEmpListByKey(string key)
        {
            List<t_Employee> listT = new List<t_Employee>();
            List<t_Employee> listInfo = new List<t_Employee>();
            if (!Utility.HelpClass.Cache.Exists("Cache_EmpInfo"))
            {
                listT = ReturnEmpCCache();
            }
            else
            {
                listT = Utility.HelpClass.Cache.GetCache("Cache_EmpInfo") as List<t_Employee>;
            }

            //检索字母

            listInfo = listT.FindAll(r => r.empID.ToLower().IndexOf(key.ToLower()) > -1 ? true : false);

            return listInfo;
        }
        /// <summary>
        /// 獲取員工列表并設置集合緩存
        /// </summary>
        /// <returns></returns>
        public static List<t_Employee> ReturnEmpCCache()
        {
            List<t_Employee> listT = new List<t_Employee>();
            if (!Utility.HelpClass.Cache.Exists("Cache_EmpInfo"))
            {
                try
                {
                    DataTable dtt = GetAllEmpToDT();
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        t_Employee tinfo = new t_Employee();
                        tinfo.empID = dtt.Rows[i]["empID"].To_String();
                        tinfo.empMail = dtt.Rows[i]["empMail"].To_String();
                        tinfo.empName = dtt.Rows[i]["empName"].To_String();
                        listT.Add(tinfo);
                    }

                    if (listT.Count > 0)
                        Cache.SetCache("Cache_EmpInfo", listT, 60 * 60 * 8);
                }
                catch
                {
                }

            }
            else
            {
                listT = Cache.GetCache("Cache_EmpInfo") as List<t_Employee>;
            }

            return listT;
        }
        /// <summary>
        /// 獲取所有的員工數據集
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetAllEmpToDT()
        {
            return new EmpDAL().GetAllEmpToDT();
        }

        /// <summary>
        /// 验证某个部门下是否存在员工
        /// </summary>
        /// <param name="deptID">部门ID</param>
        /// <returns></returns>
        public bool IsExitEmpByDeptID(string deptID)
        {
            return new EmpDAL().IsExitEmpByDeptID(deptID);
        }
        /// <summary>
        /// 獲取員工上級主管編號
        /// </summary>
        /// <param name="strWhere">查詢條件</param>
        /// <returns>DataTable</returns>
        public DataTable GetEmpList(string strWhere)
        {
            return edal.GetEmpList(strWhere);
        }
        /// <summary>
        /// 根據員工編號，獲取員工詳細信息
        /// </summary>
        /// <param name="strWhere">查詢條件</param>
        /// <returns>DataTable</returns>
        public DataTable GetEmpInfoByID(string strWhere)
        {

            return edal.GetEmpInfoByID(strWhere);
        }
    }
}
