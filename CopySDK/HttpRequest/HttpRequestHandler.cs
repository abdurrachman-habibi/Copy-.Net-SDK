﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using CopySDK.Models;

namespace CopySDK.HttpRequest
{
    public class HttpRequestHandler
    {
        private readonly HttpClient _client;
        private readonly HttpClientHandler _handler;

        public HttpRequestHandler()
        {
            _handler = new HttpClientHandler();
            _client = new HttpClient(_handler);
        }

        public async Task<string> ReadAsStringAsync(HttpRequestItem httpRequestItem)
        {
            HttpResponseMessage httpResponse = await ExecuteAsync(httpRequestItem);

            return await httpResponse.Content.ReadAsStringAsync();

        }

        public async Task<byte[]> ReadAsByteArrayAsync(HttpRequestItem httpRequestItem)
        {
            HttpResponseMessage httpResponse = await ExecuteAsync(httpRequestItem);

            return await httpResponse.Content.ReadAsByteArrayAsync();
        }

        public async Task<HttpResponseMessage> ExecuteAsync(HttpRequestItem httpRequestItem)
        {
            HttpRequestMessage httpRequest = new HttpRequestMessage()
                {
                    RequestUri = new Uri(httpRequestItem.URL),
                    Method = httpRequestItem.HttpMethod,
                    Content = httpRequestItem.HttpContent
                };
            
            httpRequest.Headers.Add("Authorization", httpRequestItem.AuthzHeader);

            if (httpRequestItem.IsDataRequest)
            {
                httpRequest.Headers.Add("X-Api-Version", "1");
                httpRequest.Headers.Add("Accept", "application/json");
            }

            //if (httpRequestItem.IsFileUpload)
            //{
            //    httpRequest.Headers.TryAddWithoutValidation("Content-Type", "multipart/form-data");
            //}

            HttpResponseMessage httpResponse =
                await _client.SendAsync(httpRequest, HttpCompletionOption.ResponseContentRead);

            return httpResponse;
        }
    }
}
