using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using ChatGPTBasedBot;
using static ChatGPTBasedBot.Helpers;
using System.Net.Http.Headers;

namespace ChatGPTBasedBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string configFilePath = "";

            Console.WriteLine("Path to config file: ");
            configFilePath = Console.ReadLine();

            var config = ReadConfigData(configFilePath);

            if (config == null)
            {
                return;
            }

            // Check if required folders exist and if images folder is required
            string imagesFolder = config.imageFolderPath;
            if (imagesFolder != null)
            {
                if (!Directory.Exists(imagesFolder))
                {
                    Console.WriteLine($"Folder \"{imagesFolder}\" does not exist.");
                    Console.WriteLine("Make sure you have a folder named \"Images\" with image files named \"Image1.jpg\", \"Image2.jpg\" etc.");
                    Console.WriteLine("Also make sure your images are numbered in the same order as your products.");
                    return;
                }
            }
            string productsFolder = config.dataFolderPath;

            if (!Directory.Exists(productsFolder))
            {
                Console.WriteLine($"Folder \"{productsFolder}\" does not exist.");
                Console.WriteLine("Make sure you have a folder named \"Data\" with text files named \"Product1.txt\", \"Product2.txt\" etc.");
                Console.WriteLine("Also make sure your text files are numbered in the same order as your images.");
                return;
            }

            DateTime postDate = LoadPostDate();
            if (postDate == DateTime.MinValue)
            {
                postDate = GetRandomDate();
                SavePostDate(postDate);
            }
            Console.WriteLine($"Post will be published: {postDate}");

            while (true)
            {
                if (ShouldPostNow(postDate))
                {
                    await ChatGPT.RunChat(config);
                    postDate = GetRandomDate(); // Set new random date
                    SavePostDate(postDate);
                    Console.WriteLine($"Next post will be published: {postDate}");
                }

                await Task.Delay(60000); // Check every minute
            }
        }
    }
}