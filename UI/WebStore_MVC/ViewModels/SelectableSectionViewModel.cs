using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore_MVC.ViewModels
{
    public class SelectableSectionViewModel
    {

        public IEnumerable<SectionViewModel> Sections { get; set; }

        public int? SectionId { get; init; }

        public int? ParentSectionId { get; init; }

    }
}
