using DMC.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace DMC.DAL
{
    public class FaultPhenomenaDAL
    {
        /// <summary>
        /// 新增设备分类
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool NewFaultPhenomena(FaultPhenomenaEntity entity)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("Insert into t_FaultPhenomena(CategoryId,CategoryName,OrderId,PCategoryId,Usey,CategoryText)");
            sbSql.Append("VALUES(@CategoryId,@CategoryName,@OrderId,@PCategoryId,@Usey,@CategoryText)");
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("CategoryId", DbType.String, entity.CategoryId));
            param.Add(DBFactory.Helper.FormatParameter("CategoryName", DbType.String, entity.CategoryName));
            param.Add(DBFactory.Helper.FormatParameter("CategoryText", DbType.String, entity.CategoryText));
            param.Add(DBFactory.Helper.FormatParameter("OrderId", DbType.String, entity.OrderId));
            param.Add(DBFactory.Helper.FormatParameter("Usey", DbType.String, entity.Usey));
            if (string.IsNullOrWhiteSpace(entity.PCategoryId) || Convert.IsDBNull(entity.PCategoryId))
            {
                param.Add(DBFactory.Helper.FormatParameter("PCategoryId", DbType.String, DBNull.Value));
            }
            else
            {
                param.Add(DBFactory.Helper.FormatParameter("PCategoryId", DbType.String, entity.PCategoryId));
            }

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
        /// 设备分类是否存在
        /// </summary>
        /// <param name="MacAddress"></param>
        /// <returns></returns>
        public bool IsExists(FaultPhenomenaEntity entity)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT COUNT(1) FROM t_FaultPhenomena WHERE CategoryId=@CategoryId or CategoryName=@CategoryName ");
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("CategoryId", DbType.String, entity.CategoryId));
            param.Add(DBFactory.Helper.FormatParameter("CategoryName", DbType.String, entity.CategoryName));
            int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(sbSql.ToString(), param.ToArray()));
            return result > 0;
        }

        /// <summary>
        /// 更新设备类别信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateFaultPhenomena(FaultPhenomenaEntity entity)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("Update t_FaultPhenomena");
            sbSql.Append("   SET CategoryName=@CategoryName,OrderId=@OrderId,Usey=@Usey,PCategoryId=@PCategoryId,CategoryText=@CategoryText ");
            sbSql.Append(" WHERE CategoryId=@CategoryId ");
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("CategoryId", DbType.String, entity.CategoryId));
            param.Add(DBFactory.Helper.FormatParameter("CategoryName", DbType.String, entity.CategoryName));
            param.Add(DBFactory.Helper.FormatParameter("OrderId", DbType.String, entity.OrderId));
            param.Add(DBFactory.Helper.FormatParameter("Usey", DbType.String, entity.Usey));
            param.Add(DBFactory.Helper.FormatParameter("CategoryText", DbType.String, entity.CategoryText));
            if (string.IsNullOrWhiteSpace(entity.PCategoryId) || Convert.IsDBNull(entity.PCategoryId))
            {
                param.Add(DBFactory.Helper.FormatParameter("PCategoryId", DbType.String, DBNull.Value));
            }
            else
            {
                param.Add(DBFactory.Helper.FormatParameter("PCategoryId", DbType.String, entity.PCategoryId));
            }
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
        /// 获取所有有效的设备分类
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllFaultPhenomena()
        {
            return DBFactory.Helper.ExecuteDataSet("SELECT * FROM t_FaultPhenomena WHERE Usey='Y'", null).Tables[0];
        }
    }
}
