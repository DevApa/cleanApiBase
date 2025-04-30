using Newtonsoft.Json;

namespace bg.crm.integration.application.dtos.responses
{
    public class MsDtoResponseSuccess<T>
    {
        [JsonProperty("traceId")]
        public string? TraceId { get; set; }
        [JsonProperty("data")]
        public T Data { get; set; }

        /// <summary>
        /// MsDtoResponseSuccess constructor
        /// </summary>
        /// <param name="traceId"></param>
        /// <param name="data"></param>
        public MsDtoResponseSuccess(string? traceId, T data)
        {
            TraceId = traceId;
            Data = data;
        }
    }
}