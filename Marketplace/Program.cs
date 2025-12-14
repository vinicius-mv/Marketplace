using Marketplace;
using System.Reflection;

public static class Program
{
    public static string CurrentDirectory { get; }

    static Program() =>
            CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

    private static void Main(string[] args)
    {
        var builder = CreateHostBuilder(args);
        var app = builder.Build();
        app.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        var configuration = BuildConfiguration(args);

        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                webBuilder.UseConfiguration(configuration);
                webBuilder.UseContentRoot(CurrentDirectory);
                webBuilder.UseKestrel();
            });
    }

    private static IConfiguration BuildConfiguration(string[] args)
            => new ConfigurationBuilder()
                .SetBasePath(CurrentDirectory)
                .Build();
}