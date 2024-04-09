using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Common;

public static class EfBuilderExtensions
{
    public static void Enum<TEnum>(this ModelBuilder builder) where TEnum : Enum
    {
        builder.Entity<EnumTable<TEnum>>().ToTable(typeof(TEnum).Name);
        builder.Entity<EnumTable<TEnum>>().Property(s => s.Id).HasConversion<string>();

        var enumValues = System.Enum
            .GetValues(typeof(TEnum))
            .Cast<TEnum>()
            .ToList();
                
        foreach (var enumValue in enumValues)
        {
            builder.Entity<EnumTable<TEnum>>()
                .HasData(new EnumTable<TEnum> { Id = enumValue });
        }
    }
    
    public static void WithEnum<TEnum>(this EntityTypeBuilder builder) where TEnum : struct, Enum
    {
        var enumType = typeof(TEnum);
        var nullableEnumType = typeof(TEnum?);

        var entityType = builder.Metadata.ClrType;
        var enumProperties = entityType
            .GetProperties()
            .Where(p => p.PropertyType == enumType || p.PropertyType == nullableEnumType)
            .ToList();

        foreach (var enumProperty in enumProperties)
        {
            builder.HasOne(typeof(EnumTable<TEnum>))
                .WithMany()
                .HasForeignKey(enumProperty.Name)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }



    private class EnumTable<TEnum> where TEnum : Enum
    {
        public TEnum Id { get; set; }
    }
}