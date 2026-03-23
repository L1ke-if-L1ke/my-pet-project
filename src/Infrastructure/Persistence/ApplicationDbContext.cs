using Domain.ProjectContexts;
using Domain.ProjectContexts.Entities;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public DbSet<Project> Projects => Set<Project>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Игнорируем типы, которые не должны быть таблицами
        modelBuilder.Ignore<ProjectTaskMemberInfo>();
        modelBuilder.Ignore<ProjectTaskStatusInfo>();
        modelBuilder.Ignore<ProjectTaskInfo>();

        modelBuilder.Entity<Project>(builder =>
        {
            builder.HasKey(p => p.Id);

            // === Конвертация Value Objects проекта ===
            builder.Property(p => p.Id)
                .HasConversion(
                    id => id.Value,
                    value => ProjectId.Create(value));

            builder.Property(p => p.Name)
                .HasConversion(
                    v => v.Value,
                    v => ProjectName.Create(v))
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasConversion(
                    v => v.Value,
                    v => ProjectDescription.Create(v))
                .HasMaxLength(500);

            // ProjectLifeTime: Owned Type
            builder.OwnsOne(p => p.LifeTime, lifeTimeBuilder =>
            {
                lifeTimeBuilder.Property(lt => lt.CreatedAt)
                    .HasColumnName("CreatedAt")
                    .IsRequired();
                lifeTimeBuilder.Property(lt => lt.FinishedAt)
                    .HasColumnName("FinishedAt") 
                    .IsRequired(false);
            });

            // === Owned collection: ProjectTask ===
            builder.OwnsMany(p => p.Tasks, taskBuilder =>
            {
                // Ключ задачи
                taskBuilder.HasKey(t => t.Id);

                taskBuilder.Property(t => t.Id)
                    .HasConversion(
                        id => id.Value,
                        value => ProjectTaskId.Create(value));

                // Внешний ключ на проект (обязательно для OwnsMany!)
                taskBuilder.WithOwner().HasForeignKey("ProjectId");

                // Маппим простой скалярный свойство, если есть
                // taskBuilder.Property(t => t.SomeSimpleProperty).IsRequired();

                // Игнорируем навигацию обратно на Project (чтобы не было цикла)
                taskBuilder.Ignore(t => t.Project);

                // Игнорируем сложные вложенные объекты, которые не нужно хранить
                taskBuilder.Ignore(t => t.Limit);
                taskBuilder.Ignore(t => t.StatusInfo);
                taskBuilder.Ignore(t => t.Information);
                taskBuilder.Ignore(t => t.TaskMembers);

                // Имя таблицы для задач
                taskBuilder.ToTable("ProjectTasks");
            });
        });
    }
}