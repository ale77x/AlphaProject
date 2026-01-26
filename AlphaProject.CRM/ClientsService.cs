using AlphaProject.Shared.Dtos; // modello condiviso oppure definire un DTO identico
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;


namespace AlphaProject.CRM
{
    
    public class ClientsService
    {
        private readonly HttpClient _http;
        public ClientsService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiWithUserToken");
        }

        private const string Url = "api/clients";

        public async Task<List<ClientDto>> GetClientsAsync()
        {
            var response = await _http.GetAsync(Url);

            // Se la risposta è 401/403, LogoutHandler avrà già cancellato il cookie.
            // Restituisci una lista vuota (o null) senza lanciare eccezioni.
            if (response.StatusCode == HttpStatusCode.Unauthorized ||
                response.StatusCode == HttpStatusCode.Forbidden)
            {
                return new List<ClientDto>();
            }

            // Per altri codici di errore puoi scegliere di rilanciare o gestire diversamente.
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<ClientDto>>() ?? new();

        }
            //await _http.GetFromJsonAsync<List<ClientDto>>(Url) ?? new();

        public async Task<ClientDto?> GetClientAsync(int id) =>
            await _http.GetFromJsonAsync<ClientDto>($"{Url}/{id}");

        public async Task<bool> CreateClientAsync(ClientDto client)
        {
            var response = await _http.PostAsJsonAsync(Url, client);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateClientAsync(int id, ClientDto client)
        {
            var response = await _http.PutAsJsonAsync($"{Url}/{id}", client);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteClientAsync(int id)
        {
            var response = await _http.DeleteAsync($"{Url}/{id}");
            return response.IsSuccessStatusCode;
        }
    }


}
