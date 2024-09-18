// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Azure.Identity;
using GraphNotifications.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
//using System.Threading;

namespace GraphNotifications.Services
{
    public class GraphClientService : IGraphClientService
    {
        private readonly AppSettings _settings;
        private readonly ILogger _logger;

        public GraphClientService(IOptions<AppSettings> options, ILogger<GraphClientService> logger)
        {
            _settings = options.Value;
            _logger = logger;
        }

        public GraphServiceClient GetUserGraphClient(string userAssertion)
        {
            var tenantId = _settings.TenantId;
            var clientId = _settings.ClientId;
            var clientSecret = _settings.ClientSecret;
            var authorityUrl = string.IsNullOrEmpty(_settings.AuthUrl) 
                    ? new Uri($"{AzureAuthorityHosts.AzurePublicCloud.ToString().TrimEnd('/')}/{tenantId}")
                    : new Uri($"{_settings.AuthUrl.TrimEnd('/')}/{tenantId}");
            var graphUrl = string.IsNullOrEmpty(_settings.GraphUrl) 
                    ? "https://graph.microsoft.com" 
                    : _settings.GraphUrl;

            if (string.IsNullOrEmpty(tenantId) ||
                string.IsNullOrEmpty(clientId) ||
                string.IsNullOrEmpty(clientSecret))
            {
                _logger.LogError("Required settings missing: 'tenantId', 'apiClientId', and 'apiClientSecret'.");
                throw new ArgumentNullException("Required settings missing: 'tenantId', 'apiClientId', and 'apiClientSecret'.");
            }
            
            var onBehlfofCredentialOptions = new OnBehalfOfCredentialOptions
                { AuthorityHost = authorityUrl };

            var onBehalfOfCredential = new OnBehalfOfCredential(
                tenantId, clientId, clientSecret, userAssertion, onBehlfofCredentialOptions);                      

            IEnumerable<string> scopes = new List<string> { $"{graphUrl.TrimEnd('/')}/.default" };  // Replace with your custom scope

            _logger.LogInformation("Returning GraphServiceClient");
            var graphClient= new GraphServiceClient(onBehalfOfCredential, scopes); 
            graphClient.BaseUrl = $"{graphUrl.TrimEnd('/')}/v1.0";
            return graphClient;
        }
    }
}
