using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.Entities.Specification
{
    public static class CustomerSpecification
    {
        public class GetAll : Specification<Customer>
        {
            public GetAll()
            {
                Query
                    .Include(c => c.Orders)
                    .Include(c => c.Projects);
            }
        }
        public class GetByEmail : Specification<Customer>
        {
            public GetByEmail(string email)
            {
                Query.Include(c => c.Orders).Include(c => c.Projects)
                    .Where(c => c.Email == email);
            }
        }
        public class GetByPhone : Specification<Customer>
        {
            public GetByPhone(string phone)
            {
                Query.Include(c => c.Orders).Include(c => c.Projects)
                   .Where(c => c.Phone == phone);
            }
        }
        public class GetById : Specification<Customer>
        {
            public GetById(int id)
            {
                Query
                    .Include(c => c.Projects)
                    .Include(c => c.Orders)
                    .Where(c => c.Id == id);
            }
        }
    }
}
