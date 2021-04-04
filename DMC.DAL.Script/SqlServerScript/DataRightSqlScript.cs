
namespace DMC.DAL.Script.SqlServer
{
    public class DataRightSqlScript : IDataRight
    {
        private string _addProgramDataRight = @" INSERT INTO t_Right_UPData(UserId,ProgramId,DeptId,IsAll) " +
                                               " VALUES(@UserId,@ProgramId,@DeptId,@IsAll); ";
        /// <summary>
        /// 添加用戶程式的資料權限
        /// </summary>
        public string AddProgramDataRight
        {
            get { return _addProgramDataRight; }
        }

        private string _readProgramDataRight = @" SELECT A.deptID,A.falseDeptID,fullName, " +
                                             " (CASE ISNULL(B.UserId,'') WHEN '' THEN 'N' ELSE 'Y' END) Usy, " +
                                             " (CASE ISNULL(B.IsAll,'') WHEN '' THEN 'N' ELSE B.IsAll END) IsAll " +
                                             " FROM (SELECT '0' deptId,'0' falseDeptID,N'所有部門' fullName UNION " +
                                             " SELECT deptID,falseDeptID,simpleName fullName FROM t_Dept WHERE usy='Y') A "+
                                             " LEFT JOIN t_Right_UPData B ON A.deptID=B.DeptId AND B.UserId=@UserId AND B.ProgramId=@ProgramId ";
        //" WHERE (@CompanyId='' OR A.companyID=@CompanyId);";
        /// <summary>
        /// 獲取用戶程式的資料權限
        /// </summary>
        public string ReadProgramDataRight
        {
            get { return _readProgramDataRight; }
        }

        private string _readProgramDataList = @" select * from t_Right_UPData where UserId=@UserId and ProgramId=@ProgramId; ";

        public string ReadProgramDataList
        {
            get { return _readProgramDataList; }
        }

        private string _addRoseDataRight = @" INSERT INTO t_Right_RData(RoseId,DeptId,IsAll) VALUES(@RoseId,@DeptId,@IsAll);";
        /// <summary>
        /// 添加權限類別的資料權限
        /// </summary>
        public string AddRoseDataRight
        {
            get { return _addRoseDataRight; }
        }

        private string _readRoseDataRight = @" SELECT A.deptID,A.falseDeptID,fullName, " +
                                             " (CASE ISNULL(B.RoseID,'') WHEN '' THEN 'N' ELSE 'Y' END) Usy, " +
                                             " (CASE ISNULL(B.IsAll,'') WHEN '' THEN 'N' ELSE B.IsAll END) IsAll " +
                                             " FROM (SELECT '0' deptId,'0' falseDeptID,N'所有部門' fullName UNION " +
                                             " SELECT deptID,falseDeptID,simpleName fullName FROM t_Dept WHERE usy='Y') A "+
                                             " LEFT JOIN t_Right_RData B ON A.deptID=B.DeptId AND B.RoseId=@RoseId ";
        //" WHERE (@CompanyId='' OR A.companyID=@CompanyId);";
        /// <summary>
        /// 獲取權限類別的資料權限
        /// </summary>
        public string ReadRoseDataRight
        {
            get { return _readRoseDataRight; }
        }

        private string _readRoseDataList = @"select * from t_Right_RData where RoseId=@RoseId;";

        public string ReadRightDataList
        {
            get { return _readRoseDataList; }
        }

        private string _existsDeptId = " SELECT COUNT(1) FROM t_Dept B WHERE B.deptID=@DeptId; ";
        /// <summary>
        /// 验证部门是否存在
        /// </summary>
        public string ExistsDeptId
        {
            get { return _existsDeptId; }
        }

        private string _deleteRoseDataRight = @"DELETE t_Right_RData WHERE RoseId=@RoseId AND DeptId=@DeptId; ";
        public string DeleteRoseDataRight
        {
            get { return _deleteRoseDataRight; }
        }

        private string _updateRoseDataRight = @" UPDATE t_Right_RData SET IsAll=@IsAll WHERE RoseId=@RoseId AND DeptId=@DeptId; ";
        public string UpdateRoseDataRight
        {
            get { return _updateRoseDataRight; }
        }

        private string _deleteProgramDataRight = @" DELETE t_Right_UPData WHERE UserId=@UserId AND ProgramId=@ProgramId AND DeptId=@DeptId; ";
        public string DeleteProgramDataRight
        {
            get { return _deleteProgramDataRight; }
        }

        private string _updateProgramDataRight = @" UPDATE t_Right_UPData SET IsAll=@IsAll WHERE UserId=@UserId AND ProgramId=@ProgramId AND DeptId=@DeptId; ";
        public string UpdateProgramDataRight
        {
            get { return _updateProgramDataRight; }
        }
    }
}
