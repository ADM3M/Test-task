using api.Common;
using api.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories;

public class TestTaskDbContext : DbContext
{
    private const int EntityKeyMaxLength = 64;
    
    
    public TestTaskDbContext(DbContextOptions options) : base(options)
    {

    }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Enum<Gender>();

        builder.Entity<Patient>().Property(p => p.Id).HasMaxLength(EntityKeyMaxLength);
        builder.Entity<Patient>()
            .Property(p => p.NameId)
            .HasMaxLength(EntityKeyMaxLength);
        builder.Entity<Patient>()
            .HasOne(p => p.Name)
            .WithOne()
            .HasForeignKey<Patient>(p => p.NameId);
        builder.Entity<Patient>().WithEnum<Gender>();
        builder.Entity<Patient>().ToTable(name: nameof(Patient));

        builder.Entity<PatientName>().Property(pn => pn.Id).HasMaxLength(EntityKeyMaxLength);
        builder.Entity<PatientName>()
            .HasMany(g => g.Given)
            .WithOne()
            .HasForeignKey(g => g.PatientNameId);

        builder.Entity<PatientGiven>().Property(pg => pg.Id).HasMaxLength(EntityKeyMaxLength);
        builder.Entity<PatientGiven>().Property(pg => pg.PatientNameId).HasMaxLength(EntityKeyMaxLength);
        
        base.OnModelCreating(builder);
    }
}