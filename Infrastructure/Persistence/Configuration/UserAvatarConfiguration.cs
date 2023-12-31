﻿using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class UserAvatarConfiguration : IEntityTypeConfiguration<UserAvatar>
{
    public void Configure(EntityTypeBuilder<UserAvatar> builder)
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