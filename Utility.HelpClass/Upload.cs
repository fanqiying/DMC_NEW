using System;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing.Imaging;

namespace Utility.HelpClass
{
    /// <summary>
    /// 文件上传封装类
    /// 2013.6.7
    /// code by jeven_xiao
    /// </summary>
    public class Upload
    {
     

        #region 图片上传功能
        /// <summary>
        /// 图片上传功能
        /// </summary>
        /// <param name="filePic">文件控件ID</param>
        /// <param name="SavePath">保存路径</param>
        /// <param name="PreName">文件名前缀</param>
        /// <returns>返回图片保存路径</returns>
        public  string uploadimg( FileUpload filePic, string SavePath, string PreName, string NewFileName = "", bool DatePath = true)
        {
            int upFileMax = GetData.GetConfig("upImgMax").To_Int();
            string upFileType = GetData.GetConfig("upImgType");
            string upFiletypeName = GetData.GetConfig("upImgTypeName");

            string ImgUrl = "";
            if (filePic.HasFile)
            {
                ImgUrl = UploadFile(filePic, SavePath, PreName, upFileMax, upFileType, NewFileName, DatePath);
                if (ImgUrl == "")
                {
                    return "";
                }
                else
                {
                    return ImgUrl;
                }
            }
            return "";
        }
        #endregion


        #region 资料上传功能
        /// <summary>
        /// 资料上传功能
        /// </summary>
        /// <param name="filePic">文件控件ID</param>
        /// <param name="SavePath">保存路径</param>
        /// <param name="PreName">文件名前缀</param>
        /// <returns>返回图片保存路径</returns>
        public  string uploadAttach( FileUpload filePic, string SavePath, string PreName, string NewFileName = "", bool DatePath = true)
        {
           
            int upFileMax = GetData.GetConfig("upAttachMax").To_Int();
            string upFileType = GetData.GetConfig("upAttachType");
            string upFiletypeName = GetData.GetConfig("upAttachTypeName");

            string ImgUrl = "";
            if (filePic.HasFile)
            {
                ImgUrl = UploadDuc(filePic, SavePath, PreName, upFileMax, upFileType, NewFileName, DatePath);
                if (ImgUrl == "")
                {
                    return "";
                }
                else
                {
                    return ImgUrl;
                }
            }
            return "";
        }
        #endregion

        #region 文件上传

        /// <summary>
        /// 上传文件函数，成功返回文件路径，不成功返回空字符串（超出大小，文件类型受限，路径不存在，或无写权限）
        /// </summary>
        /// <param name="FU">上传的FileUpload控件ID</param>
        /// <param name="FPath">上传的网站地址，如“/pic/pic/”</param>
        /// <param name="filename_x">生成的文件名前缀，可为空</param>
        /// <param name="maxLength">文件的最大大小，单位KB，0为不限制</param>
        /// <param name="filetype">允许上传的MIME类型，空字符串为不限制</param>
        /// <param name="NewFileName">文件名，不包括路径和扩展名</param>
        /// <returns></returns>
        public string UploadFile(FileUpload FU, string FPath, string filename_x, int maxLength, string filetype, string NewFileName = "",bool DatePath=true)
        {
            string result = "";
            bool chkDelFile = false;
            if (NewFileName == "")
            {
                NewFileName = Upload.GetNewFileName();
                chkDelFile = true;
            }
            if (filename_x.Trim() != "")
                NewFileName = filename_x + "_" + NewFileName;
            string FileName = FU.PostedFile.FileName;
            string FileExt = FileName.Substring(FileName.LastIndexOf('.')).ToLower();//获取文件扩展名


            NewFileName += FileExt;
            bool isOK = true;
            //---------------------------验证文件大小是否超出限制
            if (maxLength > 0)
            {
                int filesize = FU.PostedFile.ContentLength;
                if (filesize / 1024 > maxLength)
                {
                    isOK = false;
                }
            }
            //---------------------------
            if (isOK)
            {
                string FileMime = FU.PostedFile.ContentType; //得到MIME类型
                if (filetype != "" && filetype.IndexOf(FileMime) > 0)  //判断上传类型
                {
                    string syspath = System.Web.HttpContext.Current.Server.MapPath(FPath);
                    if (!System.IO.Directory.Exists(syspath))
                        Directory.CreateDirectory(syspath);
                    if (DatePath)
                    {
                        FPath = FPath + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "/";
                        syspath = System.Web.HttpContext.Current.Server.MapPath(FPath);
                        if (!System.IO.Directory.Exists(syspath))
                            Directory.CreateDirectory(syspath);
                        FPath = FPath + DateTime.Now.Day.ToString() + "/";
                        syspath = System.Web.HttpContext.Current.Server.MapPath(FPath);
                        if (!System.IO.Directory.Exists(syspath))
                            Directory.CreateDirectory(syspath);
                    }
                    try
                    {
                        string __PP = System.Web.HttpContext.Current.Server.MapPath(FPath + NewFileName);
                        if (chkDelFile)
                        {
                            if (File.Exists(__PP))
                                File.Delete(__PP);
                        }
                        FU.SaveAs(__PP);
                        result = FPath + NewFileName;
                    }
                    catch
                    {
                    }
                }

            }
            return result;
        }

        public string UploadDuc(FileUpload FU, string FPath, string filename_x, int maxLength, string filetype, string NewFileName = "", bool DatePath = true)
        {
            string result = "";
            bool chkDelFile = false;
            if (NewFileName == "")
            {
                NewFileName = Upload.GetNewFileName();
                chkDelFile = true;
            }
            if (filename_x.Trim() != "")
                NewFileName = filename_x + "_" + NewFileName;
            string FileName = FU.PostedFile.FileName;
            string FileExt = FileName.Substring(FileName.LastIndexOf('.')).ToLower();//获取文件扩展名

            string fileTypes=FileExt.Substring(1);

            NewFileName += FileExt;
            bool isOK = true;
            //---------------------------验证文件大小是否超出限制
            if (maxLength > 0)
            {
                int filesize = FU.PostedFile.ContentLength;
                if (filesize / 1024 > maxLength)
                {
                    isOK = false;
                }
            }
            //---------------------------
            if (isOK)
            {
                string FileMime = fileTypes;
                if (filetype != "" && filetype.IndexOf(FileMime) > 0)  //判断上传类型
                {
                    string syspath = System.Web.HttpContext.Current.Server.MapPath(FPath);
                    if (!System.IO.Directory.Exists(syspath))
                        Directory.CreateDirectory(syspath);
                    //sav
                    if (DatePath)
                    {
                        FPath = FPath + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "/";
                        syspath = System.Web.HttpContext.Current.Server.MapPath(FPath);
                        if (!System.IO.Directory.Exists(syspath))
                            Directory.CreateDirectory(syspath);
                        FPath = FPath + DateTime.Now.Day.ToString() + "/";
                        syspath = System.Web.HttpContext.Current.Server.MapPath(FPath);
                        if (!System.IO.Directory.Exists(syspath))
                            Directory.CreateDirectory(syspath);
                    }
                    try
                    {
                        string __PP = System.Web.HttpContext.Current.Server.MapPath(FPath + NewFileName);
                        if (chkDelFile)
                        {
                            if (File.Exists(__PP))
                                File.Delete(__PP);
                        }
                        FU.SaveAs(__PP);
                        result = FPath + NewFileName;
                    }
                    catch
                    {
                    }
                }

            }
            return result;
        }
        #endregion

        #region 生成缩略图辅助函数



        /// <summary> 
        /// 获取图像编码解码器的所有相关信息 
        /// </summary> 
        /// <param name="mimeType">包含编码解码器的多用途网际邮件扩充协议 (MIME) 类型的字符串</param> 
        /// <returns>返回图像编码解码器的所有相关信息</returns> 
        private ImageCodecInfo GetCodecInfo(string mimeType)
        {
            ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in CodecInfo)
            {
                if (ici.MimeType == mimeType)
                {
                    return ici;
                }
            }
            return null;
        }

        public string GetImeName(string ext)
        {
            string result = "";
            switch (ext)
            {
                case ".jpe":
                    result = "image/jpeg";
                    break;
                case ".gif":
                    result = "image/gif";
                    break;
                case ".jpeg":
                    result = "image/jpeg";
                    break;
                case ".jpg":
                    result = "image/jpeg";
                    break;
                case ".png":
                    result = "image/png";
                    break;
                case ".tif":
                    result = "image/tiff";
                    break;
                case ".tiff":
                    result = "image/tiff";
                    break;
                case ".bmp":
                    result = "image/bmp";
                    break;
                default:
                    result = "image/jpeg";
                    break;
            }
            return result;

        }

        #endregion

        #region 生成缩略图



        /// <summary>
        /// 生成缩略图,成功返回文件路径，不成功返回空字符串（超出大小，文件类型受限，路径不存在，或无写权限）


        /// </summary>
        /// <param name="originalImagePath">源图路径</param>
        /// <param name="thumbnailPath">缩略图路径</param>
        /// <param name="filename_x">生成的文件名前缀，可为空</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式(HW,H,W,CUT)</param>    
        public string MakeThumbnail(string originalImagePath, string thumbnailPath, string filename_x, int width, int height, string mode)
        {
            return MakeThumbnail(originalImagePath, thumbnailPath, filename_x, width, height, mode, 1, 1);
        }

        /// <summary>
        /// 生成缩略图,成功返回文件路径，不成功返回空字符串（超出大小，文件类型受限，路径不存在，或无写权限）


        /// </summary>
        /// <param name="originalImagePath">源图路径</param>
        /// <param name="thumbnailPath">缩略图路径</param>
        /// <param name="filename_x">生成的文件名前缀，可为空</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式(HW,H,W,CUT,AUTO)</param>  
        /// <param name="BuildNewPath">(0使用thumbnailPath作为图片保存路径;1使用thumbnailPath作为图片保存起始路径(按日期分文件夹扩展))</param>
        /// <param name="BuildNewFileName">(0使用filename_x作为图片名称;1生成图片名称并使用filename_x为前缀)</param>
        public string MakeThumbnail(string originalImagePath, string thumbnailPath, string filename_x, int width, int height, string mode,
                                    int BuildNewPath, int BuildNewFileName)
        {
            string Result = "";
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(System.Web.HttpContext.Current.Server.MapPath(originalImagePath));
            string NewFileName = filename_x.Trim();
            if (BuildNewFileName == 1 || NewFileName.Length == 0)
            {
                if (NewFileName.Length > 0)
                    NewFileName += "_";
                NewFileName += Upload.GetNewFileName();
            }
            string FileExt = originalImagePath.Substring(originalImagePath.LastIndexOf('.')).ToLower();//获取文件扩展名


            NewFileName += FileExt;

            if (BuildNewPath == 1)
            {
                string syspath = System.Web.HttpContext.Current.Server.MapPath(thumbnailPath);
                if (!System.IO.Directory.Exists(syspath))
                    Directory.CreateDirectory(syspath);
                thumbnailPath = thumbnailPath + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "/";
                syspath = System.Web.HttpContext.Current.Server.MapPath(thumbnailPath);
                if (!System.IO.Directory.Exists(syspath))
                    Directory.CreateDirectory(syspath);
                thumbnailPath = thumbnailPath + DateTime.Now.Day.ToString() + "/";
                syspath = System.Web.HttpContext.Current.Server.MapPath(thumbnailPath);
                if (!System.IO.Directory.Exists(syspath))
                    Directory.CreateDirectory(syspath);
            }

            string SysthumbnailPath = System.Web.HttpContext.Current.Server.MapPath(thumbnailPath + NewFileName);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            ImageCodecInfo ici = GetCodecInfo(GetImeName(FileExt));
            EncoderParameters parameters = new EncoderParameters(1);
            parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, ((long)95));

            if (mode == "AUTO")
            {
                if (originalImage.Height > originalImage.Width)
                    mode = "H";
                else
                    mode = "W";
            }

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）                
                    break;
                case "W"://指定宽，高按比例                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "CUT"://指定高宽裁减（不变形）                
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight, PixelFormat.Format32bppArgb);

            //新建一个画板


            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充


            g.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制 原图片 对象
            //g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight)); 

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight), new System.Drawing.Rectangle(x, y, ow, oh), System.Drawing.GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图


                //bitmap.Save(SysthumbnailPath + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                bitmap.Save(SysthumbnailPath, ici, parameters);
                Result = thumbnailPath + NewFileName;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
            return Result;
        }

        #endregion

        #region 在图片上增加文字水印

        /// <summary>
        /// 在图片上增加文字水印
        /// </summary>
        /// <param name="Path">原服务器图片路径</param>
        /// <param name="Path_sy">生成的带文字水印的图片路径</param>
        public void AddWater(string Path, string Path_sy, string str)
        {
            string addText = str;
            System.Drawing.Image image = System.Drawing.Image.FromFile(Path);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            g.DrawImage(image, 0, 0, image.Width, image.Height);
            System.Drawing.Font f = new System.Drawing.Font("Verdana", 60);
            System.Drawing.Brush b = new System.Drawing.SolidBrush(System.Drawing.Color.Green);

            g.DrawString(addText, f, b, 35, 35);
            g.Dispose();

            image.Save(Path_sy);
            image.Dispose();
        }

        #endregion

        #region 在图片上生成图片水印

        /// <summary>
        /// 在图片上生成图片水印
        /// </summary>
        /// <param name="Path">原服务器图片路径</param>
        /// <param name="Path_syp">生成的带图片水印的图片路径</param>
        /// <param name="Path_sypf">水印图片路径</param>
        public void AddWaterPic(string Path, string Path_syp, string Path_sypf)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(Path);
            System.Drawing.Image copyImage = System.Drawing.Image.FromFile(Path_sypf);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            g.DrawImage(copyImage, new System.Drawing.Rectangle(image.Width - copyImage.Width, image.Height - copyImage.Height, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, System.Drawing.GraphicsUnit.Pixel);
            g.Dispose();

            image.Save(Path_syp);
            image.Dispose();
        }

        #endregion

        #region 生成一个随机的文件名（无扩展名）



        /// <summary>
        /// 生成一个随机的文件名（无扩展名）


        /// </summary>
        /// <returns></returns>
        public static string GetNewFileName()
        {

            string checkCode = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);
            string mikecat_strNum = System.DateTime.Now.ToString();
            mikecat_strNum = mikecat_strNum.Replace(":", "");
            mikecat_strNum = mikecat_strNum.Replace("-", "");
            mikecat_strNum = mikecat_strNum.Replace(" ", "");
            mikecat_strNum = mikecat_strNum.Substring(2);
            //mikecat_lngNum += mikecat_intNum;
            return mikecat_strNum + checkCode;
        }

        #endregion

        #region 删除磁盘文件

        public static void MoveFile(string FromFile, string ToFile)
        {
            string FromFilePath = System.Web.HttpContext.Current.Server.MapPath(FromFile);
            string ToFilePath = System.Web.HttpContext.Current.Server.MapPath(ToFile);
            System.IO.File.Move(FromFilePath, ToFilePath);
        }

        #endregion

    }
}
