USE [master]
GO
-- Create the 'bookMovie' database if it does not exist
IF NOT EXISTS (SELECT [name] FROM sys.databases WHERE [name] = 'bookMovie')
BEGIN
    CREATE DATABASE [bookMovie]
END
GO
-- Switch to the 'bookMovie' database context
USE [bookMovie]
GO
CREATE TABLE [dbo].[MovieCategory](
	[categoryId] [int] IDENTITY(1,1) NOT NULL,
	[categoryName] [nvarchar](max) NULL,
 CONSTRAINT [PK_MovieCategory] PRIMARY KEY CLUSTERED 
(
	[categoryId] ASC
)
) ON [PRIMARY] 
GO
/****** Object:  Table [dbo].[MovieImage]    Script Date: 6/18/2024 9:05:26 PM ******/
CREATE TABLE [dbo].[MovieImage](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[movieId] [nchar](10) NULL,
	[linkImage] [nvarchar](max) NULL,
 CONSTRAINT [PK_MovieImage] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)
) ON [PRIMARY]


GO
/****** Object:  Table [dbo].[Movies]    Script Date: 6/16/2024 9:17:45 PM ******/
GO
CREATE TABLE [dbo].[Movies](
	[movieId] [nchar](10) NOT NULL,
	[movieTitle] [nvarchar](50) NULL,
	[description] [nvarchar](max) NULL,
	[price] [float] NULL,
	[releaseDate] [datetime] NULL,
	[categoryId] [int] NULL,
 CONSTRAINT [PK_Movies] PRIMARY KEY CLUSTERED 
(
	[movieId] ASC
)
) ON [PRIMARY] 
GO
/****** Object:  Table [dbo].[Payments]    Script Date: 6/16/2024 9:17:45 PM ******/
GO
CREATE TABLE [dbo].[Payments](
	[paymentId] [int] IDENTITY(1,1) NOT NULL,
	[reservationId] [int] NULL,
	[totalAmount] [float] NULL,
 CONSTRAINT [PK_Payments] PRIMARY KEY CLUSTERED 
(
	[paymentId] ASC
)
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Revervations]    Script Date: 6/16/2024 9:17:45 PM ******/
GO
CREATE TABLE [dbo].[Revervations](
	[reservationId] [int] IDENTITY(1,1) NOT NULL,
	[userId] [int] NULL,
	[timeSlotId] [int] NULL,
	[seatId] [int] NULL,
	[reservationDate] [datetime] NULL,
	[statusId] [int] NULL,
 CONSTRAINT [PK_Revervations] PRIMARY KEY CLUSTERED 
(
	[reservationId] ASC
)
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 6/16/2024 9:17:45 PM ******/
GO
CREATE TABLE [dbo].[Roles](
	[roleId] [int] IDENTITY(1,1) NOT NULL,
	[roleName] [nvarchar](50) NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[roleId] ASC
)
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rooms]    Script Date: 6/16/2024 9:17:45 PM ******/
GO
CREATE TABLE [dbo].[Rooms](
	[roomId] [int] IDENTITY(1,1) NOT NULL,
	[theaterId] [int] NULL,
	[roomNumber] [nvarchar](50) NULL,
 CONSTRAINT [PK_Rooms] PRIMARY KEY CLUSTERED 
(
	[roomId] ASC
)
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Schedules]    Script Date: 6/16/2024 9:17:45 PM ******/
GO
CREATE TABLE [dbo].[Schedules](
	[schedulesId] [int] NOT NULL,
	[movieId] [nchar](10) NULL,
	[theaterId] [int] NULL,
	[timeSlotId] [int] NULL,
	[startDate] [datetime] NULL,
	[endDate] [datetime] NULL,
 CONSTRAINT [PK_Schedules] PRIMARY KEY CLUSTERED 
(
	[schedulesId] ASC
)
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Seats]    Script Date: 6/16/2024 9:17:45 PM ******/
GO
CREATE TABLE [dbo].[Seats](
	[seatId] [int] IDENTITY(1,1) NOT NULL,
	[roomId] [int] NULL,
	[seatNumber] [nchar](10) NULL,
 CONSTRAINT [PK_Seats] PRIMARY KEY CLUSTERED 
(
	[seatId] ASC
)
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Status]    Script Date: 6/16/2024 9:17:45 PM ******/
GO
CREATE TABLE [dbo].[Status](
	[statusId] [int] IDENTITY(1,1) NOT NULL,
	[statusName] [nvarchar](50) NULL,
 CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED 
(
	[statusId] ASC
)
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Theaters]    Script Date: 6/16/2024 9:17:45 PM ******/
GO
CREATE TABLE [dbo].[Theaters](
	[theaterId] [int] IDENTITY(1,1) NOT NULL,
	[theaterName] [nvarchar](max) NULL,
	[address] [nvarchar](max) NULL,
	[phoneNumber] [nvarchar](50) NULL,
 CONSTRAINT [PK_Theaters] PRIMARY KEY CLUSTERED 
(
	[theaterId] ASC
)
) ON [PRIMARY] 
GO
/****** Object:  Table [dbo].[TimeSlots]    Script Date: 6/16/2024 9:17:45 PM ******/
GO
CREATE TABLE [dbo].[TimeSlots](
	[timeSlotId] [int] IDENTITY(1,1) NOT NULL,
	[startTime] [datetime] NULL,
	[endTime] [datetime] NULL,
 CONSTRAINT [PK_TimeSlots] PRIMARY KEY CLUSTERED 
(
	[timeSlotId] ASC
)
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 6/16/2024 9:17:45 PM ******/
GO
CREATE TABLE [dbo].[Users](
	[userId] [int] IDENTITY(1,1) NOT NULL,
	[userName] [nvarchar](50) NULL,
	[email] [nvarchar](50) NULL,
	[password] [nvarchar](50) NULL,
	[phoneNumber] [nvarchar](50) NULL,
	[address] [nvarchar](max) NULL,
	[gender] [bit] NULL,
	[dob] [datetime] NULL,
	[roleId] [int] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[userId] ASC
)
) ON [PRIMARY] 
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([roleId], [roleName]) VALUES (1, N'Admin')
INSERT [dbo].[Roles] ([roleId], [roleName]) VALUES (2, N'Staff')
INSERT [dbo].[Roles] ([roleId], [roleName]) VALUES (3, N'Customer')
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([userId], [userName], [email], [password], [phoneNumber], [address], [gender], [dob], [roleId]) VALUES (1, N'hoan', N'hoan@gmail.com', NULL, NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[Users] ([userId], [userName], [email], [password], [phoneNumber], [address], [gender], [dob], [roleId]) VALUES (2, N'hoan2', N'hoan2@gmail.com', N'1234', NULL, NULL, NULL, NULL, 2)
INSERT [dbo].[Users] ([userId], [userName], [email], [password], [phoneNumber], [address], [gender], [dob], [roleId]) VALUES (3, N'danghoan', N'abc@gmail.com', N'123', N'123456789', NULL, NULL, NULL, 3)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
ALTER TABLE [dbo].[MovieImage]  WITH CHECK ADD  CONSTRAINT [FK_MovieImage_Movies] FOREIGN KEY([movieId])
REFERENCES [dbo].[Movies] ([movieId])
GO
ALTER TABLE [dbo].[MovieImage] CHECK CONSTRAINT [FK_MovieImage_Movies]
GO
ALTER TABLE [dbo].[Movies]  WITH CHECK ADD  CONSTRAINT [FK_Movies_MovieCategory] FOREIGN KEY([categoryId])
REFERENCES [dbo].[MovieCategory] ([categoryId])
GO
ALTER TABLE [dbo].[Movies] CHECK CONSTRAINT [FK_Movies_MovieCategory]
GO
ALTER TABLE [dbo].[Payments]  WITH CHECK ADD  CONSTRAINT [FK_Payments_Revervations1] FOREIGN KEY([reservationId])
REFERENCES [dbo].[Revervations] ([reservationId])
GO
ALTER TABLE [dbo].[Payments] CHECK CONSTRAINT [FK_Payments_Revervations1]
GO
ALTER TABLE [dbo].[Revervations]  WITH CHECK ADD  CONSTRAINT [FK_Revervations_Seats] FOREIGN KEY([seatId])
REFERENCES [dbo].[Seats] ([seatId])
GO
ALTER TABLE [dbo].[Revervations] CHECK CONSTRAINT [FK_Revervations_Seats]
GO
ALTER TABLE [dbo].[Revervations]  WITH CHECK ADD  CONSTRAINT [FK_Revervations_Status] FOREIGN KEY([statusId])
REFERENCES [dbo].[Status] ([statusId])
GO
ALTER TABLE [dbo].[Revervations] CHECK CONSTRAINT [FK_Revervations_Status]
GO
ALTER TABLE [dbo].[Revervations]  WITH CHECK ADD  CONSTRAINT [FK_Revervations_TimeSlots] FOREIGN KEY([timeSlotId])
REFERENCES [dbo].[TimeSlots] ([timeSlotId])
GO
ALTER TABLE [dbo].[Revervations] CHECK CONSTRAINT [FK_Revervations_TimeSlots]
GO
ALTER TABLE [dbo].[Revervations]  WITH CHECK ADD  CONSTRAINT [FK_Revervations_Users] FOREIGN KEY([userId])
REFERENCES [dbo].[Users] ([userId])
GO
ALTER TABLE [dbo].[Revervations] CHECK CONSTRAINT [FK_Revervations_Users]
GO
ALTER TABLE [dbo].[Rooms]  WITH CHECK ADD  CONSTRAINT [FK_Rooms_Theaters] FOREIGN KEY([theaterId])
REFERENCES [dbo].[Theaters] ([theaterId])
GO
ALTER TABLE [dbo].[Rooms] CHECK CONSTRAINT [FK_Rooms_Theaters]
GO
ALTER TABLE [dbo].[Schedules]  WITH CHECK ADD  CONSTRAINT [FK_Schedules_Movies] FOREIGN KEY([movieId])
REFERENCES [dbo].[Movies] ([movieId])
GO
ALTER TABLE [dbo].[Schedules] CHECK CONSTRAINT [FK_Schedules_Movies]
GO
ALTER TABLE [dbo].[Schedules]  WITH CHECK ADD  CONSTRAINT [FK_Schedules_Theaters] FOREIGN KEY([theaterId])
REFERENCES [dbo].[Theaters] ([theaterId])
GO
ALTER TABLE [dbo].[Schedules] CHECK CONSTRAINT [FK_Schedules_Theaters]
GO
ALTER TABLE [dbo].[Schedules]  WITH CHECK ADD  CONSTRAINT [FK_Schedules_TimeSlots] FOREIGN KEY([timeSlotId])
REFERENCES [dbo].[TimeSlots] ([timeSlotId])
GO
ALTER TABLE [dbo].[Schedules] CHECK CONSTRAINT [FK_Schedules_TimeSlots]
GO
ALTER TABLE [dbo].[Seats]  WITH CHECK ADD  CONSTRAINT [FK_Seats_Rooms] FOREIGN KEY([roomId])
REFERENCES [dbo].[Rooms] ([roomId])
GO
ALTER TABLE [dbo].[Seats] CHECK CONSTRAINT [FK_Seats_Rooms]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Roles] FOREIGN KEY([roleId])
REFERENCES [dbo].[Roles] ([roleId])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Roles]
GO
USE [master]
GO
ALTER DATABASE [bookMovie] SET  READ_WRITE 
GO
