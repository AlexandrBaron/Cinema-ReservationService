namespace RegistrationService.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Reservation> Reservations => Set<Reservation>();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            Random random = new Random();

            optionsBuilder.LogTo(result => System.Diagnostics.Trace.WriteLine(result), Microsoft.Extensions.Logging.LogLevel.Information);
            optionsBuilder.EnableSensitiveDataLogging();

            optionsBuilder.UseSeeding((context, _) =>
            {

                for (var i = 1; i < 20; i++)
                {
                    var user = new User() { Email = $"user{i}@email.com", PhoneNumber = random.Next(100000000, 999999999) };

                    context.Set<User>().Add(user);
                    context.SaveChanges();
                }

            });
        }
    }
}