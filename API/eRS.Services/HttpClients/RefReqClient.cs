using eRS.Models.HttpClient.Responses;
using Microsoft.CodeAnalysis.CSharp;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace eRS.Services.HttpClients;

public class RefReqClient
{
    private readonly HttpClient HttpClient = new HttpClient();

    private readonly JsonSerializerOptions serializerOptions = new(JsonSerializerDefaults.Web)
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public async Task<RefReqProperties?> GetRefReqDetails(string url)
    {
        var httpResponse = await this.HttpClient.GetAsync(url);
        var result = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception($"Response for the requested URI: {url} did not return successfully. Status Code: {httpResponse.StatusCode}");
        }

        if (httpResponse.StatusCode == HttpStatusCode.NoContent)
        {
            return default;
        }

        try
        {
            var data = JsonSerializer.Deserialize<RefReqProperties>(result, serializerOptions);

            return data == null ? default : data;
        }
        catch (JsonException)
        {
            return default;
        }
        
    }
}
