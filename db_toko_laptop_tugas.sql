USE [master]
GO
/****** Object:  Database [db_toko_laptop_tugas]    Script Date: 13/02/2025 23:38:57 ******/
CREATE DATABASE [db_toko_laptop_tugas] ON  PRIMARY 
( NAME = N'db_toko_laptop_tugas', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\DATA\db_toko_laptop_tugas.mdf' , SIZE = 2048KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'db_toko_laptop_tugas_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\DATA\db_toko_laptop_tugas_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
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
USE [db_toko_laptop_tugas]
GO
/****** Object:  Table [dbo].[BillDetailTbl]    Script Date: 13/02/2025 23:38:57 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BillTbl]    Script Date: 13/02/2025 23:38:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillTbl](
	[BNum] [int] IDENTITY(1,1) NOT NULL,
	[BDate] [date] NOT NULL,
	[CustId] [int] NOT NULL,
	[CustName] [varchar](255) NOT NULL,
	[EmpName] [varchar](50) NOT NULL,
	[Amt] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[BNum] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomerTbl]    Script Date: 13/02/2025 23:38:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerTbl](
	[CustId] [int] IDENTITY(1,1) NOT NULL,
	[CustName] [varchar](50) NOT NULL,
	[CustAdd] [varchar](50) NOT NULL,
	[CustPhone] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CustId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmployeeTbl]    Script Date: 13/02/2025 23:38:57 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductTbl]    Script Date: 13/02/2025 23:38:57 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
USE [master]
GO
ALTER DATABASE [db_toko_laptop_tugas] SET  READ_WRITE 
GO
