using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.IAM;
using Constructs;
using System.Collections.Generic;

namespace EventSignup.Resources
{
    public class LambdaResources
    {
        public Function GraphQLLambda { get; }

        public LambdaResources(Construct scope, Vpc vpc, string dbSecretArn, string dbEndpoint, string dbName, string userPoolId)
        {
            GraphQLLambda = new Function(scope, "EventSignupGraphQLLambda", new FunctionProps
            {
                Runtime = Runtime.DOTNET_8,
                Handler = "EventSignup::EventSignup.Function::FunctionHandlerAsync",
                Code = Code.FromAsset("lambda/publish"),
                Vpc = vpc,
                VpcSubnets = new SubnetSelection
                {
                    SubnetType = SubnetType.PRIVATE_WITH_EGRESS
                },
                Timeout = Duration.Seconds(30),
                MemorySize = 512,
                Environment = new Dictionary<string, string>
                {
                    ["DB_SECRET_ARN"] = dbSecretArn,
                    ["DB_ENDPOINT"] = dbEndpoint,
                    ["DB_NAME"] = dbName,
                    ["USER_POOL_ID"] = userPoolId
                }
            });
        }
    }
} 