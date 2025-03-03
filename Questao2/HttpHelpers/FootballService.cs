using System;
using System.Text.Json;
using System.Web;
using Questao2.DTOs;

namespace Questao2.HttpHelpers
{
    internal sealed class FootballService
    {
        private static readonly HttpClient _httpClient = new(); 

        public static async Task<FootballDataDTO> GetFootballMatchesAsync(Dictionary<string, string> parameters)
        {
            Uri url = new("https://jsonmock.hackerrank.com/api/football_matches");

            foreach (var param in parameters)
            {
                url = AddParameter(url, param.Key, param.Value);
            }

            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<FootballDataDTO>(responseStream);
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred: { ex.Message}");
            }
        }

        private static Uri AddParameter(Uri url, string paramName, string paramValue)
        {
            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query[paramName] = paramValue;
            uriBuilder.Query = query.ToString();
            return uriBuilder.Uri;
        }
    }
}
