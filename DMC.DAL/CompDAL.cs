using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using DMC.Model;
using DMC.DAL.Script;

namespace DMC.DAL
{
    /// <summary>
    /// 公司數據訪問操作類
    /// code by jeven_xiao
    /// 2013-6-9
    /// </summary>
    public class CompDAL
    {
        private IComp script = ScriptFactory.GetScript<IComp>();
        /// <summary>
        /// 增加新公司信息
        /// code by jeven_xiao
        /// 2013-6-9
        /// </summary>
        /// <param name="comp">公司實體</param>
        /// <returns>bool</returns>
        public bool AddComp(t_Company comp)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("companyID", DbType.String, comp.companyID, 20));
            param.Add(DBFactory.Helper.FormatParameter("compLanguage", DbType.String, comp.compLanguage, 20));
            param.Add(DBFactory.Helper.FormatParameter("compCategory", DbType.String, comp.compCategory, 1));
            param.Add(DBFactory.Helper.FormatParameter("interName", DbType.String, comp.interName, 100));
            param.Add(DBFactory.Helper.FormatParameter("outerName", DbType.String, comp.outerName, 100));
            param.Add(DBFactory.Helper.FormatParameter("simpleName", DbType.String, comp.simpleName, 50));
            param.Add(DBFactory.Helper.FormatParameter("companyNo", DbType.String, comp.companyNo, 100));
            param.Add(DBFactory.Helper.FormatParameter("addrOne", DbType.String, comp.addrOne, 200));
            param.Add(DBFactory.Helper.FormatParameter("addrTwo", DbType.String, comp.addrTwo, 200));
            param.Add(DBFactory.Helper.FormatParameter("compTel", DbType.String, comp.compTel,50));
            param.Add(DBFactory.Helper.FormatParameter("compFax", DbType.String, comp.compFax, 20));
            param.Add(DBFactory.Helper.FormatParameter("compRegNo", DbType.String, comp.compRegNo, 200));
            param.Add(DBFactory.Helper.FormatParameter("remark", DbType.String, comp.remark, 1000));
            param.Add(DBFactory.Helper.FormatParameter("createTime", DbType.DateTime, comp.createTime));
            param.Add(DBFactory.Helper.FormatParameter("usy", DbType.String, comp.usy, 1));
            param.Add(DBFactory.Helper.FormatParameter("createrID", DbType.String, comp.createrID, 20));
            param.Add(DBFactory.Helper.FormatParameter("cDeptID", DbType.String, comp.cDeptID, 20));

            using (Trans t = new Trans())
            {
                try
                {
                    if (DBFactory.Helper.ExecuteNonQuery(script.AddComp, param.ToArray()) > 0)
                    {
                        t.Commit();
                        return true;
                    }
                    else
                        return false;

                }
                catch (Exception ex)
                {
                    t.RollBack();
                    throw ex;
                }

            }

        }

        /// <summary>
        /// 更新公司資料信息
        /// code by jeven
        /// 2013-6-9
        /// </summary>
        /// <param name="comp">公司實體</param>
        /// <returns>bool</returns>
        public bool ModComp(t_Company comp)
        {  
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("companyID", DbType.String, comp.companyID, 20));
            param.Add(DBFactory.Helper.FormatParameter("compLanguage", DbType.String, comp.compLanguage, 20));
            param.Add(DBFactory.Helper.FormatParameter("compCategory", DbType.String, comp.compCategory, 1));
            param.Add(DBFactory.Helper.FormatParameter("interName", DbType.String, comp.interName, 100));
            param.Add(DBFactory.Helper.FormatParameter("outerName", DbType.String, comp.outerName, 100));
            param.Add(DBFactory.Helper.FormatParameter("simpleName", DbType.String, comp.simpleName, 50));
            param.Add(DBFactory.Helper.FormatParameter("companyNo", DbType.String, comp.companyNo, 100));
            param.Add(DBFactory.Helper.FormatParameter("addrOne", DbType.String, comp.addrOne, 200));
            param.Add(DBFactory.Helper.FormatParameter("addrTwo", DbType.String, comp.addrTwo, 200));
            param.Add(DBFactory.Helper.FormatParameter("compTel", DbType.String, comp.compTel, 50));
            param.Add(DBFactory.Helper.FormatParameter("compFax", DbType.String, comp.compFax, 20));
            param.Add(DBFactory.Helper.FormatParameter("compRegNo", DbType.String, comp.compRegNo, 200));
            param.Add(DBFactory.Helper.FormatParameter("remark", DbType.String, comp.remark, 1000));
            param.Add(DBFactory.Helper.FormatParameter("modTime", DbType.DateTime, comp.lastModTime));
            param.Add(DBFactory.Helper.FormatParameter("usy", DbType.String, comp.usy, 1));
            param.Add(DBFactory.Helper.FormatParameter("updaterID", DbType.String, comp.updaterID, 20));
            param.Add(DBFactory.Helper.FormatParameter("uDeptID", DbType.String, comp.uDeptID, 20));
            using (Trans t = new Trans())
            {
                try
                {
                    if (DBFactory.Helper.ExecuteNonQuery(script.ModComp, param.ToArray()) > 0)
                    {
                        t.Commit();
                        return true;
                    }
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    t.RollBack();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 刪除公司資料信息
        /// code by jeven_xiao
        /// 2013-6-9
        /// </summary>
        /// <param name="idList">公司ID集合eg:1,2,3,4</param>
        /// <returns></returns>
        public bool DelComp(List<string> idList)
        {
            
            using (Trans tr = new Trans())
            {
                try
                {
                    foreach (string id in idList)
                    {
                        List<DbParameter> param = new List<DbParameter>();
                        param.Add(DBFactory.Helper.FormatParameter("idstr", DbType.String, id, 20));
                        DBFactory.Helper.ExecuteNonQuery(script.DelComp , param.ToArray());

                    }
                    tr.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    tr.RollBack();
                    throw ex;

                }
            }
        }

        public DataTable GetCompany(string companyid)
        {
            DataTable dt = DBFactory.Helper.ExecuteDataSet(string.Format(script.IGetCompany, companyid), null).Tables[0];
            return dt;
        }

        /// <summary>
        /// 根據公司ID獲取公司詳細信息 2015/6/18新增
        /// </summary>
        /// <param name="companyid">公司別</param>
        /// <param name="compLanguage">語言別</param>
        /// <returns></returns>
        public DataTable GetCompanyInfoByID(string companyid, string compLanguage)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("companyID", DbType.String, companyid, 20));
            param.Add(DBFactory.Helper.FormatParameter("compLanguage", DbType.String, compLanguage, 20));
            DataTable dt = DBFactory.Helper.ExecuteDataSet(script.GetCompanyInfoByID, param.ToArray()).Tables[0];
            return dt;
        }

        /// <summary>
        /// 驗證公司資料是否存在
        /// </summary>
        /// <param name="compID">公司ID</param>
        /// <returns>bool </returns>
        public bool IsExistComp(string compID, string compLanguage)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("companyID", DbType.String, compID, 20));
            param.Add(DBFactory.Helper.FormatParameter("compLanguage", DbType.String, compLanguage, 20));
            int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(script.IsExitComp, param.ToArray()));
            return result != 0;
        }

        /// <summary>
        /// 取得所有公司别的简称
        /// </summary>
        /// <returns></returns>
        public DataTable GetCompanyList(string LanguageId)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("LanguageId", DbType.String, LanguageId, 20));
            DataTable dt = DBFactory.Helper.ExecuteDataSet(script.GetCompanyList, param.ToArray()).Tables[0];
            return dt;
        }
    }
}
