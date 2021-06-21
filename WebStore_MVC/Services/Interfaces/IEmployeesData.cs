using System;
using System.Collections.Generic;
using WebStore_MVC.Models;
using WebStore.Domain.Entities;

namespace WebStore_MVC.Services.Interfaces
{
    public interface IEmployeesData
    {
        IEnumerable<Employee> GetAll();

       public Employee Get(int id);

        public int Add(Employee employee);

       public void Update(Employee employee);

       public bool Delete(int id);
    }
}
