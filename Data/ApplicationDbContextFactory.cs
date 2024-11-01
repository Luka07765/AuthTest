using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace AuthLearning.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Define the exact base path to where appsettings.json is located
            var basePath = @"C:\Users\lukat\source\repos\AuthLearning";

            Console.WriteLine($"Base path: {basePath}");

            // Check if the file exists at the expected path
            var configFilePath = Path.Combine(basePath, "appsettings.json");
            Console.WriteLine($"Looking for configuration file at: {configFilePath}");

            if (!File.Exists(configFilePath))
            {
                Console.WriteLine($"Configuration file not found at: {configFilePath}");
                throw new FileNotFoundException($"Configuration file not found at: {configFilePath}");
            }

            // Build the configuration
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(basePath) // Set the base path to the correct directory
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Retrieve the connection string
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found in appsettings.json.");
            }

            // Set up DbContext options
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseMySql(
                connectionString,
                new MySqlServerVersion(new Version(8, 0, 23)) // Adjust MySQL version if needed
            );

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
