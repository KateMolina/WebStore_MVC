using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebStore_MVC.Services.Interfaces;
using WebStore_MVC.ViewModels;

namespace WebStore_MVC.Components
{
    public class SectionsViewComponent : ViewComponent
    {
        private readonly IProductData _ProductData;

        public SectionsViewComponent(IProductData productData)
        {
            _ProductData = productData;
        }


        public IViewComponentResult Invoke(string SectionId)
        {
            var sections = _ProductData.GetSections();
            var parent_sections = sections.Where(s => s.ParentId == null);

            var parent_section_views = parent_sections
                .Select(s => new SectionViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Order = s.Order,
                }).ToList();

            foreach (var parent_section in parent_section_views)
            {
                var childs = sections.Where(s => s.ParentId == parent_section.Id);

                foreach (var child_section in childs)
                {
                    parent_section.ChildSections.Add(new SectionViewModel
                    {
                        Id = child_section.Id,
                        Name = child_section.Name,
                        Order = child_section.Order,
                        Parent = parent_section,
                    });
                }

                parent_section.ChildSections.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));
            }

            parent_section_views.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));

            return View(parent_section_views);



        }
    }
}
