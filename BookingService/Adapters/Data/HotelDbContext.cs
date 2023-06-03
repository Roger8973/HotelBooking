using Data.Guest;
using Data.Room;
using Entities = Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options) { }
        
        public virtual DbSet<Entities.Guest> Guests { get; set; }
        public virtual DbSet<Entities.Booking> Bookings { get; set; }
        public virtual DbSet<Entities.Room> Rooms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GuestConfiguration());
            modelBuilder.ApplyConfiguration(new RoomConfiguration());
        }
    }
}