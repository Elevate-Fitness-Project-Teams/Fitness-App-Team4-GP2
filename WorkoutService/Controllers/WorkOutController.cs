using BuildingBlocks.Shared.Controllers;
using BuildingBlocks.Shared.Responses;
using BuildingBlocks.Shared.Results.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
using WorkoutService.Feature.StartWorkOutSession;
using WorkoutService.Feature.StartWorkOutSession.Dtos__ViewModels;

namespace WorkoutService.Controllers
{
    [Authorize]
    [Route("api/v1")]
    public class WorkOutController : ApiControllerBase
    {
        private readonly IMediator mediator;

        public WorkOutController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        // Operation 1: Browse workouts — GET /api/v1/workouts
        [HttpGet("workouts")]
        [ProducesResponseType(typeof(ApiResponse<List<WorkOutViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWorkOuts(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] WorkOutCategory? category = null,
            [FromQuery] WorkOutDiffeculty? difficulty = null,
            [FromQuery] int? duration = null,
            CancellationToken cancellationToken = default)
        {
            var query = new GetAllWorkOutsBySearchQuery(page, pageSize, search, category, difficulty, duration);
            var result = await mediator.Send(query, cancellationToken);
            if (!result.IsSuccess)
            {
                var errorCode = result.ErrorCode;
                if (errorCode == HandlerErrorCodesEnum.NotFoundAnyWorkOuts)
                    return Ok(ResponseFactory.Success(new List<WorkOutViewModel>(), "No workouts found", StatusCodes.Status200OK));
                if (errorCode == HandlerErrorCodesEnum.InvalidDiffecultyLevel)
                    return BadRequest(ResponseFactory.Failure("Invalid difficulty level", StatusCodes.Status400BadRequest, "VAL_INVALID_DIFFICULTY"));
                if (errorCode == HandlerErrorCodesEnum.InvalidCategoryName)
                    return BadRequest(ResponseFactory.Failure("Invalid category name", StatusCodes.Status400BadRequest, "VAL_INVALID_CATEGORY"));

                return BadRequest(ResponseFactory.Failure("Failed to retrieve workouts", StatusCodes.Status400BadRequest));
            }

            var workoutsViewModel = result.Data.Select(w => new WorkOutViewModel
            {
                WorkoutId = w.WorkoutId,
                Name = w.Name,
                PlanId = w.PlanId,
                Category = w.Category.ToString(),
                DurationInMinutes = w.DurationInMinutes,
                Difficulty = w.Difficulty.ToString()
            }).ToList();

            return Ok(ResponseFactory.Success(workoutsViewModel, "Workouts fetched successfully", StatusCodes.Status200OK));
        }

        // Operation 2: Workout details — GET /api/v1/workouts/{id}
        [HttpGet("workouts/{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<WorkoutDetailsViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetWorkoutById(
            [FromRoute] int id = 0,
            [FromQuery] int? WorkOutId = null,
            CancellationToken cancellationToken = default)
        {
            var targetId = id != 0 ? id : (WorkOutId ?? 0);
            var result = await mediator.Send(new GetWorkOutDetailsQuery(targetId), cancellationToken);
            if (!result.IsSuccess)
            {
                return NotFound(ResponseFactory.Failure("Workout not found with the specified ID", StatusCodes.Status404NotFound, "RES_WORKOUT_NOT_FOUND"));
            }

            var workoutViewModel = new WorkoutDetailsViewModel
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

            return Ok(ResponseFactory.Success(workoutViewModel, "Workout details fetched successfully", StatusCodes.Status200OK));
        }

        // Operation 3: Workouts by plan — GET /api/v1/workouts/by-plan/{planId}
        [HttpGet("workouts/by-plan/{planId}")]
        [ProducesResponseType(typeof(ApiResponse<List<WorkoutByPlanViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWorkoutsByPlanId(
            [FromRoute] string? planId = null,
            [FromQuery] string? PlanId = null,
            CancellationToken cancellationToken = default)
        {
            var targetPlanId = !string.IsNullOrEmpty(planId) ? planId : (PlanId ?? string.Empty);
            var result = await mediator.Send(new GetWorkOutsByPlanIdQuery(targetPlanId), cancellationToken);
            if (!result.IsSuccess)
            {
                var errorCode = result.ErrorCode;
                if (errorCode == HandlerErrorCodesEnum.NotFoundAnyWorkOutsForThisPlan)
                    return Ok(ResponseFactory.Success(new List<WorkoutByPlanViewModel>(), "No workouts found for this plan", StatusCodes.Status200OK));
                if (errorCode == HandlerErrorCodesEnum.PlanIdISNull)
                    return BadRequest(ResponseFactory.Failure("The plan ID is null or empty", StatusCodes.Status400BadRequest, "VAL_INVALID_PLAN_ID"));

                return BadRequest(ResponseFactory.Failure("Failed to retrieve workouts for plan", StatusCodes.Status400BadRequest));
            }

            var workoutsViewModel = result.Data.Select(w => new WorkoutByPlanViewModel
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

            return Ok(ResponseFactory.Success(workoutsViewModel, "Workouts fetched successfully", StatusCodes.Status200OK));
        }

        // Operation 4: Workouts by category — GET /api/v1/workouts/category/{categoryName}
        [HttpGet("workouts/category/{categoryName}")]
        [ProducesResponseType(typeof(ApiResponse<List<GetWorkOutsByCategoryNameViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWorkoutsByCategoryName(
            [FromRoute] WorkOutCategory? categoryName = null,
            [FromQuery] WorkOutCategory? CategoryName = null,
            CancellationToken cancellationToken = default)
        {
            var targetCategory = categoryName ?? CategoryName ?? WorkOutCategory.fullBody;
            var result = await mediator.Send(new GetWorkOutsByCategoryNameQuery(targetCategory), cancellationToken);
            if (!result.IsSuccess)
            {
                var errorCode = result.ErrorCode;
                if (errorCode == HandlerErrorCodesEnum.NotFoundAnyWorkOutsForThisCategoryName)
                    return Ok(ResponseFactory.Success(new List<GetWorkOutsByCategoryNameViewModel>(), "No workouts found for this category", StatusCodes.Status200OK));

                return BadRequest(ResponseFactory.Failure("Invalid category name", StatusCodes.Status400BadRequest, "VAL_INVALID_CATEGORY"));
            }

            var workoutsViewModel = result.Data.Select(w => new GetWorkOutsByCategoryNameViewModel
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

            return Ok(ResponseFactory.Success(workoutsViewModel, "Workouts fetched successfully", StatusCodes.Status200OK));
        }

        // Operation 5: Start workout — POST /api/v1/workouts/{id}/start
        [HttpPost("workouts/{id:int}/start")]
        [ProducesResponseType(typeof(ApiResponse<StartWorkOutSessionResultViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> StartWorkOutSession(
            [FromRoute] int id = 0,
            [FromQuery] int? WorkoutId = null,
            [FromQuery] WorkOutDiffeculty Difficulty = WorkOutDiffeculty.beginner,
            [FromQuery] int PlannedDuration = 30,
            CancellationToken cancellationToken = default)
        {
            var targetWorkoutId = id != 0 ? id : (WorkoutId ?? 0);
            var userId = GetCurrentUserId().ToString();

            var result = await mediator.Send(new StartWorkOutSessionCommand(targetWorkoutId, userId, Difficulty, PlannedDuration), cancellationToken);
            if (!result.IsSuccess)
            {
                var errorCode = result.ErrorCode;
                if (errorCode == HandlerErrorCodesEnum.WorkOutIdIsInvalid)
                    return NotFound(ResponseFactory.Failure("No workout found with the specified ID", StatusCodes.Status404NotFound, "RES_WORKOUT_NOT_FOUND"));

                return BadRequest(ResponseFactory.Failure("Failed to start workout session", StatusCodes.Status400BadRequest));
            }

            var startWorkOutSessionResultViewModel = new StartWorkOutSessionResultViewModel
            {
                SessionId = result.Data.SessionId,
                Status = result.Data.Status.ToString(),
                ExercisesInSession = result.Data.ExercisesInSession?.Select(e => new WorkoutExerciseInSessionViewModel
                {
                    Id = e.Id,
                    ExerciseId = e.ExerciseId,
                    Name = e.Name,
                    TargetMuscles = e.TargetMuscles,
                    Equipment = e.Equipment,
                    Difficulty = e.Difficulty,
                    Description = e.Description,
                    VideoUrl = e.VideoUrl,
                    SetsDefault = e.SetsDefault,
                    RepsDefault = e.RepsDefault,
                    RestTimeInSeconds = e.RestTimeInSeconds,
                    OrderIndex = e.OrderIndex
                }).ToList() ?? new List<WorkoutExerciseInSessionViewModel>()
            };

            return Ok(ResponseFactory.Success(
                startWorkOutSessionResultViewModel,
                "Workout session started successfully",
                StatusCodes.Status200OK));
        }

        // Operation 6: Browse exercises — GET /api/v1/exercises
        [HttpGet("exercises")]
        [ProducesResponseType(typeof(ApiResponse<List<ExercisesViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExercises(
            [FromQuery] int Page = 1,
            [FromQuery] int PageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var paginationRequest = new PaginationRequest(Page, PageSize);
            var result = await mediator.Send(new GetAllExricesQuery(paginationRequest), cancellationToken);
            if (!result.IsSuccess)
            {
                var errorCode = result.ErrorCode;
                if (errorCode == HandlerErrorCodesEnum.NotFoundAnyExercise)
                    return Ok(ResponseFactory.Success(new List<ExercisesViewModel>(), "No exercises found", StatusCodes.Status200OK));

                return BadRequest(ResponseFactory.Failure("Failed to retrieve exercises", StatusCodes.Status400BadRequest));
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
                "Exercises fetched successfully",
                StatusCodes.Status200OK));
        }

        // Operation 7: Exercise details — GET /api/v1/exercises/{id}
        [HttpGet("exercises/{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<ExerciseDetailsViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetExerciseById(
            [FromRoute] int id = 0,
            [FromQuery] int? ExerciseId = null,
            CancellationToken cancellationToken = default)
        {
            var targetId = id != 0 ? id : (ExerciseId ?? 0);
            var result = await mediator.Send(new GetExerciesDetailsByIdQuery(targetId), cancellationToken);
            if (!result.IsSuccess)
            {
                return NotFound(ResponseFactory.Failure("No exercise found with the specified ID", StatusCodes.Status404NotFound, "RES_EXERCISE_NOT_FOUND"));
            }

            var exerciseDetailsViewModel = new ExerciseDetailsViewModel
            {
                ExerciseId = result.Data.ExerciseId,
                Name = result.Data.Name,
                Difficulty = result.Data.Difficulty,
                Description = result.Data.Description,
            };

            return Ok(ResponseFactory.Success(
                exerciseDetailsViewModel,
                "Exercise details fetched successfully",
                StatusCodes.Status200OK));
        }

        // Operation 8: Browse workout plans — GET /api/v1/workout-plans
        [HttpGet("workout-plans")]
        [ProducesResponseType(typeof(ApiResponse<List<WorkOutPlansViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWorkoutPlans(
            [FromQuery] int Page = 1,
            [FromQuery] int PageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var paginationRequest = new PaginationRequest(Page, PageSize);
            var result = await mediator.Send(new GetWorkOutsPlansQuery(paginationRequest), cancellationToken);
            if (!result.IsSuccess)
            {
                var errorCode = result.ErrorCode;
                if (errorCode == HandlerErrorCodesEnum.NotFoundAnyExercise)
                    return Ok(ResponseFactory.Success(new List<WorkOutPlansViewModel>(), "No workout plans found", StatusCodes.Status200OK));

                return BadRequest(ResponseFactory.Failure("Failed to retrieve workout plans", StatusCodes.Status400BadRequest));
            }

            var workOutPlansViewModel = result.Data.Items.Select(e => new WorkOutPlansViewModel
            {
                PlanId = e.PlanId,
                Name = e.Name,
                Goal = e.Goal,
            }).ToList();

            return Ok(ResponseFactory.Success(
                workOutPlansViewModel,
                "Workout plans fetched successfully",
                StatusCodes.Status200OK));
        }

        // Operation 9: Workout plan details — GET /api/v1/workout-plans/{planId}
        [HttpGet("workout-plans/{planId}")]
        [ProducesResponseType(typeof(ApiResponse<WorkOutPlanViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetWorkoutPlanById(
            [FromRoute] string? planId = null,
            [FromQuery] string? PlanId = null,
            CancellationToken cancellationToken = default)
        {
            var targetPlanId = !string.IsNullOrEmpty(planId) ? planId : (PlanId ?? string.Empty);
            var result = await mediator.Send(new GetWorkOutPlanDetailsQuery(targetPlanId), cancellationToken);
            if (!result.IsSuccess)
            {
                return NotFound(ResponseFactory.Failure("No workout plan found with the specified ID", StatusCodes.Status404NotFound, "RES_PLAN_NOT_FOUND"));
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
                "Workout plan details fetched successfully",
                StatusCodes.Status200OK));
        }
    }
}