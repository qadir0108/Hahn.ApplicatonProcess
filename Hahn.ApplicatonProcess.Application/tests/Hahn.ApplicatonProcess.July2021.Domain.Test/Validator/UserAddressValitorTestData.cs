using Hahn.ApplicatonProcess.July2021.Domain.Models;
using Hahn.ApplicatonProcess.July2021.Domain.Validators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.July2021.Domain.Test.Validator
{
    public class UserAddressValitorTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {
                new AddressVm
                {
                  HouseNumber = "51-B",
                  Street = "15",
                  PostalCode = "1234",
                }
            };
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
