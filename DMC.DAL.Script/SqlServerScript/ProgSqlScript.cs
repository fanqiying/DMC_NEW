using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DMC.DAL.Script;

namespace DMC.DAL.Script.SqlServer
{
    public class ProgSqlScript : IProgram
    {
        /// <summary>
        /// 增加新的程式
        /// </summary>
        private string _addProgram = "insert into t_Program(programID,programName,menuUrl" +
                 ",functionStr,usy,createrID,cDeptID,createTime,menuId,orderid,IsMobile,MobileUrl)values(@programID,@programName,@menuUrl" +
                 ",@functionStr,@usy,@createrID,@cDeptID,@createTime,@menuId,@orderid,@IsMobile,@MobileUrl)";
        /// <summary>
        /// 删除程式资料
        /// </summary>
        private string _delProgram = "delete from t_Program where programID in(@idList)";
        /// <summary>
        /// 修改程式资料
        /// </summary>
        private string _modProgram = "update t_Program set programName=@programName,menuUrl=@menuUrl,IsMobile=@IsMobile,MobileUrl=@MobileUrl," +
                "usy=@usy,updaterID=@updaterID ,functionStr=@functionStr," +
                "uDeptID=@uDeptID ,lastModTime=@modTime,menuId=@menuId,orderid=@orderid where programID=@programID";
        /// <summary>
        /// 判断程式是否存在
        /// </summary>
        private string _isExitProg = "select programID from t_Program where programID=@programID";


        /// <summary>
        /// 设置程式的基本功能
        /// </summary>
        private string _setProgFunc = "insert into t_ProgFunc(programID,functionID)values(@programID,@functionID)";

        /// <summary>
        /// 更新程式与菜单的关联
        /// </summary>
        private string _modProgMenu = "update  t_ProgRefMenu set menuID=@menuID WHERE programID=@programID";

        /// <summary>
        /// 删除程式的基本功能
        /// </summary>
        private string _delProgFunc = "delete from t_ProgFunc where programID =@programID ";
        /// <summary>
        /// /// <summary>
        /// 是否存在程式与菜单的关联
        /// </summary>
        /// </summary>
        private string _isExitProgVsMenu = "select count(*) from t_ProgRefMenu  where programID=@programID ";
        /// <summary>
        ///  新增程式与菜单的关联
        /// </summary>
        private string _addProgMenu = "insert into t_ProgRefMenu (programID,menuID,usy) values (@programID,@menuID,@usy)";
        private string _isExitProgFunc = "select COUNT (0) from t_ProgFunc  where  programID=@programID";

        /// <summary>
        /// 取得格式化的程式列表数据
        /// </summary>
        private string _programdt = "select * from  t_Program  vf left join" +
"(SELECT distinct b.programID," +
 "Course=STUFF((SELECT ',['+[functionName]+']' " +
                " FROM (select f.programID,f.functionID," +
       "(case isnull(v.displayValue ,'') when '' then r.DefaultValue else v.DisplayValue end) as functionName" +
  "from t_ProgFunc f left join " +
      " t_Language_Resources R on f.functionID=r.ResourceId left join " +
      "+ t_Language_Value V on r.ResourceId=v.ResourceId and v.LanguageId=@LanguageId) as a" +
                "WHERE a.programID=b.programID FOR XML PATH('')), 1, 1, '') FROM t_ProgFunc b) tf" +
                "on vf.programID=tf.programID";
        private string _programFuncList = "select R.ResourceId as ActionId," +
       "(case ISNULL(L.DisplayValue,'') WHEN '' THEN R.DefaultValue ELSE L.DisplayValue END) ActionName," +
      " ISNULL(F.programID,@ProgramId) AS ProgramId," +
      " (CASE ISNULL(f.functionID,'') WHEN '' THEN 'N' ELSE 'Y' END) IsUsy " +
  "from t_Language_Resources R left join " +
      " t_Language_Value L on r.ResourceId = l.ResourceId and l.LanguageId=@LanguageId left join  " +
       "t_ProgFunc F ON R.ResourceId=F.functionID AND F.programID=@ProgramId  " +
"where R.ResourceType='03'";
        string IProgram.AddProgram
        {
            get { return _addProgram; }
        }

        string IProgram.DelProgram
        {
            get { return _delProgram; }
        }

        string IProgram.ModProgram
        {
            get { return _modProgram; }
        }

        string IProgram.IsExitProg
        {
            get { return _isExitProg; }
        }

        string IProgram.SetProgFunc
        {
            get { return _setProgFunc; }
        }



        string IProgram.DelProgFunc
        {
            get { return _delProgFunc; }
        }


        string IProgram.IsExitProgVsMenu
        {
            get { return _isExitProgVsMenu; }
        }


        string IProgram.ModProgMenu
        {
            get { return _modProgMenu; }
        }

        string IProgram.AddProgMenu
        {
            get { return _addProgMenu; }
        }


        string IProgram.IsExitProgFunc
        {
            get { return _isExitProgFunc; }
        }


        string IProgram.Programdt
        {
            get { return _programdt; }
        }


        string IProgram.ProgramFuncList
        {
            get { return _programFuncList; }
        }

        private string _programIdList = "select programID as displayid , programID as displaytext from t_Program";
        public string ProgramIdList
        {
            get { return _programIdList; }
        }
    }
}
