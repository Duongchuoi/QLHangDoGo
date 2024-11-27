using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace BaiTapLon.Models
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<chucvu> chucvus { get; set; }
        public virtual DbSet<donhang> donhangs { get; set; }
        public virtual DbSet<giohang> giohangs { get; set; }
        public virtual DbSet<hoadon> hoadons { get; set; }
        public virtual DbSet<nguoidung> nguoidungs { get; set; }
        public virtual DbSet<nhanvien> nhanviens { get; set; }
        public virtual DbSet<sanpham> sanphams { get; set; }
        public virtual DbSet<taikhoan> taikhoans { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<chucvu>()
                .Property(e => e.MaCV)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<donhang>()
                .Property(e => e.MaDH)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<donhang>()
                .Property(e => e.MaSP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<giohang>()
                .Property(e => e.id)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<giohang>()
                .Property(e => e.MaSP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<hoadon>()
                .Property(e => e.MaHD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<hoadon>()
                .Property(e => e.MaSP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<nguoidung>()
                .Property(e => e.SDT)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<nhanvien>()
                .Property(e => e.MaCV)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<nhanvien>()
                .Property(e => e.SDT)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<sanpham>()
                .Property(e => e.MaSP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<sanpham>()
                .HasMany(e => e.donhangs)
                .WithRequired(e => e.sanpham)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<taikhoan>()
                .Property(e => e.mk)
                .IsUnicode(false);

            modelBuilder.Entity<taikhoan>()
                .HasMany(e => e.donhangs)
                .WithRequired(e => e.taikhoan)
                .WillCascadeOnDelete(false);
        }
    }
}
