using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMC.DAL.Script.SqlServer
{
    public class UserRightSqlScript : IUserRight
    {
        private string _addUserInfo = @" INSERT INTO t_User(userID,userNo,userName,userMail,userDept,userType,domainID,domainAddr,usy,createrID,cDeptID,createTime,userPwd) " +
                                       " VALUES(@userID,@userNo,@userName,@userMail,@userDept,@userType,@domainID,@domainAddr,@usy,@createrID,@cDeptID,@createTime,'123456'); ";
        public string AddUserInfo
        {
            get { return _addUserInfo; }
        }

        private string _modUserInfo = @" UPDATE t_User SET userNo=@userNo,userName=@userName,userMail=@userMail,userDept=@userDept," +
                                       " userType=@userType,domainID=@domainID,domainAddr=@domainAddr," +
                                       " usy=@usy,updaterID=@updaterID,uDeptID=@uDeptID,lastModTime=@lastModTime ,defaultRole =@defaultRole" +
                                       " WHERE userID=@userID; ";
        public string ModUserInfo
        {
            get { return _modUserInfo; }
        }

        private string _deleteUserInfo = @" DELETE t_User WHERE userID=@userID; " +
                                          " DELETE t_Right_UCompany WHERE UserId=@UserId; " +
                                          " DELETE t_Right_UPAction WHERE UserId=@UserId; " +
                                          " DELETE t_Right_UPData WHERE UserId=@UserId;" +
                                          " DELETE t_Right_UProgram WHERE UserId=@UserId;" +
                                          " DELETE t_Right_URole WHERE UserId=@UserId;" +
                                          " DELETE t_Right_USupply WHERE UserId=@UserId;";
        public string DeleteUserInfo
        {
            get { return _deleteUserInfo; }
        }

        private string _existsUserId = @" SELECT COUNT(1) FROM t_User WHERE userID=@userID; ";
        public string ExistsUserId
        {
            get { return _existsUserId; }
        }

        private string _isEmployee = @" SELECT COUNT(1) FROM t_Employee WHERE empID=@EmployeeID; ";
        public string IsEmployee
        {
            get { return _isEmployee; }
        }
        private string _isSupply = @" SELECT COUNT(1) FROM t_Supply WHERE suppNumber=@SupplyId; ";
        public string IsSupply
        {
            get { return _isSupply; }
        }
        
        private string _readUserRose = " select a.RoseId,a.RoseName,(CASE ISNULL(b.UserId,'') when '' then 'N' else 'Y'  end) Usy" +
                                         " from t_Right_Rose a left join " +
                                         " t_Right_URole b on a.RoseId=b.RoseId and b.UserId=@UserId where (@UserType='' OR a.SystemType=@UserType) and (@KeyWord='' or( a.RoseId like '%'+@KeyWord+'%')) and a.Usy='Y';";

        public string ReadUserRose
        {
            get { return _readUserRose; }
        }

        private string _deleteUserRose = " DELETE t_Right_URole WHERE UserId=@UserId AND RoseId=@RoseId;";
        public string DeleteUserRose
        {
            get { return _deleteUserRose; }
        }

        private string _addUserRose = " INSERT INTO t_Right_URole(UserId,RoseId)VALUES(@UserId,@RoseId); ";
        public string AddUserRose
        {
            get { return _addUserRose; }
        }

        private string _resetPwd = " Update t_User SET userPwd=@userPwd WHERE userID=@UserId; ";
        public string ResetPwd
        {
            get { return _resetPwd; }
        }

        private string _jointESC = @" select top 10 * from(select empID DisplayId,empName DisplayText,empMail DisplayEmail from t_Employee where '01'=@UserType and usy='Y' and (empID like '%'+@Keyword+'%' or empName like '%'+@Keyword+'%' ) union all " +
                                    " select suppNumber DisplayId,suppName DisplayText,email DisplayEmail from t_Supply where '02'=@UserType and usy='Y' and (suppNumber like '%'+@Keyword+'%' or suppName like '%'+@Keyword+'%') " +
                                    " ) a ";
        public string JointESC
        {
            get { return _jointESC; }
        }


        private string _domainServer = "select * from t_Domain_Server";
        public string DomainServer
        {
            get { return _domainServer; }
        }

        private string _userProgramData = @" select DeptId,IsAll,'01' UserOrRose from t_Right_RData where RoseId in(select RoseId from t_Right_URole where UserId=@UserId) " +
                                           " union all " +
                                           " select DeptId,IsAll,'02' UserOrRose from  t_Right_UPData where UserId=@UserId and ProgramId=@ProgramId; ";

        public string UserProgramData
        {
            get { return _userProgramData; }
        }

    }
}
