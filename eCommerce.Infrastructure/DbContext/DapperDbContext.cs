using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace eCommerce.Infrastructure.DbContext;

public class DapperDbContext
{
    private readonly IConfiguration _configuration;
    private readonly IDbConnection _connection;

    public DapperDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
        
        var host = Environment.GetEnvironmentVariable("POSTGRES_HOST");
        var user = Environment.GetEnvironmentVariable("POSTGRES_USER");
        var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
        var dbName =  Environment.GetEnvironmentVariable("POSTGRES_DATABASE_NAME");
        
        string? connectionString;
        
        Console.WriteLine($"HOST: {host} -- USER: {user} -- PASS: {password}  -- DB NAME: {dbName}");

        if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(dbName))
        {
            connectionString = _configuration.GetConnectionString("PostgresConnectionValuesHardcoded");
        }
        else
        {
            connectionString = _configuration.GetConnectionString("PostgresConnectionValuesFromEnvVariables")
                ?.Replace("$POSTGRES_HOST", host)
                .Replace("$POSTGRES_USER", user)
                .Replace("$POSTGRES_PASSWORD", password)
                .Replace("$POSTGRES_DATABASE_NAME", dbName);;
        }

        //Create a new NpgsqlConnection with the retrieved connection string
        _connection = new NpgsqlConnection(connectionString);
    }


    public IDbConnection DbConnection => _connection;
}