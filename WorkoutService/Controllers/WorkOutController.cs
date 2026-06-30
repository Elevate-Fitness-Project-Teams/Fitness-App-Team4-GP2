using BuildingBlocks.Shared.Controllers;
using BuildingBlocks.Shared.Responses;
using BuildingBlocks.Shared.Results.Pagination;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkoutService.BuildingBlock.HandlersResponse;
using WorkoutService.Domain.Enums;
using WorkoutService.Feature.Browse_Exercise_Library;
using WorkoutService.Feature.Browse_Exercise_Library.Dtos__ViewModels;
using WorkoutService.Feature.BrowseWorkoutPlans;
using WorkoutService.Feature.BrowseWorkoutPlans.Dtos__ViewModels;
using WorkoutService.Feature.GetAllWorkOuts;
using WorkoutService.Feature.GetAllWorkOuts.Dtos__ViewModels;
using WorkoutService.Feature.GetExerciseDetailsById;
using WorkoutService.Feature.GetExerciseDetailsById.Dtos__ViewModels;
using WorkoutService.Feature.GetWorkOutDetails;
using WorkoutService.Feature.GetWorkOutDetails.Dtos__ViewModels;
using WorkoutService.Feature.GetWorkOutPlanDetails;
using WorkoutService.Feature.GetWorkOutPlanDetails.Dtos__ViewModels;
using WorkoutService.Feature.GetWorkOutsByCategoryName;
using WorkoutService.Feature.GetWorkOutsByCategoryName.Dtos__ViewModels;
using WorkoutService.Feature.GetWorkOutsByPlanId;
using WorkoutService.Feature.GetWorkOutsByPlanId.Dtos__ViewModels;

namespace WorkoutService.Controllers
{

    public class WorkOutController : ApiControllerBase
    {
        private readonly IMediator mediator;

        public WorkOutController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("GetWorkOuts")]
        [ProducesResponseType(typeof(WorkOutViewModel), 200)]

        public async Task<IActionResult> GetWorkOuts([FromQuery] int page,
            [FromQuery] int pageSize,
            [FromQuery] string? search,
            [FromQuery] WorkOutCategory? category,
            [FromQuery] WorkOutDiffeculty? difficulty,
            [FromQuery] int? duration,
            CancellationToken cancellationToken)
        {
            var query = new GetAllWorkOutsBySearchQuery(page, pageSize, search, category, difficulty, duration);
            var result = await mediator.Send(query, cancellationToken);
            if (!result.IsSuccess)
            {
                var _errorCode = result.ErrorCode;
                if (_errorCode == HandlerErrorCodesEnum.NotFoundAnyWorkOuts)
                    return NotFound(ResponseFactory.Failure("No WorkOuts Is Found ", 404));
                if (_errorCode == HandlerErrorCodesEnum.InvalidDiffecultyLevel)
                    return NotFound(ResponseFactory.Failure("Invalid Diffecult Level , ", 404));
                if (_errorCode == HandlerErrorCodesEnum.InvalidCategoryName)
                    return NotFound(ResponseFactory.Failure("Invalid Category Name , ", 404));

                return BadRequest();
            }
            var WorkoutsViewModel = result.Data.Select(w => new WorkOutViewModel
            {
                WorkoutId = w.WorkoutId,
                Name = w.Name,
                PlanId = w.PlanId,
                Category = w.Category.ToString(),
                DurationInMinutes = w.DurationInMinutes,
                Difficulty = w.Difficulty.ToString()

            }).ToList();

            return Ok(ResponseFactory.Success(WorkoutsViewModel, "WorkOuts Fetched Successfully", 200));
        }

        [HttpGet("GetWorkOutDetails")]
        [ProducesResponseType(typeof(WorkoutDetailsViewModel), 200)]

        public async Task<IActionResult> GetWorkOuts([FromQuery] int WorkOutId, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetWorkOutDetailsQuery(WorkOutId), cancellationToken);
            if (!result.IsSuccess)
            {
                var _errorCode = result.ErrorCode;
                if (_errorCode == HandlerErrorCodesEnum.NotFoundAnyWorkOuts)
                    return NotFound(ResponseFactory.Failure("No WorkOuts Is Found,Invalid Id ", 404));

                return BadRequest();
            }
            var WorkoutViewModel = new WorkoutDetailsViewModel
            {
                WorkoutId = result.Data.WorkoutId,
                PlanId = result.Data.PlanId,
                WorkoutName = result.Data.WorkoutName,
                WorkoutCategory = result.Data.WorkoutCategory.ToString(),
                WorkoutDifficulty = result.Data.WorkoutDifficulty.ToString(),
                DurationInMinutes = result.Data.DurationInMinutes,
                Exercises = result.Data.Exercises.Select(e => new WorkoutExerciseViewModle
                {
                    ExerciseId = e.ExerciseId,
                    ExerciseName = e.ExerciseName,
                    TargetMuscles = e.TargetMuscles,
                    ExerciseEquipment = e.ExerciseEquipment,
                    ExerciseDifficulty = e.ExerciseDifficulty.ToString(),
                    ExerciseDescription = e.ExerciseDescription,
                    ExerciseVideoUrl = e.ExerciseVideoUrl,
                    SetsDefault = e.SetsDefault,
                    RepsDefault = e.RepsDefault,
                    RestTimeInSeconds = e.RestTimeInSeconds,
                    OrderIndex = e.OrderIndex
                }).ToList()
            };

            return Ok(ResponseFactory.Success(
                WorkoutViewModel,
                "WorkOut Fetched Successfully",
                200));
        }


        [HttpGet("GetWorkOutsByPlanId")]
        [ProducesResponseType(typeof(WorkoutByPlanViewModel), 200)]
        public async Task<IActionResult> GetWorkOutsByPlanId([FromQuery] string PlanId, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetWorkOutsByPlanIdQuery(PlanId), cancellationToken);
            if (!result.IsSuccess)
            {
                var _errorCode = result.ErrorCode;
                if (_errorCode == HandlerErrorCodesEnum.NotFoundAnyWorkOutsForThisPlan)
                    return NotFound(ResponseFactory.Failure("No WorkOuts Is Found For This Plan", 404));
                if (_errorCode == HandlerErrorCodesEnum.PlanIdISNull)
                    return NotFound(ResponseFactory.Failure("The Plain Id is Null Or Empty", 404));

                return BadRequest();
            }
            var WorkoutsViewModel = result.Data.Select(w => new WorkoutByPlanViewModel
            {
                PlanId = w.PlanId,
                WorkoutId = w.WorkoutId,
                WorkoutName = w.WorkoutName,
                WorkoutCategory = w.WorkoutCategory.ToString(),
                WorkoutDifficulty = w.WorkoutDifficulty.ToString(),
                DurationInMinutes = w.DurationInMinutes,
                PlanName = w.PlanName,
                PlanDescription = w.PlanDescription,
                PlanGoal = w.PlanGoal,
                PlanStatus = w.PlanStatus

            }).ToList();

            return Ok(ResponseFactory.Success(WorkoutsViewModel, "WorkOuts Fetched Successfully", 200));
        }
        [HttpGet("GetWorkOutsByCategoryName")]
        [ProducesResponseType(typeof(GetWorkOutsByCategoryNameViewModel), 200)]
        public async Task<IActionResult> GetWorkOutsByCategoryName([FromQuery] WorkOutCategory CategoryName, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetWorkOutsByCategoryNameQuery(CategoryName), cancellationToken);
            if (!result.IsSuccess)
            {
                var _errorCode = result.ErrorCode;
                if (_errorCode == HandlerErrorCodesEnum.NotFoundAnyWorkOutsForThisCategoryName)
                    return NotFound(ResponseFactory.Failure("No WorkOuts Is Found For This Category", 404));
                if (_errorCode == HandlerErrorCodesEnum.InvalidCategoryName)
                    return NotFound(ResponseFactory.Failure("Invalid Category Name, No WorkOuts Is Found For This Category ", 404));

                return BadRequest();
            }
            var WorkoutsViewModel = result.Data.Select(w => new GetWorkOutsByCategoryNameViewModel
            {
                PlanId = w.PlanId,
                WorkoutId = w.WorkoutId,
                WorkoutName = w.WorkoutName,
                WorkoutCategory = w.WorkoutCategory.ToString(),
                WorkoutDifficulty = w.WorkoutDifficulty.ToString(),
                DurationInMinutes = w.DurationInMinutes,
                PlanName = w.PlanName,
                PlanDescription = w.PlanDescription,
                PlanGoal = w.PlanGoal,
                PlanStatus = w.PlanStatus

            }).ToList();

            return Ok(ResponseFactory.Success(WorkoutsViewModel, "WorkOuts Fetched Successfully", 200));
        }

        [HttpGet("GetAllExercies")]
        public async Task<IActionResult> GetAllExercies([FromQuery] int Page, [FromQuery] int PageSize, CancellationToken cancellationToken)
        {

            var paginationRequest = new PaginationRequest(Page, PageSize);
            var result = await mediator.Send(new GetAllExricesQuery(paginationRequest), cancellationToken);
            if (!result.IsSuccess)
            {
                var _errorCode = result.ErrorCode;
                if (_errorCode == HandlerErrorCodesEnum.NotFoundAnyExercise)
                    return NotFound(ResponseFactory.Failure("No Exercises Found", 404));

                return BadRequest();
            }

            var exercisesViewModel = result.Data.Items.Select(e => new ExercisesViewModel
            {
                ExerciseId = e.ExerciseId,
                Name = e.Name,
                Difficulty = e.Difficulty,
                Description = e.Description,
            }).ToList();

            return Ok(ResponseFactory.Success(
                exercisesViewModel,
                "Exercises Fetched Successfully",
                200));
        }
    
        [HttpGet("GetExercieDetails")]
        public async Task<IActionResult> GetExercieDetails([FromQuery] int ExerciseId, CancellationToken cancellationToken)
        {

            var result = await mediator.Send(new GetExerciesDetailsByIdQuery(ExerciseId), cancellationToken);
            if (!result.IsSuccess)
            {
                var _errorCode = result.ErrorCode;
                if (_errorCode == HandlerErrorCodesEnum.NotFoundExerciesWithSpecificID)
                    return NotFound(ResponseFactory.Failure("No Exercise Found with the Specific ID", 404));
                return BadRequest();
            }

            var exercisesViewModel = new ExerciseDetailsViewModel
            {
                ExerciseId = result.Data.ExerciseId,
                Name = result.Data.Name,
                Difficulty = result.Data.Difficulty,
                Description = result.Data.Description,
            };

            return Ok(ResponseFactory.Success(
                exercisesViewModel,
                "Exercises Fetched Successfully",
                200));
        }



        [HttpGet("GetAllWorkOutPlans")]
        public async Task<IActionResult> GetAllWorkOutPlans([FromQuery] int Page, [FromQuery] int PageSize, CancellationToken cancellationToken)
        {

            var paginationRequest = new PaginationRequest(Page, PageSize);
            var result = await mediator.Send(new GetWorkOutsPlansQuery(paginationRequest), cancellationToken);
            if (!result.IsSuccess)
            {
                var _errorCode = result.ErrorCode;
                if (_errorCode == HandlerErrorCodesEnum.NotFoundAnyExercise)
                    return NotFound(ResponseFactory.Failure("No WorkOut Plans Found", 404));
                return BadRequest();
            }

            var workOutPlansViewModel = result.Data.Items.Select(e => new WorkOutPlansViewModel
            {
               PlanId= e.PlanId,
                Name= e.Name,
                Goal= e.Goal,
            }).ToList();

            return Ok(ResponseFactory.Success(
                workOutPlansViewModel,
                "WorkOut Plans Fetched Successfully",
                200));
        }


        [HttpGet("GetWorkOutPlanDetails")]
        public async Task<IActionResult> GetWorkOutPlanDetails([FromQuery] string PlanId, CancellationToken cancellationToken)
        {

            var result = await mediator.Send(new GetWorkOutPlanDetailsQuery(PlanId), cancellationToken);
            if (!result.IsSuccess)
            {
                var _errorCode = result.ErrorCode;
                if (_errorCode == HandlerErrorCodesEnum.NotFoundPlanWithSpecificID)
                    return NotFound(ResponseFactory.Failure("No WorkOut Plan Found with the Specific ID", 404));
                return BadRequest();
            }

            var workOutPlanDetailsViewModel = new WorkOutPlanViewModel
            {
                PlanId = result.Data.PlanId,
                Name = result.Data.Name,
                Description = result.Data.Description,
                Goal = result.Data.Goal,
                Status = result.Data.Status,
                Difficulty = result.Data.Difficulty.ToString()
            };

            return Ok(ResponseFactory.Success(
                workOutPlanDetailsViewModel,
                "WorkOut Plan Details Fetched Successfully",
                200));
        }


    }
}
