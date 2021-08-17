using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Entities;

namespace WebStore.Domain
{
    public record ProductsPage(IEnumerable<Product> Products, int TotalCount);
    
}
