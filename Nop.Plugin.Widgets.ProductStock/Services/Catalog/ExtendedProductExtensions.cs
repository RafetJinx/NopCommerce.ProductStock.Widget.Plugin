using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Localization;
using Nop.Data;

namespace Nop.Services.Catalog
{
    public static class ExtendedProductExtensions
    {
        #region Methods

        public static IQueryable<Product> OrderBy(
            this IQueryable<Product> productsQuery,
            IRepository<LocalizedProperty> localizedPropertyRepository,
            Language currentLanguage,
            ProductSortingEnum orderBy,
            bool isChecked)
        {
            if (orderBy == ProductSortingEnum.NameAsc || orderBy == ProductSortingEnum.NameDesc)
            {
                var currentLanguageId = currentLanguage.Id;

                var query =
                    from product in productsQuery
                    join localizedProperty in localizedPropertyRepository.Table on new
                    {
                        product.Id,
                        languageId = currentLanguageId,
                        keyGroup = nameof(Product),
                        key = nameof(Product.Name)
                    }
                        equals new
                        {
                            Id = localizedProperty.EntityId,
                            languageId = localizedProperty.LanguageId,
                            keyGroup = localizedProperty.LocaleKeyGroup,
                            key = localizedProperty.LocaleKey
                        } into localizedProperties
                    from localizedProperty in localizedProperties.DefaultIfEmpty(new LocalizedProperty { LocaleValue = product.Name })
                    select new
                    {
                        sortName = localizedProperty == null ? product.Name : localizedProperty.LocaleValue,
                        product
                    };

                if (orderBy == ProductSortingEnum.NameAsc)
                    productsQuery = from item in query
                                    orderby item.sortName
                                    select item.product;
                else
                    productsQuery = from item in query
                                    orderby item.sortName descending
                                    select item.product;

                var inStockProductsQuery = productsQuery.Where(p => p.StockQuantity > 0);
                var outOfStockProductsQuery = productsQuery.Where(p => p.StockQuantity <= 0);

                return OrderBy(inStockProductsQuery, outOfStockProductsQuery, isChecked);
            }

            return Result(productsQuery, orderBy, isChecked);
        }

        #endregion

        #region Utilities

        private static IQueryable<Product> OrderBy(IQueryable<Product> query, ProductSortingEnum orderBy)
        {
            return orderBy switch
            {
                ProductSortingEnum.PriceAsc => query.OrderBy(p => p.Price),
                ProductSortingEnum.PriceDesc => query.OrderByDescending(p => p.Price),
                ProductSortingEnum.CreatedOn => query.OrderByDescending(p => p.CreatedOnUtc),
                ProductSortingEnum.Position when query is IOrderedQueryable => query,
                _ => query.OrderBy(p => p.DisplayOrder).ThenBy(p => p.Id)
            };
        }

        private static IQueryable<Product> OrderBy(IQueryable<Product> inStockQuery, IQueryable<Product> outOfStockQuery, bool isChecked)
        {
            var combinedProductList = isChecked
                ? inStockQuery.ToList().Concat(outOfStockQuery.ToList())
                : outOfStockQuery.ToList().Concat(inStockQuery.ToList());

            return combinedProductList.AsQueryable();
        }

        private static IQueryable<Product> Result(IQueryable<Product> query, ProductSortingEnum orderBy, bool isChecked)
        {
            var inStockProductsQuery = query.Where(p => p.StockQuantity > 0);
            var outOfStockProductsQuery = query.Where(p => p.StockQuantity <= 0);

            var orderedInStockProductsQuery = OrderBy(inStockProductsQuery, orderBy);
            var orderedOutOfStockProductsQuery = OrderBy(outOfStockProductsQuery, orderBy);

            var orderedProductsQuery = OrderBy(orderedInStockProductsQuery, orderedOutOfStockProductsQuery, isChecked);

            return orderedProductsQuery;
        }

        #endregion
    }
}