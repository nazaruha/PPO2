using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.Entities.Specification
{
    public static class ProjectSpecification
    {
        public class GetAll : Specification<Project>
        {
            public GetAll()
            {
                Query
                    .Include(p => p.Customers)
                    .Include(p => p.Storage)
                    .Include(p => p.Orders)
                    .Include(p => p.Plans);
            }
        }
        public class GetById : Specification<Project>
        {
            public GetById(int id)
            {
                Query
                    .Include(p => p.Customers)
                    .Include(p => p.Storage)
                    .Include(p => p.Orders)
                    .Include(p => p.Plans)
                    .Where(p => p.Id == id);
            }
        }
    }
}
