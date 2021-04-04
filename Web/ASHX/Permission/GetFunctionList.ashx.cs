using System.Web;
using System.Data;
using Utility.HelpClass;
using System.Web.SessionState;
using DMC.BLL;

namespace Web.ASHX
{
    /// <summary>
    /// 取得程式的基本功能列表，从多语言配置中
    /// </summary>
    public class GetFunctionList : IHttpHandler, IRequiresSessionState
    {
        ProgramService ps = new ProgramService();
        UserManageService us = new UserManageService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //獲取語言別
            string languageID = us.GetUserMain().LanguageId.To_String();
            //獲取程式編號
            string programID = GetData.GetRequest("ProgramId").Trim();
            DataTable funtionDT = ps.ProgramFuncList(languageID, programID);
            if (funtionDT.Rows.Count > 0)
            {
                context.Response.Write(JsonHelper.ConvertDTToJson(funtionDT, funtionDT.Rows.Count));
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }
}