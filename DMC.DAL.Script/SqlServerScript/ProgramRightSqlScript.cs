using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMC.DAL.Script.SqlServer
{
    public class ProgramRightSqlScript : IProgramRight
    {
        private string _addProgramRightByRose = @" INSERT INTO t_Right_RProgram(RoseId,ProgramId) VALUES(@RoseId,@ProgramId); ";
        /// <summary>
        /// 權限類別添加程式權限
        /// </summary>
        public string AddProgramRightByRose
        {
            get { return _addProgramRightByRose; }
        }

        private string _addProgramRightByUser = @" INSERT INTO t_Right_UProgram(UserId,ProgramId) VALUES(@UserId,@ProgramId); ";
        /// <summary>
        /// 使用者添加程式權限
        /// </summary>
        public string AddProgramRightByUser
        {
            get { return _addProgramRightByUser; }
        }

        private string _deleteProgramRightByRose = @"  DELETE t_Right_RPAction WHERE RoseId=@RoseId AND ProgramId=@ProgramId; " +
                                                    " DELETE t_Right_RProgram WHERE RoseId=@RoseId AND ProgramId=@ProgramId; ";
        /// <summary>
        /// 刪除權限類別對應的程式權限
        /// </summary>
        public string DeleteProgramRightByRose
        {
            get { return _deleteProgramRightByRose; }
        }

        private string _deleteProgramRightByUser = @" DELETE t_Right_UPAction WHERE UserId=@UserId AND ProgramId=@ProgramId; " +
                                                    " DELETE t_Right_UProgram WHERE UserId=@UserId AND ProgramId=@ProgramId; ";
        /// <summary>
        /// 刪除使用者權限對應的程式權限
        /// </summary>
        public string DeleteProgramRightByUser
        {
            get { return _deleteProgramRightByUser; }
        }

        private string _searchProgramRightByRose = "SELECT * " +
      "FROM (  SELECT DISTINCT A.RoseId,A.ProgramId, (CASE ISNULL(C.DisplayValue,'') WHEN '' THEN R.DefaultValue ELSE C.DisplayValue END) DisplayValue " +
                " FROM t_Right_RProgram A LEFT JOIN " +
                     " t_Right_RPAction B ON A.ProgramId=B.ProgramId LEFT JOIN " +
                     " t_Language_Resources R ON A.ProgramId=R.ResourceId LEFT JOIN " +
                     " t_Language_Value C ON R.ResourceId=C.ResourceId AND C.[LanguageId]=@LangId " +
              " WHERE A.RoseId=@RoseId and " +
                    " (@KeyWord='' or " +
                    " A.ProgramId like '%'+@KeyWord+'%' or " +
                    " C.DisplayValue like '%'+@KeyWord+'%' ) " +
          " )A " +
" OUTER APPLY( " +
    " SELECT [ActionIds]= " +
     " replace(" +
    "	replace(" +
            " replace((SELECT ActionId AS value " +
                    "   FROM t_Right_RPAction N " +
                "	  WHERE RoseId = A.RoseId AND ProgramId=a.ProgramId " +
                 "  ORDER BY N.ActionId ASC " +
                 "  FOR XML AUTO),'\"/><N value=\"',',') " +
                ",'<N value=\"','') " +
             ",'\"/>','') " +
           ")N " +
" OUTER APPLY( " +
    " SELECT [ActionIdNames]= " +
    " replace(" +
        " replace(" +
        " replace((SELECT (CASE ISNULL(D.DisplayValue,'') WHEN '' THEN R1.DefaultValue ELSE D.DisplayValue END) as value " +
                "	   FROM t_Right_RPAction M LEFT JOIN " +
                    "        t_Language_Resources R1 On M.ActionId=R1.ResourceId LEFT JOIN " +
                        "    t_Language_Value D  ON R1.ResourceId=D.ResourceId AND D.[LanguageId]='0001' " +
                     " WHERE M.RoseId = A.RoseId AND ProgramId=a.ProgramId " +
                 "  ORDER BY M.ActionId ASC FOR XML AUTO),'\"/><M value=\"',',') " +
                ",'<M value=\"','') " +
            ",'\"/>','') " +
            ")M ";
        /// <summary>
        /// 權限類別的程式權限搜索
        /// </summary>
        public string SearchProgramRightByRose
        {
            get { return _searchProgramRightByRose; }
        }

        private string _searchProgramRightByUser = "SELECT *  " +
	  " FROM (  SELECT DISTINCT A.UserId,A.ProgramId, (CASE ISNULL(C.DisplayValue,'') WHEN '' THEN R.DefaultValue ELSE C.DisplayValue END) DisplayValue  " +
				" FROM t_Right_UProgram A LEFT JOIN   " +
					"  t_Right_RPAction B ON A.ProgramId=B.ProgramId LEFT JOIN  " +
					 " t_Language_Resources R ON A.ProgramId=R.ResourceId LEFT JOIN  " + 
					 " t_Language_Value C ON R.ResourceId=C.ResourceId AND C.[LanguageId]=@LangId  " +
	           " WHERE A.UserId=@UserId and  " +
					" (@KeyWord='' or   " +
					"  A.ProgramId like '%'+@KeyWord+'%' or   " +
					 " C.DisplayValue like '%'+@KeyWord+'%' )  " +
	       " )A   " +
" OUTER APPLY(   " +
	 " SELECT [ActionIds]=   " +
	 " replace(  " +
		" replace(  " +
			" replace((SELECT ActionId AS value   " +
					  "  FROM t_Right_UPAction N   " +
					 "  WHERE UserId=A.UserId AND ProgramId=A.ProgramId  " +
				  "  ORDER BY N.ActionId ASC   " +
				   " FOR XML AUTO),'\"/><N value=\"',',')   " +
	            " ,'<N value=\"','')  " +
			"  ,'\"/>','')   " +
	      "  )N   " +
" OUTER APPLY(   " +
	 " SELECT [ActionIdNames]=   " +
	 " replace(  " +
		" replace(  " +
			" replace((SELECT (CASE ISNULL(D.DisplayValue,'') WHEN '' THEN R1.DefaultValue ELSE D.DisplayValue END) as value   " +
					  "  FROM t_Right_UPAction M LEFT JOIN   " +
					       "  t_Language_Resources R1 On M.ActionId=R1.ResourceId LEFT JOIN  " +
					       "  t_Language_Value D  ON R1.ResourceId=D.ResourceId AND D.[LanguageId]=@LangId  " +
					 "  WHERE UserId=A.UserId  AND ProgramId=a.ProgramId  " +
				   " ORDER BY M.ActionId ASC FOR XML AUTO),'\"/><M value=\"',',')   " +
				" ,'<M value=\"','')  " +
			" ,'\"/>','')   " +
			" )M "; 
        /// <summary>
        /// 使用者的程式權限搜索
        /// </summary>
        public string SearchProgramRightByUser
        {
            get { return _searchProgramRightByUser; }
        }

        private string _existsProgramId = @" SELECT COUNT(ProgramID) FROM t_Program WHERE ProgramID=@ProgramId; ";
        /// <summary>
        /// 检查程式是否存在
        /// </summary>
        public string ExistsProgramId
        {
            get { return _existsProgramId; }
        }

        private string _deleteAllProgramRightByRose = @"  DELETE t_Right_RPAction WHERE RoseId=@RoseId; " +
                                                    " DELETE t_Right_RProgram WHERE RoseId=@RoseId; ";
        public string DeleteAllProgramRightByRose
        {
            get { return _deleteAllProgramRightByRose; }
        }
        private string _deleteAllProgramRightByUser = @" DELETE t_Right_UPAction WHERE UserId=@UserId ; " +
                                                    " DELETE t_Right_UProgram WHERE UserId=@UserId ; ";
        public string DeleteAllProgramRightByUser
        {
            get { return _deleteAllProgramRightByUser; }
        }
    }
}
