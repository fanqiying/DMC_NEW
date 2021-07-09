using DMC.DAL;
using DMC.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DMC.BLL
{
    public class RepairFormService
    {
        SystemNoDAL snDal = new SystemNoDAL();
        RepairFormDAL _dal = new RepairFormDAL();
        RepairRecordDAL rrdal = new RepairRecordDAL();
        PageManage pageView = new PageManage();

        /// <summary>
        /// 获取待指派的数量
        /// </summary>
        /// <returns></returns>
        public DataTable GetAssignQty()
        {
            return _dal.GetAssignQty();
        }
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
            return pageView.PageView("t_RepairForm", "AutoId", pageIndex, pageSize, "*", "AutoId DESC", Where, out total, out pageCount);
        }

        /// <summary>
        /// 新增报修单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string NewRepairForm(RepairFormEntity entity)
        {
            string msg = string.Empty;
            try
            {
                entity.RepairFormNO = snDal.GetCategoryNoByThread("AVC", "RepairForm", "eqwi001");
                //1.检查必输字段是否为空
                if (string.IsNullOrWhiteSpace(entity.RepairFormNO))
                {
                    msg = "报修单号产生失败！";
                }
                else if (string.IsNullOrWhiteSpace(entity.DeviceId) ||
                    string.IsNullOrWhiteSpace(entity.ApplyUserId) ||
                    string.IsNullOrWhiteSpace(entity.FaultTime))
                {
                    msg = "申请人、设备编号、故障时间请维护完整";
                }
                else if (_dal.IsRepair(entity.DeviceId))
                {
                    msg = "设备编号处于报修中，无需再添加";
                }
                else if(!_dal.IsExistsModel(entity.MouldId))
                {
                     msg = "提交模具号不存在";  
                }
                else if (!_dal.IsExistsModel(entity.NewMouldId))
                {
                     msg = "提交模具号不存在"; 
                }
                else if (!_dal.NewRepairForm(entity))
                {
                    msg = "新增报修单保存数据库失败";
                }
            }
            catch (Exception ex)
            {
                
                msg = "新增报修单异常";
            }
            return msg;
        }

        /// <summary>
        /// 报修单确认
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string Confirm(RepairFormEntity entity)
        {
            string msg = string.Empty;
            try
            {
                //1.检查必输字段是否为空
                if (string.IsNullOrWhiteSpace(entity.RepairFormNO) ||
                    string.IsNullOrWhiteSpace(entity.ConfirmUser))
                {
                    msg = "请选择需要确认的报修单！";
                }
                else if (!_dal.IsExists(entity))
                {
                    msg = "报修单已删除，无法确认！";
                }
                else if (!_dal.Confirm(entity))
                {
                    msg = "报修单确认失败";
                }
            }
            catch
            {
                msg = "报修单确认异常";
            }
            return msg;
        }
        /// <summary>
        /// 指派维修员
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="AssignUser"></param>
        /// <returns></returns>
        public string RepairAssign(string RepairFormNO, string AssignUser, string LeaderUserId, string oldFormStatus)
        {
            string msg = string.Empty;
            if (string.IsNullOrWhiteSpace(RepairFormNO) ||
                string.IsNullOrWhiteSpace(AssignUser))
            {
                msg = "请指定维修单和维修员！";
            }
            else
            {
                bool isSuccess = true;
                using (Trans t = new Trans())
                {
                    try
                    {
                        //1.更新状态
                        isSuccess = _dal.RepairAssign(RepairFormNO, oldFormStatus, "20", AssignUser, t);
                        //2.写指派记录
                        isSuccess = isSuccess && _dal.AddRepairAssignLog(new RepairAssignLogEntity()
                        {
                            RepairFormNO = RepairFormNO,
                            AssignUser = AssignUser,
                            LeaderUserId = LeaderUserId
                        }, t);
                        //3.创建维修记录
                        isSuccess = isSuccess && rrdal.NewRepairRecod(RepairFormNO, AssignUser, t);
                        if (!isSuccess)
                        {
                            msg = "指派数据保存失败";
                        }
                    }
                    catch
                    {
                        msg = "指派异常";
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
