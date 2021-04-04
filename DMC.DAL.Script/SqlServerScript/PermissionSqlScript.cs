using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMC.DAL.Script.SqlServer
{
    public class PermissionSqlScript : IPermission
    {
        private string _getMainMenu = " SELECT M.menuID,M.fatherID,'' Url,(CASE ISNULL(L.DisplayValue,'') WHEN '' THEN R.DefaultValue ELSE L.DisplayValue END) AS DisplayValue " +
                                      " FROM t_Menu M LEFT JOIN " +
                                      " t_Language_Resources R on M.menuID=R.ResourceId LEFT JOIN " +
                                      " t_Language_Value L ON R.ResourceId=L.ResourceId AND L.LanguageId=@LangId " +
                                      " WHERE M.usy='Y' ";
        public string GetMainMenu
        {
            get { return _getMainMenu; }
        }

        private string _getMyCompany = "SELECT * FROM t_Right_UCompany A LEFT JOIN t_Company B ON A.CompId=B.companyID WHERE A.UserId=@UserId AND B.compLanguage=@LangId;";
        public string GetMyCompany
        {
            get { return _getMyCompany; }
        }

        private string _getMenuStatusByUser = "WITH CTEGetParent AS( " +
                                              " SELECT A.*  " +
            " FROM t_Menu A " +
     " WHERE A.menuID IN " +
           " ( SELECT DISTINCT M.menuID  " +
  " FROM t_Program P LEFT JOIN  " +
       " t_ProgRefMenu M ON P.programID=M.programID LEFT JOIN  " +
       " t_Right_UProgram UP ON P.programID=UP.ProgramId AND UP.UserId=@TypeId " +
 " WHERE UPPER(P.usy)='Y') " +
     " UNION ALL " +
    " SELECT A.*  " +
      " FROM t_Menu AS A inner join   " +
           " CTEGetParent AS B ON A.menuID=B.fatherID " +
" ) " +
" SELECT M.menuID ID,M.fatherID ParentId,'' Url, " +
       " (CASE ISNULL(L.DisplayValue,'') WHEN '' THEN R.DefaultValue ELSE L.DisplayValue END) DisplayText, " +
       " 'N' IsUse,'N' IsProgram " +
  " FROM t_Menu M LEFT JOIN  " +
       " t_Language_Resources R ON M.menuID=R.ResourceId LEFT JOIN " +
       " t_Language_Value L ON R.ResourceId=L.ResourceId  AND L.LanguageId=@LangId  " +
 " WHERE M.menuID IN(SELECT DISTINCT menuID FROM CTEGetParent)  " +
       " AND UPPER(M.usy)='Y' " +
" UNION all " +
" SELECT P.programID,M.menuID,P.menuUrl, " +
       " (CASE ISNULL(L.DisplayValue,'') WHEN '' THEN R.DefaultValue ELSE L.DisplayValue END) DisplayText, " +
       " (CASE ISNULL(UP.ProgramId,'') WHEN '' THEN 'N' ELSE 'Y' END) IsUse, " +
       " 'Y' IsProgram " +
  " FROM t_Program P LEFT JOIN  " +
       " t_ProgRefMenu M ON P.programID=M.programID LEFT JOIN  " +
       " t_Language_Resources R ON P.programID=R.ResourceId LEFT JOIN " +
       " t_Language_Value L ON R.ResourceId=L.ResourceId AND L.LanguageId=@LangId LEFT JOIN  " +
       " t_Right_UProgram UP ON P.programID=UP.ProgramId AND UP.UserId=@TypeId " +
 " WHERE UPPER(P.usy)='Y'; ";
        public string GetMenuStatusByUser
        {
            get { return _getMenuStatusByUser; }
        }

        private string _getMenuStatusByRose = " WITH CTEGetParent AS(   " +
    " SELECT A.*  " +
      " FROM t_Menu A " +
     " WHERE A.menuID IN " +
           " ( SELECT DISTINCT M.menuID  " +
  " FROM t_Program P LEFT JOIN  " +
       " t_ProgRefMenu M ON P.programID=M.programID LEFT JOIN  " +
       " t_Right_RProgram RP ON P.programID=RP.ProgramId AND RP.RoseId=@TypeId " +
 " WHERE UPPER(P.usy)='Y') " +
     " UNION ALL " +
    " SELECT A.*  " +
" FROM t_Menu AS A inner join   " +
           " CTEGetParent AS B ON A.menuID=B.fatherID ) " +
" SELECT M.menuID ID,M.fatherID ParentId,'' Url, " +
       " (CASE ISNULL(L.DisplayValue,'') WHEN '' THEN R.DefaultValue ELSE L.DisplayValue END) DisplayText, " +
       " 'N' IsUse,'N' IsProgram " +
  " FROM t_Menu M LEFT JOIN  " +
       " t_Language_Resources R ON M.menuID=R.ResourceId LEFT JOIN " +
       " t_Language_Value L ON R.ResourceId=L.ResourceId  AND L.LanguageId=@LangId  " +
 " WHERE M.menuID IN(SELECT DISTINCT menuID FROM CTEGetParent)  " +
       " AND UPPER(M.usy)='Y' " +
" UNION all " +
" SELECT P.programID,M.menuID,P.menuUrl, " +
   " (CASE ISNULL(L.DisplayValue,'') WHEN '' THEN R.DefaultValue ELSE L.DisplayValue END) DisplayText, " +
   " (CASE ISNULL(RP.ProgramId,'') WHEN '' THEN 'N' ELSE 'Y' END) IsUse, " +
   " 'Y' IsProgram " +
" FROM t_Program P LEFT JOIN  " +
   " t_ProgRefMenu M ON P.programID=M.programID LEFT JOIN  " +
   " t_Language_Resources R ON P.programID=R.ResourceId LEFT JOIN " +
   " t_Language_Value L ON R.ResourceId=L.ResourceId  AND L.LanguageId=@LangId LEFT JOIN  " +
   " t_Right_RProgram RP ON P.programID=RP.ProgramId AND RP.RoseId=@TypeId " +
" WHERE UPPER(P.usy)='Y';";
        public string GetMenuStatusByRose
        {
            get { return _getMenuStatusByRose; }
        }

        private string _getMyMenu = " WITH CTEGetParent AS (" +
    " SELECT A.*  " +
" 	  FROM t_Menu A " +
     " WHERE A.menuID IN " +
           " (SELECT DISTINCT M.menuID  " +
  " FROM t_Program P LEFT JOIN  " +
       " t_ProgRefMenu M ON P.programID=M.programID " +
 " WHERE P.programID IN " +
       " (SELECT B.ProgramId   " +
          " FROM t_Right_URole A LEFT JOIN  " +
               " t_Right_RProgram B ON A.RoseId=B.RoseId  " +
         " WHERE A.UserId=@UserId " +
        " UNION " +
        " SELECT A.ProgramId  " +
" FROM t_Right_UProgram A  " +
         " WHERE A.UserId=@UserId) AND " +
       " UPPER(P.usy)='Y') " +
     " UNION ALL " +
    " SELECT A.*  " +
" FROM t_Menu AS A inner join   " +
           " CTEGetParent AS B ON A.menuID=B.fatherID) " +
" SELECT M.menuID ID,M.fatherID ParentId,'' Url, " +
       " (CASE ISNULL(L.DisplayValue,'') WHEN '' THEN R.DefaultValue ELSE L.DisplayValue END) DisplayText, " +
       " 'N' IsProgram,M.pathImg ,M.orderid,'' IsMobile,'' MobileUrl" +
  " FROM t_Menu M LEFT JOIN  " +
       " t_Language_Resources R ON M.menuID=R.ResourceId LEFT JOIN " +
       " t_Language_Value L ON R.ResourceId=L.ResourceId AND L.LanguageId=@LangId " +
 " WHERE M.menuID IN(SELECT DISTINCT menuID FROM CTEGetParent) AND " +
       " UPPER(M.usy)='Y' " +
" UNION ALL " +
" SELECT P.programID,M.menuID,P.menuUrl, " +
       " (CASE ISNULL(L.DisplayValue,'') WHEN '' THEN R.DefaultValue ELSE L.DisplayValue END) DisplayText, " +
       " 'Y' IsProgram ,P.pathImg ,P.orderid,P.IsMobile,P.MobileUrl " +
  " FROM t_Program P LEFT JOIN  " +
       " t_ProgRefMenu M ON P.programID=M.programID LEFT JOIN  " +
       " t_Language_Resources R ON P.programID =R.ResourceId LEFT JOIN " +
       " t_Language_Value L ON R.ResourceId=L.ResourceId AND L.LanguageId=@LangId " +
 " WHERE P.programID IN " +
       " (SELECT B.ProgramId   " +
          " FROM t_Right_URole A LEFT JOIN  " +
               " t_Right_RProgram B ON A.RoseId=B.RoseId  " +
         " WHERE A.UserId=@UserId " +
        " UNION " +
        " SELECT A.ProgramId  " +
" FROM t_Right_UProgram A  " +
         " WHERE A.UserId=@UserId) AND " +
       " UPPER(P.usy)='Y';";
        public string GetMyMenu
        {
            get { return _getMyMenu; }
        }

        private string _getMyPgmOpt = " SELECT A.ProgramID,'1' as IsUserOrRose, " +
       " (SELECT CASE ISNULL(V1.DisplayValue,'') WHEN '' THEN R1.DefaultValue ELSE V1.DisplayValue END FROM t_Language_Resources R1 LEFT JOIN t_Language_Value V1 ON R1.ResourceId=V1.ResourceId AND V1.LanguageId=@LangId WHERE R1.ResourceId=@ProgramId )  AS ProgramName, " +
       " A.functionID AS ActionId, " +
       " (SELECT CASE ISNULL(V2.DisplayValue,'') WHEN '' THEN R2.DefaultValue ELSE V2.DisplayValue END FROM t_Language_Resources R2 LEFT JOIN t_Language_Value V2 ON R2.ResourceId=V2.ResourceId AND V2.LanguageId=@LangId WHERE R2.ResourceId=a.functionID )  AS ActionName, " +
       " (CASE ISNULL(B.ActionId,'') WHEN '' THEN 'N' ELSE 'Y' END) Usy " +
  " FROM t_ProgFunc A  " +
" LEFT JOIN t_Right_UPAction B ON A.ProgramID=B.ProgramID AND A.functionID=B.ActionId AND B.UserId=@UserId  " +
 " WHERE A.ProgramID=@ProgramId " +
     " union all " +
 " SELECT A.ProgramID,'2' as IsUserOrRose, " +
       " (SELECT CASE ISNULL(V1.DisplayValue,'') WHEN '' THEN R1.DefaultValue ELSE V1.DisplayValue END FROM t_Language_Resources R1 LEFT JOIN t_Language_Value V1 ON R1.ResourceId=V1.ResourceId AND V1.LanguageId=@LangId WHERE R1.ResourceId=@ProgramId )  AS ProgramName, " +
       " A.functionID AS ActionId, " +
       " (SELECT CASE ISNULL(V2.DisplayValue,'') WHEN '' THEN R2.DefaultValue ELSE V2.DisplayValue END FROM t_Language_Resources R2 LEFT JOIN t_Language_Value V2 ON R2.ResourceId=V2.ResourceId AND V2.LanguageId=@LangId WHERE R2.ResourceId=a.functionID )  AS ActionName, " +
       " (CASE ISNULL(B.ActionId,'') WHEN '' THEN 'N' ELSE 'Y' END) Usy " +
  " FROM t_ProgFunc A  " +
" LEFT JOIN t_Right_RPAction B ON A.ProgramID=B.ProgramID AND A.functionID=B.ActionId " +
 " WHERE A.ProgramID=@ProgramId AND  " +
       " B.RoseId IN(SELECT RoseId FROM t_Right_URole WHERE UserId=@UserId);";
        public string GetMyPgmOpt
        {
            get { return _getMyPgmOpt; }
        }
    }
}
