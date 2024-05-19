
using KMCSharp.MainModules.Microsoft.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Xml;
using System.Threading.Tasks;

namespace KMCSharp.Modules.Microsoft.API
{
    internal interface IMicrosoftAPI
    {
        Dictionary<string, ServiceStatus> GetServiceStatus();

        Statistics GetStatistics();

        Guid NameToUUID(string userName);
    }

    public class MainAPI
    {
        private static IMicrosoftAPI Api = Api ?? new MainAPInternal();

        /// <summary>
        /// 取回正版 API 服务状态，取回失败抛异常
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, ServiceStatus> GetServiceStatus()
        {
            return Api.GetServiceStatus();
        }

        /// <summary>
        ///     获取销量等信息，如果获取失败将抛异常
        /// </summary>
        /// <returns></returns>
        public static Statistics GetStatistics()
        {
            return Api.GetStatistics();
        }

        /// <summary>
        ///     通过用户名获取UUID，如果获取失败将抛异常
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public static Guid NameToUUID(string userName)
        {
            return Api.NameToUUID(userName);
        }
    }
}
