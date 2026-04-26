using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace G2_Shared_Infrastructure
{
	public class G2GatewayMiddleware
	{
		private readonly RequestDelegate _next;

		public G2GatewayMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			if (context.Request.Path.StartsWithSegments("/health"))
			{
				await _next(context);
				return;
			}

			bool fromGateway = context.Request.Headers.TryGetValue("X-From-Gateway", out var secret) && secret == "GS-Gateway-Trusted-Token-111";
			bool fromInternal = context.Request.Headers.TryGetValue("X-From-Internal", out var secret2) && secret2 == "GS-Internal-Trusted-Token-111";
			if (!fromGateway && !fromInternal)
			{
				context.Response.StatusCode = 403;
				await context.Response.WriteAsync("Direct access is forbidden. You must go through the API Gateway.");
				return;
			}
			await _next(context);
		}
	}
}
