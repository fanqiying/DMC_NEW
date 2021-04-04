
namespace DMC.DAL.Script
{
    public interface IUserRight
    {
        /// <summary>
        /// 添加用戶信息
        /// </summary>
        string AddUserInfo { get; }
        /// <summary>
        /// 修改用戶信息
        /// </summary>
        string ModUserInfo { get; }
        /// <summary>
        /// 刪除用戶信息
        /// </summary>
        string DeleteUserInfo { get; }
        /// <summary>
        /// 檢查用戶Id是否存在
        /// </summary>
        string ExistsUserId { get; }
        /// <summary>
        /// 檢查是否為員工
        /// </summary>
        string IsEmployee { get; }
        /// <summary>
        /// 檢查是否為供應商
        /// </summary>
        string IsSupply { get; }
        /// <summary>
        /// 檢查是否為客戶
        /// </summary>
        //string IsCustomer { get; }
        /// <summary>
        /// 重置密碼
        /// </summary>
        string ResetPwd { get; }
        /// <summary>
        /// 獲取用戶
        /// </summary>
        string ReadUserRose { get; }
        /// <summary>
        /// 刪除用戶角色
        /// </summary>
        string DeleteUserRose { get; }
        /// <summary>
        /// 添加用戶角色
        /// </summary>
        string AddUserRose { get; }
        /// <summary>
        /// 联动获取员工、供应商和客户表中的编号、姓名和邮箱
        /// </summary>
        string JointESC { get; }
        /// <summary>
        /// 獲取域服務器
        /// </summary>
        string DomainServer { get; }
        /// <summary>
        /// 獲取用戶的資料權限
        /// </summary>
        string UserProgramData { get; }
    }
}
