﻿using System.Net.Http;
using CopySDK.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CopySDK.HttpRequest;
using CopySDK.Managers;
using CopySDK.Models;
using Newtonsoft.Json;

namespace CopySDK
{
    public class CopyClient
    {
        public Config Config { get; set; }
        public AuthToken AuthToken { get; set; }

        public UserManager UserManager { get; set; }

        public CopyClient(Config config, AuthToken authToken)
        {
            Config = config;
            AuthToken = authToken;            
        }

        public async Task<AuthToken> GetAccessToken(string verifier)
        {
            string authzHeader = AuthorizationHeader.CreateForAccess(Config.ConsumerKey, Config.ConsumerSecret, AuthToken.Token, AuthToken.TokenSecret, verifier);

            string url = string.Format(URL.AccessToken);

            HttpRequestItem httpRequestItem = new HttpRequestItem()
            {
                URL = URL.AccessToken,
                HttpMethod = HttpMethod.Get,
                AuthzHeader = authzHeader,
                HttpContent = null,
                IsDataRequest = false
            };

            HttpRequestHandler httpRequestHandler = new HttpRequestHandler();
            string executeAsync = await httpRequestHandler.ExecuteAsync(httpRequestItem);

            string[] kvpairs = executeAsync.Split('&');
            Dictionary<string, string> parameters = kvpairs.Select(pair => pair.Split('='))
                                                        .ToDictionary(kv => kv[0], kv => kv[1]);

            if (parameters.ContainsKey("oauth_error_message"))
            {
                return null;
            }
            else
            {
                return new AuthToken()
                {
                    Token = parameters["oauth_token"],
                    TokenSecret = parameters["oauth_token_secret"]
                };
            }
        }
    }
}
