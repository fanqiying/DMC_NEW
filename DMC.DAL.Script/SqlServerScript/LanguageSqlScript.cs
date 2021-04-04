using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMC.DAL.Script.SqlServer
{
    internal class LanguageSqlScript : ILanguage
    {
        /// <summary>
        /// 創建語句
        /// </summary>
        private string _createLanguage = @" INSERT INTO t_Language_Resources(ResourceId,ResourceType,DefaultValue,GroupKey,GroupValue,Usy,CreateDeptId,CreateTime,CreateUser) " +
                                          " VALUES(@ResourceId,@ResourceType,@DefaultValue,@GroupKey,@GroupValue,@Usy,@CreateDeptId,@CreateTime,@CreateUser) ;";
        /// <summary>
        /// 創建多語言對應的值
        /// </summary>
        private string _createLanguageValue = @" INSERT INTO t_Language_Value(LanguageId,ResourceId,DisplayValue)" +
                                               " VALUES(@LanguageId,@ResourceId,@DisplayValue) ";

        private string _clearLanguageValue = " DELETE t_Language_Value WHERE ResourceId=@ResourceId;";
        /// <summary>
        /// 驗證Key是否已存在
        /// </summary>
        private string _existKey = " SELECT Count(0) FROM t_Language_Resources WHERE ResourceId=@ResourceId ; ";

        private string _existsDefaultValue = " SELECT Count(0) FROM t_Language_Resources WHERE DefaultValue=@DefaultValue and ResourceId<>@ResourceId; ";
        /// <summary>
        /// 修改語句
        /// </summary>
        private string _modifyLanguage = @" UPDATE t_Language_Resources " +
                                              "SET ResourceType=@ResourceType," +
                                                 " DefaultValue=@DefaultValue," +
                                                 " GroupKey=@GroupKey," +
                                                 " GroupValue=@GroupValue," +
                                                 " Usy=@Usy," +
                                                 " UpdateDeptId=@UpdateDeptId," +
                                                 " UpdateTime=@UpdateTime," +
                                                 " UpdateUser=@UpdateUser " +
                                           " WHERE ResourceId=@ResourceId ;";
        /// <summary>
        /// 通過key刪除
        /// </summary>
        private string _deleteLanguageByKey = @" DELETE t_Language_Resources WHERE ResourceId=@ResourceId ; DELETE t_Language_Value WHERE ResourceId=@ResourceId ;";
        /// <summary>
        /// 通過AutoID刪除
        /// </summary>
        private string _deleteLanguageByAutoId = @" DELETE t_Language_Resources WHERE AutoID=@AutoID; ";
        /// <summary>
        /// 添加語言類別
        /// </summary>
        private string _addLanguageType = @" INSERT INTO t_Language_Type(LanguageId,LanguageName,Description,Usy,CreateDeptId,CreateTime,CreateUser) " +
                                           " VALUES(@LanguageId,@LanguageName,@Description,@Usy,@CreateDeptId,@CreateTime,@CreateUser); ";
        /// <summary>
        /// 刪除語言類別
        /// </summary>
        private string _deleteLanguageType = @" DELETE t_Language_Type WHERE LanguageId=@LanguageId;";
        /// <summary>
        /// 修改語言類別
        /// </summary>
        private string _updateLanguageType = @" UPDATE t_Language_Type " +
                                                 " SET LanguageName=@LanguageName," +
                                                     " Description=@Description," +
                                                     " Usy=@Usy," +
                                                     " UpdateDeptId=@UpdateDeptId," +
                                                     " UpdateTime=@UpdateTime," +
                                                     " UpdateUser=@UpdateUser " +
                                               " WHERE LanguageId=@LanguageId;";
        /// <summary>
        /// 獲取所有的語言類別
        /// </summary>
        private string _getAllLanguageType = " SELECT * FROM t_Language_Type order by sorter;";
        /// <summary>
        /// 判斷語言類別是否存在
        /// </summary>
        private string _existsLanguageType = " SELECT COUNT(LanguageId) FROM t_Language_Type WHERE LanguageId=@LanguageId;";

        private string _getResourceText = @" SELECT CASE A.Usy WHEN 'N' THEN @ResourceId ELSE  (CASE ISNULL(b.DisplayValue,'') WHEN '' THEN A.DefaultValue ELSE b.DisplayValue END) END DisplayText " +
                                             " FROM t_Language_Resources A LEFT JOIN " +
                                                  " t_Language_Value B ON A.ResourceId=B.ResourceId AND B.LanguageId=@LanguageId " +
                                            " WHERE A.ResourceId=@ResourceId; ";

        private string _getResourceSelect = " SELECT A.GroupValue AS DisplayID, (CASE A.Usy WHEN 'N' THEN A.GroupValue ELSE (CASE ISNULL(B.DisplayValue,'') WHEN '' THEN A.DefaultValue ELSE B.DisplayValue END) END) AS DisplayText " +
                                              " FROM t_Language_Resources A LEFT JOIN " +
                                                   " t_Language_Value B ON A.ResourceId=B.ResourceId AND B.LanguageId=@LanguageId " +
											 " WHERE A.Usy='Y' AND A.GroupKey=@GroupKey ORDER BY A.GroupValue ASC;";

        private string _getResourceValues = @" SELECT A.LanguageId,A.LanguageName,ISNULL(B.DisplayValue,'') DisplayValue " +
                                               " FROM t_Language_Type A LEFT JOIN " +
                                                    " t_Language_Value B ON A.LanguageId=B.LanguageId AND B.ResourceId=@ResourceId WHERE A.Usy='Y'; ";

        private string _getResourceByType = @" SELECT A.ResourceId LanguageKey,(CASE A.Usy WHEN 'N' THEN A.ResourceId ELSE (CASE ISNULL(B.DisplayValue,'') WHEN '' THEN A.DefaultValue ELSE B.DisplayValue END) END) LanguageValue " +
                                               " FROM t_Language_Resources A LEFT JOIN " +
                                                    " t_Language_Value B ON A.ResourceId=B.ResourceId AND B.LanguageId=@LanguageId;";
        /// <summary>
        /// 創建數據
        /// </summary>
        public string CreateLanguageResources
        {
            get
            {
                return _createLanguage;
            }
        }

        public string CreateLanguageValue
        {
            get { return _createLanguageValue; }
        }

        public string ClearLanguageValue { get { return _clearLanguageValue; } }
        /// <summary>
        /// 驗證Key是否已存在
        /// </summary>
        public string ExistKey
        {
            get
            {
                return _existKey;
            }
        }
        /// <summary>
        /// 修改數據
        /// </summary>
        public string ModifyLanguage
        {
            get { return _modifyLanguage; }
        }
        /// <summary>
        /// 通過Key刪除數據
        /// </summary>
        public string DeleteLanguageByKey
        {
            get { return _deleteLanguageByKey; }
        }
        /// <summary>
        /// 通過AutoID刪除數據
        /// </summary>
        public string DeleteLanguageByAutoId
        {
            get { return _deleteLanguageByAutoId; }
        }
        
        /// <summary>
        /// 添加語言類別
        /// </summary>
        public string AddLanguageType
        {
            get { return _addLanguageType; }
        }

        /// <summary>
        /// 刪除語言類別
        /// </summary>
        public string DeleteLanguageType
        {
            get { return _deleteLanguageType; }
        }
        /// <summary>
        /// 更新語言類別
        /// </summary>
        public string UpdateLanguageType
        {
            get { return _updateLanguageType; }
        }
        /// <summary>
        /// 獲取所有的語言類別
        /// </summary>
        public string GetAllLanguageType
        {
            get { return _getAllLanguageType; }
        }
        /// <summary>
        /// 判斷語言類別是否存在
        /// </summary>
        public string ExistsLanguageType
        {
            get { return _existsLanguageType; }
        }
        /// <summary>
        /// 獲取對應資源的文本內容
        /// </summary>
        public string GetResourceText
        {
            get { return _getResourceText; }
        }
        /// <summary>
        /// 獲取下拉框對應的選項
        /// </summary>
        public string GetResourceSelect
        {
            get { return _getResourceSelect; }
        }
        /// <summary>
        /// 獲取多語言的值
        /// </summary>
        public string GetResourceValues
        {
            get { return _getResourceValues; }
        }

        public string GetResourceByType
        {
            get { return _getResourceByType; }
        }

        /// <summary>
        /// 檢查默認值是否存在
        /// </summary>
        public string ExistsDefaultValue
        {
            get { return _existsDefaultValue; }
        }
    }
}
