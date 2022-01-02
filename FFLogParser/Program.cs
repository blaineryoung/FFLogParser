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

            string clientId = File.ReadAllText(clientFile);
            string secret = File.ReadAllText(secretFile);

            FFLogClient ffLogClient= await FFLogClient.Connect(clientId, secret);



            foreach (Tuple<int, int> timestamp in await ffLogClient.GetTimestamps("BFRxMhXCZWfybqrY"))
            {
                Console.WriteLine($"{timestamp.Item1} - {timestamp.Item2}");
            }

            string name = await ffLogClient.GetAbilityName(16540);
            name = await ffLogClient.GetAbilityName(16540);
            Console.WriteLine($"{name}");
        }
    }
}