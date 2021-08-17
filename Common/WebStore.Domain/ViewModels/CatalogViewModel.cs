using System.Collections.Generic;

namespace WebStore_MVC.ViewModels
{
    public class CatalogViewModel
    {
        public int? SectionId { get; set; }
        public int? BrandId { get; set; }

        public IEnumerable<ProductViewModel> Products { get; set; }

        public PageViewModel PageViewModel { get; set; }
    }

}
