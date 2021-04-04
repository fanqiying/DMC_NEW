using System.Xml;
using System.Text.RegularExpressions;
namespace Utility.HelpClass
{
    /// <summary>
    /// XML 文件操作封装类(包含对XML文件的相关操作)
    /// 2012.3.16
    /// code by xiaolibing
    ///  </summary>

    public class XmlHelper
    {
        private XmlDocument xmlDoc = null;

        public XmlHelper()
        {
            xmlDoc = new XmlDocument();
        }

        /// <summary>
        /// 加载xml文件
        /// </summary>
        /// <param name="fileName">xml 文件路径</param>
        public void Load(string fileName)
        {
            xmlDoc.Load(fileName);
        }

        /// <summary>
        /// xml文件保存
        /// </summary>
        /// <param name="path"></param>
        public void Save(string path)
        {
            xmlDoc.Save(path);
        }

        /// <summary>
        /// 读取xml内容
        /// </summary>
        /// <param name="xml">xml 内容</param>
        public void LoadXml(string xml)
        {
            xmlDoc.LoadXml(xml);
        }

        /// <summary>
        /// xpath 是否存在
        /// </summary>
        /// <param name="xpath">xpath 路径</param>
        /// <returns></returns>
        public bool IsExists(string xpath)
        {
            return xmlDoc.DocumentElement.SelectSingleNode(xpath) != null ? true : false;
        }

        /// <summary>
        /// xpath 是否存在
        /// </summary>
        /// <param name="xn">节点</param>
        /// <param name="xpath">xpath 路径</param>
        /// <returns></returns>
        public bool IsExists(XmlNode xn, string xpath)
        {
            return xn.SelectSingleNode(xpath) != null ? true : false;
        }

        /// <summary>
        /// 选择指定节点
        /// </summary>
        /// <param name="xpath">xpath 路径</param>
        /// <returns></returns>
        public XmlNodeList SelectNodes(string xpath)
        {
            return xmlDoc.DocumentElement.SelectNodes(xpath);
        }

        /// <summary>
        /// 选择指定节点
        /// </summary>
        /// <param name="xn">节点</param>
        /// <param name="xpath">xpath 路径</param>
        /// <returns></returns>
        public XmlNodeList SelectNodes(XmlNode xn, string xpath)
        {
            return xn.SelectNodes(xpath);
        }

        /// <summary>
        /// 选择单节点
        /// </summary>
        /// <param name="xpath">xpath 路径</param>
        /// <returns></returns>
        public string SelectSingleNode(string xpath)
        {
            return IsExists(xpath) ? xmlDoc.DocumentElement.SelectSingleNode(xpath).InnerText : string.Empty;
        }

        /// <summary>
        /// 选择单节点
        /// </summary>
        /// <param name="xn">节点</param>
        /// <param name="xpath">xpath 路径</param>
        /// <returns></returns>

        public string SelectSingleNode(XmlNode xn, string xpath)
        {
            return IsExists(xn, xpath) ? xn.SelectSingleNode(xpath).InnerText : string.Empty;
        }

        /// <summary>
        /// 过滤XML中的非法字符，在解析之前先把字符串中的这些非法字符过滤掉
        /// </summary>
        /// <param name="errStr">需要过滤的字符串</param>
        /// <returns>过滤后的字符串</returns>
        public static string XmlSafeStr(string errStr)
        {
            string strOutput = errStr;
            strOutput = Regex.Replace(strOutput, "[\\x00-\\x08\\x0b-\\x0c\\x0e-\\x1f]", "");
            strOutput = strOutput.Replace("&", "");
            strOutput = strOutput.Replace("&apos;", "");
            strOutput = strOutput.Replace("&quot;", "");
            strOutput = strOutput.Replace("&gt;", "");
            strOutput = strOutput.Replace("&lt;", "");
            return strOutput;
        }
    }
}
