using System.Web;
using Utility.HelpClass;
using System.Web.SessionState;
using DMC.BLL;

namespace Web.ASHX
{
    /// <summary>
    /// 修改密碼操作
    /// </summary>
    public class ModPwd : IHttpHandler, IRequiresSessionState
    {
        UserManageService uc = new UserManageService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //接收傳參
            string oldpwd = GetData.GetRequest("oldpwd").Trim();
            string newpwd = GetData.GetRequest("pwd").To_String();
            string pwdAgain = GetData.GetRequest("pwdtwo").Trim();
            string userID = uc.GetUserMain().userID.To_String();

            if (oldpwd != "" && newpwd != "" && pwdAgain != "")
            {
                string eoldpwd = UserRightManageService.EnCryptPassword(oldpwd);
                //验证旧密码是否正确
                bool sataus = uc.IsOkOldPwd(userID, eoldpwd);
                if (sataus == true)
                {
                    //执行修改密码操作
                    string enewpwd = UserRightManageService.EnCryptPassword(newpwd);
                    if (uc.ModUserPwd(userID, enewpwd) == true)
                    {
                        context.Response.Write("OK");
                    }
                }
                else
                {
                    context.Response.Write("ERR");
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