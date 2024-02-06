using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ContactDb = ContactManagerCS.DAL.Entities.Contact;

namespace ContactManagerCS.DAL.Database.Configurations;

public class ContactConfiguration : IEntityTypeConfiguration<ContactDb>
{
    public void Configure(EntityTypeBuilder<ContactDb> builder)
    {
        builder
            .ToTable("contact");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.Property(x => x.Phone)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(x => x.Company)
            .IsRequired(false)
            .HasMaxLength(100);
    }
}
