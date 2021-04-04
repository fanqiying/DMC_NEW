
namespace DMC.DAL.Script
{
   public interface IComp
    {
       /// <summary>
       /// 獲取公司列表
       /// </summary>
       string GetCompanyList { get; }
       /// <summary>
       /// 公司添加
       /// </summary>
       string AddComp { get; }
       /// <summary>
       /// 公司修改
       /// </summary>
       string ModComp { get; }
       /// <summary>
       /// 公司刪除
       /// </summary>
       string DelComp { get; }
       /// <summary>
       /// 判斷公司是否存在
       /// </summary>
       string IsExitComp { get; }
       /// <summary>
       /// 獲取公司
       /// </summary>
       string IGetCompany { get; }

       /// <summary>
       /// 根據公司ID獲取公司詳細信息 2015/6/18新增
       /// </summary>
       string GetCompanyInfoByID { get; }
    }
}
