using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Valumni
{
    public static class SlackStats
    {
        private static readonly HttpClient Client = new HttpClient();

        [FunctionName("SlackStats")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req, ILogger log)
        {
            var token = Environment.GetEnvironmentVariable("SlackApiToken");

            var response = await Client.GetAsync("https://slack.com/api/users.list?token=" + token);
            var content = await response.Content.ReadAsStringAsync();

            var result = JObject.Parse(content);
            var memberCount = result["members"].Count() - 1; // slackbot

            return new OkObjectResult(memberCount);
        }
    }
}