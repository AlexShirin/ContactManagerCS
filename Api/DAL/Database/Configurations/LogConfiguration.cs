using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LogDb = ContactManagerCS.DAL.Entities.Log;

namespace ContactManagerCS.DAL.Database.Configurations;

public class LogConfiguration : IEntityTypeConfiguration<LogDb>
{
    public void Configure(EntityTypeBuilder<LogDb> builder)
    {
        builder
            .ToTable("log");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Message)
            .IsRequired();

        builder.Property(x => x.Timestamp)
            .IsRequired();
    }
}
