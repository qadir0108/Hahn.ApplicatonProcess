using System;
using Hahn.ApplicatonProcess.July2021.Data;
using Microsoft.EntityFrameworkCore;

namespace Hahn.ApplicatonProcess.July2021.Data.Test.Infrastructure
{
    public class DatabaseTestBase : IDisposable
    {
        protected readonly HahnContext Context;

        public DatabaseTestBase()
        {
            var options = new DbContextOptionsBuilder<HahnContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            Context = new HahnContext(options);

            Context.Database.EnsureCreated();

            DatabaseInitializer.Initialize(Context);
        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();

            Context.Dispose();
        }
    }
}