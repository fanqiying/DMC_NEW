using DMC.DAL;
using DMC.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DMC.BLL
{
    public class RepairRecordService
    {
        RepairFormDAL _dal = new RepairFormDAL();
        RepairRecordDAL rrDal = new RepairRecordDAL();

        PageManage pageView = new PageManage();

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <param name="total"></param>
        /// <param name="Where"></param>
        /// <returns></returns>
        public DataTable Search(int pageSize, int pageIndex, out int pageCount, out int total, string Where = "")
        {
            return pageView.PageView("t_RepairRecord", "AutoId", pageIndex, pageSize, "*", "AutoId DESC", Where, out total, out pageCount);
        }

        /// <summary>
        /// 看板查询
        /// </summary>
        /// <returns></returns>
        public DataTable KanBan()
        {
            return _rfDal.KanBan();
        }

        public DataTable KanBan(int pageSize, int pageIndex, out int pageCount, out int total, string Where = "")
        {
            StringBuilder sbTable = new StringBuilder();
            sbTable.Append(@" t_RepairForm a left join 
                              t_RepairRecord b on a.RepairFormNO=b.RepairFormNO left join 
                              t_FaultPosition c on  a.PositionId=c.PPositionId and a.PhenomenaId=c.PositionId left join 
                              t_Device d on a.DeviceId=d.DeviceId ");
            StringBuilder sbShow = new StringBuilder();
            sbShow.Append(" A.*,isnull(repairstatus,0)repairstatus,c.GradeTime,c.Grade StandGrade,datediff(minute ,a.faulttime, GetDate()) manhoure,d.KeepUserId ");
            sbShow.Append(" ,(case   FormStatus when 10 then datediff(minute,a.Intime,GETDATE())    when 40 then  datediff(minute, b.RepairSTime, GETDATE())   when 30 then datediff(minute, b.RepairETime, GETDATE())    when 50 then datediff(minute, b.QCConfirmTime, GETDATE())	else 0    end) rowcolor");
            return pageView.PageView(sbTable.ToString(), "a.AutoId", pageIndex, pageSize, sbShow.ToString(), "a.AutoId DESC", Where, out total, out pageCount);
        }

        public DataTable SearchHoure(int pageSize, int pageIndex, out int pageCount, out int total, string Where = "")
        {
            StringBuilder sbTable = new StringBuilder();
            sbTable.Append(" t_RepairRecord a left join t_FaultPosition b on a.PhenomenaId=b.PositionId ");
            StringBuilder sbShow = new StringBuilder();
            sbShow.Append(" A.*,b.GradeTime,b.Grade StandGrade,datediff(minute ,RepairSTime,isnull(RepairETime,GetDate()))-(case when RepairSTime<cast(convert(varchar(10),RepairSTime,121)+ ' 12:30:00' as datetime) and isnull(RepairETime,GetDate())>cast(convert(varchar(10),RepairSTime,121)+ ' 13:30:00' as datetime) then 1 else 0 end)*40 manhoure ");
            return pageView.PageView(sbTable.ToString(), "a.AutoId", pageIndex, pageSize, sbShow.ToString(), "a.AutoId DESC", Where, out total, out pageCount);
        }

        /// <summary>
        /// 评分计算
        /// </summary>
        /// <param name="AutoId"></param>
        public void AppraiseGrade(int AutoId)
        {
        }

        /// <summary>
        /// 维修员挂单
        /// </summary>
        /// <param name="RepairFormNO"></param>
        /// <returns></returns>
        public string Reject(string RepairFormNO, int AutoId, string RebackType, string RebackReason, string OldRepairStatus)
        {
            string msg = string.Empty;
            if (string.IsNullOrWhiteSpace(RepairFormNO))
            {
                msg = "请指定维修单号！";
            }
            else
            {
                bool isSuccess = true;
                using (Trans t = new Trans())
                {
                    try
                    {
                        //1.更新状态
                        isSuccess = _dal.RepairManReject(RepairFormNO, OldRepairStatus, "12", RebackType, RebackReason, t);
                        //2.挂单
                        isSuccess = isSuccess && rrDal.Reject(AutoId, RepairFormNO, RebackType, RebackReason, t);
                        if (!isSuccess)
                        {
                            msg = "报修单挂单失败";
                        }
                    }
                    catch
                    {
                        msg = "报修单挂单异常";
                        isSuccess = false;
                    }
                    if (isSuccess)
                        t.Commit();
                    else
                        t.RollBack();
                }
            }
            return msg;
        }

        /// <summary>
        /// 报修单提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string Confirm(RepairRecordEntity entity, string OldRepairStatus)
        {
            string msg = string.Empty;
            try
            {
                //1.检查必输字段是否为空
                if (string.IsNullOrWhiteSpace(entity.RepairFormNO) ||
                    string.IsNullOrWhiteSpace(entity.FaultAnalysis))
                {
                    msg = "请选择需要提交的报修单号，并填写故障分析！";
                }
                else
                {
                    bool isSuccess = true;
                    using (Trans t = new Trans())
                    {
                        try
                        {
                            //1.更新状态
                            isSuccess = _dal.SetFormStatus(entity.RepairFormNO, OldRepairStatus, entity.RepairStatus, "", "", "", t);
                            //2.提交
                            isSuccess = isSuccess && rrDal.Confirm(entity, t);
                            if (!isSuccess)
                            {
                                msg = "报修单提交失败";
                            }
                        }
                        catch
                        {
                            msg = "报修单提交异常";
                            isSuccess = false;
                        }
                        if (isSuccess)
                            t.Commit();
                        else
                            t.RollBack();
                    }
                }
            }
            catch
            {
                msg = "报修单提交异常";
            }
            return msg;
        }

        /// <summary>
        /// QC确认
        /// </summary>
        /// <param name="RepairFormNO"></param>
        /// <returns></returns>
        public string QCConfirm(int AutoId, string RepairFormNO, string IPQCNumber)
        {
            string msg = string.Empty;
            if (string.IsNullOrWhiteSpace(RepairFormNO))
            {
                msg = "请指定QC确认的维修单号！";
            }
            else
            {
                bool isSuccess = true;
                using (Trans t = new Trans())
                {
                    try
                    {
                        //1.更新状态
                        isSuccess = _dal.SetFormStatus(RepairFormNO, "40", "50", "", "", "", t);
                        //2.挂单
                        isSuccess = isSuccess && rrDal.QCConfirm(AutoId, RepairFormNO, IPQCNumber, t);
                        if (!isSuccess)
                        {
                            msg = "QC确认失败";
                        }
                    }
                    catch(Exception ex)
                    {
                        msg = "QC确认异常";
                        isSuccess = false;
                    }
                    if (isSuccess)
                        t.Commit();
                    else
                        t.RollBack();
                }
            }
            return msg;
        }
        /// <summary>
        /// 生产组长确认
        /// </summary>
        /// <param name="RepairFormNO"></param>
        /// <returns></returns>
        public string ProductionConfirm(int AutoId, string RepairFormNO)
        {
            string msg = string.Empty;
            if (string.IsNullOrWhiteSpace(RepairFormNO))
            {
                msg = "请指定QC确认的维修单号！";
            }
            else
            {
                bool isSuccess = true;
                using (Trans t = new Trans())
                {
                    try
                    {
                        //1.更新状态
                        isSuccess = _dal.SetFormStatus(RepairFormNO, "50", "60", "", "", "", t);
                        //2.挂单
                        isSuccess = isSuccess && rrDal.ProductionConfirm(AutoId, RepairFormNO, t);
                        if (!isSuccess)
                        {
                            msg = "QC确认失败";
                        }
                    }
                    catch
                    {
                        msg = "QC确认异常";
                        isSuccess = false;
                    }
                    if (isSuccess)
                        t.Commit();
                    else
                        t.RollBack();
                }
            }
            return msg;
        }
        RepairFormDAL _rfDal = new RepairFormDAL();
        /// <summary>
        /// QC驳回返修
        /// </summary>
        /// <param name="RepairFormNO"></param>
        /// <returns></returns>
        public string QCRConfirm(int AutoId, string RepairFormNO, string RebackReason, string IPQCNumber)
        {
            string msg = string.Empty;
            if (string.IsNullOrWhiteSpace(RepairFormNO) ||
                string.IsNullOrWhiteSpace(RebackReason))
            {
                msg = "请指定需要返修的维修单号，并备注返修原因！";
            }
            else
            {
                bool isSuccess = true;
                //查询有多少比历史单号
                string FormNo = RepairFormNO.Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries)[0];
                int RebackCount = _rfDal.RebackCount(FormNo);
                string NewRepairFormNO = string.Format("{0}_R{1}", FormNo, RebackCount);
                using (Trans t = new Trans())
                {
                    try
                    {
                        //1.QC返修，原单状态也更改为QC已确认
                        isSuccess = _dal.SetFormStatus(RepairFormNO, "40", "64", "", RebackReason, IPQCNumber, t);
                        //2.QC返修
                        isSuccess = isSuccess && rrDal.QCRConfirm(AutoId, RepairFormNO, RebackReason, IPQCNumber, t);
                        //3.创建新的报修与维修信息
                        isSuccess = isSuccess && rrDal.NewRepairRecord(AutoId, RepairFormNO, NewRepairFormNO, IPQCNumber, "24", t);
                        if (!isSuccess)
                        {
                            msg = "返修失败";
                        }
                    }
                    catch(Exception ex)
                    {
                        msg = "返修异常";
                        isSuccess = false;
                    }
                    if (isSuccess)
                        t.Commit();
                    else
                        t.RollBack();
                }
            }
            return msg;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AutoId"></param>
        /// <param name="RepairFormNO"></param>
        /// <param name="RebackReason"></param>
        /// <param name="oldStatus">驳回返修oldStatus=30 生产员返修，oldStatus=50生产组长返修</param>
        /// <param name="newStatus">驳回返修newStatus=61 生产员返修，newStatus=65生产组长返修</param>
        /// <param name="newRStatus">12 待指派(挂单),14 待指派(IPQC返修),15 待指派(组长返修)</param>
        /// <returns></returns>
        public string LeaderReject(int AutoId, string RepairFormNO, string RebackReason, string LeaderID,string oldStatus, string newStatus, string newRStatus)
        {
            string msg = string.Empty;
            if (string.IsNullOrWhiteSpace(RepairFormNO) ||
                string.IsNullOrWhiteSpace(RebackReason))
            {
                msg = "请指定需要返修的维修单号，并备注返修原因！";
            }
            else
            {
                bool isSuccess = true;
                //查询有多少比历史单号
                string FormNo = RepairFormNO.Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries)[0];
                int RebackCount = _rfDal.RebackCount(FormNo);
                string NewRepairFormNO = string.Format("{0}_R{1}", FormNo, RebackCount);
                using (Trans t = new Trans())
                {
                    try
                    {
                        //1.组长返修，原单状态也更改为已确认
                        isSuccess = _dal.SetFormStatus(RepairFormNO, oldStatus, newStatus, "", RebackReason, "", t);
                        //2.组长返修
                        isSuccess = isSuccess && rrDal.LeaderReject(AutoId, RepairFormNO, LeaderID,RebackReason, newStatus, t);
                        //3.创建新的报修与维修信息
                        isSuccess = isSuccess && rrDal.NewRepairRecord(AutoId, RepairFormNO, NewRepairFormNO, "", newRStatus, t);
                        if (!isSuccess)
                        {
                            msg = "返修失败";
                        }
                    }
                    catch
                    {
                        msg = "返修异常";
                        isSuccess = false;
                    }
                    if (isSuccess)
                        t.Commit();
                    else
                        t.RollBack();
                }
            }
            return msg;
        }
        /// <summary>
        /// 组长确认
        /// </summary>
        /// <param name="RepairFormNO"></param>
        /// <returns></returns>
        public string LeaderAppraise(int AutoId, string RepairFormNO, string ConfirmUser)
        {
            string msg = string.Empty;
            if (string.IsNullOrWhiteSpace(RepairFormNO))
            {
                msg = "请指定需要评价的维修单号，并备注评价内容！";
            }
            else
            {
                bool isSuccess = true;
                using (Trans t = new Trans())
                {
                    try
                    {
                        //1.评价
                        isSuccess = rrDal.LeaderAppraise(AutoId, RepairFormNO, ConfirmUser, t);
                        if (!isSuccess)
                        {
                            msg = "评价失败";
                        }
                    }
                    catch(Exception ex)
                    {
                        msg = "评价异常";
                        isSuccess = false;
                    }
                    if (isSuccess)
                        t.Commit();
                    else
                        t.RollBack();
                }
            }
            return msg;
        }
    }
}
