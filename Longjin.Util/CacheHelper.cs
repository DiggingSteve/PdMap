using System;
using System.Web.Caching;
using System.Web;

namespace Longjin.Util
{
    /// <summary>
    /// 
    /// </summary>
    public class CacheHelper
    {
        /// <summary>
        /// 创建缓存项的文件依赖
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="obj">object对象</param>
        /// <param name="fileName">文件绝对路径</param>
        public static void Insert(string key, object obj, string fileName)
        {
            //创建缓存依赖项
            CacheDependency dep = new CacheDependency(fileName);
            //创建缓存
            HttpRuntime.Cache.Insert(key, obj, dep);
        }

        /// <summary>
        /// 插入缓存数据 绝对过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expires"></param>
        public static void Insert(string key, object obj, int expires)
        {
            if (string.IsNullOrEmpty(key) || obj == null)
            {
                return;
            }
            HttpRuntime.Cache.Insert(key, obj, null, DateTime.Now.AddMinutes(expires), Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// 可调过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="slidingExpiration"></param>
        public static void Insert(string key, object obj, TimeSpan slidingExpiration)
        {
            if (string.IsNullOrEmpty(key) || obj == null)
            {
                return;
            }
            HttpRuntime.Cache.Insert(key, obj, null, DateTime.UtcNow.Add(slidingExpiration), slidingExpiration);
        }

        /// <summary>
        /// 缓存不过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Insert(string key, object value)
        {
            if (string.IsNullOrEmpty(key) || value == null)
            {
                return;
            }
            HttpRuntime.Cache.Insert(key, value);
        }

        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Set(string key, object value)
        {
            if (string.IsNullOrEmpty(key) || value == null)
            {
                return;
            }
            HttpRuntime.Cache.Remove(key);
            Insert(key, value);
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        public static object Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            return HttpRuntime.Cache.Remove(key);
        }

        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns>object对象</returns>
        public static object Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            return HttpRuntime.Cache.Get(key);
        }

        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <typeparam name="T">T对象</typeparam>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            object obj = Get(key);
            return obj == null ? default(T) : (T)obj;
        }
    }
}
