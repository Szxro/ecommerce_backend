using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class ProductFilesConfiguration : IEntityTypeConfiguration<ProductFiles>
{
    public void Configure(EntityTypeBuilder<ProductFiles> builder)
    {
        builder.OwnsOne(x => x.File, options => 
        {
            options.Property(x => x.FileName).HasColumnName("FileName");
            options.Property(x => x.FileGuid).HasColumnName("FileGuid");
            options.Property(x => x.FileExtension).HasColumnName("FileExtension");
            options.Property(x => x.FilePath).HasColumnName("FilePath");
        });
    }
}
