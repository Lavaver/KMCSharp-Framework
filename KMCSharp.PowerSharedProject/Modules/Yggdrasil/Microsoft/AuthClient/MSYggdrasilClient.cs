using LitJson;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace KMCSharp.Modules.Yggdrasil
{
    /// <summary>
    /// 微软正版验证客户端<br></br>
    /// 特别感谢如下项目对本项目的鼎力支持！没有你们，将无法支撑起 KMCSharp Framework 复兴项目的推动：<br></br>
    /// - Yuns-Lab/MCMsLogin-Python: Python 版本的一个微软登录 Minecraft 类库（<see href="https://github.com/Yuns-Lab/MCMsLogin-Python"/>）<br></br>
    /// - LitJson （<see href="https://www.newtonsoft.com/json"/>）<br></br>
    /// 这个验证客户端相比起 KMCCC 框架代码对 Microsoft 登录做了针对性优化，例如针对使用 Microsoft 令牌获取 Xbox Live （XBL）令牌，再用 Xbox 令牌获取 XSTS 令牌，然后通过 XSTS 令牌取回 Minecraft 访问令牌，精简且现代化的异步代码可快速登录正版 Minecraft<br></br>
    /// 同时，鉴于整个过程均为非输入帐密进行，因此需要使用浏览器进行登录。登陆完成后页面会变白，这时候需要将地址栏地址复制粘贴到要求输入的地方，该类库将自动提取链接中 code 部分作为 url 参数传入
    /// </summary>
    public class MSYggdrasilClient
    {
        private static readonly HttpClient httpClient = new HttpClient();

        
        public class XboxLiveAuthenticationResponse
        {
            public string Token { get; set; }
            public DisplayClaims DisplayClaims { get; set; }
        }

        public class DisplayClaims
        {
            public Xui[] xui { get; set; }
        }

        public class Xui
        {
            public string uhs { get; set; }
        }

        public class MinecraftTokenResponse
        {
            public string access_token { get; set; }
        }


        /// <summary>
        /// 登陆步骤 1 - 取回 Xbox Live 令牌
        /// </summary>
        /// <param name="url">Minecraft 网页登陆完成后地址栏中的 code 值</param>
        /// <returns>Xbox Live 令牌（同时也会一同返回 uhs （用户哈希码），uhs 为第三步获取 Minecraft 访问令牌最重要的钥匙。没有它，将无法登录）</returns>
        /// <exception cref="Exception"></exception>
        public static async Task<(string xblToken, string uhs)> Auth_XBL(string url)
        {
            Console.WriteLine("开始微软正版验证，正在取回 Xbox Live 令牌（步骤 1 / 4）");
            var code = url.Split('=')[1];
            var content = new FormUrlEncodedContent(new[]
            {
        new KeyValuePair<string, string>("Properties", "{\"AuthMethod\":\"RPS\",\"SiteName\":\"user.auth.xboxlive.com\",\"RpsTicket\":" + code + "}"),
        new KeyValuePair<string, string>("RelyingParty", "http://auth.xboxlive.com"),
        new KeyValuePair<string, string>("TokenType", "JWT")
    });

            var response = await httpClient.PostAsync("https://user.auth.xboxlive.com/user/authenticate", content);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var authenticationResponse = JsonMapper.ToObject<XboxLiveAuthenticationResponse>(responseBody);
                Console.WriteLine("取回 Xbox Live 令牌成功（步骤 1 / 4）");
                return (authenticationResponse.Token, authenticationResponse.DisplayClaims.xui[0].uhs);
            }
            else
            {
                Console.WriteLine("取回 Xbox Live 令牌失败");
                throw new Exception();
            }
        }

        /// <summary>
        /// 登陆步骤 2 - 取回 XSTS 令牌
        /// </summary>
        /// <param name="xblToken">Xbox Live 令牌</param>
        /// <returns>XSTS 令牌与用户哈希码</returns>
        /// <exception cref="Exception"></exception>
        public static async Task<(string xstsToken, string uhs)> Auth_XSTS(string xblToken)
        {
            Console.WriteLine("开始微软正版验证，正在取回 XSTS 令牌（步骤 2 / 4）");
            var content = new FormUrlEncodedContent(new[]
            {
        new KeyValuePair<string, string>("Properties", "{\"SandboxId\":\"RETAIL\",\"UserTokens\":[\"" + xblToken + "\"]}"),
        new KeyValuePair<string, string>("RelyingParty", "rp://api.minecraftservices.com/"),
        new KeyValuePair<string, string>("TokenType", "JWT")
    });

            var response = await httpClient.PostAsync("https://xsts.auth.xboxlive.com/xsts/authorize", content);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var authenticationResponse = JsonMapper.ToObject<XboxLiveAuthenticationResponse>(responseBody);
                Console.WriteLine("取回 XSTS 令牌成功（步骤 2 / 4）");
                return (authenticationResponse.Token, authenticationResponse.DisplayClaims.xui[0].uhs);
            }
            else
            {
                Console.WriteLine("取回 XSTS 令牌失败");
                throw new Exception();
            }
        }

        /// <summary>
        /// 登陆步骤 3 - 取回 Minecraft 访问令牌
        /// </summary>
        /// <param name="uhs">用户哈希码</param>
        /// <param name="xstsToken">XSTS 令牌</param>
        /// <returns>Minecraft 访问令牌</returns>
        /// <exception cref="Exception"></exception>
        public static async Task<string> GetMinecraftToken(string uhs, string xstsToken)
        {
            Console.WriteLine("开始微软正版验证，正在取回 Minecraft 令牌（步骤 3 / 4）");
            var content = new FormUrlEncodedContent(new[]
            {
            new KeyValuePair<string, string>("identityToken", $"XBL3.0 x={uhs};{xstsToken}")
        });

            var response = await httpClient.PostAsync("https://api.minecraftservices.com/authentication/login_with_xbox", content);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonMapper.ToObject<MinecraftTokenResponse>(responseBody);
                Console.WriteLine("取回 Minecraft 令牌成功（步骤 3 / 4）");
                return tokenResponse.access_token;
            }
            else
            {
                Console.WriteLine("取回 Minecraft 令牌失败");
                throw new Exception();
            }
        }

        /// <summary>
        /// 登陆步骤 4 - 检查是否购入正版
        /// </summary>
        /// <param name="token">Minecraft 访问令牌</param>
        /// <returns>若购买了正版则返回购入版本状态并提示登陆完成，如果未购买正版则抛出异常</returns>
        /// <exception cref="Exception"></exception>
        public static async Task<(bool haveJE, bool haveBE)> CheckLicense(string token)
        {
            Console.WriteLine("开始微软正版验证，正在检查用户是否已购买 Minecraft（步骤 4 / 4）");
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.minecraftservices.com/entitlements/mcstore");
            request.Headers.Add("Authorization", $"Bearer {token}");

            var response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var json = JsonMapper.ToObject(responseBody);
                var items = json["items"];
                bool haveJE = false, haveBE = false;
                for (int i = 0; i < items.Count; i++)
                {
                    var item = items[i];
                    if (item["name"].ToString() == "game_minecraft")
                    {
                        haveJE = true;
                    }
                    else if (item["name"].ToString() == "game_minecraft_bedrock")
                    {
                        haveBE = true;
                    }
                }
                Console.WriteLine("登录完成！可继续启动！");
                return (haveJE, haveBE);
            }
            else
            {
                Console.WriteLine("该账户未购买 Minecraft 。");
                throw new Exception();
            }
        }

        /// <summary>
        /// 取得正版用户信息
        /// </summary>
        /// <param name="token">Minecraft 访问令牌</param>
        /// <returns>用户信息</returns>
        /// <exception cref="Exception"></exception>
        public static async Task<(string mcid, string mcname)> GetProfile(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.minecraftservices.com/minecraft/profile");
            request.Headers.Add("Authorization", $"Bearer {token}");

            var response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var json = JsonMapper.ToObject(responseBody);
                if (json.Keys.Contains("path")) return (null, null);
                return (json["id"].ToString(), json["name"].ToString());
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
