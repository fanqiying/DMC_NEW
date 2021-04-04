using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMC.DAL.Script
{
    public interface ILenovoPart
    {
        /// <summary>
        /// 新增
        /// </summary>
        string Add { get; }

        /// <summary>
        /// 修改
        /// </summary>
        string Edit { get; }

        /// <summary>
        /// 刪除
        /// </summary>
        string Delete { get; }

        /// <summary>
        /// 驗證是否重复
        /// </summary>
        string Exists { get; }
    }
}
