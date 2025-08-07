using Domain.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Repository
{
    /// <summary>
    /// Repositorio genérico con CRUD básico y soporte para Specification Pattern.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad del dominio.</typeparam>
    public interface IBaseRepository<T> where T : class
    {
        // --- Lectura simple ---
        Task<T?> GetByIdAsync(object id);
        Task<IReadOnlyList<T>> ListAllAsync();

        // --- Specification Pattern ---
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
        Task<T?> GetEntityWithSpec(ISpecification<T> spec);
        Task<int> CountAsync(ISpecification<T> spec);

        // --- Creación ---
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);

        // --- Actualización ---
        Task UpdateAsync(T entity);
        Task UpdateRangeAsync(IEnumerable<T> entities);

        // --- Borrado ---
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(IEnumerable<T> entities);

        // --- Persistencia explícita (opcional) ---
        Task<int> SaveChangesAsync();
    }
}