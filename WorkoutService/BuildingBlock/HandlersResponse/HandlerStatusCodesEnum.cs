namespace WorkoutService.BuildingBlock.HandlersResponse
{
    public enum HandlerErrorCodesEnum
    {
        // Get WorkOuts 

        NotFoundAnyWorkOuts=400,
        NotFoundAnyWorkOutsForThisPlan=401,
        PlanIdISNull=402,
        NotFoundAnyWorkOutsForThisCategoryName=403,
        InvalidCategoryName=404,
        InvalidDiffecultyLevel=405,
    }
}
