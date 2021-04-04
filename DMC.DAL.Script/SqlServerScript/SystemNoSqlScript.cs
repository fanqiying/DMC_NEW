using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;
namespace DMC.DAL.Script.SqlServer
{
    public class SystemNoSqlScript : It_SystemNo
    {
        /// <summary>
        /// 添加
        /// <summary>
        private string _Addt_SystemNo = " insert into t_SystemNo(CompanyId,ModuleType,ModularType,Category,ReceiptType,keyword,DateType,CodeLen,Mark,Usy,CreateUserId,CreateTime,CreateDeptId) values (@CompanyId,@ModuleType,@ModularType,@Category,@ReceiptType,@keyword,@DateType,@CodeLen,@Mark,@Usy,@CreateUserId,@CreateTime,@CreateDeptId)";
        public string Addt_SystemNo { get { return _Addt_SystemNo; } }
        /// <summary>
        /// 修改
        /// <summary>
        private string _Modt_SystemNo = "update t_SystemNo set DateType=@DateType,CodeLen=@CodeLen,Mark=@Mark,Usy=@Usy,UpdateUserId=@UpdateUserId,UpdateTime=@UpdateTime,UpdateDeptId=@UpdateDeptId where AutoID=@AutoID ";
        public string Modt_SystemNo { get { return _Modt_SystemNo; } }
        /// <summary>
        /// 刪除
        /// <summary>
        private string _Delt_SystemNo = "delete from t_SystemNo where AutoID=@AutoID";
        public string Delt_SystemNo { get { return _Delt_SystemNo; } }
        /// <summary>
        /// 判斷是否存在
        /// <summary>
        private string _IsExitt_SystemNo = "select count(0) from t_SystemNo where CompanyId=@CompanyId and ModuleType=@ModuleType and ModularType=@ModularType and Category=@Category and ReceiptType=@ReceiptType and keyword=@keyword ";
        public string IsExitt_SystemNo { get { return _IsExitt_SystemNo; } }

        private string _IsRepeatRule = "select count(0) from t_SystemNo where AutoId<>@AutoId and ModuleType=@ModuleType and ModularType=@ModularType and Category=@Category and ReceiptType=@ReceiptType and keyword=@keyword and CodeLen=@CodeLen and DateType=@DateType and CompanyId=@CompanyId ";
        public string IsRepeatRule { get { return _IsRepeatRule; } }
        /// <summary>
        /// 查詢
        /// <summary>
        private string _GetListt_SystemNo = "selectAutoID,CompanyId,ModuleType,ModularType,Category,ReceiptType,keyword,DateType,CodeLen,Mark,Usy,CreateUserId,CreateTime,CreateDeptId,UpdateUserId,UpdateTime,UpdateDeptId FROM t_SystemNo  where 1=1 ";
        public string GetListt_SystemNo { get { return _GetListt_SystemNo; } }
        /// <summary>
        /// 取得所有
        /// <summary>
        private string _GetAllt_SystemNo = "p_PageView";
        public string GetAllt_SystemNo { get { return _GetAllt_SystemNo; } }
        /// <summary>
        /// 根據單據別和關鍵字，獲取對應的規則
        /// </summary>
        private string _generatSystemNo = "select * from t_SystemNo where keyword=@keyword and CompanyId=@CompanyId ";
        public string GeneratSystemNo
        {
            get { return _generatSystemNo; }
        }

        private string _GlobelNo = "select * from t_SystemNo where ReceiptType=@ReceiptType and keyword=@keyword";
        public string GlobelNo
        {
            get { return _GlobelNo; }
        }
        private string _stepAddCode = " update t_SystemNo set CurrCode=CurrCode+1,GeneratTime=GETDATE() where AutoID=@AutoID ";
        public string StepAddCode
        {
            get{return _stepAddCode;}
        }
        /// <summary>
        /// 更新流水碼
        /// </summary>
        private string _updateCode = " update t_SystemNo set CurrCode=@CurrCode,GeneratTime=GETDATE() where AutoID=@AutoID";
        public string UpdateCode
        {
            get { return _updateCode; }
        }

        /// <summary>
        /// 獲取程式對應的單據別列表
        /// </summary>
        private string _getReceiptList = " SELECT ReceiptType FROM t_SystemNo WHERE keyword=@keyword";
        public string GetReceiptList
        {
            get { return _getReceiptList; }
        }

        private string _GetCategoryNo = "select * from t_SystemNo where CompanyId=@CompanyId and Category=@Category and keyword=@keyword";
        /// <summary>
        /// 根据公司、类别、关键字生成单据
        /// </summary>
        public string GetCategoryNo
        {
            get { return _GetCategoryNo; }
        }
    }
}
