using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Questao2.DTOs
{
    internal class ResultGamesDTO
    {
        [JsonPropertyName("competition")]
        public string Competition { get; set; }

        [JsonPropertyName("year")]
        public int Year { get; set; }

        [JsonPropertyName("round")]
        public string Round { get; set; }

        [JsonPropertyName("team1")]
        public string Team1 { get; set; }

        [JsonPropertyName("team2")]
        public string Team2 { get; set; }

        [JsonPropertyName("team1goals")]
        public string Team1goals { get; set; }

        [JsonPropertyName("team2goals")]
        public string Team2goals { get; set; }

        public int GetTeamGoals(EnumTeam team)
        {
            if (team == EnumTeam.team1)
            {
                int.TryParse(Team1goals, out int intGoals1);
                return intGoals1;
            }

            int.TryParse(Team2goals, out int intGoals2);
            return intGoals2;
        }
    }

    public enum EnumTeam
    {
        team1 = 1,
        team2 = 2
    }
}
