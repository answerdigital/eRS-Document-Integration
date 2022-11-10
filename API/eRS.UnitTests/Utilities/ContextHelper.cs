using eRS.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;

namespace eRS.UnitTests.Utilities;

public static class ContextHelper
{
    public static eRSContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<eRSContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString(), b => b.EnableNullChecks(false))
            .Options;

        var dbContext = new eRSContext(options);
        dbContext.Database.EnsureCreated();

        return dbContext;
    }
}
