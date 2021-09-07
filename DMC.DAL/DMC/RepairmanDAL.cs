using DMC.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace DMC.DAL
{
    /// <summary>
    /// 维修员数据操作
    /// </summary>
    public class RepairmanDAL
    {
        /// <summary>
        /// 白天工作状况统计
        /// </summary>
        /// <returns></returns>
        public DataTable GetRepairmDayWorking()
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(@"select RepairmanId,
        RepairmanName,
       workedtime,--已过去工时
       totaltime,--总工时
       sum(isnull(manhoure,0)) workingtime,--工作工时
       (totaltime-workedtime)surplustime,--剩余工时
       (workedtime-sum(isnull(manhoure,0))) resttime --空闲工时
from( 
select a.RepairmanId,a.RepairmanName,
       datediff(minute ,RepairSTime,isnull(RepairETime,GetDate())) manhoure,--工作工时
       datediff(minute ,Convert(varchar(10),GetDate(),121)+' 07:15:00',GetDate()) workedtime,--已过去工时
       datediff(minute ,(Convert(varchar(10),GetDate(),121)+' 07:15:00'),(Convert(varchar(10),GetDate(),121)+' 19:30:00')) totaltime
  from dbo.t_Repairman a with(nolock) left join
       dbo.t_RepairRecord b  with(nolock) on a.RepairmanId=b.RepairmanId and b.RepairSTime between Convert(varchar(10),GetDate(),121)+' 07:15:00' and Convert(varchar(10),GetDate(),121)+' 19:15:00'
where YearMonth=Convert(varchar(7),GetDate(),121) and
     -- ClassType=1 and IsWorking=1    
IsWorking=1 AND  (WorkRangeTimeBegin<GetDate() and WorkRangeTimeEnd>GetDate())   
      ) tp
group by RepairmanId,RepairmanName,workedtime,totaltime ");
            //List<DbParameter> param = new List<DbParameter>();
            //param.Add(DBFactory.Helper.FormatParameter("ClassType", DbType.String, ClassType));
            return DBFactory.Helper.ExecuteDataSet(sbSql.ToString(), null).Tables[0];
        }
        /// <summary>
        /// 晚上工作状况统计
        /// </summary>
        /// <returns></returns>
        public DataTable GetRepairmNightWorking()
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(@"select RepairmanId,
RepairmanName,
       workedtime,--已过去工时
       totaltime,--总工时
       sum(isnull(manhoure,0)) workingtime,--工作工时
       (totaltime-workedtime)surplustime,--剩余工时
       (workedtime-sum(isnull(manhoure,0))) resttime --空闲工时
from( 
select a.RepairmanId,a.RepairmanName,
       datediff(minute ,RepairSTime,isnull(RepairETime,GetDate())) manhoure,--工作工时
       datediff(minute ,Convert(varchar(10),DATEADD(dd,-1,GetDate()),121)+' 19:15:00',GetDate()) workedtime,--已过去工时
       datediff(minute ,(Convert(varchar(10),DATEADD(dd,-1,GetDate()),121)+' 19:15:00'),(Convert(varchar(10),GetDate(),121)+' 07:15:00')) totaltime
  from dbo.t_Repairman a  with(nolock) left join
       dbo.t_RepairRecord b  with(nolock)  on a.RepairmanId=b.RepairmanId and b.RepairSTime between Convert(varchar(10),DATEADD(dd,-1,GetDate()),121)+' 19:15:00' and Convert(varchar(10),GetDate(),121)+' 07:15:00'
where YearMonth=Convert(varchar(7),GetDate(),121) and
      --ClassType=0 and IsWorking=1 
IsWorking=1 AND  (WorkRangeTimeBegin<GetDate() and WorkRangeTimeEnd>GetDate()) 
      ) tp
group by RepairmanId,RepairmanName,workedtime,totaltime ");
            //List<DbParameter> param = new List<DbParameter>();
            //param.Add(DBFactory.Helper.FormatParameter("ClassType", DbType.String, ClassType));
            return DBFactory.Helper.ExecuteDataSet(sbSql.ToString(), null).Tables[0];
        }
        /// <summary>
        /// 获取指定班别的上班人员
        /// </summary>
        /// <param name="ClassType"></param>
        /// <returns></returns>
        public DataTable GetOnDutyUser(string ClassType)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(@"SELECT A.RepairmanId,(A.RepairmanName+'('+CAST(ISNULL(B.WaitRepairNum,0)AS VARCHAR)+')')RepairmanName
  FROM t_Repairman A LEFT JOIN (SELECT RepairmanId,COUNT(1) WaitRepairNum FROM t_RepairRecord 
WHERE RepairStatus IN(20,23,24,25)
GROUP BY RepairmanId
) B ON A.RepairmanId=B.RepairmanId WHERE IsWorking=1 AND  (WorkRangeTimeBegin<GetDate() and WorkRangeTimeEnd>GetDate()) ");
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("ClassType", DbType.String, ClassType));
            param.Add(DBFactory.Helper.FormatParameter("WorkDate", DbType.String, DateTime.Now.ToString("yyyy-MM-dd")));
            return DBFactory.Helper.ExecuteDataSet(sbSql.ToString(), param.ToArray()).Tables[0];
        }
        /// <summary>
        /// 新增维修员
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool NewRepairman(RepairmanEntity entity)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("Insert into t_Repairman(RepairmanId,RepairmanName,ClassType,IsLeader,IsWorking,WorkRangeTime,PhotoUrl,YearMonth,WorkRangeTimeBegin,WorkRangeTimeEnd,WorkDate,WorkNum,GroupName)");
            sbSql.Append("VALUES(@RepairmanId,@RepairmanName,@ClassType,@IsLeader,@IsWorking,@WorkRangeTime,@PhotoUrl,@YearMonth,@WorkRangeTimeBegin,@WorkRangeTimeEnd,@WorkDate,@WorkNum,@GroupName)");
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("RepairmanId", DbType.String, entity.RepairmanId));
            param.Add(DBFactory.Helper.FormatParameter("RepairmanName", DbType.String, entity.RepairmanName));
            param.Add(DBFactory.Helper.FormatParameter("ClassType", DbType.String, entity.ClassType));
            param.Add(DBFactory.Helper.FormatParameter("IsLeader", DbType.String, entity.IsLeader));
            param.Add(DBFactory.Helper.FormatParameter("IsWorking", DbType.String, entity.IsWorking));
            param.Add(DBFactory.Helper.FormatParameter("WorkRangeTime", DbType.String, entity.WorkRangeTime));
            param.Add(DBFactory.Helper.FormatParameter("PhotoUrl", DbType.String, entity.PhotoUrl));
            param.Add(DBFactory.Helper.FormatParameter("YearMonth", DbType.String, entity.YearMonth));
            param.Add(DBFactory.Helper.FormatParameter("WorkRangeTimeBegin", DbType.String, entity.WorkRangeTimeBegin));
            param.Add(DBFactory.Helper.FormatParameter("WorkRangeTimeEnd", DbType.String, entity.WorkRangeTimeEnd));
            param.Add(DBFactory.Helper.FormatParameter("WorkDate", DbType.String, entity.WorkDate));
            param.Add(DBFactory.Helper.FormatParameter("WorkNum", DbType.String, entity.WorkNum));
            if (!string.IsNullOrEmpty(entity.GroupName))
                param.Add(DBFactory.Helper.FormatParameter("GroupName", DbType.String, entity.GroupName));
            else
                param.Add(DBFactory.Helper.FormatParameter("GroupName", DbType.String, DBNull.Value));
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
        public bool IsExists(RepairmanEntity entity)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT COUNT(1) FROM t_Repairman WHERE RepairmanId=@RepairmanId and WorkDate=@WorkDate");
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("RepairmanId", DbType.String, entity.RepairmanId));
            param.Add(DBFactory.Helper.FormatParameter("WorkDate", DbType.String, entity.WorkDate));
            int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(sbSql.ToString(), param.ToArray()));
            return result > 0;
        }
        public bool DeleteRepairman_WordDate(string YearMonth)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("delete t_Repairman WHERE  YearMonth=@YearMonth");
            List<DbParameter> param = new List<DbParameter>();
            //param.Add(DBFactory.Helper.FormatParameter("RepairmanId", DbType.String, entity.RepairmanId));
            param.Add(DBFactory.Helper.FormatParameter("YearMonth", DbType.String, YearMonth));

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
        /// 获取班次
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string GetClassType(RepairmanEntity entity)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT classtype FROM t_Repairman WHERE RepairmanId=@RepairmanId and WorkDate=@WorkDate");
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("RepairmanId", DbType.String, entity.RepairmanId));
            param.Add(DBFactory.Helper.FormatParameter("WorkDate", DbType.String, entity.WorkDate));
            string result = Convert.ToString(DBFactory.Helper.ExecuteScalar(sbSql.ToString(), param.ToArray()));
            return result;
        }
        /// <summary>
        /// 更新维修员信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateRepairman(RepairmanEntity entity)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("Update t_Repairman");
            sbSql.Append("   SET RepairmanName=@RepairmanName,ClassType=@ClassType,IsLeader=@IsLeader,IsWorking=@IsWorking, ");
            sbSql.Append("       WorkRangeTime=@WorkRangeTime,PhotoUrl=@PhotoUrl,WorkRangeTimeBegin=@WorkRangeTimeBegin,WorkRangeTimeEnd=@WorkRangeTimeEnd, ");
            sbSql.Append("       WorkNum=@WorkNum ");
            sbSql.Append(" WHERE RepairmanId=@RepairmanId and WorkDate=@WorkDate");
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("RepairmanId", DbType.String, entity.RepairmanId));
            param.Add(DBFactory.Helper.FormatParameter("RepairmanName", DbType.String, entity.RepairmanName));
            param.Add(DBFactory.Helper.FormatParameter("ClassType", DbType.String, entity.ClassType));
            param.Add(DBFactory.Helper.FormatParameter("IsLeader", DbType.String, entity.IsLeader));
            param.Add(DBFactory.Helper.FormatParameter("IsWorking", DbType.String, entity.IsWorking));
            param.Add(DBFactory.Helper.FormatParameter("WorkRangeTime", DbType.String, entity.WorkRangeTime));
            param.Add(DBFactory.Helper.FormatParameter("PhotoUrl", DbType.String, entity.PhotoUrl));
            param.Add(DBFactory.Helper.FormatParameter("WorkRangeTimeBegin", DbType.String, entity.WorkRangeTimeBegin));
            param.Add(DBFactory.Helper.FormatParameter("WorkRangeTimeEnd", DbType.String, entity.WorkRangeTimeEnd));
            param.Add(DBFactory.Helper.FormatParameter("WorkDate", DbType.String, entity.WorkDate));
            param.Add(DBFactory.Helper.FormatParameter("WorkNum", DbType.String, entity.WorkNum));
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
        /// 更新维修员信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool SetWorking(RepairmanEntity entity)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("Update t_Repairman");
            sbSql.Append("   SET IsWorking=@IsWorking ");
            sbSql.Append(" WHERE RepairmanId=@RepairmanId and WorkDate=@WorkDate ");
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("RepairmanId", DbType.String, entity.RepairmanId));
            param.Add(DBFactory.Helper.FormatParameter("IsWorking", DbType.String, entity.IsWorking));
            param.Add(DBFactory.Helper.FormatParameter("WorkDate", DbType.String, entity.WorkDate));

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
        /// 删除维修员信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool DeleteRepairman(RepairmanEntity entity)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("DELETE FROM t_Repairman");
            sbSql.Append(" WHERE RepairmanId=@RepairmanId and WorkDate=@WorkDate ");
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("RepairmanId", DbType.String, entity.RepairmanId));
            param.Add(DBFactory.Helper.FormatParameter("WorkDate", DbType.String, entity.WorkDate));
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
    }
}
