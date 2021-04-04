using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using DMC.DAL;

namespace DMC.BLL
{
	/// <summary>
	/// 供應商業務操作類
	/// code by jeven
	/// 2013-9-25
	/// </summary>
    public class SupplyManageService
    {
        private SupplyManageDAL supplyManage = new SupplyManageDAL();
        private RoseManageDAL roseManage = new RoseManageDAL();
        /// <summary>
        /// 獲取對應角色的供應商權限
        /// </summary>
        /// <param name="RoseId">權限編號</param>
        /// <returns></returns>
        public DataTable ReadSupplyByRose(string RoseId)
        {
            return supplyManage.ReadSupplyByRose(RoseId);
        }

        /// <summary>
        /// 權限類別供應商權限保存
        /// </summary>
        /// <param name="RoseId">權限編號</param>
        /// <param name="supplyIdList">供應商列表</param>
        /// <returns>
        /// ER0001：權限類別編號設置錯誤
        /// ER0007：權限類別編號不存在
        /// ER0017：供應商設置錯誤
        /// ER0018：供應商不存在
        /// success：供應商權限添加成功
        /// fail:供应商添加失败
        /// UD0001：數據庫操作產生異常
        /// </returns>
        public string SaveSupplyByRose(string RoseId, string supplyIdList)
        {
            string result = string.Empty;
            if (string.IsNullOrEmpty(RoseId))
                result = "ER0001";
            if (string.IsNullOrEmpty(result) && !roseManage.ExistsRoseId(RoseId))
                result = "ER0007";

            List<string> currentSupply = supplyIdList.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            List<string> historySupply = new List<string>();
            DataTable dt = ReadSupplyByRose(RoseId);
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["Usy"].ToString() == "Y")
                {
                    historySupply.Add(dr["suppNumber"].ToString());
                }
            }
            List<string> deleteList = historySupply.FindAll(o => !currentSupply.Contains(o)).ToList();
            List<string> newList = currentSupply.FindAll(o => !historySupply.Contains(o)).ToList();


            if (string.IsNullOrEmpty(result))
            {
                using (Trans t = new Trans())
                {
                    try
                    {
                        bool isResult = true;
                        foreach (string SupplyId in deleteList)
                        {
                            isResult = supplyManage.DeleteSupplyRightByRose(RoseId, SupplyId, t);
                            if (!isResult)
                            {
                                result = "ER0028";
                                break;
                            }
                        }

                        if (isResult)
                        {
                            foreach (string item in newList)
                            {
                                if (string.IsNullOrEmpty(item))
                                {
                                    result = "ER0017";
                                    isResult = false;
                                    break;
                                }
                                //检查供应商是否存在
                                if (!supplyManage.ExistsSupplyId(item, t))
                                {
                                    result = "ER0018";
                                    isResult = false;
                                    break;
                                }
                                if (!supplyManage.AddSupplyByRose(RoseId, item))
                                {
                                    result = "ER0029";
                                    isResult = false;
                                    break;
                                }
                            }
                        }

                        if (!isResult)
                        {
                            t.RollBack();
                            if (string.IsNullOrEmpty(result))
                                result = "EB0059";
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
                            result = "UD0001";
                        t.RollBack();
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 獲取個人權限對應的供應商
        /// </summary>
        /// <param name="UserId">用戶編號</param>
        /// <param name="CompanyId">公司編號</param>
        /// <returns></returns>
        public DataTable ReadSupplyByUser(string UserId, string CompanyId = "")
        {
            return supplyManage.ReadSupplyByUser(UserId);
        }

        /// <summary>
        /// 個人供應商權限保存
        /// </summary>
        /// <param name="UserId">用戶編號</param>
        /// <param name="supplyIdList">供應商類表</param>
        /// <returns>
        /// EU0008：用戶設置格式錯誤
        /// ER0017：供應商設置錯誤
        /// ER0018：供應商不存在
        /// success：供應商權限添加成功
        /// fail:供应商添加失败
        /// UD0001：數據庫操作產生異常
        /// </returns>
        public string SaveSupplyByUser(string UserId, string supplyIdList)
        {
            string result = string.Empty;
            if (string.IsNullOrEmpty(UserId))
                result = "EU0008";
            //if (string.IsNullOrEmpty(result) && !roseManage.ExistsRoseId(RoseId))
            //    result = "ER0007";

            if (string.IsNullOrEmpty(result))
            {
                using (Trans t = new Trans())
                {
                    try
                    {
                        bool isResult = supplyManage.DeleteSupplyRightByUser(UserId, t);
                        if (isResult)
                        {
                            List<string> items = supplyIdList.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            foreach (string item in items)
                            {
                                if (string.IsNullOrEmpty(item))
                                {
                                    result = "ER0017";
                                    isResult = false;
                                    break;
                                }
                                //检查供应商是否存在
                                if (!supplyManage.ExistsSupplyId(item, t))
                                {
                                    result = "ER0018";
                                    isResult = false;
                                    break;
                                }
                                if (!supplyManage.AddSupplyByUser(UserId, item))
                                {
                                    result = "ER0030";
                                    isResult = false;
                                    break;
                                }
                            }
                        }

                        if (!isResult)
                        {
                            t.RollBack();
                            if (string.IsNullOrEmpty(result))
                                result = "EB0059";
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
                            result = "UD0001";
                        t.RollBack();
                    }
                }
            }
            return result;
        }
    }
}
