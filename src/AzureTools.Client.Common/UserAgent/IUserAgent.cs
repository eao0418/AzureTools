// IUserAgent.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Client.Common.UserAgent
{
	/// <summary>
	/// Interface for user agent strings.
	/// </summary>
	public interface IUserAgent
	{
		/// <summary>
		///  Gets the default user agent string for the device.
		/// </summary>
		/// <returns>A <see cref="string"/> user agent.</returns>
		public abstract static string GetDefault();
	}
}
