namespace wikiasp.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ElementModels : DbContext
    {
        public ElementModels()
            : base("name=ElementModels")
        {
        }

        public virtual DbSet<Element> Element { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
