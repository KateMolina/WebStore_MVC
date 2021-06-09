using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore_MVC.Services.Interfaces;

namespace WebStore_MVC.Services.InSql
{
    public class SqlEmployeesData : IEmployeesData
    {
        public WebStoreDB _db;
        int currentMaxId;

        public SqlEmployeesData(WebStoreDB db)
        {
            _db = db;
            currentMaxId = _db.Employees.Max(item => item.Id);
        }

        public IEnumerable<Employee> GetAll() => _db.Employees;
       

        public int Add(Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));
            if (_db.Employees.Contains(employee)) return employee.Id;
            employee.Id=currentMaxId++;
            _db.Employees.Add(employee);
            return employee.Id;
        }
        public Employee Get(int id)
        {
           Employee empl=_db.Employees.SingleOrDefault(employee => employee.Id == id);

            return empl;
        }

           
        public void Update(Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));
            if (_db.Employees.Contains(employee)) return;
            var modifiedEmp = Get(employee.Id);
            modifiedEmp.Name = employee.Name;
            modifiedEmp.LastName = employee.LastName;
            modifiedEmp.Age = employee.Age;
        }
        public bool Delete(int id)
        {
            var employee = Get(id);
            if (employee is null) { return false; }
            _db.Employees.Remove(employee);
          
            return true;
        }



    }
}
