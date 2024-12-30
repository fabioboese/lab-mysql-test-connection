using MySql.Data.MySqlClient;

namespace test_mysql_connection;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var interval = int.Parse(Environment.GetEnvironmentVariable("INTERVAL") ?? "5");
        var connectionString = Environment.GetEnvironmentVariable("MYSQLCONNSTR");
        var mysql = Environment.GetEnvironmentVariable("MYSQLCMD");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var client = new MySqlConnection(connectionString);

                var cmd = new MySqlCommand(mysql, client);
                client.Open();
                var reader = cmd.ExecuteReader();
                Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.fff")} Connection and Command executed successfully!");
                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.fff")} An error occurred: {ex.Message}");
            }

            await Task.Delay(1000 * interval, stoppingToken);
        }
    }
}
