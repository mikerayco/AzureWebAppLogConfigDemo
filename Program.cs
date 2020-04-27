using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.Azure.Management.WebSites;
using Microsoft.Azure.Management.WebSites.Models;
using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Common;
using Microsoft.Rest;
using Microsoft.Rest.Azure;
using Microsoft.Rest.Azure.Authentication;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace AzureWebAppLogConfigDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            runAsync().Wait();
        }

        private static async Task runAsync()
        {
            var resourceGroupName = "";
            var webSiteName = "";

            var clientId = "";
            var clientSecret = "";
            var subscriptionId = "";
            var tenantId = "";
            var sasUrl = "";

            var serviceCredentials =
                await ApplicationTokenProvider.LoginSilentAsync(
                    tenantId, clientId, clientSecret);
            var client = new WebSiteManagementClient(
                serviceCredentials);
            client.SubscriptionId = subscriptionId;

            var appSettings = new StringDictionary(
                name: "properties",
                properties: new Dictionary<string, string> {
                    { "DIAGNOSTICS_AZUREBLOBCONTAINERSASURL", sasUrl },
                    { "DIAGNOSTICS_AZUREBLOBRETENTIONINDAYS", "30" },
                }
            );
            client.WebApps.UpdateApplicationSettings(
                resourceGroupName: resourceGroupName,
                name: webSiteName,
                appSettings: appSettings
            );

            Console.WriteLine("done");
        }
    }
}
