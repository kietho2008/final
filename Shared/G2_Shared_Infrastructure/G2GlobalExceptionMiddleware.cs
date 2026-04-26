using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;


namespace G2_Shared_Infrastructure
{
	public class G2GlobalExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<G2GlobalExceptionMiddleware> _logger;

		public G2GlobalExceptionMiddleware(RequestDelegate next, ILogger<G2GlobalExceptionMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An unhandled exception occurred.");
				await HandleExceptionAsync(context, ex);
			}
		}

		private static Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			context.Response.ContentType = "application/json";
			
			var statusCode = HttpStatusCode.InternalServerError;
			var result = "An unexpected error occurred.";

			if (exception is ArgumentException || exception.Message.Contains("Invalid"))
			{
				statusCode = HttpStatusCode.BadRequest;
				result = exception.Message;
			}
			else if (exception.Message.Contains("not found"))
			{
				statusCode = HttpStatusCode.NotFound;
				result = exception.Message;
			}

			context.Response.StatusCode = (int)statusCode;

			var response = new { error = result, detail = exception.InnerException?.Message };
			return context.Response.WriteAsync(JsonSerializer.Serialize(response));
		}
	}
}
