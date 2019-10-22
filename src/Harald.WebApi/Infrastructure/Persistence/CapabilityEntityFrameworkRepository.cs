﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Harald.WebApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace Harald.WebApi.Infrastructure.Persistence
{
    public class CapabilityEntityFrameworkRepository : ICapabilityRepository
    {
        private readonly HaraldDbContext _dbContext;

        public CapabilityEntityFrameworkRepository(HaraldDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Capability>> GetById(Guid id)
        {
            var capabilities = await _dbContext
                .Capabilities
                .AsNoTracking()
                .Where(x => x.Id == id)
                .ToListAsync();

            return capabilities;
        }

        public async Task Add(Capability capability)
        {
            await _dbContext.Capabilities.AddAsync(capability);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(Capability capability)
        {
            _dbContext.Capabilities.Update(capability);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Remove(Capability capability)
        {
            _dbContext.Capabilities.Remove(capability);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Capability>> GetAll()
        {
            return await _dbContext
                .Capabilities
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync();
        }
    }
}