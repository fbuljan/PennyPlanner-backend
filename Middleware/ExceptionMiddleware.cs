﻿using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PennyPlanner.Exceptions;
using System.Text.Json;

namespace PennyPlanner.Middleware
{
    public class ExceptionMiddleware
    {
        private RequestDelegate Next { get; }

        public ExceptionMiddleware(RequestDelegate next)
        {
            Next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await Next(context);
            }
            catch (UserNotFoundException ex)
            {
                context.Response.ContentType = "application/problem+json";
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                var problemDetails = new ProblemDetails()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Detail = string.Empty,
                    Instance = "",
                    Title = $"User for id {ex.Id} not found.",
                    Type = "Error"
                };

                var problemDetailsJson = JsonSerializer.Serialize(problemDetails);
                await context.Response.WriteAsync(problemDetailsJson);
            }
            catch (UserAlreadyExistsException ex)
            {
                context.Response.ContentType = "application/problem+json";
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                var problemDetails = new ProblemDetails()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Detail = ex.Message,
                    Instance = "",
                    Title = ex.ErrorMessage != null ? ex.ErrorMessage : $"User already exists.",
                    Type = "Error"
                };

                var problemDetailsJson = JsonSerializer.Serialize(problemDetails);
                await context.Response.WriteAsync(problemDetailsJson);
            }
            catch (AccountNotFoundException ex)
            {
                context.Response.ContentType = "application/problem+json";
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                var problemDetails = new ProblemDetails()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Detail = string.Empty,
                    Instance = "",
                    Title = $"Account for id {ex.Id} not found.",
                    Type = "Error"
                };

                var problemDetailsJson = JsonSerializer.Serialize(problemDetails);
                await context.Response.WriteAsync(problemDetailsJson);
            }
            catch (AccountNameAlreadyInUseException ex)
            {
                context.Response.ContentType = "application/problem+json";
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                var problemDetails = new ProblemDetails()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Detail = string.Empty,
                    Instance = "",
                    Title = $"User with id {ex.Id} already has an account with name {ex.Name}.",
                    Type = "Error"
                };

                var problemDetailsJson = JsonSerializer.Serialize(problemDetails);
                await context.Response.WriteAsync(problemDetailsJson);
            }
            catch (GoalNotFoundException ex)
            {
                context.Response.ContentType = "application/problem+json";
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                var problemDetails = new ProblemDetails()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Detail = string.Empty,
                    Instance = "",
                    Title = $"Goal for id {ex.Id} not found.",
                    Type = "Error"
                };

                var problemDetailsJson = JsonSerializer.Serialize(problemDetails);
                await context.Response.WriteAsync(problemDetailsJson);
            }
            catch (TransactionNotFoundException ex)
            {
                context.Response.ContentType = "application/problem+json";
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                var problemDetails = new ProblemDetails()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Detail = string.Empty,
                    Instance = "",
                    Title = $"Transaction for id {ex.Id} not found.",
                    Type = "Error"
                };

                var problemDetailsJson = JsonSerializer.Serialize(problemDetails);
                await context.Response.WriteAsync(problemDetailsJson);
            }
            catch (ValidationException ex)
            {
                context.Response.ContentType = "application/problem+json";
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                var problemDetails = new ProblemDetails()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Detail = JsonSerializer.Serialize(ex.Errors),
                    Instance = "",
                    Title = "Validation Error",
                    Type = "Error"
                };

                var problemDetailsJson = JsonSerializer.Serialize(problemDetails);
                await context.Response.WriteAsync(problemDetailsJson);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/problem+json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var problemDetails = new ProblemDetails()
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = ex.Message,
                    Instance = "",
                    Title = "Internal Server Error - something went wrong.",
                    Type = "Error"
                };

                var problemDetailsJson = JsonSerializer.Serialize(problemDetails);
                await context.Response.WriteAsync(problemDetailsJson);
            }
        }
    }
}
