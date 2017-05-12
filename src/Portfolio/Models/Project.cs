using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Portfolio.Models
{
    public class Project
    {
        public string sid { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string Body { get; set; }
        public string Status { get; set; }
        public string[] ToArray { get; set; }

        public static List<Project> GetProjects()
        {
            var client = new RestClient("https://api.github.com");

            var request = new RestRequest("/access_token=d7a7ca8b2f1d43c2b39c771f28ae9c6dbf8ed70d/users/katherinemat/repos", Method.GET);

            request.AddHeader("User-Agent", "katherinemat");

            client.Authenticator = new HttpBasicAuthenticator("katherinemat", "d7a7ca8b2f1d43c2b39c771f28ae9c6dbf8ed70d");

            var response = new RestResponse();

            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();

            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);

            string jsonOutput = jsonResponse.ToString();

            var projectList = JsonConvert.DeserializeObject<List<Project>>(jsonOutput);

            return projectList;
        }

        public static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();
            theClient.ExecuteAsync(theRequest, response => {
                tcs.SetResult(response);
            });
            return tcs.Task;
        }
    }
}
