﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopySDK;
using CopySDK.Authentication;
using CopySDK.Helper;
using CopySDK.Models;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            //string generateAuthzHeader = oAuth.GenerateAuthzHeader(URL.RequestToken, "GET");
            //string forRequest = AuthorizationHeader.CreateForRequest("oob", "cIAKv1kFCwXn2izGsMl8vZmfpfBcJSv1", "vNY1oLTr2WieLYxgCA6tDgdfCS1zTRA2IMzhmQLoQOS7nmIK", URL.RequestToken);

            Scope scope = new Scope()
            {
                Profile = new ProfilePermission()
                {
                    Read = true,
                    Write = true,
                    Email = new EmailPermission()
                    {
                        Read = true
                    }
                }
            };

            CopyConfig copyConfig = new CopyConfig("http://copysdk", "cIAKv1kFCwXn2izGsMl8vZmfpfBcJSv1", "vNY1oLTr2WieLYxgCA6tDgdfCS1zTRA2IMzhmQLoQOS7nmIK", scope);

            Task<AuthToken> requestToken = copyConfig.GetRequestToken();

            Task.WhenAll(requestToken);

            AuthToken authToken = requestToken.Result;

            var url = string.Format("{0}?oauth_token={1}", URL.Authorize, authToken.Token);

            System.Diagnostics.Process.Start(url);

            string verifier = "";

            CopyClient copyClient = new CopyClient(copyConfig.Config, authToken);

            Task<AuthToken> accessToken = copyClient.GetAccessToken(verifier);

            Task.WhenAll(requestToken);

            AuthToken result = accessToken.Result;




            //OAuth oAuth = new OAuth();
            //oAuth["callback"] = "http://copysdk";
            //oAuth["consumer_key"] = "cIAKv1kFCwXn2izGsMl8vZmfpfBcJSv1";
            //oAuth["consumer_secret"] = "vNY1oLTr2WieLYxgCA6tDgdfCS1zTRA2IMzhmQLoQOS7nmIK";

            //OAuthSession oAuthSession = oAuth.AcquireRequestToken(URL.RequestToken, "GET");

            //var url = string.Format("{0}?oauth_token={1}", URL.Authorize, oAuthSession.Token);

            //System.Diagnostics.Process.Start(url);

            //string verifier = "";

            //oAuthSession = oAuth.AcquireAccessToken(URL.AccessToken, "GET", verifier);





            //string token = "Nyfr5GWBCX5gaUKElefZcdJSNobTC8Ln";
            //string tokenSecret = "Jfbaqv2SgaX3HvNZ8QXt1BdHV7Z00KxlP745b2JSxpjGlQ4I";
            //string verifier = "291708487082923003087fc9ed34a7c2";
            //string forRequest = AuthorizationHeader.CreateForAccess("cIAKv1kFCwXn2izGsMl8vZmfpfBcJSv1", "vNY1oLTr2WieLYxgCA6tDgdfCS1zTRA2IMzhmQLoQOS7nmIK", token, tokenSecret, verifier);

        }
    }
}
