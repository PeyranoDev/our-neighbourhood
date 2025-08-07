using Domain.Entities;

namespace Domain.Repository
{
    public interface IRequestRepository
    {
        Task<bool> AddAsync(Request request);
        IQueryable<Request> GetAsQueryable();
        Task<bool> UpdateAsync(Request request);
        Task<Request?> GetLatestByVehicleAsync(int vehicleId);
        Task<bool> DeleteOldRequestsAsync(TimeSpan olderThan);
    }
}