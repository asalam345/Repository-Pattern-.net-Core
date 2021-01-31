using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Entities
{
    public class RepositoryContext
    {
		public static string GetConnectionString()
		{
			var bilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
			return bilder.Build().GetSection("ConnectionStrings").GetSection("ChatDatabase").Value;
		}
	}
}
