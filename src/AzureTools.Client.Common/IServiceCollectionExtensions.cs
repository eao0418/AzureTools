// IServiceCollectionExtensions.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Client.Common
{
	using AzureTools.Client.Common.UserAgent;
	using Microsoft.Extensions.DependencyInjection;

	internal static class IServiceCollectionExtensions
	{
		internal static IServiceCollection AddUserAgentFactory(this IServiceCollection services)
		{
			services.AddSingleton<IUserAgentFactory, UserAgentFactory>(provider =>
			{
				var iPhoneUserAgents = new Dictionary<string, string>()
				{
					{ ClientTypes.Safari, IPhoneUserAgent.Safari },
					{ ClientTypes.Chrome, IPhoneUserAgent.Chrome },
					{ ClientTypes.Firefox, IPhoneUserAgent.Firefox },
					{ ClientTypes.Edge, IPhoneUserAgent.Edge },
					{ ClientTypes.Opera, IPhoneUserAgent.Opera },
					{ ClientTypes.Default, IPhoneUserAgent.GetDefault() },
				};

				var androidUserAgents = new Dictionary<string, string>()
				{
					{ ClientTypes.Android, AndroidPhoneUserAgent.Android },
					{ ClientTypes.Chrome, AndroidPhoneUserAgent.Chrome },
					{ ClientTypes.Firefox, AndroidPhoneUserAgent.Firefox },
					{ ClientTypes.Edge, AndroidPhoneUserAgent.Edge },
					{ ClientTypes.Opera, AndroidPhoneUserAgent.Opera },
					{ ClientTypes.Default, AndroidPhoneUserAgent.GetDefault() },
				};

				var windowsDesktopUserAgents = new Dictionary<string, string>()
				{
					{ ClientTypes.Edge, WindowsUserAgent.Edge },
					{ ClientTypes.InternetExplorer, WindowsUserAgent.InternetExplorer },
					{ ClientTypes.Brave, WindowsUserAgent.Brave },
					{ ClientTypes.Chrome, WindowsUserAgent.Chrome },
					{ ClientTypes.Firefox, WindowsUserAgent.Firefox },
					{ ClientTypes.Opera, WindowsUserAgent.Opera },
					{ ClientTypes.Default, WindowsUserAgent.GetDefault() },
					{ ClientTypes.EdgeChromium, WindowsUserAgent.EdgeChromium },
					{ ClientTypes.Vivaldi, WindowsUserAgent.Vivaldi },
				};

				var macUserAgents = new Dictionary<string, string>()
				{
					{ ClientTypes.Safari, MacUserAgent.Safari },
					{ ClientTypes.Chrome, MacUserAgent.Chrome },
					{ ClientTypes.Firefox, MacUserAgent.Firefox },
					{ ClientTypes.Opera, MacUserAgent.Opera },
					{ ClientTypes.Default, MacUserAgent.GetDefault() },
					{ ClientTypes.Brave, MacUserAgent.Brave },
					{ ClientTypes.Edge, MacUserAgent.Edge },
					{ ClientTypes.Vivaldi, MacUserAgent.Vivaldi },
				};

				var userAgents = new Dictionary<string, IReadOnlyDictionary<string, string>>()
				{
					{ DeviceTypes.IPhone, iPhoneUserAgents },
					{ DeviceTypes.Android, androidUserAgents },
					{ DeviceTypes.WindowsDesktop, windowsDesktopUserAgents },
					{ DeviceTypes.Mac, macUserAgents },
				};

				return new UserAgentFactory(userAgents);
			});

			return services;
		}
	}
}
