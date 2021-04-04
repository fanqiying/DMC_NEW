using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Data;


namespace Utility.HelpClass
{
    /// <summary>
    /// 基本数据类型转换,表达式验证封装类
    /// 2013年6月7日code by jeven_xiao
    /// </summary>   
    public static  class FunConvert
    {
        //格式转换 START--------------------------------------------------------

        #region 把object类型转换为字符类型 To_String To_Str  To_String0_00
        /// <summary>
        /// 把object类型转换为字符类型  
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>字符类型</returns>
        public static string To_String(this object obj, string defaultValue = "")
        {
            if (obj == null)
                return defaultValue;
            else
                return obj.ToString().Trim();
        }
        /// <summary>
        /// 把object类型转换为字符类型

        /// </summary>
        /// <param name="obj"></param>
        /// <returns>字符类型</returns>
        public static string To_String(this object obj, int maxLength)
        {
            string text = obj.To_String();
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            if (text.Length > maxLength)
                text = text.Substring(0, maxLength);
            return text;
        }

        /// <summary>
        /// 当输入的字符串为空字符串时，返回默认值，否则返回自身
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetStr(object a, string value)
        {
            string str = a.To_String();
            if (str == "")
                return value;
            return str;
        }

        /// <summary>
        /// 将double小数转换为带2位小数的字符串

        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string To_String0_00(this double num)
        {
            return num.ToString("0.00");
        }
        /// <summary>
        /// 将float小数转换为带2位小数的字符串

        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string To_String0_00(this float num)
        {
            return num.ToString("0.00");
        }

        #endregion

        #region 把object类型转换为数字类型 To_Int To_Long To_Decimal To_Float To_Double
        /// <summary>
        /// 获取对象的整数形式，非有效整数时返回指定的整数值

        /// </summary>
        /// <param name="a"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static int To_Int(this object a, int num = 0)
        {
            int result = 0;
            try
            {
                result = int.Parse(a.ToString().Trim());
            }
            catch
            {
                result = num;
            }
            return result;
        }

        /// <summary>
        /// 获取对象的长整数形式，非有效整数时返回指定的整数值

        /// </summary>
        /// <param name="a"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static long To_Long(this object a, long num=0)
        {
            long result = 0;
            try
            {
                result = long.Parse(a.ToString().Trim());
            }
            catch
            {
                result = num;
            }
            return result;
        }

        /// <summary>
        /// 获取对象的decimal形式的值，无效时返回0
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static decimal To_Decimal(this object a)
        {
            decimal result = 0.00M;
            try
            {
                result = decimal.Parse(a.ToString().Trim());
            }
            catch
            {
            }
            return decimal.Parse(result.ToString("0.00"));
        }

        /// <summary>
        /// 获取对象的单精度形式的值，无效时返回0
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static float To_Float(this object a)
        {
            float result = 0.00F;
            try
            {
                result = float.Parse(a.ToString().Trim());
            }
            catch
            {
            }
            return float.Parse(result.ToString("0.00"));
        }
        /// <summary>
        /// 获取对象的双精度形式的值，无效时返回0
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static double To_Double(this object a)
        {
            double result = 0.00;
            try
            {
                result = double.Parse(a.ToString().Trim());
            }
            catch
            {
            }
            return double.Parse(result.ToString("0.00"));
        }
        #endregion

        #region 价格处理 To_PriceToPage To_PriceToDB
        /// <summary>
        /// 把object类型转换为显示价格

        /// </summary>
        /// <param name="a"></param>
        /// <param name="ch">金额前缀符号</param>
        /// <returns>价格</returns>
        public static string To_PriceToPage(this object a, string ch="")
        {
            string result = a.To_String();
            try
            {
                result = ((Decimal)a.To_Decimal() / 100.00M).ToString("0.00");
            }
            catch { result = "0.00"; }
            return ch + result;
        }

        /// <summary>
        /// 获取将价格从"分"转换为"元"的十进制数格式

        /// </summary>
        /// <param name="obj">需要转换的原始数值</param>
        /// <param name="DefValue">默认值</param>
        /// <param name="IsInt">是否为整型格式true:当小数部分为.00时仅保留整数部分</param>
        /// <returns></returns>
        public static decimal To_PriceToPage(object obj, decimal DefValue = 0m, bool IsInt = false)
        {
            decimal result = DefValue;
            try
            {
                result = decimal.Parse(obj.ToString());
                result = Math.Round(result / 100, 2, MidpointRounding.AwayFromZero);
            }
            catch { }
            if (IsInt)
                return decimal.Parse(result.ToString("0"));
            else
                return decimal.Parse(result.ToString("0.00"));
        }

        /// <summary>
        /// 获取将价格从"元"转换为"分"的整型格式

        /// </summary>
        /// <param name="obj">需要转换的原始数值</param>
        /// <param name="DefValue">默认值</param>
        /// <returns></returns>
        public static int To_PriceToDB(this object obj, int DefValue = 0)
        {
            int result = DefValue;
            decimal tmpD = 0m;
            try
            {
                tmpD = decimal.Parse(obj.ToString());
                tmpD = Math.Round(tmpD * 100, 0, MidpointRounding.AwayFromZero);
                result = int.Parse(tmpD.ToString());
            }
            catch { }
            return result;
        }

        #endregion

        #region 字符串处理 To_FormatStrForByte To_StringArray To_Replace To_Enum To_CHKSqlStr

        #region 按单字节格式化(截取)字符串 To_FormatStrForByte
        /// <summary>
        /// 按单字节格式化(截取)字符串

        /// </summary>
        /// <param name="title">原始标题字符串</param>
        /// <param name="_length">允许显示的最大字数(1字数等于2字节)</param>
        /// <returns></returns>
        public static string To_FormatStrForByte(string title, int _length)
        {
            string _title = title.To_String();
            int byteL = System.Text.Encoding.Default.GetByteCount(_title);  //标题的字节长度

            int iType = 0;  //字类型 1单字节 2双字节 3英文字母大写
            if (byteL >= _length * 2)
            {
                decimal CurrByteCount = 0;  //当前字节总数
                int CurrCharCount = 0;      //当前字符总数
                int iByteCode = 0;          //字符转换的整数值

                for (int i = 0; i < _title.Length; i++)  //遍历字符串的每个字符(包括单字节和双字节)
                {
                    iByteCode = Convert.ToInt32(_title.ToCharArray()[i]);
                    if (iByteCode > 255)
                    {
                        CurrByteCount += 2;         //按中文字符对待 字节数+2
                        iType = 2;
                    }
                    else
                    {
                        if (iByteCode >= 65 && iByteCode <= 90)
                        {
                            CurrByteCount += 1.2m;     //英文字符大写形式 字节数+1.2
                            iType = 3;
                        }
                        else
                        {
                            CurrByteCount += 1;       //按英文字符对待 字节数+1
                            iType = 1;
                        }
                    }
                    if (CurrByteCount >= _length * 2)
                    {
                        if (_title.Substring(0, CurrCharCount + 1) == title)
                            _title = title;
                        else
                        {
                            if (iType == 2 || iType == 3)
                                _title = _title.Substring(0, CurrCharCount) + "..";
                            else
                                _title = _title.Substring(0, CurrCharCount - 1) + "..";
                        }
                        break;
                    }
                    else
                        CurrCharCount += 1;
                }
            }
            return _title;
        }
        #endregion

        /// <summary>
        /// 把object类型以指定字符分隔，转换为数组

        /// </summary>
        /// <param name="str"></param>
        /// <param name="ch">指定的分割字符</param>
        /// <returns>数组</returns>
        public static string[] To_StringArray(this object str, char ch)
        {
            string text = str.To_String();
            string[] textarray = text.Split(new char[] { ch });
            return textarray;
        }
        /// <summary>
        /// 过滤字符串，多个key用“|”隔开
        /// </summary>
        /// <param name="str"></param>
        /// <param name="keyList"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string To_Replace(this string str, string keyList, string value="")
        {
            string[] arrKey = keyList.Split('|');
            foreach (string s in arrKey)
            {
                str = str.Replace(s, value);
            }
            return str;
        }
        /// <summary>
        /// 把string类型转换为枚举类型.
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="str">需要转化的值</param>
        /// <param name="def">转换失败默认值</param>
        /// <returns>枚举类型</returns>
        public static T To_Enum<T>(this string str, string def)
        {

            T result;
            try
            {
                result = (T)(Enum.Parse(typeof(T), str));
            }
            catch
            {
                result = (T)(Enum.Parse(typeof(T), def));
            }
            return result;
        }

        /// <summary>
        /// 将字符串列表分页
        /// </summary>
        /// <param name="prodcStr">字符串列表</param>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="intPageIndex">当前页码</param>
        /// <param name="rCount">总记录数</param>
        /// <param name="pCount">总页码</param>
        /// <param name="order">排序，默认 倒序</param>
        /// <param name="splitStr">字符串分隔符。默认“,”</param>
        /// <returns></returns>
        public static string Get_PageStr(this string prodcStr, int PageSize, ref int intPageIndex, ref int rCount, ref int pCount, string order = "desc", char splitStr = ',')
        {
            if (prodcStr.IndexOf(splitStr.ToString() + splitStr.ToString())>=0)
                prodcStr = prodcStr.Replace(splitStr.ToString() + splitStr.ToString(), splitStr.ToString());
            if (prodcStr.Substring(0, 1) == splitStr.ToString())
                prodcStr = prodcStr.Substring(1);
            if (prodcStr.Substring(prodcStr.Length-1, 1) == splitStr.ToString())
                prodcStr = prodcStr.Substring(0, prodcStr.Length - 1);

            order = order.ToLower();

            string[] prodarr = prodcStr.Split(splitStr);
            rCount = prodarr.Length;

            pCount = rCount / PageSize;
            if (rCount % PageSize != 0)
                pCount++;
            if (order == "desc")
            {
                if (intPageIndex < 1)
                    intPageIndex = pCount;
            }
            else
            {
                if (intPageIndex < 1)
                    intPageIndex = 1;

            }
            if (intPageIndex > pCount)
                intPageIndex = pCount;
            string prodcStrpage = "";
            if (order == "desc")
            {
                for (int i = rCount - PageSize * (intPageIndex - 1) - 1, count = 0; i >= 0 && count < PageSize; count++, i--)
                {
                    if (prodarr[i]!="")
                        prodcStrpage += "," + prodarr[i];
                }
            }
            else
            {
                for (int i = PageSize * (intPageIndex - 1), count = 0; i < rCount && count < PageSize; count++, i++)
                {
                    if (prodarr[i] != "")
                        prodcStrpage += "," + prodarr[i];
                }
            }
            if (prodcStrpage != "")
                prodcStrpage = prodcStrpage.Substring(1);
            return prodcStrpage;
        }

        /// <summary>
        /// 从字符串数组中随机取num个元素

        /// </summary>
        /// <param name="list"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static IList<string> getListItems(IList<string> list, int num)
        {
            IList<string> temp_list = new List<string>(list);
            IList<string> return_list = new List<string>();
            Random random = new Random();
            for (int i = 0; i < num; i++)
            {
                if (temp_list.Count > 0)
                {
                    int arrIndex = random.Next(0, temp_list.Count);
                    return_list.Add(temp_list[arrIndex]);
                    temp_list.RemoveAt(arrIndex);
                }
                else
                {
                    //列表项取完后,退出循环,比如列表本来只有10项,但要求取出20项.
                    break;
                }
            }
            return return_list;
        }

        #region  过滤文本框字符串 To_CHKSqlStr
        /// <summary>
        /// 过滤文本框字符串
        /// </summary>
        /// <param name="text">Input</param>
        /// <param name="maxLength">长度</param>
        /// <returns>有效的文本框数据</returns>
        public static string To_CHKSqlStr(this string text, int maxLength)
        {
            text = text.Trim();
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            if (text.Length > maxLength)
                text = text.Substring(0, maxLength);
            text = Regex.Replace(text, "[\\s]{2,}", " ");	//two or more spaces
            text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n");	//<br>
            text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " ");	//&nbsp;
            text = Regex.Replace(text, "<(.|\\n)*?>", string.Empty);	//any other tags
            text = text.Replace("'", "''");
            return text;
        }
        /// <summary>
        /// 过滤文本框字符串
        /// </summary>
        /// <param name="text"></param>
        /// <returns>有效的文本框数据</returns>
        public static string To_CHKSqlStr(this string text)
        {
            text = text.Trim();
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            text = Regex.Replace(text, "[\\s]{2,}", " ");	//two or more spaces
            text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n");	//<br>
            text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " ");	//&nbsp;
            text = Regex.Replace(text, "<(.|\\n)*?>", string.Empty);	//any other tags
            text = text.Replace("'", "''");
            return text;
        }
        #endregion

        #endregion

        #region 时间处理 To_DateTime To_DateTimeStr
        /// <summary>
        /// 时间转换
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Nullable<DateTime> To_DateTime(this object obj, Nullable<DateTime> d = null)
        {
            Nullable<DateTime> result;
            try
            {
                result = Convert.ToDateTime(obj);
            }
            catch
            {
                result = d;
            }
            return result;
        }
        public static string To_DateTimeStr(Object obj, IFormatProvider provider, string format = "yyyy-MM-dd", string DefValue = "- -")
        {
            string result = DefValue;
            try
            {
                result = DateTime.Parse(obj.ToString()).ToString(format, provider);
            }
            catch { }
            return result;
        }

        public static string To_DateTimeStr(Object obj, string format = "yyyy-MM-dd", string DefValue = "- -")
        {
            string result = DefValue;
            try
            {
                result = DateTime.Parse(obj.ToString()).ToString(format);
            }
            catch { }
            return result;
        }
        #endregion

        #region 字符串格式化 To_FillFront
        /// <summary>
        /// 在字符串前补字符c，使长度=length
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="length"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string To_FillFront(object obj, int length, char c)
        {
            string r = obj.To_String();
            if (r.Length < length)
            {
                for (int i = 0, j = length - r.Length; i < j; i++)
                {
                    r = c.ToString() + r;
                }
            }
            return r;
        }
        #endregion

        //格式转换 END----------------------------------------------------------

        //验证 START------------------------------------------------------------

        #region 验证是否匹配26个英文字母、数字、下划线
        /// <summary>
        /// 验证是否匹配26个英文字母、数字、下划线
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool Is_LetterFigure_(this string str)
        {
            return Regex.IsMatch(str, @"^[A-Za-z0-9_]+$");
        }

        #endregion

        #region 验证是否匹配26个英文字母、数字

        /// <summary>
        /// 验证是否匹配26个英文字母、数字

        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool Is_LetterFigure(this string str)
        {
            return Regex.IsMatch(str, @"^[A-Za-z0-9]+$");
        }

        #endregion

        #region 验证是否匹配26个英文字母、数字、中杠线
        /// <summary>
        /// 验证是否匹配26个英文字母、数字、中杠线
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool Is_LetterFigureX(this string str)
        {
            return Regex.IsMatch(str, @"^[A-Za-z0-9-]+$");
        }

        #endregion

        #region 验证是否匹配数字
        /// <summary>
        /// 验证是否匹配数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool Is_Figure(this string str)
        {
            return Regex.IsMatch(str, @"^[0-9]+$");
        }

        #endregion

        #region 验证是否有效的邮箱格式

        /// <summary>
        /// 验证是否有效的邮箱格式

        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool Is_Email(this string str)
        {
            return Regex.IsMatch(str, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
        }
        #endregion

        #region 验证是有效的日期格式
        public static bool Is_DateTime(this string str)
        {
            bool r = true;
            try
            {
                DateTime now = DateTime.Parse(str);
            }
            catch
            {
                r = false;
            }
            return r;
        }

        #endregion

        #region 验证是否有效的身份证格式
        public static bool _checkID(string personalid)
        {
            string[] arrVerifyCode = { "1", "0", "x", "9", "8", "7", "6", "5", "4", "3", "2" };
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            byte[] Checker = { 1, 9, 8, 7, 6, 5, 4, 3, 2, 1, 1 };
            string Ai = "";
            if (personalid.Length == 18)
            {
                Ai = personalid.Substring(0, 17);
            }
            else if (personalid.Length == 15)
            {
                Ai = personalid.Substring(0, 6) + "19" + personalid.Substring(6, 9);
            }
            else
            {
                return false;
            }
            if (!Ai.Is_Figure())
            {
                return false;
            }
            string BirthDay = "";
            int intYear = 0;
            int intMonth = 0;
            int intDay = 0;
            int TotalmulAiWi = 0;
            int modValue = 0;
            string strVerifyCode = "";
            intYear = Convert.ToInt32(Ai.Substring(6, 4));
            intMonth = Convert.ToInt32(Ai.Substring(10, 2));
            intDay = Convert.ToInt32(Ai.Substring(12, 2));
            BirthDay = intYear.ToString() + "-" + intMonth.ToString() + "-" + intDay.ToString();
            if (BirthDay.Is_DateTime())
            {
                DateTime DateBirthDay = DateTime.Parse(BirthDay);
                if (DateBirthDay > DateTime.Now)
                {
                    return false;
                }

                int intYearLength = DateBirthDay.Year - DateBirthDay.Year;
                if (intYearLength < -140)
                {
                    return false;
                }
            }
            if (intMonth > 12 || intDay > 31)
            {
                return false;
            }
            for (int i = 0; i < 17; i++)
            {
                try
                {
                    TotalmulAiWi = TotalmulAiWi + (Convert.ToInt32(Ai.Substring(i, 1)) * Convert.ToInt32(Wi[i].ToString()));
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            modValue = TotalmulAiWi % 11;
            strVerifyCode = arrVerifyCode[modValue].ToString();
            Ai = Ai + strVerifyCode;
            if (personalid.Length == 18 && personalid != Ai)
            {
                return false;
            }
            return true;
        }
        public static string ToGetGBKChinese(this string s)
        {
            return System.Web.HttpUtility.UrlDecode(s); 
        }
        public static string To_18_CN_ID(this string CN_ID_15)
        {
            if (CN_ID_15.Length == 15)
            {
                string[] arrVerifyCode = { "1", "0", "x", "9", "8", "7", "6", "5", "4", "3", "2" };
                string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
                byte[] Checker = { 1, 9, 8, 7, 6, 5, 4, 3, 2, 1, 1 };
                string Ai = "";
                Ai = CN_ID_15.Substring(0, 6) + "19" + CN_ID_15.Substring(6, 9);

                int TotalmulAiWi = 0;
                int modValue = 0;
                string strVerifyCode = "";

                for (int i = 0; i < 17; i++)
                {
                    try
                    {
                        TotalmulAiWi = TotalmulAiWi + (Convert.ToInt32(Ai.Substring(i, 1)) * Convert.ToInt32(Wi[i].ToString()));
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
                modValue = TotalmulAiWi % 11;
                strVerifyCode = arrVerifyCode[modValue].ToString();
                Ai = Ai + strVerifyCode;
                return Ai;
            }
            else
                return CN_ID_15;
        }

        /// <summary>
        /// 验证是否有效的中国大陆身份证格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool Is_CN_ID(this string str)
        {
            if (Regex.IsMatch(str, @"(^\d{15}$)|(^\d{17}(?:\d|x|X)$)"))
            {
                return _checkID(str);
            }
            else
                return false;
        }
        #endregion

        #region 验证是否有效的固定电话号码(或传真)
        /// <summary>
        /// 验证是否有效的固定电话号码(或传真)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool Is_PhoneCode(this string str)
        {
            return Regex.IsMatch(str, @"^(\d{3}|\d{4})-\d{8}|\d{4}-\d{7}|\d{7}|\d{8}$");
        }
        #endregion

        #region 验证是否有效的URL地址
        /// <summary>
        /// 验证是否有效的URL地址
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool Is_URL(this string str)
        {
            return Regex.IsMatch(str, @"^(http://)([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$", RegexOptions.IgnoreCase);
        }
        #endregion

        #region 验证非负整数
        /// <summary>
        /// 验证非负整数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool Is_Integer_Z_0(this string str)
        {
            return Regex.IsMatch(str, @"^[1-9]([1-9]|0)\d*$");
        }
        #endregion

        #region 验证数字组合
        /// <summary>
        /// 验证数字组合(别名:整型区域)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool Is_IntArea(this string str)
        {
            return Regex.IsMatch(str, @"^([1-9]|0)\d*$");
        }
        #endregion

        #region 验证整数
        /// <summary>
        /// 验证整数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool Is_Integer(this string str)
        {
            int i;
            try
            {
                i = int.Parse(str);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 验证邮政编码
        /// <summary>
        /// 验证邮政编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool Is_PostalCode(this string str)
        {
            return Regex.IsMatch(str, @"^\d{6}$");
        }
        #endregion

        #region 验证单精度型数据
        /// <summary>
        /// 验证单精度型数据
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool Is_Float(this string str)
        {
            float f;
            try
            {
                f = float.Parse(str);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 验证手机号码
        /// <summary>
        /// 验证手机号码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool Is_MobileCode(this string str)
        {
            return Regex.IsMatch(str, @"^(013|015|018|13|15|18)\d{9}$");
        }
        #endregion

        #region 按单字节验证字符串长度是否超过指定长度

        /// <summary>
        /// 按单字节验证字符串长度是否超过指定长度

        /// </summary>
        /// <param name="title">需要验证的字符串</param>
        /// <param name="_length">允许的最大单字节数量</param>
        /// <returns>字符串单字节长度小于等于允许的最大单字节数量：真|大于则：假</returns>
        public static bool Is_StringLength(this string title, int _length)
        {
            int byteL = System.Text.Encoding.Default.GetByteCount(title);  //获取字节长度
            if (byteL > _length)
                return false;
            else
                return true;
        }
        #endregion

        //验证 END--------------------------------------------------------------

        #region 过滤html标记
        /// <summary>
        /// 过滤html标记
        /// </summary>
        /// <param name="HTMLStr">要过滤的字符串</param>
        /// <returns>string</returns>
        public static string CutHTML(this string strHtml)
        {
            string[] aryReg ={   
          @"<script[^>]*?>.*?</script>",      
          @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(\\[""'tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>",    
          @"([\r\n])[\s]+",   
          @"&(quot|#34);",   
          @"&(amp|#38);",   
          @"&(lt|#60);",   
          @"&(gt|#62);",     
          @"&(nbsp|#160);",     
          @"&(iexcl|#161);",   
          @"&(cent|#162);",   
          @"&(pound|#163);",   
          @"&(copy|#169);",   
          @"&#(\d+);",   
          @"-->",   
          @"<!--.*\n"   
         };

            string[] aryRep =   {   
             "",   
             "",   
             "",   
             "\"",   
             "&",   
             "<",   
             ">",   
             "   ",   
             "\xa1",//chr(161),   
             "\xa2",//chr(162),   
             "\xa3",//chr(163),   
             "\xa9",//chr(169),   
             "",   
             "\r\n",   
             ""   
            };
            string newReg = aryReg[0];
            string strOutput = strHtml;
            for (int i = 0; i < aryReg.Length; i++)
            {
                Regex regex = new Regex(aryReg[i], RegexOptions.IgnoreCase);
                strOutput = regex.Replace(strOutput, aryRep[i]);
            }
            strOutput.Replace("<", "");
            strOutput.Replace(">", "");
            strOutput.Replace("\r\n", "");
            return strOutput;
        }
        #endregion

        

        /// <summary>
        /// 如果是空字符串（NULL 或 ""）,返回默认值def
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static string Is_null(this object obj, string def)
        {
            if (obj.To_String() == "")
                return def;
            else
                return obj.To_String();
        }

     
    }
}
