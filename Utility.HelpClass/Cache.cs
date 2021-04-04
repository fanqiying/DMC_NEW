using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using System.Collections;
using System.Web;
namespace Utility.HelpClass
{
	/// <summary>
	/// 公共缓存操作类
	/// code by jeven_xiao
	/// 2013-6-9
	/// </summary>
	public static class Cache {
		public static void ClearAll() {
			IDictionaryEnumerator enumerator = HttpContext.Current.Cache.GetEnumerator();
			while (enumerator.MoveNext()) {
				HttpContext.Current.Cache.Remove(enumerator.Key.ToString());
			}
		}

		public static bool Contains(string key) {
			return (HttpContext.Current.Cache[key] != null);
		}


		/// <summary>
		/// 缓存是否存在
		/// </summary>
		/// <param name="key">缓存的键值</param>
		/// <returns></returns>
		public static bool Exists(string key) {
			return Contains(key);
		}


		/// <summary>
		/// 获取缓存值

		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static object GetCache(string key) {
			try {
				if (HttpContext.Current.Cache[key] != null) {
					return HttpContext.Current.Cache[key];
				}
			}
			catch {
			}
			return null;
		}

		/// <summary>
		/// 获取缓存值
		/// </summary>
		/// <param name="key">缓存KEY</param>
		/// <param name="toString">转换成字符串</param>
		/// <returns></returns>
		public static string GetCache(string key, bool toString) {
			try {
				if (HttpContext.Current.Cache[key] != null) {
					return ToString(HttpContext.Current.Cache[key]);
				}
			}
			catch {
			}
			return "";
		}


		/// <summary>
		/// 获取缓存并转换为string
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static string GetCacheToString(string key) {
			try {
				if (HttpContext.Current.Cache[key] != null) {
					return ToString(HttpContext.Current.Cache[key]);
				}
			}
			catch {
			}
			return "";
		}

		public static bool IsReply(string cacheName, int seconds) {
			if (!Exists(cacheName)) {
				SetCache(cacheName, "", seconds);
				return true;
			}
			return false;
		}


		/// <summary>
		/// 移除缓存
		/// </summary>
		/// <param name="key"></param>
		public static void Remove(string key) {
			try {
				if (HttpContext.Current.Cache[key] != null) {
					HttpContext.Current.Cache.Remove(key);
				}
			}
			catch {
			}
		}


		/// <summary>
		/// 设置缓存
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="seconds">从当前时间起缓存多少秒</param>
		/// <returns></returns>
		public static bool SetCache(string key, object value, int seconds) {
			if (value != null) {
				try {
					if (HttpContext.Current.Cache[key] != null) {
						HttpContext.Current.Cache.Remove(key);
					}
					if (seconds <= 0) {
						HttpContext.Current.Cache.Insert(key, value);
					}
					else {
						HttpContext.Current.Cache.Insert(key, value, null, DateTime.Now.AddSeconds((double)seconds), Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
					}
					return true;
				}
				catch {
				}
			}
			return false;
		}

		public static string ToString(object obj) {
			if (obj == null) {
				return "";
			}
			return obj.ToString();
		}

		public static TimeSpan NoSlidingExpiration { get; set; }
	}
}
