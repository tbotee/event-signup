using Amazon.Lambda.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace EventSignup.Lambda 
{
    public class Function : Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction
    {
        protected override void Init(IWebHostBuilder builder)
        {
            builder.ConfigureServices((services) =>
                {

                var region = Environment.GetEnvironmentVariable("AWS_REGION") 
                    ?? "eu-north-1";
                var userPoolId = Environment.GetEnvironmentVariable("USER_POOL_ID");
                
                var cognitoAuthority = $"https://cognito-idp.{region}.amazonaws.com/{userPoolId}";
                
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.Authority = cognitoAuthority;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateAudience = false, // Or true, and set Audience
                        };
                    });

                services.AddAuthorization();

                services
                    .AddGraphQLServer()
                    .AddAuthorization()
                    .AddQueryType<Query>()
                    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);
            })
            .ConfigureLogging(logging => logging.AddConsole())
            .Configure(app =>
            {
                app.UseRouting();
                app.UseAuthentication();
                app.UseAuthorization();
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapGraphQL(); 
                });
            });
        }

    }
}
