using ProjectTaskManagement.Application.Common.Models;
using ProjectTaskManagement.Application.UseCases.AuthCases.Commands;
using ProjectTaskManagement.Application.UseCases.AuthCases.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ProjectTaskManagement.Api.Endpoints;

public static class AuthEndpoints
{
    public static RouteGroupBuilder MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth")
            .WithTags("auth")
            .WithGroupName("auth");

        group.MapPost("/register", Register)
            .AllowAnonymous()
            .WithName("Register")
            .WithOpenApi();

        group.MapPost("/login", Login)
            .AllowAnonymous()
            .WithName("Login")
            .WithOpenApi();

        return group;
    }

    private static async Task<DataResponse<RegisterResponseDto>> Register(
        [FromServices] ISender sender,
        [FromBody] RegisterCommand command) =>
        await sender.Send(command);

    private static async Task<DataResponse<AuthResponseDto>> Login(
        [FromServices] ISender sender,
        [FromBody] LoginCommand command) =>
        await sender.Send(command);
}
