using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using DMC.DAL.Script;
using DMC.Model;

namespace DMC.DAL
{
    public class PermissionDAL
    {
        private IUserRight script = ScriptFactory.GetScript<IUserRight>();
        private IPermission ps = ScriptFactory.GetScript<IPermission>();
        /// <summary>
        /// 獲取我的菜單目錄
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="langId">語言編號</param>
        /// <param name="MenuTree">返回的樹目錄</param>
        /// <returns>
        /// EU0005 用戶不存在
        /// EU0006 語言類別不存在
        /// 返回空表示成功
        /// </returns>
        public string GetMyMenu(string userId, string langId, string CompanyId, ref DataTable MenuTree)
        {
            string result = string.Empty;
            try
            {
                List<DbParameter> param = new List<DbParameter>();
                param.Add(DBFactory.Helper.FormatParameter("UserId", DbType.String, userId, 20));
                param.Add(DBFactory.Helper.FormatParameter("CompanyId", DbType.String, CompanyId, 20));
                param.Add(DBFactory.Helper.FormatParameter("LangId", DbType.String, langId, 20));
                MenuTree = DBFactory.Helper.ExecuteDataSet(ps.GetMyMenu, param.ToArray()).Tables[0];
                result = "";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return result;
        }

        /// <summary>
        /// 獲取我的公司
        /// </summary>
        /// <param name="userId">用戶編號</param>
        /// <param name="langId">語言編號</param>
        /// <param name="myCompany">返回公司列表</param>
        /// <returns>
        /// EU0005 用戶不存在
        /// EU0006 語言類別不存在
        /// 返回空表示成功
        /// </returns>
        public string GetMyCompany(string userId, string langId, ref DataTable myCompany)
        {
            string result = string.Empty;
            try
            {
                List<DbParameter> param = new List<DbParameter>();
                param.Add(DBFactory.Helper.FormatParameter("UserId", DbType.String, userId, 20));
                param.Add(DBFactory.Helper.FormatParameter("LangId", DbType.String, langId, 20));
                myCompany = DBFactory.Helper.ExecuteDataSet(ps.GetMyCompany, param.ToArray()).Tables[0];
                result = "";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return result;
        }

        /// <summary>
        /// 獲取我的程式操作權限
        /// </summary>
        /// <param name="userId">用戶編號</param>
        /// <param name="programId">程式編號</param>
        /// <param name="langId">語言編號</param>
        /// <param name="myPgmOpt">返回程式權限操作列表</param>
        /// <returns>
        /// EU0005 用戶不存在
        /// EU0006 語言類別不存在
        /// EU0011 程式編號不存在
        /// 返回空表示成功
        /// </returns>
        public string GetMyPgmOpt(string userId, string programId, string langId, ref List<ProgramActionInfo> paInfoList)
        {
            string result = string.Empty;
            paInfoList = new List<ProgramActionInfo>();
            try
            {
                List<DbParameter> param = new List<DbParameter>();
                param.Add(DBFactory.Helper.FormatParameter("UserId", DbType.String, userId, 20));
                param.Add(DBFactory.Helper.FormatParameter("ProgramId", DbType.String, programId, 20));
                param.Add(DBFactory.Helper.FormatParameter("LangId", DbType.String, langId, 20));

                DataTable myPgmOpt = DBFactory.Helper.ExecuteDataSet(ps.GetMyPgmOpt, param.ToArray()).Tables[0];
                //處理權限重複的情況，個人權限優先處理
                foreach (DataRow dr in myPgmOpt.Rows)
                {
                    ProgramActionInfo paInfo = paInfoList.Find(o => o.ActionId == dr["ActionId"].ToString());
                    if (paInfo == null)
                    {
                        paInfo = new ProgramActionInfo();
                        paInfo.ActionId = dr["ActionId"].ToString();
                        paInfo.ActionName = dr["ActionName"].ToString();
                        paInfo.IsUse = dr["Usy"].ToString();
                        paInfo.IsUserOrRose = dr["IsUserOrRose"].ToString();
                        paInfo.ProgramId = dr["ProgramId"].ToString();
                        paInfo.ProgramName = dr["ProgramName"].ToString();
                        paInfoList.Add(paInfo);
                    }
                    else
                    {
                        if (paInfo.IsUse == "Y" || dr["Usy"].ToString() == "Y")
                        {
                            paInfo.IsUse = "Y";
                        }
                    }
                }
                result = "";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return result;
        }

        /// <summary>
        /// 獲取用戶的資料權限
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public DataTable UserProgramData(string UserId, string ProgramId)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("UserId", DbType.String, UserId, 20));
            param.Add(DBFactory.Helper.FormatParameter("ProgramId", DbType.String, ProgramId, 20));
            DataTable dt = DBFactory.Helper.ExecuteDataSet(script.UserProgramData, param.ToArray()).Tables[0];
            return dt;
        }

    }
}
