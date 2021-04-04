namespace DMC.DAL.Script
{
    public interface IJobDetail
    {
        /// <summary>
        /// 添加
        /// <summary>
        string Addt_sJobDetail { get; }
        /// <summary>
        /// 修改
        /// <summary>
        string Modt_sJobDetail { get; }
        /// <summary>
        /// 啟動或停止任務
        /// </summary>
        string RunOrStop { get; }
        /// <summary>
        /// ?除
        /// <summary>
        string Delt_sJobDetail { get; }
        /// <summary>
        /// ??是否存在
        /// <summary>
        string IsExitt_sJobDetail { get; }
        /// <summary>
        /// 根据查?返回?据集
        /// <summary>
        string GetListt_sJobDetail { get; }
        /// <summary>
        /// 取得所有?料列表
        /// <summary>
        string GetAllt_sJobDetail { get; }
    }
}
