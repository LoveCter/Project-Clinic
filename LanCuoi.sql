USE [master]
GO
/****** Object:  Database [user]    Script Date: 03/01/2025 10:47:30 CH ******/
CREATE DATABASE [user]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'user', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\user.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'user_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\user_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [user] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [user].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [user] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [user] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [user] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [user] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [user] SET ARITHABORT OFF 
GO
ALTER DATABASE [user] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [user] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [user] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [user] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [user] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [user] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [user] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [user] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [user] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [user] SET  DISABLE_BROKER 
GO
ALTER DATABASE [user] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [user] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [user] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [user] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [user] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [user] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [user] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [user] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [user] SET  MULTI_USER 
GO
ALTER DATABASE [user] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [user] SET DB_CHAINING OFF 
GO
ALTER DATABASE [user] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [user] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [user] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [user] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [user] SET QUERY_STORE = ON
GO
ALTER DATABASE [user] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [user]
GO
/****** Object:  Table [dbo].[CTHD_N]    Script Date: 03/01/2025 10:47:30 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CTHD_N](
	[ID_CTHDN] [nvarchar](50) NOT NULL,
	[ID_HDN] [nvarchar](50) NULL,
	[ID_Medicine] [nvarchar](50) NULL,
	[CountN] [int] NULL,
	[PriceN] [money] NULL,
 CONSTRAINT [PK_CTHD_N] PRIMARY KEY CLUSTERED 
(
	[ID_CTHDN] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CTHD_X]    Script Date: 03/01/2025 10:47:30 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CTHD_X](
	[ID_CTHDX] [nchar](10) NOT NULL,
	[ID_HDX] [nchar](10) NULL,
	[ID_Medicine] [nchar](10) NULL,
	[CountX] [nchar](10) NULL,
	[PriceX] [nchar](10) NULL,
 CONSTRAINT [PK_CTHD_X] PRIMARY KEY CLUSTERED 
(
	[ID_CTHDX] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 03/01/2025 10:47:30 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[IdCus] [int] NULL,
	[NameCus] [nvarchar](50) NULL,
	[PhoneNumberCus] [nvarchar](50) NULL,
	[Account] [nvarchar](50) NULL,
	[Password] [int] NULL,
	[Email] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employee]    Script Date: 03/01/2025 10:47:30 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employee](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[PhoneNumber] [varchar](15) NULL,
	[Role] [nvarchar](50) NULL,
	[YearsOfWork] [int] NOT NULL,
 CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HD_N]    Script Date: 03/01/2025 10:47:30 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HD_N](
	[ID_HDN] [nvarchar](50) NOT NULL,
	[DateN] [datetime] NULL,
	[AmountN] [int] NULL,
	[ID_NVN] [nvarchar](50) NULL,
 CONSTRAINT [PK_HDN] PRIMARY KEY CLUSTERED 
(
	[ID_HDN] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HD_X]    Script Date: 03/01/2025 10:47:30 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HD_X](
	[ID_HDX] [nvarchar](50) NOT NULL,
	[ID_NVX] [nvarchar](50) NULL,
	[ID_Cus] [nvarchar](50) NULL,
	[DateX] [datetime] NULL,
	[AmountX] [int] NULL,
 CONSTRAINT [PK_HD_X] PRIMARY KEY CLUSTERED 
(
	[ID_HDX] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 03/01/2025 10:47:30 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[Name] [nvarchar](50) NULL,
	[Qty] [int] NULL,
	[Price] [int] NULL,
	[Total] [int] NULL,
	[ID] [int] NOT NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order2]    Script Date: 03/01/2025 10:47:30 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order2](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Qty] [int] NULL,
	[Price] [int] NULL,
	[Total] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ordered]    Script Date: 03/01/2025 10:47:30 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ordered](
	[OrderID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 03/01/2025 10:47:30 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 03/01/2025 10:47:30 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ProductID] [int] NOT NULL,
	[Product_name] [nchar](20) NULL,
	[Type] [nchar](10) NULL,
	[Product_Suplier] [nchar](10) NULL,
	[Describe] [nvarchar](50) NULL,
	[Quantity] [int] NULL,
	[EXP] [datetime] NULL,
	[Price] [int] NULL,
	[Cost] [int] NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products_Medicine]    Script Date: 03/01/2025 10:47:30 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products_Medicine](
	[ProductID] [int] NULL,
	[Product_name] [nchar](10) NULL,
	[Quantity] [int] NULL,
	[EXP] [nvarchar](100) NULL,
	[Price] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Receipt]    Script Date: 03/01/2025 10:47:30 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Receipt](
	[ProductID] [int] NOT NULL,
	[ReceiptDay] [date] NULL,
 CONSTRAINT [PK_Receipt] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Register]    Script Date: 03/01/2025 10:47:30 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Register](
	[email] [nvarchar](50) NULL,
	[username] [nvarchar](50) NULL,
	[password] [nvarchar](50) NULL,
	[ID] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Suplier]    Script Date: 03/01/2025 10:47:30 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Suplier](
	[MaNCC] [nvarchar](50) NOT NULL,
	[TenNCC] [nvarchar](50) NULL,
	[PhoneNCC] [int] NULL,
	[Note] [nvarchar](50) NULL,
 CONSTRAINT [PK_Suplier] PRIMARY KEY CLUSTERED 
(
	[MaNCC] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transactions]    Script Date: 03/01/2025 10:47:30 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transactions](
	[TransactionID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NULL,
	[TransactionType] [nchar](10) NOT NULL,
	[Quantity] [int] NOT NULL,
	[TransactionDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[TransactionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Customer] ([IdCus], [NameCus], [PhoneNumberCus], [Account], [Password], [Email]) VALUES (1, N'Nhac Phong', N'0939731313', N'Phong', 123, N'nhacphong@gmail.com')
INSERT [dbo].[Customer] ([IdCus], [NameCus], [PhoneNumberCus], [Account], [Password], [Email]) VALUES (2, N'Ngoc Thanh', N'0939731821', N'Thanh', 111, N'ngocthanhnguyen@gmail.com')
INSERT [dbo].[Customer] ([IdCus], [NameCus], [PhoneNumberCus], [Account], [Password], [Email]) VALUES (3, N'Quang Minh', N'093973231', N'Minh', 313, N'dmtt@gmail.com')
INSERT [dbo].[Customer] ([IdCus], [NameCus], [PhoneNumberCus], [Account], [Password], [Email]) VALUES (4, N'Nhat Dang', N'093932231', N'Dang', 3333, N'nnd@gmail.com')
GO
INSERT [dbo].[Employee] ([ID], [Name], [PhoneNumber], [Role], [YearsOfWork]) VALUES (1, N'bao', N'972870949', N'Admin', 3)
INSERT [dbo].[Employee] ([ID], [Name], [PhoneNumber], [Role], [YearsOfWork]) VALUES (2, N'rita', N'972870949', N'Admin', 4)
INSERT [dbo].[Employee] ([ID], [Name], [PhoneNumber], [Role], [YearsOfWork]) VALUES (3, N'son', N'972870911', N'Admin', 9)
INSERT [dbo].[Employee] ([ID], [Name], [PhoneNumber], [Role], [YearsOfWork]) VALUES (12, N'Hoang', N'0202020', N'Employee', 78)
GO
INSERT [dbo].[Order2] ([ID], [Name], [Qty], [Price], [Total]) VALUES (1, N'Panadol             ', 1, 157, 157)
INSERT [dbo].[Order2] ([ID], [Name], [Qty], [Price], [Total]) VALUES (2, N'Aspirin             ', 122, 148, 18056)
INSERT [dbo].[Order2] ([ID], [Name], [Qty], [Price], [Total]) VALUES (3, N'Ibuprofen           ', 86, 158, 13588)
INSERT [dbo].[Order2] ([ID], [Name], [Qty], [Price], [Total]) VALUES (4, N'Paracetamol         ', 199, 157, 31243)
INSERT [dbo].[Order2] ([ID], [Name], [Qty], [Price], [Total]) VALUES (5, N'Diclofenac          ', 327, 186, 60822)
INSERT [dbo].[Order2] ([ID], [Name], [Qty], [Price], [Total]) VALUES (6, N'Naproxen            ', 170, 177, 30090)
INSERT [dbo].[Order2] ([ID], [Name], [Qty], [Price], [Total]) VALUES (7, N'Amlodipine          ', 126, 233, 29358)
INSERT [dbo].[Order2] ([ID], [Name], [Qty], [Price], [Total]) VALUES (9, N'Azithromycin        ', 56, 152, 8512)
INSERT [dbo].[Order2] ([ID], [Name], [Qty], [Price], [Total]) VALUES (20, N'Gabapentin          ', 24, 159, 3816)
INSERT [dbo].[Order2] ([ID], [Name], [Qty], [Price], [Total]) VALUES (21, N'Gabapentine         ', 62, 143, 8866)
INSERT [dbo].[Order2] ([ID], [Name], [Qty], [Price], [Total]) VALUES (22, N'test                ', 45, 145, 6525)
GO
INSERT [dbo].[Ordered] ([OrderID]) VALUES (11)
GO
INSERT [dbo].[Products] ([ProductID], [Product_name], [Type], [Product_Suplier], [Describe], [Quantity], [EXP], [Price], [Cost]) VALUES (1, N'Panadol             ', N'Type 1    ', N'EcoMart   ', N'High quality product', 112, CAST(N'2025-01-01T00:00:00.000' AS DateTime), 120, 157)
INSERT [dbo].[Products] ([ProductID], [Product_name], [Type], [Product_Suplier], [Describe], [Quantity], [EXP], [Price], [Cost]) VALUES (2, N'Aspirin             ', N'Type 2    ', N'SunFood   ', N'Durable and reliable', 199, CAST(N'2025-06-15T00:00:00.000' AS DateTime), 121, 148)
INSERT [dbo].[Products] ([ProductID], [Product_name], [Type], [Product_Suplier], [Describe], [Quantity], [EXP], [Price], [Cost]) VALUES (3, N'Ibuprofen           ', N'Type 1    ', N'FreshCo   ', N'Best selling item', 134, CAST(N'2024-12-31T00:00:00.000' AS DateTime), 132, 158)
INSERT [dbo].[Products] ([ProductID], [Product_name], [Type], [Product_Suplier], [Describe], [Quantity], [EXP], [Price], [Cost]) VALUES (4, N'Paracetamol         ', N'Type 3    ', N'EcoMart   ', N'Eco-friendly product', 0, CAST(N'2025-03-20T00:00:00.000' AS DateTime), 131, 157)
INSERT [dbo].[Products] ([ProductID], [Product_name], [Type], [Product_Suplier], [Describe], [Quantity], [EXP], [Price], [Cost]) VALUES (5, N'Diclofenac          ', N'Type 2    ', N'SunFood   ', N'Premium quality', 53, CAST(N'2025-08-10T00:00:00.000' AS DateTime), 156, 186)
INSERT [dbo].[Products] ([ProductID], [Product_name], [Type], [Product_Suplier], [Describe], [Quantity], [EXP], [Price], [Cost]) VALUES (6, N'Naproxen            ', N'Type 1    ', N'FreshCo   ', N'Affordable price', 142, CAST(N'2025-09-01T00:00:00.000' AS DateTime), 142, 177)
INSERT [dbo].[Products] ([ProductID], [Product_name], [Type], [Product_Suplier], [Describe], [Quantity], [EXP], [Price], [Cost]) VALUES (7, N'Amlodipine          ', N'Type 3    ', N'EcoMart   ', N'Highly recommended', 50, CAST(N'2025-04-15T00:00:00.000' AS DateTime), 196, 233)
INSERT [dbo].[Products] ([ProductID], [Product_name], [Type], [Product_Suplier], [Describe], [Quantity], [EXP], [Price], [Cost]) VALUES (8, N'Amoxicillin         ', N'Type 2    ', N'SunFood   ', N'Popular choice', 180, CAST(N'2025-11-20T00:00:00.000' AS DateTime), 120, 149)
INSERT [dbo].[Products] ([ProductID], [Product_name], [Type], [Product_Suplier], [Describe], [Quantity], [EXP], [Price], [Cost]) VALUES (9, N'Azithromycin        ', N'Type 1    ', N'FreshCo   ', N'Limited stock', 3, CAST(N'2025-02-10T00:00:00.000' AS DateTime), 120, 152)
INSERT [dbo].[Products] ([ProductID], [Product_name], [Type], [Product_Suplier], [Describe], [Quantity], [EXP], [Price], [Cost]) VALUES (10, N'Simvastatin         ', NULL, N'FreshCo   ', NULL, 3, CAST(N'2025-02-10T00:00:00.000' AS DateTime), 120, 152)
INSERT [dbo].[Products] ([ProductID], [Product_name], [Type], [Product_Suplier], [Describe], [Quantity], [EXP], [Price], [Cost]) VALUES (11, N'Metformin           ', NULL, N'FreshCo   ', NULL, 3, CAST(N'2025-02-10T00:00:00.000' AS DateTime), 120, 144)
INSERT [dbo].[Products] ([ProductID], [Product_name], [Type], [Product_Suplier], [Describe], [Quantity], [EXP], [Price], [Cost]) VALUES (12, N'Cetirizine          ', NULL, N'FreshCo   ', NULL, 3, CAST(N'2025-02-10T00:00:00.000' AS DateTime), 120, 143)
INSERT [dbo].[Products] ([ProductID], [Product_name], [Type], [Product_Suplier], [Describe], [Quantity], [EXP], [Price], [Cost]) VALUES (15, N'Levothyroxine       ', NULL, N'FreshCo   ', NULL, 3, CAST(N'2025-02-10T00:00:00.000' AS DateTime), 120, 145)
INSERT [dbo].[Products] ([ProductID], [Product_name], [Type], [Product_Suplier], [Describe], [Quantity], [EXP], [Price], [Cost]) VALUES (20, N'Gabapentin          ', NULL, N'FreshCo   ', NULL, 3, CAST(N'2025-02-10T00:00:00.000' AS DateTime), 120, 159)
INSERT [dbo].[Products] ([ProductID], [Product_name], [Type], [Product_Suplier], [Describe], [Quantity], [EXP], [Price], [Cost]) VALUES (21, N'Gabapentine         ', NULL, N'FreshCo   ', NULL, 29, CAST(N'2025-02-10T00:00:00.000' AS DateTime), 120, 143)
INSERT [dbo].[Products] ([ProductID], [Product_name], [Type], [Product_Suplier], [Describe], [Quantity], [EXP], [Price], [Cost]) VALUES (22, N'test                ', NULL, N'FreshCo   ', NULL, 6, CAST(N'2025-02-10T00:00:00.000' AS DateTime), 120, 145)
GO
INSERT [dbo].[Products_Medicine] ([ProductID], [Product_name], [Quantity], [EXP], [Price]) VALUES (2, N'02        ', 59, N'2025-06-15', 121)
INSERT [dbo].[Products_Medicine] ([ProductID], [Product_name], [Quantity], [EXP], [Price]) VALUES (3, N'03        ', 39, N'2024-12-31', 132)
INSERT [dbo].[Products_Medicine] ([ProductID], [Product_name], [Quantity], [EXP], [Price]) VALUES (4, N'04        ', 39, N'2025-03-20', 131)
INSERT [dbo].[Products_Medicine] ([ProductID], [Product_name], [Quantity], [EXP], [Price]) VALUES (5, N'05        ', 45, N'2025-08-10', 156)
INSERT [dbo].[Products_Medicine] ([ProductID], [Product_name], [Quantity], [EXP], [Price]) VALUES (6, N'06        ', 141, N'2025-09-01', 142)
INSERT [dbo].[Products_Medicine] ([ProductID], [Product_name], [Quantity], [EXP], [Price]) VALUES (7, N'07        ', 50, N'2025-04-15', 1963)
INSERT [dbo].[Products_Medicine] ([ProductID], [Product_name], [Quantity], [EXP], [Price]) VALUES (8, N'08        ', 39, N'2025-11-20', 120)
INSERT [dbo].[Products_Medicine] ([ProductID], [Product_name], [Quantity], [EXP], [Price]) VALUES (9, N'09        ', 3, N'2025-02-10', 120)
GO
INSERT [dbo].[Receipt] ([ProductID], [ReceiptDay]) VALUES (1, CAST(N'2024-12-01' AS Date))
INSERT [dbo].[Receipt] ([ProductID], [ReceiptDay]) VALUES (2, CAST(N'2024-12-01' AS Date))
INSERT [dbo].[Receipt] ([ProductID], [ReceiptDay]) VALUES (3, CAST(N'2024-12-01' AS Date))
INSERT [dbo].[Receipt] ([ProductID], [ReceiptDay]) VALUES (4, CAST(N'2024-12-01' AS Date))
INSERT [dbo].[Receipt] ([ProductID], [ReceiptDay]) VALUES (5, CAST(N'2024-12-01' AS Date))
INSERT [dbo].[Receipt] ([ProductID], [ReceiptDay]) VALUES (6, CAST(N'2024-12-01' AS Date))
INSERT [dbo].[Receipt] ([ProductID], [ReceiptDay]) VALUES (7, CAST(N'2024-12-05' AS Date))
INSERT [dbo].[Receipt] ([ProductID], [ReceiptDay]) VALUES (8, CAST(N'2024-12-05' AS Date))
INSERT [dbo].[Receipt] ([ProductID], [ReceiptDay]) VALUES (9, CAST(N'2024-12-05' AS Date))
INSERT [dbo].[Receipt] ([ProductID], [ReceiptDay]) VALUES (10, CAST(N'2024-12-05' AS Date))
INSERT [dbo].[Receipt] ([ProductID], [ReceiptDay]) VALUES (20, CAST(N'2024-12-05' AS Date))
INSERT [dbo].[Receipt] ([ProductID], [ReceiptDay]) VALUES (21, CAST(N'2024-12-05' AS Date))
INSERT [dbo].[Receipt] ([ProductID], [ReceiptDay]) VALUES (22, CAST(N'2024-12-05' AS Date))
GO
INSERT [dbo].[Register] ([email], [username], [password], [ID]) VALUES (N'charitale@gmail.com', N'admin2', N'2', 2)
INSERT [dbo].[Register] ([email], [username], [password], [ID]) VALUES (N'buiqbao710@gmail.com', N'admin1', N'1', 1)
INSERT [dbo].[Register] ([email], [username], [password], [ID]) VALUES (N'admin4@gmail.com', N'admin4', N'1234', 11)
INSERT [dbo].[Register] ([email], [username], [password], [ID]) VALUES (N'admin5@gmail.com', N'admin5', N'12346', 4)
INSERT [dbo].[Register] ([email], [username], [password], [ID]) VALUES (N'phongsonbui@gmail.com', N'admin3', N'3', 3)
INSERT [dbo].[Register] ([email], [username], [password], [ID]) VALUES (N'asidj@gmail.com', N'admin', N'123', NULL)
INSERT [dbo].[Register] ([email], [username], [password], [ID]) VALUES (N'ad@gmail.com', N'ad', N'123', 5)
INSERT [dbo].[Register] ([email], [username], [password], [ID]) VALUES (N'admin7@gmail.com', N'admin7', N'12346', 7)
INSERT [dbo].[Register] ([email], [username], [password], [ID]) VALUES (N'admin5@gmail.com', N'admin5', N'1234', 6)
INSERT [dbo].[Register] ([email], [username], [password], [ID]) VALUES (N'rita@gmail.com', N'rita', N'1234', 9)
INSERT [dbo].[Register] ([email], [username], [password], [ID]) VALUES (N'ritaa@gmail.com', N'ritaâ', N'1234', 10)
INSERT [dbo].[Register] ([email], [username], [password], [ID]) VALUES (N'nv@gmail.com', N'nv', N'1', 12)
GO
INSERT [dbo].[Suplier] ([MaNCC], [TenNCC], [PhoneNCC], [Note]) VALUES (N'1', N'EcoMart', 772344122, N'Leading supplier of eco-friendly products')
INSERT [dbo].[Suplier] ([MaNCC], [TenNCC], [PhoneNCC], [Note]) VALUES (N'2', N'SunFood', 972870099, N'Supplier of fresh and organic food')
INSERT [dbo].[Suplier] ([MaNCC], [TenNCC], [PhoneNCC], [Note]) VALUES (N'3', N'FreshCo', 872870939, N'Trusted source for quality groceries')
GO
SET IDENTITY_INSERT [dbo].[Transactions] ON 

INSERT [dbo].[Transactions] ([TransactionID], [ProductID], [TransactionType], [Quantity], [TransactionDate]) VALUES (7, 1, N'Import    ', 12, CAST(N'2024-12-23T23:31:26.257' AS DateTime))
INSERT [dbo].[Transactions] ([TransactionID], [ProductID], [TransactionType], [Quantity], [TransactionDate]) VALUES (8, 1, N'Import    ', 90, CAST(N'2024-12-23T23:31:37.333' AS DateTime))
INSERT [dbo].[Transactions] ([TransactionID], [ProductID], [TransactionType], [Quantity], [TransactionDate]) VALUES (9, 1, N'Export    ', 90, CAST(N'2024-12-23T23:31:41.530' AS DateTime))
INSERT [dbo].[Transactions] ([TransactionID], [ProductID], [TransactionType], [Quantity], [TransactionDate]) VALUES (10, 9, N'Export    ', 87, CAST(N'2024-12-23T23:33:42.823' AS DateTime))
INSERT [dbo].[Transactions] ([TransactionID], [ProductID], [TransactionType], [Quantity], [TransactionDate]) VALUES (12, 1, N'Import    ', 12, CAST(N'2024-12-25T22:16:18.537' AS DateTime))
INSERT [dbo].[Transactions] ([TransactionID], [ProductID], [TransactionType], [Quantity], [TransactionDate]) VALUES (13, 1, N'Import    ', 12, CAST(N'2024-12-25T22:17:52.250' AS DateTime))
INSERT [dbo].[Transactions] ([TransactionID], [ProductID], [TransactionType], [Quantity], [TransactionDate]) VALUES (14, 1, N'Export    ', 24, CAST(N'2024-12-25T22:18:18.653' AS DateTime))
INSERT [dbo].[Transactions] ([TransactionID], [ProductID], [TransactionType], [Quantity], [TransactionDate]) VALUES (15, 1, N'Export    ', 24, CAST(N'2024-12-25T22:18:22.390' AS DateTime))
INSERT [dbo].[Transactions] ([TransactionID], [ProductID], [TransactionType], [Quantity], [TransactionDate]) VALUES (16, 1, N'Import    ', 24, CAST(N'2024-12-25T22:18:23.937' AS DateTime))
INSERT [dbo].[Transactions] ([TransactionID], [ProductID], [TransactionType], [Quantity], [TransactionDate]) VALUES (18, 21, N'Import    ', 3, CAST(N'2024-12-30T22:04:34.030' AS DateTime))
INSERT [dbo].[Transactions] ([TransactionID], [ProductID], [TransactionType], [Quantity], [TransactionDate]) VALUES (19, 21, N'Import    ', 3, CAST(N'2024-12-30T22:04:46.190' AS DateTime))
INSERT [dbo].[Transactions] ([TransactionID], [ProductID], [TransactionType], [Quantity], [TransactionDate]) VALUES (20, 21, N'Import    ', 20, CAST(N'2024-12-31T00:24:22.457' AS DateTime))
INSERT [dbo].[Transactions] ([TransactionID], [ProductID], [TransactionType], [Quantity], [TransactionDate]) VALUES (21, 4, N'Export    ', 12, CAST(N'2025-01-02T23:16:08.870' AS DateTime))
INSERT [dbo].[Transactions] ([TransactionID], [ProductID], [TransactionType], [Quantity], [TransactionDate]) VALUES (22, 5, N'Export    ', 123, CAST(N'2025-01-02T23:17:06.097' AS DateTime))
INSERT [dbo].[Transactions] ([TransactionID], [ProductID], [TransactionType], [Quantity], [TransactionDate]) VALUES (23, 2, N'Export    ', 1, CAST(N'2025-01-03T21:17:09.670' AS DateTime))
INSERT [dbo].[Transactions] ([TransactionID], [ProductID], [TransactionType], [Quantity], [TransactionDate]) VALUES (24, 4, N'Export    ', 12, CAST(N'2025-01-03T21:22:46.367' AS DateTime))
INSERT [dbo].[Transactions] ([TransactionID], [ProductID], [TransactionType], [Quantity], [TransactionDate]) VALUES (25, 3, N'Export    ', 1, CAST(N'2025-01-03T21:25:37.593' AS DateTime))
INSERT [dbo].[Transactions] ([TransactionID], [ProductID], [TransactionType], [Quantity], [TransactionDate]) VALUES (26, 3, N'Export    ', 1, CAST(N'2025-01-03T21:25:44.413' AS DateTime))
INSERT [dbo].[Transactions] ([TransactionID], [ProductID], [TransactionType], [Quantity], [TransactionDate]) VALUES (27, 6, N'Export    ', 123, CAST(N'2025-01-03T21:26:06.270' AS DateTime))
INSERT [dbo].[Transactions] ([TransactionID], [ProductID], [TransactionType], [Quantity], [TransactionDate]) VALUES (28, 6, N'Export    ', 12, CAST(N'2025-01-03T21:30:35.583' AS DateTime))
INSERT [dbo].[Transactions] ([TransactionID], [ProductID], [TransactionType], [Quantity], [TransactionDate]) VALUES (29, 3, N'Export    ', 2, CAST(N'2025-01-03T21:42:39.603' AS DateTime))
INSERT [dbo].[Transactions] ([TransactionID], [ProductID], [TransactionType], [Quantity], [TransactionDate]) VALUES (30, 3, N'Export    ', 12, CAST(N'2025-01-03T21:44:55.497' AS DateTime))
INSERT [dbo].[Transactions] ([TransactionID], [ProductID], [TransactionType], [Quantity], [TransactionDate]) VALUES (31, 5, N'Export    ', 55, CAST(N'2025-01-03T21:45:55.637' AS DateTime))
INSERT [dbo].[Transactions] ([TransactionID], [ProductID], [TransactionType], [Quantity], [TransactionDate]) VALUES (32, 6, N'Export    ', 23, CAST(N'2025-01-03T21:53:42.293' AS DateTime))
INSERT [dbo].[Transactions] ([TransactionID], [ProductID], [TransactionType], [Quantity], [TransactionDate]) VALUES (33, 4, N'Export    ', 51, CAST(N'2025-01-03T22:18:34.097' AS DateTime))
INSERT [dbo].[Transactions] ([TransactionID], [ProductID], [TransactionType], [Quantity], [TransactionDate]) VALUES (34, 5, N'Export    ', 12, CAST(N'2025-01-03T22:22:17.937' AS DateTime))
INSERT [dbo].[Transactions] ([TransactionID], [ProductID], [TransactionType], [Quantity], [TransactionDate]) VALUES (35, 4, N'Export    ', 12, CAST(N'2025-01-03T22:25:18.083' AS DateTime))
SET IDENTITY_INSERT [dbo].[Transactions] OFF
GO
ALTER TABLE [dbo].[Transactions] ADD  DEFAULT (getdate()) FOR [TransactionDate]
GO
USE [master]
GO
ALTER DATABASE [user] SET  READ_WRITE 
GO
