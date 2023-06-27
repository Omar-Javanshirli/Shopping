using Shopping.Core.Models.JWTDbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Core.Repositories
{
    public interface ICustomUserRepository
    {
        Task<bool> Validate(string email, string password);

        Task<CustomUser> FindById(int id);

        Task<CustomUser> FindByEmail(string email);
    }
}
