using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "My API")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new Client[]
            {
                 new Client
                 {
                    ClientId = "Client1",
                    ClientName = "凭证式",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("123456".Sha256())
                    },
                    AllowedScopes = { "api1" }
                 },
                 new Client
                 {
                     ClientId = "Client2",
                     ClientName = "密码式",
                     AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                     ClientSecrets =
                     {
                         new Secret("123456".Sha256())
                     },
                     AllowedScopes = { "api1" }
                 },
                 new Client
                 {
                     ClientId = "Client3",
                     ClientName = "OIDC隐式",
                     AllowedGrantTypes = GrantTypes.Implicit,
                     RequireConsent = true,
                     // where to redirect to after login
                     RedirectUris = { "http://localhost:7002/signin-oidc" },

                     // where to redirect to after logout
                     PostLogoutRedirectUris = { "http://localhost:7002/signout-callback-oidc" },

                     AllowedScopes = new List<string>
                     {
                         IdentityServerConstants.StandardScopes.OpenId,
                         IdentityServerConstants.StandardScopes.Profile
                     }
                 },
                 new Client
                 {
                    ClientId = "Client4",
                    ClientName = "混合模式",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RequireConsent = true,
                    ClientSecrets =
                    {
                        new Secret("123456".Sha256())
                    },
                    RedirectUris           = { "http://localhost:7003/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:7003/signout-callback-oidc" },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    },
                    AllowOfflineAccess = true
                 },
                 new Client
                 {
                    ClientId = "Client5",
                    ClientName = "JavaScript Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    RedirectUris =           { "http://localhost:7004/callback.html" },
                    PostLogoutRedirectUris = { "http://localhost:7004/index.html" },
                    AllowedCorsOrigins =     { "http://localhost:7004" },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    }
                 }
            };
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1001",
                    Username = "edward",
                    Password = "qweewq",
                    Claims = new []
                    {
                        new Claim("name", "杨锐泽"),
                        new Claim("phone", "15001759330")
                    }
                }
            };
        }
    }
}
