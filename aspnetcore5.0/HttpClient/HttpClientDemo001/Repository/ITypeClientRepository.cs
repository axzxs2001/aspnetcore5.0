using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HttpClientDemo001.Repository
{
    public interface ITypeClientRepository
    {
        Task<List<Entity>> GetEntities();
        
    }
}
