
namespace DMC.DAL.Script.SqlServer
{
    public class ActionRightSqlScript:IActionRight
    {
        private string _addAllProgramActionByUser = @" INSERT INTO t_Right_UPAction(UserId,ProgramId,ActionId) "+
                                                     " SELECT @UserId,ProgramId,functionID "+
                                                       " FROM t_ProgFunc "+
                                                      " WHERE ProgramId=@ProgramId;";
        /// <summary>
        /// 添加使用者程式的所有操作權限
        /// </summary>
        public string AddAllProgramActionByUser
        {
            get { return _addAllProgramActionByUser; }
        }

        private string _addAllProgramActionByRose = @" INSERT INTO t_Right_RPAction(RoseId,ProgramId,ActionId) "+
                                                     " SELECT @RoseId,ProgramId,functionID "+
                                                       " FROM t_ProgFunc "+
                                                      " WHERE ProgramId=@ProgramId; ";
        /// <summary>
        /// 添加權限類別程式的所有操作權限
        /// </summary>
        public string AddAllProgramActionByRose
        {
            get { return _addAllProgramActionByRose; }
        }

        private string _addProgramActionByUser = @" INSERT INTO t_Right_UPAction(UserId,ProgramId,ActionId) VALUES(@UserId,@ProgramId,@ActionId); ";
        /// <summary>
        /// 添加使用者程式的操作權限
        /// </summary>
        public string AddProgramActionByUser
        {
            get { return _addProgramActionByUser; }
        }

        private string _addProgramActionByRose = @" INSERT INTO t_Right_RPAction(RoseId,ProgramId,ActionId) VALUES(@RoseId,@ProgramId,@ActionId); ";
        /// <summary>
        /// 添加權限類別程式的操作權限
        /// </summary>
        public string AddProgramActionByRose
        {
            get { return _addProgramActionByRose; }
        }

        private string _deleteProgramActionByUser = @" DELETE  t_Right_UPAction WHERE UserId=@UserId AND ProgramId=@ProgramId;";
        /// <summary>
        /// 刪除使用者程式的所有操作權限
        /// </summary>
        public string DeleteProgramActionByUser
        {
            get { return _deleteProgramActionByUser; }
        }

        private string _deleteProgramActionByRose = @" DELETE  t_Right_RPAction WHERE RoseId=@RoseId AND ProgramId=@ProgramId;";
        /// <summary>
        /// 刪除權限類別程式的所有操作
        /// </summary>
        public string DeleteProgramActionByRose
        {
            get { return _deleteProgramActionByRose; }
        }

        private string _readProgramActionByUser = @" SELECT A.ProgramID,
		   (SELECT (CASE ISNULL(V1.DisplayValue,'') WHEN '' THEN R1.DefaultValue ELSE V1.DisplayValue END) DisplayValue 
		      FROM t_Language_Resources R1 LEFT JOIN  
		           t_Language_Value V1 ON R1.ResourceId=V1.ResourceId AND LanguageId=@LanguageId 
		     WHERE R1.ResourceId=A.ProgramID ) AS ProgramName,
		   A.functionID AS ActionId,
		   (SELECT (CASE ISNULL(V2.DisplayValue,'') WHEN '' THEN R2.DefaultValue ELSE V2.DisplayValue END) DisplayValue 
		      FROM t_Language_Resources R2 LEFT JOIN  
		           t_Language_Value V2 ON R2.ResourceId=V2.ResourceId AND LanguageId=@LanguageId 
		     WHERE R2.ResourceId=A.functionID ) AS ActionName,
		   (CASE ISNULL(B.ActionId,'') WHEN '' THEN 'N' ELSE 'Y' END) Usy
	  FROM t_ProgFunc A 
 LEFT JOIN t_Right_UPAction B ON A.ProgramID=B.ProgramID AND A.functionID=B.ActionId AND B.UserId=@UserId  
	 WHERE A.ProgramID=@ProgramId;";
        /// <summary>
        /// 獲取使用者程式對應的操作權限
        /// </summary>
        public string ReadProgramActionByUser
        {
            get { return _readProgramActionByUser; }
        }

        private string _readProgramActionByRose = @"	SELECT A.ProgramID,
		   (SELECT (CASE ISNULL(V1.DisplayValue,'') WHEN '' THEN R1.DefaultValue ELSE V1.DisplayValue END) DisplayValue 
		      FROM t_Language_Resources R1 LEFT JOIN  
		           t_Language_Value V1 ON R1.ResourceId=V1.ResourceId AND LanguageId=@LanguageId 
		     WHERE R1.ResourceId=A.ProgramID ) AS ProgramName,
		   A.functionID AS ActionId,
		   (SELECT (CASE ISNULL(V2.DisplayValue,'') WHEN '' THEN R2.DefaultValue ELSE V2.DisplayValue END) DisplayValue 
		      FROM t_Language_Resources R2 LEFT JOIN  
		           t_Language_Value V2 ON R2.ResourceId=V2.ResourceId AND LanguageId=@LanguageId 
		     WHERE R2.ResourceId=A.functionID ) AS ActionName,
		   (CASE ISNULL(B.ActionId,'') WHEN '' THEN 'N' ELSE 'Y' END) Usy
	  FROM t_ProgFunc A 
 LEFT JOIN t_Right_RPAction B ON A.ProgramID=B.ProgramID AND A.functionID=B.ActionId AND B.RoseId=@RoseId  
	 WHERE A.ProgramID=@ProgramId;";
        /// <summary>
        /// 獲取權限類別程式對應的操作權限
        /// </summary>
        public string ReadProgramActionByRose
        {
            get { return _readProgramActionByRose; }
        }
    }
}
