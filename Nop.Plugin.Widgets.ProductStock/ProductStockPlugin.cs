using Nop.Plugin.Widgets.ProductStock.Components;
using Nop.Services.Cms;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.Widgets.ProductStock
{
    public class ProductStockPlugin : BasePlugin, IWidgetPlugin
    {
        #region Fields

        private readonly ILocalizationService _localizationService;

        #endregion

        #region Ctor

        public ProductStockPlugin(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        #endregion

        public override async Task InstallAsync()
        {
            await _localizationService.AddOrUpdateLocaleResourceAsync("Plugins.Widgets.ProductStock.TextBoxName", "Stock Quantity: High to Low", "en-US");
            await _localizationService.AddOrUpdateLocaleResourceAsync("Plugins.Widgets.ProductStock.TextBoxName", "Stok Miktarı: Yüksekten Düşüğe", "tr-TR");

            await base.InstallAsync();
        }

        public override Task UninstallAsync()
        {
            return base.UninstallAsync();
        }

        public bool HideInWidgetList => false;

        public Type GetWidgetViewComponent(string widgetZone)
        {
            return typeof(ProductStockViewComponent);
        }

        public async Task<IList<string>> GetWidgetZonesAsync()
        {
            return await Task.FromResult(new List<string> { PublicWidgetZones.CategoryDetailsBeforeProductList });
        }
    }
}
