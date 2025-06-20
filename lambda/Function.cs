using Amazon.Lambda.Core;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace EventSignup.Lambda
{
    public class Function
    {
        private static readonly IRequestExecutor _executor;

        static Function()
        {
            var services = new ServiceCollection();

            services
                .AddGraphQL()
                .AddQueryType<Query>();

            var serviceProvider = services.BuildServiceProvider();
            _executor = serviceProvider
                .GetRequiredService<IRequestExecutorResolver>()
                .GetRequestExecutorAsync()
                .GetAwaiter()
                .GetResult();
        }

        public async Task<object?> FunctionHandler(AppSyncRequest request, ILambdaContext context)
        {
            var query = BuildGraphQLQuery(request);

            IExecutionResult result = await _executor.ExecuteAsync(query);

            if (result is IQueryResult queryResult && queryResult.Errors?.Count > 0)
                throw new Exception(string.Join("\n", queryResult.Errors.Select(e => e.Message)));

            return ((IQueryResult)result).Data?.Values.FirstOrDefault();
        }

        private string BuildGraphQLQuery(AppSyncRequest request)
        {
            var args = request.Arguments.Any()
                ? $"({string.Join(", ", request.Arguments.Select(arg => $"{arg.Key}: {arg.Value}"))})"
                : "";

            return $"query {{ {request.Info.FieldName}{args} {{ id name date }} }}";
        }
    }
}
