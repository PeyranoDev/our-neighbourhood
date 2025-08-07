using Domain.Entities;

namespace Domain.Repository
{
    /// <summary>
    /// Repositorio de Tower: hereda el genérico con Specification Pattern
    /// y añade sólo lo específico de Tower.
    /// </summary>
    public interface ITowerRepository : IBaseRepository<Tower>
    {
        Task<Tower?> GetByNameAsync(string name);
    }
}