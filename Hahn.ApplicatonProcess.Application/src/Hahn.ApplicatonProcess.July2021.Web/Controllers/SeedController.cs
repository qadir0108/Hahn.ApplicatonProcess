using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hahn.ApplicatonProcess.July2021.Data.UnitOfWoork;

namespace Hahn.ApplicatonProcess.July2021.Web.ApiControllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("seed")]
    public class SeedController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        /// <summary>
        /// Seed Controller
        /// </summary>
        /// <param name="uow"></param>
        public SeedController(IUnitOfWork uow)
        {
            this._uow = uow;
        }

        /// <summary>
        /// This is used to seed initial data into database
        /// </summary>
        /// <returns></returns>
        [HttpPost("users")]
        public async Task SeedUsers()
        {
            await _uow.UsersRepository.AddAsync(new Data.Entities.User()
            {
                Age = 37,
                FirstName = "Kamran",
                LastName = "Qadir",
                Email = "qadir0108@hotmail.com",
                Address = new Data.Entities.Address() { HouseNumber = "51/B", Street = "15", PostalCode = "1234" },
                Assets = new List<Data.Entities.Asset> {
                    new Data.Entities.Asset()
                    {
                        Id =  "bitcoin",
                        Symbol = "BTC",
                        Name = "Bitcoin"
                    },
                    new Data.Entities.Asset()
                    {
                        Id =  "ethereum",
                        Symbol = "ETH",
                        Name = "Ethereum"
                    },
                    new Data.Entities.Asset()
                    {
                        Id =  "tether",
                        Symbol = "USDT",
                        Name = "Tether"
                    },
                }
            });
            await _uow.SaveChangesAsync();
        }

    }
}
