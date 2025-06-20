
using System.Text.Json;

namespace EventSignup.Lambda
{
    public class AppSyncRequest
    {
        public AppSyncInfo Info { get; set; } = default!;
        public Dictionary<string, JsonElement> Arguments { get; set; } = default!;
    }

}
