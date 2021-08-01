using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.July2021.Domain.Models
{
    public class TrackDataVm
    {
        public int userId { get; set; }
        public string assetId { get; set; }
        public bool trackUntrack { get; set; }
    }
}
