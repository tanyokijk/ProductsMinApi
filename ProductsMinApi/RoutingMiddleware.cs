namespace ProductsMinApi;

public static class RoutingMiddlewareExtensions
{

    public static IApplicationBuilder UseRoutingMiddleware(this IApplicationBuilder builder)
    {
        return builder.Use(async (context, next) =>
        {
            var path = context.Request.Path.Value?.ToLower();

                if (path == "/") await HandleProductsRequest(context);
                else if (path == "/index"  ) await HandleProductsRequest(context);
                else if (path == "/login") await HandleProductsRequest(context);
                else await next();
        });
    }

    private static async Task HandleProductsRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        await context.Response.WriteAsync("Products Page");
    }
}
