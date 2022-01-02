using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FatedLogParser.FFLogObjects
{
    public static class ReportList
    {
        public static string GetQuery(string reportId)
        {
            return $"{{\"query\":\"{{\\r\\n reportData\\r\\n    {{\\r\\n report(code: \\\"{reportId}\\\")\\r\\n        {{\\r\\n            fights\\r\\n            {{\\r\\n                startTime\\r\\n                endTime\\r\\n            }}\\r\\n        }}\\r\\n    }}\\r\\n}}\\r\\n\"}}";
        }

        public static ReportListData Deserialize(string response)
        {
            AllReports? ar = JsonConvert.DeserializeObject<AllReports>(response);
            return ar.data;
        }
    }

    public class AllReports
    {
        public ReportListData data { get; set; }
    }

    public class ReportListData
    {
        public Reportdata reportData { get; set; }
    }

    public class Reportdata
    {
        public Report report { get; set; }
    }

    public class Report
    {
        public Fight[] fights { get; set; }
    }

    public class Fight
    {
        public int startTime { get; set; }
        public int endTime { get; set; }
    }
}
