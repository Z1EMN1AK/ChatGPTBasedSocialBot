using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatGPTBasedBot
{
    public class configData
    {
        public string apiKey { get; set; }
        public string chatGPTModel { get; set; }
        public string webhookUrl { get; set; }
        public string dataFolderPath { get; set; }
        public string imageFolderPath { get; set; }
    }
}
