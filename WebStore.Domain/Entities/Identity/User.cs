using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Domain.Entities.Identity
{
    public class User:IdentityUser
    {
        public const string UserName = "Admin";
        public const string Password = "Admin";

    }

    public class Role : IdentityRole
    {
        public const string Administrators="Administrators";
        public const string Users="Users";
    }
}
