using Microsoft.EntityFrameworkCore;
using Shopping.Core.Models.JWTDbModels;
using Shopping.Core.Repositories;
using System.Threading.Tasks;

namespace Shopping.Data.Repositories
{
    public class CustomUserRespository:ICustomUserRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomUserRespository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CustomUser> FindByEmail(string email)
        {
            return await _context.customUsers.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<CustomUser> FindById(int id)
        {
            return await _context.customUsers.FindAsync(id);
        }

        public async Task<bool> Validate(string email, string password)
        {
            return await _context.customUsers.AnyAsync(x => x.Email == email && x.Password == password);
        }
    }
}
