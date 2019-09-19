using System.Collections.Generic;
using System.Linq;

namespace Harald.WebApi.Features.Connections.Infrastructure.DrivingAdapters.Api.Model
{
    public class ItemsEnvelope<T>
    {
        public T[] Items { get; }

        public ItemsEnvelope(IEnumerable<T> items)
        {
            Items = items.ToArray();
        }
    }
}