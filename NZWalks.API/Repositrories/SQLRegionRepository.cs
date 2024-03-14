﻿using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositrories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLRegionRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
           var existing =await dbContext.Regions.FirstOrDefaultAsync(x=> x.Id == id);
            if (existing == null) { return null; }
             dbContext.Regions.Remove(existing);
            await dbContext.SaveChangesAsync();
            return existing;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await dbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
       return await dbContext.Regions.FirstOrDefaultAsync(region=>region.Id == id);
        }

        public async Task<Region?> UpdateAsync(Guid id, Region update)
        {
           var existingRegion=await dbContext.Regions.FirstOrDefaultAsync(x=>x.Id == id);
            if (existingRegion == null) {
                return null;
            }
            existingRegion.Code= update.Code;
            existingRegion.Name= update.Name;
            existingRegion.RegionImageUrl = update.RegionImageUrl;
            await dbContext.SaveChangesAsync();
            return existingRegion;
        }
    }
}
