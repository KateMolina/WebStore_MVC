using System;
using System.Collections.Generic;
using System.Linq;
using WebStore_MVC.Data;
using WebStore_MVC.Models;
using WebStore_MVC.Services.Interfaces;

namespace WebStore_MVC.Services
{
    public class InMemoryEmployeesData:IEmployeesData
    {
        
        private int _CurrentMaxId;

        public InMemoryEmployeesData()
        {
            _CurrentMaxId = TestData.Employees.Max(i => i.Id);
        }
        public IEnumerable<Employee> GetAll() => TestData.Employees;

        public int Add(Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));
            if (TestData.Employees.Contains(employee)) return employee.Id;
            employee.Id = ++_CurrentMaxId;
            TestData.Employees.Add(employee);
            return employee.Id;
        }

        public Employee Get(int id) => TestData.Employees.SingleOrDefault(employee => employee.Id == id);

        public void Update(Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));
            if (TestData.Employees.Contains(employee)) return;
            var dbItem = Get(employee.Id);
            if (dbItem is null) return;

            dbItem.LastName = employee.LastName;
            dbItem.FirstName = employee.FirstName;
            dbItem.Age = employee.Age;
        }

        public bool Delete(int id)
        {
            var dbItem = Get(id);
            if (dbItem is null) return false;
            return TestData.Employees.Remove(dbItem);

        }

       
    }
}
