using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Repositories
{
    public class UserRepository(LocalDbContext dbContext)
    {
        public LocalDbContext DbContext { get; } = dbContext;

        public User FindByUserName(string userName)
        {
            return DbContext.Users.FirstOrDefault(user => user.UserName == userName) ?? throw new Exception($"User with username '{userName}' not found");
        }

        public async Task<List<User>> FindAll()
        {
            return await DbContext.Users.ToListAsync();
        }

        public void Add(User user)
        {
            DbContext.Users.Add(user);
            DbContext.SaveChanges();
        }

        public User FindById(int id)
        {
            try
            {
                return DbContext.Users.Find(id);
            }
            catch
            {
                return null;
            }
        }
    }
}