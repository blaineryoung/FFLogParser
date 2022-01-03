using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FatedLogParser.FFLogObjects
{
    public static class PlayerInfo
    {
        public static string GetQuery(string reportId, int startTime, int endTime)
        {
            return $"{{\"query\":\"{{\\r\\n reportData\\r\\n    {{\\r\\n report(code: \\\"{reportId}\\\")\\r\\n        {{\\r\\n            playerDetails(startTime: {startTime}, endTime:{endTime})\\r\\n        }}\\r\\n    }}\\r\\n}}\\r\\n\"}}";
        }

        public static PlayerDetails Deserialize(string response)
        {
            PlayerListRoot? ar = JsonConvert.DeserializeObject<PlayerListRoot>(response);
            return ar.data.reportData.report.playerDetails.data.playerDetails;
        }
    }


    public class PlayerListRoot
    {
        public PlayerData data { get; set; }
    }

    public class PlayerData
    {
        public PlayerReportdata reportData { get; set; }
    }

    public class PlayerReportdata
    {
        public PlayerReport report { get; set; }
    }

    public class PlayerReport
    {
        public Playerdetails playerDetails { get; set; }
    }

    public class Playerdetails
    {
        public PlayerData1 data { get; set; }
    }

    public class PlayerData1
    {
        public PlayerDetails playerDetails { get; set; }
    }

    public class PlayerDetails
    {
        public Healer[] healers { get; set; }
        public Dp[] dps { get; set; }
        public Tank[] tanks { get; set; }
    }

    public class Healer
    {
        public string name { get; set; }
        public int id { get; set; }
        public int guid { get; set; }
        public string type { get; set; }
        public string server { get; set; }
        public string icon { get; set; }
        public object[] combatantInfo { get; set; }
    }

    public class Dp
    {
        public string name { get; set; }
        public int id { get; set; }
        public int guid { get; set; }
        public string type { get; set; }
        public string server { get; set; }
        public string icon { get; set; }
        public object[] combatantInfo { get; set; }
    }

    public class Tank
    {
        public string name { get; set; }
        public int id { get; set; }
        public int guid { get; set; }
        public string type { get; set; }
        public string server { get; set; }
        public string icon { get; set; }
        public object[] combatantInfo { get; set; }
    }

}
