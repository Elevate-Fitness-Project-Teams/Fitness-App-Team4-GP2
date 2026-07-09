using FCEService.Domain.Entities;
using FCEService.Features.CalculateMetrics;
using FCEService.Features.GetFitnessMetrics;
using FCEService.Features.GetFitnessStats;
using FCEService.Features.SaveFitnessStats;
using FCEService.Features.Shared.Dtos;
using Mapster;

namespace FCEService.Common.Mappings
{
    public sealed class MappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
           
            config.NewConfig<UserFitnessStat, SubmitFitnessStatsResponse>()
                  .Map(dest => dest.Gender, src => src.Gender.ToString())
                  .Map(dest => dest.Goal, src => src.Goal.ToString())
                  .Map(dest => dest.ActivityLevel, src => src.ActivityLevel.ToString());

            config.NewConfig<UserFitnessStatDto, GetFitnessStatsResponse>()
                 .Map(dest => dest.Gender, src => src.Gender.ToString())
                 .Map(dest => dest.Goal, src => src.Goal.ToString())
                 .Map(dest => dest.ActivityLevel, src => src.ActivityLevel.ToString());


            config.NewConfig<CalculatedMetricDto, CalculateMetricsResponse>()
                  .Map(dest => dest.Status, src => src.Status.ToString());

            config.NewConfig<CalculatedMetricDto, GetFitnessMetricsResponse>()
                 .Map(dest => dest.Status, src => src.Status.ToString());

        }
    }
}
