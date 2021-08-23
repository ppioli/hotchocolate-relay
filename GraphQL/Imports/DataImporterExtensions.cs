namespace ConferencePlanner.GraphQL.Imports
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Accounting.Models;
    using Data;
    using HotChocolate.Execution.Configuration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;

    public static class ImportRequestExecutorBuilderExtensions
    {
        public static IRequestExecutorBuilder SeedDatabase(
            this IRequestExecutorBuilder builder)
        {
            return builder.ConfigureSchemaAsync(async (services, _, ct) =>
            {
                IDbContextFactory<ApplicationDbContext> factory =
                    services.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
                await using ApplicationDbContext dbContext = factory.CreateDbContext();


                if (!dbContext.Accounts.Any())
                {
                    var accounts = ReadFile<Account>("Imports/accounts.json");
                    var accountsByParentId = accounts
                        .Where(a => a.ParentId is not null)
                        .GroupBy(a => a.ParentId);

                    foreach (var group in accountsByParentId)
                    {
                        var account = accounts.First(a => a.Code == group.Key.ToString());
                        account.Children = group.ToList();
                        account.IsCategory = true;
                        foreach (var accountChild in account.Children) accountChild.ParentId = null;
                    }


                    await dbContext.AddRangeAsync(accounts.Where(a => a.ParentId == null), ct);
                }

                var rng = new Random();
                if (!dbContext.JournalEntries.Any())
                    for (var i = 0; i < 100; i++)
                    {
                        var acc1 = dbContext.Accounts.First(a => !a.IsCategory && a.Type == AccountClass.Asset);
                        var acc2 = dbContext.Accounts.First(a => !a.IsCategory && a.Type == AccountClass.Liability);
                        var amount = (decimal)rng.NextDouble() * 100000m;
                        var je = new JournalEntry($"Asiento {i}", DateTime.Now)
                        {
                            Consolidated = true,
                            JournalEntryLines = new List<JournalEntryLine>
                            {
                                new(1, $"Linea {i}-1", acc1, DoubleEntryValue.CreditAmount(amount)),
                                new(2, $"Linea {i}-1", acc2, DoubleEntryValue.DebitAmount(amount))
                            }
                        };
                        dbContext.Add(je);
                    }

                await dbContext.SaveChangesAsync(ct);
            });
        }

        private static List<TData> ReadFile<TData>(string filename)
        {
            using StreamReader r = new(filename);

            string json = r.ReadToEnd();

            return JsonConvert.DeserializeObject<List<TData>>(json);
        }
    }
}