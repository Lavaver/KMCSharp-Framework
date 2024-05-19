using KMCSharp.Modules.Yggdrasil;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace KMCSharp.Auth
{

    public class MSAuth : IAuthenticator
    {
        string loginUrl;
        public string Type
        {
            get { return "KMCSharp.Microsoft"; }
        }

        public AuthenticationInfo Do()
        {
            Uri uri = new Uri(loginUrl);
            string query = uri.Query;
            var queryParams = HttpUtility.ParseQueryString(query);
            string codeValue = queryParams.Get("code");

            if (!string.IsNullOrEmpty(codeValue))
            {
                var result = Task.Run(async () =>
                {
                    (string xblToken, string uhs) = await MSYggdrasilClient.Auth_XBL(codeValue);
                    (string xstsToken, string uhsFromStep1) = await MSYggdrasilClient.Auth_XSTS(xblToken);
                    string minecraftToken = await MSYggdrasilClient.GetMinecraftToken(xstsToken, uhsFromStep1);
                    (bool haveJE, bool haveBE) = await MSYggdrasilClient.CheckLicense(minecraftToken);

                    if (haveJE || haveBE)
                    {
                        (string UsrUUID, string Usrname) = await MSYggdrasilClient.GetProfile(minecraftToken);
                        Guid UsrGUUID = Guid.Parse(UsrUUID);

                        return new AuthenticationInfo
                        {
                            DisplayName = Usrname,
                            UUID = UsrGUUID,
                            AccessToken = minecraftToken,
                            Properties = "{}",
                            UserType = "Microsoft"
                        };
                    }
                    else
                    {
                        // Handle the case where the user doesn't have the required licenses
                        // ...
                        return new AuthenticationInfo
                        {
                            Error = "User does not have required licenses"
                        };
                    }
                }).Result;

                return result;
            }
            else
            {
                return new AuthenticationInfo
                {
                    Error = "Microsoft login failed"
                };
            }
        }

        public Task<AuthenticationInfo> DoAsync(CancellationToken token)
        {
            return Task.Factory.StartNew((Func<AuthenticationInfo>)Do, token);
        }

    }
}
