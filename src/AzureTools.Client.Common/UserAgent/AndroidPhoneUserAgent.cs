// AndroidPhoneUserAgent.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Client.Common.UserAgent
{
	/// <summary>
	/// Convenience class for Android phone user agents.
	/// </summary>
	public class AndroidPhoneUserAgent : IUserAgent
	{
		internal const string Android = "Mozilla/5.0 (Linux; Android 13; SM-S908B) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 Mobile Safari/537.36";
		internal const string Chrome = "Mozilla/5.0 (Linux; Android 10; K) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.6099.43 Mobile Safari/537.36";
		internal const string Firefox = "Mozilla/5.0 (Android 10; Mobile; rv:89.0) Gecko/89.0 Firefox/89.0";
		internal const string Edge = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4501.0 Safari/537.36 Edg/91.0.866.0";
		internal const string Opera = "Opera/9.80 (Android 10; Opera Mini/56.0.2254/172.31; U; en) Presto/2.12.423 Version/12.16";
		internal const string Brave = "Mozilla/5.0 (Linux; Android 10; SM-S908B) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.120 Mobile Safari/537.36 Brave/91.0.4472.120";

		public static string GetDefault()
		{
			return Android;
		}
	}
}
