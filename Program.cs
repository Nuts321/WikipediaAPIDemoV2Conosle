using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using WikipediaApiDemoV2.Models;

namespace WikipediaApiDemoV2
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                
                Console.WriteLine("\t----To exit this app write quit!----");
                Console.Write("\t----What would you like to search for?  ");
                string search = Console.ReadLine();
                if (search=="quit")
                {
                    break;
                }
                search = search.Replace(" ", "%20");
                string url = @"https://en.wikipedia.org/w/api.php?format=json&action=query&prop=extracts&exlimit=max&explaintext&exintro&titles=" + search + "&redirects=";

                SearchInfo(url);
            }        
        }

        //give back info about search
        private static void SearchInfo(string url)
        {
            string result = GetJson(url);

            var ms = JsonConvert.DeserializeObject<RootObject>(result);

            var firstKey = ms.query.pages.First().Key;
            string title = ms.query.pages[firstKey].title;
            string text = ms.query.pages[firstKey].extract;
            int id = ms.query.pages[firstKey].pageid;
            Console.WriteLine($"PageId:\t{id}\n");
            Console.WriteLine($"Title:\t{title}\n");
            Console.WriteLine($"Text:\n{text}\n");
        }

        //get json
        private static string GetJson(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            try
            {
                WebResponse response = request.GetResponse();

                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException exp)
            {
                WebResponse response = exp.Response;

                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    string errorText = reader.ReadToEnd();
                }
                throw;
            }
        }
    }
}
