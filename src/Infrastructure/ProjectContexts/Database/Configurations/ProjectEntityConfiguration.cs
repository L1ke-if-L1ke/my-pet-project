using Domain.ProjectContexts;
using Domain.ProjectContexts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations
{
    //наследуем интерфейс IEntityTypeConfiguration<T>.
    //в Generic аргументе устанавливаем класс, который конфигурируем
    public sealed class ProjectEntityConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            //указываем с какой таблицей связять
            //или какую таблицу создать для этого класса
            builder.ToTable("projects");

            //указываем, какое свойство класса будет ключом в таблице
            builder.HasKey(x => x.Id).HasName("pk_projects");

            //конфигурируем работу со свойствами, где свойства - кастомный класс из 1 поля
            builder.Property(x => x.Name).HasColumnName("name").IsRequired().HasMaxLength(ProjectName.MAX_PROJECT_NAME_LENGTH).HasConversion(toDb => toDb.Value, fromDb => ProjectName.Create(fromDb));
            builder.HasIndex(x => x.Name).IsUnique();
            builder
                .Property(x => x.Description)
                .HasColumnName("descriprion")
                .IsRequired()
                .HasMaxLength(ProjectDescription.MAX_PROJECT_DESCRIPTION_LENGTH)
                .HasConversion(toDb =>toDb.Value, fromDb => ProjectDescription.Create(fromDb));
            //конфигурируем работу с ключом
            builder
                .Property(x => x.Id)
                .HasColumnName("id")
                .HasConversion(toDb => toDb.Value, fromDb => ProjectId.Create(fromDb));
            
            //конфигурируем работу со свойствами, где свойства - сложный объект из нескольких полей
            builder.ComplexProperty(
                x => x.LifeTime,
                cpb =>
                {
                    cpb.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
                    cpb.Property(x => x.FinishedAt).HasColumnName("finished_at").IsRequired(false);
                });
            //конфигурируем связь 1 ко многим
            //1 проект = много задач
            builder.HasMany(x => x.Tasks).WithOne(t => t.Project).HasForeignKey(t => t.ProjectId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        }
    }
}
