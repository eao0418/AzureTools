// WindowsUserAgent.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Client.Common.UserAgent
{
	/// <summary>
	/// Convenience class for Windows user agents.
	/// </summary>
	public class WindowsUserAgent : IUserAgent
	{
		internal const string InternetExplorer = "Mozilla/5.0 (Windows NT 10.0; Trident/7.0; rv:11.0) like Gecko";
		internal const string Chrome = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.114 Safari/537.36";
		internal const string Firefox = "Mozilla/5.0 (Windows NT 10.0; rv:89.0) Gecko/20100101 Firefox/89.0";
		internal const string Edge = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; ServiceUI 14) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.114 Safari/537.36 Edg/91.0.864.59";
		internal const string Opera = "Opera/9.80 (Windows NT 10.0; Win64; x64) Presto/2.12.388 Version/12.18";
		internal const string Brave = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Brave Chrome/91.0.4472.114 Safari/537.36";
		internal const string Vivaldi = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Vivaldi/4.0.2312.33 Chrome/91.0.4472.114 Safari/537.36";
		internal const string EdgeChromium = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.85 Mobile Safari/537.36 EdgA/90.0.818.46";

		public static string GetDefault()
		{
			return EdgeChromium;
		}
	}
}
