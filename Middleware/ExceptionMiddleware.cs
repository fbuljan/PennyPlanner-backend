﻿using Microsoft.AspNetCore.Mvc;
using PennyPlanner.Exceptions;
using System.ComponentModel.DataAnnotations;
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
