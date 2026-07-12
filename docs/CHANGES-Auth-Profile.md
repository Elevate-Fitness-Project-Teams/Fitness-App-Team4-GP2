# Change Log — AuthService & ProfileService

_Branch: `testing`_
_Scope: AuthService and ProfileService only_

Two tasks: (1) replace all manual request validation with FluentValidation, and
(2) revoke active sessions after a password change. Both are complete and both services
build clean (0 errors, 0 warnings).

---

## Task 1 — Request validation using FluentValidation — ✅ DONE

FluentValidation is now wired through the MediatR pipeline. Every command is validated by a
dedicated validator **before** its handler runs; all previous manual/imperative validation
has been removed.

### How it works
`BuildingBlocks.Shared.Behaviors.ValidationBehavior<,>` is an `IPipelineBehavior` that runs all
registered `IValidator<TRequest>` instances and, on failure, short-circuits with a
`Result.Fail(...)`. It maps each FluentValidation failure's **`ErrorCode`** to the
`Error.Code` (the JSON key in the response envelope) and its message to `Error.Description`.
Because of this, each rule uses `.WithErrorCode("...")` to preserve the exact error codes the
old manual checks returned (e.g. `email`, `password`, `phoneNumber`, `AUTH_PASSWORD_MISMATCH`,
`VAL_INVALID_FILE_TYPE`, `VAL_FILE_TOO_LARGE`, `VAL_REQUIRED_FIELD`) — so the API contract is
unchanged.

### DI wiring (both services)
- Added `FluentValidation.DependencyInjectionExtensions` (12.1.1) to `AuthService.csproj` and
  `ProfileService.csproj`.
- In each `Program.cs`, inside `AddMediatR`:
  `cfg.AddOpenBehavior(typeof(global::BuildingBlocks.Shared.Behaviors.ValidationBehavior<,>))`
  (the `global::` prefix disambiguates the shared `BuildingBlocks` namespace from each service's
  own `*.BuildingBlocks` namespace), plus
  `builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly)`.

This mirrors the pattern already used by FCEService/ProgressService.

### Validators added
Each lives beside its command in the feature folder (Vertical Slice), as a
`sealed class XValidator : AbstractValidator<XCommand>`. Compound field checks
(email / password / phone / OTP / name-length) use a null-safe `.Must(...)` predicate so a
single error code/message is produced for null, empty, and bad-format alike — faithfully
replicating the deleted `InputValidator` semantics.

**AuthService (9):**
Register, Login, ForgotPassword, VerifyOtp, ResetPassword, Profile/ChangePassword,
Profile/UpdateUserInfo, RefreshToken.
(ResetPassword & ChangePassword also enforce new==confirm via a guarded `AUTH_PASSWORD_MISMATCH`
rule.)

**ProfileService (4):**
ChangePassword, UpdateProfile, UpdateSettings, UploadProfilePicture.

### Manual validation removed
- Deleted `AuthService/BuildingBlocks/Validation/InputValidator.cs` (and its now-empty folder).
- Removed the `Validate(...)` method and its call from `RegisterCommandHandler`.
- Removed inline `InputValidator` checks from Login, ForgotPassword, VerifyOtp, ResetPassword
  handlers, and the `AUTH_PASSWORD_MISMATCH` guard from ResetPassword.
- Removed the commented-out mismatch block from AuthService `Profile/ChangePasswordCommandHandler`.
- ProfileService: removed the mismatch check from `ChangePasswordCommandHandler`, the
  `ValidatePicture(...)` method (and its consts) from `UploadProfilePictureCommandHandler`, and
  the "at least one section" check from `UpdateSettingsCommandHandler`.

---

## Task 2 — Revoke active sessions after password change — ✅ DONE

Implemented in
[`AuthService/Features/Profile/ChangePassword/ChangePasswordCommandHandler.cs`](../AuthService/Features/Profile/ChangePassword/ChangePasswordCommandHandler.cs).
The user's active refresh tokens are marked revoked **before** calling
`UserManager.ChangePasswordAsync`. Those token entities are tracked on the same scoped
`DbContext` that Identity uses, so its internal `SaveChanges` commits the new password hash, the
rotated security stamp, and the revocations **atomically in one transaction**. If the current
password is wrong, `ChangePasswordAsync` returns without saving, so the tracked revocations are
discarded and nothing is revoked.

> Existing stateless JWT **access** tokens remain valid until expiry; revoking refresh tokens
> prevents session renewal, which is the enforceable part for a stateless JWT.

---

## Verification
- `dotnet build AuthService/AuthService.csproj` → succeeded, 0 errors / 0 warnings.
- `dotnet build ProfileService/ProfileService.csproj` → succeeded, 0 errors / 0 warnings.
