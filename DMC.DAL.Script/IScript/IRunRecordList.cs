namespace DMC.DAL.Script
{
    public interface IRunRecordList
    {
        /// <summary>
        /// 添加
        /// <summary>
       string Addt_sRunRecordList { get; }
        /// <summary>
        /// 修改
        /// <summary>
       string Modt_sRunRecordList { get; }
        /// <summary>
        /// ?除
        /// <summary>
       string Delt_sRunRecordList { get; }
        /// <summary>
        /// ??是否存在
        /// <summary>
       string IsExitt_sRunRecordList { get; }
        /// <summary>
        /// 根据查?返回?据集
        /// <summary>
       string GetListt_sRunRecordList { get; }
        /// <summary>
        /// 取得所有?料列表
        /// <summary>
       string GetAllt_sRunRecordList { get; }
    }
}
