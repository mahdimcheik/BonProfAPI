using System.Reflection.Emit;
using BonProf.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BonProf.Models;
using BonProf.Models;
using BonProf.Utilities;

namespace BonProf.Contexts;

public class MainContext : IdentityDbContext<UserApp, RoleApp, Guid>
{
    public DbSet<UserApp> Users { get; set; }
    public DbSet<RoleApp> Roles { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Gender> Genders { get; set; }

    // Profile entities
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Student> Students { get; set; }
    // public DbSet<Profile> Profiles { get; set; }

    // Related entities
    public DbSet<Address> Addresses { get; set; }
    public DbSet<TypeAddress> TypeAddresses { get; set; }
    public DbSet<Cursus> Cursuses { get; set; }
    public DbSet<Experience> Experiences { get; set; }
    public DbSet<Formation> Formations { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<CategoryCursus> CategoryCursuses { get; set; }
    public DbSet<LevelCursus> LevelCursuses { get; set; }

    // reservations
    public DbSet<Slot> Slots { get; set; }
    public DbSet<TypeSlot> TypeSlots { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }

    // payments and transactions
    public DbSet<Payment> Payments { get; set; } // paiement d'une commande
    public DbSet<TeacherPayout> TeacherPayouts { get; set; } // une ligne par professeur (les comptes des profs)
    public DbSet<TeacherWalletTransaction> TeacherWalletTransactions { get; set; } // historique des transactions du portefeuille, acomptes, retraits, remboursements
    public DbSet<TypeTeacherTransaction> TypeTransactions { get; set; } // types de transactions (acompte, retrait, remboursement)
    public DbSet<StatusTransaction> StatusTransactions { get; set; } // statuts des transactions (en attente, complété, échoué)
    public DbSet<PaymentMethod> PaymentMethods { get; set; } // méthodes de paiement (carte bancaire, PayPal, etc.)

    public MainContext(DbContextOptions options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<UserApp>().ToTable("Users");
        builder.Entity<RoleApp>().ToTable("Roles");

        // Global query filters (must stay in Fluent API)
        builder.Entity<Cursus>().HasQueryFilter(d => d.ArchivedAt == null);
        builder.Entity<Formation>().HasQueryFilter(d => d.ArchivedAt == null);
        builder.Entity<Address>().HasQueryFilter(d => d.ArchivedAt == null);
        builder.Entity<Product>().HasQueryFilter(d => d.ArchivedAt == null);
        builder.Entity<Slot>().HasQueryFilter(d => d.ArchivedAt == null);

        // Seed Roles
        List<RoleApp> roles = new()
        {
            new RoleApp
            {
                Id = HardCode.ROLE_SUPER_ADMIN,
                Name = "SuperAdmin",
                NormalizedName = "SUPERADMIN",
                Color =  "#3d82f2",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                ConcurrencyStamp = "SUPERADMIN-STAMP-2025",
            },
            new RoleApp
            {
                Id = HardCode.ROLE_ADMIN,
                Name = "Admin",
                Color =  "#31bdd6",
                NormalizedName = "ADMIN",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                ConcurrencyStamp = "SUPERADMIN-STAMP-2025",
            },
            new RoleApp
            {
                Id = HardCode.ROLE_TEACHER,
                Name = "Teacher",
                Color =  "#31d68f",
                NormalizedName = "TEACHER",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                ConcurrencyStamp = "SUPERADMIN-STAMP-2025",
            },
            new RoleApp
            {
                Id = HardCode.ROLE_STUDENT,
                Name = "Student",
                NormalizedName = "STUDENT",
                Color =  "#f57ad0",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                ConcurrencyStamp = "SUPERADMIN-STAMP-2025",
            },
        };
        builder.Entity<RoleApp>().HasData(roles);
        // Seed Genders
        List<Gender> genders = new()
        {
            new Gender
            {
                Id = HardCode.GENDER_FEMALE,
                Name = "Female",
                Color = "#ff69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new Gender
            {
                Id = HardCode.GENDER_MALE,
                Name = "Male",
                Color = "#fa69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new Gender
            {
                Id = HardCode.GENDER_OTHER,
                Name = "Other",
                Color = "#ab69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
        };

        builder.Entity<Gender>().HasData(genders);

        // Seed Account status
        List<StatusAccount> accountStatuses = new()
        {
            new StatusAccount
            {
                Id = HardCode.ACCOUNT_ACTIVE,
                Name = "Active",
                Color = "#ff69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new StatusAccount
            {
                Id = HardCode.ACCOUNT_PENDING,
                Name = "Pending",
                Color = "#fa69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new StatusAccount
            {
                Id = HardCode.ACCOUNT_BANNED,
                Name = "Banned",
                Color = "#ab69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
        };

        // Seed type address
        List<TypeAddress> typeAddresses = new()
        {
            new TypeAddress
            {
                Id = HardCode.TYPE_ADDRESS_HOME,
                Name = "Home",
                Color = "#ff69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new TypeAddress
            {
                Id = HardCode.TYPE_ADDRESS_BILLING,
                Name = "Billing",
                Color = "#fa69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
        };

        builder.Entity<TypeAddress>().HasData(typeAddresses);

        builder.Entity<StatusAccount>().HasData(accountStatuses);

        // Seed Transaction Status
        List<StatusTransaction> statusTransactions = new()
        {
            new StatusTransaction
            {
                Id = HardCode.STATUS_TRANSACTION_PENDING,
                Name = "Pending",
                Color = "#ff69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new StatusTransaction
            {
                Id = HardCode.STATUS_TRANSACTION_PAID,
                Name = "Paid",
                Color = "#fa69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new StatusTransaction
            {
                Id = HardCode.STATUS_TRANSACTION_FAILED,
                Name = "Failed",
                Color = "#ab69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
        };

        builder.Entity<StatusTransaction>().HasData(statusTransactions);

        // Seed Transaction Status
        List<TypeTeacherTransaction> typeTeacherTransactions = new()
        {
            new TypeTeacherTransaction
            {
                Id = HardCode.TYPE_TEACHER_TRANSACTION_PAYMENT,
                Name = "Payment",
                Color = "#ff69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new TypeTeacherTransaction
            {
                Id = HardCode.TYPE_TEACHER_TRANSACTION_PAYOUT,
                Name = "Payout",
                Color = "#fa69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new TypeTeacherTransaction
            {
                Id = HardCode.TYPE_TEACHER_TRANSACTION_REFUND,
                Name = "Refund",
                Color = "#ab69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
        };

        builder.Entity<TypeTeacherTransaction>().HasData(typeTeacherTransactions);

        // seed reservations status
        List<StatusReservation> statusReservations = new()
        {
            new StatusReservation
            {
                Id = HardCode.RESERVATION_PENDING,
                Name = "Pendind",
                Color = "#ff69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new StatusReservation
            {
                Id = HardCode.RESERVATION_ACCEPTED,
                Name = "Accepted",
                Color = "#fa69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new StatusReservation
            {
                Id = HardCode.RESERVATION_DONE,
                Name = "Done",
                Color = "#ab69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new StatusReservation
            {
                Id = HardCode.RESERVATION_REJECTED,
                Name = "Rejected",
                Color = "#ab69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
        };
        builder.Entity<StatusReservation>().HasData(statusReservations);

        // seed languages
        List<Language> languages = new()
        {
            new Language
            {
                Id = HardCode.LANGUAGE_ARAB,
                Name = "Arabe",
                Color = "#ff69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new Language
            {
                Id = HardCode.LANGUAGE_FRENCH,
                Name = "Francais",
                Color = "#fa69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new Language
            {
                Id = HardCode.LANGUAGE_ENGLISH,
                Name = "Anglais",
                Color = "#ab69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
        };

        builder.Entity<Language>().HasData(languages);

        // courses type + category + level seeding can be added here similarly

        List<LevelCursus> levelCursuses = new()
        {
            new LevelCursus
            {
                Id = HardCode.LEVEL_ALL,
                Name = "Tous niveaux",
                Color = "#ff69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new LevelCursus
            {
                Id = HardCode.LEVEL_BEGINNER,
                Name = "Débutant",
                Color = "#fa69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new LevelCursus
            {
                Id = HardCode.LEVEL_INTERMEDIATE,
                Name = "Intermédiaire",
                Color = "#ab69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new LevelCursus
            {
                Id = HardCode.LEVEL_ADVANCED,
                Name = "Avancé",
                Color = "#ab69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
        };

        builder.Entity<LevelCursus>().HasData(levelCursuses);

        List<CategoryCursus> categoryCursuses = new()
        {
            new CategoryCursus
            {
                Id = HardCode.CATEGORY_BACK,
                Name = "Back-end",
                Color = "#ff69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new CategoryCursus
            {
                Id = HardCode.CATEGORY_FRONT,
                Name = "Front-end",
                Color = "#fa69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new CategoryCursus
            {
                Id = HardCode.CATEGORY_TECHNICS,
                Name = "Techniques",
                Color = "#ab69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new CategoryCursus
            {
                Id = HardCode.CATEGORY_SOFT,
                Name = "Software",
                Color = "#ab69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
        };

        builder.Entity<CategoryCursus>().HasData(categoryCursuses);

        // type slots
        List<TypeSlot> typeSlots = new()
        {
            new TypeSlot
            {
                Id = HardCode.TYPE_SLOT_PRESENTIAL,
                Name = "Presentiel",
                Color = "#ff69b4",
                Icon = "pi pi-arrow-down-left-and-arrow-up-right-to-center",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new TypeSlot
            {
                Id = HardCode.TYPE_SLOT_VISIO,
                Name = "Visio",
                Color = "#fa69b4",
                Icon = "pi pi-desktop",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
              new TypeSlot
            {
                Id = HardCode.TYPE_SLOT_ALL,
                Name = "Tous",
                Color = "#fa69b4",
                Icon = "pi pi-crown",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
        };
        builder.Entity<TypeSlot>().HasData(typeSlots);

        // global filtrer
        builder.Entity<Cursus>().HasQueryFilter(d => d.ArchivedAt == null);
        builder.Entity<Formation>().HasQueryFilter(d => d.ArchivedAt == null);
        builder.Entity<Address>().HasQueryFilter(d => d.ArchivedAt == null);
        builder.Entity<Product>().HasQueryFilter(d => d.ArchivedAt == null);
        builder.Entity<Slot>().HasQueryFilter(d => d.ArchivedAt == null);

    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        base.OnConfiguring(builder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<DateTimeOffset>()
            .HaveConversion<CustomDateTimeConversion>();
        base.ConfigureConventions(configurationBuilder);
    }
}
