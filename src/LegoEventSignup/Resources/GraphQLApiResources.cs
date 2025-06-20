using Amazon.CDK;
using Amazon.CDK.AWS.AppSync;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Cognito;
using Constructs;

namespace LegoEventSignup.Resources
{
    public class GraphQLApiResources
    {
        public GraphqlApi Api { get; }
        public LambdaDataSource LambdaDs { get; }

        public GraphQLApiResources(Construct scope, Function lambda, UserPool userPool)
        {
            Api = new GraphqlApi(scope, "LegoEventSignupApi", new GraphqlApiProps
            {
                Name = "LegoEventSignupApi",
                Definition = Definition.FromFile("graphql/schema.graphql"),
                AuthorizationConfig = new AuthorizationConfig
                {
                    DefaultAuthorization = new AuthorizationMode
                    {
                        AuthorizationType = AuthorizationType.USER_POOL,
                        UserPoolConfig = new UserPoolConfig
                        {
                            UserPool = userPool
                        }
                    },
                    AdditionalAuthorizationModes = new[]
                    {
                        new AuthorizationMode
                        {
                            AuthorizationType = AuthorizationType.API_KEY
                        }
                    }
                },
                XrayEnabled = true
            });

            LambdaDs = Api.AddLambdaDataSource("LambdaDatasource", lambda);
            AddResolvers(LambdaDs);
        }

        private void AddResolvers(LambdaDataSource api)
        {
            
        }
    }
}