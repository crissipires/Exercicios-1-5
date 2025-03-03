using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Questao2.DTOs
{
    internal class FootballDataDTO
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("per_page")]
        public int Per_page { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("total_pages")]
        public int Total_pages { get; set; }

        [JsonPropertyName("data")]
        public List<ResultGamesDTO> Data { get; set; }

    }
  
}
