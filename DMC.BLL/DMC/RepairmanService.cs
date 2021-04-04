using DMC.DAL;
using DMC.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DMC.BLL
{
    /// <summary>
    /// 维修员服务类
    /// </summary>
    public class RepairmanService
    {
        RepairmanDAL _dal = new RepairmanDAL();
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
            StringBuilder sbShow = new StringBuilder();
            sbShow.Append(@"[AutoId]
      ,[RepairmanId]
      ,[RepairmanName]
      ,[ClassType]
      ,[IsLeader]
      ,[IsWorking]
      ,case when isnull([WorkRangeTimeBegin],'')=''then '' else convert(varchar,cast([WorkRangeTimeBegin] as datetime),120) end [WorkRangeTimeBegin]
      ,[PhotoUrl]
      ,[YearMonth]
      ,case when isnull([WorkRangeTimeEnd],'')=''then '' else convert(varchar,cast([WorkRangeTimeEnd] as datetime),120) end [WorkRangeTimeEnd]
      ,[WorkDate]
      ,[WorkRangeTime]
      ,[WorkNum]");
            return pageView.PageView("t_Repairman", "AutoId", pageIndex, pageSize, sbShow.ToString(), "AutoId DESC", Where, out total, out pageCount);
        }
        /// <summary>
        /// 获取当前工作状态
        /// </summary>
        /// <returns></returns>
        public DataTable GetRepairmWorking()
        {
            if ((DateTime.Now.Hour > 8 && DateTime.Now.Hour < 19) ||
                (DateTime.Now.Hour == 7 && DateTime.Now.Minute >= 30) ||
                (DateTime.Now.Hour == 19 && DateTime.Now.Minute <= 30))
            {
                //白班
                return _dal.GetRepairmDayWorking();
            }
            else
            {
                //晚班
                return _dal.GetRepairmNightWorking();
            }
        }
        /// <summary>
        /// 获取值班人员
        /// </summary>
        /// <param name="ClassType"></param>
        /// <returns></returns>
        public DataTable GetOnDutyUser(string ClassType)
        {
            return _dal.GetOnDutyUser(ClassType);
        }

        /// <summary>
        /// 新增维修员
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string NewRepairman(RepairmanEntity entity)
        {
            string msg = string.Empty;
            try
            {
                //1.检查必输字段是否为空
                if (string.IsNullOrWhiteSpace(entity.RepairmanId) ||
                    string.IsNullOrWhiteSpace(entity.RepairmanName))
                {
                    msg = "维修员工号和姓名请维护完整";
                }
                else if (_dal.IsExists(entity))
                {
                    msg = "维修员已存在，无需再添加";
                }
                else if (!_dal.NewRepairman(entity))
                {
                    msg = "新增维修员保存数据库失败";
                }
            }
            catch
            {
                msg = "新增维修员异常";
            }
            return msg;
        }

        /// <summary>
        /// 修改维修员
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string UpdateRepairman(RepairmanEntity entity)
        {
            string msg = string.Empty;
            try
            {
                //1.检查必输字段是否为空
                if (string.IsNullOrWhiteSpace(entity.RepairmanId) ||
                    string.IsNullOrWhiteSpace(entity.RepairmanName))
                {
                    msg = "维修员工号和姓名请维护完整";
                }
                else if (!_dal.IsExists(entity))
                {
                    msg = "维修员编号已删除";
                }
                else if (!_dal.UpdateRepairman(entity))
                {
                    msg = "更新维修员保存数据库失败";
                }
            }
            catch
            {
                msg = "更新维修员异常";
            }
            return msg;
        }

        /// <summary>
        /// 删除维修员
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string DeleteRepairman(RepairmanEntity entity)
        {
            string msg = string.Empty;
            try
            {
                //1.检查必输字段是否为空
                if (string.IsNullOrWhiteSpace(entity.RepairmanId))
                {
                    msg = "请选择需要删除的维修员";
                }
                else if (!_dal.IsExists(entity))
                {
                    msg = "维修员已删除";
                }
                else if (!_dal.DeleteRepairman(entity))
                {
                    msg = "维修员删除数据库失败";
                }
            }
            catch
            {
                msg = "维修员删除异常";
            }
            return msg;
        }
        public string DeleteRepairman_WordDate(string workdate)
        {
            string msg = string.Empty;
            try
            {
                 if (!_dal.DeleteRepairman_WordDate(workdate))
                {
                    msg = "维修员删除数据库失败";
                }
            }
            catch
            {
                msg = "维修员删除异常";
            }
            return msg;
        }
        /// <summary>
        /// 值班设置
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string SetWorking(RepairmanEntity entity)
        {
            string msg = string.Empty;
            try
            {
                //1.检查必输字段是否为空
                if (string.IsNullOrWhiteSpace(entity.RepairmanId))
                {
                    msg = "请选择需要值班状态变更的维修员";
                }
                else if (!_dal.SetWorking(entity))
                {
                    msg = "值班状态变更数据库失败";
                }
            }
            catch
            {
                msg = "值班状态变更异常";
            }
            return msg;
        }
        /// <summary>
        /// 获取班次
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string GetClassType(RepairmanEntity entity)
        {
            string msg = string.Empty;
            try
            {
                //1.检查必输字段是否为空
                if (string.IsNullOrWhiteSpace(entity.RepairmanId))
                {
                    msg = "请选择需要值班状态变更的维修员";
                }
                msg = _dal.GetClassType(entity);

            }
            catch
            {
                msg = "值班状态变更异常";
            }
            return msg;
        }
    }
}
