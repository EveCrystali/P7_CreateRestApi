using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;

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
