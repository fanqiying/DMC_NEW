
namespace DMC.DAL.Script
{
    public interface IEmplyee
    {
        /// <summary>
        /// 增加员工
        /// </summary>
        string AddEmp { get; }
        /// <summary>
        /// 跟心员工资料
        /// </summary>
        string ModEmp { get; }
        /// <summary>
        /// 删除员工资料
        /// </summary>
        string DelEmp { get; }
        /// <summary>
        /// 验证员工资料是否存在？
        /// </summary>
        string IsExitEmp { get; }
        /// <summary>
        /// 獲取有郵箱的員工帳號
        /// </summary>
        string GetExistsEmailEmp { get; }
        /// <summary>
        /// 更新員工表中的員工郵箱信息
        /// </summary>
        string UpdateEmpEmailInfo { get; }


        string GetAllEmpToDT { get; }

        /// <summary>
        /// 验证某一部门下是否存在员工
        /// </summary>
        string IsExitEmpByDeptID { get; }
        /// <summary>
        /// 獲取電話
        /// </summary>
        string ReadEmpTel { get; }
        /// <summary>
        /// 根據條件獲取記錄
        /// </summary>
        string GetEmpList { get; }

		/// <summary>
		/// 取员工的详细信息根据员工的编号
		/// </summary>
		string GetEmpInfoByID { get; }
    }
}
