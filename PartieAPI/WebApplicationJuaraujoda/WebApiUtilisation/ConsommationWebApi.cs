using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Dto; 

namespace WebApiUtilisation
{
    public class ConsommationWebApi
    {
        public HttpClient Client { get; set; } = new HttpClient();

        private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        /// <summary>
        /// Effectue une requête GET vers une route de l'API et désérialise la réponse en un objet de type T.
        /// </summary>
        /// <typeparam name="T">Le type de l'objet à retourner (par exemple, PlayerDto, List<PlayerDto>, etc.).</typeparam>
        /// <param name="route">La route relative de l'API (par exemple, "/Players", "/Players/1").</param>
        /// <returns>L'objet désérialisé, ou null en cas d'erreur.</returns>
        /// <exception cref="ApiException">Si la requête échoue ou si la désérialisation échoue.</exception>
        public async Task<T?> GetFromRoute<T>(string route)
        {
            try
            {
                Uri uri = new Uri(Client.BaseAddress!, route);

                Debug.WriteLine($"[GET] Appel de l'endpoint: {uri}"); 

                HttpResponseMessage response = await Client.GetAsync(uri);

                Debug.WriteLine($"[GET] Statut HTTP reçu : {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"[GET] Erreur HTTP. Contenu : {errorContent}");
                    throw new Exception($"Request failed with status code {response.StatusCode} for route {route}");
                }

                string content = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"[GET] Contenu brut : {content}");

                var apiResponse = JsonSerializer.Deserialize<ApiResponseDto<T>>(content, _serializerOptions);

                if (apiResponse == null)
                {
                    Debug.WriteLine("[GET] La désérialisation de ApiResponseDto a retourné null.");
                    throw new Exception($"Deserialization of ApiResponseDto returned null for route {route}");
                }

                return apiResponse.Result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GET] Exception : {ex}");
                throw new Exception($"Error when accessing route {route}: {ex.Message}", ex);
            }
        }


        /// <summary>
        /// Effectue une requête POST vers une route de l'API.
        /// </summary>
        /// <typeparam name="TInputDto">Le type de l'objet à envoyer dans le corps de la requête.</typeparam>
        /// <typeparam name="TOutputDto">Le type de l'objet attendu en réponse.</typeparam>
        /// <param name="route">La route relative de l'API.</param>
        /// <param name="item">L'objet à envoyer dans le corps de la requête.</param>
        /// <returns>L'objet désérialisé de la réponse, ou null en cas d'erreur.</returns>
        /// <exception cref="ApiException">Si la requête échoue ou si la désérialisation échoue.</exception>
        public async Task<TOutputDto?> PostItemAsync<TInputDto, TOutputDto>(string route, TInputDto item)
            where TInputDto : class
            where TOutputDto : class
        {
            try
            {
                Uri uri = new Uri(Client.BaseAddress!, route);
                Debug.WriteLine($"[POST] Début de la requête POST vers : {uri} avec un objet de type {typeof(TInputDto)}");

                string json = JsonSerializer.Serialize(item, _serializerOptions);
                Debug.WriteLine($"[POST] JSON sérialisé : {json}");
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await Client.PostAsync(uri, content);
                Debug.WriteLine($"[POST] Statut HTTP reçu : {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"[POST] Erreur HTTP : {errorContent}");
                    throw new Exception($"POST failed with status {response.StatusCode} for route {route}");
                }

                string stringContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"[POST] Contenu de la réponse : {stringContent}");

                var apiResponse = JsonSerializer.Deserialize<ApiResponseDto<TOutputDto>>(stringContent, _serializerOptions);

                if (apiResponse == null)
                {
                    Debug.WriteLine("[POST] La désérialisation de ApiResponseDto a retourné null.");
                    return null; 
                }

                return apiResponse.Result; 
            }
            catch (Exception exc)
            {
                Debug.WriteLine($"[POST] Exception : {exc.Message}");
                throw new Exception($"Error in POST at route {route}: {exc.Message}", exc);
            }
        }

        /// <summary>
        ///  Méthode POST simplifiée lorsque le type d'entrée et de sortie sont identiques.
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="route"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<TDto?> PostItemAsync<TDto>(string route, TDto item) where TDto : class
            => await PostItemAsync<TDto, TDto>(route, item);

        /// <summary>
        ///  Effectue une requête PUT
        /// </summary>
        /// <typeparam name="TInputDto">Le type de donnée à envoyer</typeparam>
        /// <typeparam name="TOutputDto">Le type de donnée attendu</typeparam>
        /// <param name="route">La route</param>
        /// <param name="item">L'item à envoyer</param>
        /// <returns>L'item de retour ou null</returns>
        /// <exception cref="ApiException"></exception>
        public async Task<TOutputDto?> PutItemAsync<TInputDto, TOutputDto>(string route, TInputDto item)
            where TInputDto : class
            where TOutputDto : class
        {
            try
            {
                Debug.WriteLine($"[PUT] Début de la requête PUT vers : {route} avec un objet de type {typeof(TInputDto)}");
                Uri uri = new Uri(Client.BaseAddress!, route);
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
                    throw new Exception($"PUT failed with status {response.StatusCode} for route {route}");
                }
                string stringContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"[PUT] Contenu de la réponse : {stringContent}");
                var apiResponse = JsonSerializer.Deserialize<ApiResponseDto<TOutputDto>>(stringContent, serializerOptions);
                if (apiResponse == null)
                {
                    Debug.WriteLine("[PUT] La désérialisation de ApiResponseDto a retourné null.");
                    return null; 
                }

                return apiResponse.Result; 
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[PUT] Exception : {ex}");
                throw new Exception($"Error in PUT at route {route}: {ex.Message}", ex);
            }
        }

        /// <summary>
        ///  Méthode PUT simplifiée
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="route"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<TDto?> PutItemAsync<TDto>(string route, TDto item) where TDto : class
            => await PutItemAsync<TDto, TDto>(route, item);

        /// <summary>
        ///  Effectue une requête DELETE
        /// </summary>
        /// <param name="route">La route</param>
        /// <returns>true si la requête a réussi, false sinon</returns>
        /// <exception cref="ApiException"></exception>
        public async Task<bool> DeleteItemAsync(string route)
        {
            try
            {
                Debug.WriteLine($"[DELETE] Début de la requête DELETE vers : {route}");
                Uri uri = new Uri(Client.BaseAddress!, route);
                HttpResponseMessage response = await Client.DeleteAsync(uri);
                Debug.WriteLine($"[DELETE] Statut HTTP reçu : {response.StatusCode}");
                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"[DELETE] Erreur HTTP : {errorContent}");
                    throw new Exception($"DELETE failed with status {response.StatusCode} for route {route}");
                }
                Debug.WriteLine("[DELETE] Requête DELETE réussie.");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[DELETE] Exception : {ex}");
                throw new Exception($"Error in DELETE at route {route}: {ex.Message}", ex);
            }
        }
    }
}