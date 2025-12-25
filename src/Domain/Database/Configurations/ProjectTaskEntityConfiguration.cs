using Domain.ProjectContexts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Database.Configurations
{
    public sealed class ProjectTaskEntityConfiguration : IEntityTypeConfiguration<ProjectTask>
    {
        public void Configure(EntityTypeBuilder<ProjectTask> builder)
        {
            //таблица задачи проекта
            builder.ToTable("project_tasks");

            //ключ задач проекта
            builder.HasKey(x => x.Id).HasName("pk_project_tasks");

            //конфигурируем ключ
            builder
                .Property(x => x.Id)
                .HasColumnName("id")
                .HasConversion(toDb => toDb.Value, fromDb => ProjectTaskId.Create(fromDb));

            //конфигурируем внешний ключ к проекту
            builder
                .Property(x => x.ProjectId)
                .HasColumnName("project_id")
                .HasConversion(toDb => toDb.Value, fromDb => ProjectId.Create(fromDb));

            //конфигурируем сложный объект, который состоит из других сложных объектов
            builder.ComplexProperty(
                x => x.StatusInfo,
                cpb =>
                {
                    cpb.ComplexProperty(
                        s => s.Status,
                        statusBuilder =>
                        {
                            statusBuilder.Property(s => s.Name).HasColumnName("name").IsRequired();
                            statusBuilder.Property(s => s.Value).HasColumnName("value").IsRequired();
                        }
                    );

                    cpb.ComplexProperty(
                        s => s.Schedule,
                        scheduleBuilder =>
                        {
                            scheduleBuilder.Property(s => s.Created).HasColumnName("created_at");
                            scheduleBuilder
                                .Property(s => s.Closed)
                                .HasColumnName("closed")
                                .IsRequired(false);
                        }
                        );
                }
            );
            builder.ComplexProperty(
                x => x.Information,
                cpb =>
                {
                    cpb.Property(s => s.Description)
                    .HasColumnName("description")
                    .HasMaxLength(ProjectTaskInfo.MAX_DESCRIPTION_LENGTH)
                    .IsRequired();
                }
           );
            //конфигурируем связи 1 задача много участников задачи
            builder
                 .HasMany(x => x.TaskMembers)
                 .WithOne(x => x.Task)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
