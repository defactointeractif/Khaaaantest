/****** Object:  Table [dbo].[khaaaantest_Contests]    Script Date: 2012-11-21 11:30:11 PM ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[khaaaantest_Contests](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[ContestId] [nvarchar](10) NOT NULL, 
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[NumberOfWinners] [int] NOT NULL,
	[CreatedOnDate] [datetime] NOT NULL,
 CONSTRAINT [PK_khaaaantest_Contests] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[khaaaantest_Contests] ADD  CONSTRAINT [DF_khaaaantest_Contests_CreatedOnDate]  DEFAULT (getdate()) FOR [CreatedOnDate]

ALTER TABLE [dbo].[khaaaantest_Contests] ADD  CONSTRAINT [DF_khaaaantest_Contests_NumberOfWinners]  DEFAULT ((1)) FOR [NumberOfWinners]

/****** Object:  Table [dbo].[khaaaantest_Contestant]    Script Date: 2012-11-21 11:29:57 PM ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[khaaaantest_Contestant](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](150) NOT NULL,
	[ContestId] [nvarchar](10) NOT NULL,
	[WinnerNumber] [int] NOT NULL,
	[IPAdd] [nvarchar](50) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_khaaaantest_Contestant] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[khaaaantest_Contestant] ADD  CONSTRAINT [DF_khaaaantest_Contestant_WinnerNumber]  DEFAULT ((-1)) FOR [WinnerNumber]

ALTER TABLE [dbo].[khaaaantest_Contestant] ADD  CONSTRAINT [DF_khaaaantest_Contestant_IPAdd]  DEFAULT ('Manual entry') FOR [IPAdd]

ALTER TABLE [dbo].[khaaaantest_Contestant] ADD  CONSTRAINT [DF_khaaaantest_Contestant_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]