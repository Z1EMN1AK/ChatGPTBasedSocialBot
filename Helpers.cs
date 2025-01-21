using System;
using System.IO;
using Newtonsoft.Json;

namespace ChatGPTBasedBot
{
    public static class Helpers
    {
        public static DateTime GetRandomDate()
        {
            Random random = new Random();
            int days = random.Next(3, 4); // Random number of days from 3 to 4
            int hour = random.Next(8, 18); // Random hour from 8 to 17
            int minute = random.Next(0, 60); // Random minute from 0 to 59
            return DateTime.Now.Date.AddDays(days).AddHours(hour).AddMinutes(minute);
        }

        public static bool ShouldPostNow(DateTime postDate)
        {
            DateTime currentTime = DateTime.Now;
            return currentTime >= postDate;
        }

        public static void SavePostDate(DateTime postDate)
        {
            File.WriteAllText("postDate.txt", postDate.ToString("o"));
        }

        public static configData ReadConfigData(string configFilePath)
        {
            if (!File.Exists(configFilePath))
            {
                Console.WriteLine("No config file found.");
                Console.WriteLine("Make sure you have a config file named 'ProgramConfig.cfg' in the same directory as the executable.");
                return null;
            }

            using (StreamReader sr = new StreamReader(configFilePath))
            {
                string data = sr.ReadToEnd();
                var config = JsonConvert.DeserializeObject<configData>(data);

                if (config == null)
                {
                    Console.WriteLine("Failed to read config data.");
                    return null;
                }

                if (string.IsNullOrEmpty(config.apiKey) || string.IsNullOrEmpty(config.webhookUrl))
                {
                    Console.WriteLine("Config data is incomplete.");
                    return null;
                }

                return config;
            }
        }

        public static DateTime LoadPostDate()
        {
            if (File.Exists("postDate.txt"))
            {
                string postDateString = File.ReadAllText("postDate.txt");
                if (DateTime.TryParse(postDateString, out DateTime postDate))
                {
                    return postDate;
                }
            }
            return DateTime.MinValue;
        }
    }
}