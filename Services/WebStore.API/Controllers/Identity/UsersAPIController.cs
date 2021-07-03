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
    [Route(WebAPIAddresses.Users)]
    [ApiController]
    public class UsersAPIController : ControllerBase
    {
        private readonly UserStore<User, Role, WebStoreDB> userStore;

        public UsersAPIController(WebStoreDB db)
        {
            userStore = new UserStore<User, Role, WebStoreDB>(db);
        }

        [HttpGet("all")]
        public async Task<IEnumerable<User>> GetAllUsers() => await userStore.Users.ToArrayAsync();
    }
}
