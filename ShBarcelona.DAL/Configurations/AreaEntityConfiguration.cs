using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShBarcelona.DAL.Entities;

namespace ShBarcelona.DAL.Configurations
{
    public class AreaEntityConfiguration : IEntityTypeConfiguration<AreaEntity>
    {
        public void Configure(EntityTypeBuilder<AreaEntity> builder)
        {
            builder.ToTable("Area");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).UseMySqlIdentityColumn();
            builder.HasIndex(p => p.Name).IsUnique();
            builder.Property(a => a.ResponsableId).IsRequired();
        }
    }
}
