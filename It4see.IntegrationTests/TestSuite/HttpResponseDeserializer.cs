using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace It4see.IntegrationTests.TestSuite;

public static class HttpResponseDeserializer
{
    public static async Task<T?> DeserializeAsync<T>(this HttpResponseMessage httpResponseMessage)
    {
        httpResponseMessage.EnsureSuccessStatusCode();

        var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<T>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }
}
