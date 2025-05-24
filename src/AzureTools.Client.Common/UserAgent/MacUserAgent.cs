// MacUserAgent.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Client.Common.UserAgent
{
	/// <summary>
	/// Convenience class for Mac user agents.
	/// </summary>
	public class MacUserAgent : IUserAgent
	{
		internal const string Chrome = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.114 Safari/537.36";
		internal const string Firefox = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.14; rv:89.0) Gecko/20100101 Firefox/89.0";
		internal const string Safari = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.1.1 Safari/605.1.15";
		internal const string Edge = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.114 Safari/537.36 Edg/91.0.864.59";
		internal const string Opera = "Opera/9.80 (Macintosh; Intel Mac OS X 10.14.6) Presto/2.12.388 Version/12.18";
		internal const string Brave = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Brave Chrome/91.0.4472.114 Safari/537.36";
		internal const string Vivaldi = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Vivaldi/4.0.2312.33 Chrome/91.0.4472.114 Safari/537.36";

		public static string GetDefault()
		{
			return Safari;
		}
	}
}
