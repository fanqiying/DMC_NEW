namespace DMC.DAL.Script
{
    public interface ITaskDetails
    {
        /// <summary>
        /// 添加
        /// <summary>
        string Addt_sTaskDetails { get; }
        /// <summary>
        /// 修改
        /// <summary>
        string Modt_sTaskDetails { get; }
        /// <summary>
        ///刪除
        /// <summary>
        string Delt_sTaskDetails { get; }
        /// <summary>
        ///刪除
        /// <summary>
        string DelList { get; }
        string DelTaskIDList { get; }
        /// <summary>
        /// ??是否存在
        /// <summary>
        string IsExitt_sTaskDetails { get; }
        /// <summary>
        /// 根据查?返回?据集
        /// <summary>
        string GetListt_sTaskDetails { get; }
        /// <summary>
        /// 取得所有?料列表
        /// <summary>
        string GetAllt_sTaskDetails { get; }

        /// <summary>
        /// 獲取工作所包含的步驟類型
        /// </summary>
        string GetJobStepTypes { get; }
    }
}
