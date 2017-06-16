using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Longjin.Util
{
    public class DTKeys
    {

        /// <summary>
        /// 当前登录用户的信息
        /// </summary>
        public const string SessionKey_CurrentUser = "SessionKey_CurrentUser";

        /// <summary>
        /// 当前登录用户的用户Id
        /// </summary>
        public const string SessionKey_CurrentId = "SessionKey_CurrentId";

        /// <summary>
        /// 用户缓存key格式
        /// </summary>
        public const string CacheKey_UserInfo = "CacheKey_UserInfo_{0}";
    }
}
