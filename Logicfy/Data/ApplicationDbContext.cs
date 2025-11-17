using Logicfy.Models;
using Microsoft.EntityFrameworkCore;

namespace Logicfy.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        // -----------------------------
        // PROGRAM YAPISI TABLOLARI
        // -----------------------------
        public DbSet<ProgramlamaDili> ProgramlamaDilleri { get; set; }
        public DbSet<Unite> Uniteler { get; set; }
        public DbSet<Kisim> Kismlar { get; set; }
        public DbSet<Ders> Dersler { get; set; }

        // -----------------------------
        // SORU VE SORU TİPLERİ
        // -----------------------------
        public DbSet<Soru> Sorular { get; set; }
        public DbSet<SoruSecenek> SoruSecenekleri { get; set; }
        public DbSet<SoruKelimeBlok> SoruKelimeBloklari { get; set; }
        public DbSet<SoruFonksiyonCozum> SoruFonksiyonCozumleri { get; set; }
        public DbSet<SoruCanliPreview> SoruCanliPreviews { get; set; }

        // -----------------------------
        // KULLANICI / İLERLEME
        // -----------------------------
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<KullaniciDersKaydi> KullaniciDersKayitlari { get; set; }
        public DbSet<KullaniciDersIlerleme> KullaniciDersIlerlemeleri { get; set; }
        public DbSet<KullaniciUnitProgress> KullaniciUnitProgressleri { get; set; }
        public DbSet<KullaniciKisimProgress> KullaniciKisimProgressleri { get; set; }
        public DbSet<KullaniciSoruCevap> KullaniciSoruCevaplari { get; set; }
        public DbSet<KullaniciXpLog> KullaniciXpLoglari { get; set; }
        public DbSet<KullaniciGunlukSeri> KullaniciGunlukSerileri { get; set; }

        // -----------------------------
        // ANALİTİK TABLOLARI
        // -----------------------------
        public DbSet<DersPerformansAnalitik> DersAnalitigi { get; set; }
        public DbSet<SoruAnalitik> SoruAnalitigi { get; set; }

        // -----------------------------
        // TAKİP TABLOLARI
        // -----------------------------
        public DbSet<DersTakip> DersTakipleri { get; set; }
        public DbSet<UniteTakip> UniteTakipleri { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ============================================
            // PROGRAMLAMA DİLİ
            // ============================================

            modelBuilder.Entity<ProgramlamaDili>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Ad).IsRequired().HasMaxLength(200);

                e.HasMany(x => x.Uniteler)
                 .WithOne(u => u.ProgramlamaDili)
                 .HasForeignKey(u => u.ProgramlamaDiliId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ============================================
            // UNITE
            // ============================================

            modelBuilder.Entity<Unite>(e =>
            {
                e.HasKey(x => x.Id);

                e.HasMany(x => x.Kismlar)
                 .WithOne(x => x.Unite)
                 .HasForeignKey(x => x.UniteId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ============================================
            // KISIM
            // ============================================

            modelBuilder.Entity<Kisim>(e =>
            {
                e.HasKey(x => x.Id);

                e.HasMany(x => x.Dersler)
                 .WithOne(x => x.Kisim)
                 .HasForeignKey(x => x.KisimId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ============================================
            // DERS
            // ============================================

            modelBuilder.Entity<Ders>(e =>
            {
                e.HasKey(x => x.Id);

                e.HasMany(x => x.Sorular)
                 .WithOne(x => x.Ders)
                 .HasForeignKey(x => x.DersId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ============================================
            // SORU (ANA TABLO)
            // ============================================

            modelBuilder.Entity<Soru>(e =>
            {
                e.HasKey(x => x.Id);

                e.HasOne(x => x.DogruCevap)
                 .WithMany()
                 .HasForeignKey(x => x.DogruCevapId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ============================================
            // SORU ŞIKLARI
            // ============================================

            modelBuilder.Entity<SoruSecenek>(e =>
            {
                e.HasKey(x => x.Id);

                e.HasOne(x => x.Soru)
                 .WithMany()
                 .HasForeignKey(x => x.SoruId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ============================================
            // KOD BLOK
            // ============================================

            modelBuilder.Entity<SoruKelimeBlok>(e =>
            {
                e.HasKey(x => x.Id);

                e.HasOne(x => x.Soru)
                 .WithOne()
                 .HasForeignKey<SoruKelimeBlok>(x => x.SoruId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ============================================
            // FONKSIYON ÇÖZÜM
            // ============================================

            modelBuilder.Entity<SoruFonksiyonCozum>(e =>
            {
                e.HasKey(x => x.Id);

                e.HasOne(x => x.Soru)
                 .WithMany()
                 .HasForeignKey(x => x.SoruId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ============================================
            // CANLI PREVIEW
            // ============================================

            modelBuilder.Entity<SoruCanliPreview>(e =>
            {
                e.HasKey(x => x.Id);

                e.HasOne(x => x.Soru)
                 .WithOne()
                 .HasForeignKey<SoruCanliPreview>(x => x.SoruId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ============================================
            // KULLANICILAR
            // ============================================

            modelBuilder.Entity<Kullanici>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Email).IsRequired();
            });

            // ============================================
            // KULLANICI DERS KAYDI
            // ============================================

            modelBuilder.Entity<KullaniciDersKaydi>(e =>
            {
                e.HasKey(x => x.Id);

                e.HasOne(x => x.Kullanici)
                 .WithMany(x => x.DersKayitlari)
                 .HasForeignKey(x => x.KullaniciId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ============================================
            // KULLANICI İLERLEMELERİ
            // ============================================

            modelBuilder.Entity<KullaniciDersIlerleme>(e =>
            {
                e.HasKey(x => x.Id);
            });

            modelBuilder.Entity<KullaniciUnitProgress>(e =>
            {
                e.HasKey(x => x.Id);
            });

            modelBuilder.Entity<KullaniciKisimProgress>(e =>
            {
                e.HasKey(x => x.Id);
            });

            modelBuilder.Entity<KullaniciSoruCevap>(e =>
            {
                e.HasKey(x => x.Id);
            });

            modelBuilder.Entity<KullaniciXpLog>(e =>
            {
                e.HasKey(x => x.Id);
            });

            modelBuilder.Entity<KullaniciGunlukSeri>(e =>
            {
                e.HasKey(x => x.Id);
            });

            // ============================================
            // ANALİTİK TABLOLAR
            // ============================================

            modelBuilder.Entity<DersPerformansAnalitik>(e =>
            {
                e.HasKey(x => x.Id);
            });

            modelBuilder.Entity<SoruAnalitik>(e =>
            {
                e.HasKey(x => x.Id);
            });

            // ============================================
            // TAKİP TABLOLARI
            // ============================================

            modelBuilder.Entity<DersTakip>(e =>
            {
                e.HasKey(x => x.Id);
            });

            modelBuilder.Entity<UniteTakip>(e =>
            {
                e.HasKey(x => x.Id);
            });

        }
    }
}
