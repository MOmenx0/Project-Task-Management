using System;
using ProjectTaskManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ProjectTaskManagement.Infrastructure.Migrations;

[DbContext(typeof(ApplicationDbContext))]
partial class ApplicationDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "9.0.0")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

        modelBuilder.Entity("ProjectTaskManagement.Domain.Entities.Project", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("int");

            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

            b.Property<DateTime>("CreatedAt")
                .HasColumnType("datetime2");

            b.Property<string>("Description")
                .IsRequired()
                .HasMaxLength(2000)
                .HasColumnType("nvarchar(2000)");

            b.Property<string>("Name")
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("nvarchar(200)");

            b.Property<int>("UserId")
                .HasColumnType("int");

            b.HasKey("Id");

            b.HasIndex("UserId");

            b.ToTable("Projects", (string)null);
        });

        modelBuilder.Entity("ProjectTaskManagement.Domain.Entities.ProjectTask", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("int");

            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

            b.Property<string>("Description")
                .IsRequired()
                .HasMaxLength(2000)
                .HasColumnType("nvarchar(2000)");

            b.Property<DateTime?>("DueDate")
                .HasColumnType("datetime2");

            b.Property<int>("Priority")
                .HasColumnType("int");

            b.Property<int>("ProjectId")
                .HasColumnType("int");

            b.Property<int>("Status")
                .HasColumnType("int");

            b.Property<string>("Title")
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("nvarchar(200)");

            b.HasKey("Id");

            b.HasIndex("ProjectId");

            b.ToTable("Tasks", (string)null);
        });

        modelBuilder.Entity("ProjectTaskManagement.Domain.Entities.User", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("int");

            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

            b.Property<string>("Email")
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnType("nvarchar(256)");

            b.Property<string>("PasswordHash")
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            b.HasKey("Id");

            b.HasIndex("Email")
                .IsUnique();

            b.ToTable("Users", (string)null);
        });

        modelBuilder.Entity("ProjectTaskManagement.Domain.Entities.Project", b =>
        {
            b.HasOne("ProjectTaskManagement.Domain.Entities.User", "User")
                .WithMany("Projects")
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("User");
        });

        modelBuilder.Entity("ProjectTaskManagement.Domain.Entities.ProjectTask", b =>
        {
            b.HasOne("ProjectTaskManagement.Domain.Entities.Project", "Project")
                .WithMany("Tasks")
                .HasForeignKey("ProjectId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("Project");
        });

        modelBuilder.Entity("ProjectTaskManagement.Domain.Entities.Project", b =>
        {
            b.Navigation("Tasks");
        });

        modelBuilder.Entity("ProjectTaskManagement.Domain.Entities.User", b =>
        {
            b.Navigation("Projects");
        });
#pragma warning restore 612, 618
    }
}
