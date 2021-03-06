// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WebApplication2.Models;

namespace WebApplication2.Migrations
{
    [DbContext(typeof(MarkerContext))]
    partial class MarkerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.18")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("WebApplication2.Models.Wallet", b =>
                {
                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<string>("AmountGas")
                        .HasColumnType("text");

                    b.Property<string>("Comission")
                        .HasColumnType("text");

                    b.Property<string>("ComissionGas")
                        .HasColumnType("text");

                    b.Property<string>("InputAmount")
                        .HasColumnType("text");

                    b.Property<string>("PrivateKey")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TransactionHash")
                        .HasColumnType("text");

                    b.HasKey("Address");

                    b.ToTable("Wallets");
                });
#pragma warning restore 612, 618
        }
    }
}
