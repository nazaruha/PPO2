using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.Entities.Specification
{
    public static class ProductSpecification
    {
        public class GetAll : Specification<Product>
        {
            public GetAll()
            {
                Query
                    .Include(p => p.Manufacturer)
                    .Include(p => p.Orders)
                    .Include(p => p.Storage);
            }
        }
        public class GetById : Specification<Product>
        {
            public GetById(int id)
            {
                Query
                    .Include(p => p.Manufacturer)
                    .Include(p => p.Orders)
                    .Include(p => p.Storage)
                    .Where(p => p.Id == id);
            }
        }
        public class GetByName : Specification<Product>
        {
            public GetByName(string name)
            {
                Query
                   .Include(p => p.Manufacturer)
                   .Include(p => p.Orders)
                   .Include(p => p.Storage)
                   .Where(p => p.Name == name);
            }
        }

        public class GetByManufacturerIdAndName : Specification<Product>
        {
            public GetByManufacturerIdAndName(string name, int manufacturerId)
            {
                Query.Include(p => p.Manufacturer).Include(p => p.Storage).Include(p => p.Orders)
                    .Where(p => p.ManufacturerId == manufacturerId && p.Name == name);
            }
        }
        public class GetByIdAndManufacturerId : Specification<Product>
        {
            public GetByIdAndManufacturerId(int id, int manufacturerId)
            {
                Query
                    .Include(p => p.Manufacturer).Include(p => p.Orders).Include(p => p.Storage)
                    .Where(p => p.Id == id && p.ManufacturerId == manufacturerId);
            }
        }
    }
}
