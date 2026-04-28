using System.Data;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace FSI.SmartPark.Infrastructure.Data;

public class SmartParkDbContext
{
    private readonly string _connectionString;

    public SmartParkDbContext(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("SmartPark")
            ?? throw new InvalidOperationException("ConnectionString 'SmartPark' não configurada.");
    }

    public IDbConnection CreateConnection() => new MySqlConnection(_connectionString);
}
