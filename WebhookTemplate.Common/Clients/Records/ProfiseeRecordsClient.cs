using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Profisee.WebhookTemplate.Common.Configuration;

namespace Profisee.WebhookTemplate.Common.Clients.Records
{
    public class ProfiseeRecordsClient : BaseProfiseeClient, IProfiseeRecordsClient
    {
        private static readonly int apiVersion = 1;

        public ProfiseeRecordsClient(HttpClient httpClient, IOptions<AppSettings> appSettings, ILogger<ProfiseeRecordsClient> logger)
            : base(httpClient, appSettings, logger)
        {

        }

        public async Task<ProfiseeContentResponse> UpdateRecordAsync(Guid entityUId,
            string recordCode,
            Dictionary<string, object> attributeNameValuePairs)
        {
            const string uriFormat = "v{0}/Records/{1}/{2}";
            var requestUri = string.Format(uriFormat, apiVersion, entityUId, recordCode);

            var result = await PatchAsync(requestUri, attributeNameValuePairs);

            return result;
        }
    }
}
