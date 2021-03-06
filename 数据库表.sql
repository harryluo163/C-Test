USE [FSHouseSpiderData]
GO
/****** Object:  Table [dbo].[TempLicense]    Script Date: 2016/11/18 11:43:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TempLicense](
	[ID] [varchar](36) NULL,
	[KeyWord] [varchar](500) NULL,
	[PKeyWord] [varchar](500) NULL,
	[SpiderDate] [varchar](10) NULL,
	[UrlStr] [varchar](200) NULL,
	[UrlType] [varchar](50) NULL,
	[Para] [text] NULL,
	[License] [varchar](50) NULL,
	[CerDate] [varchar](10) NULL,
	[ProName] [varchar](500) NULL,
	[Address] [varchar](500) NULL,
	[BatchUrl] [varchar](200) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
