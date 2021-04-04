namespace DMC.DAL.Script
{
    public interface ITaskFrequency
    {
        /// <summary>
        /// 添加
        /// <summary>
       string Addt_sTaskFrequency { get; }
        /// <summary>
        /// 修改
        /// <summary>
       string Modt_sTaskFrequency { get; }
        /// <summary>
        /// ?除
        /// <summary>
       string Delt_sTaskFrequency { get; }
        /// <summary>
        /// ??是否存在
        /// <summary>
       string IsExitt_sTaskFrequency { get; }
        /// <summary>
        /// 根据查?返回?据集
        /// <summary>
       string GetListt_sTaskFrequency { get; }
        /// <summary>
        /// 取得所有?料列表
        /// <summary>
       string GetAllt_sTaskFrequency { get; }
    }
}
