using DMC.DAL;
using DMC.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DMC.BLL
{
    public class FaultPhenomenaService
    {
        FaultPhenomenaDAL _dal = new FaultPhenomenaDAL();
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
            return pageView.PageView("t_FaultPhenomena", "AutoId", pageIndex, pageSize, "*", "AutoId DESC", Where, out total, out pageCount);
        }

        /// <summary>
        /// 新增故障分类
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string NewFaultPhenomena(FaultPhenomenaEntity entity)
        {
            string msg = string.Empty;
            try
            {
                //1.检查必输字段是否为空
                if (string.IsNullOrWhiteSpace(entity.CategoryId) ||
                    string.IsNullOrWhiteSpace(entity.CategoryName) ||
                    string.IsNullOrWhiteSpace(entity.CategoryText))
                {
                    msg = "故障类别编号和名称请维护完整";
                }
                else if (_dal.IsExists(entity))
                {
                    msg = "故障类别编号或名称已存在，无需再添加";
                }
                else if (!_dal.NewFaultPhenomena(entity))
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
        public string UpdateFaultPhenomena(FaultPhenomenaEntity entity)
        {
            string msg = string.Empty;
            try
            {
                //1.检查必输字段是否为空
                if (string.IsNullOrWhiteSpace(entity.CategoryId) ||
                    string.IsNullOrWhiteSpace(entity.CategoryName) ||
                    string.IsNullOrWhiteSpace(entity.CategoryText))
                {
                    msg = "故障类别编号和名称请维护完整";
                }
                else if (!_dal.IsExists(entity))
                {
                    msg = "故障类别编号或名称已删除";
                }
                else if (!_dal.UpdateFaultPhenomena(entity))
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
        /// 获取设备分类的树结构
        /// </summary>
        /// <returns></returns>
        public StringBuilder GetFaultPhenomenaTree()
        {
            StringBuilder result = new StringBuilder();
            result.Append("[{\"id\":\"\",\"text\":\"主分类\",\"attributes\":{\"categorytext\":\"\"}");
            DataTable allFaultPhenomena = _dal.GetAllFaultPhenomena();
            if (allFaultPhenomena != null && allFaultPhenomena.Rows.Count > 0)
            {
                result.AppendFormat(",\"children\":{0}", GetFaultPhenomenaTreeNode("", allFaultPhenomena));
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
        protected StringBuilder GetFaultPhenomenaTreeNode(string PCategoryId, DataTable dt)
        {
            StringBuilder content = new StringBuilder();
            DataRow[] drList = dt.Select("ISNULL(PCategoryId,'')='" + PCategoryId + "'");
            foreach (DataRow dr in drList)
            {
                if (content.Length > 0)
                    content.Append(",");
                content.Append("{");
                //分类编号
                content.AppendFormat("\"id\":\"{0}\"", dr["CategoryId"].ToString());
                //分类名称
                content.AppendFormat(",\"text\":\"{0}\"", dr["CategoryName"].ToString());
                //分类全称
                content.AppendFormat(",\"attributes\":{0}", "{\"categorytext\":\"" + dr["CategoryText"].ToString() + "\"}");
                //子分类设置
                StringBuilder childrenStr = GetFaultPhenomenaTreeNode(dr["CategoryId"].ToString(), dt);
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
