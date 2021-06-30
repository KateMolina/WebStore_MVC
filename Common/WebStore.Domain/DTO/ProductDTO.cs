using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Entities;

namespace WebStore.Domain.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public BrandDTO Brand { get; set; }

        public SectionDTO Section { get; set; }

    }

    public class BrandDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

    }
    public class SectionDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public int ParentId { get; set; }
    }

    public static class BrandDTOMapper
    {
        public static BrandDTO ToDTO(this Brand brand)
        {
            if (brand is null) return null;

            return new BrandDTO
            {
                Id = brand.Id,
                Name = brand.Name,
                Order = brand.Order,

            };
        }
        public static Brand FromDTO(this BrandDTO brand)
        {
            if (brand is null) return null;
            return new Brand
            {
                Id = brand.Id,
                Name = brand.Name,
                Order = brand.Order,
            };
        }

        public static IEnumerable<BrandDTO> ToDTO(this IEnumerable<Brand> brands) => brands.Select(ToDTO);
        public static IEnumerable<Brand> FromDTO(this IEnumerable<BrandDTO> brands) => brands.Select(FromDTO);

    }

    public static class SectionDTOMapper
    {
        public static SectionDTO ToDTO(this Section section)
        {
            if (section is null) return null;

            return new SectionDTO
            {
                Id = section.Id,
                Name = section.Name,
                Order = section.Order,
                ParentId = (int)section.ParentId,

            };
        }
        public static Section FromDTO(this SectionDTO section)
        {
            if (section is null) return null;
            return new Section
            {
                Id = section.Id,
                Name = section.Name,
                Order = section.Order,
                ParentId = section.ParentId,
            };
        }

        public static IEnumerable<SectionDTO> ToDTO(this IEnumerable<Section> sections) => sections.Select(ToDTO);
        public static IEnumerable<Section> FromDTO(this IEnumerable<SectionDTO> sections) => sections.Select(FromDTO);

    }

    public static class ProductDTOMapper
    {
        public static ProductDTO ToDTO(this Product product) => product is null
            ? null
            : new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Order = product.Order,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Brand = product.Brand.ToDTO(),
                Section = product.Section.ToDTO(),

            };
        public static Product FromDTO(this ProductDTO product) => product is null
           ? null
           : new Product
           {
               Id = product.Id,
               Name = product.Name,
               Order = product.Order,
               Price = product.Price,
               ImageUrl = product.ImageUrl,
               Brand = product.Brand.FromDTO(),
               Section = product.Section.FromDTO(),

           };
        public static IEnumerable<ProductDTO> ToDTO(this IEnumerable<Product> sections) => sections.Select(ToDTO);
        public static IEnumerable<Product> FromDTO(this IEnumerable<ProductDTO> sections) => sections.Select(FromDTO);
    }


}
