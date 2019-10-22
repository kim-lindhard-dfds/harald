using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Harald.WebApi.Domain
{
    public interface ICapabilityRepository
    {
        Task<IEnumerable<Capability>> GetAll();
        
        Task<IEnumerable<Capability>> GetById(Guid id);
        Task Add(Capability capability);
        Task Update(Capability capability);
        Task Remove(Capability capability);
    }
}