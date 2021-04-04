
namespace DMC.DAL.Script.SqlServer
{
    public class UserSqlScript : IUser
    {
        /// <summary>
        /// 系統帳號是否存在
        /// </summary>
		private string _isExitUserID = "select Count(0) from t_User where userID=@userID";




        /// <summary>
        /// 判斷賬號是否已失效
        /// </summary>
        private string _isUsedUserId = @"select     count(0)  from    t_User   a inner  join    
t_Employee     b  on     a.userNo =  b.empID
where  a.usy ='Y'   and     b.usy = 'Y'  and  a.userID=@userID ";




        /// <summary>
        /// 系統密碼是否正確
        /// </summary>
        private string _isOKPwd = "select userPwd from t_User where userID=@userID and userPwd collate Chinese_PRC_CS_AS_WS =@userPwd";

        /// <summary>
        /// 验证用户的公司别
        /// </summary>
        private string _isOKcomp = "select * from t_User AS A LEFT OUTER JOIN t_Right_UCompany AS B ON A.userID = B.UserId where A.userID=@userID AND userPwd collate Chinese_PRC_CS_AS_WS=@userPwd and compid=@compID";
        /// <summary>
        /// 根据用户ID取得用户的详细信息
        /// </summary>
        private string _getUserInfoByID = @" 
        select b.autoid,b.userid,b.userno,b.username,b.userpwd,b.usermail,b.deflanguage,
        b.usertype,b.domainid,b.domainaddr,b.lastloginip,b.lastlogintime,b.usy,b.createrid,b.cdeptid,b.updaterid,b.udeptid,b.createtime,b.lastmodtime,b.defaultRole,
        a.extTelNo,a.empdept as userDept
        from   t_employee a,
               t_user b
        where  b.userno=a.empid
               and b.userID=@userID
        ";
        /// <summary>
        /// 是否是域账号登录
        /// </summary>
        private string _isDomainID = "select count(*) from t_User where domainID=@userID";
        /// <summary>
        ///域账号的公司别是否正确
        /// </summary>
        private string _isDomainComp = "select * from t_User AS A LEFT OUTER JOIN t_Right_UCompany AS B ON A.userID = B.UserId where domainID=@userID AND CompId=@compID";

        /// <summary>
        /// 更新用戶的登錄信息
        /// </summary>
        private string _ModUserLoginInfo = "update t_User  set lastLoginIP=@lastLoginIP,lastLoginTime=@lastLoginTime where userID=@userID";


        /// <summary>
        /// 根据域账号取用户的信息
        /// </summary>
        private string _getUserInfoByDomain = "select a.*,b.extTelNo from t_User a left join t_Employee b on a.userno=b.empID WHERE UserId in(select A.userId from t_User AS A LEFT OUTER JOIN t_Right_UCompany AS B ON A.userID = B.UserId where domainID=@userID AND CompId=@compID)";

        /// <summary>
        /// 修改用户密码
        /// </summary>
        private string _modUserPwd = "update t_User set userPwd =@userPwd where userID =@userID";

        /// <summary>
        /// 旧密码是否正确
        /// </summary>
        private string _isOkOldPwd = "select COUNT (0) from t_User where userID =@userID and userPwd =@userPwd";

        private string _isExitUserIDByEmpID = "select COUNT (*) from t_User where userNo=@empID";

        public string IsExitUserID
        {
            get { return _isExitUserID; }
        }


        public string IsOKPwd
        {
            get { return _isOKPwd; }
        }



        string IUser.IsOKcomp
        {
            get { return _isOKcomp; }
        }

        string IUser.IsExitUserID
        {
            get { return _isExitUserID; }
        }

        string IUser.IsOKPwd
        {
            get { return _isOKPwd; }
        }


        string IUser.GetUserInfoByID
        {
            get { return _getUserInfoByID; }
        }

        string IUser.IsDomainID
        {
            get { return _isDomainID; }
        }

        string IUser.IsDomainComp
        {
            get { return _isDomainComp; }
        }

        string IUser.ModUserLoginInfo
        {
            get { return _ModUserLoginInfo; }
        }  

        string IUser.GetUserInfoByDomain
        {
            get { return _getUserInfoByDomain; }
        }

        string IUser.ModUserPwd
        {
            get { return _modUserPwd; }
        }


        string IUser.IsOkOldPwd
        {
            get { return _isOkOldPwd; }
        }


        public string IsExitUserIDByEmpID
        {
            get { return _isExitUserIDByEmpID; }
        }




        string IUser.IsUsedUserId
        {
            get
            {
                return _isUsedUserId;
            }
        }

    }
}
