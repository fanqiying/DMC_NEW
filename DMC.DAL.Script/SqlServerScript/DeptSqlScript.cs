
namespace DMC.DAL.Script.SqlServer
{
    public class DeptSqlScript : IDept
    {
        /// <summary>
        /// 增加新的部門
        /// </summary>
        public string AddDept
        {
            get { return _addDept; }
        }

        /// <summary>
        /// 刪除部門資料
        /// </summary>
        public string DelDept
        {
            get { return _delDept; }
        }

        /// <summary>
        /// 修改部門資料信息
        /// </summary>
        public string ModDept
        {
            get { return _modDept; }
        }

        //驗證部門資料是否存在
        public string IsExitDept
        {
            get { return _isExitDept; }
        } 
        /// 取得所有部门的DATATABLE信息
        public string GetAllDeptToDT
        {
            get { return _getAllDeptToDT; }
        }

        /// <summary>
        /// 根据部门ID取得部门主管
        /// </summary>
        public string GetDeptHeaderByID {
            get { return _getDeptHeaderByID; }
        }

        /// <summary>
        /// 取得部门的性质
        /// </summary>
        string IDept.GetDeptNature
        {
            get { return _getDeptNature; }
        }
        /// <summary>
        /// 增加新部門的數據腳本
        /// </summary>
        private string _addDept = "insert into t_Dept(deptID,companyID,falseDeptID,simpleName,fullName," +
                 "deptNature,deptGroup,deptHeader,usy,createrID,cDeptID,createTime)" +
                 "values(@deptID,@companyID,@falseDeptID,@simpleName,@fullName," +
                 "@deptNature ,@deptGroup,@deptHeader,@usy,@createrID,@cDeptID,@createTime)";

        /// <summary>
        /// 刪除指定的部門資料信息
        /// </summary>
        private string _delDept = " delete from t_Dept  where deptID  in (@idList)";

       /// <summary>
       /// 根據部門ID更新部門資料信息 腳本
       /// </summary>
        private string _modDept = "update t_Dept set companyID=@companyID,falseDeptID=@falseDeptID ," +
                "simpleName=@simpleName,fullName=@fullName ,deptNature=@deptNature," +
                "deptGroup=@deptGroup,deptHeader=@deptHeader,usy=@usy,updaterID=@updaterID ," +
                "uDeptID=@uDeptID ,lastModTime=@modTime where deptID =@deptID ";

        /// <summary>
        /// 驗證部門資料是否存在的數據腳本
        /// </summary>
        private string _isExitDept = "select deptID from t_Dept where deptID=@deptID";
 
        /// <summary>
        /// 取得所有部门的DATATABLE信息
        /// </summary>
        private string _getAllDeptToDT = "select * from t_dept where usy='Y' ";
        /// <summary>
        /// 取部门主管
        /// </summary>
        private string _getDeptHeaderByID = "select deptHeader from t_Dept   where deptID=@deptID";
        /// <summary>
        ///  取部门性质
        /// </summary>
        private string _getDeptNature = "select deptNature  from t_Dept where deptID =@deptID";


      
    }
}
