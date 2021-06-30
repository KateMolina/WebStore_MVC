using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using WebStore_MVC.Data;
using WebStore_MVC.Services.Interfaces;
using WebStore.Domain.Entities;

namespace WebStore_MVC.Services
{
    public class InMemoryEmployeesData:IEmployeesData
    {
        
        private int _CurrentMaxId;
        private readonly ILogger<InMemoryEmployeesData> _logger;

        public InMemoryEmployeesData(ILogger<InMemoryEmployeesData>logger)
        {
            _CurrentMaxId = TestData.Employees.Max(i => i.Id);
            _logger = logger;
        }
        public IEnumerable<Employee> GetAll() => TestData.Employees;

        public int Add(Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));
            if (TestData.Employees.Contains(employee)) return employee.Id;
            employee.Id = ++_CurrentMaxId;
            TestData.Employees.Add(employee);
            _logger.LogInformation("Employee {0} has been added", employee.Id);
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
            dbItem.Name = employee.Name;
            dbItem.Age = employee.Age;
            _logger.LogInformation("Employee {0} has been updated", employee.Id);

        }

        public bool Delete(int id)
        {
            var dbItem = Get(id);
            if (dbItem is null) 
            {
                _logger.LogWarning("Deleting employee {0} - id not found", id);
                return false; 
            }
            var result = TestData.Employees.Remove(dbItem);
            if (result)
            {
                _logger.LogInformation("Employee {0} has been removed", id);
            }
            else
            {
                _logger.LogError("Problems while deleting employee {0} - id not found", id);
            }
            return result;

        }

       
    }
}
