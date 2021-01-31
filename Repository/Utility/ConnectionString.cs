using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration.Json;
using System.IO;

namespace ChatServerApi.Utility
{
	public static class MySettings
	{
		public static string GetConnectionString()
		{
			var bilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
			return bilder.Build().GetSection("ConnectionStrings").GetSection("ChatDatabase").Value;
		}
		public static string GetSecretKey()
		{
			var bilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
			return bilder.Build().GetSection("AppSettings").GetSection("Secret").Value;
		}

	}
}
