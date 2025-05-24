// IPhoneUserAgent.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Client.Common.UserAgent
{
	/// <summary>
	/// Convenience class for storing iPhone user agent strings.
	/// </summary>
	public class IPhoneUserAgent : IUserAgent
	{
		internal const string Safari = "Mozilla/5.0 (iPhone; CPU iPhone OS 14_6 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.1.1 Mobile/15E148 Safari/604.1";
		internal const string Chrome = "Mozilla/5.0 (iPhone; CPU iPhone OS 14_6 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/91.0.4472.114 Mobile/15E148 Safari/604.1";
		internal const string Firefox = "Mozilla/5.0 (iPhone; CPU iPhone OS 14_6 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) FxiOS/37.0 Mobile/15E148 Safari/605.1.15";
		internal const string Edge = "Mozilla/5.0 (iPhone; CPU iPhone OS 14_6 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) EdgiOS/46.1.2 Mobile/15E148 Safari/605.1.15";
		internal const string Opera = "Opera/9.80 (iPhone; Opera Mini/56.0.2254/172.31; U; en) Presto/2.12.423 Version/12.16";
		internal const string Brave = "Mozilla/5.0 (iPhone; CPU iPhone OS 14_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.0 Mobile/15E148 Safari/604.1";

		public static string GetDefault()
		{
			return Safari;
		}
	}
}
