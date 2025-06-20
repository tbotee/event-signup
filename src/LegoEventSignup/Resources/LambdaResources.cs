using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.IAM;
using Constructs;
using System.Collections.Generic;

namespace LegoEventSignup.Resources
{
    public class LambdaResources
    {
        public Function GraphQLLambda { get; }

        public LambdaResources(Construct scope, Vpc vpc, string dbSecretArn, string dbEndpoint, string dbName, string userPoolId)
        {
            // var lambdaRole = new Role(scope, "GraphQLLambdaRole", new RoleProps
            // {
            //     AssumedBy = new ServicePrincipal("lambda.amazonaws.com"),
            //     ManagedPolicies = new[]
            //     {
            //         ManagedPolicy.FromAwsManagedPolicyName("service-role/AWSLambdaVPCAccessExecutionRole"),
            //         ManagedPolicy.FromAwsManagedPolicyName("service-role/AWSLambdaBasicExecutionRole")
            //     }
            // });

            // lambdaRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
            // {
            //     Effect = Effect.ALLOW,
            //     Actions = new[] { "secretsmanager:GetSecretValue" },
            //     Resources = new[] { dbSecretArn }
            // }));

            // lambdaRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
            // {
            //     Effect = Effect.ALLOW,
            //     Actions = new[] { "cognito-idp:VerifyToken" },
            //     Resources = new[] { $"arn:aws:cognito-idp:*:*:userpool/{userPoolId}" }
            // }));

            GraphQLLambda = new Function(scope, "LegoEventSignupGraphQLLambda", new FunctionProps
            {
                Runtime = Runtime.DOTNET_8,
                Handler = "EventSignup.Lambda::EventSignup.Lambda.Function::FunctionHandler",
                Code = Code.FromAsset("lambda"),
                Vpc = vpc,
                VpcSubnets = new SubnetSelection
                {
                    SubnetType = SubnetType.PRIVATE_WITH_EGRESS
                },
                // Role = lambdaRole,
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