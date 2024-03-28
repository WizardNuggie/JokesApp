using JokesApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace JokesApp.Services
{
    public class JokeService
    {
        HttpClient httpClient;
        JsonSerializerOptions options;
        string URL = $@"https://v2.jokeapi.dev/joke/Any";

        public JokeService()
        {
            //http client
            httpClient = new HttpClient();

            //options when doing serialization/deserialization
            options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
        }

        public async Task<Joke> GetRandomJoke()
        {
            Joke j = null;
            var response = await httpClient.GetAsync($"{URL}");
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                j = JsonSerializer.Deserialize<Joke>(jsonString,options);
                var node = JsonNode.Parse(jsonString);
                {
                    if (node["error"].GetValue<bool>() == true)
                    {
                        j = new Joke()
                        {
                            ServiceError = JsonSerializer.Deserialize<ServiceError>(jsonString)
                        };
                    }
                    else
                        if (node["type"].GetValue<string>()== "single")
                            j=JsonSerializer.Deserialize<OneLiner>(jsonString,options);
                        else
                            j=JsonSerializer.Deserialize<TwoPartJoke>(jsonString,options);  
                    
                }

            }
            return j;
        }
        
    }

}



