USE [QQZone]
GO
/****** Object:  Table [dbo].[A_Album]    Script Date: 2017/12/10 22:51:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[A_Album](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[AlbumName] [nvarchar](30) NULL,
	[Amount] [int] NOT NULL,
	[AlbumDescription] [nvarchar](50) NULL,
	[WhoCanSee] [nvarchar](30) NULL,
 CONSTRAINT [PK_A_Album] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[A_Comment]    Script Date: 2017/12/10 22:51:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[A_Comment](
	[id] [int] IDENTITY(2,2) NOT NULL,
	[CommentedId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[CommentContent] [nvarchar](50) NOT NULL,
	[Time] [nvarchar](20) NOT NULL,
	[Kind] [nvarchar](20) NOT NULL,
	[BelongFatherCommentID] [int] NULL,
	[WhoCanSee] [nvarchar](50) NULL,
 CONSTRAINT [PK_A_Comment] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[A_Diary]    Script Date: 2017/12/10 22:51:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[A_Diary](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[DiaryContent] [ntext] NOT NULL,
	[LikeAmount] [int] NOT NULL,
	[SeenAmount] [int] NOT NULL,
	[Time] [nvarchar](30) NOT NULL,
	[classid] [int] NULL,
	[WhoCanSee] [nvarchar](30) NULL,
	[idInAll] [int] NULL,
 CONSTRAINT [PK_A_Diary] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[A_Like]    Script Date: 2017/12/10 22:51:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[A_Like](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[LikedId] [int] NOT NULL,
	[LikeKind] [nvarchar](20) NOT NULL,
	[Time] [nvarchar](30) NOT NULL,
 CONSTRAINT [PK_A_Like] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[A_Message]    Script Date: 2017/12/10 22:51:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[A_Message](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[MessagerId] [int] NOT NULL,
	[MessageContent] [nvarchar](50) NOT NULL,
	[Time] [nvarchar](30) NOT NULL,
	[WhoCanSee] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_A_Message] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[a_move]    Script Date: 2017/12/10 22:51:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[a_move](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[doUserID] [int] NOT NULL,
	[doneUserID] [int] NULL,
	[ToSeeURL] [nvarchar](100) NOT NULL,
	[WhoCanSee] [nvarchar](100) NOT NULL,
	[Time] [timestamp] NOT NULL,
 CONSTRAINT [PK_a_move] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[A_Photo]    Script Date: 2017/12/10 22:51:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[A_Photo](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[picture] [nvarchar](100) NOT NULL,
	[albumID] [int] NOT NULL,
	[Time] [nvarchar](30) NOT NULL,
	[WhoCanSee] [nvarchar](30) NULL,
	[likeAmount] [int] NOT NULL,
 CONSTRAINT [PK_A_Photo] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[A_Status]    Script Date: 2017/12/10 22:51:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[A_Status](
	[id] [int] IDENTITY(1,2) NOT NULL,
	[UserId] [int] NOT NULL,
	[Time] [nvarchar](20) NULL,
	[StatusContent] [nvarchar](50) NULL,
	[picture] [nvarchar](100) NULL,
	[LikeAmount] [int] NULL,
	[SeenAmount] [int] NULL,
	[WhoCanSee] [nvarchar](50) NULL,
 CONSTRAINT [PK_MyNewStatus] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Classes]    Script Date: 2017/12/10 22:51:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Classes](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
	[ClassKind] [nvarchar](30) NOT NULL,
	[amount] [int] NOT NULL,
	[canDelete] [nchar](1) NULL,
 CONSTRAINT [PK_Classes] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FriendApplication]    Script Date: 2017/12/10 22:51:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FriendApplication](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[fromId] [int] NOT NULL,
	[toId] [int] NOT NULL,
	[note] [nvarchar](80) NULL,
 CONSTRAINT [PK_FriendApplication] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Friends]    Script Date: 2017/12/10 22:51:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Friends](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[FriendId] [int] NOT NULL,
 CONSTRAINT [PK_Friends] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 2017/12/10 22:51:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[NickName] [nvarchar](30) NOT NULL,
	[Number] [nvarchar](20) NOT NULL,
	[password] [nvarchar](50) NOT NULL,
	[LoginStatus] [nvarchar](10) NOT NULL,
	[name] [nvarchar](15) NULL,
	[sex] [nchar](2) NOT NULL,
	[age] [int] NULL,
	[headPicture] [nvarchar](100) NULL,
	[Email] [nvarchar](30) NOT NULL,
	[birthday] [nvarchar](30) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[whocantsee]    Script Date: 2017/12/10 22:51:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[whocantsee](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[visitorID] [int] NOT NULL,
	[visitedID] [int] NOT NULL,
	[combinedID] [int] NULL,
	[kind] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_whocantsee] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ZoneInfo]    Script Date: 2017/12/10 22:51:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ZoneInfo](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Title] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_ZoneInfo] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[A_Album]  WITH CHECK ADD  CONSTRAINT [FK_A_Album_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[A_Album] CHECK CONSTRAINT [FK_A_Album_Users]
GO
ALTER TABLE [dbo].[A_Comment]  WITH CHECK ADD  CONSTRAINT [FK_A_Comment_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[A_Comment] CHECK CONSTRAINT [FK_A_Comment_Users]
GO
ALTER TABLE [dbo].[A_Like]  WITH CHECK ADD  CONSTRAINT [FK_A_Like_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[A_Like] CHECK CONSTRAINT [FK_A_Like_Users]
GO
ALTER TABLE [dbo].[A_Message]  WITH CHECK ADD  CONSTRAINT [FK_A_Message_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[A_Message] CHECK CONSTRAINT [FK_A_Message_Users]
GO
ALTER TABLE [dbo].[A_Photo]  WITH CHECK ADD  CONSTRAINT [FK_A_Photo_Users] FOREIGN KEY([albumID])
REFERENCES [dbo].[A_Album] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[A_Photo] CHECK CONSTRAINT [FK_A_Photo_Users]
GO
ALTER TABLE [dbo].[A_Status]  WITH CHECK ADD  CONSTRAINT [FK_A_Status_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[A_Status] CHECK CONSTRAINT [FK_A_Status_Users]
GO
ALTER TABLE [dbo].[Classes]  WITH CHECK ADD  CONSTRAINT [FK_Classes_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Classes] CHECK CONSTRAINT [FK_Classes_Users]
GO
ALTER TABLE [dbo].[FriendApplication]  WITH CHECK ADD  CONSTRAINT [FK_FriendApplication_Users1] FOREIGN KEY([toId])
REFERENCES [dbo].[Users] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[FriendApplication] CHECK CONSTRAINT [FK_FriendApplication_Users1]
GO
ALTER TABLE [dbo].[Friends]  WITH CHECK ADD  CONSTRAINT [FK_Friends_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Friends] CHECK CONSTRAINT [FK_Friends_Users]
GO
