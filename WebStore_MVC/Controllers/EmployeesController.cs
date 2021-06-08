﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStore_MVC.Models;
using WebStore_MVC.Services.Interfaces;
using WebStore_MVC.ViewModels;

namespace WebStore_MVC.Controllers
{
   // [Route("Staff")]
    public class EmployeesController : Controller
    {
        private readonly IEmployeesData _EmployeesData;
        private readonly ILogger _logger;

        public EmployeesController(IEmployeesData employeesData, ILogger<EmployeesController> logger)
        {
            _EmployeesData = employeesData;
            _logger = logger;
        }

        //[Route("all")]
        public IActionResult Index() => View(_EmployeesData.GetAll());

       // [Route("info-id-{id}")]
        public IActionResult Details(int id)
        {
            var employee = _EmployeesData.Get(id);

            if (employee == null) return NotFound();
            return View(employee);
        }

        public IActionResult Create(int id) => View("Edit", new EmployeeViewModel());


        public IActionResult Edit(int? id)
        {
            if (id is null) return View(new EmployeeViewModel());
            var employee = _EmployeesData.Get((int)id);
            if (employee is null) return NotFound();

            var viewModel = new EmployeeViewModel()
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Age = employee.Age,
        };

            return View(viewModel);
        }


        [HttpPost]
        public IActionResult Edit(EmployeeViewModel Model)
        {
            if (!ModelState.IsValid) return View(Model);

            _logger.LogInformation("Updating details of the employee by id {}", Model.Id);

            var employee = new Employee()
            {
                Id = Model.Id,
                FirstName = Model.FirstName,
                LastName = Model.LastName,
                Age=Model.Age,
            };
            if (employee.Id == 0) _EmployeesData.Add(employee);
            else _EmployeesData.Update(employee);

            _logger.LogInformation("Updating details of the employee by id {} completed", Model.Id);

            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            if (id <= 0) return BadRequest();
            var employee = _EmployeesData.Get(id);

            if (employee is null) return NotFound();

            return View(new EmployeeViewModel()
            {
            Id=employee.Id,
            FirstName = employee.FirstName,
            LastName=employee.LastName,
            Age=employee.Age,
            });
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            _logger.LogInformation("Deletion of the employee by id {}", id);
            _EmployeesData.Delete(id);
            _logger.LogInformation("Deletion of the employee by id {} has been completed", id);
            return RedirectToAction("Index");
        }



    }
}
