using UltimateERP.Application.Interfaces;
using UltimateERP.Application.Features.Setup.DTOs;

namespace UltimateERP.Application.Features.Setup.Services;

public interface IBranchFilterService
{
    IQueryable<T> ApplyBranchFilter<T>(IQueryable<T> query, BranchFilterDto filter) where T : Domain.Common.BaseEntity;
}

public class BranchFilterService : IBranchFilterService
{
    public IQueryable<T> ApplyBranchFilter<T>(IQueryable<T> query, BranchFilterDto filter) where T : Domain.Common.BaseEntity
    {
        if (filter.BranchId.HasValue)
            query = query.Where(e => e.BDId == filter.BranchId.Value);

        return query;
    }
}
