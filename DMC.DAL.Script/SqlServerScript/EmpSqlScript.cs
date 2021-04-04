using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DMC.DAL.Script;

namespace DMC.DAL.Script.SqlServer
{
    public class EmpSqlScript : IEmplyee
    {
        /// <summary>
        /// 增加员工
        /// </summary>
        private string _addEmp = " Insert into t_Employee(empID,companyID,empName,empDept,empTitle,extTelNo," +
            "empMail,signerID,usy,createrID,cDeptID,createTime)values (@empID,@companyID ," +
            "@empName,@empDept,@empTitle,@extTelNo,@empMail,@signerID,@usy ,@createrID ,@cDeptID,@createTime)";
        /// <summary>
        /// 修改员工资料
        /// </summary>
        private string _modEmp = "update t_Employee set companyID =@companyID ,empName=@empName," +
           "empDept=@empDept,empTitle=@empTitle,extTelNo=@extTelNo,empMail=@empMail," +
           "signerID=@signerID,usy=@usy,updaterID=@updaterID,uDeptID=@uDeptID," +
           "lastModTime=@modTime where empID =@empID; "+
           "update t_user set userMail=@empMail,userName=@empName where userNo=@empID and userType='01';";
        /// <summary>
        /// 删除员工资料
        /// </summary>
        private string _delEmp = "delete from t_Employee  where empID=@idList";
        /// <summary>
        /// 验证员工资料是否存在？
        /// </summary>
        private string _isExitEmp = "select empID from t_Employee where empID=@empID";
        private string _GetEmpList = @"select a.empID,a.companyID,a.empName,
a.empDept,a.empTitle,a.extTelNo,a.empMail,
a.signerID,a.usy,a.createrID,a.cDeptID,a.createTime ,b.simpleName
from t_Employee as a left join t_Dept as b  on a.empdept=b.deptID    where 1=1  ";
        public string GetEmpList
        {
            get { return _GetEmpList; }
        }



		private string _GetEmpInfoByID = @"select a.empID as employeeid,a.companyID,a.empName as name,
a.empDept as deptid,a.empTitle,a.extTelNo,a.empMail,
a.signerID,a.usy,a.createrID,a.cDeptID,a.createTime ,b.simpleName
from t_Employee as a left join t_Dept as b  on a.empdept=b.deptID    where 1=1  ";
		public string GetEmpInfoByID { get { return _GetEmpInfoByID; } }

        public string AddEmp
        {
            get { return _addEmp; }
        }

        public string ModEmp
        {
            get { return _modEmp; }
        }

        public string DelEmp
        {
            get { return _delEmp; }
        }

        public string IsExitEmp
        {
            get { return _isExitEmp; }
        } 
        private string _getExistsEmailEmp = @"select empMail+'('+empName+'/'+empID+')' displaytext,empID displayvalue " +
                                              " from t_Employee " +
                                             " where empMail like'%@%' ";
        public string GetExistsEmailEmp
        {
            get { return _getExistsEmailEmp; }
        }

        private string _updateEmpEmailInfo = @" update t_Employee set empName=@empName,empMail=@empMail where empID=@empID;" +
                                              " update t_User set userName=@empName,userMail=@empMail where userNo=@empID and userType='01';";
        private string _getAllEmpToDT = "SELECT * FROM t_Employee where usy='Y';";


        private string _isExitEmpByDeptID = "select COUNT (*) from t_Employee  where empDept =@deptID";
        public string UpdateEmpEmailInfo
        {
            get { return _updateEmpEmailInfo; }
        }

        

        public string GetAllEmpToDT
        {
            get { return _getAllEmpToDT; }
        }
   

        public string IsExitEmpByDeptID
        {
            get { return _isExitEmpByDeptID; }
        }

        private string _ReadEmpTel = "SELECT a.userID,b.extTelNo tel FROM t_User a left join t_Employee b on a.userNo=b.empID where a.userID in(@Create,@Update) and a.userType='01'";
        /// <summary>
        /// 獲取員工的分機號碼
        /// </summary>
        public string ReadEmpTel
        {
            get { return _ReadEmpTel; }
        }
    }
}
