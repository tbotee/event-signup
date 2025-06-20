using Amazon.CDK;
using Amazon.CDK.AWS.Cognito;
using Constructs;

namespace LegoEventSignup.Resources
{
    public class CognitoResources
    {
        public UserPool UserPool { get; }
        public UserPoolClient UserPoolClient { get; }

        public CognitoResources(Construct scope)
        {
            UserPool = new UserPool(scope, "LegoUserPool", new UserPoolProps
            {
                UserPoolName = "LegoEventUserPool",
                SelfSignUpEnabled = false,
                SignInAliases = new SignInAliases { Username = true, Email = true },
                AutoVerify = new AutoVerifiedAttrs { Email = true },
                StandardAttributes = new StandardAttributes
                {
                    Email = new StandardAttribute { Required = true, Mutable = false }
                },
                PasswordPolicy = new PasswordPolicy
                {
                    MinLength = 8,
                    RequireLowercase = true,
                    RequireUppercase = true,
                    RequireDigits = true,
                    RequireSymbols = true
                },
                AccountRecovery = AccountRecovery.EMAIL_ONLY
            });

            UserPoolClient = new UserPoolClient(scope, "LegoUserPoolClient", new UserPoolClientProps
            {
                UserPool = UserPool,
                GenerateSecret = false,
                AuthFlows = new AuthFlow
                {
                    UserPassword = true,
                    AdminUserPassword = true
                }
            });
        }
    }
}