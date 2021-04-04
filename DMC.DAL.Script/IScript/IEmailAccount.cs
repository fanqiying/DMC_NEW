namespace DMC.DAL.Script
{
    public interface IEmailAccount
    {
        /// <summary>
        /// 添加
        /// <summary>
       string Addt_sEmailAccount { get; }
        /// <summary>
        /// 修改
        /// <summary>
       string Modt_sEmailAccount { get; }
        /// <summary>
        /// ?除
        /// <summary>
       string Delt_sEmailAccount { get; }
        /// <summary>
        /// ??是否存在
        /// <summary>
       string IsExitt_sEmailAccount { get; }
        /// <summary>
        /// 根据查?返回?据集
        /// <summary>
       string GetListt_sEmailAccount { get; }
        /// <summary>
        /// 取得所有?料列表
        /// <summary>
       string GetAllt_sEmailAccount { get; }
    }
}
