using XeGo.Services.Rating.API.Data;
using XeGo.Services.Rating.API.Entities;
using XeGo.Services.Rating.API.Repository.IRepository;
using XeGo.Shared.Lib.Repository;

namespace XeGo.Services.Rating.API.Repository
{
    public class UserRatingRepository(AppDbContext db) : Repository<UserRating>(db), IUserRatingRepository
    {
        private readonly AppDbContext _dbContext = db;

    }
}
