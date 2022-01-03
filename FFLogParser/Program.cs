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
            string clientFile = args[0];
            string secretFile = args[1];

            string reportId = "BFRxMhXCZWfybqrY";

            string clientId = File.ReadAllText(clientFile);
            string secret = File.ReadAllText(secretFile);

            FFLogClient ffLogClient= await FFLogClient.Connect(clientId, secret);

            int raidIndex = 0;

            foreach (Tuple<int, int> timestamp in await ffLogClient.GetTimestamps(reportId))
            {
                Console.WriteLine($"Raid {raidIndex++}:{timestamp.Item1} - {timestamp.Item2}");

                PlayerDetails players = await ffLogClient.GetPlayers(reportId, timestamp);

                Console.WriteLine("Healers:");
                foreach (Healer h in players.healers)
                {
                    Console.WriteLine($"{h.name} - {h.id}");
                }

                foreach (Tank t in players.tanks)
                {
                    Console.WriteLine($"{t.name} - {t.id}");
                }

                foreach (Dp d in players.dps)
                {
                    Console.WriteLine($"{d.name} - {d.id}");
                }
            }

            string name = await ffLogClient.GetAbilityName(16540);
            name = await ffLogClient.GetAbilityName(16540);
            Console.WriteLine($"{name}");
        }
    }
}