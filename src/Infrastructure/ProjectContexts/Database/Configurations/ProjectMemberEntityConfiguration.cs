using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.ProjectContexts.Entities;

namespace Infrastructure.Database.Configurations
{
    public sealed class ProjectMemberEntityConfiguration : IEntityTypeConfiguration<ProjectTaskMemberInfo>
    {
        public void Configure(EntityTypeBuilder<ProjectTaskMemberInfo> builder)
        {
            builder.ToTable("project_member_info");

            builder.HasKey(x => x.MemberId);

            builder.Property(x => x.MemberId).HasColumnName("member_id");

            builder.Property(x => x.MemberEmail).HasColumnName("member_email");
            builder.HasIndex(x => x.MemberEmail).IsUnique();
            builder.HasIndex(x => x.MemberLogin).IsUnique();

            builder.Property(x => x.MemberLogin).HasColumnName("member_login");
        }
    }
}
