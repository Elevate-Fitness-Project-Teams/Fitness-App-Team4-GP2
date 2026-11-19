using System.Security.Claims;
using BuildingBlocks.Shared.Controllers;
using BuildingBlocks.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NutritionService.Controllers;
using ProgressService.Controllers;
using SubscriptionService.Controllers;
using WorkoutService.Controllers;
using Xunit;

namespace Elevate.FitnessApp.Tests.Security
{
    public class TestableController : ApiControllerBase
    {
        public bool CheckOwnerOrAdmin(Guid targetUserId) => IsOwnerOrAdmin(targetUserId);
        public Guid ResolveCurrentUserId() => GetCurrentUserId();
        public bool TryResolveCurrentUserId(out Guid userId) => TryGetCurrentUserId(out userId);
    }

    public class SecurityTests
    {
        [Fact]
        public void SEC_001_WorkOutController_Has_Authorize_Attribute()
        {
            // Verify controller-level [Authorize] is present on WorkOutController
            var authAttribute = typeof(WorkOutController)
                .GetCustomAttributes(typeof(AuthorizeAttribute), inherit: true);

            Assert.NotEmpty(authAttribute);
        }

        [Fact]
        public void SEC_002_NutritionController_Has_Authorize_Attribute()
        {
            // Verify controller-level [Authorize] is present on NutritionController
            var authAttribute = typeof(NutritionController)
                .GetCustomAttributes(typeof(AuthorizeAttribute), inherit: true);

            Assert.NotEmpty(authAttribute);
        }

        [Fact]
        public void SEC_002_Unauthenticated_Claim_Extraction_Does_Not_Throw_NullReference_Or_Format_Exception()
        {
            var controller = new TestableController
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext() // Anonymous user, no claims
                }
            };

            var success = controller.TryResolveCurrentUserId(out var userId);

            Assert.False(success);
            Assert.Equal(Guid.Empty, userId);

            // Calling GetCurrentUserId should throw typed AppException (mapped to 401)
            var ex = Assert.Throws<AppException>(() => controller.ResolveCurrentUserId());
            Assert.Equal("AUTH_INVALID_TOKEN", ex.Error.Code);
            Assert.Equal(401, ex.Error.ToStatusCode());
        }

        [Fact]
        public void SEC_003_To_006_IDOR_Owner_Access_Allowed()
        {
            var userId = Guid.NewGuid();
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userId.ToString())
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);

            var controller = new TestableController
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            Assert.True(controller.CheckOwnerOrAdmin(userId));
        }

        [Fact]
        public void SEC_003_To_006_IDOR_Cross_User_Access_Rejected_For_Regular_User()
        {
            var userA = Guid.NewGuid();
            var userB = Guid.NewGuid();

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userA.ToString())
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);

            var controller = new TestableController
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            // User A trying to access User B's resources must be rejected
            Assert.False(controller.CheckOwnerOrAdmin(userB));
        }

        [Fact]
        public void SEC_003_To_006_IDOR_Admin_Can_Access_Target_User()
        {
            var adminId = Guid.NewGuid();
            var userB = Guid.NewGuid();

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, adminId.ToString()),
                new(ClaimTypes.Role, "Admin")
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);

            var controller = new TestableController
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            // Admin is allowed
            Assert.True(controller.CheckOwnerOrAdmin(userB));
        }

        [Fact]
        public void Controller_Inheritance_Verifies_Progress_And_Subscription_Controllers_Are_Protected()
        {
            Assert.NotEmpty(typeof(ProgressController).GetCustomAttributes(typeof(AuthorizeAttribute), inherit: true));
            Assert.NotEmpty(typeof(SubscriptionController).GetCustomAttributes(typeof(AuthorizeAttribute), inherit: true));
        }
    }
}

