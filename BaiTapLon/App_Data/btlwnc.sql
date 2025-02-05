USE [master]
GO
/****** Object:  Database [bltwnc]    Script Date: 6/21/2024 5:13:01 PM ******/
CREATE DATABASE [bltwnc]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'bltwnc', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.DUONGCHUOI\MSSQL\DATA\bltwnc.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'bltwnc_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.DUONGCHUOI\MSSQL\DATA\bltwnc_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [bltwnc] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [bltwnc].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [bltwnc] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [bltwnc] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [bltwnc] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [bltwnc] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [bltwnc] SET ARITHABORT OFF 
GO
ALTER DATABASE [bltwnc] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [bltwnc] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [bltwnc] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [bltwnc] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [bltwnc] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [bltwnc] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [bltwnc] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [bltwnc] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [bltwnc] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [bltwnc] SET  ENABLE_BROKER 
GO
ALTER DATABASE [bltwnc] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [bltwnc] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [bltwnc] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [bltwnc] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [bltwnc] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [bltwnc] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [bltwnc] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [bltwnc] SET RECOVERY FULL 
GO
ALTER DATABASE [bltwnc] SET  MULTI_USER 
GO
ALTER DATABASE [bltwnc] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [bltwnc] SET DB_CHAINING OFF 
GO
ALTER DATABASE [bltwnc] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [bltwnc] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [bltwnc] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [bltwnc] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'bltwnc', N'ON'
GO
ALTER DATABASE [bltwnc] SET QUERY_STORE = OFF
GO
USE [bltwnc]
GO
/****** Object:  Table [dbo].[chucvu]    Script Date: 6/21/2024 5:13:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[chucvu](
	[MaCV] [char](10) NOT NULL,
	[TenCV] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaCV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[donhang]    Script Date: 6/21/2024 5:13:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[donhang](
	[MaDH] [char](10) NOT NULL,
	[ThoiGian] [datetime] NOT NULL,
	[tk] [nvarchar](50) NOT NULL,
	[MaSP] [char](10) NOT NULL,
	[SoLuong] [int] NULL,
	[DiaChi] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MaDH] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[giohang]    Script Date: 6/21/2024 5:13:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[giohang](
	[id] [char](10) NOT NULL,
	[MaSP] [char](10) NULL,
	[SoLuong] [int] NULL,
	[tk] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[hoadon]    Script Date: 6/21/2024 5:13:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[hoadon](
	[MaHD] [char](10) NOT NULL,
	[MaSP] [char](10) NULL,
	[tk] [nvarchar](50) NULL,
	[SoLuong] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaHD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[nguoidung]    Script Date: 6/21/2024 5:13:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[nguoidung](
	[TenND] [nvarchar](50) NULL,
	[Avatar] [nvarchar](50) NULL,
	[Tuoi] [int] NULL,
	[email] [nvarchar](50) NOT NULL,
	[SDT] [char](10) NULL,
	[DiaChi] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[nhanvien]    Script Date: 6/21/2024 5:13:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[nhanvien](
	[TenNV] [nvarchar](50) NULL,
	[Avatar] [nvarchar](50) NULL,
	[Tuoi] [int] NULL,
	[MaCV] [char](10) NULL,
	[email] [nvarchar](50) NOT NULL,
	[SDT] [char](10) NULL,
	[DiaChi] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sanpham]    Script Date: 6/21/2024 5:13:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sanpham](
	[MaSP] [char](10) NOT NULL,
	[TenSP] [nvarchar](50) NULL,
	[MoTa] [nvarchar](200) NULL,
	[Anh] [nvarchar](50) NULL,
	[Loai] [nvarchar](50) NULL,
	[SoLuong] [int] NULL,
	[Gia] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaSP] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[taikhoan]    Script Date: 6/21/2024 5:13:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[taikhoan](
	[tk] [nvarchar](50) NOT NULL,
	[mk] [varchar](50) NULL,
	[quyen] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[tk] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[chucvu] ([MaCV], [TenCV]) VALUES (N'1         ', N'Quản lý')
INSERT [dbo].[chucvu] ([MaCV], [TenCV]) VALUES (N'2         ', N'Nhân Viên')
GO
INSERT [dbo].[donhang] ([MaDH], [ThoiGian], [tk], [MaSP], [SoLuong], [DiaChi]) VALUES (N'502fb467  ', CAST(N'2024-05-30T11:20:15.853' AS DateTime), N'duongchuoi@gmail.com', N'12        ', 1, N'dichvong')
INSERT [dbo].[donhang] ([MaDH], [ThoiGian], [tk], [MaSP], [SoLuong], [DiaChi]) VALUES (N'57a817c8  ', CAST(N'2024-05-30T11:20:15.837' AS DateTime), N'duongchuoi@gmail.com', N'15        ', 3, N'dichvong')
INSERT [dbo].[donhang] ([MaDH], [ThoiGian], [tk], [MaSP], [SoLuong], [DiaChi]) VALUES (N'5e0ad00e  ', CAST(N'2024-04-13T19:40:48.937' AS DateTime), N'duongchuoi@gmail.com', N'16        ', 1, N'dichvong')
INSERT [dbo].[donhang] ([MaDH], [ThoiGian], [tk], [MaSP], [SoLuong], [DiaChi]) VALUES (N'9e5370db  ', CAST(N'2024-05-30T11:20:15.850' AS DateTime), N'duongchuoi@gmail.com', N'11        ', 1, N'dichvong')
GO
INSERT [dbo].[nguoidung] ([TenND], [Avatar], [Tuoi], [email], [SDT], [DiaChi]) VALUES (N'duongchuoi', NULL, 20, N'duongchuoi@gmail.com', N'0866762845', N'dichvong')
GO
INSERT [dbo].[nhanvien] ([TenNV], [Avatar], [Tuoi], [MaCV], [email], [SDT], [DiaChi]) VALUES (N'Bùi Sỹ Dương', N'cover3.jpg', 20, N'1         ', N'bui963852@gmail.com', N'0866762845', N'alo')
GO
INSERT [dbo].[sanpham] ([MaSP], [TenSP], [MoTa], [Anh], [Loai], [SoLuong], [Gia]) VALUES (N'1         ', N'Animi Dolor Pariatur', NULL, N'product-1.jpg', N'decoration', 100, 10)
INSERT [dbo].[sanpham] ([MaSP], [TenSP], [MoTa], [Anh], [Loai], [SoLuong], [Gia]) VALUES (N'11        ', N'Smooth Disk', NULL, N'product-11.jpg', N'decoration', 99, 46)
INSERT [dbo].[sanpham] ([MaSP], [TenSP], [MoTa], [Anh], [Loai], [SoLuong], [Gia]) VALUES (N'12        ', N'Table Black', NULL, N'product-12.jpg', N'furniture', 99, 67)
INSERT [dbo].[sanpham] ([MaSP], [TenSP], [MoTa], [Anh], [Loai], [SoLuong], [Gia]) VALUES (N'13        ', N'Table Wood Pine', NULL, N'product-13.jpg', N'furniture', 100, 50)
INSERT [dbo].[sanpham] ([MaSP], [TenSP], [MoTa], [Anh], [Loai], [SoLuong], [Gia]) VALUES (N'14        ', N'Teapot with black tea', NULL, N'product-14.jpg', N'accessory', 100, 25)
INSERT [dbo].[sanpham] ([MaSP], [TenSP], [MoTa], [Anh], [Loai], [SoLuong], [Gia]) VALUES (N'15        ', N'Unique Decoration', NULL, N'product-15.jpg', N'decoration', 97, 15)
INSERT [dbo].[sanpham] ([MaSP], [TenSP], [MoTa], [Anh], [Loai], [SoLuong], [Gia]) VALUES (N'16        ', N'Vase Of Flowers', NULL, N'product-16.jpg', N'decoration', 99, 77)
INSERT [dbo].[sanpham] ([MaSP], [TenSP], [MoTa], [Anh], [Loai], [SoLuong], [Gia]) VALUES (N'17        ', N'Wood Eggs', NULL, N'product-17.jpg', N'decoration', 100, 19)
INSERT [dbo].[sanpham] ([MaSP], [TenSP], [MoTa], [Anh], [Loai], [SoLuong], [Gia]) VALUES (N'18        ', N'Wooden Box', NULL, N'product-18.jpg', N'decoration', 100, 27)
INSERT [dbo].[sanpham] ([MaSP], [TenSP], [MoTa], [Anh], [Loai], [SoLuong], [Gia]) VALUES (N'19        ', N'Wooden Cups', NULL, N'product-19.jpg', N'accessory', 100, 29)
INSERT [dbo].[sanpham] ([MaSP], [TenSP], [MoTa], [Anh], [Loai], [SoLuong], [Gia]) VALUES (N'2         ', N'Art Deco Home', NULL, N'product-2.jpg', N'accessory', 100, 30)
INSERT [dbo].[sanpham] ([MaSP], [TenSP], [MoTa], [Anh], [Loai], [SoLuong], [Gia]) VALUES (N'3         ', N'Artificial potted plant', NULL, N'product-3.jpg', N'decoration', 100, 40)
INSERT [dbo].[sanpham] ([MaSP], [TenSP], [MoTa], [Anh], [Loai], [SoLuong], [Gia]) VALUES (N'4         ', N'Dark Green Jug', NULL, N'product-4.jpg', N'accessory', 100, 17.1)
INSERT [dbo].[sanpham] ([MaSP], [TenSP], [MoTa], [Anh], [Loai], [SoLuong], [Gia]) VALUES (N'5         ', N'Drinking Glasses', NULL, N'product-5.jpg', N'accessory', 100, 21)
INSERT [dbo].[sanpham] ([MaSP], [TenSP], [MoTa], [Anh], [Loai], [SoLuong], [Gia]) VALUES (N'6         ', N'Helen Chair', NULL, N'product-6.jpg', N'furniture', 100, 69.5)
INSERT [dbo].[sanpham] ([MaSP], [TenSP], [MoTa], [Anh], [Loai], [SoLuong], [Gia]) VALUES (N'7         ', N'High Quality Glass Bottle', NULL, N'product-7.jpg', N'accessory', 100, 30.1)
INSERT [dbo].[sanpham] ([MaSP], [TenSP], [MoTa], [Anh], [Loai], [SoLuong], [Gia]) VALUES (N'8         ', N'Living Room & Bedroom Lights', NULL, N'product-8.jpg', N'accessory', 100, 45)
INSERT [dbo].[sanpham] ([MaSP], [TenSP], [MoTa], [Anh], [Loai], [SoLuong], [Gia]) VALUES (N'9         ', N'Nancy Chair', NULL, N'product-9.jpg', N'furniture', 100, 90)
GO
INSERT [dbo].[taikhoan] ([tk], [mk], [quyen]) VALUES (N'bui963852@gmail.com', N'admin123', N'admin')
INSERT [dbo].[taikhoan] ([tk], [mk], [quyen]) VALUES (N'duongchuoi@gmail.com', N'duongchuoi963', N'nguoidung')
GO
ALTER TABLE [dbo].[donhang]  WITH CHECK ADD FOREIGN KEY([MaSP])
REFERENCES [dbo].[sanpham] ([MaSP])
GO
ALTER TABLE [dbo].[donhang]  WITH CHECK ADD FOREIGN KEY([tk])
REFERENCES [dbo].[taikhoan] ([tk])
GO
ALTER TABLE [dbo].[giohang]  WITH CHECK ADD FOREIGN KEY([MaSP])
REFERENCES [dbo].[sanpham] ([MaSP])
GO
ALTER TABLE [dbo].[giohang]  WITH CHECK ADD FOREIGN KEY([tk])
REFERENCES [dbo].[taikhoan] ([tk])
GO
ALTER TABLE [dbo].[hoadon]  WITH CHECK ADD FOREIGN KEY([MaSP])
REFERENCES [dbo].[sanpham] ([MaSP])
GO
ALTER TABLE [dbo].[hoadon]  WITH CHECK ADD FOREIGN KEY([tk])
REFERENCES [dbo].[taikhoan] ([tk])
GO
ALTER TABLE [dbo].[nhanvien]  WITH CHECK ADD FOREIGN KEY([MaCV])
REFERENCES [dbo].[chucvu] ([MaCV])
GO
USE [master]
GO
ALTER DATABASE [bltwnc] SET  READ_WRITE 
GO
