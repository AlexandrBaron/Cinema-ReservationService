namespace RegistrationService.Infrastructure.Data.Config
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.Property(x => x.ReservationStatus).HasDefaultValue(0);
        }
    }
}
