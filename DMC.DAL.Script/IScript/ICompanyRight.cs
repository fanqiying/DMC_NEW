
namespace DMC.DAL.Script
{
    /// <summary>
    /// 公司權限
    /// </summary>
    public interface ICompanyRight
    {
        /// <summary>
        /// 添加用戶的公司權限
        /// </summary>
        string AddUserCompanyRight { get; }
        /// <summary>
        /// 刪除用戶的公司權限
        /// </summary>
        string DeleteUserCompanyRight { get; }
        /// <summary>
        /// 獲取用戶的公司權限
        /// </summary>
        string ReadUserCompanyRight { get; }
        /// <summary>
        /// 添加個人程式的公司權限
        /// </summary>
        string AddProgramCompanyRight { get; }
        /// <summary>
        /// 刪除個人程式的公司權限
        /// </summary>
        string DeleteProgramCompanyRight { get; }
        /// <summary>
        /// 獲取個人程式的公司權限
        /// </summary>
        string ReadProgramCompanyRight { get; }
        /// <summary>
        /// 是否存在授權的公司
        /// </summary>
        string ExistsCompanyId { get; }
        /// <summary>
        /// 獲取用戶的公司權限
        /// </summary>
        string ReadUserCompany { get; }
    }
}
