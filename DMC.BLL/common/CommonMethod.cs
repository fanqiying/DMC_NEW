using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DMC.BLL.common
{
    public class CommonMethod
    {
        /// <summary>
        /// object安全轉換成int
        /// </summary>
        /// <param name="o"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int SafeGetIntFromObj(object o, int defaultValue)
        {
            if (((o == null) || (o == DBNull.Value)) || (o.ToString() == ""))
            {
                return defaultValue;
            }
            int result = -1;
            if (!int.TryParse(o.ToString(), out result))
            {
                try
                {
                    result = Convert.ToInt32(o);
                }
                catch
                {
                    result = defaultValue;
                }
            }
            return result;
        }

        /// <summary>
        /// string安全轉換成float
        /// </summary>
        /// <param name="textvalue"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public static float SafeGetFloatFromString(string textvalue, float defaultvalue)
        {
            if (string.IsNullOrEmpty(textvalue))
            {
                return defaultvalue;
            }
            float num = defaultvalue;
            try
            {
                num = (float)Convert.ToDouble(textvalue);
            }
            catch
            {
            }
            return num;
        }

        /// <summary>
        /// object安全轉換成float
        /// </summary>
        /// <param name="textvalue"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public static float SafeGetFloatFromObject(object textvalue, float defaultvalue)
        {
            if ((textvalue == null) || (textvalue == DBNull.Value))
            {
                return defaultvalue;
            }
            float num = defaultvalue;
            try
            {
                num = (float)Convert.ToDouble(textvalue.ToString());
            }
            catch
            {
            }
            return num;
        }

        /// <summary>
        /// object安全轉換成double
        /// </summary>
        /// <param name="textvalue"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public static double SafeGetDoubleFromObject(object textvalue, double defaultvalue)
        {
            if ((textvalue == null) || (textvalue == DBNull.Value))
            {
                return defaultvalue;
            }
            double num = defaultvalue;
            try
            {
                num = Convert.ToDouble(textvalue.ToString());
            }
            catch
            {
            }
            return num;
        }

        /// <summary>
        /// object安全轉換成decimal
        /// </summary>
        /// <param name="objValue"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public static decimal SafeGetDecimalFromObject(object objValue, decimal defaultvalue)
        {
            if ((objValue == null) || (objValue == DBNull.Value))
            {
                return defaultvalue;
            }
            decimal num = defaultvalue;
            try
            {
                num = Convert.ToDecimal(objValue);
            }
            catch
            {
            }
            return num;
        }

        /// <summary>
        /// object安全轉換成long
        /// </summary>
        /// <param name="o"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static long SafeGetLongFromObj(object o, long defaultValue)
        {
            if (((o == null) || (o == DBNull.Value)) || (o.ToString() == ""))
            {
                return defaultValue;
            }
            long result = -1L;
            return (long.TryParse(o.ToString(), out result) ? result : defaultValue);
        }

        /// <summary>
        /// object安全轉換成string
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string SafeGetStringFromObj(object o)
        {
            return ((o == null) ? "" : ((o == DBNull.Value) ? "" : o.ToString().Trim()));
        }

        /// <summary>
        /// 安全過濾字符串中的特殊字符
        /// 默認過濾非中文、字母、數字、中劃線、下劃線、小數點、米號、中英文逗號、小括號等字符
        /// </summary>
        /// <param name="strValue">需過濾的字符串</param>
        /// <param name="strPattern">自定義過濾正則表達式</param>
        /// <returns></returns>
        public static string SafeFilterSpecialChar(string strValue,string strPattern="")
        {
            string pattern = "[A-Za-z0-9\u4e00-\u9fa5-_.\\*,，\\(\\)\\s`~!@#\\$%\\^&\\+=\\|\\\\<>\\?/]+";
            if (string.IsNullOrWhiteSpace(strPattern)==false)
                pattern = strPattern;
            string result = "";
            try
            {
                MatchCollection mc = Regex.Matches(strValue, pattern);
                if (mc == null || mc.Count <= 0)
                    return result;
                for (int i = 0; i < mc.Count; i++)
                {
                    result += mc[i].ToString();
                }
                return result;
            }
            catch
            {
                return result;
            }
        }
    }
}
