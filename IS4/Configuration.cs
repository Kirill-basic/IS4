using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace IS4
{
    public static class Configuration
    {
        public static IEnumerable<ApiScope> GetApiScopes() => new List<ApiScope> { new ApiScope(name: "ApiOne", displayName: "ApiOne") };
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = "rc.scope",
                    UserClaims =
                    {
                        ClaimTypes.Role,
                        ClaimTypes.Email,
                        ClaimTypes.MobilePhone
                    }
                }
            };
        public static IEnumerable<ApiResource> GetApiResources() =>
            new List<ApiResource>
            {
                //new ApiResource("ApiOne"),
                new ApiResource("ApiTwo"),
                new ApiResource("ApiOne"),
                
                //adding claims to access token
                //new ApiResource("ApiOne", "ApiOne", new string[] { ClaimTypes.Role, ClaimTypes.Email, ClaimTypes.MobilePhone })
                //{
                //    Scopes = new string[]
                //    {
                //        "rc.scope"
                //    }
                //}
            };

        public static IEnumerable<Client> GetClients() => new List<Client>
        {
            new Client
            {
                ClientId="client_id",
                ClientSecrets={new Secret("client_secret".ToSha256())},

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = {"ApiOne"}
            },
            new Client
            {
                ClientId="client_id_mvc",
                ClientSecrets={new Secret("client_secret_mvc".ToSha256())},

                RedirectUris = {"https://localhost:44368/signin-oidc"},

                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes =
                {
                    "ApiOne",
                    "ApiTwo",
                    IdentityServer4.IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServer4.IdentityServerConstants.StandardScopes.Profile,
                    "rc.scope"
                },
                RequireConsent=false,
                
                //puts all the claims in the id token
                //AlwaysIncludeUserClaimsInIdToken=true
            },
            new Client
            {
                ClientId="custom_id",
                ClientSecrets={new Secret("custom_secret".ToSha256())},

                RedirectUris = {"https://localhost:44379/signin-oidc"},

                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes =
                {
                    "ApiOne",
                    "ApiTwo",
                    IdentityServer4.IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServer4.IdentityServerConstants.StandardScopes.Profile,
                    "rc.scope"
                },
                RequireConsent=false,
            }
        };
    }
}
