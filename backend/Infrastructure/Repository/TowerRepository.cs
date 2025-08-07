// Infrastructure/Repository/TowerRepository.cs
using Domain.Entities;
using Domain.Repository;
using Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class TowerRepository
        : BaseRepository<Tower>, ITowerRepository
    {
        public TowerRepository(AqualinaAPIContext context)
            : base(context)
        { }

        public Task<Tower?> GetByNameAsync(string name)
            => _dbSet
                .FirstOrDefaultAsync(t => t.Name.ToLower() == name.ToLower());
    }
}
