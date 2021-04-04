using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using DMC.DAL.Script;
using DMC.Model;

namespace DMC.DAL
{
    public class RoseManageDAL
    {
        private ILanguage script = ScriptFactory.GetScript<ILanguage>();
        private IRightType scriptType = ScriptFactory.GetScript<IRightType>(); 

        public DataTable GetAllRoseType()
        {
            DataTable dt = DBFactory.Helper.ExecuteDataSet(scriptType.GetAllType, null).Tables[0];
            return dt;
        }

        /// <summary>
        /// 獲取角色的用戶明細
        /// </summary>
        /// <param name="RoseId"></param>
        /// <returns></returns>
        public DataTable RoseUserInfo(string RoseId)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("RoseId", System.Data.DbType.String, RoseId, 20));
            DataTable dt = DBFactory.Helper.ExecuteDataSet(scriptType.RoseUserList, param.ToArray()).Tables[0];
            return dt;
        }

        /// <summary>
        /// 角色添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddRose(t_Right_Rose model)
        {
            try
            {
                //參數設置
                List<DbParameter> param = new List<DbParameter>();
                param.Add(DBFactory.Helper.FormatParameter("RoseId", System.Data.DbType.String, model.RoseId, 20));
                param.Add(DBFactory.Helper.FormatParameter("RoseName", System.Data.DbType.String, model.RoseName, 50));
                param.Add(DBFactory.Helper.FormatParameter("Usy", System.Data.DbType.String, model.Usy, 1));
                param.Add(DBFactory.Helper.FormatParameter("SystemType", System.Data.DbType.String, model.SystemType, 2));
                param.Add(DBFactory.Helper.FormatParameter("CreateDeptId", System.Data.DbType.String, model.CreateDeptId, 20));
                param.Add(DBFactory.Helper.FormatParameter("CreateUserId", System.Data.DbType.String, model.CreateUserId, 20));
                param.Add(DBFactory.Helper.FormatParameter("CreateTime", System.Data.DbType.DateTime, System.DateTime.Now));

                //添加資源基本屬性
                int result = DBFactory.Helper.ExecuteNonQuery(scriptType.CreateRoseType, param.ToArray());
                return result == 1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// 權限類別修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ModRose(t_Right_Rose model)
        {
            try
            {
                //參數設置
                List<DbParameter> param = new List<DbParameter>();
                param.Add(DBFactory.Helper.FormatParameter("RoseId", System.Data.DbType.String, model.RoseId, 20));
                param.Add(DBFactory.Helper.FormatParameter("RoseName", System.Data.DbType.String, model.RoseName, 50));
                param.Add(DBFactory.Helper.FormatParameter("Usy", System.Data.DbType.String, model.Usy, 1));
                param.Add(DBFactory.Helper.FormatParameter("SystemType", System.Data.DbType.String, model.SystemType, 2));
                param.Add(DBFactory.Helper.FormatParameter("UpdateUserId", System.Data.DbType.String, model.UpdateUserId, 20));
                param.Add(DBFactory.Helper.FormatParameter("UpdateDeptId", System.Data.DbType.String, model.UpdateDeptId, 20));
                param.Add(DBFactory.Helper.FormatParameter("UpdateTime", System.Data.DbType.DateTime, System.DateTime.Now));

                //添加資源基本屬性
                int result = DBFactory.Helper.ExecuteNonQuery(scriptType.ModifyRose, param.ToArray());
                return result == 1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// 刪除角色類別
        /// </summary>
        /// <param name="roseIdList"></param>
        /// <returns></returns>
        public bool DelRose(List<string> roseIdList)
        {
            using (Trans t = new Trans())
            {
                try
                {
                    foreach (string RoseId in roseIdList)
                    {
                        //參數設置
                        List<DbParameter> param = new List<DbParameter>();
                        param.Add(DBFactory.Helper.FormatParameter("RoseId", System.Data.DbType.String, RoseId, 20));

                        DBFactory.Helper.ExecuteScalar(scriptType.DeleteRoseType +
                                                       scriptType.DeleteRightRProgram +
                                                       scriptType.DeleteRightRPAction +
                                                       scriptType.DeleteRightRData +
                                                       scriptType.DeleteRightRsupply, param.ToArray());
                    }
                    t.Commit();
                    return true;
                }
                catch
                {
                    t.RollBack();
                    return false;
                }
            }
        }

        /// <summary>
        /// 權限複製
        /// </summary>
        /// <param name="roseIdAim">目的權限編號</param>
        /// <param name="roseIdSocure">來源權限編號</param>
        /// <returns></returns>
        public bool CopyRose(string roseIdAim, string roseIdSocure)
        {
            using (Trans t = new Trans())
            {
                try
                {
                    //參數設置
                    List<DbParameter> param = new List<DbParameter>();
                    param.Add(DBFactory.Helper.FormatParameter("RoseIdAim", System.Data.DbType.String, roseIdAim, 20));
                    param.Add(DBFactory.Helper.FormatParameter("RoseIdSocure", System.Data.DbType.String, roseIdSocure, 20));
                    DBFactory.Helper.ExecuteScalar(scriptType.CopyRightRProgram +
                    scriptType.CopyRightRPAcion +
                    scriptType.CopyRightRData +
                    scriptType.CopyRightRsupply, param.ToArray());
                    t.Commit();
                    return true;
                }
                catch
                {
                    t.RollBack();
                    return false;
                }
            }
        }
        /// <summary>
        /// 判斷權限類別是否已授權
        /// </summary>
        /// <param name="RoseId"></param>
        /// <returns></returns>
        public bool RoseExitsRight(string RoseId)
        {
            //參數設置
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("RoseId", System.Data.DbType.String, RoseId, 20));
            int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(scriptType.RoseExitsRight, param.ToArray()));
            return result != 0;
        }

        /// <summary>
        /// 驗證權限類別是否存在
        /// </summary>
        /// <param name="RoseId"></param>
        /// <returns></returns>
        public bool ExistsRoseId(string RoseId)
        {
            //參數設置
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("RoseId", System.Data.DbType.String, RoseId, 20));
            int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(scriptType.ExistsRoseId, param.ToArray()));
            return result != 0;
        }

        /// <summary>
        /// 驗證權限類別是否存在
        /// </summary>
        /// <param name="RoseId"></param>
        /// <returns></returns>
        public bool ExistsRoseId(string RoseId, Trans t)
        {
            //參數設置
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("RoseId", System.Data.DbType.String, RoseId, 20));
            int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(scriptType.ExistsRoseId, param.ToArray(), t));
            return result != 0;
        }

        /// <summary>
        /// 驗證權限類別是否存在
        /// </summary>
        /// <param name="RoseId"></param>
        /// <returns></returns>
        public bool ExistsRoseName(string RoseName, string RoseId = "")
        {
            //參數設置
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("RoseId", System.Data.DbType.String, RoseId, 20));
            param.Add(DBFactory.Helper.FormatParameter("RoseName", System.Data.DbType.String, RoseName, 50));
            int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(scriptType.ExistsRoseName, param.ToArray()));
            return result != 0;
        }

        /// <summary>
        /// 驗證權限是否處於使用中
        /// </summary>
        /// <param name="RoseIdList"></param>
        /// <returns></returns>
        public bool RightIsUsing(string RoseIdList)
        {
            //參數設置
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("RoseIdList", System.Data.DbType.String, RoseIdList, 1000));
            int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(scriptType.RightIsUsing, param.ToArray()));
            return result != 0;
        }
        /// <summary>
        /// 驗證權限是否處於使用中,并返回id
        /// </summary>
        /// <param name="RoseIdList"></param>
        /// <returns></returns>
        public string RightIsUsingId(string RoseIdList)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("RoseIdList", System.Data.DbType.String, RoseIdList, 1000));
            DataSet ds = DBFactory.Helper.ExecuteDataSet(scriptType.RightIsUsingId, param.ToArray());
            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                string id = string.Empty;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    id += dr[0].ToString() + ",";
                }
                return id.TrimEnd(',');
            }
            else
            {
                return "";
            }
        }
    }
}
