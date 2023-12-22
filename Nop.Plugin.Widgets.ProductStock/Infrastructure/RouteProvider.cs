using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Widgets.ProductStock.Infrastructure
{
    public class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapControllerRoute(name: ProductStockDefaults.ConfigurationRouteName,
                pattern: $"category/products/",
                defaults: new { controller = "ExtendedCatalog", action = "GetCategoryProducts" });
        }

        public int Priority => 0;
    }
}
