using Amazon.CDK;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventSignup
{
    sealed class Program
    {
        private static ILogger<Program> _logger;

        public static void Main(string[] args)
        {

            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSimpleConsole(options =>
                {
                    options.SingleLine = true;
                    options.TimestampFormat = "hh:mm:ss ";
                });
                builder.SetMinimumLevel(LogLevel.Information);
            });

            _logger = loggerFactory.CreateLogger<Program>();

            var app = new App();
            
            try
            {
                _logger.LogInformation("Starting CDK synth...");
                new EventSignupStack(app, "EventSignupStack", new StackProps
                {
                    Env = new Amazon.CDK.Environment
                    {
                        Account = System.Environment.GetEnvironmentVariable("CDK_DEFAULT_ACCOUNT"),
                        Region = System.Environment.GetEnvironmentVariable("CDK_DEFAULT_REGION"),
                    }
                });

                app.Synth();

                _logger.LogInformation("CDK synth completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error synthesizing CDK app: {ex.Message}");
                _logger.LogError(ex, "Error synthesizing CDK app");
                System.Environment.Exit(1);
            }
        }
    }
}
