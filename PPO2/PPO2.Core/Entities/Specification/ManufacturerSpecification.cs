using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.Entities.Specification
{
    public static class ManufacturerSpecification
    {
        public class GetAll : Specification<Manufacturer>
        {
            public GetAll()
            {
                Query.Include(m => m.Products);
            }
        }
        public class GetById : Specification<Manufacturer>
        {
            public GetById(int id)
            {
                Query
                    .Include(m => m.Products)
                    .Where(m => m.Id == id);
            }
        }
        public class GetByName : Specification<Manufacturer> 
        {
            public GetByName(string name)
            {
                Query
                    .Include(m => m.Products)
                    .Where(m => m.Name == name);
            }
        }
    }
}
