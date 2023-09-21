using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

internal class AvatarConfiguration : IEntityTypeConfiguration<Avatar>
{
    public void Configure(EntityTypeBuilder<Avatar> builder)
    {
        builder.OwnsOne(x => x.File, options =>
        {
            // To not have table_name_property_name (Avatar_FileName)
            options.Property(x => x.FileName).HasColumnName("FileName");
            options.Property(x => x.FileGuid).HasColumnName("FileGuid");
            options.Property(x => x.FileExtension).HasColumnName("FileExtension");
            options.Property(x => x.FilePath).HasColumnName("FilePath");
        });
    }
}