using Application.Schemas.Requests;
using Domain.Common;
using Domain.Entities;
using System;
using System.Linq.Expressions;

namespace Application.Specifications
{
    /// <summary>
    /// Encapsula filtros, includes, orden y paging para Vehicle.
    /// </summary>
    public class VehicleByFilterSpecification : BaseSpecification<Vehicle>
    {
        public VehicleByFilterSpecification(VehicleFilterParams filters, PaginationParams paging)
        {
            // 1) Criterios dinámicos
            if (!string.IsNullOrWhiteSpace(filters.Plate))
                AddCriteria(v => v.Plate.Contains(filters.Plate!));
            if (!string.IsNullOrWhiteSpace(filters.Model))
                AddCriteria(v => v.Model.Contains(filters.Model!));
            if (filters.IsActive.HasValue)
                AddCriteria(v => v.IsActive == filters.IsActive.Value);
            if (filters.HasRequests.HasValue)
            {
                if (filters.HasRequests.Value)
                    AddCriteria(v => v.Requests.Any());
                else
                    AddCriteria(v => !v.Requests.Any());
            }

            // 2) Includes
            if (filters.IncludeRequests)
                AddInclude(v => v.Requests!);

            // 3) Orden dinámico
            if (!string.IsNullOrWhiteSpace(paging.SortBy))
            {
                Expression<Func<Vehicle, object>> orderExpr = paging.SortBy.ToLower() switch
                {
                    "model" => v => v.Model!,
                    "isactive" => v => v.IsActive,
                    _ => v => v.Plate!
                };
                if (paging.SortOrder?.ToLower() == "desc")
                    ApplyOrderByDescending(orderExpr);
                else
                    ApplyOrderBy(orderExpr);
            }
            else
            {
                ApplyOrderBy(v => v.Plate!);
            }

            // 4) Paginación
            var skip = (paging.PageNumber - 1) * paging.PageSize;
            ApplyPaging(skip, paging.PageSize);
        }
    }
}