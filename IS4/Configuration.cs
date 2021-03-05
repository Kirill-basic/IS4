using Constants;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IS4
{
    public static class Configuration
    {
        public static IEnumerable<ApiResource> GetApiResources() =>
            new List<ApiResource>
            {
                new ApiResource(Scopes.ApiOneScope) {Scopes= { Scopes.ApiOneScope } },
                new ApiResource(Scopes.ApiTwoScope) {Scopes= { Scopes.ApiTwoScope } },
                new ApiResource(Scopes.ApiThreeScope) {Scopes= { Scopes.ApiThreeScope } }
            };

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

                RedirectUris = {"https://localhost:6001/signin-oidc"},

                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes =
                {
                    Scopes.ApiOneScope,
                    Scopes.ApiTwoScope,
                    Scopes.ApiThreeScope,
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                },
                RequireConsent=false,
                AlwaysIncludeUserClaimsInIdToken=true,
                AlwaysSendClientClaims=true
            },
            new Client
            {
                ClientId = Clients.Wpf,

                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                RequireClientSecret = false,

                RedirectUris = { "http://localhost/sample-wpf-app" },

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    Scopes.ApiOneScope,
                },
                AllowAccessTokensViaBrowser = true,
                RequireConsent = false,
            }
        };
    }
}
