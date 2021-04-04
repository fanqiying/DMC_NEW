using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Utility.HelpClass
{
    internal class TokenKey
    {
        #region 属性定义
        public string Token { get; set; }
        public CodeUsage Usage { get; set; }
        #endregion
        /// <summary>
        /// 返回字符串的哈希代码
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Token.GetHashCode() + Usage.ToString().GetHashCode();
        }
        /// <summary>
        /// 匹配方法
        /// </summary>
        /// <param name="obj">匹配對象</param>
        /// <returns>bool</returns>
        public override bool Equals(object obj)
        {
            var o = obj as TokenKey;
            return (o != null) && (o.Token == Token && o.Usage == Usage);
        }
    }
    /// <summary>
    /// 编码生成類
    /// </summary>
    public static class CodeGenerator
    {
        //定義鍵值對
        static Dictionary<TokenKey, Code> codePoll = new Dictionary<TokenKey, Code>();
        static Random random = new Random();
        static object lockObj = new object();
        //生成方法
        public static string Generate(string token, CodeUsage usage)
        {
            //驗證配置對象
            var config = CheckConfig(usage);
            Code code = null;
            var pollKey = new TokenKey() { Usage = usage, Token = token };
            lock (lockObj)
            {
                //鍵值對賦值
                codePoll = codePoll.Where(c => c.Value.Expiration > DateTime.Now).ToDictionary(k => k.Key, k => k.Value);
                if (codePoll.ContainsKey(pollKey))
                    code = codePoll[pollKey];
                else
                {
                    code = InternalGenerate(config);
                    codePoll.Add(pollKey, code);
                }
            };
            return code.Value;
        }
        /// <summary>
        /// 驗證Code
        /// </summary>
        /// <param name="token"></param>
        /// <param name="code">碼</param>
        /// <param name="usage">枚舉變量</param>
        /// <returns></returns>
        public static bool CheckCode(string token, string code, CodeUsage usage)
        {
            var pollKey = new TokenKey() { Usage = usage, Token = token };
            lock (lockObj)
            {
                codePoll = codePoll.Where(c => c.Value.Expiration > DateTime.Now).ToDictionary(k => k.Key, k => k.Value);
            }
            return codePoll.ContainsKey(pollKey) && codePoll[pollKey].Value == code;
        }
        /// <summary>
        /// code 生成類
        /// </summary>
        /// <param name="config">配置對象</param>
        /// <returns>Code</returns>
        private static Code InternalGenerate(Config config)
        {
            int number;
            char code;
            string checkCode = String.Empty;
            for (int i = 0; i < config.CodeLength; i++)
            {
                number = random.Next();
                if (number % 3 == 0)
                {
                    code = (char)('A' + (char)(number % 26));
                }
                else
                {
                    code = (char)('0' + (char)(number % 10));
                }
                checkCode += code.ToString();
            }
            return new Code() { Value = checkCode, Expiration = DateTime.Now.Add(config.Expiration) };
        }
        /// <summary>
        /// 配置對象驗證
        /// </summary>
        /// <param name="usage">枚舉使用對象</param>
        /// <returns></returns>
        private static Config CheckConfig(CodeUsage usage)
        {
            Config config = new Config();
            config.ReadConfig(usage);
            return config;
        }
    }
    /// <summary>
    /// 配置類
    /// </summary>
    internal class Config
    {
        /// <summary>
        /// 配置類構造方法
        /// </summary>
        public Config()
        {
            CodeLength = 4;
            Expiration = TimeSpan.FromSeconds(1);
        }
        /// <summary>
        /// 讀取節點配置
        /// </summary>
        /// <param name="usage">枚舉變量</param>
        public void ReadConfig(CodeUsage usage)
        {
            var settings = ConfigurationManager.AppSettings[usage.ToString().ToLower()];
            if (settings == null)
                throw new ArgumentException("缺少AppSettings配置節:" + usage.ToString().ToLower());
            var mins = 0;
            if (Int32.TryParse(settings, out mins))
            {
                Expiration = TimeSpan.FromSeconds(mins);
            }
            else
            {
                throw new ArgumentException("AppSettings配置節配置不正確:" + usage.ToString().ToLower());
            }
        }
        //code長度屬性
        public int CodeLength { get; set; }
        //時間
        public TimeSpan Expiration { get; set; }
    }
    internal class Code
    {
        public string Value { get; set; }
        public DateTime Expiration { get; set; }
        public override string ToString()
        {
            return Value;
        }
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            var o = obj as Code;
            return (o != null) && (o.Value == Value);
        }
    }
    /// <summary>
    /// 編碼枚舉
    /// </summary>
    public enum CodeUsage
    {
        Mail,//郵件
        WeChat,//微信
        Message//消息
    }
}
