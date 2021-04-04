
namespace DMC.DAL.Script.SqlServer
{
    class CompSqlScript :IComp
    {
        /// <summary>
        /// 增加新的公司信息的腳本方法
        /// </summary>
        private string _addComp = "insert into t_Company(companyID,compLanguage,compCategory,interName,outerName,"+
                 "simpleName,companyNo,addrOne,addrTwo,compTel,compFax,compRegNo,"+
                 "remark,usy,createrID,cDeptID,createTime)"+
                 "values(@companyID,@compLanguage ,@compCategory ,@interName ,@outerName ,"+
	             "@simpleName ,@companyNo ,@addrOne ,@addrTwo ,@compTel ,@compFax ,"+
	             "@compRegNo ,@remark ,@usy ,@createrID ,@cDeptID,@createTime)";
        /// <summary>
        /// 修改公司資料的數據腳本
        /// </summary>
        private string _modComp=" update t_Company set compCategory=@compCategory ,"+
				"interName=@interName,outerName=@outerName ,simpleName=@simpleName,companyNo=@companyNo,addrOne=@addrOne,"+
				"addrTwo=@addrTwo,compTel=@compTel ,compFax=@compFax,compRegNo=@compRegNo,remark=@remark,"+
                "usy=@usy,updaterID=@updaterID ,uDeptID=@uDeptID ,lastModTime=@modTime where companyID=@companyID and compLanguage=@compLanguage";
        /// <summary>
        /// 刪除公司資料信息
        /// </summary>
		private string _delComp = " delete from t_Company  where autoID=@idstr";
        /// <summary>
        /// 驗證該筆公司資料是否存在
        /// </summary>
        private string _isExitComp = "select count(0) from t_Company where companyID =@companyID and compLanguage=@compLanguage";
        
        /// <summary>
        /// 取得所有的公司别的简称
        /// </summary>
        private string _getComanyList = "select simpleName as DisplayText,companyID as DisplayID from t_Company where usy='Y' and compLanguage=@LanguageId";

        string IComp.AddComp
        {
            get { return _addComp; }
        }

        string IComp.ModComp
        {
            get { return _modComp; }
        }

        string IComp.DelComp
        {
            get { return _delComp; }
        }

        string IComp.IsExitComp
        {
            get { return _isExitComp; }
        } 
        /// <summary>
        /// 取公司别集合
        /// </summary>
        string IComp.GetCompanyList
        {
            get { return _getComanyList; }
        }

        private string _IGetCompany = "select * from t_Company where usy='Y' and companyid='{0}'";
        public string IGetCompany
        {
            get { return _IGetCompany; }
        }

        /// <summary>
        /// 根據公司ID獲取公司詳細信息 2015/6/18新增
        /// </summary>
        private string _GetCompanyInfoByID = "SELECT *  from  t_Company  WHERE companyID=@companyID AND compLanguage=@compLanguage";
        public string GetCompanyInfoByID
        {
            get { return _GetCompanyInfoByID; }
        }
    }
}
