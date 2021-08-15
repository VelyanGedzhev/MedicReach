using MedicReach.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace MedicReach.Tests
{
    public static class DatabaseMock
    {
        public static MedicReachDbContext Instance
        {
            get
            {
                var dbContextOptions = new DbContextOptionsBuilder<MedicReachDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

                return new MedicReachDbContext(dbContextOptions);
            }
        }
    }
}
