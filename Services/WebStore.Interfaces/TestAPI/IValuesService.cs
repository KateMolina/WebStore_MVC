using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Interfaces.TestAPI
{
   public interface IValuesService
    {
        IEnumerable<string> GetAll();

        string GetByIndex(int index);

        void Add(int index);

        void Edit(int index, string s);

        bool Delete(int index);

    }
}
