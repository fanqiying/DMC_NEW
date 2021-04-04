using DMC.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace DMC.DAL
{
    public class FaultPositionDAL
    {

        /// <summary>
        /// 新增设备分类
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool NewFaultPosition(FaultPositionEntity entity)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("Insert into t_FaultPosition(PositionId,PositionName,OrderId,PPositionId,Usey,PositionText,GradeTime,Grade)");
            sbSql.Append("VALUES(@PositionId,@PositionName,@OrderId,@PPositionId,@Usey,@PositionText,@GradeTime,@Grade)");
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("PositionId", DbType.String, entity.PositionId));
            param.Add(DBFactory.Helper.FormatParameter("PositionName", DbType.String, entity.PositionName));
            param.Add(DBFactory.Helper.FormatParameter("PositionText", DbType.String, entity.PositionText));
            param.Add(DBFactory.Helper.FormatParameter("OrderId", DbType.String, entity.OrderId));
            param.Add(DBFactory.Helper.FormatParameter("Usey", DbType.String, entity.Usey));
            param.Add(DBFactory.Helper.FormatParameter("GradeTime", DbType.String, entity.GradeTime));
            param.Add(DBFactory.Helper.FormatParameter("Grade", DbType.String, entity.Grade));
            if (string.IsNullOrWhiteSpace(entity.PPositionId) || Convert.IsDBNull(entity.PPositionId))
            {
                param.Add(DBFactory.Helper.FormatParameter("PPositionId", DbType.String, DBNull.Value));
            }
            else
            {
                param.Add(DBFactory.Helper.FormatParameter("PPositionId", DbType.String, entity.PPositionId));
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
        public bool IsExists(FaultPositionEntity entity)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT COUNT(1) FROM t_FaultPosition WHERE PositionId=@PositionId or PositionName=@PositionName ");
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("PositionId", DbType.String, entity.PositionId));
            param.Add(DBFactory.Helper.FormatParameter("PositionName", DbType.String, entity.PositionName));
            int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(sbSql.ToString(), param.ToArray()));
            return result > 0;
        }

        /// <summary>
        /// 更新设备类别信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateFaultPosition(FaultPositionEntity entity)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("Update t_FaultPosition");
            sbSql.Append("   SET PositionName=@PositionName,OrderId=@OrderId,Usey=@Usey,PPositionId=@PPositionId,PositionText=@PositionText, ");
            sbSql.Append("       GradeTime=@GradeTime,Grade=@Grade ");
            sbSql.Append(" WHERE PositionId=@PositionId ");
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("PositionId", DbType.String, entity.PositionId));
            param.Add(DBFactory.Helper.FormatParameter("PositionName", DbType.String, entity.PositionName));
            param.Add(DBFactory.Helper.FormatParameter("OrderId", DbType.String, entity.OrderId));
            param.Add(DBFactory.Helper.FormatParameter("Usey", DbType.String, entity.Usey));
            param.Add(DBFactory.Helper.FormatParameter("PositionText", DbType.String, entity.PositionText));
            param.Add(DBFactory.Helper.FormatParameter("GradeTime", DbType.String, entity.GradeTime));
            param.Add(DBFactory.Helper.FormatParameter("Grade", DbType.String, entity.Grade));
            if (string.IsNullOrWhiteSpace(entity.PPositionId) || Convert.IsDBNull(entity.PPositionId))
            {
                param.Add(DBFactory.Helper.FormatParameter("PPositionId", DbType.String, DBNull.Value));
            }
            else
            {
                param.Add(DBFactory.Helper.FormatParameter("PPositionId", DbType.String, entity.PPositionId));
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
        public DataTable GetFaultPositionMain()
        {
            return DBFactory.Helper.ExecuteDataSet("SELECT * FROM t_FaultPosition WHERE Usey='Y' and ISNULL(PPositionId,'')=''", null).Tables[0];
        }

        /// <summary>
        /// 获取所有有效的设备分类
        /// </summary>
        /// <returns></returns>
        public DataTable GetFaultPositionNode(string PPositionId)
        {
            return DBFactory.Helper.ExecuteDataSet("SELECT * FROM t_FaultPosition WHERE Usey='Y' and PPositionId=N'" + PPositionId + "'", null).Tables[0];
        }

        /// <summary>
        /// 获取所有有效的设备分类
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllFaultPosition()
        {
            return DBFactory.Helper.ExecuteDataSet("SELECT * FROM t_FaultPosition WHERE Usey='Y'", null).Tables[0];
        }
    }
}
