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
        private readonly WebStoreDB _db;

        public SqlEmployeesData(WebStoreDB db)
        {
            _db = db;
        }

        public IEnumerable<Employee> GetAll() => _db.Employees;
       

        public int Add(Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));
            _db.Employees.Add(employee);
            _db.SaveChanges();
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
            var modifiedEmp = Get(employee.Id);
            modifiedEmp.Name = employee.Name;
            modifiedEmp.LastName = employee.LastName;
            modifiedEmp.Age = employee.Age;
            _db.SaveChanges();
        }
        public bool Delete(int id)
        {
            var employee = Get(id);
            _db.Employees.Remove(employee);
            _db.SaveChanges();
            return true;
        }



    }
}
