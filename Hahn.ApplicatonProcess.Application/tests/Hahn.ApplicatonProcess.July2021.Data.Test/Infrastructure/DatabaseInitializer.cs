using Hahn.ApplicatonProcess.July2021.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hahn.ApplicatonProcess.July2021.Data.Test.Infrastructure
{
    public class DatabaseInitializer
    {
        public static void Initialize(HahnContext context)
        {
            if (context.Users.Any())
            {
                return;
            }

            Seed(context);
        }

        private static void Seed(HahnContext context)
        {
            var assets = new List<Data.Entities.Asset> {
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
            };
            context.Assets.AddRange(assets);
            context.SaveChanges();

            var users = new[]
            {
                new Data.Entities.User()
                {
                    Age = 37,
                    FirstName = "Kamran",
                    LastName = "Qadir",
                    Email = "qadir0108@hotmail.com",
                    Address = new Data.Entities.Address() { HouseNumber = "51/B", Street = "15", PostalCode = 1234 },
                },
                new Data.Entities.User()
                {
                    Age = 25,
                    FirstName = "Sabeeh",
                    LastName = "Kamran",
                    Email = "sabeehkamran2010@outlook.com",
                    Address = new Data.Entities.Address() { HouseNumber = "1-A", Street = "20", PostalCode = 4321 },
                },
            };
            context.Users.AddRange(users);
            context.SaveChanges();
           
        }
    }
}