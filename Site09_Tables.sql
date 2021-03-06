USE [master]
GO
/****** Object:  Database [site09]    Script Date: 11/5/2019 1:57:42 PM ******/
CREATE DATABASE [site09]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'site09', FILENAME = N'C:\Program Files (x86)\Plesk\Databases\MSSQL\MSSQL13.MSSQLSERVER2016\MSSQL\DATA\site09.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'site09_log', FILENAME = N'C:\Program Files (x86)\Plesk\Databases\MSSQL\MSSQL13.MSSQLSERVER2016\MSSQL\DATA\site09_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [site09] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [site09].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [site09] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [site09] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [site09] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [site09] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [site09] SET ARITHABORT OFF 
GO
ALTER DATABASE [site09] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [site09] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [site09] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [site09] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [site09] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [site09] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [site09] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [site09] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [site09] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [site09] SET  ENABLE_BROKER 
GO
ALTER DATABASE [site09] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [site09] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [site09] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [site09] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [site09] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [site09] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [site09] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [site09] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [site09] SET  MULTI_USER 
GO
ALTER DATABASE [site09] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [site09] SET DB_CHAINING OFF 
GO
ALTER DATABASE [site09] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [site09] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [site09] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [site09] SET QUERY_STORE = OFF
GO
USE [site09]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
USE [site09]
GO
/****** Object:  User [site09]    Script Date: 11/5/2019 1:57:43 PM ******/
CREATE USER [site09] FOR LOGIN [site09] WITH DEFAULT_SCHEMA=[site09]
GO
ALTER ROLE [db_ddladmin] ADD MEMBER [site09]
GO
ALTER ROLE [db_backupoperator] ADD MEMBER [site09]
GO
ALTER ROLE [db_datareader] ADD MEMBER [site09]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [site09]
GO
/****** Object:  Schema [site09]    Script Date: 11/5/2019 1:57:43 PM ******/
CREATE SCHEMA [site09]
GO
/****** Object:  Table [site09].[Betya_FriendsTB]    Script Date: 11/5/2019 1:57:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [site09].[Betya_FriendsTB](
	[User_ID] [int] NOT NULL,
	[Friend_ID] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [site09].[Betya_Polls_VotesTB]    Script Date: 11/5/2019 1:57:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [site09].[Betya_Polls_VotesTB](
	[Poll_ID] [int] NOT NULL,
	[Choice_1] [int] NOT NULL,
	[Choice_2] [int] NOT NULL,
	[Choice_3] [int] NOT NULL,
	[Choice_4] [int] NOT NULL,
	[Choice_5] [int] NOT NULL,
	[Choice_6] [int] NOT NULL,
 CONSTRAINT [PK_Betya_Polls_VotesTB] PRIMARY KEY CLUSTERED 
(
	[Poll_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [site09].[Betya_PollsTB]    Script Date: 11/5/2019 1:57:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [site09].[Betya_PollsTB](
	[Poll_ID] [int] IDENTITY(1,1) NOT NULL,
	[User_ID] [int] NOT NULL,
	[Poll_Title] [varchar](50) NOT NULL,
	[Choice_1] [varchar](50) NULL,
	[Choice_2] [varchar](50) NULL,
	[Choice_3] [varchar](50) NULL,
	[Choice_4] [varchar](50) NULL,
	[Choice_5] [varchar](50) NULL,
	[Choice_6] [varchar](50) NULL,
	[Poll_Date] [date] NULL,
	[Image_1] [varchar](max) NULL,
	[Image_2] [varchar](max) NULL,
	[Image_3] [varchar](max) NULL,
	[Image_4] [varchar](max) NULL,
	[Image_5] [varchar](max) NULL,
	[Image_6] [varchar](max) NULL,
	[IsPrivate] [bit] NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_Betya_PollsTB] PRIMARY KEY CLUSTERED 
(
	[Poll_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [site09].[Betya_UsersInvitedToPoll]    Script Date: 11/5/2019 1:57:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [site09].[Betya_UsersInvitedToPoll](
	[Poll_ID] [int] NOT NULL,
	[User_ID] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [site09].[Betya_UsersTB]    Script Date: 11/5/2019 1:57:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [site09].[Betya_UsersTB](
	[User_ID] [int] IDENTITY(1,1) NOT NULL,
	[User_Name] [nvarchar](50) NOT NULL,
	[User_Email] [varchar](50) NOT NULL,
	[User_Password] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Betya_UsersTB] PRIMARY KEY CLUSTERED 
(
	[User_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [site09].[Betya_UserVotedTB]    Script Date: 11/5/2019 1:57:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [site09].[Betya_UserVotedTB](
	[User_ID] [int] NOT NULL,
	[Poll_ID] [int] NOT NULL,
	[Voted] [bit] NOT NULL,
 CONSTRAINT [PK_Betya_UserVotedTB] PRIMARY KEY CLUSTERED 
(
	[User_ID] ASC,
	[Poll_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [site09].[FinalProject_Kasem_CitiesTB]    Script Date: 11/5/2019 1:57:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [site09].[FinalProject_Kasem_CitiesTB](
	[City_ID] [int] IDENTITY(1,1) NOT NULL,
	[City_Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_FinalProject_Kasem_CitiesTB] PRIMARY KEY CLUSTERED 
(
	[City_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [site09].[FinalProject_Kasem_FieldsTB]    Script Date: 11/5/2019 1:57:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [site09].[FinalProject_Kasem_FieldsTB](
	[Field_ID] [int] IDENTITY(1,1) NOT NULL,
	[Field_Name] [nvarchar](50) NOT NULL,
	[City_ID] [int] NOT NULL,
	[Map_coordinates] [varchar](50) NULL,
 CONSTRAINT [PK_FinalProject_Kasem_FieldsTB] PRIMARY KEY CLUSTERED 
(
	[Field_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [site09].[FinalProject_Kasem_FriendsTB]    Script Date: 11/5/2019 1:57:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [site09].[FinalProject_Kasem_FriendsTB](
	[User_ID] [int] NOT NULL,
	[Friend_ID] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [site09].[FinalProject_Kasem_GroupsTB]    Script Date: 11/5/2019 1:57:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [site09].[FinalProject_Kasem_GroupsTB](
	[Group_ID] [int] IDENTITY(1,1) NOT NULL,
	[Admin_ID] [int] NOT NULL,
	[Group_Name] [nvarchar](50) NOT NULL,
	[Max_Players] [int] NOT NULL,
	[Users_Joined] [int] NOT NULL,
	[Group_Picture] [varchar](50) NULL,
 CONSTRAINT [PK_FinalProject_Kasem_GroupsTB] PRIMARY KEY CLUSTERED 
(
	[Group_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [site09].[FinalProject_Kasem_MatchesTB]    Script Date: 11/5/2019 1:57:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [site09].[FinalProject_Kasem_MatchesTB](
	[Match_ID] [int] IDENTITY(1,1) NOT NULL,
	[Admin_ID] [int] NOT NULL,
	[Match_Name] [nvarchar](50) NOT NULL,
	[Match_Picture] [varchar](50) NULL,
	[Field_ID] [int] NOT NULL,
	[City_ID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Players_Joined] [int] NOT NULL,
	[Max_Players] [int] NOT NULL,
	[IsPrivate] [bit] NOT NULL,
	[Match_Key] [nvarchar](50) NULL,
	[Match_Date] [date] NOT NULL,
	[Match_Time] [time](7) NOT NULL,
 CONSTRAINT [PK_FinalProject_Kasem_MatchesTB] PRIMARY KEY CLUSTERED 
(
	[Match_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [site09].[FinalProject_Kasem_UsersTB]    Script Date: 11/5/2019 1:57:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [site09].[FinalProject_Kasem_UsersTB](
	[User_ID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Password] [varchar](50) NULL,
	[ProfilePIC] [varchar](max) NULL,
	[IsInMatch] [bit] NOT NULL,
	[Match_ID] [int] NOT NULL,
	[Facebook_ID] [varchar](max) NULL,
	[Google_ID] [varchar](max) NULL,
 CONSTRAINT [PK_FinalProject_Kasem_UsersTB] PRIMARY KEY CLUSTERED 
(
	[User_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [site09].[KatRegel_CitiesTB]    Script Date: 11/5/2019 1:57:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [site09].[KatRegel_CitiesTB](
	[City_ID] [int] IDENTITY(1,1) NOT NULL,
	[City_Name] [varchar](30) NOT NULL,
	[City_Location] [varchar](50) NULL,
 CONSTRAINT [PK_KatRegel_CitiesTB] PRIMARY KEY CLUSTERED 
(
	[City_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [site09].[KatRegel_FieldsTB]    Script Date: 11/5/2019 1:57:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [site09].[KatRegel_FieldsTB](
	[Field_ID] [int] IDENTITY(1,1) NOT NULL,
	[Field_Name] [varchar](30) NOT NULL,
	[Field_Picture] [varchar](500) NULL,
	[City_ID] [int] NOT NULL,
 CONSTRAINT [PK_KatRegel_FieldsTB] PRIMARY KEY CLUSTERED 
(
	[Field_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [site09].[KatRegel_MatchesTB]    Script Date: 11/5/2019 1:57:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [site09].[KatRegel_MatchesTB](
	[Match_ID] [int] IDENTITY(1,1) NOT NULL,
	[Match_Name] [varchar](15) NOT NULL,
	[Match_Start] [time](7) NOT NULL,
	[Match_Ends] [time](7) NOT NULL,
	[User_ID] [int] NOT NULL,
	[NumOfUsers] [int] NOT NULL,
	[Match_Date] [date] NOT NULL,
	[Field_ID] [int] NOT NULL,
	[IsPrivate] [bit] NULL,
	[Match_Lock] [varchar](10) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_KatRegel_MatchesTB] PRIMARY KEY CLUSTERED 
(
	[Match_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [site09].[KatRegel_UsersTB]    Script Date: 11/5/2019 1:57:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [site09].[KatRegel_UsersTB](
	[User_ID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](20) NOT NULL,
	[LastName] [varchar](20) NOT NULL,
	[Username] [varchar](20) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Password] [varchar](24) NOT NULL,
	[Profile_Picture] [image] NULL,
	[IsInGame] [bit] NULL,
	[Match_ID] [int] NULL,
 CONSTRAINT [PK_KatRegel_UsersTB] PRIMARY KEY CLUSTERED 
(
	[User_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [site09].[Python_AttendanceTB]    Script Date: 11/5/2019 1:57:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [site09].[Python_AttendanceTB](
	[Student_ID] [int] NOT NULL,
	[Student_Name] [nvarchar](50) NOT NULL,
	[Student_LastName] [nvarchar](50) NOT NULL,
	[Class_Name] [nvarchar](50) NOT NULL,
	[Attendance] [nvarchar](50) NOT NULL,
	[Date] [date] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [site09].[Python_MatchesTB]    Script Date: 11/5/2019 1:57:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [site09].[Python_MatchesTB](
	[Match_ID] [int] IDENTITY(1,1) NOT NULL,
	[Match_Name] [nvarchar](50) NOT NULL,
	[User_ID] [int] NOT NULL,
	[IsPrivate] [bit] NOT NULL,
	[Match_Key] [varchar](50) NULL,
	[Match_Date] [date] NOT NULL,
	[City] [varchar](50) NOT NULL,
	[Field] [varchar](50) NOT NULL,
	[Players] [int] NOT NULL,
	[Match_Time] [time](7) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Python_MatchesTB] PRIMARY KEY CLUSTERED 
(
	[Match_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [site09].[Python_StudentsTB]    Script Date: 11/5/2019 1:57:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [site09].[Python_StudentsTB](
	[Student_ID] [int] IDENTITY(1,1) NOT NULL,
	[Student_Name] [nvarchar](50) NOT NULL,
	[Student_LastName] [nvarchar](50) NOT NULL,
	[Class_Name] [nvarchar](50) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [site09].[Python_UsersTB]    Script Date: 11/5/2019 1:57:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [site09].[Python_UsersTB](
	[User_ID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](50) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[inMatch] [bit] NOT NULL,
	[Match_ID] [int] NOT NULL,
 CONSTRAINT [PK_Python_UsersTB] PRIMARY KEY CLUSTERED 
(
	[User_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [site09].[Betya_FriendsTB] ([User_ID], [Friend_ID]) VALUES (1005, 1)
INSERT [site09].[Betya_FriendsTB] ([User_ID], [Friend_ID]) VALUES (1005, 3)
INSERT [site09].[Betya_FriendsTB] ([User_ID], [Friend_ID]) VALUES (1005, 4)
INSERT [site09].[Betya_FriendsTB] ([User_ID], [Friend_ID]) VALUES (4, 1)
INSERT [site09].[Betya_FriendsTB] ([User_ID], [Friend_ID]) VALUES (4, 2)
INSERT [site09].[Betya_FriendsTB] ([User_ID], [Friend_ID]) VALUES (3, 1)
INSERT [site09].[Betya_FriendsTB] ([User_ID], [Friend_ID]) VALUES (1, 3)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (130, 2, 1, 1, 1, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (131, 3, 1, 1, 1, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (132, 2, 1, 1, 2, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (133, 2, 1, 1, 1, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (134, 2, 1, 1, 1, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (135, 2, 1, 1, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (136, 2, 1, 1, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (137, 2, 1, 1, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (138, 3, 1, 1, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (139, 2, 1, 1, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (140, 2, 1, 1, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (141, 2, 1, 1, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (142, 2, 0, 1, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (143, 2, 0, 1, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (144, 2, 0, 1, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (145, 2, 0, 1, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (146, 2, 0, 1, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (147, 2, 0, 1, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (148, 2, 0, 1, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (149, 2, 0, 1, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (150, 2, 0, 1, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (151, 2, 0, 1, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (152, 2, 0, 1, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (153, 2, 0, 1, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (154, 2, 0, 1, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (155, 2, 0, 1, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (156, 2, 0, 1, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (157, 2, 0, 1, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (158, 2, 0, 1, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (159, 2, 0, 1, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (160, 2, 0, 1, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (161, 2, 0, 1, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (162, 2, 0, 1, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (163, 2, 0, 1, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (164, 2, 0, 1, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (165, 2, 0, 1, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (166, 2, 0, 1, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (167, 2, 0, 0, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (168, 1, 0, 0, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (169, 1, 0, 0, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (170, 0, 0, 1, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (171, 0, 1, 0, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (172, 1, 0, 0, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (173, 1, 0, 0, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (174, 0, 0, 0, 0, 0, 1)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (175, 1, 0, 0, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (176, 0, 1, 0, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (177, 1, 0, 0, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (178, 0, 1, 0, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (182, 0, 1, 0, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (183, 0, 1, 0, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (184, 0, 1, 0, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (185, 0, 1, 0, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (186, 10, 7, 5, 20, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (187, 230, 150, 500, 40, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (188, 0, 0, 0, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (189, 0, 0, 0, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (190, 0, 0, 0, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (191, 0, 0, 0, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (192, 0, 0, 0, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (193, 0, 1, 0, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (195, 0, 0, 0, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (196, 0, 0, 0, 0, 0, 0)
INSERT [site09].[Betya_Polls_VotesTB] ([Poll_ID], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6]) VALUES (197, 1, 0, 0, 0, 0, 0)
SET IDENTITY_INSERT [site09].[Betya_PollsTB] ON 

INSERT [site09].[Betya_PollsTB] ([Poll_ID], [User_ID], [Poll_Title], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6], [Poll_Date], [Image_1], [Image_2], [Image_3], [Image_4], [Image_5], [Image_6], [IsPrivate], [IsActive]) VALUES (130, 1005, N'poll test 1 ? ', N'1', N'2', N'3', N'4', N'5', N'6', CAST(N'2019-08-27' AS Date), NULL, NULL, NULL, NULL, NULL, NULL, 0, 1)
INSERT [site09].[Betya_PollsTB] ([Poll_ID], [User_ID], [Poll_Title], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6], [Poll_Date], [Image_1], [Image_2], [Image_3], [Image_4], [Image_5], [Image_6], [IsPrivate], [IsActive]) VALUES (131, 1005, N'poll test 2 ?', N'1', N'2', N'3', N'4', N'5', NULL, CAST(N'2019-08-28' AS Date), NULL, NULL, NULL, NULL, NULL, NULL, 0, 1)
INSERT [site09].[Betya_PollsTB] ([Poll_ID], [User_ID], [Poll_Title], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6], [Poll_Date], [Image_1], [Image_2], [Image_3], [Image_4], [Image_5], [Image_6], [IsPrivate], [IsActive]) VALUES (132, 1005, N'poll test 3 ? ', N'1', N'2', N'3', N'4', N'5', NULL, CAST(N'2019-08-19' AS Date), NULL, NULL, NULL, NULL, NULL, NULL, 0, 1)
INSERT [site09].[Betya_PollsTB] ([Poll_ID], [User_ID], [Poll_Title], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6], [Poll_Date], [Image_1], [Image_2], [Image_3], [Image_4], [Image_5], [Image_6], [IsPrivate], [IsActive]) VALUES (133, 1005, N'poll test 4 ? ', N'1', N'2', N'3', N'4', NULL, NULL, CAST(N'2019-08-20' AS Date), NULL, NULL, NULL, NULL, NULL, NULL, 0, 1)
INSERT [site09].[Betya_PollsTB] ([Poll_ID], [User_ID], [Poll_Title], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6], [Poll_Date], [Image_1], [Image_2], [Image_3], [Image_4], [Image_5], [Image_6], [IsPrivate], [IsActive]) VALUES (134, 1005, N'poll test 5 ? ', N'1', N'2', N'3', N'4', N'5', NULL, CAST(N'2019-08-20' AS Date), NULL, NULL, NULL, NULL, NULL, NULL, 0, 1)
INSERT [site09].[Betya_PollsTB] ([Poll_ID], [User_ID], [Poll_Title], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6], [Poll_Date], [Image_1], [Image_2], [Image_3], [Image_4], [Image_5], [Image_6], [IsPrivate], [IsActive]) VALUES (135, 1005, N'poll test 6 ? ', N'1', N'2', N'3', N'4', N'5', N'6', CAST(N'2019-08-31' AS Date), NULL, NULL, NULL, NULL, NULL, NULL, 0, 1)
INSERT [site09].[Betya_PollsTB] ([Poll_ID], [User_ID], [Poll_Title], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6], [Poll_Date], [Image_1], [Image_2], [Image_3], [Image_4], [Image_5], [Image_6], [IsPrivate], [IsActive]) VALUES (136, 1005, N'poll test 7 ? ', N'1', N'2', N'3', NULL, NULL, NULL, CAST(N'2019-08-29' AS Date), NULL, NULL, NULL, NULL, NULL, NULL, 0, 1)
INSERT [site09].[Betya_PollsTB] ([Poll_ID], [User_ID], [Poll_Title], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6], [Poll_Date], [Image_1], [Image_2], [Image_3], [Image_4], [Image_5], [Image_6], [IsPrivate], [IsActive]) VALUES (137, 1005, N'poll test 8 ? ', N'Qwa1', N'2', N'3', NULL, NULL, NULL, CAST(N'2019-08-29' AS Date), NULL, NULL, NULL, NULL, NULL, NULL, 0, 1)
INSERT [site09].[Betya_PollsTB] ([Poll_ID], [User_ID], [Poll_Title], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6], [Poll_Date], [Image_1], [Image_2], [Image_3], [Image_4], [Image_5], [Image_6], [IsPrivate], [IsActive]) VALUES (138, 1005, N'poll test 9 ? ', N'Qwa1', N'2', N'3', N'1', N'2', NULL, CAST(N'2019-08-29' AS Date), NULL, NULL, NULL, NULL, NULL, NULL, 0, 1)
INSERT [site09].[Betya_PollsTB] ([Poll_ID], [User_ID], [Poll_Title], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6], [Poll_Date], [Image_1], [Image_2], [Image_3], [Image_4], [Image_5], [Image_6], [IsPrivate], [IsActive]) VALUES (139, 1005, N'poll test 10 ? ', N'1', N'2', N'3', NULL, NULL, NULL, CAST(N'2019-08-30' AS Date), NULL, NULL, NULL, NULL, NULL, NULL, 0, 1)
INSERT [site09].[Betya_PollsTB] ([Poll_ID], [User_ID], [Poll_Title], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6], [Poll_Date], [Image_1], [Image_2], [Image_3], [Image_4], [Image_5], [Image_6], [IsPrivate], [IsActive]) VALUES (186, 1005, N'poll test', N'sdf', N'sdfg', N'gsdfa', N'adssa', NULL, NULL, CAST(N'2019-08-10' AS Date), NULL, NULL, NULL, NULL, NULL, NULL, 0, 0)
INSERT [site09].[Betya_PollsTB] ([Poll_ID], [User_ID], [Poll_Title], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6], [Poll_Date], [Image_1], [Image_2], [Image_3], [Image_4], [Image_5], [Image_6], [IsPrivate], [IsActive]) VALUES (187, 1005, N'who will die next episode ?', N'aria', N'jon snow', N'sansa', N'danyres', NULL, NULL, CAST(N'2019-06-05' AS Date), NULL, NULL, NULL, NULL, NULL, NULL, 0, 0)
INSERT [site09].[Betya_PollsTB] ([Poll_ID], [User_ID], [Poll_Title], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6], [Poll_Date], [Image_1], [Image_2], [Image_3], [Image_4], [Image_5], [Image_6], [IsPrivate], [IsActive]) VALUES (188, 1005, N'gggg', N'Habaj', N'VVW', N'Usjna', NULL, NULL, NULL, CAST(N'2019-08-01' AS Date), NULL, NULL, NULL, NULL, NULL, NULL, 0, 0)
INSERT [site09].[Betya_PollsTB] ([Poll_ID], [User_ID], [Poll_Title], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6], [Poll_Date], [Image_1], [Image_2], [Image_3], [Image_4], [Image_5], [Image_6], [IsPrivate], [IsActive]) VALUES (189, 1005, N'polo', N'Ddee1', N'Cfr2', N'Vvf3', NULL, NULL, NULL, CAST(N'2019-08-01' AS Date), NULL, NULL, NULL, NULL, NULL, NULL, 0, 0)
INSERT [site09].[Betya_PollsTB] ([Poll_ID], [User_ID], [Poll_Title], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6], [Poll_Date], [Image_1], [Image_2], [Image_3], [Image_4], [Image_5], [Image_6], [IsPrivate], [IsActive]) VALUES (190, 1005, N'sdrf', N'Drrr', N'Gyyy', N'Ffg', NULL, NULL, NULL, CAST(N'2019-08-08' AS Date), NULL, NULL, NULL, NULL, NULL, NULL, 0, 0)
INSERT [site09].[Betya_PollsTB] ([Poll_ID], [User_ID], [Poll_Title], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6], [Poll_Date], [Image_1], [Image_2], [Image_3], [Image_4], [Image_5], [Image_6], [IsPrivate], [IsActive]) VALUES (191, 1005, N'sdfr', N'Fft', N'Gyy', N'Fg', NULL, NULL, NULL, CAST(N'2019-08-01' AS Date), NULL, NULL, NULL, NULL, NULL, NULL, 0, 0)
INSERT [site09].[Betya_PollsTB] ([Poll_ID], [User_ID], [Poll_Title], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6], [Poll_Date], [Image_1], [Image_2], [Image_3], [Image_4], [Image_5], [Image_6], [IsPrivate], [IsActive]) VALUES (192, 1005, N'dgg', N'Fgh', N'Yu', N'Hh', NULL, NULL, NULL, CAST(N'2019-08-30' AS Date), NULL, NULL, NULL, NULL, NULL, NULL, 0, 1)
INSERT [site09].[Betya_PollsTB] ([Poll_ID], [User_ID], [Poll_Title], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6], [Poll_Date], [Image_1], [Image_2], [Image_3], [Image_4], [Image_5], [Image_6], [IsPrivate], [IsActive]) VALUES (193, 1005, N'xcgg', N'Rgyu', N'Ghu', N'Uiu', NULL, NULL, NULL, CAST(N'2019-08-07' AS Date), NULL, NULL, NULL, NULL, NULL, NULL, 0, 0)
INSERT [site09].[Betya_PollsTB] ([Poll_ID], [User_ID], [Poll_Title], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6], [Poll_Date], [Image_1], [Image_2], [Image_3], [Image_4], [Image_5], [Image_6], [IsPrivate], [IsActive]) VALUES (194, 1005, N'Best Team ?', N'Barcelona ', N'Real M ', N'Man City', NULL, NULL, NULL, CAST(N'2019-08-05' AS Date), NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0)
INSERT [site09].[Betya_PollsTB] ([Poll_ID], [User_ID], [Poll_Title], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6], [Poll_Date], [Image_1], [Image_2], [Image_3], [Image_4], [Image_5], [Image_6], [IsPrivate], [IsActive]) VALUES (195, 1005, N'poll test abc ?', N'Abc1', N'Abc2', NULL, NULL, NULL, NULL, CAST(N'2019-08-28' AS Date), NULL, NULL, NULL, NULL, NULL, NULL, 0, 1)
INSERT [site09].[Betya_PollsTB] ([Poll_ID], [User_ID], [Poll_Title], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6], [Poll_Date], [Image_1], [Image_2], [Image_3], [Image_4], [Image_5], [Image_6], [IsPrivate], [IsActive]) VALUES (196, 1005, N'poll test Abc1 ?', N'Abc1', N'Abc2', NULL, NULL, NULL, NULL, CAST(N'2019-08-14' AS Date), NULL, NULL, NULL, NULL, NULL, NULL, 0, 1)
INSERT [site09].[Betya_PollsTB] ([Poll_ID], [User_ID], [Poll_Title], [Choice_1], [Choice_2], [Choice_3], [Choice_4], [Choice_5], [Choice_6], [Poll_Date], [Image_1], [Image_2], [Image_3], [Image_4], [Image_5], [Image_6], [IsPrivate], [IsActive]) VALUES (197, 1011, N'who is gonna win next game ?', N'Fc barcelona', N'Real madrid', NULL, NULL, NULL, NULL, CAST(N'2019-08-22' AS Date), NULL, NULL, NULL, NULL, NULL, NULL, 0, 1)
SET IDENTITY_INSERT [site09].[Betya_PollsTB] OFF
SET IDENTITY_INSERT [site09].[Betya_UsersTB] ON 

INSERT [site09].[Betya_UsersTB] ([User_ID], [User_Name], [User_Email], [User_Password]) VALUES (1, N'Muhammed', N'User1@hotmail.com', N'1234')
INSERT [site09].[Betya_UsersTB] ([User_ID], [User_Name], [User_Email], [User_Password]) VALUES (2, N'Muhammed', N'User1@hotmail.com', N'1234')
INSERT [site09].[Betya_UsersTB] ([User_ID], [User_Name], [User_Email], [User_Password]) VALUES (3, N'Kasem', N'User2@hotmail.com', N'1234')
INSERT [site09].[Betya_UsersTB] ([User_ID], [User_Name], [User_Email], [User_Password]) VALUES (4, N'Khaled', N'Khaled@hotmail.com', N'1233')
INSERT [site09].[Betya_UsersTB] ([User_ID], [User_Name], [User_Email], [User_Password]) VALUES (5, N'ADMIN', N'abohosenkasem@gmail.com', N'Kasem123')
INSERT [site09].[Betya_UsersTB] ([User_ID], [User_Name], [User_Email], [User_Password]) VALUES (6, N'fgdsfgsdfg', N'dfgsdgd', N'dsfgdfgdfg')
INSERT [site09].[Betya_UsersTB] ([User_ID], [User_Name], [User_Email], [User_Password]) VALUES (1005, N'User3000', N'1', N'1')
INSERT [site09].[Betya_UsersTB] ([User_ID], [User_Name], [User_Email], [User_Password]) VALUES (1006, N'UserNameTset', N'UserNameTest@gmail.com', N'Abcd12345')
INSERT [site09].[Betya_UsersTB] ([User_ID], [User_Name], [User_Email], [User_Password]) VALUES (1007, N'username123', N'username123@gmail.com', N'Abcd12345')
INSERT [site09].[Betya_UsersTB] ([User_ID], [User_Name], [User_Email], [User_Password]) VALUES (1008, N'username123', N'username123@gmail.com', N'Abcd12345')
INSERT [site09].[Betya_UsersTB] ([User_ID], [User_Name], [User_Email], [User_Password]) VALUES (1009, N'Muhammed123', N'Muhammed123jjj@gmail.com', N'Muhammed123')
INSERT [site09].[Betya_UsersTB] ([User_ID], [User_Name], [User_Email], [User_Password]) VALUES (1010, N'Polo012345', N'Polo012345@gmail.com', N'Polo012345')
INSERT [site09].[Betya_UsersTB] ([User_ID], [User_Name], [User_Email], [User_Password]) VALUES (1011, N'kasem3456', N'kasem@gmail.com', N'Kasem12345')
SET IDENTITY_INSERT [site09].[Betya_UsersTB] OFF
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (5, 130, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (5, 131, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (5, 132, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 130, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 131, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 132, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 133, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 134, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 135, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 136, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 137, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 138, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 139, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 141, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 149, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 151, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 153, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 155, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 157, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 159, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 161, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 163, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 164, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 165, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 166, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 167, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 168, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 169, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 170, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 171, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 172, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 173, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 174, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 175, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 176, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 177, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 178, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 182, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 183, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 184, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 185, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1005, 193, 1)
INSERT [site09].[Betya_UserVotedTB] ([User_ID], [Poll_ID], [Voted]) VALUES (1011, 197, 1)
SET IDENTITY_INSERT [site09].[FinalProject_Kasem_CitiesTB] ON 

INSERT [site09].[FinalProject_Kasem_CitiesTB] ([City_ID], [City_Name]) VALUES (1, N'kfar qara')
SET IDENTITY_INSERT [site09].[FinalProject_Kasem_CitiesTB] OFF
SET IDENTITY_INSERT [site09].[FinalProject_Kasem_FieldsTB] ON 

INSERT [site09].[FinalProject_Kasem_FieldsTB] ([Field_ID], [Field_Name], [City_ID], [Map_coordinates]) VALUES (1, N'hawarni', 1, NULL)
SET IDENTITY_INSERT [site09].[FinalProject_Kasem_FieldsTB] OFF
INSERT [site09].[FinalProject_Kasem_FriendsTB] ([User_ID], [Friend_ID]) VALUES (4, 1)
INSERT [site09].[FinalProject_Kasem_FriendsTB] ([User_ID], [Friend_ID]) VALUES (4, 2)
INSERT [site09].[FinalProject_Kasem_FriendsTB] ([User_ID], [Friend_ID]) VALUES (4, 3)
INSERT [site09].[FinalProject_Kasem_FriendsTB] ([User_ID], [Friend_ID]) VALUES (4, 5)
INSERT [site09].[FinalProject_Kasem_FriendsTB] ([User_ID], [Friend_ID]) VALUES (33, 34)
INSERT [site09].[FinalProject_Kasem_FriendsTB] ([User_ID], [Friend_ID]) VALUES (33, 1)
INSERT [site09].[FinalProject_Kasem_FriendsTB] ([User_ID], [Friend_ID]) VALUES (33, 2)
INSERT [site09].[FinalProject_Kasem_FriendsTB] ([User_ID], [Friend_ID]) VALUES (33, 3)
INSERT [site09].[FinalProject_Kasem_FriendsTB] ([User_ID], [Friend_ID]) VALUES (33, 3)
INSERT [site09].[FinalProject_Kasem_FriendsTB] ([User_ID], [Friend_ID]) VALUES (33, 3)
INSERT [site09].[FinalProject_Kasem_FriendsTB] ([User_ID], [Friend_ID]) VALUES (33, 9)
INSERT [site09].[FinalProject_Kasem_FriendsTB] ([User_ID], [Friend_ID]) VALUES (33, 15)
INSERT [site09].[FinalProject_Kasem_FriendsTB] ([User_ID], [Friend_ID]) VALUES (33, 21)
INSERT [site09].[FinalProject_Kasem_FriendsTB] ([User_ID], [Friend_ID]) VALUES (33, 21)
INSERT [site09].[FinalProject_Kasem_FriendsTB] ([User_ID], [Friend_ID]) VALUES (33, 30)
INSERT [site09].[FinalProject_Kasem_FriendsTB] ([User_ID], [Friend_ID]) VALUES (33, 10)
INSERT [site09].[FinalProject_Kasem_FriendsTB] ([User_ID], [Friend_ID]) VALUES (33, 16)
INSERT [site09].[FinalProject_Kasem_FriendsTB] ([User_ID], [Friend_ID]) VALUES (33, 22)
INSERT [site09].[FinalProject_Kasem_FriendsTB] ([User_ID], [Friend_ID]) VALUES (33, 11)
INSERT [site09].[FinalProject_Kasem_FriendsTB] ([User_ID], [Friend_ID]) VALUES (33, 17)
INSERT [site09].[FinalProject_Kasem_FriendsTB] ([User_ID], [Friend_ID]) VALUES (33, 23)
INSERT [site09].[FinalProject_Kasem_FriendsTB] ([User_ID], [Friend_ID]) VALUES (33, 24)
INSERT [site09].[FinalProject_Kasem_FriendsTB] ([User_ID], [Friend_ID]) VALUES (33, 12)
INSERT [site09].[FinalProject_Kasem_FriendsTB] ([User_ID], [Friend_ID]) VALUES (33, 25)
INSERT [site09].[FinalProject_Kasem_FriendsTB] ([User_ID], [Friend_ID]) VALUES (33, 13)
INSERT [site09].[FinalProject_Kasem_FriendsTB] ([User_ID], [Friend_ID]) VALUES (33, 26)
INSERT [site09].[FinalProject_Kasem_FriendsTB] ([User_ID], [Friend_ID]) VALUES (33, 14)
INSERT [site09].[FinalProject_Kasem_FriendsTB] ([User_ID], [Friend_ID]) VALUES (33, 27)
INSERT [site09].[FinalProject_Kasem_FriendsTB] ([User_ID], [Friend_ID]) VALUES (33, 28)
INSERT [site09].[FinalProject_Kasem_FriendsTB] ([User_ID], [Friend_ID]) VALUES (33, 29)
INSERT [site09].[FinalProject_Kasem_FriendsTB] ([User_ID], [Friend_ID]) VALUES (33, 6)
INSERT [site09].[FinalProject_Kasem_FriendsTB] ([User_ID], [Friend_ID]) VALUES (33, 31)
INSERT [site09].[FinalProject_Kasem_FriendsTB] ([User_ID], [Friend_ID]) VALUES (33, 7)
SET IDENTITY_INSERT [site09].[FinalProject_Kasem_MatchesTB] ON 

INSERT [site09].[FinalProject_Kasem_MatchesTB] ([Match_ID], [Admin_ID], [Match_Name], [Match_Picture], [Field_ID], [City_ID], [IsActive], [Players_Joined], [Max_Players], [IsPrivate], [Match_Key], [Match_Date], [Match_Time]) VALUES (4, 1, N'test', NULL, 1, 1, 1, 1, 6, 1, N'1234', CAST(N'2019-09-27' AS Date), CAST(N'14:00:00' AS Time))
INSERT [site09].[FinalProject_Kasem_MatchesTB] ([Match_ID], [Admin_ID], [Match_Name], [Match_Picture], [Field_ID], [City_ID], [IsActive], [Players_Joined], [Max_Players], [IsPrivate], [Match_Key], [Match_Date], [Match_Time]) VALUES (5, 3, N'test1', NULL, 1, 1, 1, 1, 6, 1, N'1234', CAST(N'2020-01-27' AS Date), CAST(N'14:00:00' AS Time))
SET IDENTITY_INSERT [site09].[FinalProject_Kasem_MatchesTB] OFF
SET IDENTITY_INSERT [site09].[FinalProject_Kasem_UsersTB] ON 

INSERT [site09].[FinalProject_Kasem_UsersTB] ([User_ID], [Username], [Email], [Password], [ProfilePIC], [IsInMatch], [Match_ID], [Facebook_ID], [Google_ID]) VALUES (1, N'kasem95', N'abohosenkasem@gmail.com', N'Kasem12345', N'Poppy1_10_23_2019_1-32-46_PM.jpg', 1, 4, NULL, NULL)
INSERT [site09].[FinalProject_Kasem_UsersTB] ([User_ID], [Username], [Email], [Password], [ProfilePIC], [IsInMatch], [Match_ID], [Facebook_ID], [Google_ID]) VALUES (2, N'kasemkasem', N'kasemkasem@gmail.com', N'Kasem12345', NULL, 0, -1, NULL, NULL)
INSERT [site09].[FinalProject_Kasem_UsersTB] ([User_ID], [Username], [Email], [Password], [ProfilePIC], [IsInMatch], [Match_ID], [Facebook_ID], [Google_ID]) VALUES (3, N'Test', N'Test@gmail.com', N'Test123456', NULL, 1, 5, NULL, NULL)
INSERT [site09].[FinalProject_Kasem_UsersTB] ([User_ID], [Username], [Email], [Password], [ProfilePIC], [IsInMatch], [Match_ID], [Facebook_ID], [Google_ID]) VALUES (6, N'TestTest', N'Kasem95@gmail.com', N'Kasem12345', NULL, 0, -1, NULL, NULL)
INSERT [site09].[FinalProject_Kasem_UsersTB] ([User_ID], [Username], [Email], [Password], [ProfilePIC], [IsInMatch], [Match_ID], [Facebook_ID], [Google_ID]) VALUES (7, N'Tttt', N'Tttt@gmail.com', N'Kasem12345', NULL, 0, -1, NULL, NULL)
INSERT [site09].[FinalProject_Kasem_UsersTB] ([User_ID], [Username], [Email], [Password], [ProfilePIC], [IsInMatch], [Match_ID], [Facebook_ID], [Google_ID]) VALUES (8, N'Ggg', N'Ggg@gmail.com', N'Kasem12345', NULL, 0, -1, NULL, NULL)
INSERT [site09].[FinalProject_Kasem_UsersTB] ([User_ID], [Username], [Email], [Password], [ProfilePIC], [IsInMatch], [Match_ID], [Facebook_ID], [Google_ID]) VALUES (9, N'Test1', N'Test1@gmail.com', N'Kasem12345', NULL, 0, -1, NULL, NULL)
INSERT [site09].[FinalProject_Kasem_UsersTB] ([User_ID], [Username], [Email], [Password], [ProfilePIC], [IsInMatch], [Match_ID], [Facebook_ID], [Google_ID]) VALUES (10, N'Test2', N'Test2@gmail.com', N'Kasem12345', NULL, 0, -1, NULL, NULL)
INSERT [site09].[FinalProject_Kasem_UsersTB] ([User_ID], [Username], [Email], [Password], [ProfilePIC], [IsInMatch], [Match_ID], [Facebook_ID], [Google_ID]) VALUES (11, N'Test3', N'Test3@gmail.com', N'Kasem12345', NULL, 0, -1, NULL, NULL)
INSERT [site09].[FinalProject_Kasem_UsersTB] ([User_ID], [Username], [Email], [Password], [ProfilePIC], [IsInMatch], [Match_ID], [Facebook_ID], [Google_ID]) VALUES (12, N'Test5', N'Test5@gmail.com', N'Kasem12345', NULL, 0, -1, NULL, NULL)
INSERT [site09].[FinalProject_Kasem_UsersTB] ([User_ID], [Username], [Email], [Password], [ProfilePIC], [IsInMatch], [Match_ID], [Facebook_ID], [Google_ID]) VALUES (13, N'Test6', N'Test6@gmail.com', N'Kasem12345', NULL, 0, -1, NULL, NULL)
INSERT [site09].[FinalProject_Kasem_UsersTB] ([User_ID], [Username], [Email], [Password], [ProfilePIC], [IsInMatch], [Match_ID], [Facebook_ID], [Google_ID]) VALUES (14, N'Test7', N'Test7@gmail.com', N'Kasem12345', NULL, 0, -1, NULL, NULL)
INSERT [site09].[FinalProject_Kasem_UsersTB] ([User_ID], [Username], [Email], [Password], [ProfilePIC], [IsInMatch], [Match_ID], [Facebook_ID], [Google_ID]) VALUES (15, N'Test10', N'Test10@gmail.com', N'Kasem1995', NULL, 0, -1, NULL, NULL)
INSERT [site09].[FinalProject_Kasem_UsersTB] ([User_ID], [Username], [Email], [Password], [ProfilePIC], [IsInMatch], [Match_ID], [Facebook_ID], [Google_ID]) VALUES (16, N'Test20', N'Test20@gmail.com', N'Kasem12345', NULL, 0, -1, NULL, NULL)
INSERT [site09].[FinalProject_Kasem_UsersTB] ([User_ID], [Username], [Email], [Password], [ProfilePIC], [IsInMatch], [Match_ID], [Facebook_ID], [Google_ID]) VALUES (17, N'Test30', N'Test30@gmail.com', N'Kasem12345', N'hhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhh', 0, -1, NULL, NULL)
INSERT [site09].[FinalProject_Kasem_UsersTB] ([User_ID], [Username], [Email], [Password], [ProfilePIC], [IsInMatch], [Match_ID], [Facebook_ID], [Google_ID]) VALUES (18, N'Kasem', N'Kasem@gmail.com', N'Kasem12345', NULL, 0, -1, NULL, NULL)
INSERT [site09].[FinalProject_Kasem_UsersTB] ([User_ID], [Username], [Email], [Password], [ProfilePIC], [IsInMatch], [Match_ID], [Facebook_ID], [Google_ID]) VALUES (19, N'Jasem', N'Jasem@gmail.com', N'Kasem12345', N'', 0, -1, NULL, NULL)
INSERT [site09].[FinalProject_Kasem_UsersTB] ([User_ID], [Username], [Email], [Password], [ProfilePIC], [IsInMatch], [Match_ID], [Facebook_ID], [Google_ID]) VALUES (20, N'Jasem95', N'Jasem95@gmail.com', N'Kasem12345', N'file:///data/user/0/host.exp.exponent/cache/ExperienceData/%2540kasem95%252FSoccer_Vision_Client/Camera/39a36b7c-e5d5-4239-8bdf-3afcb16da3c9.jpg', 0, -1, NULL, NULL)
INSERT [site09].[FinalProject_Kasem_UsersTB] ([User_ID], [Username], [Email], [Password], [ProfilePIC], [IsInMatch], [Match_ID], [Facebook_ID], [Google_ID]) VALUES (21, N'Test1000', N'Test1000@gmail.com', N'Kasem12345', NULL, 0, -1, NULL, NULL)
INSERT [site09].[FinalProject_Kasem_UsersTB] ([User_ID], [Username], [Email], [Password], [ProfilePIC], [IsInMatch], [Match_ID], [Facebook_ID], [Google_ID]) VALUES (22, N'Test2000', N'Test2000@gmail.com', N'Kasem12345', NULL, 0, -1, NULL, NULL)
INSERT [site09].[FinalProject_Kasem_UsersTB] ([User_ID], [Username], [Email], [Password], [ProfilePIC], [IsInMatch], [Match_ID], [Facebook_ID], [Google_ID]) VALUES (23, N'Test3000', N'Test3000@gmail.com', N'Kasem12345', NULL, 0, -1, NULL, NULL)
INSERT [site09].[FinalProject_Kasem_UsersTB] ([User_ID], [Username], [Email], [Password], [ProfilePIC], [IsInMatch], [Match_ID], [Facebook_ID], [Google_ID]) VALUES (24, N'Test4000', N'Test4000@gmail.com', N'Kasem12345', NULL, 0, -1, NULL, NULL)
INSERT [site09].[FinalProject_Kasem_UsersTB] ([User_ID], [Username], [Email], [Password], [ProfilePIC], [IsInMatch], [Match_ID], [Facebook_ID], [Google_ID]) VALUES (25, N'Test5000', N'Test5000@gmail.com', N'Kasem12345', NULL, 0, -1, NULL, NULL)
INSERT [site09].[FinalProject_Kasem_UsersTB] ([User_ID], [Username], [Email], [Password], [ProfilePIC], [IsInMatch], [Match_ID], [Facebook_ID], [Google_ID]) VALUES (26, N'Test6000', N'Test6000@gmail.com', N'Kasem12345', NULL, 0, -1, NULL, NULL)
INSERT [site09].[FinalProject_Kasem_UsersTB] ([User_ID], [Username], [Email], [Password], [ProfilePIC], [IsInMatch], [Match_ID], [Facebook_ID], [Google_ID]) VALUES (27, N'Test7000', N'Test7000@gmail.com', N'Kasem12345', NULL, 0, -1, NULL, NULL)
INSERT [site09].[FinalProject_Kasem_UsersTB] ([User_ID], [Username], [Email], [Password], [ProfilePIC], [IsInMatch], [Match_ID], [Facebook_ID], [Google_ID]) VALUES (28, N'Test8000', N'Test8000@gmail.com', N'Kasem12345', NULL, 0, -1, NULL, NULL)
INSERT [site09].[FinalProject_Kasem_UsersTB] ([User_ID], [Username], [Email], [Password], [ProfilePIC], [IsInMatch], [Match_ID], [Facebook_ID], [Google_ID]) VALUES (29, N'Test9000', N'Test9000@gmail.com', N'Kasem12345', N'Test9000_Picture_23_10_2019_16-02-15.jpg', 0, -1, NULL, NULL)
INSERT [site09].[FinalProject_Kasem_UsersTB] ([User_ID], [Username], [Email], [Password], [ProfilePIC], [IsInMatch], [Match_ID], [Facebook_ID], [Google_ID]) VALUES (30, N'Test10000', N'Test10000@gmail.com', N'Kasem12345', N'Test10000_Picture_23_10_2019_19-11-20.jpg', 0, -1, NULL, NULL)
INSERT [site09].[FinalProject_Kasem_UsersTB] ([User_ID], [Username], [Email], [Password], [ProfilePIC], [IsInMatch], [Match_ID], [Facebook_ID], [Google_ID]) VALUES (31, N'The ANGRY Man', N'Boring_life1995@hotmail.co.uk', N'Basel12345', N'The ANGRY Man_Picture_23_10_2019_20-40-11.jpg', 0, -1, NULL, NULL)
INSERT [site09].[FinalProject_Kasem_UsersTB] ([User_ID], [Username], [Email], [Password], [ProfilePIC], [IsInMatch], [Match_ID], [Facebook_ID], [Google_ID]) VALUES (33, N'Kasem Abo Hosen', N'kassem96@walla.com', NULL, N'https://platform-lookaside.fbsbx.com/platform/profilepic/?asid=1695379700595924&height=200&width=200&ext=1575405242&hash=AeTCv8gF4gIBjDoE', 0, -1, N'1695379700595924', NULL)
INSERT [site09].[FinalProject_Kasem_UsersTB] ([User_ID], [Username], [Email], [Password], [ProfilePIC], [IsInMatch], [Match_ID], [Facebook_ID], [Google_ID]) VALUES (34, N'kasem Abo hosen', N'abohosenkasem@gmail.com', NULL, N'https://lh3.googleusercontent.com/a-/AAuE7mAcs3nBbWW3fgtLAXuGI8NIij04mmPMNLIIUXMR4w', 0, -1, NULL, N'104795224796751395132')
SET IDENTITY_INSERT [site09].[FinalProject_Kasem_UsersTB] OFF
SET IDENTITY_INSERT [site09].[KatRegel_CitiesTB] ON 

INSERT [site09].[KatRegel_CitiesTB] ([City_ID], [City_Name], [City_Location]) VALUES (1, N'kfar qara', NULL)
INSERT [site09].[KatRegel_CitiesTB] ([City_ID], [City_Name], [City_Location]) VALUES (2, N'Haifa', NULL)
INSERT [site09].[KatRegel_CitiesTB] ([City_ID], [City_Name], [City_Location]) VALUES (3, N'Tel Aviv', NULL)
INSERT [site09].[KatRegel_CitiesTB] ([City_ID], [City_Name], [City_Location]) VALUES (4, N'Hadera', NULL)
INSERT [site09].[KatRegel_CitiesTB] ([City_ID], [City_Name], [City_Location]) VALUES (5, N'Netanya', NULL)
INSERT [site09].[KatRegel_CitiesTB] ([City_ID], [City_Name], [City_Location]) VALUES (6, N'Aum Alfahem', NULL)
INSERT [site09].[KatRegel_CitiesTB] ([City_ID], [City_Name], [City_Location]) VALUES (7, N'Arara', NULL)
INSERT [site09].[KatRegel_CitiesTB] ([City_ID], [City_Name], [City_Location]) VALUES (8, N'Musmus', NULL)
INSERT [site09].[KatRegel_CitiesTB] ([City_ID], [City_Name], [City_Location]) VALUES (9, N'Pardes Hanah', NULL)
SET IDENTITY_INSERT [site09].[KatRegel_CitiesTB] OFF
SET IDENTITY_INSERT [site09].[KatRegel_FieldsTB] ON 

INSERT [site09].[KatRegel_FieldsTB] ([Field_ID], [Field_Name], [Field_Picture], [City_ID]) VALUES (1, N'hawarni', NULL, 1)
INSERT [site09].[KatRegel_FieldsTB] ([Field_ID], [Field_Name], [Field_Picture], [City_ID]) VALUES (2, N'High School', NULL, 1)
INSERT [site09].[KatRegel_FieldsTB] ([Field_ID], [Field_Name], [Field_Picture], [City_ID]) VALUES (3, N'Grade School', NULL, 1)
INSERT [site09].[KatRegel_FieldsTB] ([Field_ID], [Field_Name], [Field_Picture], [City_ID]) VALUES (5, N'Stadium Hapoel', NULL, 1)
INSERT [site09].[KatRegel_FieldsTB] ([Field_ID], [Field_Name], [Field_Picture], [City_ID]) VALUES (6, N'Stadium Haifa', NULL, 2)
INSERT [site09].[KatRegel_FieldsTB] ([Field_ID], [Field_Name], [Field_Picture], [City_ID]) VALUES (7, N'Stadium Haifa 1', NULL, 2)
INSERT [site09].[KatRegel_FieldsTB] ([Field_ID], [Field_Name], [Field_Picture], [City_ID]) VALUES (9, N'Stadium Haifa 2', NULL, 2)
INSERT [site09].[KatRegel_FieldsTB] ([Field_ID], [Field_Name], [Field_Picture], [City_ID]) VALUES (10, N'Sammy Ofer', NULL, 2)
INSERT [site09].[KatRegel_FieldsTB] ([Field_ID], [Field_Name], [Field_Picture], [City_ID]) VALUES (11, N'Tel Aviv Stadium', NULL, 3)
INSERT [site09].[KatRegel_FieldsTB] ([Field_ID], [Field_Name], [Field_Picture], [City_ID]) VALUES (12, N'Bloom Field', NULL, 3)
INSERT [site09].[KatRegel_FieldsTB] ([Field_ID], [Field_Name], [Field_Picture], [City_ID]) VALUES (13, N'Menora Mivtachim', NULL, 3)
INSERT [site09].[KatRegel_FieldsTB] ([Field_ID], [Field_Name], [Field_Picture], [City_ID]) VALUES (14, N'Tel Aviv Stadium 1', NULL, 3)
SET IDENTITY_INSERT [site09].[KatRegel_FieldsTB] OFF
SET IDENTITY_INSERT [site09].[KatRegel_MatchesTB] ON 

INSERT [site09].[KatRegel_MatchesTB] ([Match_ID], [Match_Name], [Match_Start], [Match_Ends], [User_ID], [NumOfUsers], [Match_Date], [Field_ID], [IsPrivate], [Match_Lock], [IsActive]) VALUES (23, N'test2', CAST(N'14:00:00' AS Time), CAST(N'17:00:00' AS Time), 5, 1, CAST(N'2019-07-07' AS Date), 3, 0, NULL, 0)
INSERT [site09].[KatRegel_MatchesTB] ([Match_ID], [Match_Name], [Match_Start], [Match_Ends], [User_ID], [NumOfUsers], [Match_Date], [Field_ID], [IsPrivate], [Match_Lock], [IsActive]) VALUES (26, N'test5', CAST(N'14:00:00' AS Time), CAST(N'17:00:00' AS Time), 8, 2, CAST(N'2019-10-07' AS Date), 6, 0, NULL, 1)
INSERT [site09].[KatRegel_MatchesTB] ([Match_ID], [Match_Name], [Match_Start], [Match_Ends], [User_ID], [NumOfUsers], [Match_Date], [Field_ID], [IsPrivate], [Match_Lock], [IsActive]) VALUES (50, N'test1', CAST(N'12:00:00' AS Time), CAST(N'15:00:00' AS Time), 4, 3, CAST(N'2019-08-06' AS Date), 7, 0, NULL, 1)
INSERT [site09].[KatRegel_MatchesTB] ([Match_ID], [Match_Name], [Match_Start], [Match_Ends], [User_ID], [NumOfUsers], [Match_Date], [Field_ID], [IsPrivate], [Match_Lock], [IsActive]) VALUES (51, N'gggg', CAST(N'08:00:00' AS Time), CAST(N'11:00:00' AS Time), 9, 4, CAST(N'2019-07-20' AS Date), 2, 1, N'1234', 1)
INSERT [site09].[KatRegel_MatchesTB] ([Match_ID], [Match_Name], [Match_Start], [Match_Ends], [User_ID], [NumOfUsers], [Match_Date], [Field_ID], [IsPrivate], [Match_Lock], [IsActive]) VALUES (52, N'Muhammed12', CAST(N'12:00:00' AS Time), CAST(N'15:00:00' AS Time), 16, 1, CAST(N'2019-07-18' AS Date), 7, 0, NULL, 1)
SET IDENTITY_INSERT [site09].[KatRegel_MatchesTB] OFF
SET IDENTITY_INSERT [site09].[KatRegel_UsersTB] ON 

INSERT [site09].[KatRegel_UsersTB] ([User_ID], [FirstName], [LastName], [Username], [Email], [Password], [Profile_Picture], [IsInGame], [Match_ID]) VALUES (3, N'kasem', N'abohosen', N'ADMIN', N'abohosenkasem@gmail.com', N'Kasem123', NULL, 0, -1)
INSERT [site09].[KatRegel_UsersTB] ([User_ID], [FirstName], [LastName], [Username], [Email], [Password], [Profile_Picture], [IsInGame], [Match_ID]) VALUES (4, N'wassem', N'abohosen', N'wassem123', N'wasem@gmail.com', N'Wasem123', NULL, 1, 50)
INSERT [site09].[KatRegel_UsersTB] ([User_ID], [FirstName], [LastName], [Username], [Email], [Password], [Profile_Picture], [IsInGame], [Match_ID]) VALUES (5, N'hello', N'h', N'hello123', N'hello@gmail.com', N'Hello123', NULL, 0, -1)
INSERT [site09].[KatRegel_UsersTB] ([User_ID], [FirstName], [LastName], [Username], [Email], [Password], [Profile_Picture], [IsInGame], [Match_ID]) VALUES (6, N'gg', N'gg', N'gg123', N'gg@gmail.com', N'Gggg1234', NULL, 1, 50)
INSERT [site09].[KatRegel_UsersTB] ([User_ID], [FirstName], [LastName], [Username], [Email], [Password], [Profile_Picture], [IsInGame], [Match_ID]) VALUES (7, N'bassam', N'abohosien', N'bassam22', N'bassam22@walla.co.il', N'Bah200172', NULL, 1, 50)
INSERT [site09].[KatRegel_UsersTB] ([User_ID], [FirstName], [LastName], [Username], [Email], [Password], [Profile_Picture], [IsInGame], [Match_ID]) VALUES (8, N'Basel', N'Medleg', N'Basel Medleg', N'medlegbasel@gmail.com', N'123321ASd', NULL, 1, 26)
INSERT [site09].[KatRegel_UsersTB] ([User_ID], [FirstName], [LastName], [Username], [Email], [Password], [Profile_Picture], [IsInGame], [Match_ID]) VALUES (9, N'B', N'M', N'Vader hekleg', N'Basel@hotmail.com', N'Fucku123', NULL, 1, 51)
INSERT [site09].[KatRegel_UsersTB] ([User_ID], [FirstName], [LastName], [Username], [Email], [Password], [Profile_Picture], [IsInGame], [Match_ID]) VALUES (10, N'mahdi', N'atamna', N'mahdi.atamna', N'abcd@gmail.com', N'Chelsea12', NULL, 1, 51)
INSERT [site09].[KatRegel_UsersTB] ([User_ID], [FirstName], [LastName], [Username], [Email], [Password], [Profile_Picture], [IsInGame], [Match_ID]) VALUES (11, N'ttt', N'ttt', N'jh123', N'jh123@gmail.com', N'Jh123456', NULL, 1, 26)
INSERT [site09].[KatRegel_UsersTB] ([User_ID], [FirstName], [LastName], [Username], [Email], [Password], [Profile_Picture], [IsInGame], [Match_ID]) VALUES (12, N'Arees', N'Abu hussein', N'Arees', N'areesabuh99@gmail.com', N'Arees1941999', NULL, 1, 51)
INSERT [site09].[KatRegel_UsersTB] ([User_ID], [FirstName], [LastName], [Username], [Email], [Password], [Profile_Picture], [IsInGame], [Match_ID]) VALUES (14, N'Ggg', N'Ggg', N'Ggg123', N'ggg@gmail.com', N'Ggg12345', NULL, 1, 51)
INSERT [site09].[KatRegel_UsersTB] ([User_ID], [FirstName], [LastName], [Username], [Email], [Password], [Profile_Picture], [IsInGame], [Match_ID]) VALUES (15, N'Belal', N'Abo helal', N'Belal318', N'belal.a75@gmail.com', N'Belal123', NULL, 0, -1)
INSERT [site09].[KatRegel_UsersTB] ([User_ID], [FirstName], [LastName], [Username], [Email], [Password], [Profile_Picture], [IsInGame], [Match_ID]) VALUES (16, N'Muhammed', N'jbareen', N'muhammed12', N'muhammed@gmail.com', N'Mooo123456', NULL, 1, 52)
SET IDENTITY_INSERT [site09].[KatRegel_UsersTB] OFF
SET IDENTITY_INSERT [site09].[Python_MatchesTB] ON 

INSERT [site09].[Python_MatchesTB] ([Match_ID], [Match_Name], [User_ID], [IsPrivate], [Match_Key], [Match_Date], [City], [Field], [Players], [Match_Time], [IsActive]) VALUES (6, N'gadfa', 2, 1, N'1234', CAST(N'2019-09-22' AS Date), N'kfar qara', N'hawarni', 2, CAST(N'18:25:00' AS Time), 1)
INSERT [site09].[Python_MatchesTB] ([Match_ID], [Match_Name], [User_ID], [IsPrivate], [Match_Key], [Match_Date], [City], [Field], [Players], [Match_Time], [IsActive]) VALUES (8, N'abcd', 4, 1, N'1234', CAST(N'2019-10-18' AS Date), N'netanya', N'netanya stadium', 1, CAST(N'18:00:00' AS Time), 1)
INSERT [site09].[Python_MatchesTB] ([Match_ID], [Match_Name], [User_ID], [IsPrivate], [Match_Key], [Match_Date], [City], [Field], [Players], [Match_Time], [IsActive]) VALUES (9, N'fdgdfh', 5, 0, N'', CAST(N'2019-10-28' AS Date), N'netanya', N'netanya stadium', 1, CAST(N'18:00:00' AS Time), 1)
SET IDENTITY_INSERT [site09].[Python_MatchesTB] OFF
SET IDENTITY_INSERT [site09].[Python_StudentsTB] ON 

INSERT [site09].[Python_StudentsTB] ([Student_ID], [Student_Name], [Student_LastName], [Class_Name]) VALUES (1, N'Muhammed', N'Jbareen', N'Class_A')
INSERT [site09].[Python_StudentsTB] ([Student_ID], [Student_Name], [Student_LastName], [Class_Name]) VALUES (2, N'Ben', N'Han', N'Class_A')
INSERT [site09].[Python_StudentsTB] ([Student_ID], [Student_Name], [Student_LastName], [Class_Name]) VALUES (3, N'Amy', N'Smith', N'Class_A')
INSERT [site09].[Python_StudentsTB] ([Student_ID], [Student_Name], [Student_LastName], [Class_Name]) VALUES (4, N'Khaled', N'Mhamid', N'Class_A')
INSERT [site09].[Python_StudentsTB] ([Student_ID], [Student_Name], [Student_LastName], [Class_Name]) VALUES (5, N'Rammy', N'Yousef', N'Class_A')
INSERT [site09].[Python_StudentsTB] ([Student_ID], [Student_Name], [Student_LastName], [Class_Name]) VALUES (6, N'Sara', N'Malik', N'Class_A')
INSERT [site09].[Python_StudentsTB] ([Student_ID], [Student_Name], [Student_LastName], [Class_Name]) VALUES (7, N'Avi', N'Shalom', N'Class_A')
INSERT [site09].[Python_StudentsTB] ([Student_ID], [Student_Name], [Student_LastName], [Class_Name]) VALUES (8, N'Jimmy', N'Notron', N'Class_A')
INSERT [site09].[Python_StudentsTB] ([Student_ID], [Student_Name], [Student_LastName], [Class_Name]) VALUES (10, N'Emma', N'Stone', N'Class_A')
INSERT [site09].[Python_StudentsTB] ([Student_ID], [Student_Name], [Student_LastName], [Class_Name]) VALUES (11, N'Jess', N'James', N'Class_A')
SET IDENTITY_INSERT [site09].[Python_StudentsTB] OFF
SET IDENTITY_INSERT [site09].[Python_UsersTB] ON 

INSERT [site09].[Python_UsersTB] ([User_ID], [Username], [Email], [Password], [inMatch], [Match_ID]) VALUES (2, N'kasem95', N'abohosenkasem@gmail.com', N'Kasem123', 1, 6)
INSERT [site09].[Python_UsersTB] ([User_ID], [Username], [Email], [Password], [inMatch], [Match_ID]) VALUES (3, N'wasem', N'wasem@gmail.com', N'Wasem123', 1, 6)
INSERT [site09].[Python_UsersTB] ([User_ID], [Username], [Email], [Password], [inMatch], [Match_ID]) VALUES (4, N'example', N'example@gmail.com', N'Example123', 1, 8)
INSERT [site09].[Python_UsersTB] ([User_ID], [Username], [Email], [Password], [inMatch], [Match_ID]) VALUES (5, N'gggg', N'gggg@gmail.com', N'Ggggg123', 1, 9)
SET IDENTITY_INSERT [site09].[Python_UsersTB] OFF
ALTER TABLE [site09].[FinalProject_Kasem_FieldsTB]  WITH CHECK ADD FOREIGN KEY([City_ID])
REFERENCES [site09].[FinalProject_Kasem_CitiesTB] ([City_ID])
GO
ALTER TABLE [site09].[FinalProject_Kasem_GroupsTB]  WITH CHECK ADD  CONSTRAINT [FK__FinalProj__Admin__47C69FAC] FOREIGN KEY([Admin_ID])
REFERENCES [site09].[FinalProject_Kasem_UsersTB] ([User_ID])
GO
ALTER TABLE [site09].[FinalProject_Kasem_GroupsTB] CHECK CONSTRAINT [FK__FinalProj__Admin__47C69FAC]
GO
ALTER TABLE [site09].[FinalProject_Kasem_MatchesTB]  WITH CHECK ADD  CONSTRAINT [FK__FinalProj__Admin__46D27B73] FOREIGN KEY([Admin_ID])
REFERENCES [site09].[FinalProject_Kasem_UsersTB] ([User_ID])
GO
ALTER TABLE [site09].[FinalProject_Kasem_MatchesTB] CHECK CONSTRAINT [FK__FinalProj__Admin__46D27B73]
GO
ALTER TABLE [site09].[FinalProject_Kasem_MatchesTB]  WITH CHECK ADD  CONSTRAINT [FK__FinalProj__City___14B10FFA] FOREIGN KEY([City_ID])
REFERENCES [site09].[FinalProject_Kasem_CitiesTB] ([City_ID])
GO
ALTER TABLE [site09].[FinalProject_Kasem_MatchesTB] CHECK CONSTRAINT [FK__FinalProj__City___14B10FFA]
GO
ALTER TABLE [site09].[FinalProject_Kasem_MatchesTB]  WITH CHECK ADD  CONSTRAINT [FK__FinalProj__Field__13BCEBC1] FOREIGN KEY([Field_ID])
REFERENCES [site09].[FinalProject_Kasem_FieldsTB] ([Field_ID])
GO
ALTER TABLE [site09].[FinalProject_Kasem_MatchesTB] CHECK CONSTRAINT [FK__FinalProj__Field__13BCEBC1]
GO
ALTER TABLE [site09].[KatRegel_FieldsTB]  WITH CHECK ADD FOREIGN KEY([City_ID])
REFERENCES [site09].[KatRegel_CitiesTB] ([City_ID])
GO
/****** Object:  StoredProcedure [site09].[insertFB_Account]    Script Date: 11/5/2019 1:57:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [site09].[insertFB_Account] @Username nvarchar(50), @Email varchar(50), @IsInMatch bit, @Match_ID int, @User_ID bigint
AS
SET IDENTITY_INSERT [site09].[FinalProject_Kasem_UsersTB] ON
INSERT INTO [site09].[FinalProject_Kasem_UsersTB]
           ([Username]
           ,[Email]
           ,[IsInMatch]
           ,[Match_ID]
		   ,[User_ID])
     VALUES
           (@Username,@Email,@IsInMatch,@Match_ID,@User_ID)
SET IDENTITY_INSERT [site09].[FinalProject_Kasem_UsersTB] off
GO
USE [master]
GO
ALTER DATABASE [site09] SET  READ_WRITE 
GO
