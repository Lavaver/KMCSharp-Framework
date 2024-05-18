using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace KMCSharp.Auth
{
    public class WarpedAuhenticator : IAuthenticator
    {
        private readonly AuthenticationInfo _info;

        /// <summary>
        ///     创建反常验证器
        /// </summary>
        /// <param name="info">要包装的验证信息</param>
        public WarpedAuhenticator(AuthenticationInfo info)
        {
            _info = info;
        }

        /// <summary>
        ///     标注包装验证器
        /// </summary>
        public string Type
        {
            get { return "LibWarped"; }
        }

        public AuthenticationInfo Do()
        {
            return _info;
        }

        public Task<AuthenticationInfo> DoAsync(CancellationToken token)
        {
            return Task.Factory.StartNew(() => _info, token);
        }
    }
}
