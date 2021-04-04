using System.Web;
using System.Web.SessionState;
using Utility.HelpClass;
using DMC.BLL;

namespace Web.ASHX
{
    /// <summary>
    /// 根據部門ID獲取部門主管
    /// </summary>
    public class GetdeptHeader : IHttpHandler, IRequiresSessionState
    {
        DeptService dser = new DeptService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string deptID = GetData.GetRequest("deptID").Trim();
            if (deptID != "")
            {
                string result = dser.GetDeptHeaderByID(deptID);
                if (result != "")
                {
                    context.Response.Write(result);

                }

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