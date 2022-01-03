using FatedLogParser.FFLogObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace FatedLogParser
{


    public class FFLogClient
    {
        HttpClient client;

        Dictionary<int, string> abilityNames = new Dictionary<int, string>();

        private FFLogClient (HttpClient client)
        {
            this.client = client;
        }

        public static async Task<FFLogClient> Connect(string clientId, string secret)
        {
            Uri authUri = new Uri("https://www.fflogs.com/oauth/token");
            HttpClient authClient = new HttpClient();
            authClient.DefaultRequestHeaders.Authorization = new BasicAuthenticationHeaderValue(clientId, secret);
            FormUrlEncodedContent content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("grant_type", "client_credentials") });

            var response = await authClient.PostAsync(authUri, content);
            response.EnsureSuccessStatusCode();
            string rs = await response.Content.ReadAsStringAsync();

            TokenResponse? tr = JsonConvert.DeserializeObject<TokenResponse>(rs);

            HttpClient appClient = new HttpClient();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            appClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tr.access_token);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            return new FFLogClient(appClient);
        }

        public async Task<IEnumerable<Tuple<int, int>>> GetTimestamps(string reportId)
        {
            string raw = await this.MakeRequest(ReportList.GetQuery(reportId));
            ReportListData data = ReportList.Deserialize(raw);

            return data.reportData.report.fights.Select(x => new Tuple<int, int>(x.startTime, x.endTime));
        }

        public async Task<string> GetAbilityName(int id)
        {
            string name;

            if (false == abilityNames.TryGetValue(id, out name))
            {
                string raw = await this.MakeRequest(Ability.GetQuery(id));
                AbilityDefinition ab = Ability.Deserialize(raw);

                name = null == ab ? "Unknown" : ab.name;
                abilityNames.Add(id, name);
            }

            return name;
        }

        public async Task<PlayerDetails> GetPlayers(string reportId, Tuple<int, int> timestamp)
        {
            string raw = await this.MakeRequest(PlayerInfo.GetQuery(reportId, timestamp.Item1, timestamp.Item2));
            PlayerDetails data = PlayerInfo.Deserialize(raw);

            return data;
        }

        public async Task<EventDatum[]> GetEvents(string reportId, Tuple<int, int> timestamp)
        {
            string raw = await this.MakeRequest(EventInfo.GetQuery(reportId, timestamp.Item1, timestamp.Item2));
            EventDatum[] data = EventInfo.Deserialize(raw);

            return data;
        }

        public async Task<Dictionary<int, string>> GetPlayerMappings(string reportId, Tuple<int, int> timestamp)
        {
            Dictionary<int, string> mappings = new Dictionary<int, string>();
            string raw = await this.MakeRequest(PlayerInfo.GetQuery(reportId, timestamp.Item1, timestamp.Item2));
            PlayerDetails data = PlayerInfo.Deserialize(raw);

            foreach (Healer h in data.healers)
            {
                mappings.Add(h.id, h.name);
            }

            foreach (Tank h in data.tanks)
            {
                mappings.Add(h.id, h.name);
            }

            foreach (Dp h in data.dps)
            {
                mappings.Add(h.id, h.name);
            }

            return mappings;
        }

        private async Task<string> MakeRequest(string query)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://www.fflogs.com/api/v2/client"),
                Content = new StringContent(query, Encoding.UTF8, MediaTypeNames.Application.Json),
            };

            var response = await this.client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string rs = await response.Content.ReadAsStringAsync();
            return rs;
        }
    }

    public class TokenResponse
    {
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string access_token { get; set; }
    }
}
