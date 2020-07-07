using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Core.Identity.Models;
using IdentityModel;
using IdentityModel.Client;
using IdentityServer4.Models;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;

namespace Core.Identity.Controllers
{
    public class IdentityController : BaseController
    {
        //开放ID配置可查看：https://localhost:44354/.well-known/openid-configuration
        public IdentityController(ILogger<IdentityController> logger) : base(logger) { }

        /// <summary>
        /// 获取Token
        /// https://localhost:44354/Identity/GetToken
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, HttpGet]
        public string GetToken([FromForm]ClientModel model)
        {
            var httpClient = new HttpClient();
            var disco = httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = $"{ Request.Scheme }://{Request.Host}"
            }).Result;

            if (disco.IsError)
            {
                throw new Exception(disco.Error);
            }
            var tokenResponse = httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = model.ClientId,
                ClientSecret = model.ClientSecret,
                Scope = model.Scope
            });
            var json = tokenResponse.Result.Json;
            return json.ToString();
        }
    }
}