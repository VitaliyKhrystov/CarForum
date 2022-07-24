﻿// <auto-generated />
using System;
using CarForum.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CarForum.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20220721110945__initial2")]
    partial class _initial2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.26")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CarForum.Domain.Entities.Response", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Reply")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TopicFieldID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TopicFieldID");

                    b.ToTable("Responses");
                });

            modelBuilder.Entity("CarForum.Domain.Entities.TopicField", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("QuestionExtension")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QuestionShort")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TopicFields");
                });

            modelBuilder.Entity("CarForum.Domain.Entities.Response", b =>
                {
                    b.HasOne("CarForum.Domain.Entities.TopicField", "TopicField")
                        .WithMany("Responces")
                        .HasForeignKey("TopicFieldID");
                });
#pragma warning restore 612, 618
        }
    }
}
