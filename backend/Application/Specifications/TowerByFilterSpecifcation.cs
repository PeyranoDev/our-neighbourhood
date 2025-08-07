using Application.Schemas.Requests;
using Domain.Common;
using Domain.Entities;
using System;
using System.Linq.Expressions;

namespace Application.Specifications
{
    /// <summary>
    /// Filtra, ordena e incluye Apartments en Tower.
    /// </summary>
    public class TowerByFilterSpecification : BaseSpecification<Tower>
    {
        public TowerByFilterSpecification(TowerFilterParams filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.Name))
                AddCriteria(t => t.Name.Contains(filter.Name));

            AddInclude(t => t.Apartments!);

            if (!string.IsNullOrWhiteSpace(filter.SortBy) &&
                filter.SortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
            {
                if (filter.SortOrder?.Equals("desc", StringComparison.OrdinalIgnoreCase) == true)
                    ApplyOrderByDescending(t => t.Name!);
                else
                    ApplyOrderBy(t => t.Name!);
            }
            else
            {
                ApplyOrderBy(t => t.Name!);
            }

            // 4) Paginación
            var skip = (filter.PageNumber - 1) * filter.PageSize;
            ApplyPaging(skip, filter.PageSize);
        }
    }
}