using System;
using System.Collections.Generic;
using System.Linq;
using DMC.DAL;
using System.Data;

namespace DMC.BLL
{
	/// <summary>
	/// 權限管理處理業務數據
	/// code by klint
	/// </summary>
    public class DataManageService
    {
        private DataManageDAL dataManage = new DataManageDAL();
        private RoseManageDAL roseManage = new RoseManageDAL();
        private ProgramManageDAL programManage = new ProgramManageDAL();
        private UserRightManageDAL userManage = new UserRightManageDAL();
        /// <summary>
        /// 獲取權限類別的資料權限
        /// </summary>
        /// <param name="RoseId"></param>
        /// <returns></returns>
        public DataTable ReadDataByRose(string RoseId)
        {
            return dataManage.ReadDataByRose(RoseId);
        }
		/// <summary>
		/// 獲取權限類別的資料權限
		/// </summary>
		/// <param name="RoseId"></param>
		/// <returns></returns>
        public DataTable ReadDataRightByRose(string RoseId)
        {
            return dataManage.ReadDataRightByRose(RoseId);
        }
		/// <summary>
		/// 部門狀態
		/// </summary>
        public class DeptIdStatus
        {
            public string DeptId { get; set; }
            public string IsAll { get; set; }
        }

        /// <summary>
        /// 權限類別的資料權限保存
        /// </summary>
        /// <param name="RoseId">權限編號</param>
        /// <param name="DeptIdAndStatusList">部門和狀態列表</param>
        /// <returns>
        /// ER0001：權限類別編號設置錯誤
        /// ER0019：部門設置錯誤
        /// ER0020：部門不存在
        /// SR0008：資料權限添加成功
        /// UD0001：數據庫操作產生異常
        /// </returns>
        public string SaveDataByRose(string RoseId, string DeptIdAndStatusList)
        {
            string result = string.Empty;
            if (string.IsNullOrEmpty(RoseId))
                result = "ER0001";
            if (string.IsNullOrEmpty(result) && !roseManage.ExistsRoseId(RoseId))
                result = "ER0007";
            //組合當前數據
            List<DeptIdStatus> currentList = new List<DeptIdStatus>();
            List<string> items = DeptIdAndStatusList.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (string item in items)
            {
                if (string.IsNullOrEmpty(item) && item.IndexOf("|") <= 0)
                {
                    result = "ER0019";
                    break;
                }
                DeptIdStatus dis = new DeptIdStatus();
                dis.DeptId = item.Substring(0, item.IndexOf("|"));
                dis.IsAll = item.Substring(item.Length - 1, 1);
                currentList.Add(dis);
            }
            //獲取歷史數據
            List<DeptIdStatus> historyList = new List<DeptIdStatus>();
            DataTable dt = ReadDataRightByRose(RoseId);
            foreach (DataRow dr in dt.Rows)
            {
                DeptIdStatus dis = new DeptIdStatus();
                dis.DeptId = dr["DeptID"].ToString();
                dis.IsAll = dr["IsAll"].ToString();
                historyList.Add(dis);
            }

            //計算需要新增的資料權限
            List<DeptIdStatus> newList = currentList.FindAll(o => !historyList.Exists(h => h.DeptId == o.DeptId));     //新增
            List<DeptIdStatus> deleteList = historyList.FindAll(o => !currentList.Exists(h => h.DeptId == o.DeptId));  //刪除
            List<DeptIdStatus> updateList = currentList.FindAll(o => !newList.Exists(h => h.DeptId == o.DeptId));  //修改

            if (string.IsNullOrEmpty(result))
            {
                using (Trans t = new Trans())
                {
                    try
                    {
                        bool isResult = true;
                        foreach (DeptIdStatus item in deleteList)
                        {
                            isResult = dataManage.DeleteRoseDataRight(RoseId, item.DeptId, t);
                            if (!isResult)
                            {
                                result = "ER0020";
                                break;
                            }
                        }
                        if (isResult)
                        {
                            foreach (DeptIdStatus item in updateList)
                            {
                                isResult = dataManage.UpdateDataByRose(RoseId, item.DeptId, item.IsAll, t);
                                if (!isResult)
                                {
                                    result = "ER0022";
                                    break;
                                }
                            }
                        }

                        if (isResult)
                        {

                            foreach (DeptIdStatus item in newList)
                            {
                               
                                if (!dataManage.SaveDataByRose(RoseId, item.DeptId, item.IsAll, t))
                                {
                                    result = "ER0023";
                                    isResult = false;
                                    break;
                                }
                            }
                        }

                        if (!isResult)
                        {
                            t.RollBack();
                            if (string.IsNullOrEmpty(result))
                                result = "ER0024";
                        }
                        else
                        {
                            t.Commit();
                            result = "success";
                        }
                    }
                    catch
                    {
                        if (string.IsNullOrEmpty(result))
                            result = "newLabel050";
                        t.RollBack();
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 獲取用戶程式的資料權限
        /// </summary>
        /// <param name="UserId">用戶編號</param>
        /// <param name="ProgramId">程式編號</param>
        /// <param name="CompanyId">公司編號</param>
        /// <returns></returns>
        public DataTable ReadDataByUserAndProgram(string UserId, string ProgramId)
        {
            return dataManage.ReadDataByUserAndProgram(UserId, ProgramId);
        }
		/// <summary>
		/// 獲取用戶程式的資料權限
		/// </summary>
		/// <param name="UserId">用戶編號</param>
		/// <param name="ProgramId">程式編號</param>
		/// <param name="CompanyId">公司編號</param>
		/// <returns></returns>
        public DataTable ReadProgramDataList(string UserId, string ProgramId)
        {
            return dataManage.ReadProgramDataList(UserId, ProgramId);
        }

        /// <summary>
        /// 用戶程式的資料權限保存
        /// </summary>
        /// <param name="UserId">用戶編號</param>
        /// <param name="ProgramId">程式編號</param>
        /// <param name="DeptIdAndStatusList">部門和狀態列表</param>
        /// <returns>
        /// EU0008：用戶編號設置錯誤
        /// ER0012：程式編號設置錯誤
        /// ER0019：部門設置錯誤
        /// ER0020：部門不存在
        /// SR0008：資料權限添加成功
        /// UD0001：數據庫操作產生異常
        /// </returns>
        public string SaveDataByUserAndProgram(string UserId, string ProgramId, string DeptIdAndStatusList)
        {
            string result = string.Empty;
            if (string.IsNullOrEmpty(UserId))
                result = "ER0001";
            //检查用户是否存在--待处理
            if (string.IsNullOrEmpty(result) && !userManage.ExistsUserId(UserId))
                result = "ER0007";
            if (string.IsNullOrEmpty(result) && string.IsNullOrEmpty(ProgramId))
                result = "ER0012";
            if (!programManage.ExistsProgramId(ProgramId))
                result = "ER0012";

            //組合當前數據
            List<DeptIdStatus> currentList = new List<DeptIdStatus>();
            List<string> items = DeptIdAndStatusList.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (string item in items)
            {
                if (string.IsNullOrEmpty(item) && item.IndexOf("|") <= 0)
                {
                    result = "ER0019";
                    break;
                }
                DeptIdStatus dis = new DeptIdStatus();
                dis.DeptId = item.Substring(0, item.IndexOf("|"));
                dis.IsAll = item.Substring(item.Length - 1, 1);
                currentList.Add(dis);
            }

            //獲取歷史數據
            List<DeptIdStatus> historyList = new List<DeptIdStatus>();
            DataTable dt = ReadProgramDataList(UserId, ProgramId);
            foreach (DataRow dr in dt.Rows)
            {
                DeptIdStatus dis = new DeptIdStatus();
                dis.DeptId = dr["deptID"].ToString();
                dis.IsAll = dr["IsAll"].ToString();
                historyList.Add(dis);
            }

            //計算需要新增的資料權限
            List<DeptIdStatus> newList = currentList.FindAll(o => !historyList.Exists(h => h.DeptId == o.DeptId));     //新增
            List<DeptIdStatus> deleteList = historyList.FindAll(o => !currentList.Exists(h => h.DeptId == o.DeptId));  //刪除
            List<DeptIdStatus> updateList = currentList.FindAll(o => !newList.Exists(h => h.DeptId == o.DeptId));  //修改

            if (string.IsNullOrEmpty(result))
            {
                using (Trans t = new Trans())
                {
                    try
                    {
                        bool isResult = true;
                        foreach (DeptIdStatus item in deleteList)
                        {
                            isResult = dataManage.DeleteProgramDataRight(UserId, ProgramId, item.DeptId, t);
                            if (!isResult)
                            {
                                result = "ER0025";
                                break;
                            }
                        }
                        if (isResult)
                        {
                            foreach (DeptIdStatus item in updateList)
                            {
                                isResult = dataManage.UpdateDataByUserAndProgram(UserId, ProgramId, item.DeptId, item.IsAll, t);
                                if (!isResult)
                                {
                                    result = "ER0026";
                                    break;
                                }
                            }
                        }

                        if (isResult)
                        {

                            foreach (DeptIdStatus item in newList)
                            {
                                //检查部门设置是否错误
                                //if (!dataManage.ExistsDeptId(item.DeptId, t))
                                //{
                                //    result = "ER0020";
                                //    isResult = false;
                                //    break;
                                //}
                                if (!dataManage.SaveDataByUserAndProgram(UserId, ProgramId, item.DeptId, item.IsAll, t))
                                {
                                    result = "ER0027";
                                    isResult = false;
                                    break;
                                }
                            }
                        }

                        if (!isResult)
                        {
                            t.RollBack();
                            if (string.IsNullOrEmpty(result))
                                result = "ER0027";
                        }
                        else
                        {
                            t.Commit();
                            result = "success";
                        }
                    }
                    catch
                    {
                        if (string.IsNullOrEmpty(result))
                            result = "newLabel051";
                        t.RollBack();
                    }
                }
            }
            return result;
        }
    }
}
