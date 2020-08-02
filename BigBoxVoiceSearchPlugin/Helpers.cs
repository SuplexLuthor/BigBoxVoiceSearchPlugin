using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using BigBoxVoiceSearchPlugin.Models;
using Newtonsoft.Json;

namespace BigBoxVoiceSearchPlugin
{
    public static class Helpers
    {
        public static void Log(string logMessage)
        {
            using (StreamWriter w = File.AppendText("BigBoxVoiceSearchPlugin.txt"))
            {
                w.Write("\r\nLog Entry : ");
                w.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
                w.WriteLine("  :");
                w.WriteLine($"  :{logMessage}");
                w.WriteLine("-------------------------------");
            }
        }

        public static void LogException(Exception ex, string context)
        {
            Helpers.Log($"An exception occurred while attempting to {context}");
            Helpers.Log($"Exception message: {ex?.Message ?? "null"}");
            Helpers.Log($"Exception stack: {ex?.StackTrace ?? "null"}");
        }

        public static readonly string GamesDatabaseFileName = "BigBoxVoiceSearchDatabase.json";
        public static void WriteGamesDatabaseAsJson(Dictionary<string, List<GameMatch>> phraseGamesDictionary)
        {
            using (StreamWriter file = File.CreateText(GamesDatabaseFileName))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, phraseGamesDictionary);
            }
        }

        public static Dictionary<string, List<GameMatch>> ReadGamesDatabaseAsJson()
        {
            using (StreamReader file = File.OpenText(GamesDatabaseFileName))
            {
                JsonSerializer serializer = new JsonSerializer();
                return (Dictionary<string, List<GameMatch>>)serializer.Deserialize(file, typeof(Dictionary<string, List<GameMatch>>));
            }
        }
    }
}
