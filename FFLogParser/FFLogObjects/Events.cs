using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FatedLogParser.FFLogObjects
{
    public static class EventInfo
    {
        public static string GetQuery(string reportId, int startTime, int endTime)
        {
            return $"{{\"query\":\"{{\\r\\n reportData\\r\\n    {{\\r\\n report(code: \\\"{reportId}\\\")\\r\\n        {{\\r\\n            events(startTime: {startTime}, endTime:{endTime})  \\r\\n  {{\\r\\n                data\\r\\n            }}\\r\\n      }}\\r\\n    }}\\r\\n}}\\r\\n\"}}";
        }

        public static EventDatum[] Deserialize(string response)
        {
            EventRoot? ar = JsonConvert.DeserializeObject<EventRoot>(response);
            return ar.data.reportData.report.events.data;
        }
    }


    public class EventRoot
    {
        public EventData data { get; set; }
    }

    public class EventData
    {
        public EventReportdata reportData { get; set; }
    }

    public class EventReportdata
    {
        public EventReport report { get; set; }
    }

    public class EventReport
    {
        public Events events { get; set; }
    }

    public class Events
    {
        public EventDatum[] data { get; set; }
    }

    public class EventDatum
    {
        public int timestamp { get; set; }
        public string type { get; set; }
        public int sourceID { get; set; }
        public int targetID { get; set; }
        public int abilityGameID { get; set; }
        public int hitType { get; set; }
        public int amount { get; set; }
        public bool tick { get; set; }
        public int unmitigatedAmount { get; set; }
        public float finalizedAmount { get; set; }
        public bool simulated { get; set; }
        public int expectedAmount { get; set; }
        public int expectedCritRate { get; set; }
        public float actorPotencyRatio { get; set; }
        public float guessAmount { get; set; }
        public float multiplier { get; set; }
        public float directHitPercentage { get; set; }
        public int absorbed { get; set; }
        public int sourceMarker { get; set; }
        public bool directHit { get; set; }
        public int bonusPercent { get; set; }
        public int packetID { get; set; }
        public bool melee { get; set; }
        public int targetMarker { get; set; }
        public int value { get; set; }
        public int bars { get; set; }
        public int overheal { get; set; }
        public int duration { get; set; }
        public int sourceInstance { get; set; }
        public bool unpaired { get; set; }
        public int absorb { get; set; }
        public int extraAbilityGameID { get; set; }
        public int targetInstance { get; set; }
        public int stack { get; set; }
    }

}
