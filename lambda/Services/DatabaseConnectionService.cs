using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EventSignup.Services
{
    public class DatabaseConnectionService : IDatabaseConnectionService
    {
        private readonly ILogger<DatabaseConnectionService> _logger;
        private string? _connectionString;

        public DatabaseConnectionService(ILogger<DatabaseConnectionService> logger)
        {
            _logger = logger;
        }

        public string GetConnectionString()
        {
            if (_connectionString != null)
                return _connectionString;

            try
            {
                var secretArn = Environment.GetEnvironmentVariable("DB_SECRET_ARN");
                var dbEndpoint = Environment.GetEnvironmentVariable("DB_ENDPOINT");
                var dbName = Environment.GetEnvironmentVariable("DB_NAME");

                if (string.IsNullOrEmpty(secretArn) || string.IsNullOrEmpty(dbEndpoint) || string.IsNullOrEmpty(dbName))
                {
                    throw new InvalidOperationException("Database environment variables are not configured.");
                }

                using var secretsClient = new AmazonSecretsManagerClient();
                var secretRequest = new GetSecretValueRequest { SecretId = secretArn };
                var secretResponse = secretsClient.GetSecretValueAsync(secretRequest).GetAwaiter().GetResult();
                
                if (string.IsNullOrEmpty(secretResponse.SecretString))
                {
                    throw new InvalidOperationException("Secret value is empty or null");
                }

                var secret = JsonSerializer.Deserialize<DatabaseSecret>(secretResponse.SecretString);
                
                if (secret == null || string.IsNullOrEmpty(secret.Username) || string.IsNullOrEmpty(secret.Password)
                    || string.IsNullOrEmpty(secret.Dbname))
                {
                    throw new InvalidOperationException("Invalid secret format. Expected JSON with 'username' and 'password' fields");
                }
                
                _connectionString = $"Host={dbEndpoint};Port={secret.Port};Database={secret.Dbname};Username={secret.Username};Password={secret.Password};";
                
                return _connectionString;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get database connection string");
                throw;
            }
        }
    }

    public class DatabaseSecret
    {
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;
        
        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;
        
        [JsonPropertyName("dbname")]
        public string Dbname { get; set; } = string.Empty;
        
        [JsonPropertyName("engine")]
        public string Engine { get; set; } = string.Empty;
        
        [JsonPropertyName("port")]
        public int Port { get; set; }
        
        [JsonPropertyName("host")]
        public string Host { get; set; } = string.Empty;
        
        [JsonPropertyName("dbInstanceIdentifier")]
        public string DbInstanceIdentifier { get; set; } = string.Empty;
    }
} 