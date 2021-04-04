/*******************************************************************
 * 類  名（ClassName）  ：QMS.BLL.common
 * 關鍵字（KeyWord）    ：
 * 描  述（Description）：
 * 版  本（Version）    ：1.0
 * 日  期（Date）       ：2020/11/23 10:59:50
 * 作  者（Author）     ：devin_shu
******************************修改記錄******************************
 * 版本       時間      作者      描述
 * 
********************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMC.BLL.common
{
    public class FileHelper
    {
        private string m_FileURL = string.Empty;
        public FileHelper(string p_FilePath)
        {
            m_FileURL = p_FilePath;
            if (Directory.Exists(p_FilePath)==false)
            {
                Directory.CreateDirectory(m_FileURL);
            }
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="p_FileName">文件名</param>
        /// <param name="p_FileBytes">文件流</param>
        /// <returns></returns>
        public bool SaveFile(string p_FileName, byte[] p_FileBytes)
        {
            try
            {
                string filePath = Path.Combine(m_FileURL, p_FileName);
                File.WriteAllBytes(filePath, p_FileBytes);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SaveFile(string p_Path, string p_FileName, byte[] p_FileBytes)
        {
            try
            {
                if (Directory.Exists(p_Path) == false)
                {
                    Directory.CreateDirectory(p_Path);
                }
                string filePath = Path.Combine(p_Path, p_FileName);
                File.WriteAllBytes(filePath, p_FileBytes);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 刪除本地待上傳文件夾中的文件
        /// </summary>
        /// <param name="p_FileName">文件名</param>
        /// <returns></returns>
        public bool DeleteFile(string p_FileName)
        {
            try
            {
                string fileFullName = Path.Combine(m_FileURL, p_FileName);
                if (File.Exists(fileFullName))
                {
                    File.Delete(fileFullName);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取目录下所有文件的文件名集合（子文件夹下的文件不包含）
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllFileName()
        {
            List<string> lst = new List<string>();
            try
            {
                DirectoryInfo dinfo = new DirectoryInfo(m_FileURL);
                foreach (var f in dinfo.GetFileSystemInfos())
                {
                    if (f is FileInfo)
                    {
                        lst.Add(f.Name);
                    }
                }
                return lst;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 獲取文件字節流
        /// </summary>
        /// <param name="p_FileName"></param>
        /// <returns></returns>
        public byte[] GetFileByte(string p_FileName)
        {
            try
            {
                string filePath = Path.Combine(m_FileURL, p_FileName);
                byte[] bytes;
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    bytes = new byte[fs.Length];
                    fs.Read(bytes, 0, (int)fs.Length);
                    fs.Close();
                }
                return bytes;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 驗證文件是否存在
        /// </summary>
        /// <param name="p_FileName"></param>
        /// <returns></returns>
        public bool ExistFile(string p_FileName)
        {
            try
            {
                string filePath = Path.Combine(m_FileURL, p_FileName);
                return File.Exists(filePath);
            }
            catch
            {
                return false;
            }
        }
    }
}
