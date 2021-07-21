using DMC.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace DMC.DAL
{
    public class DeviceDAL
    {
        /// <summary>
        /// 新增设备分类
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool NewDevice(DeviceEntity entity)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("Insert into t_Device(DeviceId,DeviceName,CategoryId,CategoryText,Placement,Remark,Usey,KeepUserId,Status)");
            sbSql.Append("VALUES(@DeviceId,@DeviceName,@CategoryId,@CategoryText,@Placement,@Remark,@Usey,@KeepUserId,1)");
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("DeviceId", DbType.String, entity.DeviceId));
            param.Add(DBFactory.Helper.FormatParameter("DeviceName", DbType.String, entity.DeviceName));
            param.Add(DBFactory.Helper.FormatParameter("CategoryId", DbType.String, entity.CategoryId));
            param.Add(DBFactory.Helper.FormatParameter("CategoryText", DbType.String, entity.CategoryText));
            param.Add(DBFactory.Helper.FormatParameter("Placement", DbType.String, entity.Placement));
            param.Add(DBFactory.Helper.FormatParameter("Remark", DbType.String, entity.Remark));
            param.Add(DBFactory.Helper.FormatParameter("Usey", DbType.String, entity.Usey));
            param.Add(DBFactory.Helper.FormatParameter("KeepUserId", DbType.String, entity.KeepUserId));

            using (Trans t = new Trans())
            {
                try
                {
                    if (DBFactory.Helper.ExecuteNonQuery(sbSql.ToString(), param.ToArray()) > 0)
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
        /// 设备编号是否存在
        /// </summary>
        /// <param name="MacAddress"></param>
        /// <returns></returns>
        public bool IsExists(DeviceEntity entity)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT COUNT(1) FROM t_Device WHERE DeviceId=@DeviceId ");
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("DeviceId", DbType.String, entity.DeviceId)); 
            int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(sbSql.ToString(), param.ToArray()));
            return result > 0;
        }

        /// <summary>
        /// 更新设备信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateDevice(DeviceEntity entity)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("Update t_Device");
            sbSql.Append("   SET DeviceName=@DeviceName,CategoryId=@CategoryId,Usey=@Usey,CategoryText=@CategoryText,Placement=@Placement, ");
            sbSql.Append("       Remark=@Remark,KeepUserId=@KeepUserId ");
            sbSql.Append(" WHERE DeviceId=@DeviceId ");
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("DeviceId", DbType.String, entity.DeviceId));
            param.Add(DBFactory.Helper.FormatParameter("DeviceName", DbType.String, entity.DeviceName));
            param.Add(DBFactory.Helper.FormatParameter("CategoryId", DbType.String, entity.CategoryId));
            param.Add(DBFactory.Helper.FormatParameter("CategoryText", DbType.String, entity.CategoryText));
            param.Add(DBFactory.Helper.FormatParameter("Placement", DbType.String, entity.Placement));
            param.Add(DBFactory.Helper.FormatParameter("Remark", DbType.String, entity.Remark));
            param.Add(DBFactory.Helper.FormatParameter("Usey", DbType.String, entity.Usey));
            param.Add(DBFactory.Helper.FormatParameter("KeepUserId", DbType.String, entity.KeepUserId));

            using (Trans t = new Trans())
            {
                try
                {
                    if (DBFactory.Helper.ExecuteNonQuery(sbSql.ToString(), param.ToArray()) > 0)
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
        /// 删除设备信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool DeleteDevice(DeviceEntity entity)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("DELETE FROM t_Device");
            sbSql.Append(" WHERE DeviceId=@DeviceId ");
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("DeviceId", DbType.String, entity.DeviceId));

            using (Trans t = new Trans())
            {
                try
                {
                    if (DBFactory.Helper.ExecuteNonQuery(sbSql.ToString(), param.ToArray()) > 0)
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
        /// 通过模具号查询模具是否存在
        /// </summary>
        /// <param name="DeviceId">模具编号，多模具用/分割</param>
        /// <returns></returns>
        public bool IsExistsModel(string DeviceId)
        {
            StringBuilder sbSql = new StringBuilder();
            string inDeviceId ="('"+DeviceId.Replace("/",",")+"')";
            sbSql.Append("select count(*) from t_Device where CategoryId=B01 and DeviceId in "+inDeviceId);
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("DeviceId", DbType.String, DeviceId));
            int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(sbSql.ToString(), param.ToArray()));
            string[] sArray = DeviceId.Split('/');
            return result ==sArray.Length;
        }
    }
}
