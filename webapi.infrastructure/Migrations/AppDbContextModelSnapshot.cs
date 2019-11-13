﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using webapi.infrastructure.Persistance;

namespace webapi.infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0");

            modelBuilder.Entity("webapi.core.Domain.Entities.Airline", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Airlines");
                });

            modelBuilder.Entity("webapi.core.Domain.Entities.Airport", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Airports");
                });

            modelBuilder.Entity("webapi.core.Domain.Entities.Customer", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("BookingCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("webapi.core.Domain.Entities.Date", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DepartureDate")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Dates");
                });

            modelBuilder.Entity("webapi.core.Domain.Entities.DateFlight", b =>
                {
                    b.Property<int>("DateId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FlightId")
                        .HasColumnType("TEXT");

                    b.Property<int>("SeatsLeft")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("DateId", "FlightId");

                    b.HasIndex("FlightId");

                    b.ToTable("DateFlights");
                });

            modelBuilder.Entity("webapi.core.Domain.Entities.Flight", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("AirlineId")
                        .HasColumnType("TEXT");

                    b.Property<string>("AirportFrom")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("AirportTo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("FlightTime")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SeatsCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("StartTime")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AirlineId");

                    b.HasIndex("AirportFrom");

                    b.HasIndex("AirportTo");

                    b.ToTable("Flights");
                });

            modelBuilder.Entity("webapi.core.Domain.Entities.FlightTicketCategory", b =>
                {
                    b.Property<int>("TicketCategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FlightId")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.HasKey("TicketCategoryId", "FlightId");

                    b.HasAlternateKey("FlightId", "TicketCategoryId");

                    b.ToTable("FlightTicketCategories");
                });

            modelBuilder.Entity("webapi.core.Domain.Entities.Luggage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("LuggageWeight")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Luggages");
                });

            modelBuilder.Entity("webapi.core.Domain.Entities.Order", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("CustomerId")
                        .HasColumnType("TEXT");

                    b.Property<int>("DateId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TicketCount")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("TEXT");

                    b.Property<int?>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("DateId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("webapi.core.Domain.Entities.Ticket", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("FlightId")
                        .HasColumnType("TEXT");

                    b.Property<int>("LuggageId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("OrderId")
                        .HasColumnType("TEXT");

                    b.Property<int>("PassengerGender")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PassengerName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<int>("TicketCategoryId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("FlightId");

                    b.HasIndex("LuggageId");

                    b.HasIndex("OrderId");

                    b.HasIndex("TicketCategoryId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("webapi.core.Domain.Entities.TicketCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("TicketCategories");
                });

            modelBuilder.Entity("webapi.core.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Identifier")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .HasColumnType("TEXT");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("webapi.core.Domain.Entities.DateFlight", b =>
                {
                    b.HasOne("webapi.core.Domain.Entities.Date", "Date")
                        .WithMany("DateFlights")
                        .HasForeignKey("DateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("webapi.core.Domain.Entities.Flight", "Flight")
                        .WithMany("DateFlights")
                        .HasForeignKey("FlightId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("webapi.core.Domain.Entities.Flight", b =>
                {
                    b.HasOne("webapi.core.Domain.Entities.Airline", "Airline")
                        .WithMany("Flights")
                        .HasForeignKey("AirlineId");

                    b.HasOne("webapi.core.Domain.Entities.Airport", "AirportFromData")
                        .WithMany()
                        .HasForeignKey("AirportFrom")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("webapi.core.Domain.Entities.Airport", "AirportToData")
                        .WithMany()
                        .HasForeignKey("AirportTo")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("webapi.core.Domain.Entities.FlightTicketCategory", b =>
                {
                    b.HasOne("webapi.core.Domain.Entities.Flight", "Flight")
                        .WithMany("FlightTicketCategories")
                        .HasForeignKey("FlightId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("webapi.core.Domain.Entities.TicketCategory", "TicketCategory")
                        .WithMany("FlightTicketCategories")
                        .HasForeignKey("TicketCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("webapi.core.Domain.Entities.Order", b =>
                {
                    b.HasOne("webapi.core.Domain.Entities.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId");

                    b.HasOne("webapi.core.Domain.Entities.Date", "Date")
                        .WithMany()
                        .HasForeignKey("DateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("webapi.core.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("webapi.core.Domain.Entities.Ticket", b =>
                {
                    b.HasOne("webapi.core.Domain.Entities.Flight", "Flight")
                        .WithMany()
                        .HasForeignKey("FlightId");

                    b.HasOne("webapi.core.Domain.Entities.Luggage", "Luggage")
                        .WithMany()
                        .HasForeignKey("LuggageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("webapi.core.Domain.Entities.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId");

                    b.HasOne("webapi.core.Domain.Entities.TicketCategory", "TicketCategory")
                        .WithMany()
                        .HasForeignKey("TicketCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
