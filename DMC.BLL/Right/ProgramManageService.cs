using System;
using System.Collections.Generic;
using System.Linq;
using DMC.DAL;
using System.Data;

namespace DMC.BLL
{
	/// <summary>
	/// 系統程式權限業務操作類
	/// 封裝系統程式操作的所有業務邏輯方法
	/// code by klint 2013-6-12
	/// </summary>
    public class ProgramManageService
    {
        private RoseManageDAL roseManage = new RoseManageDAL();
        private UserManageDAL userManage = new UserManageDAL();
        private ProgramManageDAL programManage = new ProgramManageDAL();
        /// <summary>
        /// 程式權限搜索
        /// </summary>
        /// <param name="isUser">是否為用戶</param>
        /// <param name="IdKey">用戶編號或權限類別編號</param>
        /// <param name="KeyWord">關鍵字</param>
        /// <param name="LangId">語言別</param>
        /// <returns>返回已授權的程式列表</returns>
        public DataTable SearchProgram(bool isUser, string IdKey, string KeyWord, string LangId)
        {
            return programManage.SearchProgram(isUser, IdKey, KeyWord, LangId);
        }

        /// <summary>
        /// 程式權限添加
        /// </summary>
        /// <param name="isUser">是否為用戶</param>
        /// <param name="IdKey">用戶編號或權限類別編號</param>
        /// <param name="ProgramIdList">程式編號列表</param>
        /// <returns>
        /// SR0005：程式權限添加成功
        /// </returns>
        public string AddProgram(bool isUser, string IdKey, string ProgramIdList)
        {
            if (string.IsNullOrEmpty(ProgramIdList))
                return "ER0013";

            List<string> programList = ProgramIdList.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (programList == null || programList.Count <= 0)
                return "ER0013";

            if (isUser)
            {
                //设置不能为空
                if (string.IsNullOrEmpty(IdKey))
                    return "EU0008";
                //检查用户是否存在
                if (!userManage.IsExitUserID(IdKey))
                    return "EU0008";
            }
            else
            {
                if (string.IsNullOrEmpty(IdKey))
                    return "ER0001";
                //检查权限类别是否存在
                if (!roseManage.ExistsRoseId(IdKey))
                    return "ER0007";
            }

            if (programManage.AddProgram(isUser, IdKey, programList))
            {
                return "success";
            }
            else
            {
                return "fail";
            }
        }

        /// <summary>
        /// 程式權限刪除
        /// </summary>
        /// <param name="isUser">是否為用戶</param>
        /// <param name="IdKey">用戶編號或權限類別編號</param>
        /// <param name="ProgramId">程式編號</param>
        /// <returns>
        /// SR0004：刪除程式權限成功
        /// </returns>
        public string DelProgram(bool isUser, string IdKey, List<string> ProgramIdList)
        {
            if (ProgramIdList == null || ProgramIdList.Count <= 0)
                return "ER0012";

            if (isUser)
            {
                //设置不能为空
                if (string.IsNullOrEmpty(IdKey) || !userManage.IsExitUserID(IdKey))
                    return "EU0008";

            }
            else
            {
                if (string.IsNullOrEmpty(IdKey))
                    return "ER0001";
                //检查权限类别是否存在
                if (!roseManage.ExistsRoseId(IdKey))
                    return "ER0007";
            }

            if (programManage.DelProgram(isUser, IdKey, ProgramIdList))
            {
                return "success";
            }
            else
            {
                return "fail";
            }
        }

        /// <summary>
        /// 獲取程式的執行權限
        /// </summary>
        /// <param name="isUser">是否為用戶</param>
        /// <param name="IdKey">用戶編號或權限類別編號</param>
        /// <param name="ProgramId">程式編號</param>
        /// <param name="LangId">語言別</param>
        /// <returns>返回操作的狀態</returns>
        public DataTable ReadProgramAction(bool isUser, string IdKey, string ProgramId, string LangId)
        {
            return programManage.ReadProgramAction(isUser, IdKey, ProgramId, LangId);
        }

        /// <summary>
        /// 程式操作權限保存
        /// </summary>
        /// <param name="isUser">是否為用戶</param>
        /// <param name="IdKey">用戶編號或權限類別編號</param>
        /// <param name="ProgramId">程式編號</param>
        /// <param name="ActionIdList">操作列表</param>
        /// <returns></returns>
        public string SaveProgramAction(bool isUser, string IdKey, string ProgramId, List<string> ActionIdList)
        {
            //if (ActionIdList == null || ActionIdList.Count <= 0)
            //    return "ER0013";
            if (string.IsNullOrEmpty(ProgramId))
                return "ER0012";//程式编号设置错误
            if (!programManage.ExistsProgramId(ProgramId))
                return "EU0011"; //程式编号不存在
            if (isUser)
            {
                if (string.IsNullOrEmpty(IdKey) || !userManage.IsExitUserID(IdKey))
                    return "EU0008";
            }
            else
            {
                if (string.IsNullOrEmpty(IdKey))
                    return "ER0001";
                if (!roseManage.ExistsRoseId(IdKey))
                    return "ER0007";
            }

            if (programManage.SaveProgramAction(isUser, IdKey, ProgramId, ActionIdList))
            {
                return "success";
            }
            else
            {
                return "fail";
            }
        }
    }
}
