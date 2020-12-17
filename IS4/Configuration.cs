using Constants;
using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace IS4
{
    public static class Configuration
    {
        public static IEnumerable<ApiScope> GetApiScopes() => new List<ApiScope>
        {
            new ApiScope(Scopes.ApiOneScope),
            new ApiScope(Scopes.ApiTwoScope),
            new ApiScope(Scopes.ApiThreeScope),
        };

        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiResource> GetApiResources() =>
            new List<ApiResource>
            {
                new ApiResource(Scopes.ApiOneScope) {Scopes= { Scopes.ApiOneScope }, UserClaims={ ClaimTypes.Role, ClaimTypes.Gender} },
                new ApiResource(Scopes.ApiTwoScope) {Scopes= { Scopes.ApiTwoScope } },
                new ApiResource(Scopes.ApiThreeScope) {Scopes= { Scopes.ApiThreeScope } }
            };

        public static IEnumerable<Client> GetClients() => new List<Client>
        {
            new Client
            {
                ClientId=Clients.ApiOne,
                ClientSecrets={new Secret(Secrets.ApiOneSecret.ToSha256())},

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = {Scopes.ApiOneScope}
            },
            new Client
            {
                ClientId=Clients.Mvc,
                ClientSecrets={new Secret(Secrets.MvcSecret.ToSha256())},

                RedirectUris = {"https://localhost:44368/signin-oidc"},

                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes =
                {
                    Scopes.ApiOneScope,
                    Scopes.ApiTwoScope,
                    Scopes.ApiThreeScope,
                    IdentityServer4.IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServer4.IdentityServerConstants.StandardScopes.Profile,
                },
                RequireConsent=false,
                AlwaysIncludeUserClaimsInIdToken=false
            }
        };
    }
}
