using System.Collections.Generic;
using System.Data;
using DMC.Model;
using DMC.DAL;

namespace DMC.BLL
{
    /// <summary>
    /// 權限管理業務類
    /// 封裝權限操作的所有業務邏輯方法
    /// code by klint 2013-6-12
    /// </summary>
    public class PermissionServices
    {
        private PermissionDAL perManage = new PermissionDAL();

        /// <summary>
        /// 獲取用戶的所有程式和菜單目錄--沒有的程式不獲取
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="langId">語言編號</param>
        /// <param name="MenuTree">返回的樹目錄</param>
        /// <returns>
        /// EU0005 用戶不存在
        /// EU0006 語言類別不存在
        /// 返回空表示成功
        /// </returns>
        public string GetMyMenu(string userId, string langId, string CompanyId, ref DataTable MenuTree)
        {
            return perManage.GetMyMenu(userId, langId, CompanyId, ref MenuTree);
        }

        /// <summary>
        /// 獲取我的公司
        /// </summary>
        /// <param name="userId">用戶編號</param>
        /// <param name="langId">語言編號</param>
        /// <param name="myCompany">返回公司列表</param>
        /// <returns>
        /// EU0005 用戶不存在
        /// EU0006 語言類別不存在
        /// 返回空表示成功
        /// </returns>
        public string GetMyCompany(string userId, string langId, ref DataTable myCompany)
        {
            return perManage.GetMyCompany(userId, langId, ref myCompany);
        }

        /// <summary>
        /// 獲取我的程式操作權限
        /// </summary>
        /// <param name="userId">用戶編號</param>
        /// <param name="programId">程式編號</param>
        /// <param name="langId">語言編號</param>
        /// <param name="myPgmOpt">返回程式權限操作列表</param>
        /// <returns>
        /// EU0005 用戶不存在
        /// EU0006 語言類別不存在
        /// EU0011 程式編號不存在
        /// 返回空表示成功
        /// </returns>
        public string GetMyPgmOpt(string userId, string programId, string langId, ref List<ProgramActionInfo> paInfoList)
        {
            return perManage.GetMyPgmOpt(userId, programId, langId, ref paInfoList);
        }

        /// <summary>
        /// 檢查是否有程式權限
        /// </summary>
        /// <param name="userId">用戶編號</param>
        /// <param name="programId">程式編號</param>
        /// <param name="langId">語言編號</param>
        /// <returns></returns>
        public bool IsExistsPgm(string userId, string programId, string langId, string CompanyId)
        {
            DataTable dt = new DataTable();
            string result = perManage.GetMyMenu(userId, langId, CompanyId, ref dt);
            if (!string.IsNullOrEmpty(result))
                return false;

            if (dt.Select("IsProgram='Y' AND ID='" + programId + "'").Length > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 檢查是否有程式的操作權限
        /// </summary>
        /// <param name="userId">用戶編號</param>
        /// <param name="programId">程式編號</param>
        /// <param name="actionId">操作編號</param>
        /// <param name="langId">語言編號</param>
        /// <returns></returns>
        public bool IsExistsOpt(string userId, string programId, string actionId, string langId)
        {
            List<ProgramActionInfo> paInfoList = new List<ProgramActionInfo>();
            string result = perManage.GetMyPgmOpt(userId, programId, langId, ref paInfoList);
            if (!string.IsNullOrEmpty(result))
                return false;

            //獲取到角色的操作權限
            ProgramActionInfo paInfo = paInfoList.Find(o => o.ProgramId == programId && o.ActionId == actionId);
            if (paInfo == null)
                return false;
            return paInfo.IsUse == "Y";
        }
        /// <summary>
        /// 獲取用戶操作的部門權限
        /// </summary>
        /// <param name="UserId">用戶ID</param>
        /// <param name="ProgramId">程式編號</param>
        /// <returns>實體</returns>
        public List<DeptDataRight> GetUserProgramData(string UserId, string ProgramId)
        {
            DataTable dt = perManage.UserProgramData(UserId, ProgramId);

            List<DeptDataRight> result = new List<DeptDataRight>();
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["DeptId"].ToString() != "0")
                {
                    DeptDataRight ddr = result.Find(o => o.DeptId == dr["DeptId"].ToString());
                    if (ddr == null)
                    {
                        ddr = new DeptDataRight();
                        ddr.DeptId = dr["DeptId"].ToString();
                        ddr.IsAll = dr["IsAll"].ToString();
                        result.Add(ddr);
                    }
                    else
                    {
                        if (dr["UserOrRose"].ToString() == "02")
                        {
                            ddr.IsAll = dr["IsAll"].ToString();
                        }
                    }
                }
                else
                {
                    result.Clear();
                    DeptDataRight ddr = new DeptDataRight();
                    ddr.DeptId = dr["DeptId"].ToString();
                    ddr.IsAll = dr["IsAll"].ToString();
                    result.Add(ddr);
                    break;
                }
            }
            return result;
        }
    }

    /// <summary>
    /// 部門資料權限
    /// </summary>
    public class DeptDataRight
    {
        public string DeptId { get; set; }
        public string IsAll { get; set; }
    }
}
