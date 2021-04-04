using System.Data;
using DMC.DAL;

namespace DMC.BLL
{
	/// <summary>
	/// 公司權限管理業務方法類
	/// 處理公司權限相關操作
	/// </summary>
    public class CompanyManageService
    {
        private CompanyManageDAL companyManage = new CompanyManageDAL();
        /// <summary>
        /// 獲取用戶的公司權限
        /// </summary>
        /// <param name="RoseId"></param>
        /// <returns></returns>
        public DataTable ReadCompanyRight(string UserId, string LanguageId)
        {
            return companyManage.ReadUserCompanyRight(UserId, LanguageId);
        }
		/// <summary>
		/// 程式公司權限
		/// </summary>
		/// <param name="UserId">用戶編號</param>
		/// <param name="ProgramId">程式編號</param>
		/// <returns>DataTable</returns>
        public DataTable ReadProgramCompanyRight(string UserId, string ProgramId)
        {
            return companyManage.ReadProgramCompanyRight(UserId, ProgramId);
        }
    }
}
