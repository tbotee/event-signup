using Amazon.CDK;
using Amazon.CDK.AWS.AppSync;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Cognito;
using Constructs;
using Amazon.CDK.AwsApigatewayv2Authorizers;
using Amazon.CDK.AwsApigatewayv2Integrations;
using Amazon.CDK.AWS.Apigatewayv2;

namespace LegoEventSignup.Resources
{
    public class GraphQLApiResources
    {
        public HttpApi Api { get; }
        public LambdaDataSource LambdaDs { get; }

        public GraphQLApiResources(Construct scope, Function lambda, UserPool userPool, HttpUserPoolAuthorizer authorizer)
        {
            Api = new HttpApi(scope, "LegoGraphQLApi");

            Api.AddRoutes(new AddRoutesOptions
            {
                Path = "/graphql",
                Methods = new[] { Amazon.CDK.AWS.Apigatewayv2.HttpMethod.POST },
                Integration = new HttpLambdaIntegration("GraphQLIntegration", lambda),
            });
        }
    }
}