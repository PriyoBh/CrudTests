using System.Net;
using Newtonsoft.Json;
using RestSharp;

namespace CrudAPITests.utils;

public static class ApiHelper
{
    private static string _baseUrl = String.Empty;
    static void SetupBaseUrl()
    {
        var fileName = Path.Join(Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName, "keyLookUp.txt");
        if (!File.Exists(fileName))
            throw new FileNotFoundException();
        _baseUrl = File.ReadAllText(fileName);
    }

    public static async Task<T> GetRestResponse<T>(string url)
    {
        SetupBaseUrl();
        var options = new RestClientOptions(_baseUrl) {
        };
        var client = new RestClient(options);
        var request = new RestRequest(url);
        var response = await client.ExecuteGetAsync(request);
        if (response.StatusCode is HttpStatusCode.BadRequest || response.Content is null)
            throw new HttpRequestException();
        var result = JsonConvert.DeserializeObject<T>(response.Content);
        if (result == null)
            throw new InvalidOperationException("Failed to deserialize the response content.");
        return result;
    }

    public static async Task<RestResponse> PostRestResponse(string url, string body)
    {
        SetupBaseUrl();
        var options = new RestClientOptions(_baseUrl) {
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
        var options = new RestClientOptions(_baseUrl) {
        };
        var client = new RestClient(options);
        var request = new RestRequest(url);
        var response = await client.ExecuteDeleteAsync(request);
        return response;
    }
}