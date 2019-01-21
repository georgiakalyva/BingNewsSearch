using System;
using System.Text;
using System.Net;
using System.IO;
using System.Collections.Generic;
/// <summary>
/// This sample is the code presented in this tutorial by CodeStories.gr
/// Tutorial Link: http://www.codestories.gr/?p=450&preview=true
/// For more tutorials and news find me: 
/// Blog: http://www.codestories.gr
/// Facebook: https://www.facebook.com/codestoriesgr/
/// Twitter: https://twitter.com/GeorgiaKalyva
/// LinkedIn: https://www.linkedin.com/in/georgiakalyva
/// </summary>
namespace BingNewsSearch
{
    class Program
    {
        const string subscriptionKey = "enter your key here";
        const string uriBase = "https://api.cognitive.microsoft.com/bing/v7.0/news/search";
        const string searchTerm = "Microsoft";

        struct SearchResult
        {
            public String jsonResult;
            public Dictionary<String, String> relevantHeaders;
        }
        static void Main(string[] args)
        {
            SearchResult result = BingNewsSearch(searchTerm);
            //deserialize the JSON response
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(result.jsonResult);
            Console.WriteLine(jsonObj["value"][0]);
            Console.ReadKey();
        }
        static SearchResult BingNewsSearch(string toSearch)
        {
            var uriQuery = uriBase + "?q=" + Uri.EscapeDataString(toSearch);

            WebRequest request = WebRequest.Create(uriQuery);
            request.Headers["Ocp-Apim-Subscription-Key"] = subscriptionKey;
            HttpWebResponse response = (HttpWebResponse)request.GetResponseAsync().Result;
            string json = new StreamReader(response.GetResponseStream()).ReadToEnd();

            // Create the result object for return
            var searchResult = new SearchResult()
            {
                jsonResult = json,
                relevantHeaders = new Dictionary<String, String>()
            };

            // Extract Bing HTTP headers
            foreach (String header in response.Headers)
            {
                if (header.StartsWith("BingAPIs-") || header.StartsWith("X-MSEdge-"))
                    searchResult.relevantHeaders[header] = response.Headers[header];
            }
            return searchResult;
        }
    }
}
