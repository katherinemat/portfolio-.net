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
        public string Name { get; set; }
        public string Html_Url { get; set; }
        public string Description { get; set; }
        public string Updated_At { get; set; }

        public static List<Project> GetProjects()
        {
            var client = new RestClient("https://api.github.com");

            var request = new RestRequest("/users/katherinemat/starred", Method.GET);

            request.AddHeader("Accept", "application/vnd.github.v3+json");
            request.AddHeader("User-Agent", "katherinemat");
            //request.AddHeader("Authorization", "token d7a7ca8b2f1d43c2b39c771f28ae9c6dbf8ed70d");

            var response = new RestResponse();

            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();

            //datatype is JArray, opposed to JObject because query returns array instead of object
            JArray projectArray = JsonConvert.DeserializeObject<JArray>(response.Content);

            //converting into a string is like converting into json, which is easier to manipulate
            string jsonOutput = projectArray.ToString();
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
