// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using System.Net;

namespace DestinyCheck
{
    public class DestinyCheck
    {
        static void Main()
        {
            const string baseUri = "https://www.bungie.net/platform/";

            // Uses JSON.NET - http://www.nuget.org/packages/Newtonsoft.Json
            //using (var client = new HttpClient())
            //{
            //    client.DefaultRequestHeaders.Add("X-API-Key", "7fa7a3ab0ffb44308e112a6ba16ec7c2");

            //    var response = client.GetAsync("https://www.bungie.net/platform/Destiny/Manifest/InventoryItem/1274330687/").Result;
            //    var content = response.Content.ReadAsStringAsync().Result;
            //    dynamic item = Newtonsoft.Json.JsonConvert.DeserializeObject(content);

            //    Console.WriteLine(item.Response.data.inventoryItem.itemName); //Gjallarhorn
            //}


            var prog = new DestinyCheck();
            var result = prog.GetGroup("m9s", baseUri);

            Console.WriteLine(result.Result);

            Console.ReadLine();
        }


        // test


        async Task<string> GetGroup(string groupName, string baseUri)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-API-Key", "7fa7a3ab0ffb44308e112a6ba16ec7c2");

                //HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, ToString());

                var m9Clan = new GroupQuery()
                {
                    name = "Valus Ta'aurc the real m9",
                    groupType = 1,
                    //tagText = groupName
                };

                string json = JsonConvert.SerializeObject(m9Clan);

                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = client.PostAsync(baseUri + "GroupV2/Search/", httpContent).Result;

                var contents = await response.Content.ReadAsStringAsync();


                Root results = JsonConvert.DeserializeObject<Root>(contents);

                //var responseContent = JsonConvert.DeserializeObject<Group>();

                var clanResponse = client.GetAsync(baseUri + "GroupV2/" + results.Response.results.FirstOrDefault().groupId + "/Members/").Result;

                var clan = await clanResponse.Content.ReadAsStringAsync();
                var calnResults = JsonConvert.DeserializeObject<RootUser>(clan);

                var players = calnResults.Response.results;

                foreach (var player in players)
                {
                    //player.destinyUserInfo.membershipId
                }

                return results.Response.results.FirstOrDefault().clanInfo.clanCallsign;
            }

            return null;
        }
    }


    class GroupQuery
    {
        public string name { get; set; }
        public int groupType { get; set; }

        public string? creationDate { get; set; }

        public int? sortBy { get; set; }

        public int? groupMemberCountFilter { get; set; }

        public string? localeFilter { get; set; }

        public string? tagText { get; set; }

        public int? itemsPerPage { get; set; }

        public int? currentPage { get; set; }

        public string? requestContinuationToken { get; set; }
    }

    class groupQueryResult
    {
        public string results { get; set; }
    }

    class Group
    {
        public int groupId { get; set; }

    }


    public class ClanBannerData
    {
        public long decalId { get; set; }
        public long decalColorId { get; set; }
        public long decalBackgroundColorId { get; set; }
        public int gonfalonId { get; set; }
        public long gonfalonColorId { get; set; }
        public int gonfalonDetailId { get; set; }
        public long gonfalonDetailColorId { get; set; }
    }

    public class ClanInfo
    {
        public string clanCallsign { get; set; }
        public ClanBannerData clanBannerData { get; set; }
    }

    public class Result
    {
        public string groupId { get; set; }
        public string name { get; set; }
        public int groupType { get; set; }
        public DateTime creationDate { get; set; }
        public string about { get; set; }
        public string motto { get; set; }
        public int memberCount { get; set; }
        public string locale { get; set; }
        public int membershipOption { get; set; }
        public int capabilities { get; set; }
        public ClanInfo clanInfo { get; set; }
        public string avatarPath { get; set; }
        public string theme { get; set; }
    }

    public class Query
    {
        public string name { get; set; }
        public int groupType { get; set; }
        public int creationDate { get; set; }
        public int sortBy { get; set; }
        public int itemsPerPage { get; set; }
        public int currentPage { get; set; }
    }

    public class Response
    {
        public List<Result> results { get; set; }
        public int totalResults { get; set; }
        public bool hasMore { get; set; }
        public Query query { get; set; }
        public bool useTotalResults { get; set; }
    }

    public class MessageData
    {
    }

    public class Root
    {
        public Response Response { get; set; }
        public int ErrorCode { get; set; }
        public int ThrottleSeconds { get; set; }
        public string ErrorStatus { get; set; }
        public string Message { get; set; }
        public MessageData MessageData { get; set; }
    }
}