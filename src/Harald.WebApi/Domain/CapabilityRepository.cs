using System;
using System.Threading.Tasks;
using Harald.WebApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Harald.WebApi.Domain
{
    public class CapabilityRepository : ICapabilityRepository
    {
        private readonly HaraldDbContext _dbContext;

        public CapabilityRepository(HaraldDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Capability> Get(Guid id)
        {
            var capability = await _dbContext
                .Capabilities
                .SingleOrDefaultAsync(x => x.Id == id);

            return capability;
        }

        public async Task Add(Capability capability)
        {
            await _dbContext.Capabilities.AddAsync(capability);
            await _dbContext.SaveChangesAsync();
        }
    }
}