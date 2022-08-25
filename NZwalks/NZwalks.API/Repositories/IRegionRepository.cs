using Microsoft.AspNetCore.SignalR;
using NZwalks.API.Models.Domain;

namespace NZwalks.API.Repositories
{
    public interface IRegionRepository
    {
        Task<IEnumerable<Region>> GetAllAsync();
    }
}
