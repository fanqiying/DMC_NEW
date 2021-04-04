using System;
using System.IO;
using System.Data;
using Utility.HelpClass;
using DMC.BLL;
using System.Collections.Generic;
using System.Configuration;

namespace Web.UI
{
    public partial class DownFile : System.Web.UI.Page
    {
        /// <summary>
        /// 從配置文件中獲取系統別
        /// </summary>
        private string sys = System.Configuration.ConfigurationManager.AppSettings["system"];
        //從配置文件中獲取文件上傳路徑
        private string fileServer = "\\\\" + System.Configuration.ConfigurationManager.AppSettings["fileserver"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string filename = Request["filename"];
                string del = string.IsNullOrEmpty(Request["del"]) ? "Y" : Request["del"];
                if (!string.IsNullOrEmpty(filename))
                {
                    string filepath = System.Web.HttpContext.Current.Server.MapPath("../TempFile") + "\\";
                    if (File.Exists(filepath + filename))
                    {
                        FileInfo fileInfo = new FileInfo(filepath + filename);
                        Response.Clear();
                        Response.ClearContent();
                        Response.ClearHeaders();
                        Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                        Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                        Response.AddHeader("Content-Transfer-Encoding", "binary");
                        Response.ContentType = "application/octet-stream";
                        Response.ContentEncoding = System.Text.Encoding.UTF8;
                        Response.WriteFile(fileInfo.FullName);
                        Response.Flush();
                        Response.End();
                    }

                    if (del == "Y")
                    {
                        //刪除文件
                        File.Delete(filepath + filename);
                    }
                }
            }
        }


    }
}