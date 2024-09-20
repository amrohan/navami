using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace navami.Models;

public partial class NavamiContext : DbContext
{
    public NavamiContext()
    {
    }

    public NavamiContext(DbContextOptions<NavamiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CategoryMaster> CategoryMasters { get; set; }

    public virtual DbSet<RawMaterialUsage> RawMaterialUsages { get; set; }

    public virtual DbSet<RecipeCategory> RecipeCategories { get; set; }

    public virtual DbSet<RecipeCategoryMapping> RecipeCategoryMappings { get; set; }

    public virtual DbSet<RecipeMaster> RecipeMasters { get; set; }

    public virtual DbSet<Rmmaster> Rmmasters { get; set; }

    public virtual DbSet<RmpriceMaster> RmpriceMasters { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SubCategoryMaster> SubCategoryMasters { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VendorMaster> VendorMasters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=navami;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CategoryMaster>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A2BED41A08B");

            entity.ToTable("CategoryMaster");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<RawMaterialUsage>(entity =>
        {
            entity.HasKey(e => e.RmusageId).HasName("PK__RawMater__CD8B6DE33B12C828");

            entity.ToTable("RawMaterialUsage");

            entity.Property(e => e.RmusageId).HasColumnName("RMUsageID");
            entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.RecipeId).HasColumnName("RecipeID");
            entity.Property(e => e.Rmid).HasColumnName("RMID");

            entity.HasOne(d => d.Recipe).WithMany(p => p.RawMaterialUsages)
                .HasForeignKey(d => d.RecipeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RawMateri__Recip__787EE5A0");

            entity.HasOne(d => d.Rm).WithMany(p => p.RawMaterialUsages)
                .HasForeignKey(d => d.Rmid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RawMateria__RMID__797309D9");
        });

        modelBuilder.Entity<RecipeCategory>(entity =>
        {
            entity.HasKey(e => e.RecipeCategoryId).HasName("PK__RecipeCa__3214EC07631F7B21");

            entity.Property(e => e.RecipeCategoryId).HasColumnName("RecipeCategoryID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.RecipeCategoryName).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.RecipeCategoryCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RecipeCat__Creat__4D94879B");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.RecipeCategoryUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__RecipeCat__Updat__4E88ABD4");
        });

        modelBuilder.Entity<RecipeCategoryMapping>(entity =>
        {
            entity.HasKey(e => e.RecipeCategoryMappingId).HasName("PK__RecipeCa__37A8E59F5F00ACA4");

            entity.ToTable("RecipeCategoryMapping");

            entity.Property(e => e.RecipeCategoryMappingId).HasColumnName("RecipeCategoryMappingID");
            entity.Property(e => e.RecipeCategoryId).HasColumnName("RecipeCategoryID");
            entity.Property(e => e.RecipeId).HasColumnName("RecipeID");

            entity.HasOne(d => d.RecipeCategory).WithMany(p => p.RecipeCategoryMappings)
                .HasForeignKey(d => d.RecipeCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RecipeCat__Recip__71D1E811");

            entity.HasOne(d => d.Recipe).WithMany(p => p.RecipeCategoryMappings)
                .HasForeignKey(d => d.RecipeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RecipeCat__Recip__70DDC3D8");
        });

        modelBuilder.Entity<RecipeMaster>(entity =>
        {
            entity.HasKey(e => e.RecipeId).HasName("PK__RecipeMa__FDD988D043DEE369");

            entity.ToTable("RecipeMaster");

            entity.Property(e => e.RecipeId).HasColumnName("RecipeID");
            entity.Property(e => e.AdjustedCost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Profile).HasMaxLength(100);
            entity.Property(e => e.RecipeName).HasMaxLength(200);
            entity.Property(e => e.TotalCost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.Username).HasMaxLength(100);
        });

        modelBuilder.Entity<Rmmaster>(entity =>
        {
            entity.HasKey(e => e.Rmid).HasName("PK__RMMaster__474689A8DA7A2514");

            entity.ToTable("RMMaster");

            entity.Property(e => e.Rmid).HasColumnName("RMID");
            entity.Property(e => e.AddedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.IsNewRm).HasColumnName("IsNewRM");
            entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Party).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PriceDate).HasColumnType("datetime");
            entity.Property(e => e.Rmcode)
                .HasMaxLength(50)
                .HasColumnName("RMCode");
            entity.Property(e => e.Rmname)
                .HasMaxLength(150)
                .HasColumnName("RMName");
            entity.Property(e => e.SpecificationNo).HasMaxLength(100);
            entity.Property(e => e.SubCategoryId).HasColumnName("SubCategoryID");

            entity.HasOne(d => d.Category).WithMany(p => p.Rmmasters)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RMMaster_CategoryMaster");

            entity.HasOne(d => d.SubCategory).WithMany(p => p.Rmmasters)
                .HasForeignKey(d => d.SubCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RMMaster_SubCategoryMaster");
        });

        modelBuilder.Entity<RmpriceMaster>(entity =>
        {
            entity.HasKey(e => e.RmpriceId).HasName("PK__RMPriceM__39675B8DB7641C52");

            entity.ToTable("RMPriceMaster");

            entity.Property(e => e.RmpriceId).HasColumnName("RMPriceId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Rmid).HasColumnName("RMID");
            entity.Property(e => e.SupplierName).HasMaxLength(150);

            entity.HasOne(d => d.Rm).WithMany(p => p.RmpriceMasters)
                .HasForeignKey(d => d.Rmid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RMPriceMaster_RMMaster");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE1AA27C259B");

            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<SubCategoryMaster>(entity =>
        {
            entity.HasKey(e => e.SubCategoryId).HasName("PK__SubCateg__26BE5BF96D9788D4");

            entity.ToTable("SubCategoryMaster");

            entity.Property(e => e.SubCategoryId).HasColumnName("SubCategoryID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.SubCategoryName).HasMaxLength(150);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.SubCategoryMasters)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubCategoryMaster_CategoryMaster");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C30487B76");

            entity.Property(e => e.UserId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Mobile)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VendorMaster>(entity =>
        {
            entity.HasKey(e => e.VendorId).HasName("PK__VendorMa__FC8618F370060CD2");

            entity.ToTable("VendorMaster");

            entity.Property(e => e.VendorId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.VendorName).HasMaxLength(150);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
