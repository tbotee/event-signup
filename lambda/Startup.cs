using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using EventSignup.Services;
using Microsoft.EntityFrameworkCore;
using EventSignup.Data;

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


            services.AddSingleton<IDatabaseConnectionService, DatabaseConnectionService>();

            services.AddDbContext<EventSignupContext>((serviceProvider, options) =>
            {
                var connectionService = serviceProvider.GetRequiredService<IDatabaseConnectionService>();
                var connectionString = connectionService.GetConnectionString();
                options.UseNpgsql(connectionString);
            });

            services.AddScoped<IDatabaseService, DatabaseService>();
            
            services.AddScoped<EventSignup.Query>();
            services.AddScoped<EventSignup.Mutation>();

            services
                .AddGraphQLServer()
                .AddAuthorization()
                .AddQueryType<EventSignup.Query>()
                .AddMutationType<EventSignup.Mutation>()
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
