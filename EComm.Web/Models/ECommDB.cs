namespace EComm
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class ECommDB : DbContext
    {
      
        public ECommDB()
            : base("name=ECommDB")
        {
            Database.SetInitializer<ECommDB>(new DropCreateDatabaseIfModelChanges<ECommDB>());

        }


        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
    }


}