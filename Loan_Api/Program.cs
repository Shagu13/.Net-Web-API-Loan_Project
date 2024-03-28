using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Loan_Api
{
    public class Program
    {
        //public static void Main(string[] args)
        //{
        //    CreateHostBuilder(args).Build().Run();

        //    Task.Run(async () =>
        //    {
        //        string baseUrl = "https://localhost:44341/api/Accountant/save";
        //        string token = "yasdasdasdjhadskjasdasd";

        //        using (var client = new HttpClient())
        //        {
        //            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        //            var response = await client.PostAsync(baseUrl, null);

        //            Console.WriteLine(response.StatusCode);
        //            Console.WriteLine(await response.Content.ReadAsStringAsync());
        //        }
        //    }).Wait();
        //}
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
