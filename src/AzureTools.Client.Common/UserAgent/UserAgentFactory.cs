// UserAgentFactory.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Client.Common.UserAgent
{
	using System;
	using System.Collections.Generic;

	public class UserAgentFactory : IUserAgentFactory
	{
		private readonly IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> _userAgents;

		/// <summary>
		/// Initializes a new instance of the <see cref="UserAgentFactory"/> class.
		/// </summary>
		/// <param name="userAgents">A dictionary initialized with the values to use.</param>
		/// <exception cref="ArgumentNullException">Throws if <paramref name="userAgents"/> is null.</exception>
		/// <exception cref="ArgumentException">Throws if <paramref name="userAgents"/> is initialized as an empty collection.</exception>
		public UserAgentFactory(IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> userAgents)
		{
			this._userAgents = userAgents ?? throw new ArgumentNullException(nameof(userAgents));
			if (this._userAgents.Count == 0)
			{
				throw new ArgumentException("Value cannot be an empty collection. Initialize the collection on startup.", nameof(userAgents));
			}
		}

		/// inheritedDoc
		public string GetValue(string device, string clientType = ClientTypes.Default)
		{
			if (string.IsNullOrWhiteSpace(device))
			{
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(device));
			}

			if (this._userAgents.TryGetValue(device, out var deviceUserAgents))
			{
				if (deviceUserAgents.TryGetValue(clientType, out var userAgentValue))
				{
					return userAgentValue;
				}
				else
				{
					throw new ArgumentException($"Client '{clientType}' is not supported for device '{device}'.", nameof(clientType));
				}
			}
			else
			{
				throw new ArgumentException($"Device '{device}' is not supported.", nameof(device));
			}
		}

	}
}
