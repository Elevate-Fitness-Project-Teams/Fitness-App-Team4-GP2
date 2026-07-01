using FCEService.Domain.Entities;
using FCEService.Features.CalculateMetrics;
using FCEService.Features.SaveFitnessStats;
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

         
            config.NewConfig<CalculatedMetric, CalculateMetricsResponse>()
                  .Map(dest => dest.Status, src => src.Status.ToString());

           
        }
    }
}
