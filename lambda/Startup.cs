using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace EventSignup
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var userPoolId = Environment.GetEnvironmentVariable("USER_POOL_ID");
            var region = Environment.GetEnvironmentVariable("AWS_REGION") ?? "eu-north-1";
            var authority = $"https://cognito-idp.{region}.amazonaws.com/{userPoolId}";

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = authority;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });

            services.AddAuthorization();

            services
                .AddGraphQLServer()
                .AddAuthorization()
                .AddQueryType<Query>()
                .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });
        }
    }
}