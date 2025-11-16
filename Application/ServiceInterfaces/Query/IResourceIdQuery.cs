using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces.Query
{
    public interface IResourceIdQuery
    {
        Task<Resource> GetResourceByIdAsync(int id);
    }
}
