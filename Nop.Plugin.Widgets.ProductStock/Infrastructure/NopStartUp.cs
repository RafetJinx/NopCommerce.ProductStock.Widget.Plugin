using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.Widgets.ProductStock.Controllers;
using Nop.Services.Catalog;
using Nop.Web.Controllers;
using Nop.Web.Factories;

namespace Nop.Plugin.Widgets.ProductStock.Infrastructure
{
    public class NopStartUp : INopStartup
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IExtendedProductService, ExtendedProductService>();
            services.AddScoped<IExtendedCatalogModelFactory, ExtendedCatalogModelFactory>();
            services.AddScoped<CatalogController, ExtendedCatalogController>();
        }

        public void Configure(IApplicationBuilder application)
        {

        }

        public int Order => 3000;
    }
}