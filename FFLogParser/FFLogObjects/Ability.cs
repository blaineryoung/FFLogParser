using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FatedLogParser.FFLogObjects
{
    public static class Ability
    {
        public static string GetQuery(int abilityId)
        {
            return $"{{\"query\":\"{{\\r\\n gameData\\r\\n    {{\\r\\n ability(id: {abilityId})\\r\\n        {{\\r\\n\\r\\n name\\r\\n id\\r\\n\\r\\n        }}\\r\\n    }}\\r\\n}}\"}}";
        }

        public static AbilityDefinition Deserialize(string response)
        {
            AbilityRoot? ar = JsonConvert.DeserializeObject<AbilityRoot>(response);
            return ar.data.gameData.ability;
        }
    }


    public class AbilityRoot
    {
        public AbilityList data { get; set; }
    }

    public class AbilityList
    {
        public Gamedata gameData { get; set; }
    }

    public class Gamedata
    {
        public AbilityDefinition ability { get; set; }
    }

    public class AbilityDefinition
    {
        public string name { get; set; }
        public int id { get; set; }
    }

}
