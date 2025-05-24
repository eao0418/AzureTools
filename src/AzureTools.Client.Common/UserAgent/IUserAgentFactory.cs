// IUserAgentFactory.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Client.Common.UserAgent
{
	/// <summary>
	/// An interface for a factory that returns a user agent string for a given device.
	/// </summary>
	public interface IUserAgentFactory
	{
		/// <summary>
		/// Returns a user agent string for a given device and client combination.
		/// </summary>
		/// <param name="device">The device being 'used'.</param>
		/// <param name="clientType">The client being 'used'.</param>
		/// <returns>A user agent string for the device and client combination.</returns>
		string GetValue(string device, string clientType = "Default");
	}
}