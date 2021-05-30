using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore_MVC.Models;

namespace WebStore_MVC.Controllers
{
    [Route("Staff")]
    public class EmployeesController : Controller
    {
        private static readonly List<Employee> _Employees = new()
        {
            new Employee { Id = 1, FirstName = "Mia", LastName = "Anderson", Age = 18 },
            new Employee { Id = 2, FirstName = "Emma", LastName = "Branson", Age = 27 },
            new Employee { Id = 3, FirstName = "Sam", LastName = "Davidson", Age = 34 }
        };

        [Route("all")]
        public IActionResult Index() => View(_Employees);

        [Route("info-id-{id}")]
        public IActionResult Details(int id)
        {
            var employee = _Employees.FirstOrDefault(empl => empl.Id == id);

            if (employee == null) return NotFound();
            return View(employee);
        }


        // GET: api/values
       /* [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/
    }
}
