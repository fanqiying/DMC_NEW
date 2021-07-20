using DMC.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace DMC.DAL
{
    public class RepairFormDAL
    {
        /// <summary>
        /// 获取单据的数量
        /// </summary>
        /// <returns></returns>
        public DataTable GetAssignQty()
        {
            //
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(@"select SUM(case when FormStatus='10' THEN 1 ELSE 0 END) WaitQty, 
       SUM(case when FormStatus='23' OR  FormStatus='24' OR  FormStatus='25' THEN 1 ELSE 0 END) RebackQty,
       SUM(case when FormStatus='12' THEN 1 ELSE 0 END) ChangeQty
  from dbo.t_RepairForm
 where formstatus in('10','12','23','24','25') ");
            return DBFactory.Helper.ExecuteDataSet(sbSql.ToString(), null).Tables[0];
        }

        /// <summary>
        /// 获取单据的数量
        /// </summary>
        /// <returns></returns>
        public DataTable KanBan()
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(@"select A.*,isnull(repairstatus,0)repairstatus,c.GradeTime,c.Grade StandGrade,datediff(minute ,RepairSTime,isnull(RepairETime,GetDate()))-10-(case when RepairSTime<cast(convert(varchar(10),RepairSTime,121)+ ' 12:30:00' as datetime) and isnull(RepairETime,GetDate())>cast(convert(varchar(10),RepairSTime,121)+ ' 13:30:00' as datetime) then 1 else 0 end)*40 manhoure,d.KeepUserId
  from t_RepairForm a left join
       t_RepairRecord b on a.RepairFormNO=b.RepairFormNO left join 
       t_FaultPosition c on a.PhenomenaId=b.PositionId left join 
       t_Device d on a.DeviceId=d.DeviceId 
where isnull(repairstatus,0)<60 ");
            return DBFactory.Helper.ExecuteDataSet(sbSql.ToString(), null).Tables[0];
        }
        /// <summary>
        /// 新增报修单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool NewRepairForm(RepairFormEntity entity)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("Insert into t_RepairForm(RepairFormNO,ApplyUserId,FaultTime,DeviceId,PositionId,PositionText,PositionId1,PhenomenaId,PhenomenaText,PhenomenaId1,PhenomenaText1,PositionText1,");
            sbSql.Append("FaultStatus,FaultCode,FaultReason,Intime,FormStatus,MouldId,NewMouldId,MouldId1,NewMouldId1)");
            sbSql.Append("VALUES(@RepairFormNO,@ApplyUserId,@FaultTime,@DeviceId,@PositionId,@PositionText,@PositionId1,@PhenomenaId,@PhenomenaText,@PhenomenaId1,@PhenomenaText1,@PositionText1,");
            sbSql.Append("@FaultStatus,@FaultCode,@FaultReason,GETDATE(),10,@MouldId,@NewMouldId,@MouldId1,@NewMouldId1)");
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("RepairFormNO", DbType.String, entity.RepairFormNO));
            param.Add(DBFactory.Helper.FormatParameter("ApplyUserId", DbType.String, entity.ApplyUserId));
            param.Add(DBFactory.Helper.FormatParameter("FaultTime", DbType.String, entity.FaultTime));
            param.Add(DBFactory.Helper.FormatParameter("DeviceId", DbType.String, entity.DeviceId));
            param.Add(DBFactory.Helper.FormatParameter("PositionId", DbType.String, entity.PositionId));
            param.Add(DBFactory.Helper.FormatParameter("PositionText", DbType.String, entity.PositionText));
            param.Add(DBFactory.Helper.FormatParameter("PositionText1", DbType.String, entity.PositionText1));
            param.Add(DBFactory.Helper.FormatParameter("PositionId1", DbType.String, entity.PositionId1));
            param.Add(DBFactory.Helper.FormatParameter("PhenomenaId", DbType.String, entity.PhenomenaId));
            param.Add(DBFactory.Helper.FormatParameter("PhenomenaText", DbType.String, entity.PhenomenaText));
            param.Add(DBFactory.Helper.FormatParameter("PhenomenaId1", DbType.String, entity.PhenomenaId1));
            param.Add(DBFactory.Helper.FormatParameter("PhenomenaText1", DbType.String, entity.PhenomenaText1));
            param.Add(DBFactory.Helper.FormatParameter("FaultStatus", DbType.String, entity.FaultStatus));
            param.Add(DBFactory.Helper.FormatParameter("FaultCode", DbType.String, entity.FaultCode));
            param.Add(DBFactory.Helper.FormatParameter("FaultReason", DbType.String, entity.FaultReason));
            param.Add(DBFactory.Helper.FormatParameter("MouldId", DbType.String, entity.MouldId));
            param.Add(DBFactory.Helper.FormatParameter("NewMouldId", DbType.String, entity.NewMouldId));
            param.Add(DBFactory.Helper.FormatParameter("MouldId1", DbType.String, entity.MouldId1));
            param.Add(DBFactory.Helper.FormatParameter("NewMouldId1", DbType.String, entity.NewMouldId1));
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
        /// 判断设备是否处于报修中(未确认的报修单大于0)
        /// </summary>
        /// <param name="DeviceId"></param>
        /// <returns></returns>
        public bool IsRepair(string DeviceId)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT COUNT(1) FROM t_RepairForm WHERE DeviceId=@DeviceId AND FormStatus<60 ");
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("DeviceId", DbType.String, DeviceId));
            int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(sbSql.ToString(), param.ToArray()));
            return result > 0;
        }
        /// <summary>
        /// 通过模具号查询模具是否存在
        /// </summary>
        /// <param name="DeviceId">模具编号，多模具用/分割</param>
        /// <returns></returns>
        public bool IsExistsModel(string DeviceId)
        {
            if (DeviceId != "")
            {
                StringBuilder sbSql = new StringBuilder();
                
                sbSql.Append("select count(*) from t_Device where CategoryId='B01' and DeviceId=@DeviceId");
                List <DbParameter> param = new List<DbParameter>();
                param.Add(DBFactory.Helper.FormatParameter("DeviceId", DbType.String, DeviceId));
                int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(sbSql.ToString(), param.ToArray()));
                return result > 0;
            }
            else
            {
                return true;
            }
             
        }
        /// <summary>
        /// 判断维修单状态是否准确
        /// </summary>
        /// <param name="repairformno"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool IsRepariStatus(string repairformno,string status)
        {
            if (repairformno != "")
            {
                StringBuilder sbSql = new StringBuilder();

                sbSql.Append("select count(*) from t_RepairForm where RepairFormNO=@repairformno and FormStatus=@status");
                List<DbParameter> param = new List<DbParameter>();
                param.Add(DBFactory.Helper.FormatParameter("repairformno", DbType.String, repairformno));
                param.Add(DBFactory.Helper.FormatParameter("status", DbType.String, status));
                int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(sbSql.ToString(), param.ToArray()));
                return result > 0;
            }
            else
            {
                return true;
            }

        }
        /// <summary>
        /// 删除维修单
        /// </summary>
        /// <param name="repairformno"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool DeleteRepariStatus(string repairformno, string status)
        {
             
                StringBuilder sbSql = new StringBuilder();

                sbSql.Append("delete   t_RepairForm where RepairFormNO=@repairformno and FormStatus=@status");
                List<DbParameter> param = new List<DbParameter>();
                param.Add(DBFactory.Helper.FormatParameter("repairformno", DbType.String, repairformno));
                param.Add(DBFactory.Helper.FormatParameter("status", DbType.String, status));
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
        /// 报修单是否存在
        /// </summary>
        /// <param name="MacAddress"></param>
        /// <returns></returns>
        public bool IsExists(RepairFormEntity entity)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT COUNT(1) FROM t_RepairForm WHERE RepairFormNO=@RepairFormNO ");
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("RepairFormNO", DbType.String, entity.RepairFormNO));
            int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(sbSql.ToString(), param.ToArray()));
            return result > 0;
        }

        /// <summary>
        /// 返修次数
        /// </summary>
        /// <param name="RepairFormNO"></param>
        /// <returns></returns>
        public int RebackCount(string RepairFormNO)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat("SELECT COUNT(1) FROM t_RepairForm WHERE RepairFormNO like N'{0}%' ", RepairFormNO);
            return Convert.ToInt32(DBFactory.Helper.ExecuteScalar(sbSql.ToString(), null));
        }

        /// <summary>
        /// 报修员确认
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Confirm(RepairFormEntity entity)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("Update t_RepairForm");
            sbSql.Append("   SET FormStatus=60,ConfirmUser=@ConfirmUser,ConfirmTime=GETDATE() ");
            sbSql.Append(" WHERE RepairFormNO=@RepairFormNO;");
            sbSql.Append("Update t_RepairRecord ");
            sbSql.Append("   SET RepairStatus=60,ConfirmUser=@ConfirmUser ");
            sbSql.Append(" WHERE RepairFormNO=@RepairFormNO;");
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("RepairFormNO", DbType.String, entity.RepairFormNO));
            param.Add(DBFactory.Helper.FormatParameter("ConfirmUser", DbType.String, entity.ConfirmUser));
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
        /// 指派
        /// </summary>
        /// <param name="RepairFormNO"></param>
        /// <param name="oldFormStatus"></param>
        /// <param name="newFormStatus"></param>
        /// <param name="RepairmanId"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool RepairAssign(string RepairFormNO, string oldFormStatus, string newFormStatus, string RepairmanId, Trans t = null)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("Update t_RepairForm");
            sbSql.Append("   SET FormStatus=@newFormStatus,RepairmanId=@RepairmanId ,RepairmanName=(select top 1 RepairmanName from  t_Repairman where RepairmanId=@RepairmanId and RepairmanName is not null )");
            sbSql.Append(" WHERE RepairFormNO=@RepairFormNO AND FormStatus=@oldFormStatus");
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("RepairFormNO", DbType.String, RepairFormNO));
            param.Add(DBFactory.Helper.FormatParameter("RepairmanId", DbType.String, RepairmanId));

            param.Add(DBFactory.Helper.FormatParameter("oldFormStatus", DbType.String, oldFormStatus));
            param.Add(DBFactory.Helper.FormatParameter("newFormStatus", DbType.String, newFormStatus));
            if (t != null)
            {
                return DBFactory.Helper.ExecuteNonQuery(sbSql.ToString(), param.ToArray(), t) > 0;
            }
            else
            {
                return DBFactory.Helper.ExecuteNonQuery(sbSql.ToString(), param.ToArray()) > 0;
            }
        }

        public bool RepairManReject(string RepairFormNO, string oldFormStatus, string newFormStatus, string RebackType, string RebackReason, Trans t = null)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("Update t_RepairForm");
            sbSql.Append("   SET FormStatus=@newFormStatus,RebackReason=@RebackReason,RebackType=@RebackType,RepairmanId=null ");
            sbSql.Append(" WHERE RepairFormNO=@RepairFormNO AND FormStatus=@oldFormStatus");
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("RepairFormNO", DbType.String, RepairFormNO));
            param.Add(DBFactory.Helper.FormatParameter("RebackType", DbType.String, RebackType));
            param.Add(DBFactory.Helper.FormatParameter("RebackReason", DbType.String, RebackReason));
            param.Add(DBFactory.Helper.FormatParameter("oldFormStatus", DbType.String, oldFormStatus));
            param.Add(DBFactory.Helper.FormatParameter("newFormStatus", DbType.String, newFormStatus));
            if (t != null)
            {
                return DBFactory.Helper.ExecuteNonQuery(sbSql.ToString(), param.ToArray(), t) > 0;
            }
            else
            {
                return DBFactory.Helper.ExecuteNonQuery(sbSql.ToString(), param.ToArray()) > 0;
            }
        }

        /// <summary>
        /// 报修状态更改
        /// </summary>
        /// <param name="RepairFormNO">报修单</param>
        /// <param name="oldFormStatus">0-开立(待指派)；1-已指派(待维修)；2-已维修（待确认）；3-已维修（待QC确认）；4-已确认</param>
        /// <param name="newFormStatus">0-开立(待指派)；1-已指派(待维修)；2-已维修（待确认）；3-已维修（待QC确认）；4-已确认</param>
        /// <returns></returns>
        public bool SetFormStatus(string RepairFormNO, string oldFormStatus, string newFormStatus, string RebackType, string RebackReason, string IPQCNumber, Trans t = null)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("Update t_RepairForm");
            sbSql.Append("   SET FormStatus=@newFormStatus,RebackReason=@RebackReason,RebackType=@RebackType,IPQCNumber=@IPQCNumber ");
            sbSql.Append(" WHERE RepairFormNO=@RepairFormNO AND FormStatus=@oldFormStatus");
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("RepairFormNO", DbType.String, RepairFormNO));
            param.Add(DBFactory.Helper.FormatParameter("RebackType", DbType.String, RebackType));
            param.Add(DBFactory.Helper.FormatParameter("RebackReason", DbType.String, RebackReason));
            param.Add(DBFactory.Helper.FormatParameter("IPQCNumber", DbType.String, IPQCNumber));
            param.Add(DBFactory.Helper.FormatParameter("oldFormStatus", DbType.String, oldFormStatus));
            param.Add(DBFactory.Helper.FormatParameter("newFormStatus", DbType.String, newFormStatus));
            if (t != null)
            {
                return DBFactory.Helper.ExecuteNonQuery(sbSql.ToString(), param.ToArray(), t) > 0;
            }
            else
            {
                return DBFactory.Helper.ExecuteNonQuery(sbSql.ToString(), param.ToArray()) > 0;
            }
        }

        /// <summary>
        /// 添加指派记录
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool AddRepairAssignLog(RepairAssignLogEntity entity, Trans t)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("Insert into t_RepairAssignLog(RepairFormNO,LeaderUserId,AssignTime,AssignUser)");
            sbSql.Append("VALUES(@RepairFormNO,@LeaderUserId,GETDATE(),@AssignUser)");
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("RepairFormNO", DbType.String, entity.RepairFormNO));
            param.Add(DBFactory.Helper.FormatParameter("LeaderUserId", DbType.String, entity.LeaderUserId));
            param.Add(DBFactory.Helper.FormatParameter("AssignUser", DbType.String, entity.AssignUser));
            return DBFactory.Helper.ExecuteNonQuery(sbSql.ToString(), param.ToArray(), t) > 0;
        }

    }
}
