using Amazon.CDK;
using Constructs;
using EventSignup.Resources;

namespace EventSignup
{
    public class EventSignupStack : Stack
    {
        internal EventSignupStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var vpcResources = new VpcResources(this);
            var vpc = vpcResources.Vpc;

            var cognitoResources = new CognitoResources(this);
            var userPool = cognitoResources.UserPool;
            var userPoolClient = cognitoResources.UserPoolClient;
            var authorizer = cognitoResources.Authorizer;

            var dbResources = new DatabaseResources(this, vpc);
            var db = dbResources.DatabaseInstance;
            var dbSecret = dbResources.DatabaseSecret;

            var lambdaResources = new LambdaResources(
                this, 
                vpc, 
                dbSecret.SecretArn, 
                db.InstanceEndpoint.Hostname, 
                DatabaseResources.DB_NAME, 
                userPool.UserPoolId
            );
            var graphqlLambda = lambdaResources.GraphQLLambda;

            dbSecret.GrantRead(graphqlLambda);

            var graphqlApiResources = new GraphQLApiResources(this, graphqlLambda, userPool, authorizer);

            new CfnOutput(this, "GraphQLAPIURL", new CfnOutputProps
            {
                Value = graphqlApiResources.Api.Url,
                Description = "GraphQL API URL"
            });

            new CfnOutput(this, "UserPoolId", new CfnOutputProps 
            { 
                Value = userPool.UserPoolId,
                Description = "UserPool Cognito User Pool ID"
            });

            new CfnOutput(this, "UserPoolClientId", new CfnOutputProps
            {
                Value = userPoolClient.UserPoolClientId,
                Description = "UserPoolClient Cognito User Pool Client ID"
            });

            new CfnOutput(this, "DatabaseEndpoint", new CfnOutputProps
            {
                Value = db.DbInstanceEndpointAddress,
                Description = "EventSignupPostgresDatabase RDS Database Endpoint"
            });

            new CfnOutput(this, "VpcId", new CfnOutputProps { Value = vpc.VpcId });
        }
    }
}
