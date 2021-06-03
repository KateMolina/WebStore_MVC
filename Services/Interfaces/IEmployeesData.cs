using System;
using System.Collections.Generic;
using WebStore_MVC.Models;

namespace WebStore_MVC.Services.Interfaces
{
    public interface IEmployeesData
    {
        IEnumerable<Employee> GetAll();

        Employee Get(int id);

        int Add(Employee employee);

        void Update(Employee employee);

        bool Delete(int id);
    }
}
