using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.July2021.Data.Entities
{
    public class User
    {
        public User()
        {
            this.Assets = new HashSet<Asset>();
        }

        public int Id { get; set; }

        public int Age { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public Address Address { get; set; }
        
        public string Email { get; set; }
        
        public virtual ICollection<Asset> Assets { get; set; }
    }
}
