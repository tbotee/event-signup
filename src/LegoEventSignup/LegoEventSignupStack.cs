using Amazon.CDK;
using Constructs;
using LegoEventSignup.Resources;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.EC2;
using System.Collections.Generic;

namespace LegoEventSignup
{
    public class LegoEventSignupStack : Stack
    {
        internal LegoEventSignupStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var vpcResources = new VpcResources(this);
            var vpc = vpcResources.Vpc;

            var cognitoResources = new CognitoResources(this);
            var userPool = cognitoResources.UserPool;
            var userPoolClient = cognitoResources.UserPoolClient;

            var dbResources = new DatabaseResources(this, vpc);
            var db = dbResources.DatabaseInstance;
            var dbSecret = dbResources.DatabaseSecret;

            var graphqlLambda = new Function(this, "EventSignupGraphQLLambda", new FunctionProps
            {
                Runtime = Runtime.DOTNET_8,
                Handler = "EventSignup.Lambda::EventSignup.Lambda.Function::FunctionHandler",
                Code = Amazon.CDK.AWS.Lambda.Code.FromAsset("lambda"),
                Vpc = vpc,
                VpcSubnets = new SubnetSelection
                {
                    SubnetType = SubnetType.PRIVATE_WITH_EGRESS
                },
                Environment = new Dictionary<string, string>
                {
                    ["DB_SECRET_ARN"] = dbSecret.SecretArn,
                    ["DB_ENDPOINT"] = db.InstanceEndpoint.Hostname,
                    ["DB_NAME"] = DatabaseResources.DB_NAME,
                    ["USER_POOL_ID"] = userPool.UserPoolId
                }
            });

            dbSecret.GrantRead(graphqlLambda);

            new CfnOutput(this, "UserPoolId", new CfnOutputProps 
            { 
                Value = userPool.UserPoolId,
                Description = "LegoUserPool Cognito User Pool ID"
            });

            new CfnOutput(this, "UserPoolClientId", new CfnOutputProps
            {
                Value = userPoolClient.UserPoolClientId,
                Description = "LegoUserPoolClient Cognito User Pool Client ID"
            });

            new CfnOutput(this, "DatabaseEndpoint", new CfnOutputProps
            {
                Value = db.DbInstanceEndpointAddress,
                Description = "LegoEventSignupPostgresDatabase RDS Database Endpoint"
            });

            new CfnOutput(this, "VpcId", new CfnOutputProps { Value = vpc.VpcId });
        }
    }
}
