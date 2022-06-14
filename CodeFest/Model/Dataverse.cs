using Nancy.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EchoBot2.Models
{
    public class Dataverse
    {
        public class RootFlowList
        {
            public List<RootFlow> data { get; set; }
        }
        public class RootFlow
        {
            [JsonProperty("@odata.type")]
            public string OdataType { get; set; }

            [JsonProperty("@odata.id")]
            public string OdataId { get; set; }

            [JsonProperty("@odata.etag")]
            public string OdataEtag { get; set; }

            [JsonProperty("@odata.editLink")]
            public string OdataEditLink { get; set; }
            public string crea1_evetitulo { get; set; }
            public string crea1_evedescri { get; set; }
            public string crea1_evecodint { get; set; }

            [JsonProperty("crea1_evefchini2@OData.Community.Display.V1.FormattedValue")]
            public string Crea1Evefchini2ODataCommunityDisplayV1FormattedValue { get; set; }

            [JsonProperty("crea1_evefchini2@odata.type")]
            public string Crea1Evefchini2OdataType { get; set; }
            public DateTime crea1_evefchini2 { get; set; }

            [JsonProperty("crea1_event_maestid@odata.type")]
            public string Crea1EventMaestidOdataType { get; set; }
            public string crea1_event_maestid { get; set; }
        }

        public async Task<string> GetEventList()
        {
            string url = $"https://prod-25.brazilsouth.logic.azure.com/workflows/891e1a28b31e4a54a4455af80ad12a27/triggers/manual/paths/invoke?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=jjTrS5CiAvIiUFr98H6wrZnXK3Yx9UXnfcDpnaB_Mo4";

            using (var httpClient = new HttpClient())
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                var response = await httpClient.SendAsync(requestMessage);
                var responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
        }
        public async Task<string> SuscribeEventList(string EVECODINT,string I_MAIL,string I_USERNAME)
        {
            string url = "https://prod-01.brazilsouth.logic.azure.com:443/workflows/c0bd479a05b043518df63ffc3a17ee1b/triggers/manual/paths/invoke?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=AO_9SiOqEKag8kMPb2XhLGAqmk6WFNdUE3UJaaEePgw";
            string json = new JavaScriptSerializer().Serialize(new
            {
                EVECODINT,
                I_MAIL,
                I_USERNAME 

            });
           
            var request = new HttpRequestMessage
            {
                Content = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json"),
                Method = HttpMethod.Post,
                RequestUri = new Uri(url),

            };
            HttpClient client = new HttpClient();
            var response = await client.SendAsync(request).ConfigureAwait(false);
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<dynamic>(responseBody)?.success ?? "false";



        }
    }
}
