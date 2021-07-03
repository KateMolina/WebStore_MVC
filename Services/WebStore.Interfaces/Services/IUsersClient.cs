using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Interfaces.Services
{
    public interface IUsersClient : IUserStore<User>, IUserRoleStore<User>, IUserPasswordStore<User>, IUserPhoneNumberStore<User>, IUserTwoFactorStore<User>,
        IUserClaimStore<User>, IUserLoginStore<User>
    {
    }
}
