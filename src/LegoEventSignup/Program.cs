using Amazon.CDK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LegoEventSignup
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new LegoEventSignupStack(app, "LegoEventSignupStack", new StackProps
            {
                Env = new Amazon.CDK.Environment
                {
                    Account = System.Environment.GetEnvironmentVariable("CDK_DEFAULT_ACCOUNT"),
                    Region = System.Environment.GetEnvironmentVariable("CDK_DEFAULT_REGION"),
                }
            });
            
            try
            {
                app.Synth();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error synthesizing CDK app: {ex.Message}");
                System.Environment.Exit(1);
            }
        }
    }
}
