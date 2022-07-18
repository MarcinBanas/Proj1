using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Configuration;
using System.Linq;

namespace Proj1.Models
{
    public partial class Proj1Context : DbContext
    {
        public Proj1Context()
        {
        }

        public Proj1Context(DbContextOptions<Proj1Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Aktywa> Aktywas { get; set; } = null!;
        public virtual DbSet<Notowanium> Notowania { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["ProjBaza"].ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Aktywa>(entity =>
            {
                entity.HasKey(e => e.Idaktywa);

                entity.ToTable("Aktywa");

                entity.Property(e => e.Idaktywa).HasColumnName("IDAktywa");

                entity.Property(e => e.KodAktywa)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.NazwaAktywa)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Notowanium>(entity =>
            {
                entity.HasKey(e => e.Idnotowania);

                entity.Property(e => e.Idnotowania)
                    .ValueGeneratedNever()
                    .HasColumnName("IDNotowania");

                entity.Property(e => e.CenaMax).HasColumnType("money");

                entity.Property(e => e.CenaMin).HasColumnType("money");

                entity.Property(e => e.CenaOtwarcia).HasColumnType("money");

                entity.Property(e => e.CenaZamkniecia)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.DataIgodzina)
                    .HasColumnType("datetime")
                    .HasColumnName("DataIGodzina");

                entity.Property(e => e.Idaktywa).HasColumnName("IDAktywa");

                entity.HasOne(d => d.IdaktywaNavigation)
                    .WithMany(p => p.Notowania)
                    .HasForeignKey(d => d.Idaktywa)
                    .HasConstraintName("FK_Notowania_Aktywa");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public void DodajAktywo()
        {
            
            Console.WriteLine("Podaj nazwe aktywa:");
            string name = Console.ReadLine();
            Console.WriteLine("Podaj kod aktywa:");
            string kod = Console.ReadLine();
            var Aktywo = new Aktywa()
            {
                NazwaAktywa = name,
                KodAktywa=kod
            };
            Aktywas.Add(Aktywo);
            Console.WriteLine($"Dodano aktywo:({name}) kod:({kod})");
        }
        public void UsunAktywo()
        {

            Console.WriteLine("Podaj nazwe aktywa do usunięcia:");
            string? name = Console.ReadLine();
            var aktywoDoUsuniecia = Aktywas.Where(a => a.NazwaAktywa == name).ToList();
            if (aktywoDoUsuniecia.Count() == 0)
            {
                Console.WriteLine("Nie znaleziono dokładnej nazwy aktywa");  
            }
            if (aktywoDoUsuniecia.Count()>1)
            {
                Console.WriteLine("Podaj dokladne id");
                foreach (var item in aktywoDoUsuniecia)
                {
                    Console.WriteLine($"{item.Idaktywa}, {item.NazwaAktywa}, {item.KodAktywa}");
                }
                Console.WriteLine();
                var id = Convert.ToInt32(Console.ReadLine());
                foreach (var nrid in aktywoDoUsuniecia)
                {
                    if (nrid.Idaktywa == id)
                    {
                        Console.WriteLine($"czy napewno chcesz usunąć aktywo o id:{id} T/N");
                        var TakNie = Console.ReadLine();
                        if (TakNie == "T")
                        {
                            var aktywo = Aktywas.First(i => i.Idaktywa == id);
                            Aktywas.Remove(aktywo);
                            Console.WriteLine("Usunieto");
                            return;
                        }
                        return;
                    }
                }
                 Console.WriteLine("zly nr id!");
            }
            if(aktywoDoUsuniecia.Count() == 1)
            {
                var aktywo = Aktywas.First(i => i.NazwaAktywa == name);
                Console.WriteLine($"czy napewno chcesz usunąć aktywo o nazwie:{name} T/N");
                var TakNie = Console.ReadLine();
                if (TakNie == "T")
                {
                    Aktywas.Remove(aktywo);
                    Console.WriteLine("Usunieto");
                    return;
                }
                return;
            }
            
            
        }
    }
}
