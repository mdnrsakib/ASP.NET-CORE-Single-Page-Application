using SPA_TravelTour_Db_Application.Models;

namespace SPA_TravelTour_Db_Application.HostedService
{
    public class DbSeederHostedService : IHostedService
    {

        IServiceProvider serviceProvider;
        public DbSeederHostedService(
            IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (IServiceScope scope = serviceProvider.CreateScope())
            {

                var db = scope.ServiceProvider.GetRequiredService<TravelTourDbContext>();

                await SeedDbAsync(db);

            }
        }
        public async Task SeedDbAsync(TravelTourDbContext db)
        {
            await db.Database.EnsureCreatedAsync();
            if (!db.TravelAgents.Any())
            {
                var ta = new TravelAgent { AgentName = "A1", Email = "a1@mysite.com", PhoneNumber = "01303216097",AgentAddress="Mirpur, Dhaka" };
                await db.TravelAgents.AddAsync(ta);
                var tp = new TourPackage { PackageCategory = PakageCategory.Economy, PackageName = "P1", CostPerPerson = 2000.00M, TourTime=5 };
                tp.PackageFeatures.Add(new PackageFeature { TransportMode = TransPortMode.Bus, HotelBooking = "Chattrogram View", Status = true });
                tp.Tourists.Add(new Tourist { TouristName = "Sakib", BookingDate = new DateTime(2000, 02, 02), TouristOccupation = "Student", TouristPicture = "2.jpg" });
                await db.TourPackages.AddAsync(tp);
                AgentTourPackage atp = new AgentTourPackage { TourPackage = tp, TravelAgent = ta };
                await db.AgentTourPackages.AddAsync(atp);
                await db.SaveChangesAsync();
            }

        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

    }
}
