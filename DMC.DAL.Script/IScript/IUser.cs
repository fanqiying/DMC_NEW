
namespace DMC.DAL.Script
{
    public interface IUser
    {
        /// <summary>
        /// 更新用戶的登錄信息
        /// </summary>
        string ModUserLoginInfo { get; }
        /// <summary>
        ///域账号的公司别是否正确
        /// </summary>
        string IsDomainComp { get; }
        /// <summary>
        /// 是否是域账号
        /// </summary>
        string IsDomainID { get; }
        string IsOKcomp { get; }
        /// <summary>
        /// 系統帳號是否存在？
        /// </summary>
        string IsExitUserID { get; }

        /// <summary>
        /// 系統密碼是否正確
        /// </summary>
        string IsOKPwd { get; }

        /// <summary>
        /// 根据用户ID取得用户的详细信息
        /// </summary>
        string GetUserInfoByID { get; }

        /// <summary>
        /// 获取域账号对应的账号信息
        /// </summary>
        string GetUserInfoByDomain { get; }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        string ModUserPwd { get; }


        /// <summary>
        /// 旧密码是否正确
        /// </summary>
        string IsOkOldPwd { get; }


        /// <summary>
        /// 员工中是否存在账号
        /// </summary>
        string IsExitUserIDByEmpID { get; }

        /// <summary>
        ///  員工賬號是否已失效
        /// </summary>
        string IsUsedUserId
        {
            get;
        }

    }
}
