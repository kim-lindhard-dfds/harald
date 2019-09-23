using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Harald.WebApi.Domain
{
    public interface ICapabilityRepository
    {
        Task<IEnumerable<Capability>> GetAll();
        Task<Capability> Get(Guid id);
        Task Add(Capability capability);
        Task Update(Capability capability);
    }
}