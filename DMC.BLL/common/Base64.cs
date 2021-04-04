using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMC.BLL.common
{
    /// Base64加密解密
    ///
    public class Base64
    {
        ///
        /// 将字符串使用base64算法加密
        ///
        /// 待加密的字符串
        /// System.Text.Encoding 对象，如创建中文编码集对象：System.Text.Encoding.GetEncoding(54936)
        /// 加码后的文本字符串
        public static string EncodingForString(string sourceString, System.Text.Encoding ens)
        {
            return Convert.ToBase64String(ens.GetBytes(sourceString));
        }

        ///
        /// 将字符串使用base64算法加密
        ///
        /// 待加密的字符串
        /// 加码后的文本字符串
        public static string EncodingForString(string sourceString)
        {
            return EncodingForString(sourceString, System.Text.Encoding.GetEncoding(65001));
        }


        ///
        /// 从base64编码的字符串中还原字符串，支持中文
        ///
        /// base64加密后的字符串
        /// System.Text.Encoding 对象，如创建中文编码集对象：System.Text.Encoding.GetEncoding(54936)
        /// 还原后的文本字符串
        public static string DecodingForString(string base64String, System.Text.Encoding ens)
        {
            return ens.GetString(Convert.FromBase64String(base64String));
        }

        ///
        /// 从base64编码的字符串中还原字符串，支持中文
        ///
        /// base64加密后的字符串
        /// 还原后的文本字符串
        public static string DecodingForString(string base64String)
        {
            return DecodingForString(base64String, System.Text.Encoding.GetEncoding(65001));
        }

        ///
        /// 对任意类型的文件进行base64加码
        ///
        /// 文件的路径和文件名
        /// 对文件进行base64编码后的字符串
        public static string EncodingForFile(string fileName)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(fileName);
            System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
            string base64String = Convert.ToBase64String(br.ReadBytes((int)fs.Length));
            br.Close();
            fs.Close();
            return base64String;
        }

        ///
        /// 把经过base64编码的字符串保存为文件
        ///
        /// 经base64加码后的字符串
        /// 保存文件的路径和文件名
        /// 保存文件是否成功
        public static bool SaveDecodingToFile(string base64String, string fileName)
        {
            System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Create);
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(fs);
            bw.Write(Convert.FromBase64String(base64String));
            bw.Close();
            fs.Close();
            return true;
        }
    }

}
