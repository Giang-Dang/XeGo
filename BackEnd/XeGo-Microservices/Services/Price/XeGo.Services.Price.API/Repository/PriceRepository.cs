﻿using XeGo.Services.Price.API.Data;
using XeGo.Services.Price.API.Repository.IRepository;
using XeGo.Shared.Lib.Repository;

namespace XeGo.Services.Price.API.Repository
{
    public class PriceRepository(AppDbContext db) : Repository<Entities.Price>(db), IPriceRepository
    {
        private readonly AppDbContext _dbContext = db;

    }
}
