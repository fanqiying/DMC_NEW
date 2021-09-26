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
    /// 维修记录处理
    /// </summary>
    public class RepairRecordDAL
    {
        /// <summary>
        /// 添加维修记录
        /// </summary>
        /// <param name="RepairFormNO"></param>
        /// <param name="RepairmanId"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public int NewRepairRecod(string RepairFormNO, string RepairmanId, Trans t)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("Insert into t_RepairRecord(RepairFormNO,ApplyUserId,RepairmanId,RepairmanName,DeviceId,FaultTime,PositionId,PositionText,PhenomenaId,PhenomenaText,PositionId1,PositionText1,PhenomenaId1,PhenomenaText1,");
            sbSql.Append("FaultStatus,FaultCode,FaultReason,RepairSTime,RepairStatus,MouldId,NewMouldId,MouldId1,NewMouldId1)");
            sbSql.Append("SELECT RepairFormNO,ApplyUserId,@RepairmanId,(select top 1 RepairmanName from  t_Repairman where RepairmanId=@RepairmanId and RepairmanName is not null) ,DeviceId,isnull(RejectDate,FaultTime),PositionId,PositionText,PhenomenaId,PhenomenaText,PositionId1,PositionText1,PhenomenaId1,PhenomenaText1,");
            sbSql.Append("       FaultStatus,FaultCode,FaultReason,GETDATE(),'20',MouldId,NewMouldId,MouldId1,NewMouldId1 ");
            sbSql.Append("  FROM t_RepairForm ");
            sbSql.Append(" WHERE RepairFormNO=@RepairFormNO;");
            sbSql.Append(" select @@IDENTITY; ");//返回最新的自增序号
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("RepairFormNO", DbType.String, RepairFormNO));
            param.Add(DBFactory.Helper.FormatParameter("RepairmanId", DbType.String, RepairmanId));

            return Convert.ToInt32(DBFactory.Helper.ExecuteScalar(sbSql.ToString(), param.ToArray(), t));
        }

        /// <summary>
        /// 维修确认(提交生产员|提交QC)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Confirm(RepairRecordEntity entity, Trans t)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("Update t_RepairRecord");
            sbSql.Append("   SET PositionId=@PositionId,PositionText=@PositionText,PhenomenaId=@PhenomenaId,PhenomenaText=@PhenomenaText, ");
            sbSql.Append("       FaultCode=@FaultCode,FaultReason=@FaultReason,FaultAnalysis=@FaultAnalysis,RepairETime=GETDATE(),");
            sbSql.Append("       RepairStatus=@RepairStatus,IsRest=(case when RepairSTime<cast(convert(varchar(10),RepairSTime,121)+ ' 12:30:00' as datetime) and GetDate()>cast(convert(varchar(10),RepairSTime,121)+ ' 13:30:00' as datetime) then 1 else 0 end)");
            sbSql.Append(" WHERE RepairFormNO=@RepairFormNO AND AutoId=@AutoId ");
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("RepairFormNO", DbType.String, entity.RepairFormNO));
            param.Add(DBFactory.Helper.FormatParameter("AutoId", DbType.Int32, entity.AutoId));
            param.Add(DBFactory.Helper.FormatParameter("PositionId", DbType.String, entity.PositionId));
            param.Add(DBFactory.Helper.FormatParameter("PositionText", DbType.String, entity.PositionText));
            param.Add(DBFactory.Helper.FormatParameter("PhenomenaId", DbType.String, entity.PhenomenaId));
            param.Add(DBFactory.Helper.FormatParameter("PhenomenaText", DbType.String, entity.PhenomenaText));
            param.Add(DBFactory.Helper.FormatParameter("FaultCode", DbType.String, entity.FaultCode));
            param.Add(DBFactory.Helper.FormatParameter("FaultReason", DbType.String, entity.FaultReason));
            param.Add(DBFactory.Helper.FormatParameter("FaultAnalysis", DbType.String, entity.FaultAnalysis));
            param.Add(DBFactory.Helper.FormatParameter("RepairStatus", DbType.String, entity.RepairStatus));
            return DBFactory.Helper.ExecuteNonQuery(sbSql.ToString(), param.ToArray(), t) > 0;
        }

        /// <summary>
        /// 系统挂单
        /// </summary>
        /// <param name="RepairFormNO"></param>
        /// <returns></returns>
        public bool Reject(int AutoId, string RepairFormNO, string RebackType, string RebackReason, Trans t)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("Update t_RepairRecord ");
            sbSql.Append("   SET RepairETime=GETDATE(),RepairStatus=62,RebackType=@RebackType,RebackReason=@RebackReason ");
            sbSql.Append("   ,IsRest=(case when RepairSTime<cast(convert(varchar(10),RepairSTime,121)+ ' 12:30:00' as datetime) and GetDate()>cast(convert(varchar(10),RepairSTime,121)+ ' 13:30:00' as datetime) then 1 else 0 end) ");
            sbSql.Append(" WHERE RepairFormNO=@RepairFormNO AND AutoId=@AutoId ");
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("RepairFormNO", DbType.String, RepairFormNO));
            param.Add(DBFactory.Helper.FormatParameter("AutoId", DbType.Int32, AutoId));
            param.Add(DBFactory.Helper.FormatParameter("RebackType", DbType.String, RebackType));
            param.Add(DBFactory.Helper.FormatParameter("RebackReason", DbType.String, RebackReason));
            return DBFactory.Helper.ExecuteNonQuery(sbSql.ToString(), param.ToArray(), t) > 0;
        }

        /// <summary>
        /// QC确认
        /// </summary>
        /// <param name="RepairFormNO"></param>
        /// <returns></returns>
        public bool QCConfirm(int AutoId, string RepairFormNO, string IPQCNumber, Trans t)
        {
            StringBuilder sbSql = new StringBuilder();
            
            sbSql.Append("Update t_RepairRecord ");
            sbSql.Append("   SET QCConfirmTime=GETDATE(),RepairStatus=50,IPQCNumber=@IPQCNumber,IsRest=(case when RepairSTime<cast(convert(varchar(10),RepairSTime,121)+ ' 12:30:00' as datetime) and GetDate()>cast(convert(varchar(10),RepairSTime,121)+ ' 13:30:00' as datetime) then 1 else 0 end) ");
            sbSql.Append(" WHERE RepairFormNO=@RepairFormNO AND AutoId=@AutoId; ");
           
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("RepairFormNO", DbType.String, RepairFormNO));
            
            param.Add(DBFactory.Helper.FormatParameter("AutoId", DbType.Int32, AutoId));
            param.Add(DBFactory.Helper.FormatParameter("IPQCNumber", DbType.String, IPQCNumber));
            return DBFactory.Helper.ExecuteNonQuery(sbSql.ToString(), param.ToArray(), t) > 0;
        }
        /// <summary>
        /// 生产组长确认
        /// </summary>
        /// <param name="RepairFormNO"></param>
        /// <returns></returns>
        public bool ProductionConfirm(int AutoId, string RepairFormNO, Trans t)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("Update t_RepairRecord ");
            sbSql.Append("   SET RepairETime=GETDATE(),RepairStatus=60,IsRest=(case when RepairSTime<cast(convert(varchar(10),RepairSTime,121)+ ' 12:30:00' as datetime) and GetDate()>cast(convert(varchar(10),RepairSTime,121)+ ' 13:30:00' as datetime) then 1 else 0 end) ");
            sbSql.Append(" WHERE RepairFormNO=@RepairFormNO AND AutoId=@AutoId ");
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("RepairFormNO", DbType.String, RepairFormNO));
            param.Add(DBFactory.Helper.FormatParameter("AutoId", DbType.Int32, AutoId));

            return DBFactory.Helper.ExecuteNonQuery(sbSql.ToString(), param.ToArray(), t) > 0;
        }

        /// <summary>
        /// 返修
        /// </summary>
        /// <param name="RepairFormNO"></param>
        /// <returns></returns>
        public bool LeaderReject(int AutoId, string RepairFormNO, string LeaderID,string RebackReason, string newStatus, Trans t)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("Update t_RepairRecord ");
            sbSql.Append("   SET confirmtime=GETDATE(),confirmuser=@LeaderID,RepairStatus=@newStatus,RebackReason=@RebackReason,IsRest=(case when RepairSTime<cast(convert(varchar(10),RepairSTime,121)+ ' 12:30:00' as datetime) and GetDate()>cast(convert(varchar(10),RepairSTime,121)+ ' 13:30:00' as datetime) then 1 else 0 end) ");
            sbSql.Append(" WHERE RepairFormNO=@RepairFormNO AND AutoId=@AutoId ");
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("RepairFormNO", DbType.String, RepairFormNO));
            param.Add(DBFactory.Helper.FormatParameter("RebackReason", DbType.String, RebackReason));
            param.Add(DBFactory.Helper.FormatParameter("LeaderID", DbType.String, LeaderID));
            param.Add(DBFactory.Helper.FormatParameter("newStatus", DbType.String, newStatus));
            param.Add(DBFactory.Helper.FormatParameter("AutoId", DbType.Int32, AutoId));
            return DBFactory.Helper.ExecuteNonQuery(sbSql.ToString(), param.ToArray(), t) > 0;
        }

        /// <summary>
        /// QC返修
        /// </summary>
        /// <param name="RepairFormNO"></param>
        /// <returns></returns>
        public bool QCRConfirm(int AutoId, string RepairFormNO, string RebackReason, string IPQCNumber, Trans t)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("Update t_RepairRecord ");
            sbSql.Append("   SET  QCConfirmTime=GETDATE(),RepairStatus=64,RebackReason=@RebackReason,IPQCNumber=@IPQCNumber,IsRest=(case when RepairSTime<cast(convert(varchar(10),RepairSTime,121)+ ' 12:30:00' as datetime) and GetDate()>cast(convert(varchar(10),RepairSTime,121)+ ' 13:30:00' as datetime) then 1 else 0 end) ");
            sbSql.Append(" WHERE RepairFormNO=@RepairFormNO AND AutoId=@AutoId ");
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("RepairFormNO", DbType.String, RepairFormNO));
            param.Add(DBFactory.Helper.FormatParameter("RebackReason", DbType.String, RebackReason));
            param.Add(DBFactory.Helper.FormatParameter("IPQCNumber", DbType.String, IPQCNumber));
            param.Add(DBFactory.Helper.FormatParameter("AutoId", DbType.Int32, AutoId));
            return DBFactory.Helper.ExecuteNonQuery(sbSql.ToString(), param.ToArray(), t) > 0;
        }

        /// <summary>
        /// 返修创建单号
        /// </summary>
        /// <param name="AutoId"></param>
        /// <param name="OldRepairFormNO"></param>
        /// <param name="NewRepairFormNO"></param>
        /// <param name="IPQCNumber"></param>
        /// <param name="newStatus">12 待指派(挂单),24 待维修(IPQC返修),25 待维修(组长返修)</param>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool NewRepairRecord(int AutoId, string OldRepairFormNO, string NewRepairFormNO, string IPQCNumber, string newStatus, Trans t)
        {
            StringBuilder sbSql = new StringBuilder();

            //创建报修记录
            sbSql.Append("INSERT INTO t_RepairForm(RepairFormNO,ApplyUserId,FaultTime,DeviceId,PositionId,PositionText,PositionId1,PhenomenaId,");
            sbSql.Append("PhenomenaText,PhenomenaId1,FaultStatus,FaultCode,FaultReason,FaultAnalysis,Intime,FormStatus,RebackType,RebackReason,IPQCNumber,RepairmanId,RepairmanName,PhenomenaText1,PositionText1,MouldId,NewMouldId,MouldId1,NewMouldId1)");
            sbSql.Append("SELECT @NewRepairFormNO RepairFormNO,ApplyUserId,getdate(),DeviceId,PositionId,PositionText,PositionId1,PhenomenaId,");
            sbSql.Append("        PhenomenaText,PhenomenaId1,FaultStatus,FaultCode,(select distinct ' '+  isnull(RebackReason,'') from t_RepairForm t where left(t.RepairFormNO,13) = left(t_RepairForm.RepairFormNO,13) and t.RebackReason is not null for xml path('')) FaultReason,FaultAnalysis,GetDate() Intime,");
            sbSql.Append("        @newStatus FormStatus,RebackType,RebackReason,IPQCNumber,RepairmanId,RepairmanName,PhenomenaText1,PositionText1,MouldId,NewMouldId,MouldId1,NewMouldId1 ");
            sbSql.Append("   FROM t_RepairForm");
            sbSql.Append("  WHERE RepairFormNO=@OldRepairFormNO;");
            //创建维修记录
            sbSql.Append("INSERT INTO t_RepairRecord(RepairFormNO,RebackReason,ApplyUserId,RepairmanId,RepairmanName,DeviceId,PositionId,PositionText,PhenomenaId,");
            sbSql.Append("PhenomenaText,FaultCode,FaultReason,FaultAnalysis,RepairSTime,RepairStatus,FaultTime,PhenomenaText1,PositionText1,MouldId,NewMouldId,MouldId1,NewMouldId1)");
            sbSql.Append("SELECT @NewRepairFormNO RepairFormNO,(select distinct ' '+  isnull(RebackReason,'') from t_RepairRecord t where left(t.RepairFormNO,13) = left(t_RepairRecord.RepairFormNO,13) and t.RebackReason is not null for xml path('')) RebackReason,ApplyUserId,RepairmanId,RepairmanName,DeviceId,PositionId,PositionText,PhenomenaId,");
            sbSql.Append("       PhenomenaText,FaultCode,FaultReason,FaultAnalysis,GetDate() RepairSTime,@newStatus RepairStatus,getdate(),PhenomenaText1,PositionText1,MouldId,NewMouldId,MouldId1,NewMouldId1 ");
            sbSql.Append("  FROM t_RepairRecord");
            sbSql.Append(" WHERE AutoId=@AutoId;");
            sbSql.Append("update t_RepairForm set RepairRecordId=(select max(autoid) from t_RepairRecord where t_RepairRecord.RepairFormNO=t_RepairForm.RepairFormNO)");
            sbSql.Append(" where RepairFormNO=@NewRepairFormNO;");
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("OldRepairFormNO", DbType.String, OldRepairFormNO));
            param.Add(DBFactory.Helper.FormatParameter("NewRepairFormNO", DbType.String, NewRepairFormNO));
            param.Add(DBFactory.Helper.FormatParameter("newStatus", DbType.String, newStatus));
            param.Add(DBFactory.Helper.FormatParameter("IPQCNumber", DbType.String, IPQCNumber));

            param.Add(DBFactory.Helper.FormatParameter("AutoId", DbType.Int32, AutoId));
            return DBFactory.Helper.ExecuteNonQuery(sbSql.ToString(), param.ToArray(), t) > 0;
        }


        /// <summary>
        /// 评价
        /// </summary>
        /// <param name="RepairFormNO"></param>
        /// <returns></returns>
        public bool LeaderAppraise(int AutoId, string RepairFormNO, string ConfirmUser, Trans t)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("Update t_RepairForm ");
            sbSql.Append("   SET FormStatus=60,ConfirmUser=@ConfirmUser,ConfirmTime=GETDATE() ");
            sbSql.Append(" WHERE RepairFormNO=@RepairFormNO;");
            sbSql.Append("Update t_RepairRecord ");
            sbSql.Append("  set RepairStatus=60,ConfirmUser=@ConfirmUser,ConfirmTime=GETDATE() ");
            sbSql.Append(" WHERE RepairFormNO=@RepairFormNO AND AutoId=@AutoId ");

            //最后一个返修单确认时，返修单状态完成
            string RepairFormNO1 = RepairFormNO.Substring(0, 13);
            sbSql.Append("Update t_RepairRecord ");
            sbSql.Append("   SET  RepairStatus=60");
            sbSql.AppendFormat(" WHERE  RepairFormNO  like  N'%{0}%' ",RepairFormNO1);
            //strWhere.AppendFormat(" AND (ApplyUserId like N'%{0}%')", context.Request.Params["ApplyUserId"]);
            sbSql.Append("Update t_RepairForm ");
            sbSql.Append("   SET  FormStatus=60");
            sbSql.AppendFormat(" WHERE  RepairFormNO  like  N'%{0}%' ", RepairFormNO1);
            //sbSql.Append(" WHERE CHARINDEX(RepairFormNO , @RepairFormNO1)>1 and RepairFormNO!=@RepairFormNO1; ");
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("RepairFormNO", DbType.String, RepairFormNO));
            param.Add(DBFactory.Helper.FormatParameter("RepairFormNO1", DbType.String, RepairFormNO1));
            param.Add(DBFactory.Helper.FormatParameter("ConfirmUser", DbType.String, ConfirmUser));
            param.Add(DBFactory.Helper.FormatParameter("AutoId", DbType.Int32, AutoId));
            return DBFactory.Helper.ExecuteNonQuery(sbSql.ToString(), param.ToArray(), t) > 0;
        }
    }
}
