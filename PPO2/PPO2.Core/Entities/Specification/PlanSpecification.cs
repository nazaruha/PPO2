using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.Entities.Specification
{
    public static class PlanSpecification
    {
        public class GetAll : Specification<Plan>
        {
            public GetAll()
            {
                Query
                    .Include(p => p.Projects);
            }
        }

        public class GetById : Specification<Plan>
        {
            public GetById(int id) 
            {
                Query
                    .Include(p => p.Projects)
                    .Where(p => p.Id == id);    
            }
        }
    }
}
