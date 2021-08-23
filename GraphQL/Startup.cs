namespace ConferencePlanner.GraphQL
{
    using System.Threading.Tasks;
    using Accounting.Resolvers.Account;
    using Accounting.Resolvers.JournalEntry;
    using Attendees;
    using Conferences.Sessions;
    using Data;
    using DataLoader;
    using Imports;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Speakers;
    using Tracks;
    using Types;

    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCors(o =>
                    o.AddDefaultPolicy(b =>
                        b.AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowAnyOrigin()))

                // First we add the DBContext which we will be using to interact with our
                // Database.
                .AddPooledDbContextFactory<ApplicationDbContext>(
                    options => options.UseNpgsql("Host=localhost;Database=pepper;Username=pepper;Password=pepper"))

                // This adds the GraphQL server core service and declares a schema.
                .AddGraphQLServer()

                // Next we add the types to our schema.
                .AddQueryType()
                .AddMutationType()
                .AddSubscriptionType()
                .AddTypeExtension<AttendeeMutations>()
                .AddTypeExtension<SessionMutations>()
                .AddTypeExtension<SpeakerMutations>()
                .AddTypeExtension<AddAccountMutation>()
                .AddTypeExtension<TrackMutations>()
                .AddTypeExtension<AttendeeSubscriptions>()
                .AddTypeExtension<SessionSubscriptions>()
                .AddTypeExtension<AttendeeQueries>()
                .AddTypeExtension<SessionQueries>()
                .AddTypeExtension<SpeakerQueries>()
                .AddTypeExtension<TrackQueries>()
                .AddTypeExtension<AccountQueries>()
                .AddTypeExtension<JournalEntryQueries>()
                .AddType<AttendeeType>()
                .AddType<SessionType>()
                .AddType<SpeakerType>()
                .AddType<TrackType>()
                .AddType<AccountType>()
                .AddType<JournalEntryType>()
                // In this section we are adding extensions like relay helpers,
                // filtering and sorting.
                .AddProjections()
                .AddFiltering()
                .AddSorting()
                .AddGlobalObjectIdentification()

                // Now we add some the DataLoader to our system.
                .AddDataLoader<AttendeeByIdDataLoader>()
                .AddDataLoader<SessionByIdDataLoader>()
                .AddDataLoader<SpeakerByIdDataLoader>()
                .AddDataLoader<TrackByIdDataLoader>()
                .AddDataLoader<AccountByIdDataLoader>()
                .AddDataLoader<JournalEntryByIdDataLoader>()
                .AddDataLoader<JournalEntryTotalDataLoader>()
                // we make sure that the db exists and prefill it with conference data.
                .SeedDatabase()

                // Since we are using subscriptions, we need to register a pub/sub system.
                // for our demo we are using a in-memory pub/sub system.
                .AddInMemorySubscriptions()

                // Last we add support for persisted queries.
                // The first line adds the persisted query storage,
                // the second one the persisted query processing pipeline.
                .AddFileSystemQueryStorage("./persisted_queries")
                .UsePersistedQueryPipeline();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseCors();

            app.UseWebSockets();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                // We will be using the new routing API to host our GraphQL middleware.
                endpoints.MapGraphQL();

                endpoints.MapGet("/", context =>
                {
                    context.Response.Redirect("/graphql");
                    return Task.CompletedTask;
                });
            });
        }
    }
}