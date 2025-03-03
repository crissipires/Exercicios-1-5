using Newtonsoft.Json;
using Questao2.DTOs;
using Questao2.HttpHelpers;

public class Program
{
    public static async Task Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = await GetTotalScoredGoals(teamName, year);

        Console.WriteLine("Team "+ teamName +" scored "+ totalGoals.ToString() + " goals in "+ year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = await GetTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public static async Task<int> GetTotalScoredGoals(string team, int year)
    {
        if(string.IsNullOrEmpty(team))
        {
            throw new ArgumentException("Team name is required");
        }

        var totalTeam = 0;

        var dicTeam1 = new Dictionary<string, string>
        {
            { "year", year.ToString() },
            { "team1", team }
        };

        var dicTeam2 = new Dictionary<string, string>
        {
            { "year", year.ToString() },
            { "team2", team }
        };

        totalTeam += await GetTotalScoredByTeam(dicTeam1, EnumTeam.team1);
        totalTeam += await GetTotalScoredByTeam(dicTeam2, EnumTeam.team2);

        return totalTeam;
    }

    private static async Task<int> GetTotalScoredByTeam(Dictionary<string, string> parameters, EnumTeam enumTeam)
    {
        var totalPage = 1;
        var page = 1;
        var totalTeam = 0;

        while (page <= totalPage)
        {
            var footballData = await FootballService.GetFootballMatchesAsync(parameters);

            totalTeam += footballData.Data.Sum(x => x.GetTeamGoals(enumTeam));
            totalPage = footballData.Total_pages;

            var nextPage = footballData.Page + 1;
            parameters["page"] = nextPage.ToString();
            page++;
        }

        return totalTeam;
    }
}