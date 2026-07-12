
using BuildingBlocks.Shared.Results;

namespace NotificationService.Common
{
    public static class NotificationErrors
    {
        public static Error NotFound =>
            Error.NotFound("RES_NOT_FOUND", "The requested notification was not found.");

    }
}