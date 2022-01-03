using FatedLogParser.FFLogObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FatedLogParser // Note: actual namespace depends on the project name.
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // You can get secrets from here: https://www.fflogs.com/api/docs
            string clientFile = args[0];
            string secretFile = args[1];

            string reportId = "BFRxMhXCZWfybqrY";

            string clientId = File.ReadAllText(clientFile);
            string secret = File.ReadAllText(secretFile);

            FFLogClient ffLogClient= await FFLogClient.Connect(clientId, secret);

            int raidIndex = 0;

            Tuple<int, int> t = null;
            Dictionary<int, string> p = null;

            foreach (Tuple<int, int> timestamp in await ffLogClient.GetTimestamps(reportId))
            {
                Dictionary<int, string> playerMappings = await ffLogClient.GetPlayerMappings(reportId, timestamp);

                t = timestamp;
                p = playerMappings;
            }

            EventDatum[] events = await ffLogClient.GetEvents(reportId, t);

            Console.WriteLine("Time, Ability, Source, Target, Ammount, Type");

            foreach(EventDatum e in events)
            {
                int time = e.timestamp - t.Item1;
                string abilityName = await ffLogClient.GetAbilityName(e.abilityGameID);

                string sourceName;
                if (false == p.TryGetValue(e.sourceID, out sourceName))
                {
                    sourceName = "Boss";
                }

                string targetName;
                if (false == p.TryGetValue(e.sourceID, out targetName))
                {
                    targetName = "Boss";
                }

                Console.WriteLine($"{time}, {abilityName}, {sourceName}, {targetName}, {e.amount}, {e.type}");

            }
        }
    }
}