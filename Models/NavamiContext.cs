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

    public virtual DbSet<RecipeCategory> RecipeCategories { get; set; }

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

        modelBuilder.Entity<RecipeCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RecipeCa__3214EC07631F7B21");

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
