using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Core.Identity
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResourceResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(), //必须要添加，否则报无效的scope错误
            };
        }

        // scopes define the API resources in your system
        public static IEnumerable<ApiResource> GetApiResources()
        {
            //可访问的API资源(资源名，资源描述)
            return new List<ApiResource>
            {
                new ApiResource("SampleServices", "SampleServices")
                //可以多个不同的
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "SampleServices", //访问客户端Id,必须唯一
                    //使用客户端授权模式，客户端只需要clientid和secrets就可以访问对应的api资源。
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("witcit.com".Sha256())
                    },
                    AllowedScopes = { "SampleServices", IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Profile }
                }//可以多个不同的，与ApiResource个数一致
            };
        }
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId="1",
                    Username="爱丽丝",
                    Password="password"
                },
                new TestUser
                {
                    SubjectId="2",
                    Username="博德",
                    Password="password"
                }
            };
        }
    }
}
