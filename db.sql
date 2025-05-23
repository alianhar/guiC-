USE [master]
GO
/****** Object:  Database [db_toko_laptop_tugas]    Script Date: 12/04/2025 07:38:31 ******/
CREATE DATABASE [db_toko_laptop_tugas]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'db_toko_laptop_tugas', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\db_toko_laptop_tugas.mdf' , SIZE = 9216KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'db_toko_laptop_tugas_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\db_toko_laptop_tugas_log.ldf' , SIZE = 1536KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [db_toko_laptop_tugas] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [db_toko_laptop_tugas].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [db_toko_laptop_tugas] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [db_toko_laptop_tugas] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [db_toko_laptop_tugas] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [db_toko_laptop_tugas] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [db_toko_laptop_tugas] SET ARITHABORT OFF 
GO
ALTER DATABASE [db_toko_laptop_tugas] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [db_toko_laptop_tugas] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [db_toko_laptop_tugas] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [db_toko_laptop_tugas] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [db_toko_laptop_tugas] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [db_toko_laptop_tugas] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [db_toko_laptop_tugas] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [db_toko_laptop_tugas] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [db_toko_laptop_tugas] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [db_toko_laptop_tugas] SET  DISABLE_BROKER 
GO
ALTER DATABASE [db_toko_laptop_tugas] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [db_toko_laptop_tugas] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [db_toko_laptop_tugas] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [db_toko_laptop_tugas] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [db_toko_laptop_tugas] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [db_toko_laptop_tugas] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [db_toko_laptop_tugas] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [db_toko_laptop_tugas] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [db_toko_laptop_tugas] SET  MULTI_USER 
GO
ALTER DATABASE [db_toko_laptop_tugas] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [db_toko_laptop_tugas] SET DB_CHAINING OFF 
GO
ALTER DATABASE [db_toko_laptop_tugas] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [db_toko_laptop_tugas] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [db_toko_laptop_tugas] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [db_toko_laptop_tugas] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [db_toko_laptop_tugas] SET QUERY_STORE = ON
GO
ALTER DATABASE [db_toko_laptop_tugas] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [db_toko_laptop_tugas]
GO
/****** Object:  Table [dbo].[BillDetailTbl]    Script Date: 12/04/2025 07:38:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillDetailTbl](
	[BillDetailId] [int] IDENTITY(1,1) NOT NULL,
	[BillId] [int] NULL,
	[ProductId] [int] NULL,
	[Quantity] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[BillDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BillTbl]    Script Date: 12/04/2025 07:38:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillTbl](
	[BNum] [int] IDENTITY(1,1) NOT NULL,
	[BDate] [date] NOT NULL,
	[CustId] [int] NOT NULL,
	[CustName] [varchar](255) NOT NULL,
	[EmpName] [varchar](50) NULL,
	[Amt] [int] NOT NULL,
	[PaymentStatus] [varchar](50) NOT NULL,
	[PaymentProof] [varchar](255) NULL,
	[PaymentDate] [datetime] NULL,
	[DeliveryStatus] [varchar](50) NOT NULL,
	[DeliveryAddress] [varchar](255) NULL,
	[DeliveryFee] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[BNum] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomerTbl]    Script Date: 12/04/2025 07:38:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerTbl](
	[CustId] [int] IDENTITY(1,1) NOT NULL,
	[CustName] [varchar](50) NOT NULL,
	[CustAdd] [varchar](50) NOT NULL,
	[CustPhone] [varchar](50) NOT NULL,
	[CustPass] [varchar](50) NOT NULL,
	[CustUsername] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CustId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DeliveryRateTbl]    Script Date: 12/04/2025 07:38:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeliveryRateTbl](
	[RateId] [int] IDENTITY(1,1) NOT NULL,
	[Zone] [varchar](100) NOT NULL,
	[BaseRate] [int] NOT NULL,
	[AdditionalPerKg] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DeliveryTrackingTbl]    Script Date: 12/04/2025 07:38:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeliveryTrackingTbl](
	[TrackingId] [int] IDENTITY(1,1) NOT NULL,
	[BillId] [int] NOT NULL,
	[Status] [varchar](50) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[UpdatedBy] [int] NOT NULL,
	[Notes] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[TrackingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmployeeTbl]    Script Date: 12/04/2025 07:38:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmployeeTbl](
	[EmpNum] [int] IDENTITY(1,1) NOT NULL,
	[EmpName] [varchar](50) NOT NULL,
	[EmpAdd] [varchar](255) NOT NULL,
	[EmpDOB] [date] NOT NULL,
	[EmpPhone] [varchar](50) NOT NULL,
	[EmpPass] [varchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[EmpNum] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductTbl]    Script Date: 12/04/2025 07:38:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductTbl](
	[PrId] [int] IDENTITY(1,1) NOT NULL,
	[PrName] [varchar](255) NOT NULL,
	[PrCat] [varchar](255) NOT NULL,
	[PrQty] [int] NOT NULL,
	[PrPrice] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PrId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BillTbl] ADD  DEFAULT ('Pending') FOR [PaymentStatus]
GO
ALTER TABLE [dbo].[BillTbl] ADD  DEFAULT ('Pending') FOR [DeliveryStatus]
GO
ALTER TABLE [dbo].[BillTbl] ADD  DEFAULT ((0)) FOR [DeliveryFee]
GO
ALTER TABLE [dbo].[DeliveryTrackingTbl]  WITH CHECK ADD FOREIGN KEY([BillId])
REFERENCES [dbo].[BillTbl] ([BNum])
GO
ALTER TABLE [dbo].[DeliveryTrackingTbl]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[EmployeeTbl] ([EmpNum])
GO
USE [master]
GO
ALTER DATABASE [db_toko_laptop_tugas] SET  READ_WRITE 
GO
