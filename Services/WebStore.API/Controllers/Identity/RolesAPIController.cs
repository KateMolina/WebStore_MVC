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

        [HttpPost]
        public async Task<bool> CreateAsync(Role role)
        {
            var creation_result = await roleStore.CreateAsync(role);
            return creation_result.Succeeded;
        }

        [HttpPut]
        public async Task<bool> UpdateAsync(Role role)
        {
            var uprate_result = await roleStore.UpdateAsync(role);
            return uprate_result.Succeeded;
        }

        [HttpPost("Delete")]
        public async Task<bool> DeleteAsync(Role role)
        {
            var delete_result = await roleStore.DeleteAsync(role);
            return delete_result.Succeeded;
        }

        [HttpPost("GetRoleId")]
        public async Task<string> GetRoleIdAsync([FromBody] Role role) => await roleStore.GetRoleIdAsync(role);

        [HttpPost("GetRoleName")]
        public async Task<string> GetRoleNameAsync([FromBody] Role role) => await roleStore.GetRoleNameAsync(role);

        [HttpPost("SetRoleName/{name}")]
        public async Task<string> SetRoleNameAsync(Role role, string name)
        {
            await roleStore.SetRoleNameAsync(role, name);
            await roleStore.UpdateAsync(role);
            return role.Name;
        }

        [HttpPost("GetNormalizedRoleName")]
        public async Task<string> GetNormalizedRoleNameAsync(Role role) => await roleStore.GetNormalizedRoleNameAsync(role);

        [HttpPost("SetNormalizedRoleName/{name}")]
        public async Task<string> SetNormalizedRoleNameAsync(Role role, string name)
        {
            await roleStore.SetNormalizedRoleNameAsync(role, name);
            await roleStore.UpdateAsync(role);
            return role.NormalizedName;
        }

        [HttpGet("FindById/{id}")]
        public async Task<Role> FindByIdAsync(string id) => await roleStore.FindByIdAsync(id);

        [HttpGet("FindByName/{name}")]
        public async Task<Role> FindByNameAsync(string name) => await roleStore.FindByNameAsync(name);

    }
}
