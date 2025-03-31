namespace RegistrationService.Infrastructure.Data
{
    public class ApDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>, IDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            return CreateDbContext();
        }

        public AppDbContext CreateDbContext()
        {
            //var configurationBuilder = new ConfigurationBuilder()
            //    .AddUserSecrets<ApDbContextFactory>()
            //    .Build();
            //var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
            //optionBuilder.UseSqlServer(configurationBuilder.GetConnectionString("RegistrationService"));

            //return new AppDbContext(optionBuilder.Options);


            var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();

            optionBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=Cinema.RegistrationService;Trusted_Connection=True;");


            return new AppDbContext(optionBuilder.Options);
        }
    }
}
