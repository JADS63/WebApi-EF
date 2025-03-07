using System.Diagnostics;
using System.Net;
using System.Text.Json;
using System.Text;

namespace WebApiUtilisation
{
    public class ConsommationWebApi
    {
        HttpClient Client { get; set; } = new HttpClient();

        public async Task<T?> GetFromRoute<T>(string route)
        {
            try
            {
                Uri uri = new Uri(route, UriKind.RelativeOrAbsolute);

                T? item = default;
                HttpResponseMessage response = await Client.GetAsync(uri);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var serializerOptions = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = true
                    };
                    item = JsonSerializer.Deserialize<T>(content, serializerOptions);
                }
                return item;
            }
            catch
            {
                throw new WebException($"The route {route} seems to be invalid");
            }
        }

        public async Task<TOutputDto?> PostItemAsync<TInputDto, TOutputDto>(string route, TInputDto item) where TInputDto : class where TOutputDto : class
        {
            try
            {
                Uri uri = new Uri($"{route}", UriKind.RelativeOrAbsolute);
                var serializerOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };
                string json = JsonSerializer.Serialize<TInputDto>(item, serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await Client.PostAsync(uri, content);

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"Item of type {item.GetType()} not saved!");
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.BadRequest:
                            throw new WebException($"Bad Request - route: {route}");
                        case HttpStatusCode.Unauthorized:
                            throw new WebException($"Unauthorized - route: {route}");
                    }
                    return null;
                }

                Debug.WriteLine($"Item of type {item.GetType()} successfully saved.");

                string stringContent = await response.Content.ReadAsStringAsync();
                try
                {
                    TOutputDto? data = JsonSerializer.Deserialize<TOutputDto>(stringContent, serializerOptions);
                    return data;
                }
                catch (JsonException)
                {
                    Debug.WriteLine("Problem during Json deserialization");
                    return null;
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
                throw new WebException($"The route {route} seems to be invalid");
            }
        }

        public async Task<TDto?> PostItemAsync<TDto>(string route, TDto item) where TDto : class
                => await PostItemAsync<TDto, TDto>(route, item);
        public async Task<TDto?> PutItemAsync<TDto>(string route, TDto item) where TDto : class
        {
            try
            {
                Uri uri = new Uri($"{route}", UriKind.RelativeOrAbsolute);
                var serializerOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };

                StringContent content = null!;

                if (item != null)
                {
                    string json = JsonSerializer.Serialize<TDto>(item, serializerOptions);
                    content = new StringContent(json, Encoding.UTF8, "application/json");
                }
                HttpResponseMessage response = await Client.PutAsync(uri, content);

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"\tItem (of type {item?.GetType()} not saved!");
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.Unauthorized:
                            throw new WebException($"Unauthorized - route: {route}");
                    }
                    return null;
                }

                Debug.WriteLine(@"\tItem (of type {item.GetType()} successfully saved.");

                string stringContent = await response.Content.ReadAsStringAsync();
                TDto? addedData = JsonSerializer.Deserialize<TDto>(stringContent, serializerOptions);
                return addedData;
            }
            catch
            {
                throw new WebException($"The route {route} seems to be invalid");
            }
        }
        public async Task<bool> DeleteItemAsync(string route)
        {
            try
            {
                Uri uri = new Uri($"{route}", UriKind.RelativeOrAbsolute);

                HttpResponseMessage response = await Client.DeleteAsync(uri);

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"\tItem not deleted!");
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.Unauthorized:
                            throw new WebException($"Unauthorized - route: {route}");
                    }
                    return false;
                }

                Debug.WriteLine(@"\tItem successfully deleted.");

                return true;
            }
            catch
            {
                throw new WebException($"The route {route} seems to be invalid");
            }
        }
    }
}
