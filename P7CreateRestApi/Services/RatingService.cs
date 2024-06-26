using Dot.Net.WebApi.Data;

namespace Dot.Net.WebApi.Services
{
    public class RatingService
    {
        private readonly LocalDbContext _context;

        public RatingService(LocalDbContext context)
        {
            _context = context;
        }

    }
}
