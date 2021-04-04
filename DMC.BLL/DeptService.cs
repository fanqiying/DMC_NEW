using System.Collections.Generic;
using DMC.Model;
using DMC.DAL;
using System.Data;
using Utility.HelpClass;

namespace DMC.BLL
{
    /// <summary>
    ///部门基本业务操作类
    ///code by jeven_xiao
    ///2013-6-8
    /// </summary>
    public class DeptService
    {
        private PageManage pageView = new PageManage();
        private DeptDAL ddpt = new DeptDAL();

        /// <summary>
        /// 增加新部门资料
        /// </summary>
        /// <param name="dept">部门实体</param>
        /// <returns>string </returns>
        public string AddDept(t_Dept dept)
        {

            if (string.IsNullOrEmpty(dept.deptID))
                return "EB0016";
            if (string.IsNullOrEmpty(dept.simpleName))
                return "EB0017";
            if (IsExitDept(dept.deptID))

                return "EB0001";


            if (ddpt.AddDept(dept) == true)
            {
                return "SB0001";
            }
            else
            {
                return "UB0002";
            }





        }

        /// <summary>
        /// 更新部门资料信息
        /// </summary>
        /// <param name="dept">部门资料实体</param>
        /// <returns>string </returns>
        public string UpdateDept(t_Dept dept)
        {
            if (string.IsNullOrEmpty(dept.deptID))
                return "EB0016";
            if (string.IsNullOrEmpty(dept.simpleName))
                return "EB0017";
            if (IsExitDept(dept.deptID))
            {
                if (ddpt.UpdateDept(dept) == true)
                {
                    return "SB0004";
                }

                else
                {
                    return "UB0003";
                }
            }
            else
            {
                return "EB0001";
            }

        }

        /// <summary>
        /// 删除部门资料数据
        /// </summary>
        /// <param name="idList">部门编号集合</param>
        /// <returns>string </returns>
        public string DelDept(List<string> idList)
        {
            if (idList.Count < 1 || idList == null)
                return "EB0005";
            if (ddpt.DelDept(idList) == true)
                return "SB0007";
            else
                return "UB0004";
        }


        /// <summary>
        /// 取得所有部门资料信息的集合
        /// </summary>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="pageIndex">页码参数</param>
        ///  <param name="searchWhere">查询组合条件</param>
        /// <returns>ArrayList</returns>
        public DataTable GetAllDept(int pageSize, int pageIndex, out int pageCount, out int total, string Where = "")
        {
            string strFields = string.Empty;
            strFields += "deptID,falseDeptID,companyID,simpleName ,fullName,";
            strFields += "deptNature deptNatureId,deptNature as deptNature,deptGroup,";
            strFields += " deptHeader, usy,createrID,createrID as creater,";
            strFields += "cDeptID,cDeptID  as cDeptName,updaterID,updaterID as moder,uDeptID,uDeptID as uDeptName,";
            strFields += "createTime,lastModTime";
            return pageView.PageView("t_Dept", "autoID", pageIndex, pageSize, strFields, "autoID ASC", Where, out total, out pageCount);
        }
        /// <summary>
        /// 检查部门资料信息是否存在
        /// </summary>
        /// <param name="deptID">部门编号</param>
        /// <param name="compID">公司编号</param>
        /// <returns></returns>
        public bool IsExitDept(string deptID)
        {
            return ddpt.IsExitDept(deptID);


        }

        /// <summary>
        /// 根据用户输入的关键字 取得部门的资料集合 带cache缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<t_Dept> GetDeptListByKey(string key)
        {
            List<t_Dept> listT = new List<t_Dept>();
            List<t_Dept> listInfo = new List<t_Dept>();
            if (!Utility.HelpClass.Cache.Exists("Cache_DeptInfo"))
            {
                listT = ReturnDeptCCache();
            }
            else
            {
                listT = Utility.HelpClass.Cache.GetCache("Cache_DeptInfo") as List<t_Dept>;
            }

            //检索字母

            listInfo = listT.FindAll(r => r.deptID.ToLower().IndexOf(key.ToLower()) > -1 ? true : false);

            return listInfo;
        }

        public static List<t_Dept> ReturnDeptCCache()
        {
            List<t_Dept> listT = new List<t_Dept>();
            if (!Utility.HelpClass.Cache.Exists("Cache_DeptInfo"))
            {
                try
                {


                    DataTable dtt = GetAllDeptToDT();
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        t_Dept tinfo = new t_Dept();
                        tinfo.deptID = dtt.Rows[i]["deptID"].To_String();
                        tinfo.simpleName = dtt.Rows[i]["simpleName"].To_String();
                        tinfo.fullName = dtt.Rows[i]["fullName"].To_String();
                        tinfo.deptNature = dtt.Rows[i]["deptNature"].To_String();
                        listT.Add(tinfo);
                    }

                    if (listT.Count > 0)
                        Cache.SetCache("Cache_DeptInfo", listT, 60 * 60 * 8);
                }
                catch
                {
                }

            }
            else
            {
                listT = Cache.GetCache("Cache_DeptInfo") as List<t_Dept>;
            }

            return listT;
        }

        /// <summary>
        /// 输入关键字带出部门资料
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetAllDeptToDT()
        {
            return new DeptDAL().GetAllDeptToDT();
        }
        /// <summary>
        /// 根据部门ID取主管
        /// </summary>
        /// <param name="deptID">部门编号</param>
        /// <returns>bool</returns>
        public string GetDeptHeaderByID(string deptID)
        {
            return ddpt.GetDeptHeaderByID(deptID);
        }

        /// <summary>
        /// 取得部门性质
        /// </summary>
        /// <param name="deptID">部门编号</param>
        /// <returns></returns>
        public string GetDeptNature(string deptID)
        {
            return ddpt.GetDeptNature(deptID);
        }
    }
}
