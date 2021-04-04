
namespace DMC.DAL.Script.SqlServer
{
    public class CompanyRightSqlScript : ICompanyRight
    {
        private string _addUserCompanyRight = @" INSERT INTO t_Right_UCompany(UserId,CompId)VALUES(@UserId,@CompanyId);";
        public string AddUserCompanyRight
        {
            get { return _addUserCompanyRight; }
        }

        private string _deleteUserCompanyRight = @" DELETE t_Right_UCompany WHERE UserId=@UserId AND CompId=@CompanyId;";
        public string DeleteUserCompanyRight
        {
            get { return _deleteUserCompanyRight; }
        }

        private string _readUserCompanyRight = @" SELECT A.companyID,A.simpleName,ISNULL(b.UserId,'') UserId, (CASE ISNULL(b.UserId,'') WHEN '' THEN 'N' ELSE 'Y' END) Usy " +
                                                  " FROM t_Company A LEFT JOIN " +
                                                       " t_Right_UCompany B on a.companyID=B.CompId and b.UserId=@UserId where a.usy='Y' and compLanguage=@LanguageId ";
        public string ReadUserCompanyRight
        {
            get { return _readUserCompanyRight; }
        }

        private string _readUserCompany = @"select * from t_Right_UCompany where UserId=@UserId;";

        public string ReadUserCompany
        {
            get { return _readUserCompany; }
        }

        private string _addProgramCompanyRight = @" INSERT INTO t_Right_UPCompany(UserId,ProgramId,CompId)VALUES(@UserId,@ProgramId,@CompanyId);";
        public string AddProgramCompanyRight
        {
            get { return _addProgramCompanyRight; }
        }

        private string _deleteProgramCompanyRight = @" DELETE t_Right_UPCompany WHERE UserId=@UserId AND ProgramId=@ProgramId AND CompId=@CompanyId;";
        public string DeleteProgramCompanyRight
        {
            get { return _deleteProgramCompanyRight; }
        }

        private string _readProgramCompanyRight = @" SELECT A.companyID,A.simpleName,ISNULL(b.UserId,'') UserId, (CASE ISNULL(b.UserId,'') WHEN '' THEN 'N' ELSE 'Y' END) Usy " +
                                                  " FROM t_Company A LEFT JOIN " +
                                                       " t_Right_UPCompany B on a.companyID=B.CompId and B.UserId=@UserId and B.ProgramId=@ProgramId  where a.usy='Y'";
        public string ReadProgramCompanyRight
        {
            get { return _readProgramCompanyRight; }
        }

        private string _existsCompanyId = " SELECT COUNT(0) FROM t_Company WHERE companyID=@CompanyId;";
        public string ExistsCompanyId
        {
            get { return _existsCompanyId; }
        }
    }
}
