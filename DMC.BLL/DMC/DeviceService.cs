using DMC.DAL;
using DMC.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DMC.BLL
{
    public class DeviceService
    {
        private DeviceDAL _dal = new DeviceDAL();
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
            return pageView.PageView("t_Device", "AutoId", pageIndex, pageSize, "*", "AutoId DESC", Where, out total, out pageCount);
        }

        /// <summary>
        /// 新增设备
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string NewDevice(DeviceEntity entity)
        {
            string msg = string.Empty;
            try
            {
                //1.检查必输字段是否为空
                if (string.IsNullOrWhiteSpace(entity.CategoryId) ||
                    string.IsNullOrWhiteSpace(entity.DeviceId))
                {
                    msg = "设备编号和类别请维护完整";
                }
                else if (_dal.IsExists(entity))
                {
                    msg = "设备编号已存在，无需再添加";
                }
                else if (!_dal.NewDevice(entity))
                {
                    msg = "新增设备保存数据库失败";
                }
            }
            catch
            {
                msg = "新增设备异常";
            }
            return msg;
        }

        /// <summary>
        /// 修改设备
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string UpdateDevice(DeviceEntity entity)
        {
            string msg = string.Empty;
            try
            {
                //1.检查必输字段是否为空
                if (string.IsNullOrWhiteSpace(entity.CategoryId) ||
                    string.IsNullOrWhiteSpace(entity.DeviceId))
                {
                    msg = "设备编号和类别请维护完整";
                }
                else if (!_dal.IsExists(entity))
                {
                    msg = "设备编号已删除";
                }
                else if (!_dal.UpdateDevice(entity))
                {
                    msg = "更新设备保存数据库失败";
                }
            }
            catch
            {
                msg = "更新设备异常";
            }
            return msg;
        }

        /// <summary>
        /// 删除设备
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string DeleteDevice(DeviceEntity entity)
        {
            string msg = string.Empty;
            try
            {
                //1.检查必输字段是否为空
                if (string.IsNullOrWhiteSpace(entity.DeviceId))
                {
                    msg = "请选择需要删除的设备";
                }
                else if (!_dal.IsExists(entity))
                {
                    msg = "设备已删除";
                }
                else if (!_dal.DeleteDevice(entity))
                {
                    msg = "设备删除数据库失败";
                }
            }
            catch
            {
                msg = "设备删除异常";
            }
            return msg;
        }
        /// <summary>
        /// 通过模具号查询
        /// </summary>
        /// <param name="DeviceId"></param>
        /// <returns></returns>
        public string IsExistsModel(string DeviceId)
        {
            string msg = string.Empty;
            try
            {
                if(!_dal.IsExistsModel(DeviceId))
                {
                    msg = "模板编号不存在";
                }
            }
            catch
            {
                msg = "模具数据异常";
            }
            return msg;
        }
    }
}
