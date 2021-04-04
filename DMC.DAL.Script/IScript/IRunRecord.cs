namespace DMC.DAL.Script
{
    public interface IRunRecord
    {
        /// <summary>
        /// 添加
        /// <summary>
        string Addt_sRunRecord { get; }
        /// <summary>
        /// 修改
        /// <summary>
        string Modt_sRunRecord { get; }
        /// <summary>
        /// ?除
        /// <summary>
        string Delt_sRunRecord { get; }
        /// <summary>
        /// ??是否存在
        /// <summary>
        string IsExitt_sRunRecord { get; }
        /// <summary>
        /// 根据查?返回?据集
        /// <summary>
        string GetListt_sRunRecord { get; }
        /// <summary>
        /// 取得所有?料列表
        /// <summary>
        string GetAllt_sRunRecord { get; }

        /// <summary>
        /// 添加工作日誌
        /// </summary>
        string AddJobRecord { get; }
        /// <summary>
        /// 
        /// </summary>
        string UpdateJobRecord { get; }
    }
}
