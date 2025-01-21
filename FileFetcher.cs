using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatGPTBasedBot
{
    class FileFetcher
    {
        private string? productPath;
        private string? imageFolder;
        private string? productFolder;
        private string? path;
        private string[]? files;
        private int? fileNumber;

        public string? imagePath { get; set; }
        public string? subject { get; set; }
        public string? data { get; set; }
        public decimal? price { get; set; }
        public string? link { get; set; }

        public FileFetcher()
        {
            productFolder = "Produkty";
            imageFolder = "Obrazy";
        }

        public void FetchProdAndImg()
        {
            path = Path.Combine(Directory.GetCurrentDirectory(), productFolder);

            files = Directory.GetFiles(path);

            Random rnd = new Random();

            // Losowanie numeru pliku w zakresie od 1 do liczby plików
            fileNumber = rnd.Next(1, files.Length + 1);

            productPath = Path.Combine(path, "Produkt" + fileNumber.ToString() + ".txt");
            imagePath = imageFolder + '\\' + "ObrazProduktu" + fileNumber + ".jpg";
        }

        public void Fill()
        {
            string temp = "";
            using (StreamReader sr = new StreamReader(productPath))
            {
                temp = sr.ReadToEnd();
            }

            string[] strings = temp.Split("\n");

            for (var i = 0; i < strings.Length; i++)
            {
                if (strings[i].Contains("Tytuł: "))
                {
                    subject = strings[i].Replace("Tytuł: ", "").Trim();
                }

                if (strings[i].Contains("div class"))
                {
                    strings[i] = "";
                }

                if (strings[i].Contains("Opis: <h2>Opis</h2>"))
                {
                    data = FetchData(strings, i + 1);
                }
                else if (strings[i].Contains("Opis: "))
                {
                    data = strings[i].Replace("Opis: ", "").Trim();
                }

                if (strings[i].Contains("Cena: "))
                {
                    string priceString = strings[i].Replace("Cena: ", "").Replace(" zł", "").Trim();
                    if (decimal.TryParse(priceString, out decimal parsedPrice))
                    {
                        price = parsedPrice;
                    }
                    else
                    {
                        price = 0;
                    }
                }

                if (strings[i].Contains("ID Produktu:"))
                {
                    strings[i] = "";
                }

                if (strings[i].Contains("Link: "))
                {
                    link = strings[i].Replace("Link: ", "").Trim();
                }
            }
        }

        private string FetchData(string[] strings, int startIndex)
        {
            string data = "";

            for (var i = startIndex; i < strings.Length; i++)
            {
                if (!strings[i].Contains("Cena: ") && !strings[i].Contains("ID Produktu:") && !strings[i].Contains("Link: "))
                {
                    data += strings[i].Trim() + "\n";
                }
                else break;
            }

            return data.Trim();
        }
    }
}
