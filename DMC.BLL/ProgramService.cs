using System.Collections.Generic;
using DMC.DAL;
using DMC.Model;
using System.Data;
namespace DMC.BLL {
	/// <summary>
	/// 程式業務操作類
	/// code by jeven_xiao
	/// 2013-6-9
	/// </summary>
	public class ProgramService {
		private ProgramDAL pdal = new ProgramDAL();
		private PageManage pageView = new PageManage();

		/// <summary>
		/// 增加新的程式
		/// </summary>
		/// <param name="prog">程式实体</param>
		/// <returns>bool</returns>
		public string AddProgram(t_Program prog) {
			if (string.IsNullOrEmpty(prog.programID))
				return "EB0025";
			if (string.IsNullOrEmpty(prog.programName))
				return "EB0026";
			if (IsExitProg(prog.programID))
				return "EB0007";
			if (!IsExitProFunc(prog.programID))
				return "EB0009";//未設置程式的基本功能

			if (pdal.AddProgram(prog) == true) {
				return "SB0011";
			}
			else {
				return "UB0002";
			}


		}

		/// <summary>
		/// 更新程式与菜单的关联
		/// </summary>
		/// <param name="programID">程式編號</param>
		/// <param name="menuID">菜單編號</param>
		/// <returns>string</returns>
		public string ModProgMenu(string programID, string menuID) {
			if (string.IsNullOrEmpty(programID))
				return "EB0025";
			if (pdal.ModProgMenu(programID, menuID) == true)
				return "SB0015";
			else
				return "UB0003";

		}

		/// <summary>
		/// 设置程式与菜单的关联
		/// </summary>
		/// <param name="pm">實體對象</param>
		/// <returns>string</returns>
		public string AddProgMenu(t_ProgRefMenu pm) {
			if (pdal.AddProgMenu(pm) == true)
				return "SB0015";
			else
				return "UB0002";

		}

		/// <summary>
		/// 增加程式的基本执行功能
		/// </summary>
		/// <param name="funcIDList">功能編號ID</param>
		/// <param name="programID">程式編號</param>
		/// <returns>string</returns>
		public string SetProgFunc(List<string> funcIDList, string programID) {
			if (string.IsNullOrEmpty(programID))
				return "EB0025";//程式編號不能為空
			if (funcIDList.Count < 1 || funcIDList == null)
				return "EB0031";//未設置程式的基本功能編號

			if (pdal.SetProgFunc(funcIDList, programID) == true)
				return "SB0013";
			else
				return "UB0002";

		}

		/// <summary>
		/// 修改程式资料信息
		/// </summary>
		/// <param name="prog">程式實體</param>
		/// <returns>string</returns>
		public string ModProgram(t_Program prog) {
			if (string.IsNullOrEmpty(prog.programID))
				return "EB0025";
			if (string.IsNullOrEmpty(prog.programName))
				return "EB0026";
			if (!IsExitProFunc(prog.programID))
				return "EB0009";//未設置程式的基本功能
			if (IsExitProg(prog.programID)) {
				if (pdal.ModProgram(prog) == true) {
					return "SB0011";

				}
				else {
					return "UB0003";
				}
			}
			else {
				return "EB0013";
			}

		}

		/// <summary>
		/// 取得所有的程式列表
		/// </summary>
		/// <param name="pageSize"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageCount"></param>
		/// <param name="total"></param>
		/// <param name="Where"></param>
		/// <returns></returns>
		public DataTable GetAllProg(int pageSize, int pageIndex, out int pageCount, out int total, string Where = "") {
			string ShowField = "programID,programName,menuUrl,functionStr,usy,createrID,createrID as creater,cDeptID,cDeptID as cDeptName,createTime,updaterID,updaterID as moder,uDeptID,uDeptID as uDeptName,lastModTime,menuId,orderid,ismobile,mobileurl";
			return pageView.PageView("t_Program", "AutoId", pageIndex, pageSize, ShowField, "AutoId ASC", Where, out total, out pageCount);
		}

		/// <summary>
		/// 验证程式编号是否存在
		/// </summary>
		/// <returns></returns>
		public bool IsExitProg(string programID) {
			if (string.IsNullOrEmpty(programID))
				return false;
			return pdal.IsExitProg(programID);

		}
		/// <summary>
		/// 验证是否存在程式的基本功能
		/// </summary>
		/// <param name="programID"></param>
		/// <returns></returns>
		public bool IsExitProFunc(string programID) {
			if (string.IsNullOrEmpty(programID))
				return false;
			return pdal.IsExitProFunc(programID);
		}

		/// <summary>
		/// 验证该条程式是否设置菜单关联
		/// </summary>
		/// <param name="programID"></param>
		/// <param name="menuID"></param>
		/// <returns></returns>
		public bool IsExitProgVsMenu(string programID) {
			if (string.IsNullOrEmpty(programID))
				return false;

			return pdal.IsExitProgVsMenu(programID);
		}


		/// <summary>
		/// 删除某一只程式相关的基本执行功能
		/// </summary>
		/// <param name="idList">程式ID</param>
		/// <returns></returns>
		public string DelProgFunc(string programID) {
			if (string.IsNullOrEmpty(programID))
				return "EB0013";
			if (pdal.DelProgFunc(programID) == true)
				return "SB0016";
			else
				return "UB0004";
		}

		/// <summary>
		/// 删除程式
		/// </summary>
		/// <param name="idlist">程式ID</param>
		/// <returns>bool</returns>
		public string DelProgram(List<string> idlist) {
			if (idlist.Count < 1 || idlist == null)
				return "EB0008";
			if (pdal.DelProgram(idlist) == true)
				return "SB0012";
			else
				return "UB0004";
		}

		/// <summary>
		/// 取得程式的基本功能列表，从多语言配置中
		/// </summary>
		/// <param name="languageID"></param>
		/// <param name="programID"></param>
		/// <returns></returns>
		public DataTable ProgramFuncList(string languageID, string programID) {

			return pdal.ProgramFuncList(languageID, programID);

		}

		/// <summary>
		/// 獲取程式編號列表
		/// </summary>
		/// <returns></returns>
		public DataTable GetProgramIdList() {
			return pdal.GetProgramIdList();
		}
	}
}
