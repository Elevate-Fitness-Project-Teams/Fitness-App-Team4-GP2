// Both MediatR and MassTransit define an `IMediator`. The IDE keeps auto-importing
// MassTransit.Mediator, which makes the unqualified `IMediator` ambiguous. This alias
// pins `IMediator` to MediatR's type project-wide, so the stray using is harmless.
global using IMediator = MediatR.IMediator;
