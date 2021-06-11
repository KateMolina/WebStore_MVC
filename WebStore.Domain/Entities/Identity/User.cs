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
        public const string userName = "Admin";
        public const string password = "Admin";

    }

    public class Role : IdentityRole
    {
        public const string administrators="Administrators";
        public const string users="Users";
    }
}
