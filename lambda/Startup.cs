using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using EventSignup.Services;
using Microsoft.EntityFrameworkCore;
using EventSignup.Data;
using AppAny.HotChocolate.FluentValidation;
using EventSignup.Types;
using FluentValidation;

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

            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IParticipantService, ParticipantService>();
            services.AddScoped<IValidator<CreateEventInput>, CreateEventInputValidator>();
            services.AddScoped<IValidator<UpdateEventInput>, UpdateEventInputValidator>();
            services.AddScoped<IValidator<CreateParticipantInput>, CreateParticipantValidator>();

            services.AddHttpResponseFormatter();

            services
                .AddGraphQLServer()
                .AddQueryType<Query>()
                .AddMutationType<Mutation>()
                .AddMutationConventions(new MutationConventionOptions
                {
                    InputArgumentName = "input",
                    InputTypeNamePattern = "{MutationName}Input",
                    PayloadTypeNamePattern = "{MutationName}Payload",
                    PayloadErrorTypeNamePattern = "{MutationName}Error",
                    PayloadErrorsFieldName = "errors",
                    ApplyToAllMutations = true
                })
                .AddTypeExtension<EventMutations>()
                .AddTypeExtension<ParticipantMutations>()
                .AddTypeExtension(typeof(EventResolver))
                .AddTypeExtension(typeof(ParticipantResolver))
                .AddDataLoader<IEventByIdDataLoader, EventByIdDataLoader>()
                .AddDataLoader<IParticipantByIdDataLoader, ParticipantByIdDataLoader>()
                .AddDataLoader<IParticipantsByEventIdDataLoader, ParticipantsByEventIdDataLoader>()
                .AddMaxExecutionDepthRule(9)
                .SetMaxAllowedValidationErrors(10)
                .ModifyCostOptions(options =>
                {
                    options.MaxFieldCost = 20_000;
                    options.MaxTypeCost = 20_000;
                    options.EnforceCostLimits = true;
                    options.ApplyCostDefaults = true;
                    options.DefaultResolverCost = 10.0;
                })
                .ModifyRequestOptions(
                    options =>
                    {
                        options.IncludeExceptionDetails = true;
                    })
                .AddPagingArguments()
                .ModifyPagingOptions(opt =>
                {
                    opt.MaxPageSize = 100;
                    opt.DefaultPageSize = 25;
                    opt.IncludeTotalCount = true;
                })
                .AddFiltering()
                .AddSorting()
                .AddFluentValidation()
                .AddDiagnosticEventListener<HotChocolate.AspNetCore.Instrumentation.ServerDiagnosticEventListener>();
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
