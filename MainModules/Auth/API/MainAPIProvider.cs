﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMCSharp.MainModules.Microsoft.API
{
    public class MainAPIProvider
    {
        /// <summary>
        ///     API可用状态查询（GET）
        /// </summary>
        public static string apiStatus()
        {
            return "https://user.auth.xboxlive.com";
        }

        public static string nameToUuid(string userName)
        {
            return "https://api.mojang.com/users/profiles/minecraft/" + userName;
        }

        /// <summary>
        ///     获取指定uuid账户历史名称（GET）
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public static string historyName(Guid uuid)
        {
            return string.Format("https://api.mojang.com/user/profiles/{0}/names", uuid);
        }

        /// <summary>
        ///     返回账户信息（登录后，GET）
        /// </summary>
        /// <returns></returns>
        public static string userInfo()
        {
            return "https://api.mojang.com/user";
        }

        /// <summary>
        ///     获取销量等信息（POST）
        /// </summary>
        /// <returns></returns>
        public static string statistics()
        {
            return "https://api.mojang.com/orders/statistics";
        }
    }
}
