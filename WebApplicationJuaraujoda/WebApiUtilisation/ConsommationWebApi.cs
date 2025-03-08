using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApiUtilisation
{
    public class ConsommationWebApi
    {
        public HttpClient Client { get; set; } = new HttpClient();

        public async Task<T?> GetFromRoute<T>(string route)
        {
            try
            {
                Debug.WriteLine($"[GET] Début de la requête vers : {route}");
                Uri uri = new Uri(route, UriKind.RelativeOrAbsolute);
                HttpResponseMessage response = await Client.GetAsync(uri);
                Debug.WriteLine($"[GET] Statut HTTP reçu : {response.StatusCode}");
                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"[GET] Erreur HTTP. Contenu : {errorContent}");
                    throw new WebException($"Request failed with status code {response.StatusCode} for route {route}");
                }
                string content = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"[GET] Contenu brut : {content}");
                string trimmedContent = content.Trim();
                Debug.WriteLine($"[GET] Contenu après trim : {trimmedContent}");

                // Si T est une collection et que le contenu semble vide ou ne contient pas "result"
                if (IsCollectionType(typeof(T)) &&
                    (string.IsNullOrWhiteSpace(trimmedContent) ||
                     trimmedContent == "{}" ||
                     (trimmedContent.StartsWith("{") && !trimmedContent.ToLower().Contains("\"result\""))))
                {
                    Debug.WriteLine("[GET] Le contenu est remplacé par '[]' car il semble vide pour un type collection.");
                    content = "[]";
                }

                var serializerOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };

                T? result = JsonSerializer.Deserialize<T>(content, serializerOptions);
                if (result == null)
                {
                    Debug.WriteLine("[GET] La désérialisation a retourné null.");
                    throw new WebException($"Deserialization returned null for route {route}");
                }
                Debug.WriteLine($"[GET] Désérialisation réussie. Type : {typeof(T)}");
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GET] Exception : {ex}");
                throw new WebException($"Error when accessing route {route}: {ex.Message}");
            }
        }

        private bool IsCollectionType(Type type)
        {
            return type != typeof(string) && typeof(System.Collections.IEnumerable).IsAssignableFrom(type);
        }

        public async Task<TOutputDto?> PostItemAsync<TInputDto, TOutputDto>(string route, TInputDto item)
            where TInputDto : class
            where TOutputDto : class
        {
            try
            {
                Debug.WriteLine($"[POST] Début de la requête POST vers : {route} avec un objet de type {typeof(TInputDto)}");
                Uri uri = new Uri(route, UriKind.RelativeOrAbsolute);
                var serializerOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };
                string json = JsonSerializer.Serialize<TInputDto>(item, serializerOptions);
                Debug.WriteLine($"[POST] JSON sérialisé : {json}");
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await Client.PostAsync(uri, content);
                Debug.WriteLine($"[POST] Statut HTTP reçu : {response.StatusCode}");
                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"[POST] Erreur HTTP : {errorContent}");
                    throw new WebException($"POST failed with status {response.StatusCode} for route {route}");
                }
                string stringContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"[POST] Contenu de la réponse : {stringContent}");
                try
                {
                    TOutputDto? data = JsonSerializer.Deserialize<TOutputDto>(stringContent, serializerOptions);
                    Debug.WriteLine("[POST] Désérialisation réussie.");
                    return data;
                }
                catch (JsonException jsonEx)
                {
                    Debug.WriteLine($"[POST] Erreur de désérialisation JSON : {jsonEx}");
                    return null;
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine($"[POST] Exception : {exc.Message}");
                throw new WebException($"Error in POST at route {route}: {exc.Message}");
            }
        }

        public async Task<TDto?> PostItemAsync<TDto>(string route, TDto item) where TDto : class
            => await PostItemAsync<TDto, TDto>(route, item);

        public async Task<TOutputDto?> PutItemAsync<TInputDto, TOutputDto>(string route, TInputDto item)
            where TInputDto : class
            where TOutputDto : class
        {
            try
            {
                Debug.WriteLine($"[PUT] Début de la requête PUT vers : {route} avec un objet de type {typeof(TInputDto)}");
                Uri uri = new Uri(route, UriKind.RelativeOrAbsolute);
                var serializerOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };
                string json = JsonSerializer.Serialize<TInputDto>(item, serializerOptions);
                Debug.WriteLine($"[PUT] JSON sérialisé : {json}");
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await Client.PutAsync(uri, content);
                Debug.WriteLine($"[PUT] Statut HTTP reçu : {response.StatusCode}");
                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"[PUT] Erreur HTTP : {errorContent}");
                    throw new WebException($"PUT failed with status {response.StatusCode} for route {route}");
                }
                string stringContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"[PUT] Contenu de la réponse : {stringContent}");
                TOutputDto? data = JsonSerializer.Deserialize<TOutputDto>(stringContent, serializerOptions);
                Debug.WriteLine("[PUT] Désérialisation réussie.");
                return data;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[PUT] Exception : {ex}");
                throw new WebException($"Error in PUT at route {route}: {ex.Message}");
            }
        }

        public async Task<TDto?> PutItemAsync<TDto>(string route, TDto item) where TDto : class
            => await PutItemAsync<TDto, TDto>(route, item);

        public async Task<bool> DeleteItemAsync(string route)
        {
            try
            {
                Debug.WriteLine($"[DELETE] Début de la requête DELETE vers : {route}");
                Uri uri = new Uri(route, UriKind.RelativeOrAbsolute);
                HttpResponseMessage response = await Client.DeleteAsync(uri);
                Debug.WriteLine($"[DELETE] Statut HTTP reçu : {response.StatusCode}");
                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"[DELETE] Erreur HTTP : {errorContent}");
                    throw new WebException($"DELETE failed with status {response.StatusCode} for route {route}");
                }
                Debug.WriteLine("[DELETE] Requête DELETE réussie.");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[DELETE] Exception : {ex}");
                throw new WebException($"Error in DELETE at route {route}: {ex.Message}");
            }
        }
    }
}
