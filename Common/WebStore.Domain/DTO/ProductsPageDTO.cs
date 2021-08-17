using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Domain.DTO
{
    public record ProductsPageDTO(IEnumerable<ProductDTO> Products, int TotalCount);

    public static class ProductsPageDTOMapper
    {
        public static ProductsPageDTO ToDTO(this ProductsPage Page) => new ProductsPageDTO(Page.Products.ToDTO(), Page.TotalCount);
        public static ProductsPage FromDTO(this ProductsPageDTO Page) => new (Page.Products.FromDTO(), Page.TotalCount);
    }
}
