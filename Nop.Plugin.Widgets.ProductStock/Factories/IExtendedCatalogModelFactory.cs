using Nop.Core.Domain.Catalog;
using Nop.Plugin.Widgets.ProductStock.Models.Catalog;
using Nop.Web.Models.Catalog;

namespace Nop.Web.Factories
{
    public partial interface IExtendedCatalogModelFactory : ICatalogModelFactory
    {
        #region Methods

        Task<CatalogProductsModel> PrepareCategoryProductsModelAsync(Category category, ExtendedCatalogProductsCommand command);

        #endregion
    }
}