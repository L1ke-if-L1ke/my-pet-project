using Domain.ProjectContexts;
using Domain.ProjectContexts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ProjectContexts.Database.Configurations;

public sealed class ProjectEntityConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("projects");
        builder.HasKey(x => x.Id).HasName("pk_projects");

        // === Value Objects проекта ===
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .HasConversion(
                id => id.Value,
                value => ProjectId.Create(value));

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(ProjectName.MAX_PROJECT_NAME_LENGTH)
            .HasConversion(
                vo => vo.Value,
                value => ProjectName.Create(value));

        builder.HasIndex(x => x.Name).IsUnique();

        builder.Property(x => x.Description)
            .HasColumnName("description")
            .IsRequired(false)
            .HasMaxLength(ProjectDescription.MAX_PROJECT_DESCRIPTION_LENGTH)
            .HasConversion(
                vo => vo.Value,
                value => ProjectDescription.Create(value));

        // === Complex Property: ProjectLifeTime ===
        builder.ComplexProperty(
            x => x.LifeTime,
            cpb =>
            {
                cpb.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
                cpb.Property(x => x.FinishedAt).HasColumnName("finished_at").IsRequired(false);
            });

        // === Owned Collection: ProjectTask ===
        builder.OwnsMany<ProjectTask>(p => p.Tasks, taskBuilder =>
        {
            taskBuilder.ToTable("project_tasks");
            taskBuilder.HasKey(t => t.Id);

            // Ключ задачи
            taskBuilder.Property(t => t.Id)
                .HasColumnName("id")
                .HasConversion(
                    id => id.Value,
                    value => ProjectTaskId.Create(value));

            // Внешний ключ: свойство с конвертером
            taskBuilder.Property<ProjectId>("ProjectId")
                .HasColumnName("project_id")
                .HasConversion(
                    id => id.Value,
                    value => ProjectId.Create(value));

            taskBuilder.WithOwner().HasForeignKey("ProjectId");

            // Value Object: Information
            taskBuilder.Property(t => t.Information)
                .HasColumnName("task_description")
                .HasMaxLength(ProjectTaskInfo.MAX_DESCRIPTION_LENGTH)
                .IsRequired()
                .HasConversion(
                    info => info.Description,
                    desc => ProjectTaskInfo.Create(desc));

            // Value Object: Limit
            taskBuilder.Property(t => t.Limit)
                .HasColumnName("members_limit")
                .IsRequired()
                .HasConversion(
                    limit => limit.Value,
                    value => ProjectTaskMembersLimit.Create(value));

            // Owned Type: StatusInfo
            taskBuilder.OwnsOne<ProjectTaskStatusInfo>(t => t.StatusInfo, statusInfoBuilder =>
            {
                statusInfoBuilder.OwnsOne<ProjectTaskStatus>(s => s.Status, statusBuilder =>
                {
                    statusBuilder.Property(s => s.Name).HasColumnName("status_name").IsRequired();
                    statusBuilder.Property(s => s.Value).HasColumnName("status_value").IsRequired();
                });

                statusInfoBuilder.OwnsOne<ProjectTaskSchedule>(s => s.Schedule, scheduleBuilder =>
                {
                    scheduleBuilder.Property(s => s.Created).HasColumnName("task_created_at");
                    scheduleBuilder.Property(s => s.Closed).HasColumnName("task_closed_at").IsRequired(false);
                });
            });

            // Owned Collection: TaskMembers
            taskBuilder.OwnsMany<ProjectTaskMemberInfo>(t => t.TaskMembers, memberBuilder =>
            {
                memberBuilder.ToTable("project_task_members");
                memberBuilder.HasKey(m => m.MemberId);

                memberBuilder.Property(m => m.MemberId)
                    .HasColumnName("member_id")
                    .HasConversion(
                        id => id.Value,
                        value => ProjectMemberId.Create(value));

                memberBuilder.Property(m => m.MemberEmail).HasColumnName("member_email").IsRequired();
                memberBuilder.Property(m => m.MemberLogin).HasColumnName("member_login").IsRequired();
                memberBuilder.Ignore(m => m.Task);
            });

            taskBuilder.Ignore(t => t.Project);
        });
    }
}