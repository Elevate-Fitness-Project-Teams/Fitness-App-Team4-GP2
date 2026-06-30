using BuildingBlocks.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProgressService.Domain.Contracts;
using ProgressService.Domain.Entities;
using static ProgressService.Features.ViewWeightHistory.WeightHistoryResponse;

namespace ProgressService.Features.ViewWeightHistory
{
    public class ViewWeightHistoryHandler : IRequestHandler<ViewWeightHistoryQuery, Result<WeightHistoryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ViewWeightHistoryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<WeightHistoryResponse>> Handle(ViewWeightHistoryQuery request, CancellationToken cancellationToken)
        {
            var weightHistory = await _unitOfWork.GetRepository<WeightHistory,int>().GetAll(x => x.UserId == request.UserId)
                .OrderBy(x=>x.Date).Select(x=> new WeightEntry
                {
                    Weight = x.Weight,
                    Date = x.Date,
                    Notes = x.Notes

                }).ToListAsync();

            return new WeightHistoryResponse
            {
                WeightEntries = weightHistory
            };



        }
    }
}
