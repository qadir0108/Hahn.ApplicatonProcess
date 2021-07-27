using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.July2021.Domain.Models
{
    public class UserVm
    {
        public int Id { get; set; }

        public int Age { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public AddressVm Address { get; set; }
        
        public string Email { get; set; }
        
        public virtual List<AssetVm> Assets { get; set; }

    }
}
