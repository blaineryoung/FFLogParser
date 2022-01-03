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

            Tuple<int, int> currentTimestamp = new Tuple<int, int>(t.Item1, t.Item2);
            int daktId = p.Where(x => x.Value == "Dakt Cole").FirstOrDefault().Key;

            Console.WriteLine("Time, Source, Ability, Target, Ammount, Type, Overheal, Tick");
            for (int i = 0; i < 10; i++) // limit to 10 for testing
            {
                int highTime = currentTimestamp.Item1;
                EventDatum[] events = await ffLogClient.GetEvents(reportId, currentTimestamp);

                if (events.Length == 0)
                {
                    break;
                }

                highTime = events[events.Length - 1].timestamp;

                IEnumerable<EventDatum> filteredEvents = events.Where(x =>
                    ((x.type == "heal") && (x.targetID == daktId)) || // healing to dakt
                    ((!p.ContainsKey(x.sourceID)) && (x.type == "calculateddamage") && (x.targetID == daktId)) // boss damage );
                    );


                foreach (EventDatum e in filteredEvents)
                {
                    int time = (e.timestamp - t.Item1) /1000;
                    string abilityName = await ffLogClient.GetAbilityName(e.abilityGameID);

                    if (abilityName == "Unknown")
                    {
                        continue;
                    }

                    string sourceName;
                    if (false == p.TryGetValue(e.sourceID, out sourceName))
                    {
                        sourceName = e.type == "heal" ? "Dakt Cole" : "Boss";
                    }

                    string targetName;
                    if (false == p.TryGetValue(e.targetID, out targetName))
                    {
                        targetName = e.type == "heal" ? "Dakt Cole" : "Boss";
                    }

                    Console.WriteLine($"{time}, {sourceName}, {abilityName},  {targetName}, {e.amount}, {e.type}, {e.overheal}, {e.tick}");
                }

                currentTimestamp = new Tuple<int, int>(highTime, t.Item2);
            }
        }
    }
}