using JokesApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JokesApp.Services
{
    public class JokeService
    {
        
        HttpClient httpClient;//אובייקט לשליחת בקשות וקבלת תשובות מהשרת

        public JsonSerializerOptions options;//פרמטרים שישמשו אותנו להגדרות הjson
        
       public const string URL = $@"https://v2.jokeapi.dev/";//כתובת השרת

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
        public async Task<List<string>> GetCatsAsync()
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync($"{URL}categories");
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                try
                {
                    Categories cat = JsonSerializer.Deserialize<Categories>(jsonString, options);
                    List<string> cats = cat.categories.ToList();
                    return cats;
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        public async Task<Joke> GetRandomJoke()
        {
            Joke j = null;
            HttpResponseMessage response = await httpClient.GetAsync($"{URL}joke/Any?type=single?safe-mode");

            //if(response.StatusCode==System.Net.HttpStatusCode.OK)
           
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
               
                j = JsonSerializer.Deserialize<Joke>(jsonString,options);


                JsonNodeOptions nodeOptions = new JsonNodeOptions() { PropertyNameCaseInsensitive = true };
                JsonNode node = JsonNode.Parse(jsonString,nodeOptions);
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
        
        public async Task<bool> SubmitJokeAsync(MyJoke j)
        {
            JsonSerializerOptions options= new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase } ;
            
            //serialize the object
            string jsonString=JsonSerializer.Serialize(j,options);//serialize
            
            //insert it into a HttpString Container
            StringContent content = new StringContent(jsonString,Encoding.UTF8,"application/json");
            //send it to the server
            var response = await httpClient.PostAsync($"{URL}Submit?dry-run", content);
            
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
                return true;
            else if(response.StatusCode== System.Net.HttpStatusCode.BadRequest)
            return false;
            return false;

        }
    }


}



