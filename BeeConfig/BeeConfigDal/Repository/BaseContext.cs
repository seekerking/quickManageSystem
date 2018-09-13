using Microsoft.EntityFrameworkCore;
using BeeConfigDal.Entity;
using BeeConfigModels;
using BeeConfigModels.Common;

namespace BeeConfigDal.Repository
{
 public   class BaseContext:DbContext
    {
        public BaseContext()
        {
        }

        public BaseContext(DbContextOptions<BaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Audit> Audits { get; set; }
        public virtual DbSet<App> Apps { get; set; }
        public virtual DbSet<Config> Configs { get; set; }
        public virtual DbSet<Env> Envs { get; set; }
        public virtual DbSet<ReqLog> ReqLogs { get; set; }

        public virtual DbSet<Publish> Publishs { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
              
                optionsBuilder.UseSqlServer(ConfigHelper.Config.Connection,options=>options.UseRowNumberForPaging());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<App>(entity =>
            {
                entity.ToTable("AppTB");

                entity.HasIndex(e => e.AppId)
                    .HasName("NonClusteredIndex-20180718-103406")
                    .IsUnique();

                entity.Property(e => e.AppDesc)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.AppId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AppName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Audit>(entity =>
            {
                entity.ToTable("AuditTB");

                entity.Property(e => e.AppId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ConfigId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ConfigValue)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.EnvId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Config>(entity =>
            {
                entity.ToTable("ConfigTB");

                entity.HasIndex(e => new { e.ConfigId, e.AppId, e.EnvId })
                    .HasName("IX_ConfigId_EnvId_AppId")
                    .IsUnique();

                entity.Property(e => e.AppId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ConfigDesc)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.ConfigId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ConfigValue)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.EnvId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Env>(entity =>
            {
                entity.ToTable("EnvTB");

                entity.HasIndex(e => e.EnvId)
                    .HasName("NonClusteredIndex-20180718-103533")
                    .IsUnique();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.EnvDesc)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.EnvId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ReqLog>(entity =>
            {
                entity.ToTable("ReqLogsTB");

                entity.HasIndex(e => new { e.AppEnv, e.AppId, e.ClientIp })
                    .HasName("IX_AppEnv_AppId_ClientIp")
                    .IsUnique();

                entity.Property(e => e.AppEnv)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AppId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ClientIp)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstDate).HasColumnType("datetime");

                entity.Property(e => e.LastConfigDate).HasColumnType("datetime");

                entity.Property(e => e.LastDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("UserTB");

                entity.HasIndex(e => e.UserId)
                    .HasName("NonClusteredIndex-20180718-103844")
                    .IsUnique();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.LastUpdate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Pwd)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}
