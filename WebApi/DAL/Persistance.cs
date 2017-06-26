using SharedModels;
using System.Data.Entity;

namespace WebApi.DAL
{
    public class Persistance : DbContext
    {
        public Persistance() : base("Persistance")
        {
        }

        public virtual DbSet<MessageEntity> Messages { get; set; }
    }
}