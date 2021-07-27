using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hahn.ApplicatonProcess.July2021.Data.Entities
{
    public class Asset
    {
        public Asset()
        {
            this.Users = new HashSet<User>();
        }
        
        public string Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }

    }
}
