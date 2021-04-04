using DMC.DAL;
using DMC.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DMC.BLL
{
    public class FaultPositionService
    {
        FaultPositionDAL _dal = new FaultPositionDAL();
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
            return pageView.PageView("t_FaultPosition", "AutoId", pageIndex, pageSize, "*", "AutoId DESC", Where, out total, out pageCount);
        }

        /// <summary>
        /// 新增故障分类
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string NewFaultPosition(FaultPositionEntity entity)
        {
            string msg = string.Empty;
            try
            {
                //1.检查必输字段是否为空
                if (string.IsNullOrWhiteSpace(entity.PositionId) ||
                    string.IsNullOrWhiteSpace(entity.PositionName) ||
                    string.IsNullOrWhiteSpace(entity.PositionText))
                {
                    msg = "故障类别编号和名称请维护完整";
                }
                else if (_dal.IsExists(entity))
                {
                    msg = "故障类别编号或名称已存在，无需再添加";
                }
                else if (!_dal.NewFaultPosition(entity))
                {
                    msg = "新增故障分类保存数据库失败";
                }
            }
            catch
            {
                msg = "新增故障分类异常";
            }
            return msg;
        }

        /// <summary>
        /// 修改设备分类
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string UpdateFaultPosition(FaultPositionEntity entity)
        {
            string msg = string.Empty;
            try
            {
                //1.检查必输字段是否为空
                if (string.IsNullOrWhiteSpace(entity.PositionId) ||
                    string.IsNullOrWhiteSpace(entity.PositionName) ||
                    string.IsNullOrWhiteSpace(entity.PositionText))
                {
                    msg = "故障类别编号和名称请维护完整";
                }
                else if (!_dal.IsExists(entity))
                {
                    msg = "故障类别编号或名称已删除";
                }
                else if (!_dal.UpdateFaultPosition(entity))
                {
                    msg = "更新故障分类保存数据库失败";
                }
            }
            catch
            {
                msg = "更新故障分类异常";
            }
            return msg;
        }

        /// <summary>
        /// 获取故障位置
        /// </summary>
        /// <returns></returns>
        public DataTable GetFaultPositionMain()
        {
            return _dal.GetFaultPositionMain();
        }
        /// <summary>
        /// 获取故障分类
        /// </summary>
        /// <returns></returns>
        public DataTable GetFaultPositionNode(string PPositionId)
        {
            return _dal.GetFaultPositionNode(PPositionId);
        }
        /// <summary>
        /// 获取设备分类的树结构
        /// </summary>
        /// <returns></returns>
        public StringBuilder GetFaultPositionTree()
        {
            StringBuilder result = new StringBuilder();
            result.Append("[{\"id\":\"\",\"text\":\"主分类\",\"attributes\":{\"positiontext\":\"\"}");
            DataTable allFaultPosition = _dal.GetAllFaultPosition();
            if (allFaultPosition != null && allFaultPosition.Rows.Count > 0)
            {
                result.AppendFormat(",\"children\":{0}", GetFaultPositionTreeNode("", allFaultPosition));
            }
            result.Append("}]");
            return result;
        }

        /// <summary>
        /// 獲取菜單數據轉成JSON數據
        /// </summary>
        /// <param name="parentId">是否有父級菜單</param>
        /// <param name="dt">菜單數據實體</param>
        /// <returns>JSON字符串</returns>
        protected StringBuilder GetFaultPositionTreeNode(string PPositionId, DataTable dt)
        {
            StringBuilder content = new StringBuilder();
            DataRow[] drList = dt.Select("ISNULL(PPositionId,'')='" + PPositionId + "'");
            foreach (DataRow dr in drList)
            {
                if (content.Length > 0)
                    content.Append(",");
                content.Append("{");
                //分类编号
                content.AppendFormat("\"id\":\"{0}\"", dr["PositionId"].ToString());
                //分类名称
                content.AppendFormat(",\"text\":\"{0}\"", dr["PositionName"].ToString());
                //分类全称
                content.AppendFormat(",\"attributes\":{0}", "{\"positiontext\":\"" + dr["PositionText"].ToString() + "\"}");
                //子分类设置
                StringBuilder childrenStr = GetFaultPositionTreeNode(dr["PositionId"].ToString(), dt);
                if (childrenStr.Length > 0)
                {
                    content.AppendFormat(",\"children\":{0}", childrenStr);
                }
                else
                {
                    //只有有子节点时，才有展開和關閉操作
                    content.AppendFormat(",\"state\":\"{0}\"", "close");
                }
                content.Append("}");
            }
            if (content.Length > 0)
            {
                return new StringBuilder().Append("[").Append(content).Append("]");
            }
            return new StringBuilder();
        }
    }
}
