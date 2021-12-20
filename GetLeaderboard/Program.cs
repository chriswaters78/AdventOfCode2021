// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;


var address = "https://adventofcode.com/2021/leaderboard/private/view/353270.json";
using HttpClientHandler handler = new HttpClientHandler();
handler.CookieContainer = new CookieContainer();
using HttpClient client = new HttpClient(handler);
handler.CookieContainer.Add(new Uri(address), new Cookie("session", "XXX"));

var o = JObject.Parse(await client.GetStringAsync("https://adventofcode.com/2021/leaderboard/private/view/353270.json"));
var score = o.SelectToken("members.353270.local_score").Value<int>();
