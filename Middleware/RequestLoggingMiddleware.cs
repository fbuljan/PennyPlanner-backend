namespace PennyPlanner.Middleware
{
    public class RequestLoggingMiddleware
    {
        private RequestDelegate Next { get; }
        private ILogger<RequestLoggingMiddleware> Logger { get; }

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            Next = next;
            Logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Logger.LogInformation("[{DateTime}] Received HTTP request: {Method} {Path}",
                                   DateTime.UtcNow, context.Request.Method, context.Request.Path); 
            await Next(context);
        }
    }
}
