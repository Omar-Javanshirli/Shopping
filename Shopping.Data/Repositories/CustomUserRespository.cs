using Microsoft.EntityFrameworkCore;
using Shopping.Core.Models;
using Shopping.Core.Repositories;
using System.Threading.Tasks;

namespace Shopping.Data.Repositories
{
    internal class CustomUserRespository : ICustomUserRepository
    {
        private readonly AppDbContext _context;

        public CustomUserRespository(AppDbContext context)
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
