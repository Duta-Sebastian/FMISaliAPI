using FMISaliAPI.Data;
using FMISaliAPI.Models;
using Microsoft.EntityFrameworkCore;
using static System.Enum;

namespace FMISaliAPI.Services
{
    public static class FacilityService
    {
        public static Task<List<string>> GetFacilityTypesTask()
        {
            return Task.FromResult(GetValues<FacilityType>()
                .Select(ft => ft.ToString())
                .ToList());
        }
    }
}
