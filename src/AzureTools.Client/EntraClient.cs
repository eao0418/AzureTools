// EntraClient.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Client
{
    using AzureTools.Authentication.Settings;
    using AzureTools.Client;
    using Microsoft.Graph.Models;
    using Microsoft.Graph;
    using System;
    using System.Threading.Tasks;

    public class EntraClient
	{
		private readonly IGraphServiceClientFactory _graphServiceClientFactory;

        public EntraClient(IGraphServiceClientFactory graphServiceClientFactory)
		{
            _graphServiceClientFactory = graphServiceClientFactory ?? throw new ArgumentNullException(nameof(graphServiceClientFactory);
        }

		public async Task GetEntraUsers(AuthenticationSettings settings)
		{
            var client = _graphServiceClientFactory.GetClient(settings.GetAuthKey());

            // TO-DO: Implement custom iteration here instead of using the PageIterator. 
            var users = await client.Users
                .GetAsync(config =>
                {
                    config.QueryParameters.Top = 999;
                });

            if (users == null)
            {
                return;
            }

            var pageIterator = PageIterator<User, UserCollectionResponse>
            .CreatePageIterator(
                    client,
                    users,
                    // Callback executed for each item in
                    // the collection
                    (u) =>
                    {
                        Console.WriteLine(u.UserPrincipalName);
                        return true;
                    });

            await pageIterator.IterateAsync();
        }


    }
}
