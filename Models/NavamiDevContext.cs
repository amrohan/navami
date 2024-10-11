using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace navami.Models
{
    public partial class NavamiDevContext : DbContext
    {
        public NavamiDevContext()
        {
        }

        public NavamiDevContext(DbContextOptions<NavamiDevContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<RawMaterial> RawMaterials { get; set; }
        public virtual DbSet<RawMaterialPrice> RawMaterialPrices { get; set; }
        public virtual DbSet<RawMaterialUsage> RawMaterialUsages { get; set; }
        public virtual DbSet<Recipe> Recipes { get; set; }
        public virtual DbSet<RecipeCategory> RecipeCategories { get; set; }
        public virtual DbSet<RecipeCategoryMapping> RecipeCategoryMappings { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<SubCategory> SubCategories { get; set; }
        public virtual DbSet<UserMaster> UserMasters { get; set; }
        public virtual DbSet<Vendor> Vendors { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            // Move connection string to configuration for better security.
//#warning Connection string should be moved out of source code.
//            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=navami_dev;Trusted_Connection=True;TrustServerCertificate=True;");
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryId).HasName("PK_Category");

                entity.ToTable("Category");

                entity.Property(e => e.CategoryId).ValueGeneratedOnAdd();
                entity.Property(e => e.CategoryName).HasMaxLength(255);
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<RawMaterial>(entity =>
            {
                entity.HasKey(e => e.RawMaterialId).HasName("PK_RawMaterial");

                entity.ToTable("RawMaterial");

                entity.Property(e => e.RawMaterialId).ValueGeneratedOnAdd();
                entity.Property(e => e.RawMaterialCode).HasMaxLength(255);
                entity.Property(e => e.RawMaterialName).HasMaxLength(255);
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.SpecificationNo).HasMaxLength(255);
                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.PriceDate).HasColumnType("datetime");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.AddedOn)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");

                // Foreign Key - Category
                entity.HasOne(d => d.Category)
                    .WithMany(p => p.RawMaterials)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict) // Prevent cascading deletes.
                    .HasConstraintName("FK_RawMaterial_Category");

                // Foreign Key - SubCategory
                entity.HasOne(d => d.SubCategory)
                    .WithMany(p => p.RawMaterials)
                    .HasForeignKey(d => d.SubCategoryId)
                    .OnDelete(DeleteBehavior.Restrict) // Prevent cascading deletes.
                    .HasConstraintName("FK_RawMaterial_SubCategory");
            });

            modelBuilder.Entity<RawMaterialPrice>(entity =>
            {
                entity.HasKey(e => e.RawMaterialPriceId).HasName("PK_RawMaterialPrice");

                entity.ToTable("RawMaterialPrice");

                entity.Property(e => e.RawMaterialPriceId).ValueGeneratedOnAdd();
                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.SupplierName).HasMaxLength(255);
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.IsActive).HasDefaultValue(true);

                // Foreign Key - RawMaterial
                entity.HasOne(d => d.RawMaterial)
                    .WithMany(p => p.RawMaterialPrices)
                    .HasForeignKey(d => d.RawMaterialId)
                    .OnDelete(DeleteBehavior.Cascade) // Cascade delete to raw material prices.
                    .HasConstraintName("FK_RawMaterialPrice_RawMaterial");
            });

            modelBuilder.Entity<RawMaterialUsage>(entity =>
            {
                entity.HasKey(e => e.RawMaterialUsageId).HasName("PK_RawMaterialUsage");

                entity.ToTable("RawMaterialUsage");

                entity.Property(e => e.RawMaterialUsageId).ValueGeneratedOnAdd();
                entity.Property(e => e.Quantity).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.IsActive).HasDefaultValue(true);

                // Foreign Key - RawMaterial
                entity.HasOne(d => d.RawMaterial)
                    .WithMany(p => p.RawMaterialUsages)
                    .HasForeignKey(d => d.RawMaterialId)
                    .OnDelete(DeleteBehavior.Cascade) // Cascade delete to raw material usages.
                    .HasConstraintName("FK_RawMaterialUsage_RawMaterial");

                // Foreign Key - Recipe
                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.RawMaterialUsages)
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.Cascade) // Cascade delete to raw material usages.
                    .HasConstraintName("FK_RawMaterialUsage_Recipe");
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.HasKey(e => e.RecipeId).HasName("PK_Recipe");

                entity.ToTable("Recipe");

                entity.Property(e => e.RecipeName).HasMaxLength(255);
                entity.Property(e => e.Profile).HasMaxLength(255);
                entity.Property(e => e.Username).HasMaxLength(255);
                entity.Property(e => e.AdjustedCost).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.TotalCost).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<RecipeCategory>(entity =>
            {
                entity.HasKey(e => e.RecipeCategoryId).HasName("PK_RecipeCategory");

                entity.ToTable("RecipeCategory");

                entity.Property(e => e.RecipeCategoryId).ValueGeneratedOnAdd();
                entity.Property(e => e.RecipeCategoryName).HasMaxLength(255);
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<RecipeCategoryMapping>(entity =>
            {
                entity.HasKey(e => e.RecipeCategoryMappingId).HasName("PK_RecipeCategoryMapping");

                entity.ToTable("RecipeCategoryMapping");

                entity.Property(e => e.RecipeCategoryMappingId).ValueGeneratedOnAdd();
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                // Foreign Key - RecipeCategory
                entity.HasOne(d => d.RecipeCategory)
                    .WithMany(p => p.RecipeCategoryMappings)
                    .HasForeignKey(d => d.RecipeCategoryId)
                    .OnDelete(DeleteBehavior.Cascade) // Cascade delete to recipe category mappings.
                    .HasConstraintName("FK_RecipeCategoryMapping_RecipeCategory");

                // Foreign Key - Recipe
                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.RecipeCategoryMappings)
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.Cascade) // Cascade delete to recipe category mappings.
                    .HasConstraintName("FK_RecipeCategoryMapping_Recipe");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.RoleId).HasName("PK_Role");

                entity.ToTable("Role");

                entity.Property(e => e.RoleId).ValueGeneratedOnAdd();
                entity.Property(e => e.RoleName).HasMaxLength(255);
            });

            modelBuilder.Entity<SubCategory>(entity =>
            {
                entity.HasKey(e => e.SubCategoryId).HasName("PK_SubCategory");

                entity.ToTable("SubCategory");

                entity.Property(e => e.SubCategoryId).ValueGeneratedOnAdd();
                entity.Property(e => e.SubCategoryName).HasMaxLength(255);
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                // Foreign Key - Category
                entity.HasOne(d => d.Category)
                    .WithMany(p => p.SubCategories)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict) // Prevent cascading deletes.
                    .HasConstraintName("FK_SubCategory_Category");
            });

            modelBuilder.Entity<UserMaster>(entity =>
            {
                entity.HasKey(e => e.UserId).HasName("PK_UserMaster");

                entity.ToTable("UserMaster");

                entity.Property(e => e.UserId).ValueGeneratedOnAdd();
                entity.Property(e => e.Username).HasMaxLength(255);
                entity.Property(e => e.Email).HasMaxLength(255);
                entity.Property(e => e.Password).HasMaxLength(255);
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            });

            modelBuilder.Entity<Vendor>(entity =>
            {
                entity.HasKey(e => e.VendorId).HasName("PK_Vendor");

                entity.ToTable("Vendor");

                entity.Property(e => e.VendorId).ValueGeneratedOnAdd();
                entity.Property(e => e.VendorName).HasMaxLength(255);
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });
        }
    }
}
