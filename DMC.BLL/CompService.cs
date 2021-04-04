using System.Collections.Generic;
using DMC.Model;
using DMC.DAL;
using System.Data;
namespace DMC.BLL
{
    /// <summary>
    /// 公司業務操作類
	/// 處理系統公司信息的所有業務邏輯操作
    /// code by jeven_xiao
    /// 2013-6-9
    /// </summary>
    public class CompService
    {
        private PageManage pageView = new PageManage();
        private CompDAL cdal = new CompDAL();
        /// <summary>
        /// 增加新公司信息
        /// code by jeven_xiao
        /// 2013-6-9
        /// </summary>
        /// <param name="comp">公司實體</param>
        /// <returns>SB0003公司資料增加成功/EB0003公司資料已經存在/UB0001數據庫操作產生異常</returns>
        public  string AddComp(t_Company comp)
        {
           
            if (string.IsNullOrEmpty(comp.companyID))
                return  "EB0020";
           
            if (string.IsNullOrEmpty(comp.simpleName))
                return  "EB0021";
            if (!IsExistComp(comp.companyID,comp.compLanguage ))
            {

                if (cdal.AddComp(comp) == true)
                {
                    return "SB0003";
                }

                else
                {
                    return "UB0002";
                }
            }
            else {
                return "EB0003";
            }
            
        }

        /// <summary>
        /// 更新公司資料信息
        /// code by jeven
        /// 2013-6-9
        /// </summary>
        /// <param name="comp">公司實體</param>
        /// <returns>'SB0005' --公司資料更新成功；EB0011--公司資料不存在；UB0001--數據庫操作產生異常</returns>
        public  string ModComp(t_Company comp)
        {
           
            if (string.IsNullOrEmpty(comp.companyID))
                return "EB0020";
            if (string.IsNullOrEmpty(comp.simpleName))
                return "EB0021";
            if (IsExistComp(comp.companyID,comp.compLanguage ))
            {

                if (cdal.ModComp(comp) == true)
                {
                    return "SB0005";
                }

                else
                {
                    return "UB0003";
                }
            }
            else { return "EB0011"; }
          
            
        }

        /// <summary>
        /// 刪除公司資料信息
        /// code by jeven_xiao
        /// 2013-6-9
        /// </summary>
        /// <param name="idList">公司ID集合eg:1,2,3,4</param>
        /// <returns></returns>
        public  string DelComp(List<string> idList)
        {
            if (idList.Count < 1 || idList == null)
                return "EB0005";
            if (cdal.DelComp(idList) == true)
                return "SB0008";
            else
                return "UB0004";   
           
        }


        /// <summary>
        /// 取得所有公司资料信息的集合
        /// </summary>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="pageIndex">页码参数</param>
        ///  <param name="searchWhere">查询组合条件</param>
        /// <returns>ArrayList</returns>
        public DataTable GetAllComp(int pageSize, int pageIndex, out int pageCount, out int total, string Where = "")
        {

            return pageView.PageView("t_Company", "AutoId", pageIndex, pageSize, "*", "AutoId ASC", Where, out total, out pageCount);
        }

        /// <summary>
        /// 驗證公司資料是否存在
        /// </summary>
        /// <param name="compID">公司ID</param>
        /// <returns>string </returns>
        public bool IsExistComp(string compID, string compLanguage)
        {
            return cdal.IsExistComp(compID,compLanguage );
           
        }
        public DataTable GetCompany(string companyid)
        {
            DataTable dt = cdal.GetCompany(companyid);
            return dt;
        }
        /// <summary>
        /// 取得所有公司别的简称
        /// </summary>
        /// <returns></returns>
        public DataTable GetCompanyList(string LanguageId)
        {
            return cdal.GetCompanyList(LanguageId);
        }
        /// <summary>
        /// 根據公司ID獲取公司詳細信息 2015/6/18新增
        /// </summary>
        /// <param name="companyid">公司別</param>
        /// <param name="compLanguage">語言別</param>
        /// <returns></returns>
        public DataTable GetCompanyInfoByID(string companyid, string compLanguage)
        {
            return cdal.GetCompanyInfoByID(companyid, compLanguage);
        }

    }
}
