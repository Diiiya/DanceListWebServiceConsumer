using DanceDLL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DanceListConsumer
{
    class Program
    {
        static readonly HttpClient client = new HttpClient();

        public static string DancesUri = "http://localhost:60438/api/dances";

        public static async Task<IList<Dance>> GetStudentsAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                string content = await client.GetStringAsync(DancesUri);
                IList<Dance> dancesList = JsonConvert.DeserializeObject<IList<Dance>>(content);
                return dancesList;
            }
        }

        public static async Task<Dance> GetDancesIdAsync(string id)
        {
            string newUri = DancesUri + "/" + id;
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage responce = await client.GetAsync(newUri);
                if (responce.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new Exception("Id is not found");
                }

                responce.EnsureSuccessStatusCode();
                string content = await responce.Content.ReadAsStringAsync();
                Dance dObj = JsonConvert.DeserializeObject<Dance>(content);
                return dObj;
            }
        }

        static void ShowListObjects(IList<Dance> dList)
        {
            foreach (var dance in dList)
            {
                Console.WriteLine(dance.Name);
                Console.WriteLine(dance.Description);
                Console.WriteLine();
            }
        }

        static void ShowListObject(Dance dance)
        {
            Console.WriteLine(dance.Name);
        }

        static async Task RunAsync()
        {
            var list = await GetStudentsAsync();
            ShowListObjects(list);
            Console.WriteLine();

            Console.WriteLine("To get a dance, write an id: ");
            string id = Console.ReadLine();

            Dance dObj = await GetDancesIdAsync(id);
            ShowListObject(dObj);
        }

        static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
            Console.ReadLine();
        }

    }
}
