namespace wikiasp.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class WikiaModels : DbContext
    {
        public WikiaModels()
            : base("name=WikiaModels")
        {
        }

        public virtual DbSet<Wikias> Wikias { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
