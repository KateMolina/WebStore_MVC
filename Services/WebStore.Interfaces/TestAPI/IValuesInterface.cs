using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Interfaces.TestAPI
{
   public interface IValuesInterface
    {
        IEnumerable<string> GetAll();

        string GetByIndex(int index);

        void Add(int index);

        void Edit(int index, string s);

        void Delete(int index);

    }
}
