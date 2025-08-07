using Application.Schemas.Requests;
using Domain.Common;
using Domain.Entities;
using System;
using System.Linq.Expressions;

namespace Application.Specifications
{
    /// <summary>
    /// Especificación para filtrar, ordenar e incluir Users.
    /// </summary>
    public class UserByFilterSpecification : BaseSpecification<User>
    {
        public UserByFilterSpecification(
            UserFilterParams filters,
            PaginationParams paging)
        {
            if (!string.IsNullOrWhiteSpace(filters.Name))
                AddCriteria(u => u.Name.Contains(filters.Name));
            if (!string.IsNullOrWhiteSpace(filters.Email))
                AddCriteria(u => u.Email.Contains(filters.Email));
            if (filters.IsActive.HasValue)
                AddCriteria(u => u.IsActive == filters.IsActive.Value);
            if (filters.TowerId.HasValue)
                AddCriteria(u => u.Apartment != null &&
                                 u.Apartment.TowerId == filters.TowerId.Value);

            AddInclude(u => u.Role);
            AddInclude(u => u.Apartment!);
            AddInclude(u => u.Apartment!.Tower);

            if (!string.IsNullOrWhiteSpace(paging.SortBy))
            {
                Expression<Func<User, object>> orderExpr = paging.SortBy.ToLower() switch
                {
                    "email" => u => u.Email!,
                    "role" => u => u.Role.Type,
                    _ => u => u.Name!
                };
                if (paging.SortOrder?.ToLower() == "desc")
                    ApplyOrderByDescending(orderExpr);
                else
                    ApplyOrderBy(orderExpr);
            }
            else
            {
                ApplyOrderBy(u => u.Name!);
            }

            var skip = (paging.PageNumber - 1) * paging.PageSize;
            ApplyPaging(skip, paging.PageSize);
        }
    }
}
