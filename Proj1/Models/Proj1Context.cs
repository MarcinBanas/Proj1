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

            Console.WriteLine("Podaj nazwe aktywa(max 10 znaków):");
            Console.WriteLine("3-Wróć");
            string name = Console.ReadLine();
            int a;
            var res = int.TryParse(name, out a);
            if (res) { return; }
            var nameCounter = name;
            var count = nameCounter.Length;
            while (count < 10)
            {
                nameCounter = nameCounter + " ";
                count = nameCounter.Length;
            }
            var query = Aktywas.Where(a => a.NazwaAktywa == name).ToList().FirstOrDefault();
            if (query==null)
            {
                Console.WriteLine("Podaj kod aktywa:");
                string kod = Console.ReadLine();
                var Aktywo = new Aktywa()
                {
                    NazwaAktywa = name,
                    KodAktywa = kod
                };
                Aktywas.Add(Aktywo);
                Console.WriteLine($"Dodano aktywo:({name}) kod:({kod})");
            }
            else
            { Console.WriteLine("Już Istnieje"); }
        }
        public void UsunAktywo()
        {

            Console.WriteLine("Podaj nazwe aktywa do usunięcia:");
            Console.WriteLine("3-Wróć");
            string? name = Console.ReadLine();
            int a;
            var res = int.TryParse(name, out a);
            if (res) { return; }
            var aktywoDoUsuniecia = Aktywas.Where(a => a.NazwaAktywa == name).ToList();
            if (aktywoDoUsuniecia.Count() == 0)
            {
                Console.WriteLine("Nie znaleziono dokładnej nazwy aktywa");
            }
            if (aktywoDoUsuniecia.Count() > 1)
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
            if (aktywoDoUsuniecia.Count() == 1)
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
        public void DodajNotowanie()
        {
            Console.WriteLine("Podaj nazwe aktywa dla którego chcesz dodać notowanie:");
            Console.WriteLine("3-Wróć");
            string name = Console.ReadLine();
            int a;
            var res = int.TryParse(name, out a);
            if (res) { return; }
            var Aktywa = Aktywas.Where(a => a.NazwaAktywa == name).ToList();
            var id = Aktywa.First().Idaktywa;
            Console.WriteLine("Podaj cene otwarcia:");
            var cenaOtwarcia = Convert.ToDecimal(Console.ReadLine());
            Console.WriteLine("Podaj cene zamkniecia:");
            var cenaZamkniecia = Convert.ToDecimal(Console.ReadLine());
            Console.WriteLine("Podaj date i godzine(dd/mm/yyyy gg:mm:ss):");
            var cenaDataIGodzina = Convert.ToDateTime(Console.ReadLine());
            Console.WriteLine("Podaj cene min:");
            var cenaMin = Convert.ToDecimal(Console.ReadLine());
            Console.WriteLine("Podaj cene max:");
            var cenaMax = Convert.ToDecimal(Console.ReadLine());
            var notowanie = new Notowanium()
            {
                CenaOtwarcia = cenaOtwarcia,
                CenaZamkniecia = cenaZamkniecia,
                DataIgodzina = cenaDataIGodzina,
                CenaMax = cenaMax,
                CenaMin = cenaMin,
                Idaktywa = id
            };
            Notowania.Add(notowanie);
            Console.WriteLine("Dodano notowanie");
        }
        public void UsunNotowanie()
        {
            Console.WriteLine("Podaj nazwe aktywa dla którego chcesz usunąć notowanie:");
            Console.WriteLine("3-Wróć");
            var nazwaAktywa = Console.ReadLine();
            int a;
            var res = int.TryParse(nazwaAktywa, out a);
            if (res) { return; }
            var Aktywa = Aktywas.Where(a => a.NazwaAktywa == nazwaAktywa).ToList();
            var id = Aktywa.First().Idaktywa;
            var notowania = Notowania.Where(i => i.Idaktywa == id).ToList();
            foreach (var item in notowania)
            {
                Console.WriteLine($"ID:{ item.Idnotowania},{item.CenaMax},{item.CenaMin},{item.DataIgodzina},{item.CenaOtwarcia},{item.CenaZamkniecia}");
            }
            Console.WriteLine("Podaj id notowania do usunięcia:");
            var idUsun=Convert.ToInt32(Console.ReadLine());
            var usunNotowanie = Notowania.First(u => u.Idnotowania == idUsun);
            Notowania.Remove(usunNotowanie);
        }
        public void WyswietlAktywa()
        {
            var Aktywa = Aktywas.ToList();
            var iloscAktyw = Aktywa.Count();
            var pageMultiplication = 10;
            var choose = 0;
            foreach (var item in Aktywa.Skip(0).Take(pageMultiplication))
            {
                Console.WriteLine($"ID: {item.Idaktywa}, Nazwa: {item.NazwaAktywa}, Kod: {item.KodAktywa}");
            }
            var actualPage = 0;
            Console.WriteLine("1-nastepna strona");
            Console.WriteLine("2-poprzednia strona");
            Console.WriteLine("3-wyjscie");
            while (choose != 3)
            {
                Console.WriteLine($"Strona {actualPage}");
                choose = Convert.ToInt32(Console.ReadLine());
                if (choose == 1) { actualPage++; }
                if (choose == 2 && actualPage > 0) { actualPage--; }
                foreach (var item in Aktywa.Skip(actualPage * pageMultiplication).Take(pageMultiplication))
                {
                    Console.WriteLine($"ID: {item.Idaktywa}, Nazwa: {item.NazwaAktywa}, Kod: {item.KodAktywa}");
                }

            }
        }
        public void WyswietlNotowania()
        {
            Console.WriteLine("Podaj nazwe aktywa dla którego wyswietlić notowania:");
            Console.WriteLine("3-Wróć");
            var nazwaAktywa = Console.ReadLine();
            int a;
            var res=int.TryParse(nazwaAktywa,out a);
            if (res) { return; }
            var id=Aktywas.Where(x => x.NazwaAktywa == nazwaAktywa).ToList().First().Idaktywa;
            var notowania=Notowania.Where(a=>a.Idaktywa==id).ToList();
            var iloscNotowan = notowania.Count();
            if (iloscNotowan == 0)
            {
                Console.WriteLine("Brak notowań");
                return;
            }
            var pageMultiplication = 10;
            var choose = 0;
            foreach (var item in notowania.Skip(0).Take(pageMultiplication))
            {
                Console.WriteLine($"Data: {item.DataIgodzina} ID: {item.Idnotowania}, CenaOtwarcia: {item.CenaOtwarcia}, CenaZamkniecia: {item.CenaZamkniecia}, CenaMin: {item.CenaMin}, CenaMax: {item.CenaMax}");
            }
            var actualPage = 0;
            Console.WriteLine("1-nastepna strona");
            Console.WriteLine("2-poprzednia strona");
            Console.WriteLine("3-wyjscie");
            while (choose != 3)
            {
                Console.WriteLine($"Strona {actualPage}");
                choose = Convert.ToInt32(Console.ReadLine());
                if (choose == 1) { actualPage++; }
                if (choose == 2 && actualPage > 0) { actualPage--; }
                foreach (var item in notowania.Skip(actualPage * pageMultiplication).Take(pageMultiplication))
                {
                    Console.WriteLine($"Data: {item.DataIgodzina} ID: {item.Idnotowania}, CenaOtwarcia: {item.CenaOtwarcia}, CenaZamkniecia: {item.CenaZamkniecia}, CenaMin: {item.CenaMin}, CenaMax: {item.CenaMax}");
                }

            }
        }
    }
}
