using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces;

namespace WebStore.API.Controllers.Identity
{
    [Route(WebAPIAddresses.Roles)]
    [ApiController]
    public class RolesAPIController : ControllerBase
    {
        private readonly RoleStore<Role> roleStore;

        public RolesAPIController(WebStoreDB db)
        {
            roleStore = new RoleStore<Role>(db);
        }

        [HttpGet("all")]
        public async Task<IEnumerable<Role>> GetAllRoles() => await roleStore.Roles.ToArrayAsync();

    }
}
