using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Concrete;
using EntityLayer.Entities;

namespace EntityLayer.EntityLayer.Entities
{
    public class Category : AuditTrailer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}