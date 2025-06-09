using System.Net;
using Newtonsoft.Json;
using RestSharp;

namespace DefaultNamespace;

public static class ApiHelper
{
    private static string baseUrl;
    static void SetupBaseUrl()
    {
        var fileName = Path.Join(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,"keyLookUp.txt");
        if (!File.Exists(fileName))
            throw new FileNotFoundException();
        baseUrl = File.ReadAllText(fileName);
    }

    public static async Task<T> GetRestResponse<T>(string url)
    {
        SetupBaseUrl();
        var options = new RestClientOptions(baseUrl) {
        };
        var client = new RestClient(options);
        var request = new RestRequest(url);
        var response = await client.ExecuteGetAsync(request);
        if (response.StatusCode is HttpStatusCode.BadRequest || response.Content is null)
            throw new HttpRequestException();
        return JsonConvert.DeserializeObject<T>(response.Content);
    }

    public static async Task<RestResponse> PostRestResponse(string url, string body)
    {
        SetupBaseUrl();
        var options = new RestClientOptions(baseUrl) {
        };
        var client = new RestClient(options);
        var request = new RestRequest(url, Method.Post);
        request.AddBody(body);
        var response = await client.ExecutePostAsync(request);
        return response;
    }

    public static async Task<RestResponse> DeleteRestResponse(string url)
    {
        SetupBaseUrl();
        var options = new RestClientOptions(baseUrl) {
        };
        var client = new RestClient(options);
        var request = new RestRequest(url);
        var response = await client.ExecuteDeleteAsync(request);
        return response;
    }
}