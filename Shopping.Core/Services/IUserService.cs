using Shopping.Core.Models.JWTDbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Core.Services
{
    public interface IUserService
    {
        Task<UserViewModel> GetUserAsync();
    }
}
