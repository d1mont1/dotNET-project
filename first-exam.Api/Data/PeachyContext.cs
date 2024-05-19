using System;
using System.Collections.Generic;
using first_exam.Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace first_exam.Api.Data
{
    public partial class PeachyContext : IdentityDbContext
    {
        public PeachyContext()
        {

        }

        public PeachyContext(DbContextOptions<PeachyContext> options): base(options)
        {
        }

        public virtual DbSet<Integration> Integrations { get; set; }
        public DbSet<User> Users {  get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("data source=178.89.186.221, 1434;initial catalog=omirzak_db;user id=omirzak_user;password=Qbwd4067$;MultipleActiveResultSets=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("omirzak_user");

            OnModelCreatingPartial(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
