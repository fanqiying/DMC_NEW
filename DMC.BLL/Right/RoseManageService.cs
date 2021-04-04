using System;
using System.Collections.Generic;
using System.Linq;
using DMC.DAL;
using System.Data;
using DMC.Model;

namespace DMC.BLL
{
	/// <summary>
	/// 權限管理-角色業務操作類
	/// 封裝權限角色的所有業務方法操作
	/// code by klint 2013-6-13
	/// </summary>
    public class RoseManageService
    {
        private RoseManageDAL roseManage = new RoseManageDAL();
        private PageManage pageView = new PageManage();
        /// <summary>
        /// 搜索角色列表
        /// </summary>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">第幾页</param>
        /// <param name="pageCount">總页數</param>
        /// <param name="total">總條數</param>
        /// <param name="Where">搜索條件</param>
        /// <returns></returns>
        public DataTable Search(int pageSize, int pageIndex, out int pageCount, out int total, string Where = "")
        {
            string showfiled = "AutoID,RoseId,RoseName,SystemType,Usy,CreateUserId as createrid,CreateTime,";
            showfiled += "CreateDeptId as cdeptid,UpdateUserId as updaterid,UpdateDeptId as udeptid,UpdateTime as lastmodtime";
            return pageView.PageView("t_Right_Rose", "RoseId", pageIndex, pageSize, showfiled, "AutoId ASC", Where, out total, out pageCount);
        }
		/// <summary>
		/// 獲取權限類別集合
		/// </summary>
		/// <returns></returns>
        public DataTable GetAllType()
        {
            return roseManage.GetAllRoseType();
        }

        /// <summary>
        /// 獲取角色的用戶明細
        /// </summary>
        /// <param name="RoseId"></param>
        /// <returns></returns>
        public DataTable RoseUserInfo(string RoseId)
        {
            return roseManage.RoseUserInfo(RoseId);
        }

        private List<string> usyList = new List<string>() { "Y", "N" };
        /// <summary>
        /// 角色添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string AddRose(t_Right_Rose model)
        {
            if (string.IsNullOrEmpty(model.RoseId))
                return "ER0001";//權限類別編號設置錯誤
            if (string.IsNullOrEmpty(model.RoseName))
                return "ER0002";//權限類別名稱設置錯誤
            if (string.IsNullOrEmpty(model.SystemType))
                return "ER0003";//使用者類別設置錯誤
            //當有效否未設置或設置錯誤時，主動糾正
            if (string.IsNullOrEmpty(model.Usy) || !usyList.Contains(model.Usy))
                model.Usy = "Y";

            if (ExistsRoseId(model.RoseId))
                return "ER0006";//權限類別編號已存在

            if (ExistsRoseName(model.RoseName))
                return "ER0021";//權限類別名稱已存在

            if (roseManage.AddRose(model))
            {
                return "success";
            }
            else
            {
                return "UB0031";
            }
        }

        /// <summary>
        /// 驗證角色是否存在
        /// </summary>
        /// <param name="RoseId"></param>
        /// <returns></returns>
        public bool ExistsRoseId(string RoseId)
        {
            return roseManage.ExistsRoseId(RoseId);
        }

        /// <summary>
        /// 檢查權限類別名稱是否存在
        /// </summary>
        /// <param name="RoseName"></param>
        /// <returns></returns>
        public bool ExistsRoseName(string RoseName, string RoseId = "")
        {
            return roseManage.ExistsRoseName(RoseName, RoseId);
        }

        /// <summary>
        /// 驗證權限是否被使用
        /// </summary>
        /// <param name="RoseIdList"></param>
        /// <returns></returns>
        public bool RightIsUsing(string RoseIdList)
        {
            return roseManage.RightIsUsing(RoseIdList);
        }
        /// <summary>
        /// 驗證權限是否被使用,并返回id
        /// </summary>
        /// <param name="RoseIdList"></param>
        /// <returns></returns>
        public string RightIsUsingId(string RoseIdList)
        {
            return roseManage.RightIsUsingId(RoseIdList);
        }
        /// <summary>
        /// 權限類別修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string ModRose(t_Right_Rose model)
        {
            if (string.IsNullOrEmpty(model.RoseId))
                return "ER0001";//權限類別編號設置錯誤
            if (string.IsNullOrEmpty(model.RoseName))
                return "ER0002";//權限類別名稱設置錯誤
            if (string.IsNullOrEmpty(model.SystemType))
                return "ER0003";//使用者類別設置錯誤
            //當有效否未設置或設置錯誤時，主動糾正
            if (string.IsNullOrEmpty(model.Usy) || !usyList.Contains(model.Usy))
                model.Usy = "Y";
            if (RightIsUsing(model.RoseId ))
                return "ER0008";//權限編號處於使用中
            if (!ExistsRoseId(model.RoseId))
                return "ER0007";//權限類別編號不存在

            if (ExistsRoseName(model.RoseName, model.RoseId))
                return "ER0021";//權限類別名稱已存在

            if (roseManage.ModRose(model))
            {
                return "success";
            }
            else
            {
                return "ER0031";
            }
        }

        /// <summary>
        /// 刪除角色類別
        /// </summary>
        /// <param name="roseIdList"></param>
        /// <returns></returns>
        public string DelRose(string keyIdLists)
        {
            if (string.IsNullOrEmpty(keyIdLists))
                return "EL0005";//請選擇需要刪除的列

            //驗證權限類別是否處於使用中
            if (RightIsUsing(keyIdLists))
                return "ER0008";//權限編號處於使用中

            List<string> keyIdList = keyIdLists.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

            if (roseManage.DelRose(keyIdList))
            {
                return "success";//刪除成功
            }
            else
            {
                return "DT006";//刪除失敗
            }
        }

        /// <summary>
        /// 權限複製
        /// </summary>
        /// <param name="roseIdAim">目的權限編號</param>
        /// <param name="roseIdSocure">來源權限編號</param>
        /// <returns></returns>
        public string CopyRose(string roseIdAim, string roseIdSocure)
        {
            if (string.IsNullOrEmpty(roseIdAim) || !ExistsRoseId(roseIdAim))
                return "ER0009";//目標權限設置錯誤
            if (string.IsNullOrEmpty(roseIdSocure) || !ExistsRoseId(roseIdSocure))
                return "ER0010";//來源權限設置錯誤

            if (RoseExitsRight(roseIdAim))
                return "ER0011";

            if (roseManage.CopyRose(roseIdAim, roseIdSocure))
            {
                return "success";
            }
            else
            {
                return "ER0032";
            }
        }
        /// <summary>
        /// 驗證是否存在授權
        /// </summary>
        /// <param name="RoseId"></param>
        /// <returns></returns>
        public bool RoseExitsRight(string RoseId)
        {
            return roseManage.RoseExitsRight(RoseId);
        }
    }
}
