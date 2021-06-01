using System;
using System.Collections.Generic;
using System.Linq;
using WebStore_MVC.Models;
using WebStore_MVC.Services.Interfaces;

namespace WebStore_MVC.Services
{
    public class InMemoryEmployeesData:IEmployeesData
    {
        private readonly List<Employee> _Employees = new()
        {
            new Employee {Id = 1, FirstName = "Mia", LastName = "Anderson", Age = 18 },
            new Employee { Id = 2, FirstName = "Emma", LastName = "Branson", Age = 27 },
            new Employee { Id = 3, FirstName = "Sam", LastName = "Davidson", Age = 34 }
        };
        private int _CurrentMaxId;

        public InMemoryEmployeesData()
        {
            _CurrentMaxId = _Employees.Max(i => i.Id);
        }
        public IEnumerable<Employee> GetAll() => _Employees;

        public void Add(Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));
            //if (_Employees.Contains(employee)) return employee.Id;
            employee.Id = ++_CurrentMaxId;
            _Employees.Add(employee);
           // return employee.Id;
        }

        public Employee Get(int id) => _Employees.SingleOrDefault(employee => employee.Id == id);

        public void Update(Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));
            if (_Employees.Contains(employee)) return;
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
            return _Employees.Remove(dbItem);

        }

       
    }
}
